class_name AIPopUp

# aiPopUp.gd - Static class with popup character behaviors
# Ported from aiPopUp.cs
#
# Per porting_rules.md:
# - This is a static class with logic functions only
# - All shared state accessed via AIMethods autoload
# - Cross-file calls use class qualification

# === CONSTANTS ===
const SCREENBOTTOM: int = 205

const MOREAPPLESEASY: int = 12
const MOREAPPLESHARD: int = 10
const MOREPIZZAEASY: int = 10
const MOREPIZZAHARD: int = 5
const MORECLARKEASY: int = 10
const MORECLARKHARD: int = 5

const TIME_FALLING_FRAMES: int = 20
const TIME_TPBM: int = 45
const TIME_TPBM_PUB: int = 60   # TIME_TPBM + 15 - when PUB icon appears
const TIME_TPBM_BAN: int = 75   # TIME_TPBM + 30 - when BAN icon appears


# === ACHIEVEMENT CHECK ===

static func ais_check_for_generalist_achievement() -> void:
	if Globals.myGameConditions.get_player_apples() > 0 \
		and Globals.myGameConditions.get_player_clark() > 0 \
		and Globals.myGameConditions.get_player_pizza() > 0 \
		and Globals.myGameConditions.get_player_exam() > 0:
		AIMisc.ais_unlock_achievement(404)


# === GET ITEMS (Pickup behaviors) ===

static func ai_get_apples(s: TSprite) -> void:
	if s.n_cc == 1:
		AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_SNATCH1].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
	elif s.n_cc == 5 \
		and not (AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] == Enums.ArmPositions.ARM_CHANGING \
			and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] == Enums.ArmPositions.ARM_IRON_RING) \
		and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING:
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_APPLE
		AIMethods.spr_arm.n_cc = 0
	elif s.n_cc >= MOREAPPLESEASY or (s.n_cc >= MOREAPPLESHARD and 0 != Globals.myGameConditions.get_frosh_lameness()):
		s.b_deleted = true

	s.n_y += s.n_cc + Globals.myGameConditions.get_frosh_lameness()
	s.n_x += (AIMethods.spr_arm.n_x - s.n_x) / 5

	var spr_bounce: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_APPLE, s.n_x, s.n_y)
	spr_bounce.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_REBOUNDING
	spr_bounce.nv_x = AIMethods.randintin(-8, 8)
	spr_bounce.nv_y = 0
	spr_bounce.nv_z = AIMethods.R.randi() % 30
	Globals.myGameConditions.get_apples(1)
	if Globals.myGameConditions.get_player_apples() >= 99:
		AIMisc.ais_unlock_achievement(399)
	AIMethods.ss_pit.include(spr_bounce)
	ais_check_for_generalist_achievement()


static func ai_get_pizza(s: TSprite) -> void:
	if s.n_cc == 1:
		AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_SNATCH1].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
	elif s.n_cc == 5 \
		and not (AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] == Enums.ArmPositions.ARM_CHANGING \
			and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] == Enums.ArmPositions.ARM_IRON_RING) \
		and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING:
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_PIZZA
		AIMethods.spr_arm.n_cc = 0
	elif s.n_cc >= MOREPIZZAEASY or (s.n_cc >= MOREPIZZAHARD and 0 != Globals.myGameConditions.get_frosh_lameness()):
		s.b_deleted = true

	s.n_y += s.n_cc + Globals.myGameConditions.get_frosh_lameness()
	s.n_x += (AIMethods.spr_arm.n_x - s.n_x) / 5

	var spr_bounce: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_PIZZA, s.n_x, s.n_y)
	spr_bounce.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_REBOUNDING
	spr_bounce.nv_x = AIMethods.randintin(-8, 8)
	spr_bounce.nv_y = 0
	spr_bounce.nv_z = AIMethods.R.randi() % 30
	Globals.myGameConditions.get_pizzas(1)
	if Globals.myGameConditions.get_player_pizza() >= 99:
		AIMisc.ais_unlock_achievement(399)
	AIMethods.ss_pit.include(spr_bounce)
	ais_check_for_generalist_achievement()


static func ai_get_clark(s: TSprite) -> void:
	if s.n_cc == 1:
		AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_SNATCH1].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
	elif s.n_cc == 5 \
		and not (AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] == Enums.ArmPositions.ARM_CHANGING \
			and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] == Enums.ArmPositions.ARM_IRON_RING) \
		and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING:
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_CLARK
		AIMethods.spr_arm.n_cc = 0
	elif s.n_cc >= MORECLARKEASY or (s.n_cc >= MORECLARKHARD and 0 != Globals.myGameConditions.get_frosh_lameness()):
		s.b_deleted = true

	s.n_y += s.n_cc + Globals.myGameConditions.get_frosh_lameness()
	s.n_x += (AIMethods.spr_arm.n_x - s.n_x) / 5

	var spr_bounce: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_CLARK, s.n_x, s.n_y)
	spr_bounce.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_REBOUNDING
	spr_bounce.nv_x = AIMethods.randintin(-8, 8)
	spr_bounce.nv_y = 0
	spr_bounce.nv_z = AIMethods.R.randi() % 30
	Globals.myGameConditions.get_clarks(1)
	if Globals.myGameConditions.get_player_clark() >= 99:
		AIMisc.ais_unlock_achievement(399)
	AIMethods.ss_pit.include(spr_bounce)
	ais_check_for_generalist_achievement()


static func ai_get_exam(s: TSprite) -> void:
	if s.n_cc == 1:
		Globals.myGameConditions.get_exams(1)
		AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_SNATCH1].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
	elif s.n_cc == 5 \
		and not (AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] == Enums.ArmPositions.ARM_CHANGING \
			and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] == Enums.ArmPositions.ARM_IRON_RING) \
		and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING:
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_EXAM
		AIMethods.spr_arm.n_cc = 0
	elif s.n_cc >= 20:
		s.b_deleted = true

	s.n_y += s.n_cc + 2 + Globals.myGameConditions.get_frosh_lameness()
	s.n_x += (AIMethods.spr_arm.n_x - s.n_x) / 5
	ais_check_for_generalist_achievement()


static func ai_get_hose(s: TSprite) -> void:
	AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_SNATCH1].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))

	if not (AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] == Enums.ArmPositions.ARM_CHANGING \
		and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] == Enums.ArmPositions.ARM_IRON_RING) \
		and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING:
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
	AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_GREASE
	AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_KICKBACK] = 0
	AIMethods.spr_arm.n_cc = 0
	s.b_deleted = true

	AIMethods.spr_water_meter = SpriteInit.create_sprite(Enums.SpriteType.SPR_WATER_METER, 26, 17)
	AIMethods.ss_console.include(AIMethods.spr_water_meter)


# === ALIEN IN PIT (shared helper) ===

static func ais_alien_in_pit(s: TSprite, n_first_frame: int, n_animation_bitmaps: int) -> void:
	s.n_z = 0
	var n_total_cycled_frames: int = n_animation_bitmaps * 2 - 1
	var n_current_frame: int = (s.n_cc / 5) % n_total_cycled_frames
	var n_current_bitmap: int
	if n_current_frame < n_animation_bitmaps:
		n_current_bitmap = n_current_frame
	else:
		n_current_bitmap = n_total_cycled_frames - n_current_frame - 1

	if s.n_cc < 50:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	else:
		if s.n_x < AIDefine.D_POLE_X:
			s.set_frame(AIMethods.frm_m[n_first_frame + n_current_bitmap])
			s.n_x -= 2
		else:
			s.set_frame(AIMethods.frm[n_first_frame + n_current_bitmap])
			s.n_x += 2
		if s.n_x < -60 or s.n_x > 640 + 60:
			if s == AIMethods.spr_alien:
				AIMethods.spr_alien = null  # DO THIS WHENEVER THE ALIEN IS DELETED
			s.b_deleted = true


# === ARTSCI BEHAVIORS ===

static func ai_artsci_m_in_pit(s: TSprite) -> void:
	if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED]:
		if 0 == AIMethods.R.randi() % 40:
			s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = false
		else:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpARTSCIM_HIT1 + ((s.n_cc / 40) % 2)])
			if 0 == (s.n_cc % 40):
				if Globals.myGameConditions.is_ritual():
					AIMethods.l_sound[Enums.ASLList.LSND_ARTSCI_MALE_HIT1 + ((s.n_cc / 40) % 8)].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				else:
					AIMethods.l_sound[Enums.ASLList.LSND_ARTSCI_MALE_HIT1 + ((s.n_cc / 40) % 5)].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
	else:
		ais_alien_in_pit(s, Enums.GameBitmapEnumeration.bmpARTSCIM_WADE1, SoundbankInfo.NSPR_ARTSCIM_WADE)


static func ai_artsci_f_in_pit(s: TSprite) -> void:
	if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED]:
		if 0 == AIMethods.R.randi() % 40:
			s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = false
		else:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpARTSCIF_HIT1])
			if 0 == (s.n_cc % 40):
				AIMethods.l_sound[Enums.ASLList.LSND_ARTSCI_FEMALE_HIT1 + ((s.n_cc / 40) % 5)].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
	else:
		ais_alien_in_pit(s, Enums.GameBitmapEnumeration.bmpARTSCIF_WADE1, SoundbankInfo.NSPR_ARTSCIF_WADE)


# === COMMIE BEHAVIORS ===

static func ai_commie_m_in_pit(s: TSprite) -> void:
	if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED]:
		if 0 == AIMethods.R.randi() % 40:
			s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = false
		else:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCOMMIEM_HIT1])
			if 0 == (s.n_cc % 50):
				AIMethods.l_sound[Enums.ASLList.LSND_COMMIE_MALE_HIT1 + ((s.n_cc / 50) % 7)].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
	else:
		ais_alien_in_pit(s, Enums.GameBitmapEnumeration.bmpCOMMIEM_WADE1, SoundbankInfo.NSPR_COMMIEM_WADE)


static func ai_commie_f_in_pit(s: TSprite) -> void:
	if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED]:
		if 0 == AIMethods.R.randi() % 40:
			s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = false
		else:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCOMMIEF_HIT1 + ((s.n_cc / 40) % 2)])
			if 0 == (s.n_cc % 75):
				AIMethods.l_sound[Enums.ASLList.LSND_COMMIE_FEMALE_HIT1 + ((s.n_cc / 75) % 4)].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
	else:
		ais_alien_in_pit(s, Enums.GameBitmapEnumeration.bmpCOMMIEF_WADE1, SoundbankInfo.NSPR_COMMIEF_WADE)


# === PUSH INTO PIT ===

static func ais_push_into_pit(s: TSprite, n_first_frame: int, n_animation_bitmaps: int) -> void:
	AISupport.ais_forge_trick(8, 200)  # Push Alien into pit first time
	AIMisc.ais_unlock_achievement(4)
	Globals.myGameConditions.add_energy(50)

	if s.n_cc <= 10:
		if s.n_cc > 4:
			s.set_frame(AIMethods.frm[n_first_frame])
		s.n_y = SCREENBOTTOM
		s.nv_z = 25
		s.nv_x = -3
		s.nv_y = -1
		if s.n_cc == 1:
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_PUSH1].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
		s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = false
	else:
		AISupport.ais_plummet(s)
		if s.nv_z > -10:
			s.set_frame(AIMethods.frm[n_first_frame + 1])
		else:
			s.set_frame(AIMethods.frm[n_first_frame + 2])


static func ai_push_artsci_m_into_pit(s: TSprite) -> void:
	AIMisc.ais_unlock_achievement(4)
	if s.n_cc == 1:
		for i in range(7):
			AIMethods.l_sound[Enums.ASLList.LSND_ARTSCI_MALE_TAUNT1 + i].stop()
		AIMethods.l_sound[Enums.ASLList.LSND_ARTSCI_MALE_TAUNTR1].stop()
		AIMethods.l_sound[Enums.ASLList.LSND_ARTSCI_MALE_PUSH1 + AIMethods.R.randi() % 4].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))

	ais_push_into_pit(s, Enums.GameBitmapEnumeration.bmpARTSCIM_FALL1, SoundbankInfo.NSPR_ARTSCIM_FALL)
	if s.n_z <= 0 and s.nv_z < 0:
		AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_L, s.n_x, s.n_y))
		s.set_behavior(Callable(AIPopUp, "ai_artsci_m_in_pit"))
		AISupport.ais_chase_alien(true)


static func ai_push_artsci_f_into_pit(s: TSprite) -> void:
	AIMisc.ais_unlock_achievement(4)
	if s.n_cc == 1:
		for i in range(6):
			AIMethods.l_sound[Enums.ASLList.LSND_ARTSCI_FEMALE_TAUNT1 + i].stop()
		AIMethods.l_sound[Enums.ASLList.LSND_ARTSCI_FEMALE_PUSH1].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))

	ais_push_into_pit(s, Enums.GameBitmapEnumeration.bmpARTSCIF_FALL1, SoundbankInfo.NSPR_ARTSCIF_FALL)
	if s.n_z <= 0 and s.nv_z < 0:
		AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_L, s.n_x, s.n_y))
		s.set_behavior(Callable(AIPopUp, "ai_artsci_f_in_pit"))
		AISupport.ais_chase_alien(false)


static func ai_push_commie_m_into_pit(s: TSprite) -> void:
	AIMisc.ais_unlock_achievement(4)
	if s.n_cc == 1:
		for i in range(2):
			AIMethods.l_sound[Enums.ASLList.LSND_COMMIE_MALE_TAUNT1 + i].stop()
		AIMethods.l_sound[Enums.ASLList.LSND_COMMIE_MALE_TAUNTR1].stop()
		AIMethods.l_sound[Enums.ASLList.LSND_COMMIE_MALE_TAUNTR2].stop()
		AIMethods.l_sound[Enums.ASLList.LSND_COMMIE_MALE_TAUNTR3].stop()
		AIMethods.l_sound[Enums.ASLList.LSND_COMMIE_MALE_PUSH1 + AIMethods.R.randi() % 4].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))

	ais_push_into_pit(s, Enums.GameBitmapEnumeration.bmpCOMMIEM_FALL1, SoundbankInfo.NSPR_COMMIEM_FALL)
	if s.n_z <= 0 and s.nv_z < 0:
		AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_L, s.n_x, s.n_y))
		s.set_behavior(Callable(AIPopUp, "ai_commie_m_in_pit"))
		AISupport.ais_chase_alien(true)


static func ai_push_commie_f_into_pit(s: TSprite) -> void:
	AIMisc.ais_unlock_achievement(4)
	if s.n_cc == 1:
		for i in range(3):
			AIMethods.l_sound[Enums.ASLList.LSND_COMMIE_FEMALE_TAUNT1 + i].stop()
		AIMethods.l_sound[Enums.ASLList.LSND_COMMIE_FEMALE_PUSH1].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))

	ais_push_into_pit(s, Enums.GameBitmapEnumeration.bmpCOMMIEF_FALL1, SoundbankInfo.NSPR_COMMIEF_FALL)
	if s.n_z <= 0 and s.nv_z < 0:
		AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_L, s.n_x, s.n_y))
		s.set_behavior(Callable(AIPopUp, "ai_commie_f_in_pit"))
		AISupport.ais_chase_alien(true)


# === SCICON PUSH (Tri Pub Ban!) ===

static func ai_push_scicon_m(s: TSprite) -> void:
	match s.n_cc:
		1:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONM2])
			AIMethods.NOSPEECHFOR(450)
			for i in range(Enums.ASLList.LSND_APPLES_OFFER1, Enums.ASLList.LSND_RING_ZAP3):
				AIMethods.l_sound[i].stop()
			AIMethods.l_sound[Enums.ASLList.LSND_SCICONM_HIT_MISC].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
		25:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONM3])
			AIMisc.ais_unlock_achievement(11)
		45:
			Globals.myGameConditions.gb_tri_pub_ban = true
			AIMethods.l_sound[Enums.ASLList.LSND_FRECS_HITAPPLE2].play(SoundbankInfo.VOL_HOLLAR)
			AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_TRI))
		TIME_TPBM_PUB:
			AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PUB))
		TIME_TPBM_BAN:
			AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_BAN))
		136:
			Globals.GameLoop.change_game_state(Enums.GameStates.STATETITLE)
			if Globals.myGameConditions.is_ritual():
				AIMethods.l_sound[Enums.ASLList.LSND_FRECS_REWARDR1].play(SoundbankInfo.VOL_HOLLAR)
			else:
				AIMethods.l_sound[Enums.ASLList.LSND_FRECS_BOO3].play(SoundbankInfo.VOL_HOLLAR)


static func ai_push_scicon_f(s: TSprite) -> void:
	match s.n_cc:
		1:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONF2])
			AIMethods.NOSPEECHFOR(450)
			for i in range(Enums.ASLList.LSND_APPLES_OFFER1, Enums.ASLList.LSND_RING_ZAP3):
				AIMethods.l_sound[i].stop()
			AIMethods.l_sound[Enums.ASLList.LSND_SCICONF_HIT_MISC].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
		25:
			AIMisc.ais_unlock_achievement(11)
		85:
			Globals.myGameConditions.gb_tri_pub_ban = true
			AIMethods.l_sound[Enums.ASLList.LSND_FRECS_HITAPPLE3].play(SoundbankInfo.VOL_HOLLAR)
			AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_TRI))
		100:  # 85 + 15
			AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PUB))
		115:  # 85 + 30
			AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_BAN))
		170:
			Globals.GameLoop.change_game_state(Enums.GameStates.STATETITLE)
			if Globals.myGameConditions.is_ritual():
				AIMethods.l_sound[Enums.ASLList.LSND_FRECS_REWARDR1].play(SoundbankInfo.VOL_HOLLAR)
			else:
				AIMethods.l_sound[Enums.ASLList.LSND_FRECS_BOO3].play(SoundbankInfo.VOL_HOLLAR)


# === FOREGROUND ENTRY/EXIT SYSTEM ===

static func ai_init_foreground_entry(s: TSprite) -> void:
	match s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_ENTRY_TYPE]:
		1:  # Moving up
			s.n_x = AIMethods.randintin(15, 625)
			s.n_y = SCREENBOTTOM + 200
			s.n_dest_x = s.n_x
			s.n_dest_y = SCREENBOTTOM
		2:  # Moving right
			s.n_x = -200
			s.n_y = SCREENBOTTOM
			s.n_dest_x = 250
			s.n_dest_y = s.n_y
		3:  # Moving left
			s.n_x = 640 + 200
			s.n_y = SCREENBOTTOM
			s.n_dest_x = 640 - 250
			s.n_dest_y = s.n_y
	s.set_behavior(Callable(AIPopUp, "ai_foreground_entry"))


static func ai_foreground_entry(s: TSprite) -> void:
	match s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_ENTRY_TYPE]:
		1:  # Moving up
			if s.n_y <= s.n_dest_y + 5:
				if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_SCICON_BEHAVIOR]:
					ai_init_scicon_foreground_on_screen(s)
				else:
					ai_init_foreground_on_screen(s)
			else:
				s.n_y += (s.n_dest_y - s.n_y) / s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_RELATIVE_SPEED]
		2:  # Moving right
			if s.n_x >= s.n_dest_x - 5:
				if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_SCICON_BEHAVIOR]:
					ai_init_scicon_foreground_on_screen(s)
				else:
					ai_init_foreground_on_screen(s)
			else:
				s.n_x += (s.n_dest_x - s.n_x) / s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_RELATIVE_SPEED]
		3:  # Moving left
			if s.n_x <= s.n_dest_x + 5:
				if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_SCICON_BEHAVIOR]:
					ai_init_scicon_foreground_on_screen(s)
				else:
					ai_init_foreground_on_screen(s)
			else:
				s.n_x += (s.n_dest_x - s.n_x) / s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_RELATIVE_SPEED]

	if Globals.InputService.left_button_pressed() and AISupport.ais_mouse_over(s) \
		and not (AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] == Enums.ArmPositions.ARM_CHANGING \
			and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] == Enums.ArmPositions.ARM_IRON_RING) \
		and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING:
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_NOTHING
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
		AIMethods.spr_arm.n_cc = 0
		s.switch_to_secondary_behavior()
		s.n_cc = 0


static func ai_init_foreground_on_screen(s: TSprite) -> void:
	s.n_x = s.n_dest_x
	s.n_y = s.n_dest_y
	s.set_behavior(Callable(AIPopUp, "ai_foreground_on_screen"))
	s.n_cc = 0


static func ai_foreground_on_screen(s: TSprite) -> void:
	var timeout: int = 25 if 0 == Globals.myGameConditions.get_frosh_lameness() else 15
	if s.n_cc > timeout:
		ai_init_foreground_exit(s)

	if Globals.InputService.left_button_pressed() and AISupport.ais_mouse_over(s) \
		and not (AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] == Enums.ArmPositions.ARM_CHANGING \
			and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] == Enums.ArmPositions.ARM_IRON_RING) \
		and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING:
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_NOTHING
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
		AIMethods.spr_arm.n_cc = 0
		s.switch_to_secondary_behavior()
		s.n_cc = 0


static func ai_init_scicon_foreground_on_screen(s: TSprite) -> void:
	s.n_x = s.n_dest_x
	s.n_y = s.n_dest_y
	s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_WEAPON_BEING_TAKEN] = Enums.ArmPositions.ARM_NOTHING
	s.set_behavior(Callable(AIPopUp, "ai_scicon_foreground_on_screen"))
	s.n_cc = 0


static func ai_scicon_foreground_on_screen(s: TSprite) -> void:
	var spr_bounce: TSprite
	var n_tmp: int

	match s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_WEAPON_BEING_TAKEN]:
		Enums.ArmPositions.ARM_NOTHING:
			pass
		Enums.ArmPositions.ARM_APPLE:
			n_tmp = Globals.myGameConditions.get_player_apples()
			if 0 != n_tmp:
				spr_bounce = SpriteInit.create_sprite(Enums.SpriteType.SPR_APPLE, AIMethods.spr_arm.n_x, s.n_y)
				spr_bounce.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_REBOUNDING
				spr_bounce.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAPPLE5_1 + AIMethods.R.randi() % SoundbankInfo.NSPR_APPLE5])
				spr_bounce.nv_x = AIMethods.randintin(-8, 8)
				spr_bounce.nv_y = 0
				spr_bounce.nv_z = AIMethods.R.randi() % 30
				AIMethods.ss_pit.include(spr_bounce)
				if n_tmp > 15:
					Globals.myGameConditions.get_apples(-5)
				else:
					Globals.myGameConditions.get_apples(-1)
				s.n_cc -= 1
			else:
				s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_WEAPON_BEING_TAKEN] = Enums.ArmPositions.ARM_NOTHING
				AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = AISupport.ais_change_arm()
				AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				AIMethods.spr_arm.n_cc = 0
		Enums.ArmPositions.ARM_PIZZA:
			n_tmp = Globals.myGameConditions.get_player_pizza()
			if 0 != n_tmp:
				spr_bounce = SpriteInit.create_sprite(Enums.SpriteType.SPR_PIZZA, AIMethods.spr_arm.n_x, s.n_y)
				spr_bounce.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_REBOUNDING
				spr_bounce.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPIZZA5_1 + AIMethods.R.randi() % SoundbankInfo.NSPR_PIZZA5])
				spr_bounce.nv_x = AIMethods.randintin(-8, 8)
				spr_bounce.nv_y = 0
				spr_bounce.nv_z = AIMethods.R.randi() % 30
				AIMethods.ss_pit.include(spr_bounce)
				if n_tmp > 15:
					Globals.myGameConditions.get_pizzas(-5)
				else:
					Globals.myGameConditions.get_pizzas(-1)
				s.n_cc -= 1
			else:
				s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_WEAPON_BEING_TAKEN] = Enums.ArmPositions.ARM_NOTHING
				AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = AISupport.ais_change_arm()
				AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				AIMethods.spr_arm.n_cc = 0
		Enums.ArmPositions.ARM_CLARK:
			n_tmp = Globals.myGameConditions.get_player_clark()
			if 0 != n_tmp:
				spr_bounce = SpriteInit.create_sprite(Enums.SpriteType.SPR_CLARK, AIMethods.spr_arm.n_x, s.n_y)
				spr_bounce.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_REBOUNDING
				spr_bounce.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLARK5A_1 + AIMethods.R.randi() % SoundbankInfo.NSPR_CLARK5A])
				spr_bounce.nv_x = AIMethods.randintin(-8, 8)
				spr_bounce.nv_y = 0
				spr_bounce.nv_z = AIMethods.R.randi() % 30
				AIMethods.ss_pit.include(spr_bounce)
				if n_tmp > 15:
					Globals.myGameConditions.get_clarks(-5)
				else:
					Globals.myGameConditions.get_clarks(-1)
				s.n_cc -= 1
			else:
				s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_WEAPON_BEING_TAKEN] = Enums.ArmPositions.ARM_NOTHING
				AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = AISupport.ais_change_arm()
				AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				AIMethods.spr_arm.n_cc = 0
		Enums.ArmPositions.ARM_EXAM:
			n_tmp = Globals.myGameConditions.get_player_exam()
			if 0 != n_tmp:
				spr_bounce = SpriteInit.create_sprite(Enums.SpriteType.SPR_EXAM, AIMethods.spr_arm.n_x, s.n_y)
				spr_bounce.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_REBOUNDING
				spr_bounce.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpEXAM4_1 + AIMethods.R.randi() % SoundbankInfo.NSPR_EXAM4])
				spr_bounce.nv_x = AIMethods.randintin(-8, 8)
				spr_bounce.nv_y = 0
				spr_bounce.nv_z = AIMethods.R.randi() % 30
				AIMethods.ss_pit.include(spr_bounce)
				Globals.myGameConditions.get_exams(-1)
				s.n_cc -= 1
			else:
				s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_WEAPON_BEING_TAKEN] = Enums.ArmPositions.ARM_NOTHING
				AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = AISupport.ais_change_arm()
				AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				AIMethods.spr_arm.n_cc = 0

	if Globals.InputService.left_button_pressed():
		s.n_cc = 0
		if AISupport.ais_mouse_over(s):
			# TRI PUB BAN!
			AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_NOTHING
			AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
			AIMethods.spr_arm.n_cc = 0
			s.switch_to_secondary_behavior()
		elif AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_NOTHING:
			# LOSS OF WEAPONRY
			if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_MALE]:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONM2])
			else:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONF2])

			if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_MALE] and \
				(AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] == Enums.ArmPositions.ARM_APPLE \
				or AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] == Enums.ArmPositions.ARM_CLARK):
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSCICONM3])

			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_SNATCH1].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
			s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_WEAPON_BEING_TAKEN] = AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS]

			match AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS]:
				Enums.ArmPositions.ARM_NOTHING:
					pass
				Enums.ArmPositions.ARM_APPLE:
					AIMethods.l_sound[Enums.ASLList.LSND_SCICONM_HIT_APPLES if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_MALE] else Enums.ASLList.LSND_SCICONF_HIT_APPLES].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				Enums.ArmPositions.ARM_PIZZA:
					AIMethods.l_sound[Enums.ASLList.LSND_SCICONM_HIT_PIZZA if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_MALE] else Enums.ASLList.LSND_SCICONF_HIT_PIZZA].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				Enums.ArmPositions.ARM_CLARK:
					AIMethods.l_sound[Enums.ASLList.LSND_SCICONM_HIT_BEER if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_MALE] else Enums.ASLList.LSND_SCICONF_HIT_BEER].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				Enums.ArmPositions.ARM_EXAM:
					AIMethods.l_sound[Enums.ASLList.LSND_SCICONM_HIT_EXAM if s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_MALE] else Enums.ASLList.LSND_SCICONF_HIT_EXAM].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))

	var timeout: int = 25 if 0 == Globals.myGameConditions.get_frosh_lameness() else 45
	if s.n_cc > timeout:
		ai_init_foreground_exit(s)


static func ai_init_foreground_exit(s: TSprite) -> void:
	match s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_EXIT_TYPE]:
		1:  # Moving down
			s.n_dest_x = s.n_x
			s.n_dest_y = SCREENBOTTOM + 200
		2:  # Moving left
			s.n_dest_x = -200
			s.n_dest_y = s.n_y
		3:  # Moving right
			s.n_dest_x = 640 + 200
			s.n_dest_y = s.n_y
	s.set_behavior(Callable(AIPopUp, "ai_foreground_exit"))


static func ai_foreground_exit(s: TSprite) -> void:
	match s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_ENTRY_TYPE]:
		1:  # Moving up
			if s.n_y >= s.n_dest_y - 5:
				if s == AIMethods.spr_alien:
					AIMethods.spr_alien = null
				s.b_deleted = true
			else:
				s.n_y += (s.n_dest_y - s.n_y) / s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_RELATIVE_SPEED]
		2:  # Moving left
			if s.n_x <= s.n_dest_x + 5:
				if s == AIMethods.spr_alien:
					AIMethods.spr_alien = null
				s.b_deleted = true
			else:
				s.n_x += (s.n_dest_x - s.n_x) / s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_RELATIVE_SPEED]
		3:  # Moving right
			if s.n_x >= s.n_dest_x - 5:
				if s == AIMethods.spr_alien:
					AIMethods.spr_alien = null
				s.b_deleted = true
			else:
				s.n_x += (s.n_dest_x - s.n_x) / s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_RELATIVE_SPEED]

	if Globals.InputService.left_button_pressed() and AISupport.ais_mouse_over(s) \
		and not (AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] == Enums.ArmPositions.ARM_CHANGING \
			and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] == Enums.ArmPositions.ARM_IRON_RING) \
		and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING:
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_NOTHING
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
		AIMethods.spr_arm.n_cc = 0
		s.switch_to_secondary_behavior()
		s.n_cc = 0


# === POPUP INITIALIZATION ===

static func ai_init_popup(s: TSprite, new_frame: FrameDesc, n_speed: int,
	n_entry_type: int, n_on_screen_type: int, n_exit_type: int,
	n_voice_effect: int, f_secondary_behavior: Callable,
	b_scicon_effect: bool = false) -> void:

	s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_RELATIVE_SPEED] = n_speed
	s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_ENTRY_TYPE] = n_entry_type
	s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_ON_SCREEN_TYPE] = n_on_screen_type
	s.n_attrib[Enums.AttrForeGroundPopUpDudes.ATTR_EXIT_TYPE] = n_exit_type
	s.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_SCICON_BEHAVIOR] = b_scicon_effect

	s.set_secondary_behavior(f_secondary_behavior)

	s.set_frame(new_frame)
	ai_init_foreground_entry(s)
	AIMethods.l_sound[n_voice_effect].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
