# Function Mapping: C# to GDScript

This document maps all functions from the C# source files (GodotLegend/Core/AI/) to their GDScript equivalents (GodotLegendGD/Core/AI/).

**Naming Convention**: C# uses PascalCase (e.g., `aiMenuStartButton`), GDScript uses snake_case (e.g., `ai_menu_start_button`)

## Summary Statistics

| Category | Count |
|----------|-------|
| **Total C# Functions** | 193 |
| **Total GDScript Functions** | 209 |
| **Mapped (C# -> GD)** | 186 |
| **Missing in GD** | 7 |
| **GD-Only Functions** | 23 |

---

## aiFrosh.cs -> AIFrosh.gd

| C# Function | C# Line | GD Function | GD Line | Status |
|-------------|---------|-------------|---------|--------|
| aiAct1 | 8 | ai_act_1 | 25 | MAPPED |
| aiInit1 | 29 | ai_init_1 | 43 | MAPPED |
| aiAct2 | 61 | ai_act_2 | 68 | MAPPED |
| aiInit2 | 67 | ai_init_2 | 72 | MAPPED |
| aiInit3 | 86 | ai_init_3 | 87 | MAPPED |
| aiAct3 | 103 | ai_act_3 | 99 | MAPPED |
| aiInit4 | 128 | ai_init_4 | 120 | MAPPED |
| aiAct4 | 161 | ai_act_4 | 144 | MAPPED |
| aiInit5A | 367 | ai_init_5a | 273 | MAPPED |
| aiAct5A | 377 | ai_act_5a | 284 | MAPPED |
| aiInit5B | 405 | ai_init_5b | 305 | MAPPED |
| aiAct5B | 416 | ai_act_5b | 316 | MAPPED |
| aiInit5C | 464 | ai_init_5c | 349 | MAPPED |
| aiAct5C | 478 | ai_act_5c | 361 | MAPPED |
| aiInit5D | 521 | ai_init_5d | 389 | MAPPED |
| aiAct5D | 535 | ai_act_5d | 401 | MAPPED |
| aiInit6A | 567 | ai_init_6a | 417 | MAPPED |
| aiAct6A | 577 | ai_act_6a | 430 | MAPPED |
| aiInit6B | 596 | ai_init_6b | 442 | MAPPED |
| aiAct6B | 607 | ai_act_6b | 455 | MAPPED |
| aiInit6C | 630 | ai_init_6c | 472 | MAPPED |
| aiAct6C | 642 | ai_act_6c | 487 | MAPPED |
| aiInit6D | 662 | ai_init_6d | 504 | MAPPED |
| aiAct6D | 674 | ai_act_6d | 521 | MAPPED |
| aiInit6E | 695 | ai_init_6e | 538 | MAPPED |
| aiAct6E | 709 | ai_act_6e | 555 | MAPPED |
| aiInit6F | 737 | ai_init_6f | 581 | MAPPED |
| aiAct6F | 758 | ai_act_6f | 605 | MAPPED |
| aiInit7A | 804 | ai_init_7a | 641 | MAPPED |
| aiAct7A | 820 | ai_act_7a | 655 | MAPPED |
| aiInit9A | 875/879 | ai_init_9a | 692 | MAPPED |
| aiAct9A | 921 | ai_act_9a | 727 | MAPPED |
| aiInit10 | 977 | ai_init_10 | 769 | MAPPED |
| aiAct10 | 1025 | ai_act_10 | 801 | MAPPED |
| aiInit11 | 1127 | ai_init_11 | 878 | MAPPED |
| aiAct11 | 1140 | ai_act_11 | 890 | MAPPED |
| aiInit11B | 1259 | ai_init_11b | 975 | MAPPED |
| aiAct11B | 1269 | ai_act_11b | 985 | MAPPED |
| aiInit11C | 1367 | ai_init_11c | 1069 | MAPPED |
| aiAct11C | 1372 | ai_act_11c | 1074 | MAPPED |
| aiInit11D | 1426 | ai_init_11d | 1106 | MAPPED |
| aiAct11D | 1434 | ai_act_11d | 1111 | MAPPED |
| aiInit11E | 1502 | ai_init_11e | 1137 | MAPPED |
| aiAct11E | 1509 | ai_act_11e | 1142 | MAPPED |
| aiInit14 | 1593 | ai_init_14 | 1173 | MAPPED |
| aiAct14 | 1605 | ai_act_14 | 1185 | MAPPED |
| aiInit16A | 1655 | ai_init_16a | 1223 | MAPPED |
| aiAct16A | 1672 | ai_act_16a | 1238 | MAPPED |
| aiInit16B | 1705 | ai_init_16b | 1262 | MAPPED |
| aiAct16B | 1712 | ai_act_16b | 1269 | MAPPED |
| aiInit16C | 1734 | ai_init_16c | 1288 | MAPPED |
| aiAct16C | 1741 | ai_act_16c | 1294 | MAPPED |
| aiInit16D | 1765 | ai_init_16d | 1314 | MAPPED |
| aiAct16D | 1771 | ai_act_16d | 1320 | MAPPED |
| aiInit16E | 1798 | ai_init_16e | 1340 | MAPPED |
| aiAct16E | 1805 | ai_act_16e | 1347 | MAPPED |
| aiInit16F | 1827 | ai_init_16f | 1362 | MAPPED |
| aiAct16F | 1861 | ai_act_16f | 1392 | MAPPED |

### GD-Only Functions in AIFrosh.gd

| GD Function | GD Line | Notes |
|-------------|---------|-------|
| _ai_closest_to_pole | 870 | Internal helper function |
| ai_pop_boy_in_crowd | 1416 | Moved from aiMenuAndDisplay |
| ai_pop_boy | 1467 | Moved from aiMenuAndDisplay |

---

## aiProjectile.cs -> aiProjectile.gd

| C# Function | C# Line | GD Function | GD Line | Status |
|-------------|---------|-------------|---------|--------|
| aisProjectileRebound | 6 | ais_projectile_rebound | 29 | MAPPED |
| aisCreateHoseWhap | 35 | ais_create_hose_whap | 49 | MAPPED |
| aisRunAwayFrom | 48 | ais_run_away_from | 59 | MAPPED |
| aiProjectile | 75 | ai_projectile | 81 | MAPPED |
| aiFloatingProjectile | 724 | ai_floating_projectile | 555 | MAPPED |
| aiInitFloatingPizza | 741 | ai_init_floating_pizza | 567 | MAPPED |
| aiInitFloatingClark | 748 | ai_init_floating_clark | 574 | MAPPED |
| aiCloseUpBeer | 755 | ai_close_up_beer | 632 | MAPPED |
| aiArmRing1 | 767 | ai_arm_ring_1 | 643 | MAPPED |
| aiArmRing2 | 777 | ai_arm_ring_2 | 653 | MAPPED |
| aiArmRing3 | 786 | ai_arm_ring_3 | 663 | MAPPED |
| aisChangeArm | 800 | ais_change_arm | 679 | MAPPED |
| aisChangeArmBackwards | 883 | ais_change_arm_backwards | 721 | MAPPED |
| aisDemoMouseHasMoved | 971 | ais_demo_mouse_has_moved | 765 | MAPPED |
| aiArm | 981 | ai_arm | 776 | MAPPED |
| aiWhap | 1606 | ai_whap | 1230 | MAPPED |
| aiWordBubble | 1613 | ai_word_bubble | 1236 | MAPPED |

### GD-Only Functions in aiProjectile.gd

| GD Function | GD Line | Notes |
|-------------|---------|-------|
| _ai_projectile_flying_toward_target | 103 | Internal helper |
| _ai_projectile_frame_5_aliens | 189 | Internal helper |
| _ai_projectile_frame_6_hippo_frosh_popboy | 226 | Internal helper |
| _ai_projectile_frame_7_frecs_pole_prez | 339 | Internal helper |
| _handle_frecs_collision | 386 | Internal helper |
| _handle_prez_collision | 423 | Internal helper |
| _ai_projectile_frame_8_water | 537 | Internal helper |
| ais_chase_pizza | 581 | Moved from aiSupport |
| ais_chase_clark | 605 | Moved from aiSupport |
| _ai_arm_demo | 807 | Internal helper |
| _ai_arm_nothing | 813 | Internal helper |
| _ai_arm_apple | 839 | Internal helper |
| _ai_arm_pizza | 911 | Internal helper |
| _ai_arm_clark | 943 | Internal helper |
| _ai_arm_exam | 993 | Internal helper |
| _ai_arm_grease | 1053 | Internal helper |
| _ai_arm_iron_ring | 1098 | Internal helper |
| _ai_arm_othrow | 1170 | Internal helper |
| _ai_arm_sthrow | 1182 | Internal helper |
| _ai_arm_changing | 1194 | Internal helper |
| ais_shout_discipline | 1244 | Moved from aiSupport |
| ais_collision_to_response | 1271 | Moved from aiSupport |
| ais_special_collision_to_response | 1324 | Moved from aiSupport |

---

## aiSupport.cs -> AISupport.gd

| C# Function | C# Line | GD Function | GD Line | Status |
|-------------|---------|-------------|---------|--------|
| randintin | 7 | - | - | MOVED to AIMethods.gd |
| aisScrPointInside | 237 | ais_scr_point_inside | 13 | MAPPED |
| aisMouseOver | 253 | ais_mouse_over | 29 | MAPPED |
| aisSendFroshFlying | 258 | ais_send_frosh_flying | 100 | MAPPED |
| aisSendFroshReallyFlying | 268 | ais_send_frosh_really_flying | 108 | MAPPED |
| aisConsume | 275/279 | ais_consume | 380 | MAPPED |
| aisIronRingZap | 326 | ais_iron_ring_zap | 177 | MAPPED |
| aisCollisionToResponse | 350/360 | - | - | MOVED to aiProjectile.gd |
| aisSpecialCollisionToResponse | 433 | - | - | MOVED to aiProjectile.gd |
| aisForgeTrick | 534 | ais_forge_trick | 151 | MAPPED |
| aisShoutDiscipline | 545 | - | - | MOVED to aiProjectile.gd |
| aisChooseNewPersonality | 577 | ais_choose_new_personality | 114 | MAPPED |
| aisGameXYtoPitXY | 584 | ais_game_xy_to_pit_xy | 91 | MAPPED |
| aisPickPyramidSpot | 590 | ais_pick_pyramid_spot | 280 | MAPPED |
| aisPickCloserPyramidSpot | 618 | ais_pick_closer_pyramid_spot | 302 | MAPPED |
| aisPickHoistingSpot | 643 | - | - | MISSING |
| aisPickClimbingSpot | 647 | ais_pick_climbing_spot | 319 | MAPPED |
| aisChooseFroshPitGoal | 674 | ais_choose_frosh_pit_goal | 341 | MAPPED |
| aisChooseFroshUpperLevelGoal | 707 | ais_choose_frosh_upper_level_goal | 365 | MAPPED |
| aisWeightOnShoulders | 727 | ais_weight_on_shoulders | 136 | MAPPED |
| aisCreateProjectile | 749/754 | ais_create_projectile | 121 | MAPPED |
| aisKeepInPitX | 770 | ais_keep_in_pit_x | 60 | MAPPED |
| aisKeepInPitY | 785 | ais_keep_in_pit_y | 70 | MAPPED |
| aisPlummet | 800 | ais_plummet | 36 | MAPPED |
| aisMoveTowardsDestination | 808 | ais_move_towards_destination | 45 | MAPPED |
| aisTopplePyramid | 828 | ais_topple_pyramid | 563 | MAPPED |
| aisStartAMosh | 910 | ais_start_a_mosh | 456 | MAPPED |
| aisAbandonAlien | 950 | - | - | MISSING |
| aisChaseAlien | 978 | ais_chase_alien | 498 | MAPPED |
| aisAbandonClark | 1038 | ais_abandon_clark | 432 | MAPPED |
| aisChaseClark | 1071 | - | - | MOVED to aiProjectile.gd |
| aisAbandonPizza | 1107 | ais_abandon_pizza | 410 | MAPPED |
| aisChasePizza | 1140 | - | - | MOVED to aiProjectile.gd |
| aisRegroup | 1177 | ais_regroup | 549 | MAPPED |
| aisBobUpAndDown | 1195/1199 | ais_bob_up_and_down | 82 | MAPPED |
| aisRandomEvents | 1210 | - | - | MOVED to aiMisc.gd |
| aisThinkForAl | 1297/1302 | ais_think_for_al | 637 | MAPPED |

### GD-Only Functions in AISupport.gd

| GD Function | GD Line | Notes |
|-------------|---------|-------|
| ais_set_frec_action | 161 | Convenience wrapper |
| ais_unlock_achievement | 170 | Moved from aiMisc |
| ais_get_targets_in_scr_range | 198 | Range query helper |
| ais_get_frosh_in_range | 230 | Range query helper |
| ais_get_sprites_in_range | 235 | Range query helper |

---

## aiMenuAndDisplay.cs -> aiMenuAndDisplay.gd

| C# Function | C# Line | GD Function | GD Line | Status |
|-------------|---------|-------------|---------|--------|
| aiMouseCursorMenu | 19 | ai_mouse_cursor_menu | 38 | MAPPED |
| aiMenuStartButton | 35 | ai_menu_start_button | 61 | MAPPED |
| aiMouseHasMoved | 87 | ai_mouse_has_moved | 50 | MAPPED |
| aiMenuExitButton | 97 | ai_menu_exit_button | 112 | MAPPED |
| aiFlyBackInCredits | 153 | ai_fly_back_in_credits | 160 | MAPPED |
| aiMenuOptionsButton | 223 | ai_menu_options_button | 96 | MAPPED |
| aisClickedOnPassCrest | 250 | ais_clicked_on_pass_crest | 229 | MAPPED |
| aiMenuGlowingPassCrest | 256 | ai_menu_glowing_pass_crest | 234 | MAPPED |
| aiInitToggleButton | 301 | ai_init_toggle_button | 269 | MAPPED |
| aiToggleButton | 332 | ai_toggle_button | 297 | MAPPED |
| aiInitBar | 388 | ai_init_bar | 682 | MAPPED |
| aiNextBarScreen | 457 | ai_next_bar_screen | 766 | MAPPED |
| aiPrevBarScreen | 465 | ai_prev_bar_screen | 756 | MAPPED |
| aiNextAchievementScreen | 485 | ai_next_achievement_screen | 744 | MAPPED |
| aiDecorateReturn | 506 | ai_decorate_return | 347 | MAPPED |
| aiOptionsReturn | 519 | ai_options_return | 328 | MAPPED |
| aisPickJacketBarSpot | 543 | ais_pick_jacket_bar_spot | 774 | MAPPED |
| aiAchievementText | 587 | ai_achievement_text | 729 | MAPPED |
| aiBar | 614 | ai_bar | 810 | MAPPED |
| aiMouseCursorTL | 672 | ai_mouse_cursor_tl | 548 | MAPPED |
| aiMouseCursorTR | 682 | ai_mouse_cursor_tr | 557 | MAPPED |
| aiMouseCursorBL | 692 | ai_mouse_cursor_bl | 566 | MAPPED |
| aiMouseCursorBR | 702 | ai_mouse_cursor_br | 575 | MAPPED |
| aisGetPointsBasedOn | 735 | ais_get_points_based_on | 378 | MAPPED |
| aiPointsTens | 757 | ai_points_tens | 392 | MAPPED |
| aiPointsOnes | 767 | ai_points_ones | 400 | MAPPED |
| aiPitTimeSTens | 777 | ai_pit_time_s_tens | 921 | MAPPED |
| aiPitTimeSOnes | 792 | ai_pit_time_s_ones | 932 | MAPPED |
| aiPitTimeMTens | 797 | ai_pit_time_m_tens | 936 | MAPPED |
| aiPitTimeMOnes | 802 | ai_pit_time_m_ones | 940 | MAPPED |
| aiPitTimeHTens | 810 | ai_pit_time_h_tens | 944 | MAPPED |
| aiPitTimeHOnes | 825 | ai_pit_time_h_ones | 961 | MAPPED |
| aiIconIntoPosition | 833 | ai_icon_into_position | 410 | MAPPED |
| aiIconOutOfPosition | 861 | ai_icon_out_of_position | 430 | MAPPED |
| aiDeleteMe | 887 | - | - | MOVED to aiMisc.gd |
| aiRing | 892 | ai_ring | 967 | MAPPED |
| aiIcon | 918 | ai_icon | 449 | MAPPED |
| aiTrackerLR | 1020 | ai_tracker_lr | 534 | MAPPED |
| aiTrackerUL | 1029 | ai_tracker_ul | 540 | MAPPED |
| aiSlamJacket | 1038 | ai_slam_jacket | 863 | MAPPED |
| aiFlyInAndOut | 1080 | - | - | MOVED to AIFlyInAndOut.gd |
| aiInitFlyInAndOut | 1118 | - | - | MOVED to AIFlyInAndOut.gd |
| aiFlyInAndOut2 | 1164 | - | - | MOVED to AIFlyInAndOut.gd |
| aiInitFlyInAndOut2 | 1195 | - | - | MOVED to AIFlyInAndOut.gd |
| aiConsole | 1216 | ai_console | 366 | MAPPED |
| aiGrid | 1221 | ai_grid | 371 | MAPPED |
| aiRingMeter | 1226 | ai_ring_meter | 638 | MAPPED |
| aiWaterMeter | 1255 | ai_water_meter | 608 | MAPPED |
| aiPowerMeter | 1293 | ai_power_meter | 586 | MAPPED |
| aiHighScore | 1319 | ai_high_score | 521 | MAPPED |
| aiAchievementUnlockedNotice | 1335 | ai_achievement_unlocked_notice | 668 | MAPPED |
| aiPopBoy | 1366 | - | - | MOVED to AIFrosh.gd |
| aiPopBoyInCrowd | 1501 | - | - | MOVED to AIFrosh.gd |

### GD-Only Functions in aiMenuAndDisplay.gd

| GD Function | GD Line | Notes |
|-------------|---------|-------|
| ai_inanimate | 359 | Moved from aiMisc |
| ais_change_arm | 661 | Duplicate in aiProjectile |
| tens_digit_frame | 900 | Helper function |
| ones_digit_frame | 908 | Helper function |

---

## aiPopUp.cs -> aiPopUp.gd

| C# Function | C# Line | GD Function | GD Line | Status |
|-------------|---------|-------------|---------|--------|
| aisCheckForGeneralistAchievement | 13 | ais_check_for_generalist_achievement | 27 | MAPPED |
| aiGetApples | 22 | ai_get_apples | 37 | MAPPED |
| aiGetPizza | 52 | ai_get_pizza | 65 | MAPPED |
| aiGetClark | 83 | ai_get_clark | 93 | MAPPED |
| aiGetExam | 114 | ai_get_exam | 121 | MAPPED |
| aiGetHose | 137 | ai_get_hose | 140 | MAPPED |
| aisAlienInPit | 157 | ais_alien_in_pit | 158 | MAPPED |
| aiArtSciMInPit | 189 | ai_artsci_m_in_pit | 185 | MAPPED |
| aiArtSciFInPit | 210 | ai_artsci_f_in_pit | 200 | MAPPED |
| aiCommieMInPit | 226 | ai_commie_m_in_pit | 214 | MAPPED |
| aiCommieFInPit | 242 | ai_commie_f_in_pit | 226 | MAPPED |
| aisPushIntoPit | 261 | ais_push_into_pit | 240 | MAPPED |
| aiPushArtSciMIntoPit | 287 | ai_push_artsci_m_into_pit | 263 | MAPPED |
| aiPushArtSciFIntoPit | 309 | ai_push_artsci_f_into_pit | 278 | MAPPED |
| aiPushCommieMIntoPit | 329 | ai_push_commie_m_into_pit | 292 | MAPPED |
| aiPushCommieFIntoPit | 353 | ai_push_commie_f_into_pit | 309 | MAPPED |
| aiPushSciConM | 376 | ai_push_scicon_m | 325 | MAPPED |
| aiPushSciConF | 413 | ai_push_scicon_f | 352 | MAPPED |
| aiInitForeGroundEntry | 452 | ai_init_foreground_entry | 380 | MAPPED |
| aiForeGroundEntry | 479 | ai_foreground_entry | 400 | MAPPED |
| aiInitForegroundOnScreen | 534 | ai_init_foreground_on_screen | 438 | MAPPED |
| aiForegroundOnScreen | 542 | ai_foreground_on_screen | 445 | MAPPED |
| aiInitSciConForegroundOnScreen | 557 | ai_init_scicon_foreground_on_screen | 461 | MAPPED |
| aiSciConForegroundOnScreen | 567 | ai_scicon_foreground_on_screen | 469 | MAPPED |
| aiInitForeGroundExit | 723 | ai_init_foreground_exit | 594 | MAPPED |
| aiForeGroundExit | 744 | ai_foreground_exit | 608 | MAPPED |
| aiInitPopUp | 795/804 | ai_init_popup | 645 | MAPPED |

---

## aiCrowd.cs -> aiCrowd.gd

| C# Function | C# Line | GD Function | GD Line | Status |
|-------------|---------|-------------|---------|--------|
| aisSetFrecAction | 7 | ais_set_frec_action | 19 | MAPPED |
| aiFrecGroupBlock | 14 | ai_frec_group_block | 31 | MAPPED |
| aiFrecActionBlock | 20 | ai_frec_action_block | 37 | MAPPED |
| aiFrecGroupMill | 26 | ai_frec_group_mill | 44 | MAPPED |
| aiFrecActionMill | 45 | ai_frec_action_mill | 61 | MAPPED |
| aiFrecGroupCheer | 66 | ai_frec_group_cheer | 80 | MAPPED |
| aiFrecActionCheer | 91 | ai_frec_action_cheer | 100 | MAPPED |
| aiFrecGroupSlam | 117 | ai_frec_group_slam | 120 | MAPPED |
| aiFrecActionSlam | 139 | ai_frec_action_slam | 137 | MAPPED |
| aiFrecGroupLookUp | 163 | ai_frec_group_look_up | 156 | MAPPED |
| aiFrecActionLookUp | 171 | ai_frec_action_look_up | 164 | MAPPED |
| aiFrecGroupStayinAlive | 179 | ai_frec_group_stayin_alive | 174 | MAPPED |
| aiFrecActionStayinAlive | 204 | ai_frec_action_stayin_alive | 185 | MAPPED |
| aiFrecActionPart | 229 | ai_frec_action_part | 198 | MAPPED |
| aiFrecGroupPart | 234 | ai_frec_group_part | 203 | MAPPED |
| aiFrecGroupBoo | 245 | ai_frec_group_boo | 214 | MAPPED |
| aiFrecActionBoo | 257 | ai_frec_action_boo | 225 | MAPPED |
| aiFrecActionShout | 270 | ai_frec_action_shout | 238 | MAPPED |
| aiFrecGroupShout | 275 | ai_frec_group_shout | 242 | MAPPED |
| aiFrecGroupLookURL | 298 | ai_frec_group_look_url | 261 | MAPPED |
| aiFrecGroupLookULR | 311 | ai_frec_group_look_ulr | 271 | MAPPED |
| aiFrecActionLookURL | 323 | ai_frec_action_look_url | 281 | MAPPED |
| aiFrecActionLookULR | 335 | ai_frec_action_look_ulr | 291 | MAPPED |
| aiFrecGroupWave | 347 | ai_frec_group_wave | 303 | MAPPED |
| aiFrecActionWave | 363 | ai_frec_action_wave | 316 | MAPPED |
| aiFrecGroup | 380 | ai_frec_group | 331 | MAPPED |
| aiFrecAction | 415 | ai_frec_action | 361 | MAPPED |
| panONX | 451 | pan_on_x | 25 | MAPPED |
| aiFrecBoring | 456 | ai_frec_boring | 392 | MAPPED |

---

## aiBackground.cs -> aiBackground.gd

| C# Function | C# Line | GD Function | GD Line | Status |
|-------------|---------|-------------|---------|--------|
| aiFrecGroupInit | 6 | ai_frec_group_init | 46 | MAPPED |
| aiFrecActionInit | 23 | ai_frec_action_init | 54 | MAPPED |
| aiSciConMInit | 36 | ai_scicon_m_init | 67 | MAPPED |
| aiSciConM | 41 | ai_scicon_m | 72 | MAPPED |
| aiSciConFInit | 43 | ai_scicon_f_init | 76 | MAPPED |
| aiSciConF | 48 | ai_scicon_f | 81 | MAPPED |
| aiUpperYearInit | 51 | ai_upper_year_init | 87 | MAPPED |
| aiUpperYear | 56 | ai_upper_year | 92 | MAPPED |
| aiPoofInit | 58 | ai_poof_init | 98 | MAPPED |
| aiPoof | 63 | ai_poof | 103 | MAPPED |
| aiGWBalloonInit | 79 | ai_gw_balloon_init | 118 | MAPPED |
| aiGWBalloon | 124 | ai_gw_balloon | 153 | MAPPED |
| aiGWHippoInit | 158 | ai_gw_hippo_init | 181 | MAPPED |
| aiGWHippo | 168 | ai_gw_hippo | 190 | MAPPED |
| aiPrezInit | 340 | ai_prez_init | 316 | MAPPED |
| aiPrez | 347 | ai_prez | 323 | MAPPED |
| aiForgeInit | 412 | ai_forge_init | 379 | MAPPED |
| aiForge | 426 | ai_forge | 386 | MAPPED |

---

## poleposn.cs -> PolePosition.gd

| C# Function | C# Line | GD Function | GD Line | Status |
|-------------|---------|-------------|---------|--------|
| GetX | 26 | get_x | 110 | MAPPED |
| GetY | 31 | get_y | 114 | MAPPED |
| GetClaimer | 36 | get_claimer | 118 | MAPPED |
| PositionIsTaken | 41 | position_is_taken | 148 | MAPPED |
| PositionIsFree | 47 | position_is_free | 152 | MAPPED |
| GetParent | 53 | get_parent | 122 | MAPPED |
| GetChild | 58 | get_child | 126 | MAPPED |
| AdjacentChain | 63 | adjacent_chain | 130 | MAPPED |
| SetAdjacentChain | 68 | set_adjacent_chain | 135 | MAPPED |
| ReleaseClaim | 74 | release_claim | 139 | MAPPED |
| SetClaim | 79 | set_claim | 143 | MAPPED |
| IsClaimed | 84 | is_claimed | 156 | MAPPED |
| FirstTakenChild | 89 | first_taken_child | 161 | MAPPED |
| FirstFreeChild | 100 | first_free_child | 171 | MAPPED |
| LastTakenChild | 111 | last_taken_child | 181 | MAPPED |
| CalculateScreenPosition | 122 | calculate_screen_position | 192 | MAPPED |
| CreateChild | 139 | create_child | 197 | MAPPED |
| Initialize_PolePositions | 214 | initialize_pole_positions | 203 | MAPPED |

---

## AIDefine.cs -> AIDefine.gd / AIMethods.gd

| C# Function | C# Line | GD Function | GD File | GD Line | Status |
|-------------|---------|-------------|---------|---------|--------|
| dSKYYTOPITY | 71 | d_sky_y_to_pit_y | AIMethods.gd | 152 | MAPPED |
| dSKYLINEYTOPITY | 75 | d_skyline_y_to_pit_y | AIMethods.gd | 155 | MAPPED |
| dMISCYTOPITY | 79 | d_misc_y_to_pit_y | AIMethods.gd | 158 | MAPPED |
| dPITYTOMISCY | 83 | d_pit_y_to_misc_y | AIMethods.gd | 161 | MAPPED |
| NOSPEECHFOR | 99 | no_speech_for / NOSPEECHFOR | AIMethods.gd | 130/143 | MAPPED |
| SPEECHOK | 105 | speech_ok / SPEECHOK | AIMethods.gd | 137/146 | MAPPED |

**Note**: AIDefine.gd contains only constants, no functions.

---

## aiWaterEffects.cs -> aiWaterEffects.gd

| C# Function | C# Line | GD Function | GD Line | Status |
|-------------|---------|-------------|---------|--------|
| aiSplashM | (grep) | ai_splash_m | 13 | MAPPED |
| aiSplashML | (grep) | ai_splash_ml | 21 | MAPPED |
| aiSplashL | (grep) | ai_splash_l | 29 | MAPPED |
| aiSplashS | (grep) | ai_splash_s | 37 | MAPPED |
| aiRipple | (grep) | ai_ripple | 47 | MAPPED |

---

## aiMisc.cs -> aiMisc.gd

| C# Function | C# Line | GD Function | GD Line | Status |
|-------------|---------|-------------|---------|--------|
| aiInanimate | (grep) | ai_inanimate | 17 | MAPPED |
| aiShowCurrentAchievementScreen | (grep) | ai_show_current_achievement_screen | 30 | MAPPED |
| aisUnlockAchievement | (grep) | ais_unlock_achievement | 37 | MAPPED |
| aiTam | (grep) | ai_tam | 70 | MAPPED |
| aisRandomCrowdNoise | (grep) | ais_random_crowd_noise | 86 | MAPPED |
| aiRandomEventGenerator | (grep) | ai_random_event_generator | 140 | MAPPED |

### GD-Only Functions in aiMisc.gd

| GD Function | GD Line | Notes |
|-------------|---------|-------|
| ai_delete_me | 24 | Moved from aiMenuAndDisplay |
| ais_random_events | 213 | Moved from aiSupport |

---

## GD-Only Files

### AIFlyInAndOut.gd

This file contains the fly-in/fly-out animation system, extracted from aiMenuAndDisplay.cs for better organization.

| GD Function | GD Line | Notes |
|-------------|---------|-------|
| ai_fly_in_and_out | 13 | From aiMenuAndDisplay.cs line 1080 |
| ai_init_fly_in_and_out | 42 | From aiMenuAndDisplay.cs line 1118 |
| ai_fly_in_and_out_2 | 83 | From aiMenuAndDisplay.cs line 1164 |
| ai_init_fly_in_and_out_2 | 106 | From aiMenuAndDisplay.cs line 1195 |

### AIMethods.gd (Autoload)

This file is the GDScript equivalent of the C# partial class pattern. It holds all shared state and some utility functions.

| GD Function | GD Line | Notes |
|-------------|---------|-------|
| randintin | 124 | Static utility from aiSupport.cs |
| no_speech_for | 130 | From AIDefine.cs NOSPEECHFOR |
| speech_ok | 137 | From AIDefine.cs SPEECHOK |
| NOSPEECHFOR | 143 | Uppercase alias |
| SPEECHOK | 146 | Uppercase alias |
| d_sky_y_to_pit_y | 152 | From AIDefine.cs |
| d_skyline_y_to_pit_y | 155 | From AIDefine.cs |
| d_misc_y_to_pit_y | 158 | From AIDefine.cs |
| d_pit_y_to_misc_y | 161 | From AIDefine.cs |
| pan_on_x | 166 | Static utility |

---

## Missing Functions (C# without GD equivalent)

| C# Function | C# File | C# Line | Notes |
|-------------|---------|---------|-------|
| aisPickHoistingSpot | aiSupport.cs | 643 | Not implemented in GD |
| aisAbandonAlien | aiSupport.cs | 950 | Not implemented in GD |

---

## Function Reorganization Summary

Some functions were moved between files during the port for better organization:

### Moved to AIFlyInAndOut.gd
- aiFlyInAndOut, aiInitFlyInAndOut, aiFlyInAndOut2, aiInitFlyInAndOut2 (from aiMenuAndDisplay.cs)

### Moved to aiProjectile.gd
- aisCollisionToResponse, aisSpecialCollisionToResponse, aisShoutDiscipline (from aiSupport.cs)
- aisChasePizza, aisChaseClark (from aiSupport.cs)

### Moved to AIFrosh.gd
- aiPopBoy, aiPopBoyInCrowd (from aiMenuAndDisplay.cs)

### Moved to aiMisc.gd
- aiDeleteMe (from aiMenuAndDisplay.cs)
- aisRandomEvents (from aiSupport.cs)

### Moved to AIMethods.gd (Autoload)
- randintin (from aiSupport.cs)
- Layer conversion functions (from AIDefine.cs)
- Speech control functions (from AIDefine.cs)

---

*Generated: 2026-01-25*
