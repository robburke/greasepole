# Legend of the Greasepole - Godot 4 Migration Plan

## Overview

Port the Silverlight 4 game to Godot 4 with C#, preserving the existing `SilverLegendCore` architecture and implementing new Godot-native service adapters.

## Architecture Strategy

```
┌─────────────────────────────────────────────────────────┐
│                    GodotLegend                          │
│  (Godot 4 Project)                                      │
│                                                         │
│  ┌─────────────────────────────────────────────────┐   │
│  │              Main.tscn / Main.cs                 │   │
│  │         (Game host, calls ProcessAI/RenderFrame)│   │
│  └─────────────────────────────────────────────────┘   │
│                          │                              │
│                          ▼                              │
│  ┌─────────────────────────────────────────────────┐   │
│  │           GodotServiceFactory                    │   │
│  │    (Extends ServiceFactory, sets Instance)      │   │
│  └─────────────────────────────────────────────────┘   │
│           │         │         │         │               │
│           ▼         ▼         ▼         ▼               │
│  ┌──────────┐ ┌──────────┐ ┌──────────┐ ┌──────────┐   │
│  │ Godot    │ │ Godot    │ │ Godot    │ │ Godot    │   │
│  │ Rendering│ │ Sound    │ │ Input    │ │ Timer    │   │
│  │ Service  │ │ Service  │ │ Service  │ │ Service  │   │
│  └──────────┘ └──────────┘ └──────────┘ └──────────┘   │
│                                                         │
└─────────────────────────────────────────────────────────┘
                          │
                          ▼
┌─────────────────────────────────────────────────────────┐
│               SilverLegendCore.dll                      │
│  (Existing code, compiled as shared library)            │
│                                                         │
│  • GameLoop, GameConditions, Layers                     │
│  • TSprite, FrameDesc, SpriteSet                        │
│  • All AI code (aiFrosh, aiProjectile, etc.)           │
│  • Achievements, PolePosition                           │
│  • Interfaces (IRenderingService, ISoundService, etc.) │
└─────────────────────────────────────────────────────────┘
```

## Phase 1: Environment Setup

### 1.1 Install Godot 4 with .NET Support
- Download Godot 4.3+ (.NET version) from https://godotengine.org/download
- The .NET version includes C# support via .NET 6+
- Verify installation by creating a test project

### 1.2 Create Project Structure
```
Greasepole2025/
├── SilverLegend/              (existing - keep as reference)
├── GodotLegend/               (new Godot project)
│   ├── project.godot
│   ├── GodotLegend.csproj
│   ├── GodotLegend.sln
│   ├── Core/                  (ported SilverLegendCore)
│   ├── Services/              (Godot service implementations)
│   ├── Scenes/
│   │   └── Main.tscn
│   ├── Scripts/
│   │   └── Main.cs
│   └── Assets/
│       ├── Graphics/          (converted PNGs)
│       └── Sound/             (MP3s, copied)
```

## Phase 2: Port SilverLegendCore

### 2.1 Create Core Library
- Copy all .cs files from SilverLegendCore into GodotLegend/Core/
- Files to port:
  - Globals.cs
  - ServiceFactory.cs
  - All interfaces (IRenderingService, ISoundService, IInputService, IGameTimerService, IGameSettingsService)
  - Control/GameLoop.cs, GameConditions.cs, Layers.cs
  - Sprite/TSprite.cs, FrameDesc.cs, SpriteInit.cs, SpriteSet.cs
  - AI/*.cs (all AI files)
  - Achievements.cs
  - GameBitmapEnumeration.cs
  - All enums and supporting types

### 2.2 Modernize for .NET 6+
- Remove `#if SILVERLIGHT` conditionals (keep non-Silverlight branches)
- Update any deprecated APIs
- Fix namespace organization if needed
- Target should compile cleanly with no Silverlight dependencies

## Phase 3: Implement Godot Services

### 3.1 GodotServiceFactory
```csharp
public class GodotServiceFactory : ServiceFactory
{
    private Node _gameNode;

    public GodotServiceFactory(Node gameNode)
    {
        _gameNode = gameNode;
        Instance = this;
    }

    public override IRenderingService ProduceRenderingService()
        => new GodotRenderingService(_gameNode);
    public override ISoundService ProduceSoundService()
        => new GodotSoundService(_gameNode);
    public override IInputService ProduceInputService()
        => new GodotInputService();
    public override IGameTimerService ProduceGameTimerService()
        => new GodotTimerService();
    public override IGameSettingsService ProduceGameSettingsService()
        => new GodotSettingsService();
}
```

### 3.2 GodotTimerService (implements IGameTimerService)
- Use Godot's delta time from `_Process(double delta)`
- Track frame timing for `GetAdditionalUpdateCount()`
- Implement game time scoring with `Time.GetTicksMsec()`

### 3.3 GodotInputService (implements IInputService)
- Map `Input.GetMousePosition()` to GetMouseX/GetMouseY
- Map `Input.IsActionPressed()` to button methods
- Map keyboard to `GreasepoleKeys` enum

### 3.4 GodotRenderingService (implements IRenderingService)
- **Phase 3a (stub)**: Return true from LoadBitmapSet, no-op Draw methods
- **Phase 4 (full)**: Load textures, render sprites to CanvasLayer

### 3.5 GodotSoundService (implements ISoundService)
- **Phase 3a (stub)**: Return true from Load methods, no-op Play
- **Phase 5 (full)**: Use AudioStreamPlayer nodes

### 3.6 GodotSettingsService (implements IGameSettingsService)
- Use Godot's `ConfigFile` for persistent storage
- Store in `user://settings.cfg`

## Phase 4: Wire Up Game Loop

### 4.1 Main Scene (Main.tscn)
- Root node: Node2D or Control
- Add CanvasLayer for rendering
- Attach Main.cs script

### 4.2 Main.cs Script
```csharp
public partial class Main : Node2D
{
    public override void _Ready()
    {
        // Initialize services
        new GodotServiceFactory(this);

        // Initialize game
        Globals.myGameLoop.InitializeGame();
    }

    public override void _Process(double delta)
    {
        // Update timer service with delta
        ((GodotTimerService)Globals.GameTimerService).SetDelta(delta);

        // Run game logic
        Globals.myGameLoop.ProcessAI();

        // Render
        Globals.myGameLoop.RenderFrame();
    }
}
```

### 4.3 Milestone: Game Loop Running
- At this point, game logic should tick
- Debug output should show state transitions
- No visuals yet, but AI is processing

## Phase 5: Implement Full Rendering

### 5.1 Convert Assets
- Batch convert BMP → PNG (ImageMagick or similar)
- Maintain exact filenames for bitmap enumeration mapping
- Copy MP3s directly (Godot supports MP3)

### 5.2 GodotRenderingService (full implementation)
- Load textures on demand into Dictionary<string, Texture2D>
- Implement DrawBitmap using CanvasItem.DrawTexture or Sprite2D nodes
- Handle color replacement (SubstituteRGB) via shader or texture manipulation
- Implement text rendering

### 5.3 Sprite Rendering Strategy
Two options:

**Option A: Direct Canvas Drawing**
- Override `_Draw()` on main node
- Call `DrawTexture()` for each sprite
- Simple, matches original architecture

**Option B: Godot Sprite2D Nodes**
- Create Sprite2D node per TSprite
- Update positions each frame
- More "Godot native" but more refactoring

Recommend **Option A** for initial port (less code change), can optimize to B later.

## Phase 6: Implement Full Audio

### 6.1 GodotSoundService (full implementation)
- Create AudioStreamPlayer nodes for sound playback
- Pool players for simultaneous sounds
- Implement IStaticSound and IStreamedSound wrappers
- Handle volume/pan

### 6.2 Load Sound Banks
- Map existing sound enumerations to file paths
- Load on demand or preload based on original behavior

## Phase 7: Polish and Platform Export

### 7.1 Web Export
- Install Godot HTML5 export template
- Configure project for web
- Test in browser

### 7.2 Mobile Export (bonus)
- Configure Android export
- Configure iOS export
- Handle touch input (map to mouse)

## File-by-File Port Checklist

### Core Files (copy and update)
- [ ] Globals.cs
- [ ] ServiceFactory.cs
- [ ] IRenderingService.cs
- [ ] ISoundService.cs
- [ ] IInputService.cs
- [ ] IGameTimerService.cs
- [ ] IGameSettingsService.cs (if exists, or create)
- [ ] Control/GameLoop.cs
- [ ] Control/GameConditions.cs
- [ ] Control/Layers.cs
- [ ] Control/SoundbankInfo.cs
- [ ] Sprite/TSprite.cs
- [ ] Sprite/FrameDesc.cs
- [ ] Sprite/SpriteInit.cs
- [ ] Sprite/SpriteSet.cs
- [ ] AI/aiFrosh.cs
- [ ] AI/aiBackground.cs
- [ ] AI/aiCrowd.cs
- [ ] AI/aiProjectile.cs
- [ ] AI/aiWaterEffects.cs
- [ ] AI/aiMenuAndDisplay.cs
- [ ] AI/aiPopUp.cs
- [ ] AI/aiSupport.cs
- [ ] AI/aiMisc.cs
- [ ] AI/AIDefine.cs
- [ ] AI/aiEnums.cs
- [ ] AI/poleposn.cs
- [ ] Achievements.cs
- [ ] GameBitmapEnumeration.cs

### New Godot Files (create)
- [ ] GodotServiceFactory.cs
- [ ] GodotRenderingService.cs
- [ ] GodotSoundService.cs
- [ ] GodotInputService.cs
- [ ] GodotTimerService.cs
- [ ] GodotSettingsService.cs
- [ ] Main.cs
- [ ] Main.tscn

## Risk Assessment

| Risk | Mitigation |
|------|------------|
| Color replacement shader complexity | Start without it, add later |
| Sound timing differences | Test thoroughly, adjust Update() |
| Frame rate differences | Original targets 24 FPS, Godot is 60 - timer service handles this |
| Asset loading performance | Progressive loading already in GameLoop |

## Success Criteria

1. **Phase 4 Complete**: Game loop runs, state transitions work, debug output visible
2. **Phase 5 Complete**: Title screen renders, can navigate menus
3. **Phase 6 Complete**: Full game playable with sound
4. **Phase 7 Complete**: Exported to web and playable in browser

---

## Detailed Backlog for AI Assistants

This section provides detailed context for AI assistants who may continue this work. Each task includes context, file locations, and implementation guidance.

### Current Status (as of last session - January 2026)
- **Phases 1-6**: COMPLETE - Game runs, rendering works, sound works, input works
- **Frosh Color Replacement**: COMPLETE - Shader-based skin/shirt color variations
- **Aspect Ratio / Pillarboxing**: COMPLETE - Game maintains 4:3 ratio with pillarbox image
- **Phase 7**: NOT STARTED - Web export (this is the next priority)

#### Fixes Applied Previous Sessions:
1. **FPS corrected from 24 to 25** - GodotTimerService.cs TARGET_FRAME_TIME_MS updated per AIDefine.cs comment
2. **Sound loading fixed** - Changed LoadLongSounds(25) to LoadLongSounds(100) in GameLoop.cs so title music and narrator play
3. **Double-speed bug fixed** - Removed redundant GetAdditionalUpdateCount loop in Main.cs; ProcessAI handles its own timing
4. **Click detection fixed** - Added pending input state system in GodotInputService.cs so clicks aren't lost between frames
5. **Frosh color replacement implemented** - Created shader and RenderingServer-based system for skin/shirt color variations
6. **Aspect ratio pillarboxing** - WindowWrapper.tscn/cs maintains 4:3 ratio with Pillarbox.jpg filling extra space

---

### TASK 1: Test and Fix Sound Implementation
**Status**: COMPLETE ✓
**Priority**: High

**What was done**:
- Sound system works - title music plays, narrator works, sound effects work
- Fixed: LoadLongSounds now loads 100% (was 25%, missing title music at index 211+)
- 279 MP3 files in Assets/Sound/ folder

**Context**:
Sound system has been implemented and tested. The game has two types of sounds:
- Short sounds (ASSList enum) - 40 sound effects
- Long sounds (ASLList enum) - 239 music/voice clips

**Files involved**:
- `GodotLegend/Services/GodotSound.cs` - Contains GodotStaticSound and GodotStreamedSound classes
- `GodotLegend/Services/GodotSoundService.cs` - Service that loads and manages sounds
- `GodotLegend/Core/Control/SoundbankInfo.cs` - Contains ASSList and ASLList enums
- `GodotLegend/Assets/Sound/` - 279 MP3 files copied here

**How sounds work**:
1. Enum names map to filenames: `ssndEFFECTS_KABOOM` → `EFFECTS_KABOOM.mp3`
2. `AIMethods.sSound[]` array holds short sounds (IStaticSound)
3. `AIMethods.lSound[]` array holds long sounds (IStreamedSound)
4. Game code calls `AIMethods.sSound[(int)ASSList.ssndEFFECTS_KABOOM].Play()`

**Testing steps**:
1. Run the game: `cd GodotLegend && godot4 --path . Main.tscn`
2. Check console for `[GodotSoundService]` messages
3. Check for any "sound not found" errors
4. Play the game and verify sounds play

**Potential issues to watch for**:
- Volume conversion: Code converts 0-100 volume to Godot's dB scale (-80 to 0)
- Looping: Uses `finished` signal callback for manual looping
- MP3 import: May need to run `godot4 --headless --import` first

---

### TASK 2: Color Replacement Shader (SubstituteRGB)
**Status**: COMPLETE ✓
**Priority**: High
**Completed by**: Gemini + Antigravity

**Implementation Overview**:
The original Silverlight game used `SubstituteRGB` to dynamically recolor frosh sprites (skin tones, shirt colors). This was reimplemented using Godot's shader system and RenderingServer API.

**Files involved**:
- `GodotLegend/Shaders/color_replace.gdshader` - GLSL ES shader for RGB color replacement
- `GodotLegend/Services/GodotRenderingService.cs` - Rendering service with shader material caching

**Architecture**:
1. **Shader** (`color_replace.gdshader`): A simple canvas_item shader that takes `replace_color` and `substitute_color` uniforms. For each pixel, if it matches `replace_color` within a tolerance of 0.02, it's replaced with `substitute_color`.

2. **Material Caching**: `GodotRenderingService` maintains a `Dictionary<(r,g,b,r,g,b), ShaderMaterial>` to cache materials by color pair. This avoids creating new materials every frame.

3. **RenderingServer Batching**: The renderer uses Godot's low-level `RenderingServer` API with pooled `CanvasItem` RIDs. Sprites are batched by material - consecutive sprites with the same shader material are drawn to the same CanvasItem for performance.

4. **Draw Order**: Commands are processed in submission order. When the material changes (or switching between sprites and text), a new CanvasItem batch begins. `DrawIndex` ensures correct layering.

**How DrawBitmap works**:
```
1. Game calls DrawBitmap(sprite, frame, x, y, replaceRGB, substituteRGB)
2. If replaceRGB != substituteRGB and replaceRGB != black:
   - Get/create cached ShaderMaterial for this color pair
   - Mark command as NeedsColorReplace
3. During PerformDraw():
   - Batch consecutive sprites with same material into one CanvasItem
   - Apply material via RenderingServer.CanvasItemSetMaterial()
   - Draw textures via RenderingServer.CanvasItemAddTextureRect()
```

**Skin tone colors** (defined in GameLoop.cs:1117-1127):
- Default: RGB(248, 208, 152)
- Darker: RGB(134, 84, 39)
- Tan: RGB(222, 156, 89)
- Pale: RGB(255, 223, 191)

**Shirt colors**:
- Default purple: RGB(95, 0, 95)
- Variations for team differentiation

**Cross-platform**: Uses standard GLSL ES, works on Web, Android, iOS

---

### TASK 3: Web Export (Phase 7) - NEXT PRIORITY
**Status**: Not started
**Priority**: HIGH - This is the primary deployment target and should be done next

**Context**:
The main goal of this port is to run the game on the web. Godot 4 supports HTML5 export.

**Steps**:
1. Download Web export template from Godot
2. Configure export in Project → Export → Add → Web
3. Set export path (e.g., `export/web/`)
4. Handle any web-specific issues:
   - Audio autoplay restrictions (may need user click to start)
   - File loading (ensure assets are bundled)
   - SharedArrayBuffer requirements (needs specific headers)

**Web server requirements**:
For SharedArrayBuffer (needed for threading), server must send:
```
Cross-Origin-Opener-Policy: same-origin
Cross-Origin-Embedder-Policy: require-corp
```

**Testing**:
- Test locally with Godot's built-in server first
- Deploy to web server with correct headers
- Test in multiple browsers (Chrome, Firefox, Safari)

---

### TASK 4: Mobile Touch Input
**Status**: Not started
**Priority**: Low (bonus feature)

**Context**:
The game was originally designed for mouse input. Mobile would need touch mapped to mouse.

**Files involved**:
- `GodotLegend/Services/GodotInputService.cs` - Implements IInputService

**Implementation approach**:
1. Godot treats touch as mouse events by default (`Project Settings → Input Devices → Pointing → Emulate Mouse From Touch`)
2. May need to add on-screen buttons for keyboard commands
3. Consider viewport scaling for different screen sizes

---

### TASK 5: Text Rendering Improvements
**Status**: Basic implementation exists
**Priority**: Low

**Context**:
Text rendering uses Godot's fallback font. Could be improved with custom fonts.

**Current implementation** (in GodotRenderingService.cs):
```csharp
public void DrawText(TSprite associatedSprite, SpriteTextType textType, string text, bool boldWeight,
    int x, int y, byte r, byte g, byte b, byte a)
{
    // Uses ThemeDB.FallbackFont
    _textCommands.Add(new TextCommand { ... });
}
```

**Improvements**:
- Add custom font (original game had specific fonts)
- Handle bold weight properly
- Handle different SpriteTextType values differently

---

### TASK 6: Aspect Ratio Preservation (Pillarboxing)
**Status**: COMPLETE ✓
**Priority**: High
**Completed by**: Claude

**Problem**: When the window is resized to a non-4:3 aspect ratio, the game would stretch or display incorrectly.

**Solution**: Implemented a wrapper scene that maintains the 4:3 aspect ratio and fills extra space with a themed pillarbox image.

**Files involved**:
- `GodotLegend/Scenes/WindowWrapper.tscn` - Wrapper scene (set as main scene in project.godot)
- `GodotLegend/Scripts/WindowWrapper.cs` - Dynamic resizing script
- `GodotLegend/Assets/Pillarbox.jpg` - Background image (photo of actual Greasepole event)

**Architecture**:
```
WindowWrapper (Control, fills window)
├── Background (TextureRect, fills window with Pillarbox.jpg)
└── ViewportContainer (SubViewportContainer, dynamically sized)
    └── SubViewport (640x480 fixed internal resolution)
        └── Main (the actual game scene)
```

**How it works**:
1. `WindowWrapper` is the main scene (configured in project.godot)
2. On window resize, `WindowWrapper.cs` calculates the largest 4:3 rectangle that fits
3. The `ViewportContainer` is resized and centered to that rectangle
4. The `SubViewport` maintains the fixed 640x480 game resolution
5. `stretch = true` on ViewportContainer scales the game content to fit
6. The `Background` TextureRect fills any remaining space with the pillarbox image

**Key settings in WindowWrapper.tscn**:
- Background: `expand_mode = 2` (ignore size), `stretch_mode = 6` (keep aspect covered)
- ViewportContainer: `stretch = true`, `layout_mode = 0` (position controlled by script)
- SubViewport: `size = Vector2i(640, 480)`, `render_target_update_mode = 4` (always)

**Behavior**:
- Wide window (16:9): Pillarbox bars on left/right sides
- Tall window: Letterbox bars on top/bottom
- 4:3 window: Game fills entire window, no pillarbox visible

---

### Quick Reference: Key Files

| Purpose | File Path |
|---------|-----------|
| Window wrapper (main scene) | `GodotLegend/Scenes/WindowWrapper.tscn` + `WindowWrapper.cs` |
| Game entry point | `GodotLegend/Scenes/Main.tscn` + `Main.cs` |
| Service factory | `GodotLegend/Services/GodotServiceFactory.cs` |
| Rendering | `GodotLegend/Services/GodotRenderingService.cs` |
| Color replace shader | `GodotLegend/Shaders/color_replace.gdshader` |
| Sound | `GodotLegend/Services/GodotSoundService.cs`, `GodotSound.cs` |
| Input | `GodotLegend/Services/GodotInputService.cs` |
| Timer | `GodotLegend/Services/GodotTimerService.cs` |
| Settings | `GodotLegend/Services/GodotSettingsService.cs` |
| Game loop | `GodotLegend/Core/Control/GameLoop.cs` |
| Sprites | `GodotLegend/Core/Sprite/TSprite.cs` |
| Sound enums | `GodotLegend/Core/Control/SoundbankInfo.cs` |
| Bitmap registry | `GodotLegend/Core/Control/BitmapRegistry.cs` |
| Pillarbox image | `GodotLegend/Assets/Pillarbox.jpg` |

### Running the Project

**Godot Installation Location (on Rob's machine):**
```
C:\Users\rob\Downloads\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\
```

There are two executables:
- `Godot_v4.5.1-stable_mono_win64.exe` - Standard GUI version
- `Godot_v4.5.1-stable_mono_win64_console.exe` - Console version (shows debug output)

**Run the game (with console output):**
```bash
cd e:\projects\Greasepole2025\GodotLegend
"C:\Users\rob\Downloads\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64_console.exe" --path . Scenes/Main.tscn
```

**Open in Godot editor:**
```bash
cd e:\projects\Greasepole2025\GodotLegend
"C:\Users\rob\Downloads\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64.exe" --editor --path .
```

**Or double-click:** Open `project.godot` in the GodotLegend folder with Godot.

### Build Commands

```bash
cd e:\projects\Greasepole2025\GodotLegend

# Build C# project
dotnet build GodotLegend.csproj

# Import new assets (run after adding new images/sounds)
"C:\Users\rob\Downloads\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64_console.exe" --headless --import

# Run game
"C:\Users\rob\Downloads\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64_console.exe" --path . Scenes/Main.tscn
```

### Tip: Add Godot to PATH (optional)
To use shorter commands, add the Godot folder to your system PATH, or create an alias/batch file.

---

### Bug Fix Reference (for future debugging)

#### 1. Game Running Too Fast (Double Speed)
**Symptom**: Game AI runs at ~2x expected speed
**Root Cause**: `GetAdditionalUpdateCount()` was called in TWO places:
- Main.cs had a loop calling ProcessAI multiple times
- ProcessAI internally has its own nAIQueue loop (line 340-374)
**Fix**: Main.cs should call `ProcessAI()` once per Godot frame. ProcessAI handles its own timing.
**File**: `GodotLegend/Scripts/Main.cs`

#### 2. Clicks Sometimes Ignored
**Symptom**: Clicking buttons randomly doesn't register (hand goes down but action doesn't happen)
**Root Cause**: `_leftPressed` "just pressed" state only lasted one Godot frame. If AI wasn't checking for clicks during that exact frame, click was lost.
**Fix**: Added pending input state system. Clicks persist until an AI frame runs and has a chance to consume them. `OnAIFrameComplete()` called at end of ProcessAI to clear consumed clicks.
**Files**: `GodotLegend/Services/GodotInputService.cs`, `GodotLegend/Core/IInputService.cs`, `GodotLegend/Core/Control/GameLoop.cs`

#### 3. Title Music / Narrator Not Playing
**Symptom**: MUSIC_TITLEINIT.mp3 and NARRATOR_STARTDELAY*.mp3 don't play
**Root Cause**: `LoadLongSounds(25)` only loaded first 25% (59 sounds). Title music is at index ~211, narrator at ~217.
**Fix**: Changed to `LoadLongSounds(100)` to load all sounds.
**File**: `GodotLegend/Core/Control/GameLoop.cs` line 277

#### 4. FPS Slightly Off
**Symptom**: Game runs ~4% faster than original
**Root Cause**: Timer used 24 FPS, but original game was 25 FPS per AIDefine.cs comment
**Fix**: Changed `TARGET_FRAME_TIME_MS = 1000.0 / 25.0`
**File**: `GodotLegend/Services/GodotTimerService.cs`
