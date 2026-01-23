# Sound Comparison Audit Report

## Summary
- **Total C# sound call lines:** 250
- **Total GDScript sound call lines:** 229
- **Matched:** ~215 (equivalent behavior)
- **Discrepancies:** 14 (detailed below)

---

## Critical Issues

### 1. MISSING: Menu Music Loop System (aiMenuAndDisplay)
**Severity: CRITICAL**

The entire menu music loop system is missing from GDScript.

**C# Implementation (aiMenuAndDisplay.cs):**
- Lines 71-78: Title screen music loop
```csharp
if (!(lSound[((int)ASLList.lsndMUSIC_TITLEINIT)].IsPlaying())
    && !(sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].IsPlaying())
    && !(lSound[((int)ASLList.lsndMUSIC_SCOTLAND)].IsPlaying())
    && !(s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)]))
{
    s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)] = true;
    sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].Loop(SoundbankInfo.volMUSIC);
    sSound[((int)ASSList.ssndMENU_DECORATEREPEAT)].Stop();
}
```

- Lines 287-296: Jacket selection screen music loop
```csharp
sSound[((int)ASSList.ssndMENU_DECORATEREPEAT)].Loop(SoundbankInfo.volMUSIC);
sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].Stop();
```

- Lines 521-530: Options return music loop

**GDScript Implementation (aiMenuAndDisplay.gd):**
- Lines 86-87: **STUBBED OUT**
```gdscript
# Music handling - check if title music should loop
# Stubbed for now - needs sound service implementation
```

**Missing Sounds:**
| Sound | Action | Volume |
|-------|--------|--------|
| SSND_MENU_TITLEREPEAT | .Loop() | VOL_MUSIC |
| SSND_MENU_DECORATEREPEAT | .Loop() | VOL_MUSIC |
| SSND_MENU_TITLEREPEAT | .Stop() | - |
| SSND_MENU_DECORATEREPEAT | .Stop() | - |
| LSND_MUSIC_TITLEINIT | .IsPlaying() check | - |
| LSND_MUSIC_SCOTLAND | .IsPlaying() check | - |

---

## File-by-File Analysis

### aiProjectile.cs ↔ aiProjectile.gd

#### C# Sound Calls (78 total active lines)
| Line | Sound | Volume | Pan | Condition |
|------|-------|--------|-----|-----------|
| 126-127 | lsndEXAM_TOSS1 | volHOLLAR | panONX(s) | if !IsPlaying() |
| 187 | lsndFROSH_APPLEHIT2 | volHOLLAR | panCENTER | - |
| 255-269 | lsndPOPBOY_APPLE1/APPLER1 | volHOLLAR | panONX(s) | is_ritual() check |
| 295-299 | lsndPOPBOY_PIZZA1/2 | volHOLLAR | panONX(s) | if !IsPlaying() |
| 325-328 | lsndPOPBOY_BEER1/BEERR2/BEERR3 | - | - | IsPlaying() check only |
| 366-372 | lsndPOPBOY_EXAM1/2/3 | volHOLLAR | panONX(s) | if !IsPlaying() |
| 408 | lsndFRECS_HITAPPLE1 | volHOLLAR | (sprFrecsL.nX-320)/32 | random |
| 421-422 | ssndEFFECTS_CROWDROAR1 | volCROWD | (sprFrecsL.nX-320)/32 | if !IsPlaying() |
| 427-428 | lsndFRECS_CHEER1 | volHOLLAR | (sprFrecsL.nX-320)/32 | if !IsPlaying() |
| 440-441 | lsndFRECS_HITEXAM1 | volHOLLAR | (sprFrecsL.nX-320)/32 | random |
| 549-583 | lsndPREZ_HITAPPLE1-5 | volHOLLAR | (sprPrez.nX-320)/32 | sequential hit progression |
| 601-619 | lsndPREZ_HITCLARK1-3 | volHOLLAR | (sprPrez.nX-320)/32 | sequential hit progression |
| 633-643 | lsndPREZ_HITPIZZA1/2/R2 | volHOLLAR | (sprPrez.nX-320)/32 | is_ritual() check |
| 657-660 | lsndPREZ_HITHOSE1-4 | volHOLLAR | (sprPrez.nX-320)/32 | sequential with ritual variants |
| 668-669 | lsndPREZ_HITEXAM | volHOLLAR | (sprPrez.nX-320)/32 | - |
| 1400 | ssndWATER_HOSE | volNORMAL | - | .Loop() |
| 1424 | ssndWATER_HOSE | - | - | .Stop() |
| 1438 | ssndEFFECTS_CROWDMURMUR | - | - | .Stop() |
| 1472 | ssndEFFECTS_CROWDMURMUR | volCROWD | - | .Loop() |
| 1510-1513 | lsndRING_ZAP1/2/3 | volHOLLAR | panONX(s) | Stop all, play random |
| 1573 | ssndWATER_HOSE | - | - | .Stop() |

#### GDScript Sound Calls (66 total active lines)
All sounds properly matched with equivalent:
- Enum names translated (lsndXXX → LSND_XXX)
- Volume constants translated (volHOLLAR → VOL_HOLLAR)
- Pan functions translated (panONX(s) → AICrowd.pan_on_x(s) or explicit (n_x-320)/32)

#### Discrepancies: NONE

---

### aiMenuAndDisplay.cs ↔ aiMenuAndDisplay.gd

#### C# Sound Calls (66 total active lines)
| Line | Sound | Volume | Pan | Condition |
|------|-------|--------|-----|-----------|
| 40/42 | lsndNARRATOR_STARTDELAY1/2 | volHOLLAR | - | random |
| 62 | ssndMENU_SELECT | volFULL | - | - |
| 71-78 | **ssndMENU_TITLEREPEAT** | **volMUSIC** | - | **.Loop()** |
| 77-78 | **ssndMENU_DECORATEREPEAT** | - | - | **.Stop()** |
| 101 | ssndMENU_TOGGLE | volFULL | panONX(s) | - |
| 120 | ssndEFFECTS_ICONOUT | **default** | - | - |
| 158 | ssndEFFECTS_ICONIN | **default** | - | - |
| 237 | ssndMENU_TOGGLE | volFULL | panONX(s) | - |
| 252 | ssndMENU_SELECT | volMUSIC | panONX(s) | - |
| 259 | lsndNARRATOR_JACKETINIT | volHOLLAR | - | - |
| 287-295 | **ssndMENU_DECORATEREPEAT** | **volMUSIC** | - | **.Loop()** |
| 295 | **ssndMENU_TITLEREPEAT** | - | - | **.Stop()** |
| 341 | ssndMENU_TOGGLE | volFULL | panONX(s) | - |
| 351 | ssndMENU_DROP | volFULL | panONX(s) | - |
| 521-529 | **ssndMENU_DECORATEREPEAT** | **volMUSIC** | - | **.Loop()** |
| 528 | **ssndMENU_TITLEREPEAT** | - | - | **.Stop()** |
| 637-638 | lSound[attrAssociatedSound] | volHOLLAR | - | if !IsPlaying() |
| 882 | ssndEFFECTS_ICONIN | volFULL | panONX(s) | - |
| 899 | lsndRING_RISE | volHOLLAR | panONX(s) | - |
| 926/972 | ssndEFFECTS_ICONOUT | volFULL | panONX(s) | - |
| 993 | ssndEFFECTS_PIZZAREADY | volNORMAL | panONX(s) | - |
| 997 | ssndEFFECTS_POUR | volNORMAL | panONX(s) | - |
| 1043/1068 | ssndEFFECTS_BIGJACKETWHOOSH | volNORMAL/volFULL | - | - |
| 1057 | ssndEFFECTS_BIGJACKETSLAM | volFULL | - | - |
| 1262 | lsndHOSE_TAKE1 | **default** | - | random |
| 1284 | ssndWATER_HOSE | - | - | .Stop() |
| 1342 | ssndEFFECTS_ACHIEVEMENTUNLOCKED2 | volFULL | panONX(s) | - |
| 1378 | lsndPOPBOY_GREETING1/2 | volHOLLAR | panONX(s) | random |
| 1403-1404 | lsndPOPBOY_GREETING1/2 | - | - | IsPlaying() check |
| 1452-1453 | lsndPOPBOY_ADVICE1-6 | volHOLLAR | panONX(s) | - |
| 1474 | ssndEFFECTS_CHUG | volFULL | panONX(s) | - |
| 1478 | lsndFROSH_CLARKFINISH1 | volHOLLAR | panONX(s) | random |
| 1483 | lsndPOPBOY_BEER1 | volHOLLAR | panONX(s) | - |
| 1503 | lsndPOPBOY_CHEER1 | - | - | IsPlaying() check |
| 1509/1511 | lsndEXAM_TOSS1 | - | - | .Stop() |
| 1526 | lsndPOPBOY_CHEER1 | volCROWD | panONX(s) | - |
| 1538 | lsndPOPBOY_CHEER2 | volHOLLAR | panONX(s) | - |

#### GDScript Sound Calls (44 total active lines)
Most sounds matched, but **menu music loop system entirely missing**.

#### Discrepancies:
| Issue | C# Line | GDScript Line | Description |
|-------|---------|---------------|-------------|
| **MISSING** | 71-78 | 86-87 | SSND_MENU_TITLEREPEAT.Loop() - **STUBBED** |
| **MISSING** | 78 | - | SSND_MENU_DECORATEREPEAT.Stop() |
| **MISSING** | 287-295 | - | SSND_MENU_DECORATEREPEAT.Loop() |
| **MISSING** | 295 | - | SSND_MENU_TITLEREPEAT.Stop() |
| **MISSING** | 521-529 | - | SSND_MENU_DECORATEREPEAT.Loop() |
| **MISSING** | 528 | - | SSND_MENU_TITLEREPEAT.Stop() |
| **VOLUME** | 120 | 120 | C#: .Play() no params, GD: .play(VOL_FULL) |
| **VOLUME** | 158 | 158 | C#: .Play() no params, GD: .play(VOL_FULL) |
| **VOLUME** | 1262 | 591 | C#: .Play() no params, GD: .play(VOL_HOLLAR) |

---

### aiCrowd.cs ↔ AICrowd.gd

#### C# Sound Calls (19 total active lines)
| Line | Sound | Volume | Pan | Condition |
|------|-------|--------|-----|-----------|
| 40-41 | ssndEFFECTS_CROWDROAR1 | volCROWD | panONX(s) | if !IsPlaying() |
| 59-60 | ssndEFFECTS_CROWDROAR1 | volCROWD | panONX(s) | if !IsPlaying() |
| 83-84 | lsndFRECS_SLAM | volCROWD | - | if !IsPlaying() |
| 105-106 | lsndFRECS_SLAM | volCROWD | - | if !IsPlaying() |
| 132-133 | lsndFRECS_SLAM | volCROWD | - | if !IsPlaying() |
| 154-155 | lsndFRECS_SLAM | volCROWD | - | if !IsPlaying() |
| 249 | lsndFRECS_BOO1 | volCROWD | panONX(s) | R.Next(3) random |
| 261 | lsndFRECS_BOO1 | volCROWD | panONX(s) | R.Next(3) random |
| 287 | lsndFRECS_SLAM | volCROWD | - | - |
| 466-467 | ssndEFFECTS_CROWDROAR1 | volCROWD | panONX(s) | if !IsPlaying() |

#### GDScript Sound Calls (16 total active lines)
All matched correctly.

#### Discrepancies: NONE

---

### aiFrosh.cs ↔ AIFrosh.gd

#### C# Sound Calls (24 total active lines)
| Line | Sound | Volume | Pan | Condition |
|------|-------|--------|-----|-----------|
| 384-385 | ssndEFFECTS_PIZZAEAT | volNORMAL | panONX(s) | if !IsPlaying() |
| 422 | ssndEFFECTS_CHUG | volNORMAL | panONX(s) | - |
| 439 | ssndEFFECTS_CHUGLASTDROP | volNORMAL | panONX(s) | random |
| 447/449 | lsndFROSH_CLARKFINISH1/3 | volHOLLAR | panONX(s) | random |
| 542-543 | ssndEFFECTS_PUNCH | volNORMAL | panONX(s) | if !IsPlaying() |
| 713-714 | lsndFROSH_COW1 | volNORMAL | panONX(s) | if !IsPlaying() |
| 761-762 | lsndFROSH_SHEEP1 | volNORMAL | panONX(s) | if !IsPlaying() |
| 901 | lsndFROSH_ATTOP1 | volHOLLAR | panONX(s) | R.Next(5) random |
| 1445-1446 | ssndEFFECTS_PIZZAEAT | volNORMAL | panONX(s) | if !IsPlaying() |
| 1520 | ssndEFFECTS_CHUG | volNORMAL | panONX(s) | - |
| 1530 | ssndEFFECTS_CHUGLASTDROP | volNORMAL | panONX(s) | random |
| 1536/1538 | lsndFROSH_CLARKFINISH1/3 | volHOLLAR | panONX(s) | random |
| 1843 | lsndNARRATOR_CONGRATS | volHOLLAR | - | - |
| 1847 | lsndFROSH_GOTTAM1 | volHOLLAR | - | - |
| 1868 | lsndFROSH_GOTTAM2 | volHOLLAR | - | R.Next(2) random |
| 1875 | lsndPOPBOY_GOTTAM1 | volHOLLAR | - | - |

#### GDScript Sound Calls (25 total active lines)
All matched correctly. Note: Some sounds moved to different locations but all present.

**Note:** POPBOY_BEER sound is in aiMenuAndDisplay.cs (line 1483) but in AIFrosh.gd (line 1552). Both are present in their respective locations.

#### Discrepancies: NONE (location differences are intentional refactoring)

---

### aiSupport.cs ↔ aiSupport.gd

#### C# Sound Calls (13 total active lines)
| Line | Sound | Volume | Pan | Condition |
|------|-------|--------|-----|-----------|
| 574 | lSound[nThingToShout] | volHOLLAR | - | discipline sound |
| 840 | ssndEFFECTS_TOPPLE | volHOLLAR | - | - |
| 873-876 | lSound[i] loop | - | - | .Stop() many |
| 876 | lsndEXAM_TOSS1 | - | - | .Stop() |
| 878-879 | lSound[i] loop | - | - | .Stop() many |
| 881 | lsndPREZ_POPBOY1_1 | volHOLLAR | (sprPrez.nX-320)/32 | R.Next(3) |
| 887 | lsndPOPBOY_EXAM1/2/3 | - | - | IsPlaying() check |
| 896 | lsndEXAM_TOSS1 | - | - | IsPlaying() check |
| 903 | lSound[i] loop | - | - | .Stop() |
| 904 | lsndPREZ_ENCOURAGE1 | volHOLLAR | (sprPrez.nX-320)/32 | progression |
| 919 | lsndMUSIC_SCOTLAND | volFULL | - | - |

#### GDScript Sound Calls (12 total active lines)
All matched. Note: `ais_shout_discipline()` moved to aiProjectile.gd in GDScript.

#### Discrepancies:
| Issue | C# Line | GDScript Line | Description |
|-------|---------|---------------|-------------|
| **LOCATION** | 545-574 | aiProjectile:1241-1263 | aisShoutDiscipline moved to aiProjectile.gd |
| **PAN** | 574 | 1263 | C#: no pan param, GD: explicit 0 |
| **PAN** | 840 | 569 | C#: no pan param, GD: explicit 0 |

---

### aiPopUp.cs ↔ aiPopUp.gd

#### C# Sound Calls (40 total active lines)
| Line | Sound | Volume | Pan | Condition |
|------|-------|--------|-----|-----------|
| 25/55/86/119/139 | ssndEFFECTS_SNATCH1 | volNORMAL | panONX(s) | - |
| 200/202 | lsndARTSCI_MALE_HIT1 | volHOLLAR | panONX(s) | hit progression |
| 220 | lsndARTSCI_FEMALE_HIT1 | volHOLLAR | panONX(s) | hit progression |
| 236 | lsndCOMMIE_MALE_HIT1 | volHOLLAR | panONX(s) | hit progression |
| 252 | lsndCOMMIE_FEMALE_HIT1 | volHOLLAR | panONX(s) | hit progression |
| 273 | ssndEFFECTS_PUSH1 | volNORMAL | panONX(s) | - |
| 295-297 | lsndARTSCI_MALE_TAUNT/PUSH | volHOLLAR | panONX(s) | Stop+Play |
| 317-318 | lsndARTSCI_FEMALE_TAUNT/PUSH | volHOLLAR | panONX(s) | Stop+Play |
| 337-341 | lsndCOMMIE_MALE_TAUNT/PUSH | volHOLLAR | panONX(s) | Stop+Play |
| 361-362 | lsndCOMMIE_FEMALE_TAUNT/PUSH | volHOLLAR | panONX(s) | Stop+Play |
| 385-386 | lsndSCICONM_HitMisc | volHOLLAR | panONX(s) | Stop loop+Play |
| 394 | lsndFRECS_HITAPPLE2 | volHOLLAR | - | - |
| 407/409 | lsndFRECS_REWARDR1/BOO3 | volHOLLAR | - | is_ritual check |
| 422-423 | lsndSCICONF_HitMisc | volHOLLAR | panONX(s) | Stop loop+Play |
| 431 | lsndFRECS_HITAPPLE3 | volHOLLAR | - | - |
| 443/445 | lsndFRECS_REWARDR1/BOO3 | volHOLLAR | - | is_ritual check |
| 692 | ssndEFFECTS_SNATCH1 | volNORMAL | panONX(s) | - |
| 702-711 | lsndSCICONM/F_HitApples/Pizza/Beer/Exam | volHOLLAR | panONX(s) | male/female |
| 821 | lSound[nVoiceEffect] | volHOLLAR | panONX(s) | - |

#### GDScript Sound Calls (40 total active lines)
All matched correctly.

#### Discrepancies: NONE

---

### aiBackground.cs ↔ aiBackground.gd

#### C# Sound Calls (7 total active lines)
| Line | Sound | Volume | Pan | Condition |
|------|-------|--------|-----|-----------|
| 86 | lsndPOPBOY_HIPPOR1 | volHOLLAR | - | is_ritual() |
| 88 | lsndPOPBOY_HIPPO1 | volHOLLAR | - | !is_ritual() |
| 133 | ssndEFFECTS_KABOOM | volHOLLAR | - | - |
| 443 | lsndRING_SWING | volHOLLAR | panONX(s) | - |
| 449 | lsndRING_PRESS | volHOLLAR | panONX(s) | - |
| 451 | lsndRING_DING | volHOLLAR | panONX(s) | - |
| 454 | lsndRING_SWING | volNORMAL | panONX(s) | - |

#### GDScript Sound Calls (7 total active lines)
All matched correctly.

#### Discrepancies: NONE

---

### aiMisc.cs ↔ aiMisc.gd

#### C# Sound Calls (7 total active lines)
| Line | Sound | Volume | Pan | Condition |
|------|-------|--------|-----|-----------|
| 77 | lsndFRECS_REWARD5/6/R1 | - | - | IsPlaying() check |
| 80 | lsndFRECS_REWARD5 | volCROWD | randintin(-pan/2,pan/2) | - |
| 84 | lsndFRECS_PROGRESS2 | volCROWDSHOUT | randintin(-pan/2,pan/2) | is_ritual() affects range |
| 90 | lsndFRECS_CHANT1 | volCROWD | randintin(-pan/2,pan/2) | is_ritual() + random |
| 92 | lsndFRECS_CHANT1 | volCROWD | randintin(-pan/2,pan/2) | - |
| 95 | lsndFRECS_HOWHIGHTHEPOLE | volCROWDSHOUT | randintin(-pan/2,pan/2) | - |

#### GDScript Sound Calls (9 total active lines)
All matched correctly.

#### Discrepancies: NONE

---

## Verification Checklist Summary

| Check | Status |
|-------|--------|
| Same enum names (with convention differences) | ✅ PASS |
| Same volume constants | ⚠️ MINOR - some default param differences |
| Same pan calculations | ✅ PASS |
| Same conditional guards (is_playing checks) | ✅ PASS |
| Same random selection logic | ✅ PASS |
| Called in equivalent code locations | ⚠️ Some intentional refactoring |
| Hit sound progressions match | ✅ PASS |
| Ritual mode variants match | ✅ PASS |
| Sound stopping before playing | ✅ PASS |
| Loop vs Play usage | ❌ FAIL - Menu music loops missing |

---

## Recommendations

### Critical (Must Fix)
1. **Implement menu music loop system in aiMenuAndDisplay.gd**
   - Add SSND_MENU_TITLEREPEAT.loop() for title screen
   - Add SSND_MENU_DECORATEREPEAT.loop() for jacket selection
   - Add proper stop() calls when switching screens
   - Implement the IsPlaying() checks to prevent overlapping music

### Minor (Should Verify)
2. **Verify default volume behavior**
   - C# lines 120, 158 use .Play() with no parameters
   - GDScript uses .play(VOL_FULL)
   - Confirm if C# default matches VOL_FULL (1.0)

3. **Verify pan=0 equivalence**
   - Several GDScript calls explicitly pass 0 for pan
   - C# omits pan parameter (defaults to 0/center)
   - Functionally equivalent but worth verifying

### Notes (No Action Required)
4. **Code location differences are intentional:**
   - `ais_shout_discipline()` moved from aiSupport to aiProjectile in GDScript
   - POPBOY_BEER sound location differs but both are implemented
   - These are refactoring choices, not bugs

---

## Appendix: Sound Naming Convention Reference

| C# | GDScript |
|----|----------|
| lSound[((int)ASLList.lsndXXX)] | AIMethods.l_sound[Enums.ASLList.LSND_XXX] |
| sSound[((int)ASSList.ssndXXX)] | AIMethods.s_sound[Enums.ASSList.SSND_XXX] |
| .Play(volume, pan) | .play(volume, pan) |
| .Stop() | .stop() |
| .IsPlaying() | .is_playing() |
| .Loop(volume) | .loop(volume) |
| volHOLLAR | VOL_HOLLAR |
| volFULL | VOL_FULL |
| volCROWD | VOL_CROWD |
| volNORMAL | VOL_NORMAL |
| volMUSIC | VOL_MUSIC |
| volCROWDSHOUT | VOL_CROWD_SHOUT |
| panONX(s) | AICrowd.pan_on_x(s) or (sprite.n_x - 320) / 32 |
| R.Next(n) | AIMethods.R.randi() % n |

---

*Report generated: 2026-01-22*
*Audit performed by Claude Code*
