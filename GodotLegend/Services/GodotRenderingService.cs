using Godot;
using System.Collections.Generic;

/// <summary>
/// Godot rendering service - loads BMP textures and draws sprites.
/// </summary>
public class GodotRenderingService : IRenderingService
{
    private Node2D _gameNode;
    private bool[] _bitmapSetsLoaded = new bool[4];
    private Dictionary<string, Texture2D> _textures = new Dictionary<string, Texture2D>();
    // Unified command list to respect Z-order of submission
    private List<RenderCommand> _renderCommands = new List<RenderCommand>();
    private bool _isDrawing = false;
    private Font _defaultFont;

    // Color replacement shader
    private Shader _colorReplaceShader;
    // Cache of shader materials keyed by color pair (to avoid creating new materials every frame)
    private Dictionary<(byte, byte, byte, byte, byte, byte), ShaderMaterial> _colorReplaceMaterials =
        new Dictionary<(byte, byte, byte, byte, byte, byte), ShaderMaterial>();
    // Cache of RenderingServer canvas items for each material (for drawing with shaders)
    private Dictionary<ShaderMaterial, Rid> _shaderCanvasItems = new Dictionary<ShaderMaterial, Rid>();

    // Path to assets folder
    private const string ASSETS_PATH = "res://Assets/Graphics/";

    public GodotRenderingService(Node gameNode)
    {
        _gameNode = gameNode as Node2D;
    }

    public bool Initialize()
    {
        GD.Print("[GodotRenderingService] Initialize called");
        // Load default font - Godot's built-in font
        _defaultFont = ThemeDB.FallbackFont;

        // Load color replacement shader
        _colorReplaceShader = ResourceLoader.Load<Shader>("res://Shaders/color_replace.gdshader");
        if (_colorReplaceShader != null)
        {
            GD.Print("[GodotRenderingService] Color replace shader loaded");
        }
        else
        {
            GD.PrintErr("[GodotRenderingService] Failed to load color replace shader");
        }

        return true;
    }

    /// <summary>
    /// Gets or creates a cached ShaderMaterial for the given color replacement pair.
    /// Materials are cached to avoid creating new ones every frame.
    /// </summary>
    private ShaderMaterial GetColorReplaceMaterial(byte replaceR, byte replaceG, byte replaceB,
        byte substituteR, byte substituteG, byte substituteB)
    {
        var key = (replaceR, replaceG, replaceB, substituteR, substituteG, substituteB);

        if (_colorReplaceMaterials.TryGetValue(key, out var existing))
            return existing;

        // Create new material for this color combination
        var material = new ShaderMaterial();
        material.Shader = _colorReplaceShader;
        material.SetShaderParameter("replace_color",
            new Vector3(replaceR / 255f, replaceG / 255f, replaceB / 255f));
        material.SetShaderParameter("substitute_color",
            new Vector3(substituteR / 255f, substituteG / 255f, substituteB / 255f));

        _colorReplaceMaterials[key] = material;
        GD.Print($"[GodotRenderingService] Created color replace material: ({replaceR},{replaceG},{replaceB}) -> ({substituteR},{substituteG},{substituteB})");
        return material;
    }

    public bool LoadBitmapSet(BitmapSets bitmapSet, int percentageToLoad)
    {
        if (_bitmapSetsLoaded[(int)bitmapSet])
            return true;

        GD.Print($"[GodotRenderingService] LoadBitmapSet: {bitmapSet}, {percentageToLoad}%");

        int frameNumber;
        var bitmapInfoList = BitmapRegistry.GetBitmapLoadInfo(bitmapSet, out frameNumber);

        for (int i = 0; i < bitmapInfoList.Count; i++)
        {
            BitmapLoadInfo info = bitmapInfoList[i];
            int index = i + frameNumber;

            // Initialize the FrameDesc with bitmap info
            FrameDesc frm = AIMethods.frm[index];
            frm.InitFrame(
                info.BitmapName, info.HotspotX, info.HotspotY,
                info.SizeX1, info.SizeY1, info.SizeX2, info.SizeY2, false);
            frm.nBitmapWidthSilverlight = info.Width;
            frm.nBitmapHeightSilverlight = info.Height;

            // Initialize mirrored version
            // For mirrored frames, the hotspot X must be flipped: newHotspotX = width - originalHotspotX
            // This ensures sprites appear at the correct position when facing left vs right
            FrameDesc frmM = AIMethods.frmM[index];
            int mirroredHotspotX = info.Width - info.HotspotX;
            frmM.InitFrame(info.BitmapName, mirroredHotspotX, info.HotspotY,
                info.SizeX1, info.SizeY1, info.SizeX2, info.SizeY2, true, frm);
            frmM.nBitmapWidthSilverlight = info.Width;
            frmM.nBitmapHeightSilverlight = info.Height;

            // Preload the texture
            LoadTexture(info.BitmapName);
        }

        _bitmapSetsLoaded[(int)bitmapSet] = true;
        return true;
    }

    private Texture2D LoadTexture(string bitmapName)
    {
        if (_textures.ContainsKey(bitmapName))
            return _textures[bitmapName];

        // Convert bitmap name to file path - use PNG format
        // Names like "GAME_FROSH_FR1_1" -> "GAME_FROSH_FR1_1.png"
        string fileName = bitmapName + ".png";
        string fullPath = ASSETS_PATH + fileName;

        // Try to load the texture
        if (ResourceLoader.Exists(fullPath))
        {
            var texture = ResourceLoader.Load<Texture2D>(fullPath);
            if (texture != null)
            {
                _textures[bitmapName] = texture;
                return texture;
            }
        }

        // Return null if not found - we'll handle missing textures gracefully
        GD.PrintErr($"[GodotRenderingService] Texture not found: {fullPath}");
        return null;
    }

    public bool DisposeAll()
    {
        GD.Print("[GodotRenderingService] DisposeAll called");
        _textures.Clear();
        for (int i = 0; i < _bitmapSetsLoaded.Length; i++)
            _bitmapSetsLoaded[i] = false;
        return true;
    }

    public void DrawBitmap(TSprite associatedSprite, FrameDesc frameDesc, int nScrx, int nScry,
        byte[] replaceRGB, byte[] substituteRGB)
    {
        if (!_isDrawing || frameDesc == null || string.IsNullOrEmpty(frameDesc.szBitmap))
            return;

        // Check if color replacement is needed (non-zero colors that differ)
        bool needsColorReplace = false;
        if (replaceRGB != null && substituteRGB != null)
        {
            // Check if replace color is non-black and different from substitute
            bool hasReplaceColor = replaceRGB[0] != 0 || replaceRGB[1] != 0 || replaceRGB[2] != 0;
            bool colorsDiffer = replaceRGB[0] != substituteRGB[0] ||
                               replaceRGB[1] != substituteRGB[1] ||
                               replaceRGB[2] != substituteRGB[2];
            needsColorReplace = hasReplaceColor && colorsDiffer;
        }

        // Queue the draw command
        _renderCommands.Add(new RenderCommand
        {
            Type = RenderCommandType.Sprite,
            BitmapName = frameDesc.szBitmap,
            X = nScrx,
            Y = nScry,
            IsMirror = frameDesc.bIsMirror,
            Alpha = associatedSprite?.nA ?? 255,
            Z = associatedSprite?.nZ ?? 0,
            NeedsColorReplace = needsColorReplace,
            ReplaceR = replaceRGB?[0] ?? 0,
            ReplaceG = replaceRGB?[1] ?? 0,
            ReplaceB = replaceRGB?[2] ?? 0,
            SubstituteR = substituteRGB?[0] ?? 0,
            SubstituteG = substituteRGB?[1] ?? 0,
            SubstituteB = substituteRGB?[2] ?? 0
        });
    }

    public void DrawText(TSprite associatedSprite, SpriteTextType textType, string text, bool boldWeight,
        int x, int y, byte r, byte g, byte b, byte a)
    {
        if (!_isDrawing || string.IsNullOrEmpty(text))
            return;

        _renderCommands.Add(new RenderCommand
        {
            Type = RenderCommandType.Text,
            Text = text,
            X = x,
            Y = y,
            R = r,
            G = g,
            B = b,
            A = a, // For text, Alpha is mapped to A field
            Alpha = 255, // Unused for text but keep clean
            Bold = boldWeight
        });
    }

    public void Drawing()
    {
        // Called before drawing begins - clear previous commands
        _isDrawing = true;
        _renderCommands.Clear();
    }

    public void Drawn()
    {
        // Called after all DrawBitmap calls - trigger actual rendering
        _isDrawing = false;

        // Queue redraw on the game node
        _gameNode?.QueueRedraw();
    }

    public void RenderToScreen()
    {
        // This is called after Drawn() - actual rendering happens in _Draw
    }

    // Pool of canvas items to avoid creating/destroying every frame
    private List<Rid> _canvasItemPool = new List<Rid>();
    private int _poolIndex = 0;

    /// <summary>
    /// Called by Main.cs during _Draw to actually render the sprites and text
    /// </summary>
    public void PerformDraw(Node2D node)
    {
        // 1. Reset pool index
        _poolIndex = 0;
        
        // 2. Iterate through unified commands
        if (_renderCommands.Count == 0) return;

        Rid parentCanvasItem = node.GetCanvasItem();
        
        int cmdIndex = 0;
        while (cmdIndex < _renderCommands.Count)
        {
            var firstCmd = _renderCommands[cmdIndex];
            
            if (firstCmd.Type == RenderCommandType.Sprite)
            {
                // SPRITE BATCH START
                
                // Determine material for this batch
                ShaderMaterial batchMaterial = null;
                if (firstCmd.NeedsColorReplace)
                {
                    batchMaterial = GetColorReplaceMaterial(
                        firstCmd.ReplaceR, firstCmd.ReplaceG, firstCmd.ReplaceB,
                        firstCmd.SubstituteR, firstCmd.SubstituteG, firstCmd.SubstituteB);
                }

                // Get a canvas item for this batch
                Rid batchCanvasItem;
                if (_poolIndex < _canvasItemPool.Count)
                {
                    batchCanvasItem = _canvasItemPool[_poolIndex];
                    RenderingServer.CanvasItemClear(batchCanvasItem);
                }
                else
                {
                    batchCanvasItem = RenderingServer.CanvasItemCreate();
                    RenderingServer.CanvasItemSetParent(batchCanvasItem, parentCanvasItem);
                    _canvasItemPool.Add(batchCanvasItem);
                }
                _poolIndex++;

                // Configure the batch canvas item
                RenderingServer.CanvasItemSetMaterial(batchCanvasItem, batchMaterial != null ? batchMaterial.GetRid() : new Rid());
                // Ensure drawing follows creation order (Z-index simulation via DrawIndex)
                RenderingServer.CanvasItemSetDrawIndex(batchCanvasItem, _poolIndex); 

                // Add all consecutive sprite commands that share this material
                while (cmdIndex < _renderCommands.Count)
                {
                    var cmd = _renderCommands[cmdIndex];
                    
                    if (cmd.Type != RenderCommandType.Sprite)
                    {
                        // Found text, break batch
                        break;
                    }
                    
                    // Check material match
                    ShaderMaterial cmdMaterial = null;
                    if (cmd.NeedsColorReplace)
                    {
                        cmdMaterial = GetColorReplaceMaterial(
                            cmd.ReplaceR, cmd.ReplaceG, cmd.ReplaceB,
                            cmd.SubstituteR, cmd.SubstituteG, cmd.SubstituteB);
                    }

                    if (cmdMaterial != batchMaterial)
                    {
                        // Material changed, end this batch
                        break;
                    }

                    // Add to batch
                    var texture = LoadTexture(cmd.BitmapName);
                    if (texture != null)
                    {
                       var pos = new Vector2(cmd.X, cmd.Y);
                       var modulate = cmd.Alpha < 255 ? new Color(1, 1, 1, cmd.Alpha / 255f) : Colors.White;

                       if (cmd.IsMirror)
                       {
                            var rect = new Rect2(pos, new Vector2(-texture.GetWidth(), texture.GetHeight()));
                            RenderingServer.CanvasItemAddTextureRect(batchCanvasItem, rect, texture.GetRid(), false, modulate);
                       }
                       else
                       {
                            var rect = new Rect2(pos, new Vector2(texture.GetWidth(), texture.GetHeight()));
                            RenderingServer.CanvasItemAddTextureRect(batchCanvasItem, rect, texture.GetRid(), false, modulate);
                       }
                    }

                    cmdIndex++;
                }
            }
            else if (firstCmd.Type == RenderCommandType.Text)
            {
                // TEXT DRAW (Group consecutive text? Or one by one? Grouping is fine unless font/style changes drastically)
                // For now, group consecutive text into one CanvasItem
                
                Rid textCanvasItem;
                if (_poolIndex < _canvasItemPool.Count)
                {
                    textCanvasItem = _canvasItemPool[_poolIndex];
                    RenderingServer.CanvasItemClear(textCanvasItem);
                }
                else
                {
                    textCanvasItem = RenderingServer.CanvasItemCreate();
                    RenderingServer.CanvasItemSetParent(textCanvasItem, parentCanvasItem);
                    _canvasItemPool.Add(textCanvasItem);
                }
                _poolIndex++;
                
                RenderingServer.CanvasItemSetMaterial(textCanvasItem, new Rid()); // No shader
                RenderingServer.CanvasItemSetDrawIndex(textCanvasItem, _poolIndex); // Draws ON TOP of previous batches

                while (cmdIndex < _renderCommands.Count)
                {
                    var cmd = _renderCommands[cmdIndex];
                    if (cmd.Type != RenderCommandType.Text) break; // Break if we hit a sprite

                    var color = new Color(cmd.R / 255f, cmd.G / 255f, cmd.B / 255f, cmd.A / 255f);
                    var pos = new Vector2(cmd.X, cmd.Y + 14); 
                    int fontSize = cmd.Bold ? 14 : 13;
                    
                    _defaultFont.DrawString(textCanvasItem, pos, cmd.Text, HorizontalAlignment.Left, -1, fontSize, color);

                    cmdIndex++;
                }
            }
        }
        
        // Hide unused pool items
        for(int i = _poolIndex; i < _canvasItemPool.Count; i++)
        {
             RenderingServer.CanvasItemClear(_canvasItemPool[i]);
        }
    }

    public int GetFps()
    {
        return (int)Engine.GetFramesPerSecond();
    }

    private enum RenderCommandType
    {
        Sprite,
        Text
    }

    /// <summary>
    /// Unified render command structure to perform draws in order
    /// </summary>
    private struct RenderCommand
    {
        public RenderCommandType Type;
        
        // Common / Sprite
        public string BitmapName;
        public int X, Y, Z;
        public bool IsMirror;
        public byte Alpha;
        
        // Sprite: Color replacement
        public bool NeedsColorReplace;
        public byte ReplaceR, ReplaceG, ReplaceB;
        public byte SubstituteR, SubstituteG, SubstituteB;

        // Text
        public string Text;
        public byte R, G, B, A;
        public bool Bold;
    }
}
