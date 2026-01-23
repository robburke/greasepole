class_name AIFrosh

# AIFrosh.gd - Static class with frosh AI logic
# Ported from aiFrosh.cs
#
# Per porting_rules.md:
# - This is a static class with logic functions only
# - All shared state accessed via AIMethods autoload

# === CONSTANTS ===
const TIME_MOSH_SPEED: int = 69
const TIME_ANIMALIA: int = 350
const TIME_TAM_TUG: int = 2
const NOT_LOOKING_AT_SCREEN: bool = false
const LOOKING_AT_SCREEN: bool = true

# === STATIC STATE ===
static var n_number_of_tugs: int = 0
static var n_new_high_score: int = 0
static var b_did_he_make_it: int = 0


# === BEHAVIOR 1: FREE FALLING ===

static func ai_act_1(s: TSprite) -> void:
	AISupport.ais_plummet(s)
	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 0

	# Prevent going offscreen
	AISupport.ais_keep_in_pit_y(s)

	if s.n_z < AIDefine.D_BELLY_BUTTON_Z and s.nv_z < 0:
		# The frosh has hit the water
		s.n_z = 0
		if s.nv_z < -AIDefine.D_SPEED_FOR_BIG_SPLASH:
			AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_L, s.n_x, s.n_y))
		else:
			AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_M, s.n_x, s.n_y))
		AIMethods.ss_water.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_RIPPLE, s.n_x, s.n_y))
		ai_init_3(s)


static func ai_init_1(s: TSprite) -> void:
	# Release the Frosh's claim to a spot
	if s.pp_chosen != null and s.pp_chosen.get_claimer() == s:
		s.pp_chosen.release_claim()
		s.pp_chosen = null

	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 0
	s.nv_x = AIMethods.randintin(-AIDefine.D_THREE, AIDefine.D_THREE)
	s.nv_y = AIMethods.randintin(-AIDefine.D_ONE, AIDefine.D_ONE)
	s.nv_z = AIMethods.randintin(0, AIDefine.D_SIX)

	if 0 == AIMethods.R.randi() % 6:
		# SEND THE FROSH _REALLY_ FLYING
		s.nv_x = AIMethods.randintin(-AIDefine.D_TEN, AIDefine.D_TEN)
		s.nv_y = AIMethods.randintin(-AIDefine.D_SIX, AIDefine.D_SIX)
		s.nv_z = AIMethods.randintin(0, AIDefine.D_TEN * 3)

	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR1_1 + AIMethods.R.randi() % AIDefine.NSPR_FR1])
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_1"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 1


# === BEHAVIOR 2: LEAPING INTO PIT ===

static func ai_act_2(s: TSprite) -> void:
	s.set_behavior(Callable(AIFrosh, "ai_act_1"))


static func ai_init_2(s: TSprite) -> void:
	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 0
	s.nv_x = AIMethods.randintin(AIDefine.D_ONE, AIDefine.D_TEN * 2)
	s.nv_y = AIMethods.randintin(AIDefine.D_ONE, AIDefine.D_TEN)
	s.nv_z = AIMethods.randintin(0, AIDefine.D_TEN * 4)
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR2_1 + AIMethods.R.randi() % AIDefine.NSPR_FR2])
	if 0 == AIMethods.R.randi() % (AIDefine.NSPR_FR2 + 1):
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR1_1 + AIMethods.R.randi() % AIDefine.NSPR_FR1])
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_2"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 2


# === BEHAVIOR 3: UNDERWATER ===

static func ai_init_3(s: TSprite) -> void:
	if s.n_attrib[Enums.NAttrFrosh.ATTR_HEIGHT_OF_FALL] > (AIDefine.D_TAM_Z - 10):
		AISupport.ais_unlock_achievement(747)
	s.n_attrib[Enums.NAttrFrosh.ATTR_HEIGHT_OF_FALL] = 0

	s.nv_x = AIMethods.randintin(-AIDefine.D_SIX, AIDefine.D_SIX)
	s.nv_y = AIMethods.randintin(-AIDefine.D_THREE, AIDefine.D_THREE)
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR3_1 + AIMethods.R.randi() % AIDefine.NSPR_FR3])
	s.set_behavior(Callable(AIFrosh, "ai_act_3"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 3


static func ai_act_3(s: TSprite) -> void:
	if 0 == (s.n_cc % 2):
		s.n_x += (s.nv_x + AIMethods.randintin(-AIDefine.D_TWO, AIDefine.D_TWO)) / 2
		s.n_y += (s.nv_y + AIMethods.randintin(-AIDefine.D_ONE, AIDefine.D_ONE)) / 2

	if AIMethods.R.randi() % 50 == 0:
		AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_S, s.n_x, s.n_y))
		if AIMethods.R.randi() % 10 != 0:
			ai_init_4(s)
		else:
			ai_init_6c(s)  # Every once in a while, they swim

	if 0 == AIMethods.R.randi() % 30:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR3_1 + AIMethods.R.randi() % AIDefine.NSPR_FR3])

	AISupport.ais_keep_in_pit_x(s)
	AISupport.ais_keep_in_pit_y(s)


# === BEHAVIOR 4: WADING - THE HUB ===

static func ai_init_4(s: TSprite) -> void:
	# Initialize the HUB of the AI
	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 0
	s.n_z = 0
	s.nv_z = 0

	# Set speed as a function of strength
	var n_strength_fudged: int = s.n_attrib[Enums.NAttrFrosh.ATTR_STR] + 1
	s.nv_x = AIDefine.D_ONE + (n_strength_fudged / 3)
	if s.b_attrib[Enums.BAttrFrosh.ATTR_EXCITED]:
		s.nv_x = s.nv_x * 3

	s.nv_y = AIDefine.D_ONE + (n_strength_fudged / 4)

	# Randomly make them excited
	s.b_attrib[Enums.BAttrFrosh.ATTR_EXCITED] = abs(s.nv_x) > 2

	s.n_attrib[Enums.NAttrFrosh.ATTR_FRAME] = 0
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR4A_1])
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_4"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 4


static func ai_act_4(s: TSprite) -> void:
	# HUB OF AI
	if s.n_x < AIDefine.D_PIT_MIN_X - 300:
		s.n_x = AIDefine.D_PIT_MIN_X - 300
	if s.n_x > AIDefine.D_PIT_MAX_X + 300:
		s.n_x = AIDefine.D_PIT_MAX_X + 300

	# Generate random ripples
	if abs(s.nv_x) > 4:
		if 0 == (AIMethods.R.randi() % 1000) % AIDefine.D_SIX and 0 != Globals.myGameConditions.get_enhanced_graphics():
			AIMethods.ss_water.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_RIPPLE, s.n_x, s.n_y))
	else:
		if 0 == (AIMethods.R.randi() % 1000) % AIDefine.D_TEN and 0 != Globals.myGameConditions.get_enhanced_graphics():
			AIMethods.ss_water.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_RIPPLE, s.n_x, s.n_y))

	# Move Frosh towards destination
	if s.n_dest_x < AIDefine.D_PIT_MIN_X_MINUS_50:
		s.n_dest_x = AIDefine.D_PIT_MIN_X_MINUS_50
	elif s.n_dest_x > AIDefine.D_PIT_MAX_X_PLUS_50:
		s.n_dest_x = AIDefine.D_PIT_MAX_X_PLUS_50
	if s.n_dest_y < AIDefine.D_PIT_MIN_Y:
		s.n_dest_y = AIDefine.D_PIT_MIN_Y
	elif s.n_dest_y > AIDefine.D_PIT_MAX_Y:
		s.n_dest_y = AIDefine.D_PIT_MAX_Y
	s.nv_z = 0
	s.n_z = 0
	AISupport.ais_keep_in_pit_y(s)
	AISupport.ais_move_towards_destination(s)

	if (abs(s.n_x - s.n_dest_x) <= s.nv_x) and (abs(s.n_y - s.n_dest_y) <= s.nv_y):
		# WHEN THE FROSH REACHES THEIR GOAL
		s.n_x = s.n_dest_x
		s.n_y = s.n_dest_y

		match s.n_attrib[Enums.NAttrFrosh.ATTR_GOAL]:
			Enums.Goals.GOAL_THINK:
				ai_init_6d(s)
			Enums.Goals.GOAL_MINDLESS_WANDERING:
				if 0 == AIMethods.R.randi() % 5:
					ai_init_5c(s)
				else:
					AISupport.ais_choose_frosh_pit_goal(s)
			Enums.Goals.GOAL_PYRAMID_SPOT:
				if s.pp_chosen == null:
					AISupport.ais_choose_frosh_pit_goal(s)
				elif s.pp_chosen.get_claimer() == s:
					ai_init_7a(s)
				else:
					AISupport.ais_choose_frosh_pit_goal(s)
			Enums.Goals.GOAL_CLIMBING_UP:
				if (0 == Globals.myGameConditions.get_booster(5)) or (AIMethods.n_frosh_above_1 < AIMethods.n_frosh_level[1]):
					ai_init_9a(s, s.n_y < AIDefine.D_POLE_Y)
				else:
					AISupport.ais_choose_frosh_pit_goal(s)
			Enums.Goals.GOAL_CLARK:
				s.n_x = s.n_dest_x
				s.n_y = s.n_dest_y
				if AISupport.ais_consume(s, true):
					ai_init_5b(s)
				else:
					AISupport.ais_choose_frosh_pit_goal(s)
			Enums.Goals.GOAL_PIZZA:
				s.n_x = s.n_dest_x
				s.n_y = s.n_dest_y
				if AISupport.ais_consume(s):
					ai_init_5a(s)
				else:
					AISupport.ais_choose_frosh_pit_goal(s)
			Enums.Goals.GOAL_ARTSCI:
				if AIMethods.spr_alien == null:
					AISupport.ais_choose_frosh_pit_goal(s)
				else:
					var n_temp: int
					if s.n_x < AIMethods.spr_alien.n_x:
						n_temp = abs((AIMethods.spr_alien.n_x - AIDefine.D_ARTSCI_SPLASHING_OFFSET_X) - s.n_x)
					else:
						n_temp = abs((AIMethods.spr_alien.n_x + AIDefine.D_ARTSCI_SPLASHING_OFFSET_X) - s.n_x)
					s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = AIMethods.spr_alien.n_x < s.n_x
					if n_temp < 20:
						AIMethods.spr_alien.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
						ai_init_5c(s)
					else:
						s.n_dest_x = AIMethods.spr_alien.n_x - AIDefine.D_ARTSCI_SPLASHING_OFFSET_X if AIMethods.spr_alien.n_x < s.n_x else AIMethods.spr_alien.n_x + AIDefine.D_ARTSCI_SPLASHING_OFFSET_X
			Enums.Goals.GOAL_COMMIE:
				if AIMethods.spr_alien == null:
					AISupport.ais_choose_frosh_pit_goal(s)
				else:
					var n_temp: int
					if s.n_x < AIMethods.spr_alien.n_x:
						n_temp = abs((AIMethods.spr_alien.n_x - AIDefine.D_COMMIE_PUNCHING_OFFSET_X) - s.n_x)
					else:
						n_temp = abs((AIMethods.spr_alien.n_x + AIDefine.D_COMMIE_PUNCHING_OFFSET_X) - s.n_x)
					s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = AIMethods.spr_alien.n_x < s.n_x
					if n_temp < 20:
						AIMethods.spr_alien.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
						ai_init_5d(s)
					else:
						s.n_dest_x = AIMethods.spr_alien.n_x - AIDefine.D_COMMIE_PUNCHING_OFFSET_X if AIMethods.spr_alien.n_x < s.n_x else AIMethods.spr_alien.n_x + AIDefine.D_COMMIE_PUNCHING_OFFSET_X
			Enums.Goals.GOAL_MOSH:
				ai_init_6b(s)
			_:
				AISupport.ais_choose_frosh_pit_goal(s)
	else:
		# Frosh is moving towards destination - choose an image
		if 0 == (s.n_cc % (13 - s.nv_x)):
			s.n_attrib[Enums.NAttrFrosh.ATTR_FRAME] += 1

			if s.b_attrib[Enums.BAttrFrosh.ATTR_EXCITED]:
				if s.n_attrib[Enums.NAttrFrosh.ATTR_FRAME] >= AIDefine.NSPR_FR4E:
					s.n_attrib[Enums.NAttrFrosh.ATTR_FRAME] = 0
				if s.n_dest_x < s.n_x:
					s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = true
					s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR4B_1 + s.n_attrib[Enums.NAttrFrosh.ATTR_FRAME]])
				else:
					s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = false
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR4B_1 + s.n_attrib[Enums.NAttrFrosh.ATTR_FRAME]])
			else:
				if s.n_attrib[Enums.NAttrFrosh.ATTR_FRAME] >= AIDefine.NSPR_FR4:
					s.n_attrib[Enums.NAttrFrosh.ATTR_FRAME] = 0
				if s.n_dest_x < s.n_x:
					s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = true
					s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR4A_1 + s.n_attrib[Enums.NAttrFrosh.ATTR_FRAME]])
				else:
					s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = false
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR4A_1 + s.n_attrib[Enums.NAttrFrosh.ATTR_FRAME]])


# === BEHAVIOR 5A: EATING PIZZA ===

static func ai_init_5a(s: TSprite) -> void:
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 5
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	s.n_cc = 0
	s.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_MINDLESS_WANDERING
	s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5A_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5A_1])
	s.set_behavior(Callable(AIFrosh, "ai_act_5a"))


static func ai_act_5a(s: TSprite) -> void:
	if 0 == (s.n_cc % AIDefine.TIME_PIZZA_MUNCH):
		match s.n_cc / AIDefine.TIME_PIZZA_MUNCH:
			1:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5A_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5A_2])
				if not AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_PIZZAEAT].is_playing():
					AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_PIZZAEAT].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
			5:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5A_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5A_2])
				if 0 != AIMethods.R.randi() % AIDefine.N_PIZZA_MUNCH_AVERAGE:
					s.n_cc = AIDefine.TIME_PIZZA_MUNCH
				else:
					ai_init_4(s)
			2, 4:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5A_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5A_3])
			3:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5A_4] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5A_4])


# === BEHAVIOR 5B: DRINKING BEER ===

static func ai_init_5b(s: TSprite) -> void:
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 5
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	s.n_cc = 0
	s.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_MINDLESS_WANDERING
	s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5B_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5B_1])
	s.set_behavior(Callable(AIFrosh, "ai_act_5b"))


static func ai_act_5b(s: TSprite) -> void:
	if 0 == (s.n_cc % 3):
		match s.n_cc / 3:
			1:
				AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CHUG].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
				AIMethods.NOSPEECHFOR(50)
			3, 5, 7, 9:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5B_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5B_2])
			2, 4, 6, 8, 10:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5B_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5B_3])
			12:
				if 0 != AIMethods.R.randi() % 2:
					AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CHUGLASTDROP].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5B_4] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5B_4])
			15:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5B_5] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5B_5])
			20:
				if 0 != AIMethods.R.randi() % 6:
					AIMethods.l_sound[Enums.ASLList.LSND_FROSH_CLARKFINISH1 + AIMethods.R.randi() % SoundbankInfo.NSND_FROSH_CLARKFINISH].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				else:
					AIMethods.l_sound[Enums.ASLList.LSND_FROSH_CLARKFINISH3].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5B_6] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5B_6])
			38:
				if 0 != AIMethods.R.randi() % 2:
					ai_init_6a(s)
				elif 0 != AIMethods.R.randi() % 3:
					ai_init_4(s)
				else:
					ai_init_6b(s)


# === BEHAVIOR 5C: SPLASHING ===

static func ai_init_5c(s: TSprite) -> void:
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5C_1])
	else:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5C_1])
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_5c"))


static func ai_act_5c(s: TSprite) -> void:
	if 0 == (s.n_cc % 4):
		match s.n_cc / 4:
			1, 3, 5, 7:
				if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]:
					AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_ML,
						s.n_x - 20 + AIMethods.randintin(-15, 15), s.n_y + AIMethods.randintin(-2, 2)))
					s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5C_2])
				else:
					AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_M,
						s.n_x + 20 + AIMethods.randintin(-15, 15), s.n_y + AIMethods.randintin(-2, 2)))
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5C_2])
			2, 4, 6, 8:
				if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]:
					AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_ML,
						s.n_x - 20 + AIMethods.randintin(-15, 15), s.n_y + AIMethods.randintin(-2, 2)))
					s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5C_1])
				else:
					AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_M,
						s.n_x + 20 + AIMethods.randintin(-15, 15), s.n_y + AIMethods.randintin(-2, 2)))
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5C_1])
			9:
				AISupport.ais_choose_frosh_pit_goal(s)
				ai_init_4(s)


# === BEHAVIOR 5D: PUNCHING COMMIE ===

static func ai_init_5d(s: TSprite) -> void:
	if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5D_1])
	else:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5D_1])
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_5d"))


static func ai_act_5d(s: TSprite) -> void:
	if 0 == (s.n_cc % 8):
		match s.n_cc / 8:
			1, 3, 5:
				if not AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_PUNCH].is_playing():
					AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_PUNCH].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5D_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5D_2])
			2, 4, 6:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR5D_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR5D_1])
			7:
				AISupport.ais_choose_frosh_pit_goal(s)
				ai_init_4(s)


# === BEHAVIOR 6A: DRUNKEN SINGING ===

static func ai_init_6a(s: TSprite) -> void:
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	s.n_x = s.n_dest_x
	s.n_y = s.n_dest_y
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6A_1])
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_6a"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 6
	AIMethods.NOSPEECHFOR(15)


static func ai_act_6a(s: TSprite) -> void:
	match s.n_cc / 6:
		1, 3, 5, 7, 9:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6A_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6A_2])
		2, 4, 6, 8, 10:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6A_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6A_1])
		14:
			ai_init_4(s)


# === BEHAVIOR 6B: MOSHING ===

static func ai_init_6b(s: TSprite) -> void:
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	s.n_x = s.n_dest_x
	s.n_y = s.n_dest_y
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6B_1])
	s.set_behavior(Callable(AIFrosh, "ai_act_6b"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 6
	s.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_MINDLESS_WANDERING
	s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = true


static func ai_act_6b(s: TSprite) -> void:
	if s.n_cc > 400:
		ai_init_4(s)
	else:
		match AIMethods.spr_pole.n_cc % TIME_MOSH_SPEED:
			4, 24:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6B_2])
			16, 32:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6B_1])
			40, 56:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6B_2])
			48, 64:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6B_1])


# === BEHAVIOR 6C: SWIMMING ===

static func ai_init_6c(s: TSprite) -> void:
	s.nv_x = AIDefine.D_SWIM_SPEED
	s.nv_y = 0
	s.nv_z = 0
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_6c"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 6
	s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = s.n_x > AIDefine.D_POLE_X
	s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6C_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6C_1])
	if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]:
		s.n_dest_x = AIDefine.D_PIT_MIN_X - AIDefine.D_SWIM_DISTANCE
	else:
		s.n_dest_x = AIDefine.D_PIT_MAX_X + AIDefine.D_SWIM_DISTANCE


static func ai_act_6c(s: TSprite) -> void:
	AISupport.ais_move_towards_destination(s)
	if abs(s.n_x - s.n_dest_x) <= s.nv_x:
		AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_M, s.n_x, s.n_y))
		ai_init_4(s)

	if 0 == (s.n_cc % AIDefine.D_SWIM_FRAME_RATE):
		AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_M,
			s.n_x + (-70 if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else 70), s.n_y + AIMethods.randintin(-2, 2)))
		if 0 == (s.n_cc % (AIDefine.D_SWIM_FRAME_RATE * 2)):
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6C_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6C_1])
		else:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6C_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6C_2])


# === BEHAVIOR 6D: THINKING ===

static func ai_init_6d(s: TSprite) -> void:
	s.nv_x = 2 + AIMethods.R.randi() % 3
	s.nv_y = 1
	s.nv_z = 0
	if 0 != AIMethods.R.randi() % 2:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6D_1] if 0 != AIMethods.R.randi() % 2 else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6D_1])
	else:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6D_2] if 0 != AIMethods.R.randi() % 2 else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6D_2])
	s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = 0 == AIMethods.R.randi() % 25
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_6d"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 6
	s.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_THINK
	s.n_dest_x = s.n_x
	s.n_dest_y = s.n_y


static func ai_act_6d(s: TSprite) -> void:
	AISupport.ais_bob_up_and_down(s)
	AISupport.ais_move_towards_destination(s)
	if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]:
		match s.n_cc / 4:
			1, 3, 5, 7, 9, 11, 13:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6D_3])
			2, 4, 6, 8, 10, 12, 14:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6D_4])
	if s.n_cc > AIDefine.TIME_THINKING_TIME:
		s.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_MINDLESS_WANDERING
		s.b_attrib[Enums.BAttrFrosh.ATTR_EXCITED] = false
		ai_init_4(s)


# === BEHAVIOR 6E: COW-EAGLES (SHOCKED) ===

static func ai_init_6e(s: TSprite) -> void:
	s.use_color_swap = false
	if s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == -6:
		return
	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 0
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	s.n_cc = 0
	AIMethods.ss_console.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POOF, s.n_scr_x, s.n_scr_y))
	s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = 0 == AIMethods.R.randi() % 2
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6E_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6E_1])
	s.set_behavior(Callable(AIFrosh, "ai_act_6e"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_STR] = AIMethods.R.randi() % 5 + 3
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = -6


static func ai_act_6e(s: TSprite) -> void:
	if 0 == AIMethods.R.randi() % 150:
		var n_temp: int = AIMethods.R.randi() % 2
		if not AIMethods.l_sound[Enums.ASLList.LSND_FROSH_COW1 + n_temp].is_playing():
			AIMethods.l_sound[Enums.ASLList.LSND_FROSH_COW1 + n_temp].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
	if 0 == (s.n_cc % s.n_attrib[Enums.NAttrFrosh.ATTR_STR]):
		if 0 == (s.n_cc % (s.n_attrib[Enums.NAttrFrosh.ATTR_STR] * 2)):
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6E_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6E_2])
		else:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6E_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6E_1])
	s.n_z += AIMethods.R.randi() % 2 + 1
	if 0 == AIMethods.R.randi() % 200:
		s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = not s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]
	if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]:
		s.n_x += AIMethods.R.randi() % 3
	else:
		s.n_x -= AIMethods.R.randi() % 3
	if s.n_cc > TIME_ANIMALIA:
		AIMethods.ss_console.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POOF, s.n_scr_x, s.n_scr_y))
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR1_1])
		s.use_color_swap = true
		ai_init_1(s)


# === BEHAVIOR 6F: SHEEP (SHOCKED) ===

static func ai_init_6f(s: TSprite) -> void:
	s.use_color_swap = false
	if s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == -6:
		return
	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 0
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	s.n_cc = 0
	AIMethods.ss_console.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POOF, s.n_scr_x, s.n_scr_y))
	s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = 0 == AIMethods.R.randi() % 2
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = -6
	s.n_attrib[Enums.NAttrFrosh.ATTR_STR] = AIMethods.R.randi() % 5 + 13
	if s.n_z > 0:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6F_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6F_1])
	else:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6F_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6F_2])
	s.set_behavior(Callable(AIFrosh, "ai_act_6f"))
	if 0 == AIMethods.R.randi() % 3:
		s.n_attrib[Enums.NAttrFrosh.ATTR_STR] = AIMethods.R.randi() % 3 + 3
	else:
		s.n_attrib[Enums.NAttrFrosh.ATTR_STR] = AIMethods.R.randi() % 3 + 1


static func ai_act_6f(s: TSprite) -> void:
	if 0 == AIMethods.R.randi() % 150:
		var n_temp: int = AIMethods.R.randi() % 2
		if not AIMethods.l_sound[Enums.ASLList.LSND_FROSH_SHEEP1 + n_temp].is_playing():
			AIMethods.l_sound[Enums.ASLList.LSND_FROSH_SHEEP1 + n_temp].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
	if s.n_z > 0:
		AISupport.ais_plummet(s)
	elif s.n_z < -1:
		s.n_z = 0
	else:
		s.n_y += 1 if 0 == AIMethods.R.randi() % 4 else 0
		if 0 == (s.n_cc % s.n_attrib[Enums.NAttrFrosh.ATTR_STR]):
			if 0 == AIMethods.R.randi() % 10:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6F_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6F_2])
			else:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR6F_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR6F_3])
		AISupport.ais_bob_up_and_down(s)
		if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]:
			s.n_x += s.n_attrib[Enums.NAttrFrosh.ATTR_STR]
		else:
			s.n_x -= s.n_attrib[Enums.NAttrFrosh.ATTR_STR]
		if 0 == AIMethods.R.randi() % 200:
			s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = not s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]
		if s.n_cc > TIME_ANIMALIA:
			AIMethods.ss_console.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POOF, s.n_scr_x, s.n_scr_y))
			s.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_MINDLESS_WANDERING
			s.b_attrib[Enums.BAttrFrosh.ATTR_EXCITED] = false
			s.nv_z = 2
			s.n_z = 3
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR1_1])
			s.use_color_swap = true
			ai_init_1(s)


# === BEHAVIOR 7A: PYRAMID SPOT ===

static func ai_init_7a(s: TSprite) -> void:
	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 1
	s.nv_x = 3
	s.nv_y = 3
	s.nv_z = 0
	s.n_x = s.n_dest_x
	s.n_y = s.n_dest_y
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR7A_1])
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_7a"), AIDefine.AI_KEEP_POLE_POSITION)
	s.b_attrib[Enums.BAttrFrosh.ATTR_WEIGHT_ON_SHOULDERS] = false
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 7


static func ai_act_7a(s: TSprite) -> void:
	if s.n_cc <= 10:
		match s.n_cc:
			4:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR7A_2])
			10:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR7A_3])
	else:
		AISupport.ais_bob_up_and_down(s)

	if not s.b_attrib[Enums.BAttrFrosh.ATTR_WEIGHT_ON_SHOULDERS]:
		AISupport.ais_pick_closer_pyramid_spot(s)
	AISupport.ais_move_towards_destination(s)

	if s.pp_chosen != null and s.pp_chosen.get_parent() != null:
		if s.pp_chosen.get_parent().position_is_free():
			s.pp_chosen.release_claim()
			s.pp_chosen = s.pp_chosen.get_parent()
			s.pp_chosen.set_claim(s)
			s.n_dest_x = s.pp_chosen.get_x()
			s.n_dest_y = s.pp_chosen.get_y()
		else:
			# Try to climb - only if at the end of the chain (no child position)
			# Note: This could be changed to allow anyone without a Claimer in their Child position to go for it
			if s.pp_chosen.get_child() == null \
				and ((0 == Globals.myGameConditions.get_booster(3)) or (AIMethods.n_frosh_above_1 < AIMethods.n_frosh_level[1])) \
				and 0 == AIMethods.R.randi() % 100:
				s.set_goal(Enums.Goals.GOAL_CLIMBING_UP)
				s.set_behavior(Callable(AIFrosh, "ai_act_4"), AIDefine.AI_KEEP_POLE_POSITION)
				var person_to_climb: TSprite = s.pp_chosen.get_parent().get_claimer()
				s.n_dest_x = person_to_climb.n_x
				s.n_dest_y = person_to_climb.n_y + 1
				s.pp_abandon()


# === BEHAVIOR 9A: CLIMBING ===

static func ai_init_9a(s: TSprite, b_looking_at_screen: bool = false) -> void:
	s.nv_x = 3
	s.nv_y = 1
	s.nv_z = 1
	s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_AT_SCREEN] = b_looking_at_screen
	Globals.myGameConditions.add_energy(2)
	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] += 1
	s.n_dest_y -= 1
	if s.n_dest_y < AIDefine.D_POLE_Y + 2:
		s.n_dest_y = AIDefine.D_POLE_Y + 2

	if s.n_z > 30:
		s.n_dest_z = s.n_z + 120
	else:
		s.n_dest_z = s.n_z + 93

	if s.n_dest_z > AIDefine.D_TAM_Z and AIMethods.SPEECHOK():
		AIMethods.l_sound[Enums.ASLList.LSND_FROSH_ATTOP1 + AIMethods.R.randi() % 5].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
		AIMethods.NOSPEECHFOR(15)

	if s.n_z > AIDefine.D_TAM_Z:
		s.n_dest_x = AIDefine.D_POLE_X
		s.n_dest_y = AIDefine.D_POLE_Y + 2
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR15_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR15_1])
	else:
		if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_AT_SCREEN]:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR9B_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR9B_1])
		else:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR9A_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR9A_1])

	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_9a"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 9


static func ai_act_9a(s: TSprite) -> void:
	if s.n_z > AIDefine.D_TAM_Z + 80:
		ai_init_16a(s)
		return

	if s.n_z < s.n_dest_z:
		s.n_z += AIDefine.D_CLIMBING_SPEED
		if s.n_z > AIDefine.D_TAM_Z:
			AISupport.ais_move_towards_destination(s)
			match s.n_cc:
				4:
					if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_AT_SCREEN]:
						s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR9B_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR9B_2])
					else:
						s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR15_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR15_2])
				10:
					if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_AT_SCREEN]:
						s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR9B_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR9B_2])
					else:
						s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR15_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR15_2])
		else:
			match s.n_cc:
				4:
					if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_AT_SCREEN]:
						s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR9B_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR9B_2])
					else:
						s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR9A_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR9A_2])
				10:
					if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_AT_SCREEN]:
						s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR9B_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR9B_3])
					else:
						s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR9A_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR9A_3])
	else:
		if s.n_z > AIDefine.D_TAM_Z:
			ai_init_16a(s)
		else:
			s.n_z = s.n_dest_z
			ai_init_10(s)


# === BEHAVIOR 10: STUMBLE OVER NECKS ===

static func ai_init_10(s: TSprite) -> void:
	if s.n_z < 100:
		s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 2
	elif s.n_z < 220:
		s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 3
	elif s.n_z < 340:
		s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 4
	else:
		s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 5

	s.nv_x = 1
	s.nv_y = 1
	s.nv_z = 0
	s.n_cc = 0

	if s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] == 2:
		if s.n_x < AIDefine.D_POLE_X:
			s.n_dest_x = AIDefine.D_POLE_X - AIDefine.D_UPPER_LEVEL_DISTANCE_FROM_POLE - AIMethods.R.randi() % 30
		else:
			s.n_dest_x = AIDefine.D_POLE_X + AIDefine.D_UPPER_LEVEL_DISTANCE_FROM_POLE + AIMethods.R.randi() % 30
	else:
		if s.n_x < AIDefine.D_POLE_X:
			s.n_dest_x = AIDefine.D_POLE_X - AIDefine.D_UPPER_LEVEL_DISTANCE_FROM_POLE - AIMethods.R.randi() % 5
		else:
			s.n_dest_x = AIDefine.D_POLE_X + AIDefine.D_UPPER_LEVEL_DISTANCE_FROM_POLE + AIMethods.R.randi() % 5

	s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = s.n_dest_x < s.n_x
	AISupport.ais_choose_frosh_upper_level_goal(s)
	s.set_behavior(Callable(AIFrosh, "ai_act_10"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 10


static func ai_act_10(s: TSprite) -> void:
	var support: SpriteSet = AISupport.ais_get_frosh_in_range(
		s.n_x - 50, s.n_y - (AIDefine.D_FROSH_ARM_LINK_OFFSET_Y / 2),
		s.n_x + 50, s.n_y + (AIDefine.D_FROSH_ARM_LINK_OFFSET_Y / 2))
	var n: int = support.get_number_of_sprites()

	AISupport.ais_move_towards_destination(s)
	match s.n_cc:
		1:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR10_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR10_1])
		10, 30:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR10_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR10_2])
		15:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR10_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR10_3])
		33:
			s.n_cc = 0

	# Make sure the Frosh is supported
	var b_support: int = 0
	# Determine if anyone else is on the same level with you at this spot
	var b_same_level: int = 0
	var closest_frosh_to_pole: TSprite = null
	# See if someone's weighing you down
	var b_under_pressure: int = 0
	var b_do_not_climb: bool = false

	for i in range(n):
		if s != support.get_sprite(i):
			if abs(s.n_z - support.get_sprite(i).n_z - 90) < 50:
				b_support += 1
				AISupport.ais_weight_on_shoulders(support.get_sprite(i))
			if abs(s.n_z - support.get_sprite(i).n_z) < 50:
				b_same_level += 1
				if closest_frosh_to_pole == null:
					closest_frosh_to_pole = support.get_sprite(i)
				else:
					closest_frosh_to_pole = _ai_closest_to_pole(closest_frosh_to_pole, support.get_sprite(i))
			if (support.get_sprite(i).n_z - s.n_z) > 30:
				if support.get_sprite(i).n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == 16:
					b_do_not_climb = true
				b_under_pressure += 1

	if closest_frosh_to_pole != null and abs(closest_frosh_to_pole.n_x - s.n_x) > 10:
		b_do_not_climb = true

	if 0 == b_support:
		AISupport.ais_send_frosh_flying(s)
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR1_1 + AIMethods.R.randi() % AIDefine.NSPR_FR1])

	# If they are close enough to the pole, make them into an ARMS-UP SUPPORTER
	if abs(s.n_x - s.n_dest_x) < 10 and abs(s.n_y - s.n_dest_y) < AIDefine.D_FROSH_WIDTH_Y:
		s.n_y = s.n_dest_y
		ai_init_11(s)

	# BUT, if there is someone else here that is closer to the pole, you can climb
	# over them.
	if b_same_level != 0 and closest_frosh_to_pole != null:
		var test: TSprite = _ai_closest_to_pole(s, closest_frosh_to_pole)
		if test != s and closest_frosh_to_pole.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] >= 11:
			# If I am not closest to the pole, I can climb over them
			if s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] == Enums.UpperLevelGoals.UPPER_GOAL_CLIMB and not b_do_not_climb:
				AISupport.ais_move_towards_destination(s)
				s.n_y = closest_frosh_to_pole.n_y + (-1 if s.n_y < closest_frosh_to_pole.n_y else 1)
				s.n_z = closest_frosh_to_pole.n_z
				ai_init_11(s)
			elif s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] == Enums.UpperLevelGoals.UPPER_GOAL_SUPPORT:
				ai_init_11(s)  # This can precipitate a long-distance support


static func _ai_closest_to_pole(spr_first: TSprite, spr_second: TSprite) -> TSprite:
	var n_first: int = abs(spr_first.n_x - AIDefine.D_POLE_X) + abs(spr_first.n_y - AIDefine.D_POLE_Y)
	var n_second: int = abs(spr_second.n_x - AIDefine.D_POLE_X) + abs(spr_second.n_y - AIDefine.D_POLE_Y)
	return spr_first if n_first < n_second else spr_second


# === BEHAVIOR 11: ARMS UP SUPPORT ===

static func ai_init_11(s: TSprite) -> void:
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	s.n_cc = 0
	s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = s.n_x > AIDefine.D_POLE_X
	if s.n_y >= AIDefine.D_POLE_Y and s.n_y <= (AIDefine.D_POLE_Y + 2):
		s.n_y = AIDefine.D_POLE_Y + 3
	s.set_behavior(Callable(AIFrosh, "ai_act_11"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 11


static func ai_act_11(s: TSprite) -> void:
	var support: SpriteSet = AISupport.ais_get_frosh_in_range(
		s.n_x - 50, s.n_y - (AIDefine.D_FROSH_ARM_LINK_OFFSET_Y / 2),
		s.n_x + 50, s.n_y + (AIDefine.D_FROSH_ARM_LINK_OFFSET_Y / 2))

	var n_current_level: int = s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL]

	AISupport.ais_move_towards_destination(s)
	match s.n_cc:
		1:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_1])
		80:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_2])
		160:
			s.n_cc = 0

	# GET THREE STATS about this frosh:
	# Make sure the Frosh is supported
	var n_support: int = 0
	# Determine if anyone else is on the same level with you at this spot
	var b_same_level: bool = false
	var closest_frosh_to_pole: TSprite = null
	# See if someone's weighing you down
	var b_under_pressure: int = 0
	var b_do_not_cling: bool = false
	var n: int = support.get_number_of_sprites()

	for i in range(n):
		if s != support.get_sprite(i):
			if abs(s.n_z - support.get_sprite(i).n_z - 90) < 50:
				n_support += 1
				AISupport.ais_weight_on_shoulders(support.get_sprite(i))
			if abs(s.n_z - support.get_sprite(i).n_z) < 50:
				b_same_level = true
				if closest_frosh_to_pole == null:
					closest_frosh_to_pole = support.get_sprite(i)
				else:
					closest_frosh_to_pole = _ai_closest_to_pole(closest_frosh_to_pole, support.get_sprite(i))
				if support.get_sprite(i).n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == 14:
					b_do_not_cling = true
			if (support.get_sprite(i).n_z - s.n_z) > 30:
				b_under_pressure += 1

	# Every once in a while, check if we should climb higher
	# PERFORMANCE BOOST: INTELLIGENCE - about 80 is shoddy, about 50 is better climbing
	var n_booster_4: int = Globals.myGameConditions.get_booster(4)
	if 0 == AIMethods.R.randi() % maxi(1, Globals.myGameConditions.get_booster(2)):
		# If there aren't enough people above me
		if n_current_level < 6 and AIMethods.n_frosh_level[n_current_level + 1] < AIMethods.n_frosh_target[n_current_level + 1] \
			and b_same_level \
			and (AIMethods.n_frosh_level[n_current_level] * (3 if n_booster_4 != 0 else 4)) / (2 if n_booster_4 != 0 else 3) >= AIMethods.n_frosh_target[n_current_level] \
			and AIMethods.n_frosh_level[n_current_level + 1] < AIMethods.n_frosh_level[n_current_level] \
			and 0 == AIMethods.R.randi() % maxi(1, Globals.myGameConditions.get_booster(6)):
			# Climb to the next level!
			s.n_y = closest_frosh_to_pole.n_y + (-1 if s.n_y < closest_frosh_to_pole.n_y else 1)
			s.n_z = closest_frosh_to_pole.n_z
			ai_init_9a(s, closest_frosh_to_pole.n_y > s.n_y)
		# Else if there are too many people on my side of the pole
		elif (s.n_x < AIDefine.D_POLE_X and AIMethods.n_frosh_level_l[n_current_level] - 2 > AIMethods.n_frosh_level_r[n_current_level]) \
			or (s.n_x > AIDefine.D_POLE_X and AIMethods.n_frosh_level_r[n_current_level] - 2 > AIMethods.n_frosh_level_l[n_current_level]):
			# Move to the other side
			if s.n_x < AIDefine.D_POLE_X:
				s.n_dest_x = AIDefine.D_POLE_X + (AIDefine.D_POLE_X - s.n_x)
			else:
				s.n_dest_x = AIDefine.D_POLE_X - (s.n_x - AIDefine.D_POLE_X)
			s.n_dest_y = s.n_y
			s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = not s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT]
			ai_init_11b(s)
		# Else if there aren't enough people on this level
		elif AIMethods.n_frosh_level[n_current_level] < AIMethods.n_frosh_target[n_current_level] and 0 == AIMethods.R.randi() % 6:
			# Encourage other frosh to come up
			ai_init_11c(s)

	# Check if frosh should cling to pole
	if s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] == Enums.UpperLevelGoals.UPPER_GOAL_CLING and not b_do_not_cling \
		and abs(s.n_x - AIDefine.D_POLE_X) < 40 and abs(s.n_y - AIDefine.D_POLE_Y) < (AIDefine.D_FROSH_WIDTH_Y * 3 / 2):
		ai_init_14(s)

	if n_support < 1:
		AISupport.ais_send_frosh_flying(s)
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR1_1 + AIMethods.R.randi() % AIDefine.NSPR_FR1])


# === BEHAVIOR 11B: WALKING ACROSS PYRAMID TO BALANCE ===

static func ai_init_11b(s: TSprite) -> void:
	s.nv_z = 0
	s.nv_x = 1 + AIMethods.R.randi() % 4
	s.n_cc = 0
	if s.n_y >= AIDefine.D_POLE_Y and s.n_y <= (AIDefine.D_POLE_Y + 2):
		s.n_y = AIDefine.D_POLE_Y + 3
	s.set_behavior(Callable(AIFrosh, "ai_act_11b"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 11


static func ai_act_11b(s: TSprite) -> void:
	var support: SpriteSet = AISupport.ais_get_frosh_in_range(
		s.n_x - 50, s.n_y - (AIDefine.D_FROSH_ARM_LINK_OFFSET_Y / 2),
		s.n_x + 50, s.n_y + (AIDefine.D_FROSH_ARM_LINK_OFFSET_Y / 2))

	AISupport.ais_move_towards_destination(s)
	match s.n_cc:
		3:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11B_1])
		6:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11B_2])
		9:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11B_3])
		12:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11B_2])
		15:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11B_1])
		18:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11B_1])
		21:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11B_2])
		24:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11B_3])
		27:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11B_2])
		30:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11B_1])
			s.n_cc = 0

	# When the frosh reaches their goal
	if abs(s.n_x - s.n_dest_x) <= s.nv_x and abs(s.n_y - s.n_dest_y) <= s.nv_y:
		# Force them (near)perfectly into position
		s.n_x = s.n_dest_x
		s.n_y = s.n_dest_y
		var n_current_level: int = s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL]

		# If there are too many people on my level
		if AIMethods.n_frosh_level[n_current_level] >= AIMethods.n_frosh_target[n_current_level]:
			# If there are not enough people on the next level
			if n_current_level < 6 and AIMethods.n_frosh_level[n_current_level + 1] < AIMethods.n_frosh_target[n_current_level + 1]:
				# Climb to the next level
				ai_init_10(s)
				s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] = Enums.UpperLevelGoals.UPPER_GOAL_CLIMB
			else:
				# PERFORMANCE BOOST: THEY GOTTA BE KEEN TO JUMP
				if 0 != Globals.myGameConditions.get_booster(7):
					ai_init_2(s)
					if s.n_x < AIDefine.D_POLE_X:
						s.nv_x = -s.nv_x
						s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR2_1 + AIMethods.R.randi() % AIDefine.NSPR_FR2])
		else:
			# Put your arms up
			ai_init_11(s)

	# Make sure the Frosh is supported
	var n_support: int = 0
	var b_same_level: int = 0
	var b_under_pressure: int = 0
	var b_do_not_cling: bool = false
	var n: int = support.get_number_of_sprites()

	for i in range(n):
		if s != support.get_sprite(i):
			if abs(s.n_z - support.get_sprite(i).n_z - 90) < 50:
				n_support += 1
				AISupport.ais_weight_on_shoulders(support.get_sprite(i))
			if abs(s.n_z - support.get_sprite(i).n_z) < 50:
				b_same_level += 1
				if support.get_sprite(i).n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == 14:
					b_do_not_cling = true
			if (support.get_sprite(i).n_z - s.n_z) > 30:
				b_under_pressure += 1

	if s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] == Enums.UpperLevelGoals.UPPER_GOAL_CLING and not b_do_not_cling \
		and abs(s.n_x - AIDefine.D_POLE_X) < 40 and abs(s.n_y - AIDefine.D_POLE_Y) < (AIDefine.D_FROSH_WIDTH_Y * 3 / 2):
		ai_init_14(s)

	if n_support < 1:
		AISupport.ais_send_frosh_flying(s)
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR1_1 + AIMethods.R.randi() % AIDefine.NSPR_FR1])


# === BEHAVIOR 11C: BECKONING FROSH UP ===

static func ai_init_11c(s: TSprite) -> void:
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_11c"))


static func ai_act_11c(s: TSprite) -> void:
	var support: SpriteSet = AISupport.ais_get_frosh_in_range(
		s.n_x - 50, s.n_y - (AIDefine.D_FROSH_ARM_LINK_OFFSET_Y / 2),
		s.n_x + 50, s.n_y + (AIDefine.D_FROSH_ARM_LINK_OFFSET_Y / 2))

	AISupport.ais_move_towards_destination(s)
	match s.n_cc:
		1, 10:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_3])
		5, 15:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_4] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_4])
		20:
			s.n_cc = 0
			s.set_behavior(Callable(AIFrosh, "ai_act_11"))

	# Make sure the Frosh is supported
	var n_support: int = 0
	var n: int = support.get_number_of_sprites()

	for i in range(n):
		if s != support.get_sprite(i):
			if abs(s.n_z - support.get_sprite(i).n_z - 90) < 50:
				n_support += 1
				AISupport.ais_weight_on_shoulders(support.get_sprite(i))

	if n_support < 1:
		AISupport.ais_send_frosh_flying(s)
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR1_1 + AIMethods.R.randi() % AIDefine.NSPR_FR1])


# === BEHAVIOR 11D: EATING PIZZA UP HIGH ===

static func ai_init_11d(s: TSprite) -> void:
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_11d"))


static func ai_act_11d(s: TSprite) -> void:
	AISupport.ais_move_towards_destination(s)
	if 0 == (s.n_cc % AIDefine.TIME_PIZZA_MUNCH):
		match s.n_cc / AIDefine.TIME_PIZZA_MUNCH:
			1:
				if not AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_PIZZAEAT].is_playing():
					AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_PIZZAEAT].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_9] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_9])
			2, 4:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_10] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_10])
			3:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_9] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_9])
			5:
				if 0 != AIMethods.R.randi() % AIDefine.N_PIZZA_MUNCH_AVERAGE:
					s.n_cc = AIDefine.TIME_PIZZA_MUNCH
				else:
					s.n_cc = 0
					s.nv_x = 0
					s.nv_y = 0
					s.n_dest_x = s.n_x
					s.n_dest_y = s.n_y
					s.set_behavior(Callable(AIFrosh, "ai_act_11"))


# === BEHAVIOR 11E: BOOZING UP HIGH ===

static func ai_init_11e(s: TSprite) -> void:
	s.n_cc = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_11e"))


static func ai_act_11e(s: TSprite) -> void:
	AISupport.ais_move_towards_destination(s)
	if 0 == (s.n_cc % AIDefine.BEER_DRINKING_SPEED):
		match s.n_cc / AIDefine.BEER_DRINKING_SPEED:
			1:
				AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CHUG].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
			3, 5, 7, 9:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_5] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_5])
			2, 4, 6, 8, 10:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_6] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_6])
			12:
				if 0 != AIMethods.R.randi() % 2:
					AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CHUGLASTDROP].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_7] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_7])
			24:
				s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR11_8] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR11_8])
				if 0 != AIMethods.R.randi() % 6:
					AIMethods.l_sound[Enums.ASLList.LSND_FROSH_CLARKFINISH1 + AIMethods.R.randi() % SoundbankInfo.NSND_FROSH_CLARKFINISH].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				else:
					AIMethods.l_sound[Enums.ASLList.LSND_FROSH_CLARKFINISH3].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
			32:
				s.n_cc = 0
				s.nv_x = 0
				s.nv_y = 0
				s.n_dest_x = s.n_x
				s.n_dest_y = s.n_y
				s.set_behavior(Callable(AIFrosh, "ai_act_11"))


# === BEHAVIOR 14: CLING TO POLE ===

static func ai_init_14(s: TSprite) -> void:
	s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR14_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR14_1])
	s.n_x = AIDefine.D_POLE_X
	s.n_y = AIDefine.D_POLE_Y + 2
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 0
	s.set_behavior(Callable(AIFrosh, "ai_act_14"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 14


static func ai_act_14(s: TSprite) -> void:
	if 0 == AIMethods.R.randi() % 16:
		s.n_z -= 1
		if s.n_z < 20:
			s.n_cc = AIDefine.TIME_DROP_FROM_CLINGING

	if s.n_cc > AIDefine.TIME_DROP_FROM_CLINGING:
		if s.nv_z < 10:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR14_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR14_3])
		else:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR14_4] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR14_4])
		AISupport.ais_plummet(s)

		# Propagate falling to nearby pole-clingers (chain reaction)
		var support: SpriteSet = AISupport.ais_get_frosh_in_range(
			s.n_x - 50, s.n_y - (AIDefine.D_FROSH_ARM_LINK_OFFSET_Y / 2),
			s.n_x + 50, s.n_y + (AIDefine.D_FROSH_ARM_LINK_OFFSET_Y / 2))
		var n: int = support.get_number_of_sprites()
		if n > 0:
			for i in range(n):
				if abs(s.n_z - support.get_sprite(i).n_z) < 50 \
					and support.get_sprite(i).n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == 14 \
					and s != support.get_sprite(i):
					support.get_sprite(i).n_cc = AIDefine.TIME_DROP_FROM_CLINGING
					support.get_sprite(i).nv_z = s.nv_z

		if s.n_z < AIDefine.D_BELLY_BUTTON_Z and s.nv_z < 0:
			s.n_z = 0
			AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_L, s.n_x, s.n_y))
			AIMethods.ss_water.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_RIPPLE, s.n_x, s.n_y))
			ai_init_3(s)
	else:
		if s.n_cc > AIDefine.TIME_REACH_FOR_CLING:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR14_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR14_2])


# === BEHAVIOR 16: TAM BEHAVIORS ===

static func ai_init_16a(s: TSprite) -> void:
	n_number_of_tugs = 0
	Globals.myGameConditions.add_energy(15)
	s.nv_x = 0
	s.nv_y = 0
	s.nv_z = 0
	s.n_x = AIDefine.D_POLE_X
	s.n_y = AIDefine.D_POLE_Y + 2
	s.n_z = AIDefine.D_TAM_Z + 150
	s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16C_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16C_1])
	s.set_behavior(Callable(AIFrosh, "ai_act_16a"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 16
	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 6


static func ai_act_16a(s: TSprite) -> void:
	AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] = 1
	Globals.myGameConditions.add_energy(2)
	s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16C_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16C_1])

	if 0 == AIMethods.n_frosh_level[5]:
		ai_init_16b(s)
	else:
		if n_number_of_tugs < 3:
			match AIMethods.R.randi() % 3:
				0: ai_init_16d(s)
				_: ai_init_16c(s)
		elif n_number_of_tugs < 8:
			match AIMethods.R.randi() % 5:
				0: ai_init_16c(s)
				1: ai_init_16e(s)
				_: ai_init_16d(s)
		else:
			match AIMethods.R.randi() % 5:
				0: ai_init_16c(s)
				1: ai_init_16d(s)
				_: ai_init_16e(s)


static func ai_init_16b(s: TSprite) -> void:
	n_number_of_tugs += 1
	s.set_behavior(Callable(AIFrosh, "ai_act_16b"))
	s.n_cc = 0
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 16


static func ai_act_16b(s: TSprite) -> void:
	AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] = 1
	if s.n_cc == 1:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16B_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16B_1])
	elif s.n_cc > 25 and s.n_cc < 50 and 0 == AIMethods.R.randi() % 5:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16B_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16B_2])
	if s.n_cc > 50 and s.n_cc < 100 and 0 == AIMethods.R.randi() % 5:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16B_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16B_3])
	if s.n_cc > 100 and 0 == AIMethods.R.randi() % 50:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR14_4] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR14_4])
		s.n_z -= 85
		AISupport.ais_send_frosh_flying(s)
		s.nv_x = 0
		s.nv_y = 0
	if (s.n_cc == 28 or s.n_cc == 52) and 0 != Globals.myGameConditions.get_frosh_lameness() and 0 == AIMethods.R.randi() % 4:
		Globals.myGameConditions.add_energy(60)
		s.set_behavior(Callable(AIFrosh, "ai_act_16a"))


static func ai_init_16c(s: TSprite) -> void:
	n_number_of_tugs += 1
	s.set_behavior(Callable(AIFrosh, "ai_act_16c"))
	s.n_cc = 0


static func ai_act_16c(s: TSprite) -> void:
	AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] = 1
	if s.n_cc == 1:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16C_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16C_1])
	elif s.n_cc == 5:
		b_did_he_make_it = 1 if 0 == AIMethods.R.randi() % 3 else 0
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16C_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16C_2])
		AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_NAILS_IN_TAM] -= 1
	if s.n_cc == 8 and 0 != b_did_he_make_it:
		if AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_NAILS_IN_TAM] >= 0:
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16C_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16C_1])
		else:
			ai_init_16f(s)
	if s.n_cc == 10:
		if 0 != AIMethods.R.randi() % 3:
			s.n_cc = 0
		else:
			s.set_behavior(Callable(AIFrosh, "ai_act_16a"))


static func ai_init_16d(s: TSprite) -> void:
	n_number_of_tugs += 1
	s.set_behavior(Callable(AIFrosh, "ai_act_16d"))
	s.n_cc = 0


static func ai_act_16d(s: TSprite) -> void:
	AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] = 1
	if s.n_cc == 1:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16D_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16D_1])
	elif s.n_cc == 5:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16D_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16D_2])
	elif s.n_cc == 8:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16D_3] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16D_3])
	elif s.n_cc == 15:
		if 0 != AIMethods.R.randi() % 7:
			s.n_cc = 4
			if 0 == AIMethods.R.randi() % 2:
				AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_NAILS_IN_TAM] -= 1
		else:
			s.n_cc = 100
			s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16D_4] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16D_4])
	elif s.n_cc == 110:
		s.set_behavior(Callable(AIFrosh, "ai_act_16a"))


static func ai_init_16e(s: TSprite) -> void:
	n_number_of_tugs += 1
	s.set_behavior(Callable(AIFrosh, "ai_act_16e"))
	s.n_cc = 0
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 16


static func ai_act_16e(s: TSprite) -> void:
	AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] = 1
	if s.n_cc == 1:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16E_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16E_1])
	if s.n_cc == 15:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16E_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16E_2])
	if s.n_cc == 20:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16E_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16E_1])
	if s.n_cc == 28:
		if 0 == AIMethods.R.randi() % 3:
			ai_init_16b(s)
		else:
			s.set_behavior(Callable(AIFrosh, "ai_act_16a"))


static func ai_init_16f(s: TSprite) -> void:
	# They got the Tam. The crowd goes apeshit.
	AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] = 2
	s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 16
	AIMethods.NOSPEECHFOR(500)
	s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] = 0
	AIMethods.spr_prez.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPREZ2_2])
	AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 5
	Globals.myGameConditions.game_over()
	n_new_high_score = int(Globals.TimerService.get_current_game_time_score_milliseconds())
	Globals.Analytic("GameEndTam;" + str(n_new_high_score))

	if n_new_high_score > Globals.myGameConditions.get_high_score(Globals.myGameConditions.get_frosh_lameness()):
		AIMethods.l_sound[Enums.ASLList.LSND_NARRATOR_CONGRATS].play(SoundbankInfo.VOL_HOLLAR)
		Globals.myGameConditions.set_high_score(Globals.myGameConditions.get_frosh_lameness(), n_new_high_score)
	else:
		AIMethods.l_sound[Enums.ASLList.LSND_FROSH_GOTTAM1].play(SoundbankInfo.VOL_HOLLAR)

	s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16F_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16F_1])
	AISupport.ais_start_a_mosh()
	AIMethods.spr_frecs_r.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = AIDefine.ENERGY_SLAM + 150
	AIMethods.spr_frecs_c.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = AIDefine.ENERGY_SLAM - 20
	AIMethods.spr_frecs_l.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = AIDefine.ENERGY_SLAM + 150
	AICrowd.ais_set_frec_action(AIMethods.spr_frecs_l, Enums.CrowdActions.FA_SLAMMING)
	AICrowd.ais_set_frec_action(AIMethods.spr_frecs_c, Enums.CrowdActions.FA_CHEERING)
	AICrowd.ais_set_frec_action(AIMethods.spr_frecs_r, Enums.CrowdActions.FA_SLAMMING)
	s.set_behavior(Callable(AIFrosh, "ai_act_16f"))
	s.n_cc = 0


static func ai_act_16f(s: TSprite) -> void:
	AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] = 1
	if Globals.myGameConditions.is_pop_boy_in_pit():
		AIMethods.spr_pop_boy.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 7
	if 0 == (s.n_cc % 15) and 0 == AIMethods.R.randi() % 3:
		AIMethods.l_sound[Enums.ASLList.LSND_FROSH_GOTTAM2 + AIMethods.R.randi() % 2].play(SoundbankInfo.VOL_HOLLAR)
	if s.n_cc == 56 or s.n_cc == 200:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16F_2] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16F_2])
	if s.n_cc == 56:
		if Globals.myGameConditions.is_pop_boy_in_pit():
			AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_GOTTAM1].play(SoundbankInfo.VOL_HOLLAR)
	if s.n_cc > 58:
		s.n_z -= 2
	if s.n_cc == 120:
		s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR16F_1] if s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] else AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR16F_1])
	if s.n_cc == 275:
		Globals.GameLoop.change_game_state(Enums.GameStates.STATETITLE)


# === POP BOY IN CROWD ===

const TIME_POPBOY_JUMP: int = 70
const TIME_AL_CROUCH: int = 12

static func ai_pop_boy_in_crowd(s: TSprite) -> void:
	if s.n_cc < TIME_POPBOY_JUMP or AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_CHEER1].is_playing():
		match s.n_cc % 51:
			2: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY1_2])  # YEAH!!
			4:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY1_1])
				AIMethods.l_sound[Enums.ASLList.LSND_EXAM_TOSS1].stop()
			12:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY1_2])  # YEAH!!
				AIMethods.l_sound[Enums.ASLList.LSND_EXAM_TOSS1].stop()
			14: s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpPOPBOY1_3])
			22: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY1_3])
			24: s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpPOPBOY1_2])  # YEAH!!
			26: s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpPOPBOY1_1])
			34: s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpPOPBOY1_2])
			41: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY1_3])
			43: s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpPOPBOY1_3])
			51: s.n_cc = 0
	else:
		if s.n_cc == TIME_POPBOY_JUMP - 10:
			AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_CHEER1].play(SoundbankInfo.VOL_CROWD, AICrowd.pan_on_x(s))
		elif s.n_cc == TIME_POPBOY_JUMP:
			AISupport.ais_think_for_al(s, false)
		else:
			# It is time
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY5_2])
			if s.n_cc < 1000:
				s.n_cc = 1000
			if s.n_cc == 1002:
				AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_CHEER2].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
			if s.n_cc == 1000 + TIME_AL_CROUCH:
				s.b_deleted = true
				Globals.myGameConditions.pop_boy_jumps_in()
				AIMethods.spr_pole.frm_frame.n_z2 -= 127  # HACKUS SUPREMUS
				AIMethods.spr_pop_boy = SpriteInit.create_sprite(Enums.SpriteType.SPR_POP_BOY, s.n_x, 0)
				AISupport.ais_unlock_achievement(10)

				s.n_x += 20
				s.n_y += 60
				AISupport.ais_think_for_al(s)
				s.n_x -= 20
				s.n_y -= 60
				AIMethods.spr_pop_boy.n_z = 26  # This was originally calculated
				AIMethods.spr_pop_boy.nv_z = 16
				AIMethods.spr_pop_boy.nv_x = 2
				AIMethods.spr_pop_boy.nv_y = 6
				AIMethods.ss_pit.include(AIMethods.spr_pop_boy)


# === POP BOY (IN PIT) ===

static func ai_pop_boy(s: TSprite) -> void:
	match s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR]:
		1:  # Leaping into pit
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY5_1])
			AISupport.ais_plummet(s)
			if s.n_z <= 0:
				AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_L, s.n_x, s.n_y))
				AIMethods.NOSPEECHFOR(200)
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY2_1])
				AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_GREETING1 + AIMethods.R.randi() % 2].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 2
				s.n_cc = 0
				s.n_z = 0
		2:  # Beckoning (Greeting)
			match s.n_cc % 40:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY2_1])
				4: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY2_2])
				11: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY2_1])
				14: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY2_2])
				21: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY2_4])
				24: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY2_3])
				31: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY2_4])
				34: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY2_3])
			if not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_GREETING1].is_playing() and \
			   not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_GREETING2].is_playing():
				s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 5
				s.n_cc = 0
				s.n_attrib[Enums.NAttrFrosh.ATTR_MOTIVATION] = AIMethods.R.randi() % 3
		3:  # Walking to his spot
			match s.n_cc % 40:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY7_2])
				8: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY7_1])
				21: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY7_3])
				28: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY7_4])
			if s.n_x < AIDefine.D_POLE_X:
				s.n_x += 2
			elif s.n_x > AIDefine.D_POLE_X:
				s.n_x -= 2
			if s.n_y < AIDefine.D_POLE_Y + 1:
				s.n_y += 1
			elif s.n_y > AIDefine.D_POLE_Y + 1:
				s.n_y -= 1
			if s.n_x == AIDefine.D_POLE_X and s.n_y == AIDefine.D_POLE_Y + 1:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY6_1])
				s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 4
				s.n_cc = 0
		4:  # Base of Pole
			if (0 == AIMethods.R.randi() % 70) and s.n_cc > 30:
				s.n_cc = 0
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY6_1 + AIMethods.R.randi() % 2])
		5:  # Teaching
			match s.n_cc % 46:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY4_2])
				6: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY4_1])
				21: s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpPOPBOY4_2])
				26: s.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpPOPBOY4_1])
			if s.n_cc == 12:
				AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_ADVICE1 + s.n_attrib[Enums.NAttrFrosh.ATTR_MOTIVATION]].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				AIMethods.l_sound[Enums.ASLList.LSND_EXAM_TOSS1].stop()
				s.n_attrib[Enums.NAttrFrosh.ATTR_MOTIVATION] += 1
				s.n_attrib[Enums.NAttrFrosh.ATTR_MOTIVATION] %= 6
				AIMethods.NOSPEECHFOR(102)
				AISupport.ais_think_for_al(s)
			elif s.n_cc > 24 and \
				 not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_ADVICE1].is_playing() and \
				 not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_ADVICE2].is_playing() and \
				 not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_ADVICE3].is_playing() and \
				 not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_ADVICE4].is_playing() and \
				 not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_ADVICE5].is_playing() and \
				 not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_ADVICE6].is_playing():
				s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 3
				s.n_cc = 0
		6:  # Booze up
			match s.n_cc:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY3_4])
				6:
					AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CHUG].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY3_3])
				27:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY3_1])
					AIMethods.l_sound[Enums.ASLList.LSND_FROSH_CLARKFINISH1 + AIMethods.R.randi() % SoundbankInfo.NSND_FROSH_CLARKFINISH].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				43:
					Globals.Analytic("PopBoyBeer")
					s.n_attrib[Enums.NAttrFrosh.ATTR_MIND_SET] += 1
					s.n_attrib[Enums.NAttrFrosh.ATTR_MIND_SET] %= (1 if Globals.myGameConditions.is_ritual() else 3)
					AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_BEER1 + s.n_attrib[Enums.NAttrFrosh.ATTR_MIND_SET]].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY3_2])
					AIMethods.NOSPEECHFOR(80)
				60:
					s.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 3
					s.n_cc = 0
		7:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPOPBOY7_1])
