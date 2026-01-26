# Legend of the Greasepole - Bug Backlog

## Open Bugs

### Beer collision with Pop Boy in both C# and GD versions
**Status:** Needs investigation
**Reported:** 2025-01-24
**Description:** Beer (Clark mug) very often goes right through Pop Boy

**Analysis so far:**
- The collision check in `aiProjectile.gd:288` has an extra `n_z < 2` check that apples/pizza don't have
- This is intentional (matches C#) - Pop Boy must be on the ground to catch beer
- Possible cause: Pop Boy's `n_z` value may be staying >= 2 more often in GDScript than in C#
- Need to investigate Pop Boy's z-position management
- Need to investigate beer going through pop boy during his initial dialog even in C# version (when POPBOY_GREETING is playing - he should be able to be interrupted and go to beer drinking state)

**Files involved:**
- `GodotLegendGD/Core/AI/aiProjectile.gd` - collision detection
- `GodotLegendGD/Core/AI/AIFrosh.gd` - Pop Boy behavior

---

## Fixed Bugs

### Missing LOAD_SPLASHSCREEN background in menu modes (GDScript)
**Fixed:** 2026-01-25
**Description:** Grey background was visible in menu screens. The original game rendered LOAD_SPLASHSCREEN behind all menu content to prevent grey from showing.
**Fix:** Added splashscreen sprite as first background layer in STATETITLE and STATEDECORATE initialization in `GameLoop.gd`. STATEOPTIONS inherits from STATETITLE so no separate fix needed.
**Note:** C# version still needs this fix applied.

### Discipline bars playing wrong sound effects
**Fixed:** 2025-01-24
**Description:** Several discipline bars had wrong sound mappings:
- Bar 3 (Electrical) played DEFAULT instead of ELEC
- Bar 4 (Eng Phys) played ELEC instead of ENGPHYS
- Bar 6 (Geo) played ENGPHYS instead of GEO
- Bar 7 (Materials Science) played GEO instead of METALS
- Bar 9 (Mining) played METALS instead of MINING
- Bar 17 was missing METALS sound mapping
**Fix:** Corrected `ai_init_bar()` sound mappings in `aiMenuAndDisplay.gd` to match C# original

### MORE button on Decorate screen playing unwanted sound
**Fixed:** 2025-01-24
**Description:** The MORE button (bar navigation) played `SSND_MENU_TOGGLE` sound on click, but C# original doesn't play any sound
**Fix:** Removed sound effects from `ai_next_bar_screen()`, `ai_prev_bar_screen()`, and `ai_next_achievement_screen()` in `aiMenuAndDisplay.gd`. Also added missing toggle button keyboard support.

### Decorate screen: Missing narrator sound on open
**Fixed:** 2025-01-24
**Fix:** Added `LSND_NARRATOR_JACKETINIT.play()` in `GameLoop.gd` when entering STATEDECORATE

### Decorate screen: Quick click on discipline bars not working
**Fixed:** 2025-01-24
**Fix:** Added `ais_pick_jacket_bar_spot()` function and updated `ai_bar()` to use C# quick-click logic

### Pop Boy in pit saying wrong dialogue
**Fixed:** 2025-01-24
**Description:** Pop Boy said POPBOY_EXAM1 instead of encouraging advice when pyramid collapsed
**Fix:** Changed `AISupport.gd` to call `ais_think_for_al()` and set behavior=5 instead of playing EXAM sound directly
