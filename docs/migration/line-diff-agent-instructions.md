# Agent Task: True Line-by-Line Function Diff

## Context

A previous audit compared C# AI code to GDScript ports and missed a critical bug because it verified **structural parity** (functions exist, cases exist) but not **semantic parity** (every logical statement within those structures).

**The missed bug:** In `ai_icon` case 1, a 4-line inventory-zero check was missing. The case existed, had code, but was missing one critical runtime check. This caused icons to stay on screen when inventory hit zero.

**Why it was missed:** The audit checked:
- "Does case 1 exist?" → Yes
- "Does it have highlighting code?" → Yes
- "Does it have click handling?" → Yes
- ✓ Marked as correct

**What it should have checked:** "Does EVERY if-block in C# case 1 have a corresponding if-block in GD case 1?"

## Your Task

Perform a TRUE line-by-line comparison of each function, logging every C# statement that lacks a GDScript equivalent. Output findings to `docs/migration/line-diff-report-claude.md`.

---

## Files to Compare

### AI Logic Files

| Priority | C# Source | GDScript Target | Line Delta |
|----------|-----------|-----------------|------------|
| 1 | `GodotLegend/Core/AI/aiSupport.cs` | `GodotLegendGD/Core/AI/AISupport.gd` | -680 |
| 2 | `GodotLegend/Core/AI/aiMenuAndDisplay.cs` | `GodotLegendGD/Core/AI/aiMenuAndDisplay.gd` | -575 |
| 3 | `GodotLegend/Core/AI/aiFrosh.cs` | `GodotLegendGD/Core/AI/AIFrosh.gd` | -329 |
| 4 | `GodotLegend/Core/AI/aiProjectile.cs` | `GodotLegendGD/Core/AI/aiProjectile.gd` | -253 |
| 5 | `GodotLegend/Core/AI/aiPopUp.cs` | `GodotLegendGD/Core/AI/aiPopUp.gd` | -162 |
| 6 | `GodotLegend/Core/AI/aiCrowd.cs` | `GodotLegendGD/Core/AI/aiCrowd.gd` | -69 |
| 7 | `GodotLegend/Core/AI/aiBackground.cs` | `GodotLegendGD/Core/AI/aiBackground.gd` | -40 |
| 8 | `GodotLegend/Core/AI/poleposn.cs` | `GodotLegendGD/Core/AI/PolePosition.gd` | -19 |
| 9 | `GodotLegend/Core/AI/AIDefine.cs` | `GodotLegendGD/Core/AI/AIDefine.gd` | +9 |
| 10 | `GodotLegend/Core/AI/aiWaterEffects.cs` | `GodotLegendGD/Core/AI/aiWaterEffects.gd` | +5 |
| 11 | `GodotLegend/Core/AI/aiMisc.cs` | `GodotLegendGD/Core/AI/aiMisc.gd` | +83 |

### Enumerations (CRITICAL - integer values must match exactly)

| C# Source | GDScript Target | Notes |
|-----------|-----------------|-------|
| `GodotLegend/Core/AI/aiEnums.cs` | `GodotLegendGD/Core/Enums.gd` | All AI enums consolidated |
| `GodotLegend/Core/GameBitmapEnumeration.cs` | `GodotLegendGD/Core/Enums.gd` | Bitmap indices |

### Initialization Files (CRITICAL - sprite setup must be identical)

| C# Source | GDScript Target | Notes |
|-----------|-----------------|-------|
| `GodotLegend/Core/Sprite/SpriteInit.cs` | `GodotLegendGD/Core/Sprite/SpriteInit.gd` | Sprite creation/attribute setup |
| `GodotLegend/Core/AI/poleposn.cs` | `GodotLegendGD/Core/AI/PolePosition.gd` | Pole chain initialization |

**Priority order:** Start with files that have the largest negative line delta - these are most likely to have missing code.

---

## Methodology

### Step 1: Identify Function Pairs

For each C# function, find its GDScript equivalent using naming convention:
- C# `aiSomething` → GD `ai_something`
- C# `aisHelperName` → GD `ais_helper_name`
- C# `AISomething` → GD `ai_something` (drop caps)

### Step 2: Create Statement Inventory

For each function, list every **semantic statement** in the C# version:

**COUNT these (semantic statements):**
```csharp
if (condition)                    // Condition check
x = 5;                            // Assignment
DoSomething();                    // Method call
return value;                     // Return statement
for (i = 0; i < n; i++)          // Loop with bounds
while (condition)                 // While condition
switch (x)                        // Switch entry
case VALUE:                       // Each case label
break;                            // Break (if it affects flow)
s.nAttrib[X] = Y;                // State mutation
sSound[X].Play();                // Sound call
```

**DON'T COUNT these (structural/syntactic):**
```csharp
{                                 // Opening brace
}                                 // Closing brace
public static void Foo()          // Signature (check separately)
int x;                            // Declaration without assignment
// comment                        // Comments
```

### Step 3: Match Each Statement

For each C# semantic statement, find its GDScript equivalent:

| Status | Meaning |
|--------|---------|
| ✓ PRESENT | Equivalent statement exists in GD |
| ⚠️ DIFFERENT | Statement exists but logic differs |
| ❌ MISSING | No equivalent found in GD |

### Step 4: Flag Anomalies

**Automatic flags:**
- Function has ❌ MISSING statements → Flag as potential bug
- C# function has N statements, GD has < N-3 → Flag for review
- Any `sSound[].Play()` missing → HIGH priority
- Any `nAttrib[]` assignment missing → HIGH priority
- Any `if` condition missing → HIGH priority

---

## Output Format

Write all findings to: `docs/migration/line-diff-report.md`

Use this format:

```markdown
# Line-by-Line Diff Report

**Generated**: [date]
**Methodology**: Statement-level semantic comparison

## Summary

| File | Functions | Statements Checked | Missing | Different | Status |
|------|-----------|-------------------|---------|-----------|--------|
| aiMenuAndDisplay | 45 | 892 | 3 | 1 | ⚠️ REVIEW |
| aiSupport | 38 | 654 | 0 | 0 | ✓ OK |
| ... | ... | ... | ... | ... | ... |

**Total issues found**: X MISSING, Y DIFFERENT

---

## Detailed Findings

### aiMenuAndDisplay.cs → aiMenuAndDisplay.gd

#### ai_icon (C# lines 918-1017 → GD lines 456-527)

**Statement inventory:**

| # | C# Statement (line) | GD Equivalent (line) | Status |
|---|---------------------|---------------------|--------|
| 1 | `switch (s.nAttrib[attrIconStatus])` (919) | `match s.n_attrib[...]` (457) | ✓ |
| 2 | `case 0:` (921) | `0:` (458) | ✓ |
| 3 | `s.nvX = 5` (925) | `s.nv_x = 5` (462) | ✓ |
| ... | ... | ... | ... |
| 15 | `if (0 == aisGetPointsBasedOn(...))` (970) | - | ❌ MISSING |
| 16 | `sSound[ICONOUT].Play(...)` (972) | - | ❌ MISSING |
| 17 | `s.nAttrib[attrIconStatus] = 2` (973) | - | ❌ MISSING |

**Summary**: 47 statements in C#, 44 in GD
**Missing**: 3 statements (lines 970-973)
**Risk**: HIGH - Missing runtime inventory check causes icons to stay visible when empty

---

#### ai_something_else (C# lines X-Y → GD lines A-B)

[repeat for each function with issues]

---

### aiSupport.cs → AISupport.gd

[All functions verified ✓ - no missing statements]

---

## Issue Summary

### HIGH Priority (affects gameplay)
1. **ai_icon**: Missing inventory-zero check (FIXED in previous audit)
2. [any new findings]

### MEDIUM Priority (affects audio/visual)
1. [findings]

### LOW Priority (cosmetic/defensive)
1. [findings]
```

---

## Special Attention Areas

### 1. Switch/Match Statements
The most common source of missing code. For EVERY switch in C#:
- List all case labels
- Verify each case exists in GD match
- Verify each case has ALL statements (not just "has some code")

### 2. Nested Conditions
Easy to miss inner `if` blocks:
```csharp
if (outer) {
    DoA();
    if (inner) {    // <-- Easy to miss this
        DoB();
    }
    DoC();
}
```

### 3. Sound Calls
Every `sSound[X].Play()` or `sLoop[X].Play()` must have equivalent. Missing sounds are noticeable bugs.

### 4. State Transitions
Every `s.nAttrib[X] = Y` or `s.bAttrib[X] = Y` assignment must exist. Missing state changes cause logic bugs.

### 5. Early Returns
Check that `return` statements aren't lost - they affect control flow.

### 6. Compound Conditions
```csharp
if (A && B && C)  // All three conditions must be in GD
```

---

## Enumeration Verification (CRITICAL)

Enumerations define integer constants used throughout the codebase. A single wrong value can cause sprites to use wrong frames, sounds to play incorrectly, or state machines to malfunction.

### Methodology

For each enum in C#:

1. **Locate the GDScript equivalent** in `Enums.gd`
2. **Compare every value** - the integer assigned must be identical
3. **Check naming** - names should map correctly (PascalCase → SCREAMING_SNAKE_CASE)

### What to Check

```csharp
// C# enum
public enum SpriteType {
    sprFROSH = 0,      // Must be 0 in GD
    sprAPPLE = 1,      // Must be 1 in GD
    sprPIZZA = 2,      // Must be 2 in GD
    // ...
}
```

```gdscript
# GDScript equivalent
enum SpriteType {
    SPR_FROSH = 0,
    SPR_APPLE = 1,
    SPR_PIZZA = 2,
    # ...
}
```

### Critical Enums to Verify

| C# Enum | GDScript Enum | Why Critical |
|---------|---------------|--------------|
| `SpriteType` | `SpriteType` | Sprite creation |
| `GameBitmapEnumeration` | `GameBitmapEnumeration` | Frame indices |
| `Goals` | `Goals` | Frosh AI behavior |
| `Personalities` | `Personalities` | Frosh AI behavior |
| `ArmPositions` | `ArmPositions` | Arm state machine |
| `ASSList` | `ASSList` | Sound indices |
| `ASLList` | `ASLList` | Loop sound indices |
| `nattrFrosh` | `NAttrFrosh` | Attribute array indices |
| `battrFrosh` | `BAttrFrosh` | Boolean attribute indices |
| `attrArm` | `AttrArm` | Arm attribute indices |
| `attrIcon` | `AttrIcon` | Icon attribute indices |

### Output Format for Enum Issues

```markdown
## Enumeration Verification

### SpriteType
| C# Name | C# Value | GD Name | GD Value | Status |
|---------|----------|---------|----------|--------|
| sprFROSH | 0 | SPR_FROSH | 0 | ✓ |
| sprAPPLE | 1 | SPR_APPLE | 1 | ✓ |
| sprPIZZA | 2 | SPR_PIZZA | 3 | ❌ MISMATCH |
```

---

## Initialization Verification (CRITICAL)

Initialization functions set up sprites with correct attributes, positions, velocities, and frames. Any difference here can cause subtle bugs that only manifest during specific gameplay scenarios.

### Key Initialization Areas

#### 1. SpriteInit.cs → SpriteInit.gd

For each `CreateSprite` case, verify:
- **Frame assignment**: Same bitmap enum value
- **Position**: Same x, y coordinates or calculation
- **Velocity**: Same vx, vy values
- **Attributes**: Every `nAttrib[]` and `bAttrib[]` assignment matches
- **AI function**: Correct function pointer assigned

```csharp
// C# pattern
case SpriteType.sprFROSH:
    s.nX = 100;
    s.nY = 200;
    s.SetFrame(frm[bmpFR1_1]);
    s.nAttrib[attrGoal] = goalTHINK;
    s.AI = aiFrosh1;
    break;
```

Must match exactly in GDScript.

#### 2. PolePosition Initialization

The pole chain is complex - verify:
- **Chain count**: Same number of chains
- **Position calculations**: Same x, y formulas
- **Parent/child relationships**: Same hierarchy
- **Adjacent chain links**: Same linking logic

#### 3. Frosh Init Functions (ai_init_1, ai_init_2, etc.)

Each frosh initialization function sets starting state:
- **Goal assignment**: Same goal enum
- **Personality**: Same personality enum
- **Position/velocity**: Same calculations
- **Frame selection**: Same bitmap indices

### Initialization Checklist

For each sprite type in SpriteInit:
- [ ] Frame matches (same bitmap enum)
- [ ] Position matches (same x, y or formula)
- [ ] Velocity matches (same vx, vy)
- [ ] All nAttrib assignments present and correct
- [ ] All bAttrib assignments present and correct
- [ ] AI function assignment correct
- [ ] Sound initialization (if any) matches

### Output Format for Init Issues

```markdown
## Initialization Verification

### SpriteInit - SPR_FROSH
| Property | C# Value | GD Value | Status |
|----------|----------|----------|--------|
| Frame | bmpFR1_1 | bmpFR1_1 | ✓ |
| nX | 100 | 100 | ✓ |
| nY | 200 | 250 | ❌ DIFFERENT |
| attrGoal | goalTHINK | GOAL_THINK | ✓ |
```

---

## Known Naming Translations

| C# Pattern | GDScript Pattern |
|------------|------------------|
| `ssFr` | `AIMethods.ss_fr` |
| `sprArm` | `AIMethods.spr_arm` |
| `sSound[X]` | `AIMethods.s_sound[X]` |
| `sLoop[X]` | `AIMethods.s_loop[X]` |
| `frm[X]` | `AIMethods.frm[X]` |
| `frmM[X]` | `AIMethods.frm_m[X]` |
| `R.Next(n)` | `AIMethods.R.randi() % n` |
| `nAttrib[(int)attr.X]` | `n_attrib[Enums.Attr.X]` |
| `bAttrib[(int)battr.X]` | `b_attrib[Enums.BAttr.X]` |

---

## Example: The Bug We Missed

**C# aiIcon case 1 (lines 938-1008):**
```csharp
case 1:  // In position
    // Highlight logic (lines 940-968) - PRESENT IN GD ✓

    // THIS BLOCK WAS MISSING IN GD:
    if (0 == aisGetPointsBasedOn(s.nAttrib[attrButtonType]))  // line 970
    {
        sSound[ssndEFFECTS_ICONOUT].Play(volFULL, panONX(s));  // line 972
        s.nAttrib[attrIconStatus] = 2;                          // line 973
    }

    // Click handling (lines 976-1006) - PRESENT IN GD ✓
    break;
```

**What previous audit saw:**
- Case 1 exists? ✓
- Has highlight code? ✓
- Has click handling? ✓
- Result: "Looks complete"

**What this audit must check:**
- Line 940: highlight match → ✓ found at GD line 470
- Line 941: condition check → ✓ found
- ...
- Line 970: `if (0 == aisGetPointsBasedOn(...))` → ❌ NOT FOUND
- Line 972: sound play → ❌ NOT FOUND
- Line 973: status assignment → ❌ NOT FOUND
- Line 976: click if → ✓ found
- Result: "3 statements MISSING"

---

## Completion Checklist

### For each AI logic file pair:
- [ ] All functions identified and paired
- [ ] Each function's statements inventoried
- [ ] Each statement matched or marked missing
- [ ] Anomalies flagged with risk level
- [ ] Summary table updated

### For enumerations:
- [ ] All enums in aiEnums.cs verified against Enums.gd
- [ ] All enums in GameBitmapEnumeration.cs verified against Enums.gd
- [ ] Every enum value matches exactly (integer values identical)
- [ ] Any mismatches flagged as CRITICAL

### For initialization:
- [ ] SpriteInit.cs CreateSprite cases compared to SpriteInit.gd
- [ ] Every sprite type's initialization verified:
  - [ ] Frame assignment matches
  - [ ] Position/velocity matches
  - [ ] All attribute assignments present
  - [ ] AI function assignment correct
- [ ] PolePosition initialization verified:
  - [ ] Chain count matches
  - [ ] Position calculations match
  - [ ] Parent/child relationships match

### Final deliverable:
- [ ] `docs/migration/line-diff-report-claude.md` complete
- [ ] All HIGH priority issues listed
- [ ] Total statement counts provided
- [ ] Enumeration verification section complete
- [ ] Initialization verification section complete
