# GodotLegend C# to GodotLegendGD GDScript Porting Rules

## Environment
- **Godot (Mono/C#):** `C:\Users\rob\Downloads\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64\Godot_v4.5.1-stable_mono_win64.exe`
- **Godot (GDScript/Web):** `C:\Users\rob\Downloads\Godot_v4.5.1-stable_win64.exe` (required for web export)
- **Export Templates:** `C:\Users\rob\AppData\Roaming\Godot\export_templates\4.5.1.stable\`

---

## Web Export Commands

### Quick Export (One-liner)
```bash
"C:\Users\rob\Downloads\Godot_v4.5.1-stable_win64.exe" --headless --path "e:/projects/Greasepole2025/GodotLegendGD" --export-release "Web" "builds/web/index.html"
```

### Test Locally
```bash
cd e:/projects/Greasepole2025/GodotLegendGD/builds/web
python -m http.server 8080
# Then open: http://localhost:8080
```

### Important Notes
- **Must use non-Mono Godot** for web export (the one WITHOUT "mono" in the name)
- **Cannot use C#/Mono editor** for web - Godot 4 doesn't support C# web export
- Export preset "Web" is already configured in `export_presets.cfg`
- Output goes to `GodotLegendGD/builds/web/`

### Export Output Files
```
builds/web/
├── index.html       # Main HTML file
├── index.js         # JavaScript glue code
├── index.pck        # Game assets (textures, scenes, scripts)
├── index.wasm       # Godot engine WebAssembly binary
└── index.*.png      # Icons
```

---

## Goal
Port the AI and game logic from GodotLegend (C#) to GodotLegendGD (GDScript) with **zero logic errors**. The file structure and logic must be preserved exactly to enable debugging and comparison.

---

## CRITICAL: C# Partial Class Strategy (The "God Class" Problem)

### The Problem

In C#, `AIMethods` is declared as `public static partial class AIMethods` across 12+ files:
- `aiCrowd.cs`, `aiMenuAndDisplay.cs`, `aiFrosh.cs`, `aiProjectile.cs`, etc.

**All these files share the same static state:**
```csharp
// These variables are accessible from ANY partial file
public static TSprite sprArm;
public static TSprite sprTam;
public static SpriteSet ssIcons;
public static FrameDesc[] frm;
public static Random R;
// ... 50+ more shared variables
```

**The Risk:** If we create separate `aiCrowd.gd` and `aiMenuAndDisplay.gd` as independent GDScript classes, they **cannot see each other's variables**. This breaks the architecture and causes bugs.

### The Solution: State vs Logic Separation

```
┌─────────────────────────────────────────────────────────────────┐
│  AIMethods.gd (Autoload)                                        │
│  ─────────────────────────────────────────────────────────────  │
│  ALL SHARED STATE lives here:                                   │
│  - var spr_arm: TSprite                                         │
│  - var spr_tam: TSprite                                         │
│  - var ss_icons: SpriteSet                                      │
│  - var frm: Array[FrameDesc]                                    │
│  - var R: RandomNumberGenerator                                 │
│  - ... all static variables from C# partials                    │
└─────────────────────────────────────────────────────────────────┘
        │
        │ Logic files access state via: AIMethods.spr_arm
        ▼
┌───────────────┐ ┌───────────────┐ ┌───────────────┐
│ aiCrowd.gd    │ │ aiMenu...gd   │ │ aiFrosh.gd    │
│ ───────────── │ │ ───────────── │ │ ───────────── │
│ LOGIC ONLY    │ │ LOGIC ONLY    │ │ LOGIC ONLY    │
│ static funcs  │ │ static funcs  │ │ static funcs  │
│ that operate  │ │ that operate  │ │ that operate  │
│ on AIMethods  │ │ on AIMethods  │ │ on AIMethods  │
│ state         │ │ state         │ │ state         │
└───────────────┘ └───────────────┘ └───────────────┘
```

### Implementation Rules

**Rule 1: Static Variables → AIMethods.gd**
```csharp
// C# (in any partial file)
public static TSprite sprArm;
public static int gnPitTimeH;
```
```gdscript
# GDScript - AIMethods.gd (Autoload)
var spr_arm: TSprite
var gn_pit_time_h: int
```

**Rule 2: Logic Functions → Individual ai*.gd files**
```csharp
// C# - aiMenuAndDisplay.cs
public static void aiMenuStartButton(TSprite s) {
    if (aisMouseOver(s)) { ... }
    sprArm.nAttrib[0] = 1;  // accesses shared state
}
```
```gdscript
# GDScript - aiMenuAndDisplay.gd
class_name AIMenuAndDisplay

static func ai_menu_start_button(s: TSprite):
    if AISupport.ais_mouse_over(s): ...
    AIMethods.spr_arm.n_attrib[0] = 1  # accesses shared state via AIMethods
```

**Rule 3: Cross-File Function Calls**
```csharp
// C# - aiMenuAndDisplay.cs calls function from aiSupport.cs
if (aisMouseOver(s)) { ... }
```
```gdscript
# GDScript - must qualify with class name
if AISupport.ais_mouse_over(s): ...
```

**Rule 4: Register Callables in AIMethods**

For the delegate/Callable pattern to work, AIMethods needs to know about all AI functions:
```gdscript
# AIMethods.gd
func _ready():
    # Register all AI functions for Callable lookup
    pass

# When setting behaviors, reference the correct class:
sprite.set_behavior(Callable(AIMenuAndDisplay, "ai_menu_start_button"))
sprite.set_behavior(Callable(AICrowd, "ai_frec_group"))
sprite.set_behavior(Callable(AIFrosh, "ai_frosh_wander"))
```

### File Organization Summary

| C# Location | GDScript Location | Contains |
|-------------|-------------------|----------|
| Static vars in ANY partial | `AIMethods.gd` | All shared state |
| `aiCrowd.cs` functions | `aiCrowd.gd` | Crowd logic only |
| `aiMenuAndDisplay.cs` functions | `aiMenuAndDisplay.gd` | Menu logic only |
| `aiFrosh.cs` functions | `aiFrosh.gd` | Frosh logic only |
| `aiSupport.cs` functions | `aiSupport.gd` | Helper functions |
| ... | ... | ... |

### Autoload Configuration

In `project.godot`, AIMethods must be an Autoload so all logic files can access it:
```ini
[autoload]
AIMethods="*res://Core/AI/AIMethods.gd"
```

The logic files (`aiCrowd.gd`, etc.) are **NOT** autoloads - they're just static class definitions.

---

## Project Structure Mapping

### Source (C# - GodotLegend)
```
Core/AI/
├── AIDefine.cs          # Constants (190+ game constants)
├── aiEnums.cs           # Enumerations (14+ enums)
├── aiBackground.cs      # Background entities (Prez, Forge, Hippo, etc.)
├── aiCrowd.cs           # Crowd behavior system
├── aiFrosh.cs           # Main frosh AI (~31,000 lines)
├── aiMenuAndDisplay.cs  # Menu/UI logic
├── aiMisc.cs            # Random events, achievements, projectile pickup
├── aiPopUp.cs           # Art-sci, Commie, Hose popup characters
├── aiProjectile.cs      # Projectile physics
├── aiSupport.cs         # Helper functions, sprite management
├── aiWaterEffects.cs    # Splash/ripple animations
└── poleposn.cs          # Pole position chain system
```

### Target (GDScript - GodotLegendGD)
```
Core/AI/
├── AIMethods.gd         # AUTOLOAD: All shared state from C# partials
├── AIDefine.gd          # Constants (static class)
├── Enums.gd             # Already partially created - expand
├── aiBackground.gd      # Background logic (static class, accesses AIMethods)
├── aiCrowd.gd           # Crowd logic (static class, accesses AIMethods)
├── aiFrosh.gd           # Frosh logic (static class, accesses AIMethods)
├── aiMenuAndDisplay.gd  # Menu/UI logic (static class, accesses AIMethods)
├── aiMisc.gd            # Misc logic (static class, accesses AIMethods)
├── aiPopUp.gd           # Popup logic (static class, accesses AIMethods)
├── aiProjectile.gd      # Projectile logic (static class, accesses AIMethods)
├── aiSupport.gd         # Helper functions (static class, accesses AIMethods)
├── aiWaterEffects.gd    # Water effects (static class, accesses AIMethods)
└── PolePosition.gd      # Pole position chain class
```

**Key distinction:**
- `AIMethods.gd` = Autoload singleton holding ALL shared state
- All other `ai*.gd` files = Static classes with logic functions only

---

## Critical Porting Rules

### 1. Naming Conventions

| C# | GDScript |
|----|----------|
| `PascalCase` methods | `snake_case` methods |
| `aiMouseCursorMenu` | `ai_mouse_cursor_menu` |
| `aiInitFlyInAndOut2` | `ai_init_fly_in_and_out_2` |
| `aisMouseOver` | `ais_mouse_over` |
| `nX`, `nY`, `nZ` | `n_x`, `n_y`, `n_z` |
| `nvX`, `nvY` | `nv_x`, `nv_y` |
| `nCC` | `n_cc` |
| `nAttrib[0]` | `n_attrib[0]` |
| `bAttrib[1]` | `b_attrib[1]` |
| `frmFrame` | `frm_frame` |

### 2. Enum Mapping

**C# enum access:**
```csharp
s.nAttrib[((int)nattrFrosh.attrBehavior)]
```

**GDScript equivalent:**
```gdscript
sprite.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR]
```

**CRITICAL:** Ensure all enum values map to identical integer values. Create a validation test.

### 3. Delegate/Callable Conversion

**C# delegate pattern:**
```csharp
public delegate void AIMethod(TSprite sprite);
s.SetBehavior(aiMenuStartButton);
s.pfAI(this);  // invocation
```

**GDScript Callable pattern:**
```gdscript
var pf_ai: Callable
sprite.set_behavior(Callable(AIMethods, "ai_menu_start_button"))
pf_ai.call(self)  # invocation
```

### 4. Static Class Pattern (See "Partial Class Strategy" Above)

**C# partial static class:**
```csharp
// aiMenuAndDisplay.cs
public static partial class AIMethods
{
    public static void aiMenuStartButton(TSprite s) { ... }
    public static TSprite sprArm;  // shared across ALL partials
}
```

**GDScript - State goes to AIMethods.gd (Autoload):**
```gdscript
# AIMethods.gd - AUTOLOAD
extends Node

var spr_arm: TSprite  # shared state lives HERE
var ss_icons: SpriteSet
var frm: Array[FrameDesc]
# ... all static variables
```

**GDScript - Logic goes to individual files:**
```gdscript
# aiMenuAndDisplay.gd - STATIC CLASS (not autoload)
class_name AIMenuAndDisplay

static func ai_menu_start_button(sprite: TSprite):
    # Access shared state via AIMethods autoload
    AIMethods.spr_arm.n_attrib[0] = 1
    sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLESTART])
```

### 5. Service Access Pattern

**C# service locator:**
```csharp
Globals.InputService.GetMouseX()
Globals.InputService.LeftButtonPressed()
Globals.myGameLoop.ChangeGameState(...)
Globals.myGameConditions.GetPlayerApples()
Globals.RenderingService.GetFps()
```

**GDScript equivalent:**
```gdscript
Globals.InputService.get_mouse_x()
Globals.InputService.left_button_pressed()
Globals.GameLoop.change_game_state(...)
Globals.GameConditions.get_player_apples()
Globals.RenderingService.get_fps()
```

### 6. Frame/Bitmap Access

**C#:**
```csharp
s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMOU_SELECT2)]);
```

**GDScript:**
```gdscript
sprite.set_frame(Globals.frames[Enums.GameBitmapEnumeration.bmpMOU_SELECT2])
```

### 7. Sound System

**C#:**
```csharp
sSound[((int)ASSList.ssndMENU_SELECT)].Play(SoundbankInfo.volFULL);
lSound[((int)ASLList.lsndNARRATOR_STARTDELAY1)].Play(SoundbankInfo.volHOLLAR);
sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].Loop(SoundbankInfo.volMUSIC);
sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].Stop();
sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].IsPlaying();
```

**GDScript:**
```gdscript
Globals.SoundService.play_static(Enums.ASSList.SSND_MENU_SELECT, SoundbankInfo.VOL_FULL)
Globals.SoundService.play_long(Enums.ASLList.LSND_NARRATOR_STARTDELAY1, SoundbankInfo.VOL_HOLLAR)
Globals.SoundService.loop_static(Enums.ASSList.SSND_MENU_TITLEREPEAT, SoundbankInfo.VOL_MUSIC)
Globals.SoundService.stop(Enums.ASSList.SSND_MENU_TITLEREPEAT)
Globals.SoundService.is_playing(Enums.ASSList.SSND_MENU_TITLEREPEAT)
```

### 8. Math Functions

| C# | GDScript |
|----|----------|
| `Math.Abs(x)` | `abs(x)` or `absi(x)` |
| `R.Next(n)` | `randi() % n` or `randi_range(0, n-1)` |
| `R.Next(min, max)` | `randi_range(min, max-1)` |

### 9. Array/Collection Iteration

**C#:**
```csharp
for (int j = 0; j < ssIcons.GetNumberOfSprites(); j++)
{
    if (ssIcons.GetSprite(j).SpriteType == SpriteType.sprmnuTITLESTART)
        ...
}
```

**GDScript:**
```gdscript
for j in range(ss_icons.get_number_of_sprites()):
    if ss_icons.get_sprite(j).sprite_type == Enums.SpriteType.SPRMNU_TITLESTART:
        ...
```

### 10. Conditional Expressions

**C# ternary:**
```csharp
int nValueSelected = (s.nX > threshold) ? 1 : 0;
```

**GDScript:**
```gdscript
var n_value_selected = 1 if sprite.n_x > threshold else 0
```

---

## File-by-File Porting Checklist

### Phase 1: Foundation (Complete before AI)
- [ ] Enums.gd - Complete all enum definitions with matching values
- [ ] AIDefine.gd - All constants
- [ ] SoundbankInfo.gd - Volume constants, sound indices
- [ ] PolePosition.gd - Pole position chain class

### Phase 2: Support Systems
- [ ] aiSupport.gd - Helper functions (aisMouseOver, aisMoveTowardsDestination, etc.)
- [ ] GameConditions.gd - Game state tracking

### Phase 3: AI Files (Order matters!)
1. [ ] aiWaterEffects.gd - Simplest, good test case
2. [ ] aiMenuAndDisplay.gd - Menu logic, UI
3. [ ] aiBackground.gd - Background entities
4. [ ] aiProjectile.gd - Projectile physics
5. [ ] aiPopUp.gd - Popup characters
6. [ ] aiMisc.gd - Random events
7. [ ] aiCrowd.gd - Crowd behavior
8. [ ] aiFrosh.gd - Main AI (largest, do last)

---

## Verification Strategy

### 1. Enum Value Verification
Create a test that prints all enum values from both C# and GDScript and compares them:

```gdscript
# test_enum_values.gd
func verify_enums():
    # Each enum value must match the C# integer value
    assert(Enums.NAttrFrosh.ATTR_BEHAVIOR == 0)
    assert(Enums.NAttrFrosh.ATTR_GOAL == 1)
    # ... etc
```

### 2. Function Signature Verification
For each AI function, verify:
- Same parameter type (TSprite)
- Same attribute indices accessed
- Same frame indices used
- Same sound indices used

### 3. Logic Flow Verification
For complex functions:
1. Add debug prints at key decision points in both versions
2. Run same scenarios
3. Compare outputs

### 4. Frame-by-Frame Comparison
For critical AI:
1. Record nCC (cycle counter) values at key states
2. Compare sprite positions at same nCC values

---

## Common Pitfalls

### 1. Integer Division
**C#:** `int / int` truncates automatically
**GDScript:** Use `int(a / b)` or `a / b` (auto-truncates for int operands in 4.x)

### 2. Boolean-to-Int Conversion
**C#:** `0 == value` works as condition
**GDScript:** Same, but be explicit: `value == 0`

### 3. Modulo with Frame Counters
**C#:** `(s.nCC / 2) % 8`
**GDScript:** `(sprite.n_cc / 2) % 8` - works same

### 4. Null Checks
**C#:** `!(ppChosen == null)`
**GDScript:** `pp_chosen != null`

### 5. Array Index Safety
**C#:** Direct access may throw
**GDScript:** Check `size()` or use `get()` with default

### 6. Switch Statements
**C#:**
```csharp
switch(value) {
    case 0: ...; break;
    case 1: ...; break;
}
```

**GDScript:**
```gdscript
match value:
    0: ...
    1: ...
```

---

## Global State Variables (AIMethods.gd)

All shared state from C# partial classes lives in **AIMethods.gd** (Autoload).
Logic files access these via `AIMethods.variable_name`.

```gdscript
# In AIMethods.gd (Autoload)
extends Node

# === SPRITE REFERENCES ===
var spr_arm: TSprite
var spr_tam: TSprite
var spr_pole: TSprite
var spr_pop_boy: TSprite
var spr_power_meter: TSprite
var spr_water_meter: TSprite
var spr_ring_meter: TSprite
var spr_fps_0: TSprite
var spr_fps_1: TSprite

# Sprite sets
var ss_fr: SpriteSet          # Frosh
var ss_pit: SpriteSet         # Pit sprites
var ss_icons: SpriteSet       # Icons/UI
var ss_jacket_slam: SpriteSet # Jacket slam animation
var ss_frecs_l: SpriteSet     # Left crowd
var ss_frecs_c: SpriteSet     # Center crowd
var ss_frecs_r: SpriteSet     # Right crowd

# Frame arrays
var frm: Array[FrameDesc]     # Normal frames
var frm_m: Array[FrameDesc]   # Mirrored frames

# Sound arrays
var s_sound: Array            # Static sounds
var l_sound: Array            # Long/streaming sounds

# === GAME TIMING ===
var gn_pit_time_h: int        # Hours
var gn_pit_time_m: int        # Minutes
var gn_pit_time_s: int        # Seconds
var gb_show_fps: bool         # FPS display toggle

# === RANDOM NUMBER GENERATOR ===
var R: RandomNumberGenerator  # Equivalent to C#'s static Random R

# === CONSTANTS (from AIDefine.cs) ===
# These can alternatively live in AIDefine.gd as const
const d_MOUSE_OFFSET = 20
const d_NUM_OFFSET_SHIFTS = 20
# ... etc (190+ constants)
```

### Accessing State from Logic Files

```gdscript
# In aiMenuAndDisplay.gd
class_name AIMenuAndDisplay

static func ai_menu_start_button(s: TSprite):
    # Access shared state via AIMethods autoload
    if AISupport.ais_mouse_over(s):
        AIMethods.spr_arm.n_attrib[0] = 1

    # Access frames
    s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLESTART])

    # Access sprite sets
    var num = AIMethods.ss_icons.get_number_of_sprites()

    # Use random
    var rand_val = AIMethods.R.randi() % 10
```

---

## Testing Protocol

### Per-Function Testing
1. Port one function at a time
2. Write a minimal test that exercises the function
3. Compare behavior to C# version
4. Only move to next function after verification

### Integration Testing
1. Test menu flow completely before game logic
2. Test projectiles in isolation
3. Test crowd independently
4. Test frosh last (most complex)

### Regression Testing
After each file is complete:
1. Run full game loop
2. Verify no crashes
3. Check visual output matches

---

## Notes for Maximum Reliability

1. **Never skip a function** - Even trivial functions like `aiInanimate` must be ported
2. **Preserve magic numbers** - Don't "clean up" constants like 23, 7, 40 - they're tuned
3. **Match timing exactly** - Frame counts (nCC checks) must be identical
4. **Keep debug capability** - Port any debug/FPS display code for testing
5. **Sound is optional first** - Can stub sound calls initially to test logic
6. **Document deviations** - If you must deviate, document why
