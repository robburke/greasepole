# Layer Rendering Analysis Plan

## Problem Statement
The GDScript port (GodotLegendGD) is missing background layers during rendering:
- Sky layer not visible
- Background layers (skyline, trees, FRECs) not visible
- Only the pit layer with frosh, pizza, and apple appears to render

## Analysis Goals
1. Verify layer initialization matches C# exactly
2. Verify SpriteSet draw order and layer offsets
3. Verify parallax scrolling calculations
4. Trace the full render pipeline from GameLoop through to actual draw calls

---

## Architecture Overview

### C# Rendering Pipeline
```
GameLoop.Draw()
  → foreach SpriteSet in draw order:
      → SpriteSet.CalculateScreenCoordinates()  // uses Layer.GetY()
      → SpriteSet.Draw()
          → foreach TSprite:
              → TSprite.Draw()  // blits to screen
```

### GDScript Rendering Pipeline
```
GameLoop._draw() or render function
  → foreach SpriteSet in draw order:
      → SpriteSet.calculate_screen_coordinates()  // uses get_layer_y()
      → SpriteSet.draw()
          → foreach TSprite:
              → TSprite.draw()
```

---

## Files to Analyze

### Core Rendering Files
| File | Purpose |
|------|---------|
| `GodotLegend/Core/Control/GameLoop.cs` | C# main render loop |
| `GodotLegendGD/Core/Control/GameLoop.gd` | GDScript main render loop |
| `GodotLegend/Core/Control/Layers.cs` | C# layer management |
| `GodotLegendGD/Core/Control/Layers.gd` | GDScript layer management |
| `GodotLegend/Core/Sprite/SpriteSet.cs` | C# sprite collection + draw |
| `GodotLegendGD/Core/Sprite/SpriteSet.gd` | GDScript sprite collection + draw |
| `GodotLegend/Core/Sprite/TSprite.cs` | C# sprite draw implementation |
| `GodotLegendGD/Core/Sprite/TSprite.gd` | GDScript sprite draw implementation |

### Layer Configuration
| File | Purpose |
|------|---------|
| `GodotLegend/Core/Control/Layers.cs` | Layer initialization with Y offsets and parallax factors |
| `GodotLegendGD/Core/Control/Layers.gd` | Same for GDScript |

---

## Analysis Checklist

### 1. Layer Initialization
- [ ] Compare number of layers (should be 6: SKY, TREE, SKYLINE, FREC, PIT, MISC)
- [ ] Compare initial Y offsets for each layer
- [ ] Compare parallax factors for each layer
- [ ] Verify LayerNames enum values match

### 2. SpriteSet Assignment
- [ ] Verify each SpriteSet is assigned to correct layer index
- [ ] Check ss_clouds → LAYER_SKY
- [ ] Check ss_balloon → LAYER_SKY
- [ ] Check ss_skyline → LAYER_SKYLINE
- [ ] Check ss_trees → LAYER_TREE
- [ ] Check ss_frecs → LAYER_FREC
- [ ] Check ss_pit, ss_fr, ss_water, ss_tossed → LAYER_PIT
- [ ] Check ss_console, ss_icons, ss_mouse → LAYER_MISC

### 3. Draw Order
- [ ] Compare the order SpriteSets are drawn in GameLoop
- [ ] Background layers must draw BEFORE foreground layers
- [ ] Expected order: clouds → skyline → trees → frecs → water → pit → fr → tossed → console → icons → mouse

### 4. Screen Coordinate Calculation
- [ ] Compare Layer.GetY() in C# vs get_layer_y() in GDScript
- [ ] Verify parallax scroll distance affects layer offset correctly
- [ ] Check TSprite.CalculateScreenCoordinates() logic matches

### 5. Actual Draw Call
- [ ] Verify TSprite.draw() is being called
- [ ] Check if sprites have valid frames (frm_frame != null)
- [ ] Verify texture is being drawn at correct screen position
- [ ] Check for any visibility flags or conditions that might skip drawing

### 6. Sprite Population
- [ ] Verify background sprites are being CREATED (clouds, trees, etc.)
- [ ] Check SpriteInit.create_sprite() is called for background types
- [ ] Verify sprites are being INCLUDED in their SpriteSets
- [ ] Check that SpriteSet.n (sprite count) > 0 for background sets

---

## Key Comparisons to Make

### Layer.cs vs Layers.gd - Initial Values
```csharp
// C# - Expected values
myLayer[LAYERSKY] = new Layer(0, 3.2f);      // Y=0, parallax=3.2
myLayer[LAYERTREE] = new Layer(?, ?);        // Check these values
myLayer[LAYERSKYLINE] = new Layer(251, 5.6f);
myLayer[LAYERFREC] = new Layer(?, ?);
myLayer[LAYERPIT] = new Layer(275, 4.3f);
myLayer[LAYERMISC] = new Layer(?, ?);
```

### GameLoop Draw Order
Compare the exact sequence of SpriteSet.Draw() calls in:
- C#: `GameLoop.cs` Draw method
- GDScript: `GameLoop.gd` draw/render method

### SpriteSet.Draw() Implementation
Verify these match:
1. Loop through all sprites (0 to n)
2. Skip deleted sprites (b_deleted check)
3. Call individual sprite draw

---

## Debugging Steps

### Step 1: Add Debug Output
Add print statements to verify:
```gdscript
# In GameLoop render
print("Drawing ss_clouds: ", AIMethods.ss_clouds.n, " sprites")
print("Drawing ss_skyline: ", AIMethods.ss_skyline.n, " sprites")
# etc.
```

### Step 2: Verify Layer Offsets
```gdscript
# Print layer Y values
for i in range(6):
    print("Layer ", i, " Y offset: ", Globals.myLayers.get_offset(i))
```

### Step 3: Check Sprite Screen Positions
```gdscript
# In SpriteSet.draw()
for i in range(n):
    if sprites[i] and not sprites[i].b_deleted:
        print("Sprite at screen: ", sprites[i].n_scr_x, ", ", sprites[i].n_scr_y)
```

### Step 4: Verify Frame Data
```gdscript
# Check if sprites have valid frames
if sprites[i].frm_frame == null:
    print("WARNING: Sprite has no frame!")
```

---

## Potential Issues to Look For

1. **SpriteSets not being drawn at all** - Missing draw calls in GameLoop
2. **Wrong draw order** - Background drawing after foreground (getting covered)
3. **Layer offset wrong** - Sprites drawing off-screen (negative Y or too far down)
4. **Sprites not populated** - Background SpriteSets have n=0
5. **Frame not set** - Sprites exist but have no visual frame assigned
6. **Parallax calculation wrong** - Layer Y offset not updating with scroll
7. **Visibility/deletion flags** - Sprites marked as deleted or invisible
8. **Screen coordinate calculation** - n_scr_x/n_scr_y calculated wrong
9. **Texture not loading** - Frame exists but texture is null
10. **Z-ordering issues** - Godot's CanvasItem draw order different from expected

---

## Success Criteria

The analysis is complete when:
1. All 6 layers render with correct parallax scrolling
2. Sky/clouds visible at top
3. Skyline (buildings, etc.) visible behind pit
4. Trees visible at correct depth
5. FREC crowd visible behind pit wall
6. Pit layer (frosh, items) renders correctly (already working)
7. UI/console layer renders on top

---

## Reference: Working Screenshot Analysis

From the provided screenshot:
- ✅ Pit layer working (frosh visible, pizza, apple)
- ✅ UI layer working (timer, item counts on left)
- ✅ Pole visible
- ❌ Sky missing (should see clouds, blue sky)
- ❌ Background missing (should see skyline behind pole)
- ❌ Trees missing
- ❌ FREC crowd partially visible? (hard to tell)

The grey background suggests nothing is drawing behind the pit layer.

---

*Created: 2026-01-22*
*For: Layer rendering debug session*
