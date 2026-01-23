# Sound Discrepancy Backlog

## Status Legend
- [ ] TODO
- [x] DONE
- [-] WON'T FIX (intentional difference or not a real discrepancy)

---

## Critical Issues

### 1. [x] Menu Music Loop System Missing (aiMenuAndDisplay.gd)
**Priority:** CRITICAL
**Status:** DONE

The entire menu music loop system was stubbed out. Implemented:

- [x] 1a. Title screen music loop (C# lines 71-78)
  - Added at aiMenuAndDisplay.gd lines 86-93
  - Check if LSND_MUSIC_TITLEINIT, SSND_MENU_TITLEREPEAT, LSND_MUSIC_SCOTLAND not playing
  - Call SSND_MENU_TITLEREPEAT.loop(VOL_MUSIC)
  - Call SSND_MENU_DECORATEREPEAT.stop()

- [x] 1b. Jacket selection music loop (C# lines 287-296)
  - Added at aiMenuAndDisplay.gd in ai_menu_glowing_pass_crest()
  - Check if LSND_MUSIC_SCOTLAND, SSND_MENU_DECORATEREPEAT, LSND_MUSIC_TITLEINIT not playing
  - Call SSND_MENU_DECORATEREPEAT.loop(VOL_MUSIC)
  - Call SSND_MENU_TITLEREPEAT.stop()

- [x] 1c. Options return music loop (C# lines 521-530)
  - Added at aiMenuAndDisplay.gd in ai_options_return()
  - Same logic as jacket selection music loop

- [x] 1d. Title music intro (LSND_MUSIC_TITLEINIT) play-once behavior
  - Added at GameLoop.gd lines 88-99
  - Uses `b_play_riff_only_once` flag to ensure intro plays only once per session
  - On first title load: plays LSND_MUSIC_TITLEINIT, sets flag to true
  - On subsequent returns: skips intro, aiMenuAndDisplay starts loop directly

---

## Minor Issues

### 2. [-] Volume Parameter Differences (aiMenuAndDisplay.gd)
**Status:** WON'T FIX - Not actual discrepancies

Verified that all volume values are equivalent:
- C# DefaultVolume = 100
- C# volFULL = 100
- C# volHOLLAR = 100
- GDScript VOL_FULL = 100
- GDScript VOL_HOLLAR = 100

The following are functionally equivalent:
- [-] 2a. Line 120: SSND_EFFECTS_ICONOUT - C# uses .Play() (default 100), GD uses VOL_FULL (100)
- [-] 2b. Line 158: SSND_EFFECTS_ICONIN - C# uses .Play() (default 100), GD uses VOL_FULL (100)
- [-] 2c. Line 615: LSND_HOSE_TAKE1 - C# uses .Play() (default 100), GD uses VOL_HOLLAR (100)

---

## Location Differences (No Action Required)

### 3. [-] ais_shout_discipline() Location
**Status:** WON'T FIX - Intentional refactoring
- C#: aiSupport.cs lines 545-574
- GDScript: aiProjectile.gd lines 1241-1263
- Both implementations are correct and equivalent

### 4. [-] POPBOY_BEER Sound Location
**Status:** WON'T FIX - Intentional refactoring
- C#: aiMenuAndDisplay.cs line 1483
- GDScript: AIFrosh.gd line 1552
- Both implementations are correct and equivalent

---

## Progress Log

| Date | Item | Status | Notes |
|------|------|--------|-------|
| 2026-01-22 | 1a. Title screen music loop | DONE | Added to ai_menu_start_button() |
| 2026-01-22 | 1b. Jacket selection music loop | DONE | Added to ai_menu_glowing_pass_crest() |
| 2026-01-22 | 1c. Options return music loop | DONE | Added to ai_options_return() |
| 2026-01-22 | 2a-c. Volume parameter differences | WON'T FIX | Verified all volumes are equivalent (100) |
| 2026-01-22 | 1d. Title music intro play-once | DONE | Added b_play_riff_only_once flag to GameLoop.gd |

---

## Summary

**Total Issues:** 7
**Fixed:** 4 (critical menu music loops + intro play-once)
**Won't Fix:** 3 (not real discrepancies or intentional)
**Remaining:** 0

All sound discrepancies have been addressed.
