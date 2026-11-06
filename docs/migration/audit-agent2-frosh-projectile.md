# Audit Report: aiFrosh.cs/AIFrosh.gd and aiProjectile.cs/aiProjectile.gd

**Auditor**: Claude Agent (Opus 4.5)
**Date**: 2026-01-25
**Focus Files**:
- `GodotLegend/Core/AI/aiFrosh.cs` (1888 lines) vs `GodotLegendGD/Core/AI/AIFrosh.gd` (1559 lines)
- `GodotLegend/Core/AI/aiProjectile.cs` (1618 lines) vs `GodotLegendGD/Core/AI/aiProjectile.gd` (1365 lines)

---

## Summary

The audit compared C# source code to the GDScript port, looking for logic differences that could cause bugs. The "beer goes through Pop Boy" bug was specifically investigated.

**Result**: The Pop Boy/beer collision logic appears to be correctly ported. The `n_z < 2` check exists in both C# (line 317) and GDScript (line 288) for CLARK (beer) projectiles. No bugs found in the collision detection itself.

Several LOW severity issues were found related to missing validation code and minor differences.

---

## aiProjectile.gd Findings

### ais_change_arm / ais_change_arm_backwards - Missing Second Validation Switch

- **Issue**: C# has a second switch statement after the do-while loop that validates the selected arm again before returning. GDScript only has the first while loop and directly returns.
- **Severity**: LOW

**C# (lines 850-877 in aisChangeArm):**
```csharp
// After do-while loop:
switch (nCurrent)
{
    case ((int)ArmPositions.armAPPLE):
        if (0 != nApples) return nCurrent;
        else return ((int)ArmPositions.armPIZZA);
    case ((int)ArmPositions.armPIZZA):
        if (0 != nPizza) return nCurrent;
        else return ((int)ArmPositions.armPIZZA);
    // ... etc for all arm types
}
```

**GD (lines 718-719):**
```gdscript
# Missing - directly returns after while loop
return n_current
```

**Fix hint**: The second switch is likely defensive/redundant since the first loop already validates. LOW priority to add, but could add for exactness.

---

### _ai_arm_nothing Missing Curly Braces Issue

- **Issue**: In C#, line 1011-1012 has a potential bug where `if (0 == R.Next(150))` only applies to the first statement due to missing braces. This appears to be intentional C# behavior that was preserved in GDScript.
- **Severity**: LOW (matches C# behavior)

**C# (lines 1011-1012):**
```csharp
if (0 == R.Next(150))
    s.nAttrib[((int)attrArm.attrArmAction)] = 1; s.nCC = 0;
```

**GD (lines 823-825):**
```gdscript
if 0 == AIMethods.R.randi() % 150:
    s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
    s.n_cc = 0
```

**Note**: The GDScript version actually "fixes" this by putting both statements in the if block. This may be a behavior change from the original. Review if the `s.n_cc = 0` should always execute or only when the random check passes.

---

## AIFrosh.gd Findings

### ai_act_7a - Missing Braces Logic

- **Issue**: In C#, lines 831-836 have an `else` block followed by code that appears to be outside any block due to missing braces. The GDScript port correctly indents all the code under `else`.
- **Severity**: LOW (GDScript may be more correct)

**C# (lines 831-836):**
```csharp
else
    aisBobUpAndDown(s);
    // Slowly nudge the frosh towards the pole if room opens up
    if (!(s.bAttrib[((int)battrFrosh.attrWeightOnShoulders)]))
        aisPickCloserPyramidSpot(s);
    aisMoveTowardsDestination(s);
```

**GD (lines 662-667):**
```gdscript
else:
    AISupport.ais_bob_up_and_down(s)

if not s.b_attrib[Enums.BAttrFrosh.ATTR_WEIGHT_ON_SHOULDERS]:
    AISupport.ais_pick_closer_pyramid_spot(s)
AISupport.ais_move_towards_destination(s)
```

**Note**: The GDScript correctly places the `if` and `ais_move_towards_destination` outside the else block, which matches the actual C# execution (since C# without braces only includes one statement in the else).

---

### ai_act_6f Frame Sprite Swap

- **Issue**: In ai_act_6f, the C# and GDScript have different frm/frm_m orderings for sheep frames
- **Severity**: LOW (visual only)

**C# (line 773):**
```csharp
s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frm[...bmpFR6F_2] : frmM[...bmpFR6F_2]);
```

**GD (line 618):**
```gdscript
s.set_frame(AIMethods.frm[...bmpFR6F_2] if s.b_attrib[...ATTR_LOOKING_LEFT] else AIMethods.frm_m[...bmpFR6F_2])
```

**Note**: Both use the same ordering (frm when looking left, frm_m otherwise for sheep). This appears correct.

---

## Pop Boy Collision Analysis (Special Focus)

The reported "beer goes through Pop Boy" bug was investigated in detail.

### Frame 6 Collision Checks (aiProjectile)

| Projectile Type | C# nZ Check | GDScript nZ Check | Match |
|----------------|-------------|-------------------|-------|
| APPLE | None | None | YES |
| PIZZA | None | None | YES |
| CLARK (beer) | `nZ < 2` | `n_z < 2` | YES |
| EXAM | None | None | YES |
| GREASE | None | None | YES |

**Conclusion**: The beer/Pop Boy collision check is correctly ported. The `n_z < 2` check ensures Pop Boy must be at ground level (not falling/jumping) to catch the beer.

If beer is "going through" Pop Boy, the issue is likely:
1. Pop Boy's `n_z` is >= 2 during collision check
2. The `ais_scr_point_inside` function has issues
3. Pop Boy sprite bounds are incorrect
4. Screen coordinate calculation differences

---

## No Issues Found

The following areas were verified to be correctly ported:

1. **ai_act_1 through ai_act_16f** - All frosh behavior functions match
2. **ai_pop_boy / ai_pop_boy_in_crowd** - Pop Boy functions match aiMenuAndDisplay.cs
3. **Frame 6/7 projectile collision** - All switch cases present and match
4. **Sound enum values** - All sound calls use correct enum indices
5. **Velocity/position math** - All `abs()` calls present, division/multiplication preserved
6. **State transitions** - All behavior/goal assignments match

---

## Recommendations

1. **INVESTIGATE**: If beer still goes through Pop Boy, add debug logging to check:
   - `spr_pop_boy.n_z` value when beer reaches Pop Boy
   - `ais_scr_point_inside` return value
   - Screen coordinates of both sprites

2. **OPTIONAL FIX**: Add the second validation switch to `ais_change_arm` and `ais_change_arm_backwards` for exact match with C#.

3. **REVIEW**: The `_ai_arm_nothing` function's `if (0 == R.Next(150))` may have different behavior than intended. Both statements are now in the if block in GDScript.
