class_name SpriteInit

# SpriteInit.gd - Static class for sprite creation and initialization
# Ported from SpriteInit.cs

static var R: RandomNumberGenerator = RandomNumberGenerator.new()

static func create_sprite(type: int, x: int = 0, y: int = 0) -> TSprite:
	var s = TSprite.new()
	s.b_deleted = false
	s.n_cc = 0
	s.n_x = x
	s.n_y = y
	init_sprite(s, type)
	return s

static func delete_sprite(s: TSprite):
	s.dispose()

static func init_sprite(s: TSprite, type: int):
	s.n_tag = type
	s.sprite_type = type

	# Route to appropriate initializer based on sprite type range
	if type > Enums.SpriteType.SPRMNU_START and type < Enums.SpriteType.SPRMNU_END:
		_init_menu_sprite(s, type)
	elif type > Enums.SpriteType.SPRGAME_START and type < Enums.SpriteType.SPRGAME_END:
		_init_game_sprite(s, type)
	elif type > Enums.SpriteType.SPRTRANS_START and type < Enums.SpriteType.SPRTRANS_END:
		_init_trans_sprite(s, type)


# =============================================================================
# MENU SPRITES
# =============================================================================

static func _init_menu_sprite(s: TSprite, type: int):
	match type:
		Enums.SpriteType.SPRMNU_MOUSE_CURSOR:
			s.b_attrib[1] = true
			if AIMethods.frm.size() > Enums.GameBitmapEnumeration.bmpMOU_SELECT1:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMOU_SELECT1])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_mouse_cursor_menu"))
			s.b_super_front = true  # Draw on top of everything including text

		Enums.SpriteType.SPRMNU_OPTIONS_BACK:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpOPTIONSBACK])
			# s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_inanimate"), 0, 480, 0, 0, 1, 1)

		Enums.SpriteType.SPRMNU_TITLEBACK:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEBACK])
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_inanimate"), 640, 0, 0, 0, 2, 0)

		Enums.SpriteType.SPRMNU_TITLESTART:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLESTART])
			s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_MAKE_TITLE_SOUND_PLAY] = false
			s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_DO_NOT_ACTIVATE] = false
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_menu_start_button"), -640, 0, -95, 266, 2, 0)

		Enums.SpriteType.SPRMNU_TITLEOPTIONS:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEOPTIONS])
			s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_DO_NOT_ACTIVATE] = false
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_menu_options_button"), -640, 0, -77, 347, 2, 0)

		Enums.SpriteType.SPRMNU_TITLEEXIT:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEEXIT])
			s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN] = 0
			s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_DO_NOT_ACTIVATE] = false
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_menu_exit_button"), 45, 507, 109, 31, 0, 2)

		Enums.SpriteType.SPRMNU_BTN_TOGGLE0, Enums.SpriteType.SPRMNU_BTN_TOGGLE1, \
		Enums.SpriteType.SPRMNU_BTN_TOGGLE2, Enums.SpriteType.SPRMNU_BTN_TOGGLE3, \
		Enums.SpriteType.SPRMNU_BTN_TOGGLE4:
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_toggle_button"))
			AIMenuAndDisplay.ai_init_toggle_button(s, type - Enums.SpriteType.SPRMNU_BTN_TOGGLE0)

		Enums.SpriteType.SPRMNU_TXT_SELECT:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTXTSELECT])
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_inanimate"), 0, -200, 15, 15, 1, 1)

		Enums.SpriteType.SPRMNU_JACKET_BACK:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpJACKETBACK])
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_inanimate"), 640, 0, 0, 0, 2, 0)

		Enums.SpriteType.SPRMNU_MENU_JACKET:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMENUJACKET])
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_inanimate"), -850, 0, 0, 0, 1, 1)

		Enums.SpriteType.SPRMNU_MENU_PASS_CREST:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMENUPASSCREST1])
			s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_MAKE_TITLE_SOUND_PLAY] = false
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_menu_glowing_pass_crest"), -721, 128, 129, 128, 1, 1)

		Enums.SpriteType.SPRMNU_AI_PREV_BAR_SCREEN:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMENUPREVBARSCREEN])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_prev_bar_screen"))

		Enums.SpriteType.SPRMNU_AI_NEXT_BAR_SCREEN:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpMENUPREVBARSCREEN])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_next_bar_screen"))

		Enums.SpriteType.SPRMNU_AI_NEXT_ACHIEVEMENT_SCREEN:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMENUPREVBARSCREEN])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_next_achievement_screen"))

		Enums.SpriteType.SPRMNU_OPTIONS_RETURN:
			s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_MAKE_TITLE_SOUND_PLAY] = false
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpBTNRETURN])
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_options_return"), 640, 392, 10, 423, 1, 1)

		Enums.SpriteType.SPRMNU_DECORATE_RETURN:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpBTNRETURN])
			AIFlyInAndOut.ai_init_fly_in_and_out(s, Callable(AIMenuAndDisplay, "ai_decorate_return"), 640, 392, 10, 423, 1, 1)

		Enums.SpriteType.SPRMNU_ACHIEVEMENT_TEXT:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
			s.sprite_text = TSprite.SpriteTextType.Small
			s.n_r = 255
			s.n_g = 255
			s.n_b = 255
			# Note: Fly animation is set up in GameLoop.init_transition for STATEOPTIONS
			s.text = ""

		Enums.SpriteType.SPRMNU_ACHIEVEMENT_ADDITIONAL_TEXT:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
			s.sprite_text = TSprite.SpriteTextType.Small
			s.n_r = 255
			s.n_g = 255
			s.n_b = 255
			s.set_behavior(Callable(AIMisc, "ai_show_current_achievement_screen"))
			s.text = ""

		Enums.SpriteType.SPRMNU_BAR1, Enums.SpriteType.SPRMNU_BAR2, Enums.SpriteType.SPRMNU_BAR3, \
		Enums.SpriteType.SPRMNU_BAR4, Enums.SpriteType.SPRMNU_BAR5, Enums.SpriteType.SPRMNU_BAR6, \
		Enums.SpriteType.SPRMNU_BAR7, Enums.SpriteType.SPRMNU_BAR8, Enums.SpriteType.SPRMNU_BAR9, \
		Enums.SpriteType.SPRMNU_BAR10, Enums.SpriteType.SPRMNU_BAR11, Enums.SpriteType.SPRMNU_BAR12, \
		Enums.SpriteType.SPRMNU_BAR13, Enums.SpriteType.SPRMNU_BAR14, Enums.SpriteType.SPRMNU_BAR15, \
		Enums.SpriteType.SPRMNU_BAR16, Enums.SpriteType.SPRMNU_BAR17, Enums.SpriteType.SPRMNU_BAR18, \
		Enums.SpriteType.SPRMNU_BAR19, Enums.SpriteType.SPRMNU_BAR20:
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_bar"))
			AIMenuAndDisplay.ai_init_bar(s, type - Enums.SpriteType.SPRMNU_BAR1)


# =============================================================================
# GAME SPRITES
# =============================================================================

static func _init_game_sprite(s: TSprite, type: int):
	match type:
		# === WATER EFFECTS ===
		Enums.SpriteType.SPR_RIPPLE:
			if not AIMethods.s_sound[Enums.ASSList.SSND_WATER_RIPPLE].is_playing():
				AIMethods.s_sound[Enums.ASSList.SSND_WATER_RIPPLE].play(SoundbankInfo.VOL_4, AICrowd.pan_on_x(s))
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpRIPPLE1])
			s.set_behavior(Callable(AIWaterEffects, "ai_ripple"))

		Enums.SpriteType.SPR_SPLASH_L:
			if not AIMethods.s_sound[Enums.ASSList.SSND_WATER_SPLASHBIG].is_playing():
				AIMethods.s_sound[Enums.ASSList.SSND_WATER_SPLASHBIG].play(SoundbankInfo.VOL_7, AICrowd.pan_on_x(s))
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSPLASHL1])
			s.set_behavior(Callable(AIWaterEffects, "ai_splash_l"))

		Enums.SpriteType.SPR_SPLASH_M:
			if not AIMethods.s_sound[Enums.ASSList.SSND_WATER_SPLASHMID].is_playing():
				AIMethods.s_sound[Enums.ASSList.SSND_WATER_SPLASHMID].play(SoundbankInfo.VOL_6, AICrowd.pan_on_x(s))
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSPLASHM1])
			s.set_behavior(Callable(AIWaterEffects, "ai_splash_m"))

		Enums.SpriteType.SPR_SPLASH_ML:
			if not AIMethods.s_sound[Enums.ASSList.SSND_WATER_SPLASHMID].is_playing():
				AIMethods.s_sound[Enums.ASSList.SSND_WATER_SPLASHMID].play(SoundbankInfo.VOL_6, AICrowd.pan_on_x(s))
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpSPLASHM1])
			s.set_behavior(Callable(AIWaterEffects, "ai_splash_ml"))

		Enums.SpriteType.SPR_SPLASH_S:
			if not AIMethods.s_sound[Enums.ASSList.SSND_WATER_SPLASHSMALL].is_playing():
				AIMethods.s_sound[Enums.ASSList.SSND_WATER_SPLASHSMALL].play(SoundbankInfo.VOL_5, AICrowd.pan_on_x(s))
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSPLASHS1])
			s.set_behavior(Callable(AIWaterEffects, "ai_splash_s"))

		# === CONSOLE / UI ===
		Enums.SpriteType.SPR_CONSOLE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCONSOLE])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_console"))

		Enums.SpriteType.SPR_GRID:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGRID1])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_grid"))

		Enums.SpriteType.SPR_POWER_METER:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOWERBAR])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_power_meter"))

		Enums.SpriteType.SPR_WATER_METER:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpWATERBAR])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_water_meter"))

		Enums.SpriteType.SPR_RING_METER:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpRINGBAR])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_ring_meter"))

		Enums.SpriteType.SPR_CLOSEUP_BEER:
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CHUG].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLOSEUPBEER1])
			s.set_behavior(Callable(AIProjectile, "ai_close_up_beer"))

		# === PROJECTILES ===
		Enums.SpriteType.SPR_APPLE:
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_WHOOSH1 + R.randi() % SoundbankInfo.NSND_EFFECTS_WHOOSH].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
			s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE] = Enums.ProjTypes.PROJ_APPLE
			s.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_TOWARD_TARGET
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAPPLE1_1 + R.randi() % SoundbankInfo.NSPR_APPLE1])
			s.set_behavior(Callable(AIProjectile, "ai_projectile"))

		Enums.SpriteType.SPR_PIZZA:
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_TOSSPIZZA].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
			s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE] = Enums.ProjTypes.PROJ_PIZZA
			s.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_TOWARD_TARGET
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPIZZA1_1 + R.randi() % SoundbankInfo.NSPR_PIZZA1])
			s.set_behavior(Callable(AIProjectile, "ai_projectile"))

		Enums.SpriteType.SPR_CLARK:
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_TOSSBEER].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
			s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE] = Enums.ProjTypes.PROJ_CLARK
			s.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_TOWARD_TARGET
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLARK2_1 + R.randi() % SoundbankInfo.NSPR_CLARK2])
			s.set_behavior(Callable(AIProjectile, "ai_projectile"))

		Enums.SpriteType.SPR_EXAM:
			s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE] = Enums.ProjTypes.PROJ_EXAM
			s.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_TOWARD_TARGET
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpEXAM1_1 + R.randi() % SoundbankInfo.NSPR_EXAM1])
			s.set_behavior(Callable(AIProjectile, "ai_projectile"))

		Enums.SpriteType.SPR_GREASE:
			s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE] = Enums.ProjTypes.PROJ_GREASE
			s.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_TOWARD_TARGET
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSPRAY1 + R.randi() % 2])
			s.set_behavior(Callable(AIProjectile, "ai_projectile"))

		# === POPUP CHARACTERS ===
		Enums.SpriteType.SPR_POPUP_APPLES:
			var voice: int = Enums.ASLList.LSND_APPLES_OFFER1 + R.randi() % (12 if Globals.myGameConditions.is_ritual() else 10)
			AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpOFFERAPPLES],
				6, 1, 1, 1, voice, Callable(AIPopUp, "ai_get_apples"))

		Enums.SpriteType.SPR_POPUP_PIZZA:
			var voice: int = Enums.ASLList.LSND_PIZZA_OFFER1 + R.randi() % (4 if Globals.myGameConditions.is_ritual() else 3)
			AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpOFFERPIZZA],
				6, 1, 1, 1, voice, Callable(AIPopUp, "ai_get_pizza"))

		Enums.SpriteType.SPR_POPUP_BEER:
			var voice: int = Enums.ASLList.LSND_CLARK_OFFER1 + R.randi() % (10 if Globals.myGameConditions.is_ritual() else 6)
			AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpOFFERCLARK],
				6, 1, 1, 1, voice, Callable(AIPopUp, "ai_get_clark"))

		Enums.SpriteType.SPR_POPUP_EXAM:
			var voice: int = Enums.ASLList.LSND_EXAM_OFFER1 + R.randi() % 5
			AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpOFFEREXAM],
				6, 1, 1, 1, voice, Callable(AIPopUp, "ai_get_exam"))

		Enums.SpriteType.SPR_POPUP_HOSE:
			var voice: int = Enums.ASLList.LSND_HOSE_OFFER1 + R.randi() % (4 if Globals.myGameConditions.is_ritual() else 3)
			AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpOFFERHOSE],
				6, 1, 1, 1, voice, Callable(AIPopUp, "ai_get_hose"))

		Enums.SpriteType.SPR_POPUP_ARTSCIM:
			if Globals.myGameConditions.is_ritual() and (0 == R.randi() % 4):
				AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpARTSCIM_POPUP5],
					4, 1, 1, 1, Enums.ASLList.LSND_ARTSCI_MALE_TAUNTR1, Callable(AIPopUp, "ai_push_artsci_m_into_pit"))
			else:
				AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpARTSCIM_POPUP1 + R.randi() % (SoundbankInfo.NSPR_ARTSCIM_POPUP - 1)],
					4, 1, 1, 1, Enums.ASLList.LSND_ARTSCI_MALE_TAUNT1 + R.randi() % SoundbankInfo.NSND_ARTSCI_MALE_TAUNT, Callable(AIPopUp, "ai_push_artsci_m_into_pit"))

		Enums.SpriteType.SPR_POPUP_ARTSCIF:
			AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpARTSCIF_POPUP1 + R.randi() % SoundbankInfo.NSPR_ARTSCIF_POPUP],
				4, 1, 1, 1, Enums.ASLList.LSND_ARTSCI_FEMALE_TAUNT1 + R.randi() % SoundbankInfo.NSND_ARTSCI_FEMALE_TAUNT, Callable(AIPopUp, "ai_push_artsci_f_into_pit"))

		Enums.SpriteType.SPR_POPUP_COMMIEM:
			if 0 != R.randi() % 2:
				AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpCOMMIEM_POPUP1 + (SoundbankInfo.NSPR_COMMIEM_POPUP - 1)],
					4, 1, 1, 1, Enums.ASLList.LSND_COMMIE_MALE_PHONE1 + R.randi() % 2, Callable(AIPopUp, "ai_push_commie_m_into_pit"))
			elif Globals.myGameConditions.is_ritual() and (0 == R.randi() % 2):
				AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpCOMMIEM_POPUP1 + R.randi() % (SoundbankInfo.NSPR_COMMIEM_POPUP - 1)],
					4, 1, 1, 1, Enums.ASLList.LSND_COMMIE_MALE_TAUNTR1 + R.randi() % 3, Callable(AIPopUp, "ai_push_commie_m_into_pit"))
			else:
				AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpCOMMIEM_POPUP1 + R.randi() % (SoundbankInfo.NSPR_COMMIEM_POPUP - 1)],
					4, 1, 1, 1, Enums.ASLList.LSND_COMMIE_MALE_TAUNT1 + R.randi() % SoundbankInfo.NSND_COMMIE_MALE_TAUNT, Callable(AIPopUp, "ai_push_commie_m_into_pit"))

		Enums.SpriteType.SPR_POPUP_COMMIEF:
			AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpCOMMIEF_POPUP1 + R.randi() % SoundbankInfo.NSPR_COMMIEF_POPUP],
				4, 1, 1, 1, Enums.ASLList.LSND_COMMIE_FEMALE_TAUNT1 + R.randi() % 4, Callable(AIPopUp, "ai_push_commie_f_into_pit"))

		Enums.SpriteType.SPR_SCICON_M:
			if 0 != R.randi() % 2:
				AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONM1],
					6, 2, 2, 2, Enums.ASLList.LSND_SCICONM_POPUP1 + R.randi() % 3, Callable(AIPopUp, "ai_push_scicon_m"), true)
			else:
				AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONM1],
					6, 3, 3, 3, Enums.ASLList.LSND_SCICONM_POPUP1 + R.randi() % 3, Callable(AIPopUp, "ai_push_scicon_m"), true)
			s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_MALE] = true

		Enums.SpriteType.SPR_SCICON_F:
			if 0 != R.randi() % 2:
				AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONF1],
					6, 2, 2, 2, Enums.ASLList.LSND_SCICONF_POPUP1 + R.randi() % 4, Callable(AIPopUp, "ai_push_scicon_f"), true)
			else:
				AIPopUp.ai_init_popup(s, AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONF1],
					6, 3, 3, 3, Enums.ASLList.LSND_SCICONF_POPUP1 + R.randi() % 4, Callable(AIPopUp, "ai_push_scicon_f"), true)
			s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_MALE] = false

		# === BACKGROUND ELEMENTS ===
		Enums.SpriteType.SPR_CLOUDS:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLOUDS])
			s.set_behavior(Callable(AIMisc, "ai_inanimate"))

		Enums.SpriteType.SPR_TREES:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTREES])
			s.set_behavior(Callable(AIMisc, "ai_inanimate"))

		Enums.SpriteType.SPR_BACKDROP:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpBACKDROP])
			s.set_behavior(Callable(AIMisc, "ai_inanimate"))

		Enums.SpriteType.SPR_FREC_GROUP:
			AIBackground.ai_frec_group_init(s)

		Enums.SpriteType.SPR_FREC_ACTION:
			AIBackground.ai_frec_action_init(s)

		Enums.SpriteType.SPR_UPPER_YEAR:
			AIBackground.ai_upper_year_init(s)

		Enums.SpriteType.SPR_POOF:
			AIBackground.ai_poof_init(s)

		Enums.SpriteType.SPR_GW_BALLOON:
			AIBackground.ai_gw_balloon_init(s)

		Enums.SpriteType.SPR_GW_HIPPO:
			AIBackground.ai_gw_hippo_init(s)

		Enums.SpriteType.SPR_PREZ:
			AIBackground.ai_prez_init(s)

		Enums.SpriteType.SPR_FORGE:
			s.n_attrib[Enums.NAttrForge.ATTR_FORGE_MOTION] = 0
			s.n_attrib[Enums.NAttrForge.ATTR_FORGE_SWING] = 0
			s.n_attrib[Enums.NAttrForge.ATTR_FORGE_ENERGY] = 0
			AIBackground.ai_forge_init(s)

		Enums.SpriteType.SPR_PODIUM:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPODIUM])
			s.set_behavior(Callable(AIMisc, "ai_random_event_generator"))

		Enums.SpriteType.SPR_INANIMATE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
			s.set_behavior(Callable(AIMisc, "ai_inanimate"))

		Enums.SpriteType.SPR_POLE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOLE])
			s.set_behavior(Callable(AIMisc, "ai_inanimate"))

		Enums.SpriteType.SPR_WATER:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpWATER])
			s.set_behavior(Callable(AIMisc, "ai_inanimate"))

		# === FROSH ===
		Enums.SpriteType.SPR_FROSH:
			s.n_z = 30
			s.set_goal(Enums.Goals.GOAL_MINDLESS_WANDERING)
			s.pp_chosen = null
			s.n_dest_x = AIMethods.randintin(AIDefine.D_PIT_MIN_X, AIDefine.D_PIT_MAX_X)
			s.n_dest_y = AIMethods.randintin(AIDefine.D_PIT_MIN_Y, AIDefine.D_PIT_MAX_Y)
			s.n_dest_z = 0
			s.nv_x = 0
			s.nv_y = 0
			s.nv_z = 0
			s.n_attrib[Enums.NAttrFrosh.ATTR_MIND_SET] = Enums.MindSets.MINDSET_GULLIBLE
			s.n_attrib[Enums.NAttrFrosh.ATTR_MOTIVATION] = Enums.MotivationLevels.MOTIVATION_HIGH
			s.n_attrib[Enums.NAttrFrosh.ATTR_STR] = AIDefine.GN_DEFAULT_STRENGTH + AIMethods.randintin(-3, 3)
			s.b_attrib[Enums.BAttrFrosh.ATTR_EXCITED] = false
			s.b_attrib[Enums.BAttrFrosh.ATTR_THIRSTY] = true
			s.b_attrib[Enums.BAttrFrosh.ATTR_HUNGRY] = true
			s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 0
			AIFrosh.ai_init_2(s)

		# === MOUSE CURSORS (GAME) ===
		Enums.SpriteType.SPR_MOUSE_CURSOR_BL:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMOU_TARBL])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_mouse_cursor_bl"))

		Enums.SpriteType.SPR_MOUSE_CURSOR_BR:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMOU_TARBR])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_mouse_cursor_br"))

		Enums.SpriteType.SPR_MOUSE_CURSOR_TL:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMOU_TARTL])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_mouse_cursor_tl"))

		Enums.SpriteType.SPR_MOUSE_CURSOR_TR:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMOU_TARTR])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_mouse_cursor_tr"))

		# === ARM / HAND ===
		Enums.SpriteType.SPR_ARM:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND0_1])
			s.set_behavior(Callable(AIProjectile, "ai_arm"))
			s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = 0
			s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0

		Enums.SpriteType.SPR_ARM_RING_1:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND7_3])
			s.set_behavior(Callable(AIProjectile, "ai_arm_ring_1"))

		Enums.SpriteType.SPR_ARM_RING_2:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND7_2])
			s.set_behavior(Callable(AIProjectile, "ai_arm_ring_2"))

		Enums.SpriteType.SPR_ARM_RING_3:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND7_1])
			s.set_behavior(Callable(AIProjectile, "ai_arm_ring_3"))

		# === ICONS ===
		Enums.SpriteType.SPR_APPLE_ICON:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOAPPLE1])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_icon"))
			s.nv_y = 5
			s.n_dest_x = s.n_x
			s.n_dest_y = s.n_y
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_APPLE
			s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 0

		Enums.SpriteType.SPR_PIZZA_ICON:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOPIZZA1])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_icon"))
			s.nv_y = 5
			s.n_dest_x = s.n_x
			s.n_dest_y = s.n_y
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_PIZZA
			s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 0

		Enums.SpriteType.SPR_CLARK_ICON:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOCLARK1])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_icon"))
			s.nv_y = 5
			s.n_dest_x = s.n_x
			s.n_dest_y = s.n_y
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_CLARK
			s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 0

		Enums.SpriteType.SPR_EXAM_ICON:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOEXAM1])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_icon"))
			s.nv_y = 5
			s.n_dest_x = s.n_x
			s.n_dest_y = s.n_y
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_EXAM
			s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 0

		Enums.SpriteType.SPR_RING_ICON:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICORING1])
			s.nv_y = 5
			s.nv_x = 5
			s.n_dest_x = s.n_x
			s.n_dest_y = s.n_y
			s.n_x = -40
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_RING
			s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 0
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_ring"), -40, s.n_y, s.n_dest_x, s.n_y, 1, 1)
			AIMethods.l_sound[Enums.ASLList.LSND_RING_DING].play(SoundbankInfo.VOL_HOLLAR, 0)

		# === DIGIT DISPLAYS ===
		Enums.SpriteType.SPR_APPLE_ONES:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_points_ones"))
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_APPLE

		Enums.SpriteType.SPR_APPLE_TENS:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_APPLE
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_points_tens"))

		Enums.SpriteType.SPR_CLARK_ONES:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_points_ones"))
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_CLARK

		Enums.SpriteType.SPR_CLARK_TENS:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_CLARK
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_points_tens"))

		Enums.SpriteType.SPR_EXAM_ONES:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_points_ones"))
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_EXAM

		Enums.SpriteType.SPR_EXAM_TENS:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_EXAM
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_points_tens"))

		Enums.SpriteType.SPR_PIZZA_ONES:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_points_ones"))
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_PIZZA

		Enums.SpriteType.SPR_PIZZA_TENS:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
			s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE] = Enums.Buttons.BUTTON_PIZZA
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_points_tens"))

		# === PIT TIME DISPLAY ===
		Enums.SpriteType.SPR_PITTIME_HONES:
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_pit_time_h_ones"))
		Enums.SpriteType.SPR_PITTIME_HTENS:
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_pit_time_h_tens"))
		Enums.SpriteType.SPR_PITTIME_MONES:
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_pit_time_m_ones"))
		Enums.SpriteType.SPR_PITTIME_MTENS:
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_pit_time_m_tens"))
		Enums.SpriteType.SPR_PITTIME_SONES:
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_pit_time_s_ones"))
		Enums.SpriteType.SPR_PITTIME_STENS:
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_pit_time_s_tens"))

		# === TAM ===
		Enums.SpriteType.SPR_TAM:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTAM0_1])
			s.set_behavior(Callable(AIMisc, "ai_tam"))
			s.n_attrib[Enums.AttrTam.ATTR_NAILS_IN_TAM] = 10 if 0 != Globals.myGameConditions.get_frosh_lameness() else 20
			s.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] = 0

		# === TRACKERS ===
		Enums.SpriteType.SPR_TRACKER_LR:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpWHITE_DOT])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_tracker_lr"))

		Enums.SpriteType.SPR_TRACKER_UL:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpWHITE_DOT])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_tracker_ul"))

		# === WHAP EFFECTS ===
		Enums.SpriteType.SPR_WHAP:
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_SMACK1 + R.randi() % SoundbankInfo.NSND_EFFECTS_SMACK].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpWHAP_1 + R.randi() % SoundbankInfo.NSPR_WHAP])
			s.set_behavior(Callable(AIProjectile, "ai_whap"))

		Enums.SpriteType.SPR_HOSE_WHAP:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpWHAP_1 + R.randi() % SoundbankInfo.NSPR_WHAP])
			s.set_behavior(Callable(AIProjectile, "ai_whap"))

		# === WORD BUBBLE ===
		Enums.SpriteType.SPR_GW_WORD_BUBBLE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPOWORDS1 + R.randi() % 6])
			s.set_behavior(Callable(AIProjectile, "ai_word_bubble"))

		# === HIGH SCORE ===
		Enums.SpriteType.SPR_HIGH_SCORE:
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_high_score"))

		# === POP BOY ===
		Enums.SpriteType.SPR_POP_BOY:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY1_1])
			s.set_behavior(Callable(AIFrosh, "ai_pop_boy"))
			s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 1
			s.n_attrib[Enums.NAttrFrosh.ATTR_MOTIVATION] = 0
			s.n_attrib[Enums.NAttrFrosh.ATTR_MIND_SET] = -1
			s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] = -1

		Enums.SpriteType.SPR_POP_BOY_IN_CROWD:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY1_2])
			s.set_behavior(Callable(AIFrosh, "ai_pop_boy_in_crowd"))

		# === TRI PUB BAN ===
		Enums.SpriteType.SPR_TRI:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTRI])
			s.nv_y = 5
			s.nv_x = 5
			s.n_dest_x = 100
			s.n_dest_y = 165
			s.n_x = -160
			s.n_y = s.n_dest_y
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ICONOUT].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMisc, "ai_inanimate"), s.n_x, s.n_y, s.n_dest_x, s.n_y, 1, 1)

		Enums.SpriteType.SPR_PUB:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPUB])
			s.nv_y = 5
			s.nv_x = 5
			s.n_dest_x = 320 - 15
			s.n_dest_y = 165
			s.n_x = s.n_dest_x
			s.n_y = -250
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ICONOUT].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMisc, "ai_inanimate"), s.n_x, s.n_y, s.n_x, s.n_dest_y, 1, 1)

		Enums.SpriteType.SPR_BAN:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpBAN])
			s.nv_y = 5
			s.nv_x = 5
			s.n_dest_x = 480 + 60 - 15
			s.n_dest_y = 165
			s.n_x = 800
			s.n_y = s.n_dest_y
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ICONOUT].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMisc, "ai_inanimate"), s.n_x, s.n_y, s.n_dest_x, s.n_y, 1, 1)

		# === ACHIEVEMENT UNLOCKED ===
		Enums.SpriteType.SPR_ACHIEVEMENT_UNLOCKED, Enums.SpriteType.SPR_ACHIEVEMENT_UNLOCKED_TEXT1, \
		Enums.SpriteType.SPR_ACHIEVEMENT_UNLOCKED_TEXT2:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpACHIEVEMENTUNLOCKED])
			# Text color: white
			s.n_r = 255
			s.n_g = 255
			s.n_b = 255
			s.nv_y = 1
			s.nv_x = 1
			s.n_dest_x = 150
			s.n_dest_y = 480 - 90
			s.n_x = -200
			s.n_y = s.n_dest_y
			if type != Enums.SpriteType.SPR_ACHIEVEMENT_UNLOCKED:
				s.n_dest_y += 13
				s.n_y += 13
				s.n_x += 60
				s.n_dest_x += 60
			if type == Enums.SpriteType.SPR_ACHIEVEMENT_UNLOCKED_TEXT2:
				s.n_dest_y += 22
				s.n_y += 22
				s.n_x += 10
				s.n_dest_x += 10
			if type == Enums.SpriteType.SPR_ACHIEVEMENT_UNLOCKED:
				AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ACHIEVEMENTUNLOCKED].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_achievement_unlocked_notice"), s.n_x, s.n_y - 400, s.n_dest_x, s.n_dest_y, 1, 1)


# =============================================================================
# TRANSITION SPRITES
# =============================================================================

static func _init_trans_sprite(s: TSprite, type: int):
	match type:
		Enums.SpriteType.SPR_SLAM_JACKET:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSLAMJACKET1])
			s.set_behavior(Callable(AIMenuAndDisplay, "ai_slam_jacket"))
			s.b_super_front = true  # Draw on top of everything including text
