class_name AIBackground

# aiBackground.gd - Static class with background element behaviors
# Ported from aiBackground.cs
#
# Per porting_rules.md:
# - This is a static class with logic functions only
# - All shared state accessed via AIMethods autoload
# - Cross-file calls use class qualification

# === CONSTANTS ===
const BALLOON_WIDTH: int = 100
const D_BALLOON_OFFSET: int = 20
const D_NUM_BALLOON_OFFSET_SHIFTS: int = 20
const PREZ_LEFT: int = 175
const PREZ_RIGHT: int = 240

# Balloon offset shifts for bobbing animation
const D_BALLOON_OFFSET_SHIFTS: Array[int] = [
	0, 5, 8, 10, 11, 11, 11, 10, 8, 5,
	0, -5, -8, -10, -11, -11, -11, -10, -8, -5
]

# Iron Ring forge sequences [3][10]
const IR_SEQUENCES: Array = [
	[Enums.GameBitmapEnumeration.bmpFORGE1_1, Enums.GameBitmapEnumeration.bmpFORGE1_2,
	 Enums.GameBitmapEnumeration.bmpFORGE1_3, Enums.GameBitmapEnumeration.bmpFORGE1_4,
	 Enums.GameBitmapEnumeration.bmpFORGEDOWN, Enums.GameBitmapEnumeration.bmpFORGE2_5,
	 Enums.GameBitmapEnumeration.bmpFORGE2_4, Enums.GameBitmapEnumeration.bmpFORGE2_3,
	 Enums.GameBitmapEnumeration.bmpFORGE2_2, Enums.GameBitmapEnumeration.bmpFORGE1_1],
	[Enums.GameBitmapEnumeration.bmpFORGE1_1, Enums.GameBitmapEnumeration.bmpFORGE2_2,
	 Enums.GameBitmapEnumeration.bmpFORGE2_3, Enums.GameBitmapEnumeration.bmpFORGE2_4,
	 Enums.GameBitmapEnumeration.bmpFORGEDOWN, Enums.GameBitmapEnumeration.bmpFORGE3_5,
	 Enums.GameBitmapEnumeration.bmpFORGE3_4, Enums.GameBitmapEnumeration.bmpFORGE3_3,
	 Enums.GameBitmapEnumeration.bmpFORGE3_2, Enums.GameBitmapEnumeration.bmpFORGE1_1],
	[Enums.GameBitmapEnumeration.bmpFORGE1_1, Enums.GameBitmapEnumeration.bmpFORGE3_2,
	 Enums.GameBitmapEnumeration.bmpFORGE3_3, Enums.GameBitmapEnumeration.bmpFORGE3_4,
	 Enums.GameBitmapEnumeration.bmpFORGEDOWN, Enums.GameBitmapEnumeration.bmpFORGE4_5,
	 Enums.GameBitmapEnumeration.bmpFORGE4_4, Enums.GameBitmapEnumeration.bmpFORGE4_3,
	 Enums.GameBitmapEnumeration.bmpFORGE4_2, Enums.GameBitmapEnumeration.bmpFORGE1_1]
]


# === FREC GROUP INIT ===

static func ai_frec_group_init(s: TSprite) -> void:
	s.n_attrib[Enums.NAttrCrowd.ATTR_F_ACTION] = Enums.CrowdActions.FA_MILLING
	s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = AIDefine.ENERGY_START
	s.n_attrib[Enums.NAttrCrowd.ATTR_F_CAN_START_WAVE] = 1 if s.n_x > 320 else 0
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Milling1])
	s.set_behavior(Callable(AICrowd, "ai_frec_group"))


static func ai_frec_action_init(s: TSprite) -> void:
	s.n_attrib[Enums.NAttrCrowd.ATTR_F_ACTION] = Enums.CrowdActions.FA_MILLING
	s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = AIDefine.ENERGY_START
	s.n_attrib[Enums.NAttrCrowd.ATTR_F_CAN_START_WAVE] = 0
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Milling1])
	if 0 != Globals.myGameConditions.get_enhanced_graphics():
		s.set_behavior(Callable(AICrowd, "ai_frec_action"))
	else:
		s.set_behavior(Callable(AIMisc, "ai_inanimate"))


# === SCICON INIT ===

static func ai_scicon_m_init(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONM1])
	s.set_behavior(Callable(AIBackground, "ai_scicon_m"))


static func ai_scicon_m(s: TSprite) -> void:
	pass


static func ai_scicon_f_init(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONF1])
	s.set_behavior(Callable(AIBackground, "ai_scicon_f"))


static func ai_scicon_f(s: TSprite) -> void:
	pass


# === UPPER YEAR ===

static func ai_upper_year_init(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY1_1])
	s.set_behavior(Callable(AIBackground, "ai_upper_year"))


static func ai_upper_year(s: TSprite) -> void:
	pass


# === POOF EFFECT ===

static func ai_poof_init(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOOF1])
	s.set_behavior(Callable(AIBackground, "ai_poof"))


static func ai_poof(s: TSprite) -> void:
	if s.n_cc == 3:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOOF2])
	if s.n_cc == 5:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOOF3])
	if s.n_cc == 7:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOOF4])
	if s.n_cc == 9:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOOF1])
	if s.n_cc == 10:
		s.b_deleted = true


# === GW BALLOON ===

static func ai_gw_balloon_init(s: TSprite) -> void:
	# Pop Boy announces the balloon's arrival (with varying levels of couthness)
	if Globals.myGameConditions.is_pop_boy_in_pit():
		if Globals.myGameConditions.is_ritual():
			AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_HIPPOR1].play(SoundbankInfo.VOL_HOLLAR)
		else:
			AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_HIPPO1].play(SoundbankInfo.VOL_HOLLAR)

	if 0 != AIMethods.R.randi() % 2:
		AICrowd.ais_set_frec_action(AIMethods.spr_frecs_l, Enums.CrowdActions.FA_LOOK_ULR)
		AIMethods.spr_frecs_l.n_cc -= 120
		AICrowd.ais_set_frec_action(AIMethods.spr_frecs_c, Enums.CrowdActions.FA_LOOK_ULR)
		AIMethods.spr_frecs_c.n_cc -= 100
		AICrowd.ais_set_frec_action(AIMethods.spr_frecs_r, Enums.CrowdActions.FA_LOOK_ULR)
		s.n_attrib[0] = 0
		s.n_x = 640 + BALLOON_WIDTH
		s.n_y = 0
		s.n_z = 25
		s.nv_x = -2
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGWBALLOONL])
	else:
		AICrowd.ais_set_frec_action(AIMethods.spr_frecs_l, Enums.CrowdActions.FA_LOOK_URL)
		AICrowd.ais_set_frec_action(AIMethods.spr_frecs_c, Enums.CrowdActions.FA_LOOK_URL)
		AIMethods.spr_frecs_c.n_cc -= 100
		AICrowd.ais_set_frec_action(AIMethods.spr_frecs_r, Enums.CrowdActions.FA_LOOK_URL)
		AIMethods.spr_frecs_r.n_cc -= 120
		s.n_attrib[0] = 1
		s.n_x = 0 - BALLOON_WIDTH
		s.n_y = 0
		s.n_z = 25
		s.nv_x = 2
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGWBALLOONR])
	s.set_behavior(Callable(AIBackground, "ai_gw_balloon"))


static func ai_gw_balloon(s: TSprite) -> void:
	if s.n_cc > 100 and (s.n_x < 0 - BALLOON_WIDTH or s.n_x > 640 + BALLOON_WIDTH):
		s.b_deleted = true
		AIMethods.spr_gw_balloon = null

	if s.n_cc == 125:
		AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_KABOOM].play(SoundbankInfo.VOL_HOLLAR)
		if 0 != s.n_attrib[0]:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGWBALLOONRBANG])
		else:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGWBALLOONLBANG])
		if AIMethods.spr_gw_hippo == null:
			Globals.Analytic("HippoLaunched")
			AIMethods.spr_gw_hippo = SpriteInit.create_sprite(Enums.SpriteType.SPR_GW_HIPPO, s.n_x, AIDefine.D_POLE_Y + 50)
			AIMethods.spr_gw_hippo.n_z = s.n_z - AIMethods.d_sky_y_to_pit_y() + 30 + AIDefine.D_POLE_Y + 50
			AIMethods.ss_pit.include(AIMethods.spr_gw_hippo)

	if s.n_cc == 150:
		if 0 != s.n_attrib[0]:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGWBALLOONR])
		else:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGWBALLOONL])

	s.n_x += s.nv_x


# === GW HIPPO ===

static func ai_gw_hippo_init(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO0_1])
	s.set_behavior(Callable(AIBackground, "ai_gw_hippo"))
	s.n_attrib[0] = 0
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 20


static func ai_gw_hippo(s: TSprite) -> void:
	match s.n_attrib[0]:
		0:  # In air
			if s.n_z < 0:
				AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_L, s.n_x, s.n_y))
				s.n_z = 0
				s.nv_x = 16
				s.nv_y = 4
				s.n_dest_x = AIMethods.randintin(AIDefine.D_PIT_MIN_X, AIDefine.D_PIT_MAX_X - 80)
				s.n_dest_y = AIMethods.randintin(AIDefine.D_POLE_Y, AIDefine.D_PIT_MAX_Y - 30)
				s.n_attrib[0] = 1
				s.n_cc = 0
			else:
				AISupport.ais_plummet(s)
				if s.n_cc == 7:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO0_2])
				if s.n_cc >= 10 and 0 == (s.n_cc % 3):
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO1_1 + AIMethods.R.randi() % 3])

		1:  # Wade towards a target in the pit
			if not (abs(s.n_x - s.n_dest_x) <= s.nv_x and abs(s.n_y - s.n_dest_y) <= s.nv_y):
				if s.n_cc < 20:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO2_1] if s.n_dest_x > s.n_x else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO2_1])
				elif s.n_cc < 50:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO2_2] if s.n_dest_x > s.n_x else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO2_2])
				else:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO2_3] if s.n_dest_x > s.n_x else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO2_3])
				s.n_attrib[Enums.AttrProjectile.ATTR_POWER_OF_THROW] = 100
				AISupport.ais_collision_to_response(s, AIMethods.ss_fr, Callable(AISupport, "ais_send_frosh_really_flying"),
					AIDefine.NOWHAP, AIDefine.NOPOLESHIELDING, false)
				if 0 == (s.n_cc % 4):
					AISupport.ais_move_towards_destination(s)
			else:
				if s.n_dest_x == AIDefine.D_POLE_X and s.n_dest_y == AIDefine.D_POLE_Y:
					s.n_attrib[0] = 2
				else:
					if 0 == AIMethods.R.randi() % 4 or s.n_cc > 250:
						s.n_dest_x = AIDefine.D_POLE_X
						s.n_dest_y = AIDefine.D_POLE_Y
					else:
						s.n_cc = 0
						s.n_attrib[0] = 11
						AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_GW_WORD_BUBBLE,
							s.n_scr_x + s.frm_frame.hotspot_x + 20,
							s.n_scr_y + s.frm_frame.hotspot_y - 60))
						s.n_dest_x = AIMethods.randintin(AIDefine.D_PIT_MIN_X + 30, AIDefine.D_PIT_MAX_X - 80)
						s.n_dest_y = AIMethods.randintin(AIDefine.D_POLE_Y, AIDefine.D_PIT_MAX_Y - 30)

		11:  # WORD BUBBLE DELAY
			if s.n_cc > 40:
				s.n_attrib[0] = 1
				s.n_cc = 400

		2:  # CLIMB THE POLE
			if 0 == (s.n_cc % 4):
				s.n_x = AIDefine.D_POLE_X - 1 + AIMethods.R.randi() % 3
			s.n_y = AIDefine.D_POLE_Y + 1
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO3_1] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO3_1])
			if s.n_z < AIDefine.D_TAM_Z + 110:
				if 0 == (s.n_cc % 5):
					s.n_z += 30 + AIMethods.R.randi() % 10
			else:
				s.n_attrib[0] = 3
				s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = false
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO3_2] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO3_2])
				s.n_z = AIDefine.D_TAM_Z + 140
				s.n_y = AIDefine.D_POLE_Y + 2

		3:  # Sitting on tam
			if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED]:
				s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = false
				s.n_cc = 0
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO4_2] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO4_2])
				s.n_attrib[0] = 31
				AIMisc.ais_unlock_achievement(96)
				AISupport.ais_forge_trick(0, 2500)  # Hit Hippo with Stuff
			elif 0 == (s.n_cc % 10) and 0 == AIMethods.R.randi() % 5:
				if 0 == AIMethods.R.randi() % 6:
					s.n_attrib[0] = 4
					s.n_cc = 0
					var temp: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_GW_WORD_BUBBLE, s.n_scr_x + s.frm_frame.hotspot_x + 60, s.n_scr_y + 60)
					temp.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPOWORDS1])
					if s.n_x < AIDefine.D_POLE_X:
						s.nv_x = 7
					else:
						s.nv_x = -7
					s.nv_y = 0
					s.nv_z = 7
					AIMethods.ss_icons.include(temp)
				else:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO3_2 + AIMethods.R.randi() % 2] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO3_2 + AIMethods.R.randi() % 2])

		31:  # Recovering from hit
			if s.n_cc > 5:
				if 0 == (s.n_cc % 3):
					match (s.n_cc % 24) % 8:
						0: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO3_2] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO3_2])
						1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO4_1] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO4_1])
						2: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO3_2] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO3_2])
						3: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO4_1] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO4_1])
						4: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO3_3] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO3_3])
						5: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO4_2] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO4_2])
						6: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO3_3] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO3_3])
						7: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO4_2] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO4_2])
				if s.n_cc > 50:
					s.n_attrib[0] = 3
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO3_3] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO3_3])

		4:  # Jeronimo (jumping off)
			if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED]:
				s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = false
				s.n_cc = 0
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO4_2] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO4_2])
				s.n_attrib[0] = 31

			if s.n_cc > 30:
				AISupport.ais_plummet(s)
				if s.n_z < 0:
					s.b_deleted = true
					AIMethods.spr_gw_hippo = null
				if s.n_cc >= 10 and 0 == (s.n_cc % 3):
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHIPPO1_1 + AIMethods.R.randi() % 3] if s.n_x < AIDefine.D_POLE_X else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpHIPPO1_1 + AIMethods.R.randi() % 3])


# === PREZ (Engineering Society President) ===

static func ai_prez_init(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ1_2])
	s.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 1
	s.nv_x = -2
	s.set_behavior(Callable(AIBackground, "ai_prez"))


static func ai_prez(s: TSprite) -> void:
	match s.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION]:
		1:  # Chillin'
			if s.n_cc > 15 and (0 == AIMethods.R.randi() % 100):
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ1_1 + AIMethods.R.randi() % 3])
			if s.n_cc > 40 and (0 == AIMethods.R.randi() % 500):
				s.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 3
				s.n_cc = 0
			if s.n_cc > 40 and (0 == AIMethods.R.randi() % 500):
				s.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 2
				s.n_cc = 0
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ2_1 + AIMethods.R.randi() % 3])

		2:  # Motioning
			if s.n_cc > 15 and (0 == AIMethods.R.randi() % 50):
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ2_1 + AIMethods.R.randi() % 3])
			if s.n_cc > 40 and (0 == AIMethods.R.randi() % 50):
				s.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 1
				s.n_cc = 0
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ1_1 + AIMethods.R.randi() % 3])

		3:  # Walkin'
			s.n_x += s.nv_x
			if s.n_cc == 1 or 0 == (s.n_cc % 10):
				if s.nv_x < 0:
					s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpPREZ3_1 + (1 if 0 == (s.n_cc % 20) else 0)])
				else:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ3_1 + (1 if 0 == (s.n_cc % 20) else 0)])

			if (s.nv_x < 0 and s.n_x < PREZ_LEFT) or (s.nv_x > 0 and s.n_x > PREZ_RIGHT):
				s.nv_x = -s.nv_x
				if s.nv_x < 0:
					s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpPREZ3_1 + (1 if 0 == (s.n_cc % 20) else 0)])
				else:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ3_1 + (1 if 0 == (s.n_cc % 20) else 0)])

			if (s.n_cc > 15 and (0 == AIMethods.R.randi() % 300)) or s.n_cc > 80:
				s.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 1
				s.n_cc = 0
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ1_1 + AIMethods.R.randi() % 3])

		4:  # Speaking
			if s.n_cc < 10000:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ4_1 + AIMethods.R.randi() % 2])
				s.n_cc = 10000
			if 0 == (s.n_cc % 24) and 0 != AIMethods.R.randi() % 4:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ4_1 + AIMethods.R.randi() % 2])
			if s.n_cc > 10075:
				s.n_cc = 0
				s.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 1
				s.n_cc = 0
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ1_1 + AIMethods.R.randi() % 3])


# === FORGE (Iron Ring Forge) ===

static func ai_forge_init(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	s.n_attrib[Enums.NAttrForge.ATTR_FORGE_SWING] = 0
	s.n_attrib[Enums.NAttrForge.ATTR_FORGE_MOTION] = 0
	s.set_behavior(Callable(AIBackground, "ai_forge"))


static func ai_forge(s: TSprite) -> void:
	match s.n_attrib[Enums.NAttrForge.ATTR_FORGE_MOTION]:
		0:  # Forge is not on screen
			if s.n_attrib[Enums.NAttrForge.ATTR_FORGE_ENERGY] >= AIDefine.ENERGY_SWING:
				s.n_attrib[Enums.NAttrForge.ATTR_FORGE_ENERGY] = 0
				s.n_attrib[Enums.NAttrForge.ATTR_FORGE_MOTION] = 1
				s.n_cc = 0

		1:  # Forge is swinging
			var swing_idx: int = s.n_attrib[Enums.NAttrForge.ATTR_FORGE_SWING]
			match s.n_cc:
				1:
					s.set_frame(AIMethods.frm[IR_SEQUENCES[swing_idx][0]])
					AIMethods.l_sound[Enums.ASLList.LSND_RING_SWING].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				4:
					s.set_frame(AIMethods.frm[IR_SEQUENCES[swing_idx][1]])
				7:
					s.set_frame(AIMethods.frm[IR_SEQUENCES[swing_idx][2]])
				10:
					s.set_frame(AIMethods.frm[IR_SEQUENCES[swing_idx][3]])
				29:
					s.set_frame(AIMethods.frm[IR_SEQUENCES[swing_idx][4]])
					AIMethods.l_sound[Enums.ASLList.LSND_RING_PRESS].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				52:
					s.set_frame(AIMethods.frm[IR_SEQUENCES[swing_idx][5]])
					AIMethods.l_sound[Enums.ASLList.LSND_RING_DING].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				55:
					s.set_frame(AIMethods.frm[IR_SEQUENCES[swing_idx][6]])
				91:
					s.set_frame(AIMethods.frm[IR_SEQUENCES[swing_idx][7]])
					AIMethods.l_sound[Enums.ASLList.LSND_RING_SWING].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
				94:
					s.set_frame(AIMethods.frm[IR_SEQUENCES[swing_idx][8]])
				97:
					s.set_frame(AIMethods.frm[IR_SEQUENCES[swing_idx][9]])
				100:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
					var n_tmp: int = 0
					s.n_attrib[Enums.NAttrForge.ATTR_FORGE_SWING] += 1
					s.n_attrib[Enums.NAttrForge.ATTR_FORGE_SWING] %= 3
					s.n_attrib[Enums.NAttrForge.ATTR_FORGE_MOTION] = 0

					if 0 == s.n_attrib[Enums.NAttrForge.ATTR_FORGE_SWING]:
						# The player gets themselves an iron ring!
						while not Globals.myGameConditions.is_ring_spot_open(n_tmp):
							n_tmp += 1
						Globals.myGameConditions.take_ring_spot(n_tmp)
						if n_tmp != 0:
							AIMisc.ais_unlock_achievement(7777)
						var s_tmp: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_RING_ICON, 36 + (40 * (n_tmp / 4)), 320 + (42 * (n_tmp % 4)))
						s_tmp.n_attrib[0] = n_tmp
						Globals.myGameConditions.get_ring(1)
						AIMethods.ss_icons.include(s_tmp)
