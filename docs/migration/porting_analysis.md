# Legend of the Greasepole - C# to GDScript Porting Analysis

This document provides a comprehensive function-by-function comparison of the C# (GodotLegend) and GDScript (GodotLegendGD) implementations. Each section identifies differences, missing functionality, and items to fix.

## Summary of Critical Issues

### High Priority
1. **Sound System**: Most sound calls in GDScript are commented out, stubbed, or incomplete
2. **Collision Response**: Some collision handling functions have incomplete implementations
3. **Achievement Unlocking**: Some achievement unlock calls may be missing or incomplete
4. **Speech/Voice System**: NOSPEECHFOR/SPEECHOK functions may not properly integrate

### Medium Priority
1. **SpriteSet Layer Reference**: GDScript uses layer_index int instead of Layer object reference
2. **Callable vs Delegate**: Behavior assignment uses Callable with string method names
3. **panONX Return Type**: C# returns int, GDScript returns float

---

## Core Root Files

### Globals.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | Service Factory pattern | Direct autoload access | Architectural change - working |
| ✅ | Globals.myGameConditions | Globals.myGameConditions | Same pattern |
| ✅ | Globals.myGameLoop | Globals.myGameLoop | Same pattern |
| ✅ | Globals.myLayers | Globals.myLayers | Same pattern |

### Enums.gd
| Status | Notes |
|--------|-------|
| ✅ | Full port with snake_case naming |
| ⚠️ | Verify all enum values match exactly - some may have different casing patterns |

---

## Core/Control Files

### GameConditions.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | GetPlayerApples() | get_player_apples() | Name convention change |
| ✅ | GetPlayerClark() | get_player_clark() | Name convention change |
| ✅ | GetPlayerPizza() | get_player_pizza() | Name convention change |
| ✅ | GetPlayerExam() | get_player_exam() | Name convention change |
| ✅ | GetApples(n) | get_apples(n) | Name convention change |
| ✅ | GetClarks(n) | get_clarks(n) | Name convention change |
| ✅ | GetPizzas(n) | get_pizzas(n) | Name convention change |
| ✅ | GetExams(n) | get_exams(n) | Name convention change |
| ✅ | LoseApple() | lose_apple() | Name convention change |
| ✅ | LosePizza() | lose_pizza() | Name convention change |
| ✅ | LoseClark() | lose_clark() | Name convention change |
| ✅ | LoseExam() | lose_exam() | Name convention change |
| ✅ | AddEnergy(n) | add_energy(n) | Name convention change |
| ✅ | GetFroshLameness() | get_frosh_lameness() | Name convention change |
| ✅ | IsRitual() | is_ritual() | Name convention change |
| ✅ | IsDemo() | is_demo() | Name convention change |
| ✅ | gbTriPubBan | gb_tri_pub_ban | Name convention change |

### GameLoop.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | ChangeGameState(int) | change_game_state(int) | Name convention change |
| ✅ | current_game_state | current_game_state | Same |
| ✅ | ss_menu | ss_menu | Same |

### Layers.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | GetOffset(int) | get_offset(int) | Name convention change |
| ✅ | ForceScroll(int) | force_scroll(int) | Name convention change |
| ✅ | 6 parallax layers | 6 parallax layers | Fully ported |

### SoundbankInfo.gd
| Status | Notes |
|--------|-------|
| ✅ | All constants ported |
| ✅ | References Enums.gd for values |
| ✅ | References AIDefine.gd for sprite counts |

---

## Core/Sprite Files

### TSprite.gd
| Status | C# Field/Method | GD Equivalent | Notes |
|--------|-----------------|---------------|-------|
| ✅ | nX, nY, nZ | n_x, n_y, n_z | World coordinates |
| ✅ | nScrx, nScry | n_scr_x, n_scr_y | Screen coordinates |
| ✅ | nvX, nvY, nvZ | nv_x, nv_y, nv_z | Velocities |
| ✅ | nDestX, nDestY | n_dest_x, n_dest_y | Destination |
| ✅ | nCC | n_cc | Cycle counter |
| ✅ | nAttrib[] | n_attrib[] | Integer attributes |
| ✅ | bAttrib[] | b_attrib[] | Boolean attributes |
| ✅ | frmFrame | frm_frame | Current frame |
| ✅ | bDeleted | b_deleted | Deletion flag |
| ✅ | SetFrame() | set_frame() | Frame setter |
| ✅ | SetBehavior(delegate) | set_behavior(Callable) | **Uses Callable instead of delegate** |
| ✅ | SwitchToSecondaryBehavior() | switch_to_secondary_behavior() | Behavior switching |
| ✅ | SetSecondaryBehavior() | set_secondary_behavior() | Secondary behavior |
| ✅ | CalculateScreenCoordinates() | calculate_screen_coordinates() | Coord calculation |
| ✅ | think() | think() | AI execution |
| ✅ | draw() | draw() | Rendering |
| ⚠️ | text | text | Verify SpriteTextType enum usage |

### FrameDesc.gd
| Status | C# Field | GD Equivalent | Notes |
|--------|----------|---------------|-------|
| ✅ | nHotSpotX, nHotSpotY | hotspot_x, hotspot_y | Hotspot position |
| ✅ | nX1, nX2, nZ1, nZ2 | n_x1, n_x2, n_z1, n_z2 | Bounding box |
| ✅ | nHeight, nWidth | n_height, n_width | Dimensions |
| ✅ | texture | texture | Godot Texture2D |

### SpriteSet.gd
| Status | C# Method | GD Equivalent | Notes |
|--------|-----------|---------------|-------|
| ✅ | Include() | include() | Add sprite |
| ✅ | Remove() | remove() | Remove sprite |
| ✅ | RemoveAll() | remove_all() | Clear all |
| ✅ | Flush() | flush() | Delete and compact |
| ✅ | Compact() | compact() | Remove deleted |
| ✅ | BubbleSortY() | bubble_sort_y() | Sort by Y |
| ✅ | BubbleSortX() | bubble_sort_x() | Sort by X |
| ✅ | OrderByY() | order_by_y() | Alias for sort |
| ✅ | SortByX() | sort_by_x() | Alias for sort |
| ✅ | Think() | think() | Update all sprites |
| ✅ | Draw() | draw() | Render all sprites |
| ✅ | GetNumberOfSprites() | get_number_of_sprites() | Count |
| ✅ | GetSprite(i) | get_sprite(i) | Index access |
| ⚠️ | Layer reference | my_layer_index | **Uses int index instead of Layer object** |
| ✅ | GetLayerY() | get_layer_y() | Dynamic offset lookup |
| ✅ | Bounding box queries | World/screen point queries | Fully ported |

### SpriteInit.gd
| Status | C# Method | GD Equivalent | Notes |
|--------|-----------|---------------|-------|
| ✅ | CreateSprite(type) | create_sprite(type) | Factory method |
| ✅ | CreateSprite(type,x,y) | create_sprite(type,x,y) | With position |
| ⚠️ | All sprite type initializations | Verify all types | Check all switch/match cases |

---

## Core/AI Files

### AIMethods.gd (Autoload - Shared State)
| Status | C# Member | GD Equivalent | Notes |
|--------|-----------|---------------|-------|
| ✅ | R (Random) | R (RandomNumberGenerator) | Same pattern |
| ✅ | frm[], frmM[] | frm[], frm_m[] | Frame arrays |
| ✅ | sSound[], lSound[] | s_sound[], l_sound[] | Sound arrays |
| ✅ | All spriteset refs | All spriteset refs | ss_pit, ss_fr, etc. |
| ✅ | All sprite refs | All sprite refs | spr_arm, spr_tam, etc. |
| ✅ | randintin(low,high) | randintin(low,high) | Random helper |
| ✅ | NOSPEECHFOR() | NOSPEECHFOR() | Speech control |
| ✅ | SPEECHOK() | SPEECHOK() | Speech check |
| ✅ | dSkyYToPitY() | d_sky_y_to_pit_y() | Layer conversion |
| ⚠️ | panONX(s) | pan_on_x(s) | **Returns float instead of int** |

### AIDefine.gd
| Status | C# Constant | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | dPOLEX | D_POLE_X | Pole X position |
| ✅ | dPOLEY | D_POLE_Y | Pole Y position |
| ✅ | dPITMINX | D_PIT_MIN_X | Pit boundary |
| ✅ | dPITMAXX | D_PIT_MAX_X | Pit boundary |
| ✅ | dPOLEWIDTH | D_POLE_WIDTH | Pole dimensions |
| ✅ | dPOLEHEIGHT | D_POLE_HEIGHT | Pole dimensions |
| ✅ | INCLUDEWHAP | INCLUDE_WHAP | Collision flag |
| ✅ | NOWHAP | NO_WHAP | Collision flag |
| ✅ | POLEACTSASSHIELD | POLE_ACTS_AS_SHIELD | Collision flag |
| ✅ | All NSPR_* constants | All NSPR_* constants | Sprite counts |
| ✅ | TIME_* constants | TIME_* constants | Timing constants |
| ✅ | GRAVITY | GRAVITY | Physics constant |

### aiBackground.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | aiCloud() | ai_cloud() | Cloud animation |
| ✅ | aiTree() | ai_tree() | Tree behavior |
| ✅ | aiGWBalloon() | ai_gw_balloon() | Balloon |
| ✅ | aiGWHippo() | ai_gw_hippo() | Hippo character |
| ⚠️ | Sound calls | Sound calls | **Check sound integration** |

### aiCrowd.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | aiFRECs() | ai_frecs() | FREC crowd AI |
| ✅ | aisSetFrecAction() | ais_set_frec_action() | Crowd action setter |
| ✅ | panONX() | pan_on_x() | Pan calculation |
| ✅ | aiPole() | ai_pole() | Pole behavior |
| ✅ | aiWater() | ai_water() | Water surface |
| ⚠️ | Energy-based crowd actions | Verify energy thresholds | Check ENERGY_* constants |

### aiFrosh.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | aiFrosh() | ai_frosh() | Main frosh dispatcher |
| ✅ | aiInit1() - aiInit11*() | ai_init_1() - ai_init_11*() | State initializers |
| ✅ | aiAct1() - aiAct11*() | ai_act_1() - ai_act_11*() | State behaviors |
| ✅ | aiInit4() | ai_init_4() | Walking state |
| ✅ | aiInit5A(), aiInit5B() | ai_init_5a(), ai_init_5b() | Pizza/Clark eating |
| ✅ | aiInit11D(), aiInit11E() | ai_init_11d(), ai_init_11e() | Special states |
| ⚠️ | PolePosition integration | Verify pyramid climbing | Check ais_pick_pyramid_spot |
| ⚠️ | Sound effects | Sound effects | **Many sound calls may be stubbed** |

### aiMenuAndDisplay.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | aiMenuStartButton() | ai_menu_start_button() | Menu button |
| ✅ | aiToggleButton() | ai_toggle_button() | Toggle button |
| ✅ | aiIcon() | ai_icon() | Icon display |
| ✅ | aiMeter() | ai_meter() | Meter display |
| ✅ | aiBar() | ai_bar() | Bar display |
| ✅ | aiSlamJacket() | ai_slam_jacket() | Jacket slam effect |
| ✅ | aiAchievementUnlocked*() | ai_achievement_unlocked*() | Achievement notices |
| ✅ | aiFlyInAndOut() | ai_fly_in_and_out() | Animation helper |
| ⚠️ | Some UI sound effects | Verify sound calls | Check button click sounds |

### aiMisc.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | aiInanimate() | ai_inanimate() | Do-nothing behavior |
| ✅ | aiDeleteMe() | ai_delete_me() | Self-deletion |
| ✅ | aiShowCurrentAchievementScreen() | ai_show_current_achievement_screen() | Achievement display |
| ✅ | aisUnlockAchievement() | ais_unlock_achievement() | Achievement unlock |
| ✅ | aiTam() | ai_tam() | Tam behavior |
| ✅ | aisRandomCrowdNoise() | ais_random_crowd_noise() | Crowd sounds |
| ✅ | aiRandomEventGenerator() | ai_random_event_generator() | Random events |
| ✅ | aisRandomEvents() | ais_random_events() | **STUB - needs implementation?** |

### aiPopUp.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | aisCheckForGeneralistAchievement() | ais_check_for_generalist_achievement() | Achievement check |
| ✅ | aiGetApples() | ai_get_apples() | Pickup behavior |
| ✅ | aiGetPizza() | ai_get_pizza() | Pickup behavior |
| ✅ | aiGetClark() | ai_get_clark() | Pickup behavior |
| ✅ | aiGetExam() | ai_get_exam() | Pickup behavior |
| ✅ | aiGetHose() | ai_get_hose() | Pickup behavior |
| ✅ | aisAlienInPit() | ais_alien_in_pit() | Alien animation |
| ✅ | aiArtSciMInPit() | ai_artsci_m_in_pit() | ArtSci male |
| ✅ | aiArtSciFInPit() | ai_artsci_f_in_pit() | ArtSci female |
| ✅ | aiCommieMInPit() | ai_commie_m_in_pit() | Commie male |
| ✅ | aiCommieFInPit() | ai_commie_f_in_pit() | Commie female |
| ✅ | aisPushIntoPit() | ais_push_into_pit() | Push into pit |
| ✅ | aiPushArtSciMIntoPit() | ai_push_artsci_m_into_pit() | Push ArtSci male |
| ✅ | aiPushArtSciFIntoPit() | ai_push_artsci_f_into_pit() | Push ArtSci female |
| ✅ | aiPushCommieMIntoPit() | ai_push_commie_m_into_pit() | Push Commie male |
| ✅ | aiPushCommieFIntoPit() | ai_push_commie_f_into_pit() | Push Commie female |
| ✅ | aiPushSciConM() | ai_push_scicon_m() | SciCon male |
| ✅ | aiPushSciConF() | ai_push_scicon_f() | SciCon female |
| ✅ | aiInitForeGroundEntry() | ai_init_foreground_entry() | Entry init |
| ✅ | aiForeGroundEntry() | ai_foreground_entry() | Entry behavior |
| ✅ | aiInitForegroundOnScreen() | ai_init_foreground_on_screen() | On-screen init |
| ✅ | aiForegroundOnScreen() | ai_foreground_on_screen() | On-screen behavior |
| ✅ | aiInitSciConForegroundOnScreen() | ai_init_scicon_foreground_on_screen() | SciCon on-screen |
| ✅ | aiSciConForegroundOnScreen() | ai_scicon_foreground_on_screen() | SciCon behavior |
| ✅ | aiInitForeGroundExit() | ai_init_foreground_exit() | Exit init |
| ✅ | aiForeGroundExit() | ai_foreground_exit() | Exit behavior |
| ✅ | aiInitPopUp() | ai_init_popup() | Popup init |
| ⚠️ | Sound effects throughout | Verify sound calls | **Many sound calls** |

### aiProjectile.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | aisProjectileRebound() | ais_projectile_rebound() | Rebound behavior |
| ✅ | aisCreateHoseWhap() | ais_create_hose_whap() | Hose splash effect |
| ✅ | aisRunAwayFrom() | ais_run_away_from() | Frosh scatter |
| ✅ | aiProjectile() | ai_projectile() | Main projectile AI |
| ✅ | aiArm() | ai_arm() | Player arm control |
| ✅ | aisChangeArm() | ais_change_arm() | Weapon switching |
| ✅ | aisChangeArmBackwards() | ais_change_arm_backwards() | Reverse switch |
| ✅ | aisDemoMouseHasMoved() | ais_demo_mouse_has_moved() | Demo detection |
| ✅ | aiWhap() | ai_whap() | Whap effect |
| ✅ | aiWordBubble() | ai_word_bubble() | Word bubble |
| ✅ | aisShoutDiscipline() | ais_shout_discipline() | **STUB** |
| ✅ | aiArmRing1,2,3() | ai_arm_ring_1,2,3() | Ring animations |
| ✅ | aiCloseUpBeer() | ai_close_up_beer() | Beer drinking |
| ✅ | aiFloatingProjectile() | ai_floating_projectile() | Floating items |
| ✅ | aisCollisionToResponse() | ais_collision_to_response() | Collision helper |
| ✅ | aisSpecialCollisionToResponse() | ais_special_collision_to_response() | Special collision |
| ⚠️ | aisChasePizza() | ais_chase_pizza() | **STUB - needs implementation** |
| ⚠️ | aisChaseClark() | ais_chase_clark() | **STUB - needs implementation** |
| ❌ | Sound effects | Sound effects | **Many sound calls are TODOs** |

### aiSupport.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | aisMouseOver() | ais_mouse_over() | Mouse detection |
| ✅ | aisScrPointInside() | ais_scr_point_inside() | Point-in-sprite test |
| ✅ | aisGetTargetsInScrRange() | ais_get_targets_in_scr_range() | Screen range query |
| ✅ | aisGetSpritesInRange() | ais_get_sprites_in_range() | World range query |
| ✅ | aisSendFroshFlying() | ais_send_frosh_flying() | Frosh state change |
| ✅ | aisSendFroshReallyFlying() | ais_send_frosh_really_flying() | Strong state change |
| ✅ | aisPickPyramidSpot() | ais_pick_pyramid_spot() | Position selection |
| ✅ | aisBobUpAndDown() | ais_bob_up_and_down() | Float animation |
| ✅ | aisPlummet() | ais_plummet() | Gravity fall |
| ✅ | aisForgeTrick() | ais_forge_trick() | Forge trick system |
| ✅ | aisUnlockAchievement() | ais_unlock_achievement() | Achievement unlock |
| ✅ | aisSetFrecAction() | ais_set_frec_action() | FREC action |
| ✅ | aisChaseAlien() | ais_chase_alien() | Chase behavior |
| ✅ | aisCreateProjectile() | ais_create_projectile() | Projectile factory |
| ✅ | aisIronRingZap() | ais_iron_ring_zap() | Ring attack |

### aiWaterEffects.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | aiSplashM() | ai_splash_m() | Medium splash |
| ✅ | aiSplashML() | ai_splash_ml() | Medium left splash |
| ✅ | aiSplashL() | ai_splash_l() | Large splash |
| ✅ | aiSplashS() | ai_splash_s() | Small splash |
| ✅ | aiRipple() | ai_ripple() | Ripple effect |

### AIFlyInAndOut.gd
| Status | C# Function | GD Equivalent | Notes |
|--------|-------------|---------------|-------|
| ✅ | aiFlyInAndOut() | ai_fly_in_and_out() | Fly animation |
| ✅ | aiInitFlyInAndOut() | ai_init_fly_in_and_out() | Init fly animation |
| ✅ | aiInitFlyInAndOut2() | ai_init_fly_in_and_out_2() | Variant |

### PolePosition.gd
| Status | C# Method | GD Equivalent | Notes |
|--------|-----------|---------------|-------|
| ✅ | GetX() | get_x() | X position |
| ✅ | GetY() | get_y() | Y position |
| ✅ | GetClaimer() | get_claimer() | Sprite claiming position |
| ✅ | PositionIsTaken() | position_is_taken() | State check |
| ✅ | PositionIsFree() | position_is_free() | State check |
| ✅ | GetParent() | get_parent() | Parent position |
| ✅ | GetChild() | get_child() | Child position |
| ✅ | AdjacentChain() | adjacent_chain() | Adjacent chain |
| ✅ | SetAdjacentChain() | set_adjacent_chain() | Set adjacent |
| ✅ | ReleaseClaim() | release_claim() | Release position |
| ✅ | SetClaim() | set_claim() | Claim position |
| ✅ | IsClaimed() | is_claimed() | Check claimed |
| ✅ | FirstTakenChild() | first_taken_child() | Chain traversal |
| ✅ | FirstFreeChild() | first_free_child() | Chain traversal |
| ✅ | LastTakenChild() | last_taken_child() | Chain traversal |
| ✅ | CalculateScreenPosition() | calculate_screen_position() | Coord calc |
| ✅ | CreateChild() | create_child() | Chain building |
| ✅ | Initialize_PolePositions() | initialize_pole_positions() | Static init |
| ✅ | nPolePositionData[] | n_pole_position_data[] | Position data |
| ✅ | PoleChains[] | pole_chains[] | Chain array |

---

## Backlog Items

### Critical (Must Fix)

1. **Sound System Integration** - IN PROGRESS
   - Files: All AI files
   - Issue: Many sound calls are commented out or have TODO markers
   - Action: Implement sound playback using Godot's AudioStreamPlayer
   - Status: Sound system infrastructure complete, individual calls being added

2. ~~**ais_chase_pizza() / ais_chase_clark() Stubs**~~ - DONE
   - File: [aiProjectile.gd:474-519](GodotLegendGD/Core/AI/aiProjectile.gd#L474-L519)
   - Fixed: Implemented frosh-chasing-item logic matching C#

3. ~~**ais_shout_discipline() Stub**~~ - DONE
   - File: [aiProjectile.gd:1133-1156](GodotLegendGD/Core/AI/aiProjectile.gd#L1133-L1156)
   - Fixed: Implemented discipline shout during iron ring ceremony

4. ~~**ais_random_events() Stub**~~ - DONE
   - File: [aiMisc.gd:213-268](GodotLegendGD/Core/AI/aiMisc.gd#L213-L268)
   - Fixed: Full implementation with pyramid stability checks
   - Also added: ais_regroup(), ais_topple_pyramid(), ais_think_for_al() in aiSupport.gd

### High Priority

5. ~~**panONX Return Type Mismatch**~~ - VERIFIED OK
   - Files: AICrowd.gd (pan_on_x returns int - correct)
   - Verified: AICrowd.pan_on_x() returns int matching C#, used throughout codebase
   - Note: AIMethods.pan_on_x() returning float is unused dead code

6. ~~**Verify All Achievement IDs**~~ - DONE
   - Fixed: Added missing achievement 13 in AIMenuAndDisplay.gd:903
   - Fixed: Added ai_pop_boy_in_crowd() with achievement 10 in AIFrosh.gd:1186-1232
   - All achievement IDs now match C# version

7. ~~**SpriteInit Type Coverage**~~ - VERIFIED OK
   - File: SpriteInit.gd
   - Verified: All SpriteType cases handled (count differences due to grouping style)

### Medium Priority

8. ~~**Callable String Method Names**~~ - DONE
   - Files: All AI behavior files
   - Fixed: Verified all 119 Callable references
   - Added missing ai_pop_boy() in AIFrosh.gd (SPR_POP_BOY sprite)
   - Added missing ai_ring() in AIMenuAndDisplay.gd (ring icon behavior)

9. ~~**SpriteSet Layer Index Pattern**~~ - VERIFIED OK
   - File: [SpriteSet.gd:8-22](GodotLegendGD/Core/Sprite/SpriteSet.gd#L8-L22)
   - Uses int index + get_layer_y() which calls Globals.myLayers.get_offset()
   - Functionally equivalent to C# Layer object approach
   - All SpriteSets correctly pass LayerNames enum values

10. ~~**Enum Value Verification**~~ - VERIFIED OK
    - Compared critical enums: SpriteType, GameStates, NAttrFrosh, BAttrFrosh, Goals, ASSList, ASLList, GameBitmapEnumeration
    - All values match C# in correct order
    - C# enums found across multiple files: aiEnums.cs, GameLoop.cs, GameBitmapEnumeration.cs, SoundbankInfo.cs

### Low Priority

11. **Code Comments/Documentation**
    - Issue: Some GDScript files lack detailed comments
    - Action: Add comments explaining complex logic

12. **Type Annotations**
    - Issue: Some functions lack return type annotations
    - Action: Add proper GDScript type hints

---

## Architecture Differences

### Delegate → Callable Pattern
```csharp
// C#
s.SetBehavior(aiProjectile);
```
```gdscript
# GDScript
s.set_behavior(Callable(AIProjectile, "ai_projectile"))
```

### Partial Class → Autoload + Static Classes
```csharp
// C# - Single partial class with all state and methods
public static partial class AIMethods { ... }
```
```gdscript
# GDScript - Split into Autoload (state) + Static classes (logic)
# AIMethods.gd - Autoload with shared state
# aiBackground.gd, aiCrowd.gd, etc. - Static classes with logic
```

### Service Factory → Direct Autoload Access
```csharp
// C#
Globals.myGameConditions.GetPlayerApples();
```
```gdscript
# GDScript (same pattern works)
Globals.myGameConditions.get_player_apples()
```

### Sound Arrays
```csharp
// C#
sSound[((int)ASSList.ssndEFFECTS_SNATCH1)].Play(vol, pan);
lSound[((int)ASLList.lsndFRECS_CHEER1)].Play(vol, pan);
```
```gdscript
# GDScript
AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_SNATCH1].play(vol, pan)
AIMethods.l_sound[Enums.ASLList.LSND_FRECS_CHEER1].play(vol, pan)
```

---

## Testing Checklist

- [ ] Menu navigation works
- [ ] Game starts properly
- [ ] Frosh spawn and move correctly
- [ ] Frosh climb the pole pyramid
- [ ] All weapons can be thrown (apple, pizza, clark, exam)
- [ ] Grease hose works
- [ ] Iron ring works
- [ ] Aliens (ArtSci, Commie) appear and can be pushed into pit
- [ ] SciCon triggers Tri Pub Ban
- [ ] Achievements unlock correctly
- [ ] Sound effects play (when implemented)
- [ ] Music plays
- [ ] Parallax scrolling works
- [ ] Crowd energy system works
- [ ] Random events spawn items
- [ ] GW Balloon and Hippo appear
- [ ] Demo mode works

---

*Generated: 2026-01-21*
*Comparison: GodotLegend (C#) vs GodotLegendGD (GDScript)*
