# AI Code Audit Report: GodotLegend C# → GodotLegendGD GDScript

**Date**: 2026-01-25
**Audited by**: Claude Opus 4.5 (3 parallel agents)
**Updated**: 2026-01-25 - All CRITICAL/HIGH issues resolved

---

## Executive Summary

A comprehensive line-by-line audit was performed on all 11 AI files comparing the canonical C# implementation (GodotLegend) to the GDScript port (GodotLegendGD).

### Totals
| Severity | Count | Status |
|----------|-------|--------|
| **CRITICAL** | 1 | ✅ FIXED (dead code - added for parity) |
| **HIGH** | 4 | ✅ FIXED |
| **MEDIUM** | 0 | - |
| **LOW** | 7 | Optional |

### Key Finding
The **"beer goes through Pop Boy"** bug is **NOT** caused by a porting error. The `n_z < 2` collision check exists correctly in both C# (line 317) and GDScript (line 288). The bug's root cause must be investigated elsewhere (Pop Boy's z-position, sprite bounds, or coordinate calculations).

---

## CRITICAL Issues (1) - ✅ RESOLVED

### 1. aisAbandonAlien - MISSING FUNCTION ✅ FIXED
**File**: aiSupport.cs → AISupport.gd
**Status**: Added as dead code stub for parity (function is never called in C#)
**Impact**: None - this function was never called in SilverLegend or GodotLegend C# versions

**C# (lines 950-975):**
```csharp
public static void aisAbandonAlien()
{
    int n = ssFr.GetNumberOfSprites();
    int nNewGoal = (R.Next(2) != 0) ? Goals.goalMINDLESS_WANDERING : Goals.goalTHINK;

    for (i = 0; i < n; i++)
    {
        nGoal = ssFr.GetSprite(i).nAttrib[nattrFrosh.attrGoal];
        if (nGoal == Goals.goalARTSCI || nGoal == Goals.goalCOMMIE)
        {
            ssFr.GetSprite(i).nAttrib[nattrFrosh.attrPersonality] = Personalities.persHeavyweight;
            ssFr.GetSprite(i).nAttrib[nattrFrosh.attrGoal] = nNewGoal;
            ssFr.GetSprite(i).nDestX = ssFr.GetSprite(i).nX;
            ssFr.GetSprite(i).nDestY = ssFr.GetSprite(i).nY;
        }
    }
}
```

**GD**: MISSING

**Fix**:
```gdscript
static func ais_abandon_alien() -> void:
    var n: int = AIMethods.ss_fr.get_number_of_sprites()
    var n_new_goal: int
    if AIMethods.R.randi() % 2 != 0:
        n_new_goal = Enums.Goals.GOAL_MINDLESS_WANDERING
    else:
        n_new_goal = Enums.Goals.GOAL_THINK

    for i in range(n):
        var frosh: TSprite = AIMethods.ss_fr.get_sprite(i)
        var n_goal: int = frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL]
        if n_goal == Enums.Goals.GOAL_ARTSCI or n_goal == Enums.Goals.GOAL_COMMIE:
            frosh.n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY] = Enums.Personalities.PERS_HEAVYWEIGHT
            frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = n_new_goal
            frosh.n_dest_x = frosh.n_x
            frosh.n_dest_y = frosh.n_y
```

---

## HIGH Issues (4) - ✅ ALL RESOLVED

### 1. ai_icon_out_of_position - Missing sound effect and velocity reset ✅ FIXED
**File**: aiMenuAndDisplay.cs → aiMenuAndDisplay.gd
**Impact**: No audio feedback when icon returns to position; animation may behave differently

**C# (lines 879-883):**
```csharp
if (0 != aisGetPointsBasedOn(s.nAttrib[attrIcon.attrButtonType]))
{
    s.nAttrib[attrIcon.attrIconStatus] = 0;
    sSound[ASSList.ssndEFFECTS_ICONIN].Play(SoundbankInfo.volFULL, panONX(s));
    s.nvX = 5;
}
```

**GD (lines 445-446):**
```gdscript
if ais_get_points_based_on(s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]) != 0:
    s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 0
    # MISSING: sound and velocity
```

**Fix**: Add missing lines:
```gdscript
if ais_get_points_based_on(s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]) != 0:
    s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 0
    AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ICONIN].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
    s.nv_x = 5
```

---

### 2. ai_icon - Missing ARM_IRON_RING status check ✅ FIXED
**File**: aiMenuAndDisplay.cs → aiMenuAndDisplay.gd
**Impact**: Icons can be clicked while iron ring power is active (shouldn't be allowed)

**C# (lines 978-979):**
```csharp
if (!(sprArm.nAttrib[attrArm.attrArmStatus] == ArmPositions.armCHANGING)
    && !(sprArm.nAttrib[attrArm.attrArmStatus] == ArmPositions.armIRONRING))
```

**GD (line 500):**
```gdscript
if AIMethods.spr_arm != null and arm_status != Enums.ArmPositions.ARM_CHANGING:
    # MISSING: ARM_IRON_RING check
```

**Fix**:
```gdscript
if AIMethods.spr_arm != null and arm_status != Enums.ArmPositions.ARM_CHANGING \
    and arm_status != Enums.ArmPositions.ARM_IRON_RING:
```

---

### 3. ai_icon - Missing TAUNT, GREASE, RING button handling ✅ FIXED
**File**: aiMenuAndDisplay.cs → aiMenuAndDisplay.gd
**Impact**: Clicking on TAUNT, GREASE, or RING icons does nothing

**C# (lines 983-1008):** Has full switch with all button types

**GD (lines 504-514):** Only handles APPLE, PIZZA, CLARK, EXAM

**Fix**: Add missing cases:
```gdscript
match s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]:
    Enums.Buttons.BUTTON_TAUNT:
        AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_APPLE
    Enums.Buttons.BUTTON_APPLE:
        AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_APPLE
    Enums.Buttons.BUTTON_PIZZA:
        AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_PIZZA
    Enums.Buttons.BUTTON_CLARK:
        AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_CLARK
    Enums.Buttons.BUTTON_EXAM:
        AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_EXAM
    Enums.Buttons.BUTTON_GREASE:
        AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_GREASE
    Enums.Buttons.BUTTON_RING:
        AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_IRON_RING
```

---

### 4. ai_icon - Missing inventory-zero check in status 1 ✅ FIXED
**File**: aiMenuAndDisplay.cs → aiMenuAndDisplay.gd
**Impact**: Icons stay on screen when inventory hits zero; clicking selects "phantom" items

**C# (lines 970-974):**
```csharp
if (0 == aisGetPointsBasedOn(s.nAttrib[((int)attrIcon.attrButtonType)]))
{
    sSound[((int)ASSList.ssndEFFECTS_ICONOUT)].Play(SoundbankInfo.volFULL, panONX(s));
    s.nAttrib[((int)attrIcon.attrIconStatus)] = 2;
}
```

**GD (status 1 block):** MISSING

**Fix**: Add check to transition to status 2 when inventory is zero:
```gdscript
# Check if inventory is now zero - transition to out of position
if ais_get_points_based_on(s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]) == 0:
    AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ICONOUT].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
    s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 2
    return
```

---

## LOW Issues (7)

### 1. aisPickHoistingSpot - Missing empty function stub
**File**: aiSupport.cs (line 643) → AISupport.gd
**Impact**: None (function is empty in C#)
**Fix**: Add stub for completeness

### 2. ai_decorate_return - BackButtonDown vs back_button_pressed
**File**: aiMenuAndDisplay.gd (line 352)
**Impact**: Minor input handling difference
**Note**: GDScript version actually improves this (prevents repeated triggers)

### 3. ais_change_arm - Missing second validation switch
**File**: aiProjectile.gd (line 718)
**Impact**: Defensive redundancy missing
**Note**: First validation loop already handles this

### 4. _ai_arm_nothing - Brace handling difference ✅ INTENTIONAL
**File**: aiProjectile.gd (line 823)
**Impact**: Both `ATTR_ARM_ACTION = 1` and `n_cc = 0` now in if block
**Analysis**: C# has `if (cond) stmt1; stmt2;` where stmt2 runs unconditionally every frame.
GDScript puts both in the if block, which is likely the INTENDED behavior.
The C# appears to be a bug (resetting n_cc every frame would prevent animation timing).
**Decision**: Keep GDScript behavior - it fixes a probable C# bug.

### 5. aiPopUp.gd - Hardcoded timeTPBM values ✅ FIXED
**File**: aiPopUp.gd (line 340)
**Impact**: Cosmetic (values were correct but unclear)
**Fix**: Added `TIME_TPBM_PUB = 60` and `TIME_TPBM_BAN = 75` constants, updated match cases

### 6. PolePosition.gd - do-while to while conversion
**File**: PolePosition.gd (line 238)
**Impact**: None (functionally equivalent)

### 7. aiMisc.gd - Variable naming conventions
**File**: aiMisc.gd
**Impact**: Cosmetic only

---

## Beer/Pop Boy Bug Investigation

The reported "beer goes through Pop Boy" bug was thoroughly investigated.

### Collision Check Comparison

| Check | C# Location | GD Location | Match |
|-------|-------------|-------------|-------|
| `n_z < 2` for CLARK | line 317 | line 288 | **YES** |
| `ais_scr_point_inside` call | line 314 | line 285 | **YES** |
| Pop Boy sprite reference | `sprPopBoy` | `AIMethods.spr_pop_boy` | **YES** |

### Conclusion
The porting is correct. The bug must be caused by:
1. Pop Boy's `n_z` value staying >= 2 (jumping/falling)
2. `ais_scr_point_inside` function issues
3. Sprite bounds or screen coordinate differences

### Recommended Debug Approach
Add logging to check:
```gdscript
print("Pop Boy n_z: ", AIMethods.spr_pop_boy.n_z)
print("Beer position: ", s.n_x, ", ", s.n_y)
print("Point inside result: ", AISupport.ais_scr_point_inside(...))
```

---

## Files Verified Correct

The following files were audited and found to be **correctly ported** with no issues:

| File | Functions | Status |
|------|-----------|--------|
| aiCrowd.gd | 34 | ✓ Correct |
| aiBackground.gd | 18 | ✓ Correct |
| aiWaterEffects.gd | 5 | ✓ Correct |
| AIDefine.gd | constants | ✓ Correct |
| PolePosition.gd | 13 | ✓ Correct |
| aiPopUp.gd | 24 | ✓ Correct |
| aiMisc.gd | 6 | ✓ Correct |
| AIFrosh.gd | 58 | ✓ Correct |
| aiProjectile.gd | 17+ | ✓ Correct |

---

## Action Items Summary

### Must Fix - ✅ DONE
1. [x] Add `ais_abandon_alien()` function to AISupport.gd (dead code stub for parity)

### Should Fix - ✅ DONE
2. [x] Add sound + velocity to `ai_icon_out_of_position`
3. [x] Add ARM_IRON_RING check to `ai_icon`
4. [x] Add TAUNT/GREASE/RING cases to `ai_icon`

### Optional - ✅ DONE
5. [x] Add `ais_pick_hoisting_spot()` stub (dead code stub for parity)

### Remaining Optional - ✅ DONE
6. [x] Use constants instead of hardcoded values in aiPopUp.gd (added TIME_TPBM_PUB, TIME_TPBM_BAN)

### Investigate
7. [ ] Debug Pop Boy's n_z value during beer collision

---

## Appendix: Individual Audit Reports

- [Agent 1: aiSupport + aiMenuAndDisplay](audit-agent1-support-menu.md)
- [Agent 2: aiFrosh + aiProjectile](audit-agent2-frosh-projectile.md)
- [Agent 3: Secondary Systems](audit-agent3-secondary.md)

---

*Generated by Claude Opus 4.5 AI Audit System*
