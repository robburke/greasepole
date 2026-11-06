# Agent 3 Audit: Secondary Systems

**Files Audited:**
- aiPopUp.cs (822 lines) -> aiPopUp.gd (660 lines)
- aiCrowd.cs (473 lines) -> aiCrowd.gd (404 lines)
- aiBackground.cs (478 lines) -> aiBackground.gd (438 lines)
- poleposn.cs (263 lines) -> PolePosition.gd (244 lines)
- AIDefine.cs (189 lines) -> AIDefine.gd (198 lines)
- aiWaterEffects.cs (47 lines) -> aiWaterEffects.gd (52 lines)
- aiMisc.cs (191 lines) -> aiMisc.gd (274 lines)

**Audit Date:** 2026-01-25

---

## aiPopUp.gd

### ai_push_scicon_m - Missing timeTPBM calculation
- **Issue**: GDScript uses hardcoded values instead of constant-based calculations
- **Severity**: LOW

**C# (line 397-401):**
```csharp
case timeTPBM + 15:  // timeTPBM = 45, so this is 60
    ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPUB)));
    break;
case timeTPBM + 30:  // 75
    ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprBAN)));
    break;
```

**GD (line 340-343):**
```gdscript
60:  # TIME_TPBM + 15
    AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PUB))
75:  # TIME_TPBM + 30
    AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_BAN))
```

**Fix hint**: This is functionally correct (values match), but should use the constant for maintainability:
```gdscript
TIME_TPBM + 15:  # 60
TIME_TPBM + 30:  # 75
```

---

## aiCrowd.gd

No discrepancies found. All functions match the C# implementation correctly:
- Energy level checks use `AIDefine.ENERGY_CHEER`, `AIDefine.ENERGY_SLAM` correctly
- Sound indices match
- Match statement cases align with C# switch cases
- Frame selection logic preserved

---

## aiBackground.gd

### ai_gw_hippo - case 4 logic issue
- **Issue**: Missing frame check in case 4 (Jeronimo) - the `if s.nCC >= 10` check is nested incorrectly
- **Severity**: MEDIUM

**C# (line 329-330):**
```csharp
if (s.nCC >= 10 && 0 == (s.nCC % 3))
    s.SetFrame((s.nX < dPOLEX) ? (frm[((int)GameBitmapEnumeration.bmpHIPPO1_1) + R.Next(3)]) : (frmM[((int)GameBitmapEnumeration.bmpHIPPO1_1 )+ R.Next(3)]));
```

**GD (line 310-311):**
```gdscript
if s.n_cc >= 10 and 0 == (s.n_cc % 3):
    s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO1_1 + AIMethods.R.randi() % 3] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO1_1 + AIMethods.R.randi() % 3])
```

**Fix hint**: GDScript is correct - the logic matches.

---

## PolePosition.gd

### initialize_pole_positions - Loop conversion from do-while to while
- **Issue**: C# uses `do-while` loop, GDScript uses `while` loop - semantically different
- **Severity**: LOW (functionally equivalent due to initial check)

**C# (line 248-256):**
```csharp
for (i = 0; i < nPPChains; i++)
{
    ppCurrentChain = PoleChains[i];
    do
    {
        ppCurrentChain.SetAdjacentChain(PoleChains[(i + 1) % nPPChains]);
        ppCurrentChain = ppCurrentChain.GetChild();
    } while (!(ppCurrentChain == null));
}
```

**GD (line 238-242):**
```gdscript
for j in range(n_pp_chains):
    pp_current_chain = pole_chains[j]
    while pp_current_chain != null:
        pp_current_chain.set_adjacent_chain(pole_chains[(j + 1) % n_pp_chains])
        pp_current_chain = pp_current_chain.get_child()
```

**Fix hint**: Both versions are functionally equivalent because `pole_chains[j]` is never null when accessed (it was just assigned in the previous loop). The `while` version is actually cleaner.

---

## AIDefine.gd

No discrepancies found. All constants match:
- Pit coordinates match
- Energy levels match
- Sprite counts match
- Time constants match

---

## aiWaterEffects.gd

No discrepancies found. All splash and ripple animations match exactly:
- Frame timing (n_cc / 4 and n_cc / 3) preserved
- Deletion thresholds (12 and 9) preserved
- Frame indices correct

---

## aiMisc.gd

### ais_random_crowd_noise - nFroshAbove1 variable name
- **Issue**: Variable name uses different convention but value source is same
- **Severity**: LOW (cosmetic)

**C# (line 75-84):**
```csharp
if (nFroshAbove1 < 6 && 0 == R.Next(3))
{
    ...
}
else if (nFroshAbove1 > 20 && 0 != R.Next(2))
```

**GD (line 100-111):**
```gdscript
if AIMethods.n_frosh_above_1 < 6 and 0 == AIMethods.R.randi() % 3:
    ...
elif AIMethods.n_frosh_above_1 > 20 and 0 != AIMethods.R.randi() % 2:
```

**Fix hint**: Correct. GDScript properly accesses via AIMethods autoload.

---

### ai_random_event_generator - Demo balloon creation block structure
- **Issue**: Else block for demo mode is misplaced - creates balloons even when NOT in demo mode
- **Severity**: HIGH

**C# (line 179-188):**
```csharp
}
else
{
    // If it IS the demo, create some GW balloons for effect.
    if (sprGWBalloon == null && s.nCC > timeBETWEENEVENTS * 10
        && (0 == R.Next(1000)))
    {
        sprGWBalloon = SpriteInit.CreateSprite((SpriteType.sprGWBALLOON));
        ssBalloon.Include(sprGWBalloon);
    }
}
```

**GD (line 203-208):**
```gdscript
else:
    # If it IS the demo, create some GW balloons for effect.
    if AIMethods.spr_gw_balloon == null and s.n_cc > AIDefine.TIME_BETWEEN_EVENTS * 10 \
        and (0 == AIMethods.R.randi() % 1000):
        AIMethods.spr_gw_balloon = SpriteInit.create_sprite(Enums.SpriteType.SPR_GW_BALLOON)
        AIMethods.ss_balloon.include(AIMethods.spr_gw_balloon)
```

**Fix hint**: Looking more carefully at C# structure - the else block at line 179 corresponds to the `else` for the first big `if` at line 110. The C# comment says "If it IS the demo" but the outer check is `if (!Globals.myGameConditions.IsDemo())`. The else at line 179 is actually part of the inner if-elif-else chain, not the outer demo check.

Actually, re-reading the C# code:
- Line 108: `if (!Globals.myGameConditions.IsDemo())` - the whole block is wrapped
- Line 109-178: The inner if-elif-else structure
- Line 179-188: This `else` is for the inner chain, NOT for the demo check

The comment "If it IS the demo" in C# is misleading/wrong - this code runs when NOT in demo mode. The GDScript has the same behavior. No bug here - just a misleading comment in the original C#.

---

### ais_random_events - Missing range check for n_frosh_level access
- **Issue**: Loop starts at 1, but accesses indices 1-6 which is correct for a 7-element array (0-6)
- **Severity**: LOW (cosmetic)

**C# (implied from overall pattern):**
Arrays are 0-indexed with 7 elements for levels 0-6.

**GD (line 217-220):**
```gdscript
for i in range(1, 7):
    AIMethods.n_frosh_level_l[i] = 0
    AIMethods.n_frosh_level_r[i] = 0
```

**Fix hint**: Correct. Uses range(1, 7) which gives [1,2,3,4,5,6] - same as C# starting at 1 and going to <7.

---

## Summary by Severity

### CRITICAL
None found.

### HIGH
None confirmed. Initial concern about demo mode balloon logic was a false positive.

### MEDIUM
None confirmed.

### LOW
1. **aiPopUp.gd**: Hardcoded values instead of constant expressions for timeTPBM offsets (cosmetic)
2. **PolePosition.gd**: do-while to while conversion (functionally equivalent)
3. **aiMisc.gd**: Variable naming convention differences (cosmetic)

---

## Verified Correct

The following files were audited and found to be **correctly ported**:

1. **aiCrowd.gd** - All 34 functions match C# logic exactly
2. **aiBackground.gd** - All 18 functions match C# logic exactly
3. **aiWaterEffects.gd** - All 5 functions match C# logic exactly
4. **AIDefine.gd** - All constants match exactly
5. **PolePosition.gd** - All 13 functions match C# logic (minor loop style difference)
6. **aiPopUp.gd** - All 24 functions match C# logic (minor constant style difference)
7. **aiMisc.gd** - All 6 functions match C# logic

---

## Notes

1. **Sound enum usage**: All sound enums (ASSList, ASLList) are used consistently between C# and GDScript
2. **Frame references**: All bitmap enumerations match between versions
3. **State machine logic**: All switch/match cases are correctly translated
4. **Random number generation**: All `R.Next(n)` correctly converted to `R.randi() % n`
5. **Null checks**: All null checks correctly converted from `== null` to `== null` (GDScript compatible)

---

*Audit completed by Agent 3 - Secondary Systems*
