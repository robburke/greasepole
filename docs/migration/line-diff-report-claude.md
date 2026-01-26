# Line-by-Line Diff Report

**Generated**: 2026-01-25
**Methodology**: Statement-level semantic comparison with focus on sound calls, state transitions, and control flow

## Summary

| File | C# Lines | GD Lines | Delta | Sound Calls (C#/GD) | Status |
|------|----------|----------|-------|---------------------|--------|
| aiSupport.cs | 1337 | 684 | -653 | varies | Functions reorganized - OK |
| aiMenuAndDisplay.cs | 1559 | 1000 | -559 | varies | ai_icon bug FIXED |
| **aiFrosh.cs** | 1888 | 1559 | -329 | 18/25 | **5 BUGS FIXED** |
| aiProjectile.cs | 1621 | 1365 | -256 | 38/28 | Consolidated - OK |
| aiPopUp.cs | 822 | 662 | -160 | varies | All 8 switches PASS |
| aiCrowd.cs | - | - | -69 | 18/18 | OK |
| aiBackground.cs | - | - | -40 | 8/8 | OK |
| aiWaterEffects.cs | - | - | +5 | 0/0 | OK |
| aiMisc.cs | - | - | +83 | 7/7 | OK |
| AIDefine.cs | - | - | +9 | - | OK |
| SpriteInit.cs | - | - | - | - | All sprites match |
| PolePosition.cs | - | - | -19 | - | Perfect match |

**Total issues found**: 5 HIGH priority bugs in aiFrosh.cs - **ALL FIXED**

---

## aiProjectile Sound Calls Analysis

**C# aiProjectile.cs**: 38 `.Play()` calls
**GD aiProjectile.gd**: 28 `.play()` calls

**Delta: 10 fewer sound calls** - This is NOT missing sounds, but **code consolidation**.

### Explanation

The C# version has repetitive collision handling code for three FREC groups (FrecsL, FrecsC, FrecsR):
- Lines 408, 421-428, 440-441 (FrecsL)
- Lines 453, 467-474, 485-486 (FrecsC)
- Lines 498, 512-519, 530-531 (FrecsR)

Each section has identical sound logic repeated 3 times. The GDScript version consolidates this into a single parameterized function that processes all frec groups, reducing code duplication while maintaining identical functionality.

**Status**: ARCHITECTURAL IMPROVEMENT - Not missing functionality

---

## Verified Fix: ai_icon Inventory Check

The previously missed bug in `ai_icon` case 1 has been **FIXED**.

**C# Original (lines 970-973):**
```csharp
if (0 == aisGetPointsBasedOn(s.nAttrib[attrButtonType]))
{
    sSound[ssndEFFECTS_ICONOUT].Play(volFULL, panONX(s));
    s.nAttrib[attrIconStatus] = 2;
}
```

**GDScript Fixed (aiMenuAndDisplay.gd lines 500-503):**
```gdscript
# Check if inventory is now zero - transition to out of position
if ais_get_points_based_on(s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]) == 0:
    AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ICONOUT].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
    s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 2
```

**Status**: VERIFIED FIXED

---

## Function Reorganization Notes

Some C# functions from `aiSupport.cs` have been moved to different GDScript files:

| C# Function | C# File | GD Function | GD File |
|-------------|---------|-------------|---------|
| aisShoutDiscipline | aiSupport.cs | ais_shout_discipline | aiProjectile.gd |
| aisChaseClark | aiSupport.cs | ais_chase_clark | aiProjectile.gd |
| aisChasePizza | aiSupport.cs | ais_chase_pizza | aiProjectile.gd |
| aisRandomEvents | aiSupport.cs | ais_random_events | aiMisc.gd |

This reorganization is intentional and acceptable - functions are grouped with related code.

---

## Enumeration Verification

### Key Enums Verified

All critical enums use auto-incrementing values starting from 0, matching C# default behavior:

| C# Enum | GD Enum | Values Match |
|---------|---------|--------------|
| Goals | Goals | VERIFIED - 11 values (0-10) |
| Personalities | Personalities | VERIFIED - 4 values (0-3) |
| ArmPositions | ArmPositions | VERIFIED - 14 values (0-13) |
| Buttons | Buttons | VERIFIED - 7 values (0-6) |
| SpriteType | SpriteType | VERIFIED - ordering preserved |
| nattrFrosh | NAttrFrosh | VERIFIED - 10 values (0-9) |
| battrFrosh | BAttrFrosh | VERIFIED - 6 values (0-5) |
| attrArm | AttrArm | VERIFIED - 6 values (0-5) |
| attrIcon | AttrIcon | VERIFIED - 3 values (0-2) |
| ASLList | ASLList | VERIFIED - 156 long sounds |
| ASSList | ASSList | VERIFIED - 42 short sounds |

**Status**: All critical enums verified to match

---

## Line Delta Analysis

The large negative line deltas are explained by:

1. **C# Boilerplate**: Type declarations, braces, access modifiers (public static void)
2. **GDScript Terseness**: No braces, no explicit types, snake_case
3. **Function Reorganization**: Some functions moved between files
4. **Comment Reduction**: GDScript has fewer inline comments

These factors account for approximately 30-40% line reduction, which aligns with the observed deltas.

---

## HIGH Priority Issues - ALL FIXED

### 1. ai_act_11e - Missing case 30 (drunk frosh flying away) - **FIXED**

**C# Location:** `aiFrosh.cs` lines 1540-1547
**GDScript Fix:** `AIFrosh.gd` lines 1184-1193

Added case 30 to match block - drunk frosh now properly fall off pyramid when drinking beer on upper levels.

### 2. ai_act_11d - Missing Support check block - **FIXED**

**C# Location:** `aiFrosh.cs` lines 1467-1494
**GDScript Fix:** `AIFrosh.gd` lines 1137-1151

Added support check - frosh eating pizza on upper levels now fall if their support frosh falls away.

### 3. ai_act_11e - Missing Support check block - **FIXED**

**C# Location:** `aiFrosh.cs` lines 1555-1582
**GDScript Fix:** `AIFrosh.gd` lines 1202-1216

Added support check - frosh drinking beer on upper levels now fall if their support frosh falls away.

### 4 & 5. ai_init_11d/11e - Missing re-entry guards - **FIXED**

**C# Lines:** 1427-1428, 1503-1504
**GDScript Fix:** `AIFrosh.gd` lines 1107-1109, 1157-1159

Added re-entry guards - init functions now check if already in that behavior before resetting state.

---

## MEDIUM Priority Issues

*None* - aiPopUp was fully audited and all 8 switch statements passed.

---

## Recommendations

1. **Ongoing**: Consider adding automated tests that compare function behavior
2. **Testing**: Test frosh behavior on upper pyramid levels to verify fixes

---

## Completion Status

| Task | Status |
|------|--------|
| aiSupport function pairing | VERIFIED - reorganization documented |
| aiMenuAndDisplay ai_icon fix | VERIFIED |
| aiFrosh detailed audit | **5 BUGS FOUND AND FIXED** |
| aiProjectile sound calls | VERIFIED - consolidated (OK) |
| aiPopUp switch/match | VERIFIED - All 8 switches PASS |
| aiCrowd/aiBackground/aiWaterEffects/aiMisc/AIDefine | VERIFIED - All OK |
| Enumeration values | VERIFIED |
| SpriteInit | VERIFIED - All sprites match |
| PolePosition | VERIFIED - Perfect match (12 chains) |

---

## Fixes Applied (2026-01-25)

All 5 HIGH priority bugs in AIFrosh.gd have been fixed:

1. **ai_init_11d re-entry guard** - Added at lines 1107-1109
2. **ai_init_11e re-entry guard** - Added at lines 1157-1159
3. **ai_act_11d support check** - Added at lines 1137-1151
4. **ai_act_11e case 30 (drunk flying)** - Added at lines 1184-1193
5. **ai_act_11e support check** - Added at lines 1202-1216

## Next Steps

1. Re-test frosh behavior on upper pyramid levels
2. Build and deploy to verify no regressions
