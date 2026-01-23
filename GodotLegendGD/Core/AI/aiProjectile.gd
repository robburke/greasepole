class_name AIProjectile

# aiProjectile.gd - Static class with projectile behaviors
# Ported from aiProjectile.cs
#
# Handles:
# - Apple, pizza, Clark mug, exam, and grease hose projectiles
# - Player arm/hand control and throwing
# - Collision detection with targets (aliens, frosh, FRECs, prez, etc.)
# - Iron ring special weapon

# === STATIC VARIABLES ===
# Per C#: these were "public static" in the partial class

static var n_temp: int = 0
static var n_demo_old_mouse_x: int = 0
static var n_demo_old_mouse_y: int = 0
static var n_mouse_x: int = 0
static var n_mouse_y: int = 0
static var n_toss_power: int = 0
static var has_tossed_114_exam: bool = false

# === CONSTANTS ===
const RING_UP_START_TIME: int = 375


# === PROJECTILE REBOUND ===

static func ais_projectile_rebound(s: TSprite) -> void:
	# Alter the behaviour of a Projectile as it goes "on the rebound."
	match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
		Enums.ProjTypes.PROJ_APPLE, Enums.ProjTypes.PROJ_CLARK, Enums.ProjTypes.PROJ_PIZZA, Enums.ProjTypes.PROJ_EXAM:
			s.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET] = Enums.AttrAppleHitTargetConstants.ATTR_FLYING_REBOUNDING
			AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_WHAP,
				s.n_scr_x + s.frm_frame.hotspot_x,
				s.n_scr_y + s.frm_frame.hotspot_y))
			if s.n_x < AIDefine.D_POLE_X:
				s.nv_x = AIMethods.randintin(-4, 0) * 20
			else:
				s.nv_x = AIMethods.randintin(0, 4) * 20
			s.nv_y = 0
			s.nv_z = AIMethods.R.randi() % 40 + 10
		Enums.ProjTypes.PROJ_GREASE:
			ais_create_hose_whap(s.n_scr_x + s.frm_frame.hotspot_x, s.n_scr_y + s.frm_frame.hotspot_y, 2)
			s.b_deleted = true
			# Send the grease gooping down.


static func ais_create_hose_whap(n_x: int, n_y: int, n_distance: int) -> void:
	var spr_tmp: TSprite
	spr_tmp = SpriteInit.create_sprite(Enums.SpriteType.SPR_HOSE_WHAP, n_x, n_y)
	match n_distance:
		1: spr_tmp.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHSPLASH1_1 + AIMethods.R.randi() % 5])
		2: spr_tmp.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHSPLASH2_1 + AIMethods.R.randi() % 5])
		_: spr_tmp.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHSPLASH3_1 + AIMethods.R.randi() % 5])
	AIMethods.ss_console.include(spr_tmp)


static func ais_run_away_from(s: TSprite) -> void:
	# Make the Frosh bolt from s's nDestx-position
	var n: int = AIMethods.ss_fr.get_number_of_sprites()
	for i in range(n):
		var frosh: TSprite = AIMethods.ss_fr.get_sprite(i)
		if frosh.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == 4 or frosh.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == 7:
			AISupport.ais_send_frosh_flying(frosh)
			frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_THINK
			frosh.b_attrib[Enums.BAttrFrosh.ATTR_EXCITED] = true
			AIFrosh.ai_init_4(frosh)
			if s.n_dest_x < frosh.n_x:
				frosh.n_dest_x = frosh.n_x + (AIDefine.D_PIT_MAX_X - AIDefine.D_PIT_MIN_X) + 40
			else:
				frosh.n_dest_x = frosh.n_x - (AIDefine.D_PIT_MAX_X - AIDefine.D_PIT_MIN_X) - 40
			if frosh.n_dest_x > AIDefine.D_PIT_MAX_X + 100:
				frosh.n_dest_x = AIDefine.D_PIT_MAX_X + 100
			if frosh.n_dest_x < -100:
				frosh.n_dest_x = -100


# === MAIN PROJECTILE BEHAVIOR ===

static func ai_projectile(s: TSprite) -> void:
	match s.n_attrib[Enums.AttrProjectile.ATTR_HIT_TARGET]:
		Enums.AttrAppleHitTargetConstants.ATTR_FLYING_REBOUNDING:
			AISupport.ais_plummet(s)
			match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
				Enums.ProjTypes.PROJ_APPLE:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAPPLE5_1 + AIMethods.R.randi() % AIDefine.NSPR_APPLE5])
				Enums.ProjTypes.PROJ_EXAM:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpEXAM4_1 + AIMethods.R.randi() % AIDefine.NSPR_EXAM4])
				Enums.ProjTypes.PROJ_CLARK:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLARK5B_1 + AIMethods.R.randi() % AIDefine.NSPR_CLARK5B])
				Enums.ProjTypes.PROJ_PIZZA:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPIZZA4_1 + AIMethods.R.randi() % AIDefine.NSPR_PIZZA4])
				Enums.ProjTypes.PROJ_GREASE:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHSPLASH1_1])
			if s.n_scr_y > 480:
				s.b_deleted = true

		Enums.AttrAppleHitTargetConstants.ATTR_FLYING_TOWARD_TARGET:
			_ai_projectile_flying_toward_target(s)


static func _ai_projectile_flying_toward_target(s: TSprite) -> void:
	# Flying toward target - frame-by-frame animation based on nCC
	match s.n_cc:
		1:
			if s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE] == Enums.ProjTypes.PROJ_GREASE:
				s.n_x = s.n_attrib[Enums.AttrProjectile.ATTR_START_X]
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHOSEBALL1])
		3:
			# Frame two
			s.n_x = s.n_dest_x - ((s.n_dest_x - s.n_attrib[Enums.AttrProjectile.ATTR_START_X]) * 2 / 3)
			s.n_y = s.n_dest_y + absi((s.n_dest_y - s.n_attrib[Enums.AttrProjectile.ATTR_START_Y]) / 4)
			match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
				Enums.ProjTypes.PROJ_APPLE:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAPPLE3_1 + AIMethods.R.randi() % AIDefine.NSPR_APPLE3])
				Enums.ProjTypes.PROJ_PIZZA:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPIZZA3_1 + AIMethods.R.randi() % AIDefine.NSPR_PIZZA3])
				Enums.ProjTypes.PROJ_CLARK:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLARK3_1 + AIMethods.R.randi() % AIDefine.NSPR_CLARK3])
				Enums.ProjTypes.PROJ_GREASE:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHOSEBALL1])
				Enums.ProjTypes.PROJ_EXAM:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpEXAM3_1 + AIMethods.R.randi() % AIDefine.NSPR_EXAM3])
					ais_run_away_from(s)
					if not AIMethods.l_sound[Enums.ASLList.LSND_EXAM_TOSS1].is_playing():
						AIMethods.l_sound[Enums.ASLList.LSND_EXAM_TOSS1].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
		4:
			# Frame two.5
			s.n_x = s.n_dest_x - ((s.n_dest_x - s.n_attrib[Enums.AttrProjectile.ATTR_START_X]) * 3 / 10)
			s.n_y = s.n_dest_y
		5:
			# Frame three
			s.n_x = s.n_dest_x - ((s.n_dest_x - s.n_attrib[Enums.AttrProjectile.ATTR_START_X]) / 5)
			s.n_y = s.n_dest_y - absi((s.n_dest_y - s.n_attrib[Enums.AttrProjectile.ATTR_START_Y]) / 4)
			match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
				Enums.ProjTypes.PROJ_APPLE:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAPPLE3_1 + AIMethods.R.randi() % AIDefine.NSPR_APPLE3])
				Enums.ProjTypes.PROJ_PIZZA:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPIZZA3_1 + AIMethods.R.randi() % AIDefine.NSPR_PIZZA3])
				Enums.ProjTypes.PROJ_CLARK:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLARK3_1 + AIMethods.R.randi() % AIDefine.NSPR_CLARK3])
				Enums.ProjTypes.PROJ_GREASE:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHOSEBALL2])
				Enums.ProjTypes.PROJ_EXAM:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpEXAM3_1 + AIMethods.R.randi() % AIDefine.NSPR_EXAM3])
		6:
			# Frame four
			s.n_x = s.n_dest_x - ((s.n_dest_x - s.n_attrib[Enums.AttrProjectile.ATTR_START_X]) / 10)
			s.n_y = s.n_dest_y - absi((s.n_dest_y - s.n_attrib[Enums.AttrProjectile.ATTR_START_Y]) / 8)
			match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
				Enums.ProjTypes.PROJ_APPLE:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAPPLE4_1 + AIMethods.R.randi() % AIDefine.NSPR_APPLE4])
				Enums.ProjTypes.PROJ_PIZZA:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPIZZA4_1 + AIMethods.R.randi() % AIDefine.NSPR_PIZZA4])
				Enums.ProjTypes.PROJ_CLARK:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLARK4_1 + AIMethods.R.randi() % AIDefine.NSPR_CLARK4])
				Enums.ProjTypes.PROJ_GREASE:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHOSEBALL3])
				Enums.ProjTypes.PROJ_EXAM:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpEXAM4_1 + AIMethods.R.randi() % AIDefine.NSPR_EXAM4])
		7:
			# Frame four.5
			s.n_x = s.n_dest_x
			s.n_y = s.n_dest_y
		8:
			# Frame five - TEST FOR COLLISION WITH ALIENS
			_ai_projectile_frame_5_aliens(s)
		9:
			# Frame six - TEST FOR COLLISION WITH HIPPO AND FROSH AND POPBOY
			_ai_projectile_frame_6_hippo_frosh_popboy(s)
		10:
			# Frame seven - Test for collision with FRECS and POLE and PREZ
			_ai_projectile_frame_7_frecs_pole_prez(s)
		11:
			# Frame eight - Hit water or bank
			_ai_projectile_frame_8_water(s)
		12:
			s.n_y += 4
		13:
			s.n_y += 7
		14:
			s.n_y += 10
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGLINT_1 + AIMethods.R.randi() % 2])
		15:
			s.b_deleted = true


static func _ai_projectile_frame_5_aliens(s: TSprite) -> void:
	# Frame five - Test for collision with aliens
	match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
		Enums.ProjTypes.PROJ_APPLE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAPPLE5_1 + AIMethods.R.randi() % AIDefine.NSPR_APPLE5])
			if AIMethods.spr_alien != null and AISupport.ais_scr_point_inside(AIMethods.spr_alien, s.n_scr_x, s.n_scr_y):
				AISupport.ais_unlock_achievement(99)
				AIMethods.spr_alien.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
				if AISupport.ais_forge_trick(1, 2000) and Globals.myGameConditions.is_ritual():
					AIMethods.l_sound[Enums.ASLList.LSND_FROSH_APPLEHIT2].play(SoundbankInfo.VOL_HOLLAR, SoundbankInfo.PAN_CENTER)
				ais_projectile_rebound(s)
		Enums.ProjTypes.PROJ_PIZZA:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPIZZA5_1 + AIMethods.R.randi() % AIDefine.NSPR_PIZZA5])
			if AIMethods.spr_alien != null and AISupport.ais_scr_point_inside(AIMethods.spr_alien, s.n_scr_x, s.n_scr_y):
				AIMethods.spr_alien.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
				AISupport.ais_forge_trick(5, 300)  # Pizza at Alien
				ais_projectile_rebound(s)
		Enums.ProjTypes.PROJ_CLARK:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLARK5A_1 + AIMethods.R.randi() % AIDefine.NSPR_CLARK5A])
			if AIMethods.spr_alien != null and AISupport.ais_scr_point_inside(AIMethods.spr_alien, s.n_scr_x, s.n_scr_y):
				AIMethods.spr_alien.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
				AISupport.ais_forge_trick(4, 300)  # Beer at Alien
				ais_projectile_rebound(s)
		Enums.ProjTypes.PROJ_GREASE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHOSEBALL4])
			if AIMethods.spr_alien != null and AISupport.ais_scr_point_inside(AIMethods.spr_alien, s.n_scr_x, s.n_scr_y):
				AIMethods.spr_alien.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
				AISupport.ais_forge_trick(2, 800)  # Hose at Alien
				ais_projectile_rebound(s)
		Enums.ProjTypes.PROJ_EXAM:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpEXAM4_1 + AIMethods.R.randi() % AIDefine.NSPR_EXAM4])
			if AIMethods.spr_alien != null and AISupport.ais_scr_point_inside(AIMethods.spr_alien, s.n_scr_x, s.n_scr_y):
				AIMethods.spr_alien.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
				AISupport.ais_forge_trick(3, 800)  # Exam at Alien
				ais_projectile_rebound(s)


static func _ai_projectile_frame_6_hippo_frosh_popboy(s: TSprite) -> void:
	# Frame six - Test for collision with Hippo, Frosh, and PopBoy
	match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
		Enums.ProjTypes.PROJ_APPLE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAPPLE6_1 + AIMethods.R.randi() % AIDefine.NSPR_APPLE6])
			if AIMethods.spr_gw_hippo != null and AISupport.ais_scr_point_inside(AIMethods.spr_gw_hippo, s.n_scr_x, s.n_scr_y):
				AIMethods.spr_gw_hippo.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
				if AIMethods.spr_gw_hippo.n_z > 100:
					s.b_deleted = true
				else:
					ais_projectile_rebound(s)
				Globals.myGameConditions.add_energy(50)
			elif ais_collision_to_response(s, AIMethods.ss_fr, AISupport.ais_send_frosh_really_flying, AIDefine.INCLUDE_WHAP, AIDefine.POLE_ACTS_AS_SHIELD):
				ais_projectile_rebound(s)
				Globals.myGameConditions.add_energy(25)
			elif AIMethods.spr_pop_boy != null and AISupport.ais_scr_point_inside(AIMethods.spr_pop_boy, s.n_scr_x, s.n_scr_y):
				AISupport.ais_forge_trick(10, -500)  # Hit Al with apples
				ais_projectile_rebound(s)
				# Play popboy apple hit sound
				if AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_APPLE1].is_playing() and Globals.myGameConditions.is_ritual():
					AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_APPLE1].stop()
					AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_APPLER1].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				else:
					if not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_APPLE1].is_playing() and not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_APPLER1].is_playing():
						for i in range(Enums.ASLList.LSND_POPBOY_ADVICE1, Enums.ASLList.LSND_POPBOY_PIZZA2 + 1):
							AIMethods.l_sound[i].stop()
						if Globals.myGameConditions.is_ritual() and AIMethods.R.randi() % 2 != 0:
							AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_APPLER1].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
						else:
							AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_APPLE1].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				Globals.myGameConditions.add_energy(50)

		Enums.ProjTypes.PROJ_PIZZA:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPIZZA6_1 + AIMethods.R.randi() % AIDefine.NSPR_PIZZA6])
			if AIMethods.spr_gw_hippo != null and AISupport.ais_scr_point_inside(AIMethods.spr_gw_hippo, s.n_scr_x, s.n_scr_y):
				AIMethods.spr_gw_hippo.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
				if AIMethods.spr_gw_hippo.n_z > 100:
					s.b_deleted = true
				else:
					ais_projectile_rebound(s)
				Globals.myGameConditions.add_energy(50)
			elif ais_special_collision_to_response(s, AIMethods.ss_fr, AISupport.ais_send_frosh_really_flying, AIDefine.NO_WHAP, AIDefine.POLE_ACTS_AS_SHIELD, true, AIFrosh.ai_init_11d, AIFrosh.ai_init_5a):
				Globals.myGameConditions.add_energy(20)
			elif AIMethods.spr_pop_boy != null and AISupport.ais_scr_point_inside(AIMethods.spr_pop_boy, s.n_scr_x, s.n_scr_y):
				AISupport.ais_forge_trick(11, 500)  # Feed Al pizza
				s.b_deleted = true
				Globals.myGameConditions.add_energy(50)
				# Popboy pizza sound
				if not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_PIZZA1].is_playing() and not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_PIZZA2].is_playing():
					for i in range(Enums.ASLList.LSND_POPBOY_ADVICE1, Enums.ASLList.LSND_POPBOY_HIPPOR1 + 1):
						AIMethods.l_sound[i].stop()
					AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_PIZZA1 + AIMethods.R.randi() % 2].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))

		Enums.ProjTypes.PROJ_CLARK:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLARK6_1 + AIMethods.R.randi() % AIDefine.NSPR_CLARK6])
			if AIMethods.spr_gw_hippo != null and AISupport.ais_scr_point_inside(AIMethods.spr_gw_hippo, s.n_scr_x, s.n_scr_y):
				AIMethods.spr_gw_hippo.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
				if AIMethods.spr_gw_hippo.n_z > 100:
					s.b_deleted = true
				Globals.myGameConditions.add_energy(70)
			elif ais_special_collision_to_response(s, AIMethods.ss_fr, AISupport.ais_send_frosh_really_flying, AIDefine.NO_WHAP, AIDefine.POLE_ACTS_AS_SHIELD, true, AIFrosh.ai_init_11e, AIFrosh.ai_init_5b):
				Globals.myGameConditions.add_energy(20)
			elif AIMethods.spr_pop_boy != null and AIMethods.spr_pop_boy.n_z < 2 and AISupport.ais_scr_point_inside(AIMethods.spr_pop_boy, s.n_scr_x, s.n_scr_y):
				AISupport.ais_unlock_achievement(1999)
				if not AISupport.ais_forge_trick(12, 2000):  # Feed Al Beer first time
					AISupport.ais_forge_trick(13, 1000)  # Feed Al Beer second time
				s.b_deleted = true
				Globals.myGameConditions.add_energy(50)
				# Popboy beer collision - stop other sounds, set behavior to booze up
				if not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_BEER1].is_playing() and not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_BEERR2].is_playing() and not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_BEERR3].is_playing():
					for i in range(Enums.ASLList.LSND_POPBOY_ADVICE1, Enums.ASLList.LSND_POPBOY_PIZZA2 + 1):
						AIMethods.l_sound[i].stop()
					AIMethods.spr_pop_boy.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] = 6
					AIMethods.spr_pop_boy.n_cc = 0

		Enums.ProjTypes.PROJ_GREASE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHOSEBALL5])
			if AIMethods.spr_gw_hippo != null and AISupport.ais_scr_point_inside(AIMethods.spr_gw_hippo, s.n_scr_x, s.n_scr_y):
				if AIMethods.spr_gw_hippo.n_z > 100:
					s.b_deleted = true
				else:
					ais_create_hose_whap(s.n_scr_x + s.frm_frame.hotspot_x, s.n_scr_y + s.frm_frame.hotspot_y, 1)
				Globals.myGameConditions.add_energy(3)
			elif ais_collision_to_response(s, AIMethods.ss_fr, AISupport.ais_send_frosh_really_flying, AIDefine.NO_WHAP, AIDefine.POLE_ACTS_AS_SHIELD, false):
				ais_create_hose_whap(s.n_scr_x + s.frm_frame.hotspot_x, s.n_scr_y + s.frm_frame.hotspot_y, 1)
				s.b_deleted = true
				Globals.myGameConditions.add_energy(3)

		Enums.ProjTypes.PROJ_EXAM:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpEXAM4_1 + AIMethods.R.randi() % AIDefine.NSPR_EXAM4])
			if AIMethods.spr_gw_hippo != null and AISupport.ais_scr_point_inside(AIMethods.spr_gw_hippo, s.n_scr_x, s.n_scr_y):
				AIMethods.spr_gw_hippo.b_attrib[Enums.BAttrForeGroundPopUpDudes.ATTR_BEING_ATTACKED] = true
				if AIMethods.spr_gw_hippo.n_z > 100:
					s.b_deleted = true
				else:
					ais_projectile_rebound(s)
				Globals.myGameConditions.add_energy(150)
			elif AIMethods.spr_pop_boy != null and AISupport.ais_scr_point_inside(AIMethods.spr_pop_boy, s.n_scr_x, s.n_scr_y):
				ais_projectile_rebound(s)
				Globals.myGameConditions.add_energy(50)
				# Popboy exam sound
				if not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_EXAM1].is_playing() and not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_EXAM2].is_playing() and not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_EXAM3].is_playing():
					for i in range(Enums.ASLList.LSND_POPBOY_ADVICE1, Enums.ASLList.LSND_POPBOY_HIPPOR1 + 1):
						AIMethods.l_sound[i].stop()
					AIMethods.spr_pop_boy.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] += 1
					AIMethods.spr_pop_boy.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] %= 3
					AIMethods.l_sound[Enums.ASLList.LSND_EXAM_TOSS1].stop()
					AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_EXAM1 + AIMethods.spr_pop_boy.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL]].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
			elif ais_collision_to_response(s, AIMethods.ss_fr, AISupport.ais_send_frosh_really_flying, AIDefine.INCLUDE_WHAP, AIDefine.POLE_ACTS_AS_SHIELD):
				ais_projectile_rebound(s)
				Globals.myGameConditions.add_energy(70)


static func _ai_projectile_frame_7_frecs_pole_prez(s: TSprite) -> void:
	# Frame seven - Test for collision with FRECS, POLE, and PREZ

	# Test for pole collision
	if s.n_x > (AIDefine.D_POLE_X - AIDefine.D_POLE_WIDTH) and s.n_x < (AIDefine.D_POLE_X + AIDefine.D_POLE_WIDTH):
		if AIMethods.spr_tam != null and s.n_scr_y > AIMethods.spr_tam.n_scr_y and s.n_scr_y < (AIMethods.spr_tam.n_scr_y + AIDefine.D_POLE_HEIGHT):
			if s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE] != Enums.ProjTypes.PROJ_GREASE:
				ais_projectile_rebound(s)  # POLE
			else:
				ais_create_hose_whap(s.n_scr_x + s.frm_frame.hotspot_x, s.n_scr_y + s.frm_frame.hotspot_y, 1)
				s.b_deleted = true
			return

	# FRECS collision
	s.calculate_screen_coordinates(Globals.myLayers.get_offset(Enums.LayerNames.LAYER_PIT))

	# FRECS LEFT
	if AIMethods.spr_frecs_l != null and AISupport.ais_scr_point_inside(AIMethods.spr_frecs_l, s.n_scr_x, s.n_scr_y):
		_handle_frecs_collision(s, AIMethods.spr_frecs_l)

	# FRECS CENTER
	if AIMethods.spr_frecs_c != null and AISupport.ais_scr_point_inside(AIMethods.spr_frecs_c, s.n_scr_x, s.n_scr_y):
		_handle_frecs_collision(s, AIMethods.spr_frecs_c)

	# FRECS RIGHT
	if AIMethods.spr_frecs_r != null and AISupport.ais_scr_point_inside(AIMethods.spr_frecs_r, s.n_scr_x, s.n_scr_y):
		_handle_frecs_collision(s, AIMethods.spr_frecs_r)

	# PREZ collision
	if AIMethods.spr_prez != null:
		if AISupport.ais_scr_point_inside(AIMethods.spr_prez, s.n_scr_x, s.n_scr_y) or AISupport.ais_scr_point_inside(AIMethods.spr_prez, s.n_scr_x + 30, s.n_scr_y):
			_handle_prez_collision(s)

	# Update frame based on projectile type
	match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
		Enums.ProjTypes.PROJ_APPLE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAPPLE7_1 + AIMethods.R.randi() % AIDefine.NSPR_APPLE7])
		Enums.ProjTypes.PROJ_CLARK:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLARK7_1 + AIMethods.R.randi() % AIDefine.NSPR_CLARK7])
		Enums.ProjTypes.PROJ_PIZZA:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPIZZA7_1 + AIMethods.R.randi() % AIDefine.NSPR_PIZZA7])
		Enums.ProjTypes.PROJ_GREASE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHOSEBALL6])
		Enums.ProjTypes.PROJ_EXAM:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpEXAM4_1 + AIMethods.R.randi() % AIDefine.NSPR_EXAM4])


static func _handle_frecs_collision(s: TSprite, spr_frecs: TSprite) -> void:
	match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
		Enums.ProjTypes.PROJ_APPLE:
			AIMethods.spr_forge.n_attrib[Enums.NAttrForge.ATTR_FORGE_ENERGY] -= 600
			AISupport.ais_set_frec_action(spr_frecs, Enums.AttrCrowdActions.FA_BLOCKING)
			AIMethods.l_sound[Enums.ASLList.LSND_FRECS_HITAPPLE1 + AIMethods.R.randi() % SoundbankInfo.NSND_FRECS_HIT_APPLE].play(SoundbankInfo.VOL_HOLLAR, (spr_frecs.n_x - 320) / 32)
			ais_projectile_rebound(s)
		Enums.ProjTypes.PROJ_CLARK:
			if not AISupport.ais_forge_trick(9, 200):  # Beer at crowd 1
				AISupport.ais_forge_trick(15, 300)  # Beer at crowd 2
			spr_frecs.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += 160
			if spr_frecs.n_attrib[Enums.NAttrCrowd.ATTR_F_ACTION] == Enums.AttrCrowdActions.FA_SLAMMING:
				AISupport.ais_unlock_achievement(2000)
				AISupport.ais_forge_trick(16, 2000)  # Beer at crowd HAMMERED
			var n_temp: int = AIMethods.R.randi() % SoundbankInfo.NSND_FRECS_ROAR
			if not AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDROAR1 + n_temp].is_playing():
				AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDROAR1 + n_temp].play(SoundbankInfo.VOL_CROWD, (spr_frecs.n_x - 320) / 32)
			s.b_deleted = true
		Enums.ProjTypes.PROJ_PIZZA:
			spr_frecs.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += 60
			var n_cheer: int = AIMethods.R.randi() % SoundbankInfo.NSND_FRECS_CHEER
			if not AIMethods.l_sound[Enums.ASLList.LSND_FRECS_CHEER1 + n_cheer].is_playing():
				AIMethods.l_sound[Enums.ASLList.LSND_FRECS_CHEER1 + n_cheer].play(SoundbankInfo.VOL_HOLLAR, (spr_frecs.n_x - 320) / 32)
			s.b_deleted = true
		Enums.ProjTypes.PROJ_GREASE:
			spr_frecs.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += 3
			AISupport.ais_set_frec_action(spr_frecs, Enums.AttrCrowdActions.FA_BLOCKING)
			ais_create_hose_whap(s.n_scr_x + s.frm_frame.hotspot_x, s.n_scr_y + s.frm_frame.hotspot_y, 1)
			s.b_deleted = true
		Enums.ProjTypes.PROJ_EXAM:
			spr_frecs.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += 50
			AISupport.ais_set_frec_action(spr_frecs, Enums.AttrCrowdActions.FA_BLOCKING)
			AIMethods.l_sound[Enums.ASLList.LSND_EXAM_TOSS1].stop()
			AIMethods.l_sound[Enums.ASLList.LSND_FRECS_HITEXAM1 + AIMethods.R.randi() % SoundbankInfo.NSND_FRECS_HIT_EXAM].play(SoundbankInfo.VOL_HOLLAR, (spr_frecs.n_x - 320) / 32)
			ais_projectile_rebound(s)


static func _handle_prez_collision(s: TSprite) -> void:
	var n_hit_number: int = Globals.myGameConditions.get_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
	var prez_pan: int = (AIMethods.spr_prez.n_x - 320) / 32

	match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
		Enums.ProjTypes.PROJ_APPLE:
			ais_projectile_rebound(s)
			AIMethods.spr_forge.n_attrib[Enums.NAttrForge.ATTR_FORGE_ENERGY] -= 2500
			# Play sound based on hit number
			match n_hit_number:
				0:
					if not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLER4].is_playing() and not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLE4].is_playing():
						AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLE1].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
						AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
						Globals.myGameConditions.add_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
				1:
					if not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLE1].is_playing():
						AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLE2].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
						AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
						Globals.myGameConditions.add_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
				2:
					if not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLE2].is_playing():
						AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLE3].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
						AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
						Globals.myGameConditions.add_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
				3:
					if not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLE3].is_playing():
						var sound_idx: int = Enums.ASLList.LSND_PREZ_HITAPPLER4 if Globals.myGameConditions.is_ritual() else Enums.ASLList.LSND_PREZ_HITAPPLE4
						AIMethods.l_sound[sound_idx].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
						AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
						Globals.myGameConditions.add_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
				_:
					if not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLER4].is_playing() and not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLE4].is_playing():
						AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITAPPLE5].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
						AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
						Globals.myGameConditions.reset_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
						# Trigger Sci-Con push (tri pub ban sequence)
						AIMethods.spr_pole.set_behavior(Callable(AIPopUp, "ai_push_scicon_m"))
						AIMethods.spr_pole.n_cc = 43
		Enums.ProjTypes.PROJ_CLARK:
			AISupport.ais_forge_trick(6, 1000)  # Beer at George
			AISupport.ais_unlock_achievement(98)
			# Play sound based on hit number
			match n_hit_number:
				0:
					if not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITCLARK2].is_playing() and not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITCLARK3].is_playing():
						AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITCLARK1].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
						AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
						Globals.myGameConditions.add_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
				1:
					if not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITCLARK1].is_playing():
						AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITCLARK2].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
						AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
						Globals.myGameConditions.add_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
				_:
					if not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITCLARK2].is_playing():
						AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITCLARK3].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
						AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
						Globals.myGameConditions.reset_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
			s.b_deleted = true
		Enums.ProjTypes.PROJ_PIZZA:
			AISupport.ais_unlock_achievement(98)
			AISupport.ais_forge_trick(14, 800)  # Pizza at George
			# Play sound based on hit number
			match n_hit_number:
				0:
					if not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITPIZZA2].is_playing() and not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITPIZZAR2].is_playing():
						AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITPIZZA1].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
						AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
						Globals.myGameConditions.add_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
				_:
					if not AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITPIZZA1].is_playing():
						var sound_idx: int = Enums.ASLList.LSND_PREZ_HITPIZZAR2 if Globals.myGameConditions.is_ritual() else Enums.ASLList.LSND_PREZ_HITPIZZA2
						AIMethods.l_sound[sound_idx].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
						AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
						Globals.myGameConditions.reset_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
			s.b_deleted = true
		Enums.ProjTypes.PROJ_GREASE:
			Globals.myGameConditions.add_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
			var hose_hit: int = Globals.myGameConditions.get_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
			# Play sound based on hit number
			match hose_hit:
				1:
					AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
					AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITHOSE1].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
				2:
					AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
					var sound_idx: int = Enums.ASLList.LSND_PREZ_HITHOSER2 if Globals.myGameConditions.is_ritual() else Enums.ASLList.LSND_PREZ_HITHOSE2
					AIMethods.l_sound[sound_idx].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
					AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITHOSE1].stop()
				3:
					AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
					var sound_idx: int = Enums.ASLList.LSND_PREZ_HITHOSER3 if Globals.myGameConditions.is_ritual() else Enums.ASLList.LSND_PREZ_HITHOSE2
					AIMethods.l_sound[sound_idx].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
					AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITHOSER2].stop()
					AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITHOSE2].stop()
				4:
					AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
					var sound_idx: int = Enums.ASLList.LSND_PREZ_HITHOSER4 if Globals.myGameConditions.is_ritual() else Enums.ASLList.LSND_PREZ_HITHOSE2
					AIMethods.l_sound[sound_idx].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
					AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITHOSER3].stop()
					AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITHOSE2].stop()
					Globals.myGameConditions.reset_prez_hit(s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE])
			ais_create_hose_whap(s.n_scr_x + s.frm_frame.hotspot_x, s.n_scr_y + s.frm_frame.hotspot_y, 2)
			ais_projectile_rebound(s)
			s.b_deleted = true
		Enums.ProjTypes.PROJ_EXAM:
			AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
			AIMethods.l_sound[Enums.ASLList.LSND_PREZ_HITEXAM].play(SoundbankInfo.VOL_HOLLAR, prez_pan)
			AIMethods.l_sound[Enums.ASLList.LSND_EXAM_TOSS1].stop()
			AIMethods.no_speech_for(100)
			s.b_deleted = true


static func _ai_projectile_frame_8_water(s: TSprite) -> void:
	# Frame eight - Hit water or bank
	if s.n_scr_y > Globals.myLayers.get_offset(Enums.LayerNames.LAYER_PIT):
		# THE PROJECTILE JUST HIT THE WATER
		AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_L, s.n_x, s.n_y))
		if s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE] == Enums.ProjTypes.PROJ_PIZZA:
			ai_init_floating_pizza(s)  # Create a floating pizza sprite in the pit
		elif s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE] == Enums.ProjTypes.PROJ_CLARK:
			ai_init_floating_clark(s)  # Create a floating Clark mug in the pit
		else:
			s.b_deleted = true
	elif s.n_scr_y > (Globals.myLayers.get_offset(Enums.LayerNames.LAYER_PIT) - AIDefine.D_BANK_HEIGHT):
		# THE PROJECTILE JUST HIT THE BANK OPPOSITE
		ais_projectile_rebound(s)


# === FLOATING PROJECTILE ===

static func ai_floating_projectile(s: TSprite) -> void:
	AISupport.ais_bob_up_and_down(s, AIDefine.TIME_AVERAGE_BOB_TIME / 3)
	match s.n_attrib[Enums.AttrProjectile.ATTR_PROJECTILE_TYPE]:
		Enums.ProjTypes.PROJ_PIZZA:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpPIZZA5_1 + AIMethods.R.randi() % AIDefine.NSPR_PIZZA5])
		Enums.ProjTypes.PROJ_CLARK:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLARK5A_1 + ((s.n_cc % (AIDefine.NSPR_CLARK5A * AIDefine.TIME_CLARK_MUG_FLOAT_ROTATION)) / AIDefine.TIME_CLARK_MUG_FLOAT_ROTATION)])

	if s.n_cc > 500:
		s.b_deleted = true


static func ai_init_floating_pizza(s: TSprite) -> void:
	# Create a floating pizza sprite in the pit
	s.n_cc = 0
	s.set_behavior(Callable(AIProjectile, "ai_floating_projectile"))
	ais_chase_pizza(s)


static func ai_init_floating_clark(s: TSprite) -> void:
	# Create a floating Clark mug in the pit
	s.n_cc = 0
	s.set_behavior(Callable(AIProjectile, "ai_floating_projectile"))
	ais_chase_clark(s)


static func ais_chase_pizza(spr_pizza: TSprite) -> void:
	# Send all of the frosh chasing after a pizza
	var n: int = AIMethods.ss_fr.get_number_of_sprites()

	for i in range(n):
		if 0 == AIMethods.R.randi() % ((Globals.myGameConditions.get_booster(0) / 3) + 1):
			var frosh: TSprite = AIMethods.ss_fr.get_sprite(i)
			var n_temp: int = frosh.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR]
			if n_temp == 7 or n_temp == 10 or n_temp == 11:
				frosh.n_dest_x = spr_pizza.n_x - AIDefine.D_PIZZA_EATING_OFFSET_X if spr_pizza.n_x < frosh.n_x else spr_pizza.n_x + AIDefine.D_PIZZA_EATING_OFFSET_X
				frosh.n_dest_y = spr_pizza.n_y
				AISupport.ais_send_frosh_flying(frosh)
				frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_PIZZA
			elif n_temp == 4:
				frosh.n_dest_x = spr_pizza.n_x - AIDefine.D_PIZZA_EATING_OFFSET_X if spr_pizza.n_x < frosh.n_x else spr_pizza.n_x + AIDefine.D_PIZZA_EATING_OFFSET_X
				frosh.n_dest_y = spr_pizza.n_y
				frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_PIZZA
				if frosh.n_z < 5:
					frosh.n_z = 0
					AIFrosh.ai_init_4(frosh)
				else:
					AISupport.ais_send_frosh_flying(frosh)


static func ais_chase_clark(spr_clark: TSprite) -> void:
	# Send all of the frosh chasing after a clark (beer)
	# THIS IS A BLAST OF AISCHASEPIZZA
	var n: int = AIMethods.ss_fr.get_number_of_sprites()

	for i in range(n):
		if 0 == AIMethods.R.randi() % ((Globals.myGameConditions.get_booster(0) / 3) + 1):
			var frosh: TSprite = AIMethods.ss_fr.get_sprite(i)
			var n_temp: int = frosh.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR]
			if n_temp == 7 or n_temp == 10 or n_temp == 11:
				frosh.n_dest_x = spr_clark.n_x - AIDefine.D_PIZZA_EATING_OFFSET_X if spr_clark.n_x < frosh.n_x else spr_clark.n_x + AIDefine.D_PIZZA_EATING_OFFSET_X
				frosh.n_dest_y = spr_clark.n_y
				AISupport.ais_send_frosh_flying(frosh)
				frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_CLARK
			elif n_temp == 4:
				frosh.n_dest_x = spr_clark.n_x - AIDefine.D_PIZZA_EATING_OFFSET_X if spr_clark.n_x < frosh.n_x else spr_clark.n_x + AIDefine.D_PIZZA_EATING_OFFSET_X
				frosh.n_dest_y = spr_clark.n_y
				frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_CLARK
				if frosh.n_z < 5:
					frosh.n_z = 0
					AIFrosh.ai_init_4(frosh)
				else:
					AISupport.ais_send_frosh_flying(frosh)


# === CLOSEUP BEER ===

static func ai_close_up_beer(s: TSprite) -> void:
	if AISupport.ais_forge_trick(7, 200):  # Guzzle Beer first time
		Globals.myGameConditions.add_energy(50)
	match s.n_cc:
		1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLOSEUPBEER1])
		5: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpCLOSEUPBEER2])
		9: s.b_deleted = true


# === ARM RING ANIMATIONS ===

static func ai_arm_ring_1(s: TSprite) -> void:
	if s.n_y > 327:
		s.n_y -= 10
	else:
		if s.n_x == 44 and s.n_cc == 40:
			s.b_deleted = true
		else:
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIProjectile, "ai_arm_ring_1"), s.n_x, s.n_y, 44, s.n_y, 1, 1)


static func ai_arm_ring_2(s: TSprite) -> void:
	if s.n_y > 299:
		s.n_y -= 10
	else:
		if s.n_x == 224 and s.n_cc == 40:
			s.b_deleted = true
		else:
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIProjectile, "ai_arm_ring_2"), s.n_x, s.n_y, 224, s.n_y, 1, 1)


static func ai_arm_ring_3(s: TSprite) -> void:
	if s.n_y > 227:
		s.n_y -= 10
	else:
		if s.n_x == 245 and s.n_cc == 40:
			s.b_deleted = true
			AIMethods.spr_arm.n_cc = 300
			AIMethods.spr_arm.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND7_7])
			AIMethods.spr_arm.n_x = 320
			AIMethods.spr_arm.n_y = 700
		else:
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIProjectile, "ai_arm_ring_3"), s.n_x, s.n_y, 245, s.n_y, 1, 1)


# === ARM WEAPON SWITCHING ===

static func ais_change_arm() -> int:
	var n_current: int = AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS]
	var b_ok: int = 0
	var n_apples: int = Globals.myGameConditions.get_player_apples()
	var n_pizza: int = Globals.myGameConditions.get_player_pizza()
	var n_clark: int = Globals.myGameConditions.get_player_clark()
	var n_exams: int = Globals.myGameConditions.get_player_exam()

	if n_current == Enums.ArmPositions.ARM_GREASE or n_current == Enums.ArmPositions.ARM_IRON_RING:
		n_current = Enums.ArmPositions.ARM_APPLE
	else:
		n_current += 1

	if 0 == (n_apples + n_pizza + n_clark + n_exams):
		return Enums.ArmPositions.ARM_NOTHING

	while 0 == b_ok:
		match n_current:
			Enums.ArmPositions.ARM_APPLE:
				if 0 != n_apples: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_PIZZA
			Enums.ArmPositions.ARM_PIZZA:
				if 0 != n_pizza: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_CLARK
			Enums.ArmPositions.ARM_CLARK:
				if 0 != n_clark: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_EXAM
			Enums.ArmPositions.ARM_EXAM:
				if 0 != n_exams: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_GREASE
			Enums.ArmPositions.ARM_GREASE:
				if AIMethods.spr_water_meter != null: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_IRON_RING
			Enums.ArmPositions.ARM_IRON_RING:
				if AIMethods.spr_ring_meter != null: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_APPLE
			_:
				n_current = Enums.ArmPositions.ARM_APPLE

	return n_current


static func ais_change_arm_backwards() -> int:
	var n_current: int = AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS]
	var b_ok: int = 0
	var n_apples: int = Globals.myGameConditions.get_player_apples()
	var n_pizza: int = Globals.myGameConditions.get_player_pizza()
	var n_clark: int = Globals.myGameConditions.get_player_clark()
	var n_exams: int = Globals.myGameConditions.get_player_exam()

	if n_current == Enums.ArmPositions.ARM_GREASE or n_current == Enums.ArmPositions.ARM_IRON_RING:
		n_current = Enums.ArmPositions.ARM_APPLE
	else:
		n_current -= 1

	if 0 == (n_apples + n_pizza + n_clark + n_exams):
		return Enums.ArmPositions.ARM_NOTHING

	while 0 == b_ok:
		match n_current:
			Enums.ArmPositions.ARM_APPLE:
				if 0 != n_apples: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_IRON_RING
			Enums.ArmPositions.ARM_PIZZA:
				if 0 != n_pizza: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_APPLE
			Enums.ArmPositions.ARM_CLARK:
				if 0 != n_clark: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_PIZZA
			Enums.ArmPositions.ARM_EXAM:
				if 0 != n_exams: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_CLARK
			Enums.ArmPositions.ARM_GREASE:
				if AIMethods.spr_water_meter != null: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_EXAM
			Enums.ArmPositions.ARM_IRON_RING:
				if AIMethods.spr_ring_meter != null: b_ok = 1
				else: n_current = Enums.ArmPositions.ARM_GREASE
			_:
				n_current = Enums.ArmPositions.ARM_IRON_RING

	return n_current


# === DEMO MOUSE CHECK ===

static func ais_demo_mouse_has_moved() -> bool:
	n_mouse_x = Globals.InputService.get_mouse_x()
	n_mouse_y = Globals.InputService.get_mouse_y()
	var b_answer: bool = (n_demo_old_mouse_x != n_mouse_x) or (n_demo_old_mouse_y != n_mouse_y)
	n_demo_old_mouse_x = n_mouse_x
	n_demo_old_mouse_y = n_mouse_y
	return b_answer


# === MAIN ARM BEHAVIOR ===

static func ai_arm(s: TSprite) -> void:
	s.n_x = Globals.InputService.get_mouse_x() + ((640 - Globals.InputService.get_mouse_x()) / 6)
	s.n_y = 480 + ((Globals.InputService.get_mouse_y()) / 5)

	match s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS]:
		Enums.ArmPositions.ARM_DEMO:
			_ai_arm_demo(s)
		Enums.ArmPositions.ARM_NOTHING:
			_ai_arm_nothing(s)
		Enums.ArmPositions.ARM_APPLE:
			_ai_arm_apple(s)
		Enums.ArmPositions.ARM_PIZZA:
			_ai_arm_pizza(s)
		Enums.ArmPositions.ARM_CLARK:
			_ai_arm_clark(s)
		Enums.ArmPositions.ARM_EXAM:
			_ai_arm_exam(s)
		Enums.ArmPositions.ARM_GREASE:
			_ai_arm_grease(s)
		Enums.ArmPositions.ARM_PUSH:
			pass  # Not implemented
		Enums.ArmPositions.ARM_IRON_RING:
			_ai_arm_iron_ring(s)
		Enums.ArmPositions.ARM_OTHROW:
			_ai_arm_othrow(s)
		Enums.ArmPositions.ARM_STHROW, Enums.ArmPositions.ARM_STHROW2, Enums.ArmPositions.ARM_STHROW3:
			_ai_arm_sthrow(s)
		Enums.ArmPositions.ARM_CHANGING:
			_ai_arm_changing(s)


static func _ai_arm_demo(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	if ais_demo_mouse_has_moved() and s.n_cc > 10:
		Globals.GameLoop.change_game_state(Enums.GameStates.STATETITLE)


static func _ai_arm_nothing(s: TSprite) -> void:
	match s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION]:
		0:  # HAND, NOTHING IN IT
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND0_1])
			if 0 == AIMethods.R.randi() % 10:
				# TRY TO SWITCH TO WEAPONS
				if ais_change_arm() != Enums.ArmPositions.ARM_NOTHING:
					s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm()
					s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
					s.n_cc = 0
			if 0 == AIMethods.R.randi() % 150:
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
				s.n_cc = 0
		1:  # Unknown
			if s.n_cc == 7:
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
		2:  # Pushing
			match s.n_cc:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND0_2])
				9: s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
		3:  # Snatching
			match s.n_cc:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND0_3])
				9: s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0


static func _ai_arm_apple(s: TSprite) -> void:
	match s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION]:
		0:  # HOLDING AN APPLE
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_1])
			if s.n_cc > 100 and 0 == AIMethods.R.randi() % 50:
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1 if AIMethods.R.randi() % 2 != 0 else 3
				s.n_cc = 0
			if Globals.InputService.left_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 2
				s.n_cc = 0
			if Globals.InputService.right_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1 if AIMethods.R.randi() % 2 != 0 else 3
				s.n_cc = 0
			if Globals.InputService.toggle_forward_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm()
				s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				s.n_cc = 0
			elif Globals.InputService.toggle_back_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm_backwards()
				s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				s.n_cc = 0
		1:  # BOBBING AN APPLE: SMALL
			match s.n_cc:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_2])
				5: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_3])
				7: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_2])
				9: s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
		3:  # BOBBING AN APPLE: BIG
			match s.n_cc:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_2])
				5: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_3])
				7: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_4])
				16: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_3])
				18: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_2])
				19: s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
		2:  # TOSSING AN APPLE
			match s.n_cc:
				1:  # Left arm windup
					n_toss_power = 1
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_5])
				3:
					s.n_x -= 390
					if s.n_x > -75: s.n_x = -75
				4:
					s.n_x -= 390
					AIMethods.spr_power_meter = SpriteInit.create_sprite(Enums.SpriteType.SPR_POWER_METER, 26, 95)
					AIMethods.ss_console.include(AIMethods.spr_power_meter)
					if s.n_x > -70: s.n_x = -70
				5:
					s.n_x -= 430
					if s.n_x > -75: s.n_x = -75
					if Globals.InputService.left_button_down():
						s.n_cc -= 1
						n_toss_power += 1
				6:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_6])
				7:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_7])
				8:
					# CREATE AN APPLE
					if AIMethods.spr_power_meter != null:
						AIMethods.spr_power_meter.b_attrib[1] = true
					AISupport.ais_create_projectile(s.n_x + 20, s.n_y, Enums.SpriteType.SPR_APPLE, n_toss_power)
					if n_toss_power > 23:
						AISupport.ais_unlock_achievement(543)
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_8])
					Globals.myGameConditions.lose_apple()
					s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_OTHROW
					s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_APPLE if Globals.myGameConditions.get_player_apples() > 0 else ais_change_arm()
					s.n_cc = 0


static func _ai_arm_pizza(s: TSprite) -> void:
	s.n_x = Globals.InputService.get_mouse_x() - ((Globals.InputService.get_mouse_x()) / 6)
	match s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION]:
		0:  # HOLDING A SLICE OF PIZZA
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND2_1])
			if Globals.InputService.left_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 2
				s.n_cc = 0
		1:  # WAVING A PIZZA (REMOVED)
			s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
		2:  # TOSSING A PIZZA
			match s.n_cc:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND2_2])
				4:
					# CREATE PIZZA
					AISupport.ais_create_projectile(s.n_x - 30, s.n_y, Enums.SpriteType.SPR_PIZZA)
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND9_1])
					Globals.myGameConditions.lose_pizza()
					s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_STHROW
					s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_PIZZA if Globals.myGameConditions.get_player_pizza() > 0 else ais_change_arm()
					s.n_cc = 0

	if Globals.InputService.toggle_forward_button_pressed():
		s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm()
		s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
		s.n_cc = 0
	elif Globals.InputService.toggle_back_button_pressed():
		s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm_backwards()
		s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
		s.n_cc = 0


static func _ai_arm_clark(s: TSprite) -> void:
	s.n_x = Globals.InputService.get_mouse_x() - ((Globals.InputService.get_mouse_x()) / 6)

	match s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION]:
		0:  # HOLDING A CLARK MUG
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND3_1])
			if s.n_cc > 450 and 0 == AIMethods.R.randi() % 150:
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
				s.n_cc = 0
			if Globals.InputService.left_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 2
				s.n_cc = 0
			if Globals.InputService.right_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
				s.n_cc = 0
			if Globals.InputService.toggle_forward_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm()
				s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				s.n_cc = 0
			elif Globals.InputService.toggle_back_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm_backwards()
				s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				s.n_cc = 0
		1:  # PULLIN' THE CLARK MUG BACK (drinking)
			s.n_x = 640
			s.n_y = 480
			match s.n_cc:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND3_2])
				3: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND3_3])
				5:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND3_4])
					AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_CLOSEUP_BEER, s.n_x, s.n_y))
				27:
					AISupport.ais_unlock_achievement(50)
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND3_3])
				29: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND3_2])
				31: s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
		2:  # TOSSING A CLARK MUG
			match s.n_cc:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND3_5])
				4:
					# CREATE A FLYING BEERMUG
					AISupport.ais_create_projectile(s.n_x - 30, s.n_y, Enums.SpriteType.SPR_CLARK, 3)
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND9_1])
					Globals.myGameConditions.lose_clark()
					s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_STHROW2
					s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_CLARK if Globals.myGameConditions.get_player_clark() > 0 else ais_change_arm()
					s.n_cc = 0


static func _ai_arm_exam(s: TSprite) -> void:
	s.n_x = Globals.InputService.get_mouse_x() - ((Globals.InputService.get_mouse_x()) / 6)
	match s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION]:
		0:  # HOLDING A 114 EXAM
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND4_1])
			if 0 == AIMethods.R.randi() % 250:
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
				s.n_cc = 0
			if Globals.InputService.left_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 2
				s.n_cc = 0
			if Globals.InputService.right_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
				s.n_cc = 0
			if Globals.InputService.toggle_forward_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm()
				s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				s.n_cc = 0
			elif Globals.InputService.toggle_back_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm_backwards()
				s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				s.n_cc = 0
		1:  # TAKING A LOOK AT THE EXAM
			s.n_y -= 15
			if Globals.InputService.right_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
				s.n_cc = 0
			if Globals.InputService.toggle_forward_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm()
				s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				s.n_cc = 0
			elif Globals.InputService.toggle_back_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm_backwards()
				s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				s.n_cc = 0
			match s.n_cc:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND4_2])
				2: s.n_y += 10
				3: s.n_y += 5
				78: s.n_y += 5
				79: s.n_y += 10
				80: s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
			if Globals.InputService.left_button_pressed():
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 2
				s.n_cc = 0
		2:  # TOSSING A 114 EXAM
			AISupport.ais_unlock_achievement(114)
			has_tossed_114_exam = true
			match s.n_cc:
				1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND4_3])
				4:
					# CREATE A 114 EXAM
					AISupport.ais_create_projectile(s.n_x - 30, s.n_y, Enums.SpriteType.SPR_EXAM, 150)
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND9_1])
					Globals.myGameConditions.lose_exam()
					s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_STHROW3
					s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_EXAM if Globals.myGameConditions.get_player_exam() > 0 else ais_change_arm()
					s.n_cc = 0


static func _ai_arm_grease(s: TSprite) -> void:
	s.n_x = Globals.InputService.get_mouse_x() + s.n_attrib[Enums.AttrArm.ATTR_KICKBACK]
	if AIMethods.spr_water_meter != null:
		AIMethods.spr_water_meter.b_attrib[1] = true

	if Globals.InputService.toggle_forward_button_pressed():
		s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm()
		s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
		s.n_cc = 0
	elif Globals.InputService.toggle_back_button_pressed():
		s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm_backwards()
		s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
		s.n_cc = 0

	match s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION]:
		0:  # HOLDING THE FIRE HOSE
			s.n_attrib[Enums.AttrArm.ATTR_KICKBACK] /= 2
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND5_1])
			if Globals.InputService.left_button_down():
				AIMethods.s_sound[Enums.ASSList.SSND_WATER_HOSE].loop(SoundbankInfo.VOL_NORMAL)
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
				s.n_cc = 0
		1:  # HOSING
			AISupport.ais_unlock_achievement(69)
			s.n_attrib[Enums.AttrArm.ATTR_KICKBACK] += (2 + AIMethods.R.randi() % 2) if s.n_x > 320 else -(2 + AIMethods.R.randi() % 2)
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND5_1 + ((s.n_cc / 2) % 3)])

			# Create grease projectile
			var new_projectile: TSprite
			new_projectile = SpriteInit.create_sprite(Enums.SpriteType.SPR_GREASE,
				s.n_x + AIMethods.R.randi() % 48 - 24,
				s.n_y + AIMethods.R.randi() % 48 - 24 - AIDefine.D_PALM_HEIGHT + AIMethods.d_misc_y_to_pit_y() + 125)
			new_projectile.n_dest_x = s.n_x + AIMethods.R.randi() % 12 - 6
			new_projectile.n_dest_y = Globals.InputService.get_mouse_y() + AIMethods.d_misc_y_to_pit_y() + AIMethods.R.randi() % 12 - 6
			new_projectile.n_attrib[Enums.AttrProjectile.ATTR_START_X] = s.n_x
			new_projectile.n_attrib[Enums.AttrProjectile.ATTR_START_Y] = s.n_y - AIDefine.D_PALM_HEIGHT + AIMethods.d_misc_y_to_pit_y() + 125
			new_projectile.n_attrib[Enums.AttrProjectile.ATTR_POWER_OF_THROW] = 15
			AIMethods.ss_tossed.include(new_projectile)

			if not Globals.InputService.left_button_down():
				AIMethods.s_sound[Enums.ASSList.SSND_WATER_HOSE].stop()
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
				s.n_cc = 0


static func _ai_arm_iron_ring(s: TSprite) -> void:
	if s.n_cc < 300:
		AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDMURMUR].stop()
	elif s.n_cc < 375:
		# Force the screen to scroll up
		if s.n_cc == 310:
			if AIMethods.spr_frecs_l != null:
				AIMethods.spr_frecs_l.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = 1
			if AIMethods.spr_frecs_c != null:
				AIMethods.spr_frecs_c.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = 1
			if AIMethods.spr_frecs_r != null:
				AIMethods.spr_frecs_r.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = 1
		s.n_x = 320
		s.n_y = 480
		Globals.myLayers.force_scroll((s.n_cc - 335) / 2)
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND7_7])
		if s.n_cc >= RING_UP_START_TIME - 20:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND7_6])
		s.n_y -= ((s.n_cc - 380) * 4)
		if s.n_y < 480:
			s.n_y = 480
	elif s.n_cc < RING_UP_START_TIME + 31:
		# Shouts with fist
		if s.n_cc == RING_UP_START_TIME:
			ais_shout_discipline()
			AIMethods.spr_ring_meter = SpriteInit.create_sprite(Enums.SpriteType.SPR_RING_METER, 26, 17)
			AIMethods.ss_console.include(AIMethods.spr_ring_meter)
		if s.n_cc == RING_UP_START_TIME + 30:
			if AIMethods.spr_frecs_l != null:
				AIMethods.spr_frecs_l.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = AIDefine.ENERGY_SLAM - 50
			if AIMethods.spr_frecs_c != null:
				AIMethods.spr_frecs_c.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = AIDefine.ENERGY_SLAM - 50
			if AIMethods.spr_frecs_r != null:
				AIMethods.spr_frecs_r.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = AIDefine.ENERGY_SLAM - 50
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDMURMUR].loop(SoundbankInfo.VOL_CROWD)
		s.n_x = 320
		s.n_y = 480
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND7_6])
	elif s.n_cc < RING_UP_START_TIME + 33:
		# Shouts with fist
		s.n_x = (320 + Globals.InputService.get_mouse_x() + ((640 - Globals.InputService.get_mouse_x()) / 6)) / 2
		s.n_y = (1110 + ((Globals.InputService.get_mouse_y()) / 5)) / 2
		s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
		if AIMethods.spr_ring_meter != null:
			AIMethods.spr_ring_meter.b_attrib[1] = true
	else:
		# Attack Frosh
		AISupport.ais_unlock_achievement(7)
		Globals.myGameConditions.add_energy(4)
		s.n_x = Globals.InputService.get_mouse_x() - ((Globals.InputService.get_mouse_x()) / 6)
		if 0 != s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION]:
			s.n_y += 100
			if s.n_cc == 1005:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND7_6])
				s.n_cc = 1000
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
		else:
			s.n_y += 150
			if AIMethods.spr_ring_meter == null:
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm()
				s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
			elif Globals.InputService.left_button_pressed():
				AIMethods.l_sound[Enums.ASLList.LSND_RING_ZAP1].stop()
				AIMethods.l_sound[Enums.ASLList.LSND_RING_ZAP2].stop()
				AIMethods.l_sound[Enums.ASLList.LSND_RING_ZAP3].stop()
				AIMethods.l_sound[Enums.ASLList.LSND_RING_ZAP1 + AIMethods.R.randi() % 3].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
				AISupport.ais_iron_ring_zap()
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND7_4])
				s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 1
				s.n_cc = 1000


static func _ai_arm_othrow(s: TSprite) -> void:
	match s.n_cc:
		1: s.n_x -= 20; s.n_y += 24
		2: s.n_x -= 70; s.n_y += 60
		3: s.n_x -= 140; s.n_y += 120
		4: s.n_x -= 200; s.n_y += 140
		5: s.n_x -= 300; s.n_y += 160
		6:
			s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
			s.n_cc = 4  # Skip the first "dropping down" part


static func _ai_arm_sthrow(s: TSprite) -> void:
	match s.n_cc:
		1: s.n_x += 20; s.n_y += 24
		2: s.n_x += 40; s.n_y += 48
		3: s.n_x += 60; s.n_y += 60
		4: s.n_x += 72; s.n_y += 72
		5: s.n_x += 80; s.n_y += 84
		6:
			s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
			s.n_cc = 4  # Skip the first "dropping down" part


static func _ai_arm_changing(s: TSprite) -> void:
	match s.n_cc:
		1: s.n_y += 15
		10:
			s.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION]
			s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = 0
		2, 9: s.n_y += 30
		3, 8: s.n_y += 60
		4, 7: s.n_y += 120
		5: s.n_y += 240
		6:
			s.n_y += 480
			AIMethods.s_sound[Enums.ASSList.SSND_WATER_HOSE].stop()
			match s.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION]:
				Enums.ArmPositions.ARM_NOTHING: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND0_1])
				Enums.ArmPositions.ARM_APPLE: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_1])
				Enums.ArmPositions.ARM_PIZZA: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND2_1])
				Enums.ArmPositions.ARM_CLARK: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND3_1])
				Enums.ArmPositions.ARM_EXAM: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND4_1])
				Enums.ArmPositions.ARM_GREASE: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND5_1])
				Enums.ArmPositions.ARM_PUSH: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND0_1])
				Enums.ArmPositions.ARM_IRON_RING:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
					AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_ARM_RING_2, 224 - 100, 299 + 100))
					AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_ARM_RING_3, 245 + 100, 227 + 100))
					AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_ARM_RING_1, 44 - 100, 327 + 100))
				Enums.ArmPositions.ARM_OTHROW: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND1_8])
				Enums.ArmPositions.ARM_STHROW, Enums.ArmPositions.ARM_STHROW2, Enums.ArmPositions.ARM_STHROW3:
					s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpHAND9_1])
		_:
			if s.n_cc > 40:
				s.n_cc = 0


# === WHAP AND WORD BUBBLE ===

static func ai_whap(s: TSprite) -> void:
	# Create a "WHAP!" noise.
	if s.n_cc > AIDefine.TIME_WHAP:
		s.b_deleted = true


static func ai_word_bubble(s: TSprite) -> void:
	# Create a word bubble
	if s.n_cc > AIDefine.TIME_BUBBLE:
		s.b_deleted = true


# === SHOUT DISCIPLINE ===

static func ais_shout_discipline() -> void:
	# HOLLAR - Shout the discipline name during iron ring ceremony
	var n_thing_to_shout: int = Enums.ASLList.LSND_DISCIPLINES_DEFAULT

	for i in range(AIDefine.NUM_JBAR_SPOTS - 1, -1, -1):
		var n_next_bar: int = Globals.myGameConditions.get_j_bar(i)
		match n_next_bar:
			0: n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_APPLE
			1:
				n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_CHEM
				if i > 0 and Globals.myGameConditions.get_j_bar(i - 1) == 10:
					n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_ENGCHEM
			2: n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_CIVIL
			3: n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_ELEC
			4: n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_ENGPHYS
			5: n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_ENGCHEM
			6: n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_GEO
			7: n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_METALS
			8, 15: n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_MECH
			9: n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_MINING
			17: n_thing_to_shout = Enums.ASLList.LSND_DISCIPLINES_METALS

	AIMethods.l_sound[n_thing_to_shout].play(SoundbankInfo.VOL_HOLLAR, 0)


# === COLLISION RESPONSE HELPERS ===

static func ais_collision_to_response(s: TSprite, ss_targets: SpriteSet, response_func: Callable,
		b_include_whap: bool, b_pole_shield: bool, b_hit_only_one: bool = true) -> bool:
	# Check for collision with frosh in target spriteset
	var ss_hit: SpriteSet = AISupport.ais_get_targets_in_scr_range(
		ss_targets,
		s.n_scr_x - 10, s.n_scr_y - 10,
		s.n_scr_x + 10, s.n_scr_y + 10,
		false  # INCLUDEALLFROSH
	)

	var n: int = ss_hit.get_number_of_sprites()
	if n > 0:
		var b_did_hit: bool = false

		if b_hit_only_one:
			# Only hit the frontmost frosh (highest Y value)
			ss_hit.order_by_y()
			var him: TSprite = ss_hit.get_sprite(n - 1)

			# Check pole shield - don't hit if frosh is behind the pole
			if not (b_pole_shield and him.n_y < AIDefine.D_POLE_Y \
					and s.n_scr_x > (AIDefine.D_POLE_X - AIDefine.D_POLE_WIDTH) \
					and s.n_scr_x < (AIDefine.D_POLE_X + AIDefine.D_POLE_WIDTH)):
				b_did_hit = true
				if b_include_whap:
					AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_WHAP,
						s.n_scr_x + s.frm_frame.hotspot_x,
						s.n_scr_y + s.frm_frame.hotspot_y))
				if response_func.is_valid():
					response_func.call(him)
				# Add splash if frosh is in the mud
				if him.n_z <= AIDefine.D_BELLY_BUTTON_Z:
					AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SPLASH_L, him.n_x, him.n_y))
		else:
			# Hit all frosh in range
			for i in range(n):
				var him: TSprite = ss_hit.get_sprite(i)
				# Check pole shield
				if not (b_pole_shield and him.n_y < AIDefine.D_POLE_Y \
						and s.n_scr_x > (AIDefine.D_POLE_X - AIDefine.D_POLE_WIDTH) \
						and s.n_scr_x < (AIDefine.D_POLE_X + AIDefine.D_POLE_WIDTH)):
					b_did_hit = true
					if b_include_whap:
						AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_WHAP,
							s.n_scr_x + s.frm_frame.hotspot_x,
							s.n_scr_y + s.frm_frame.hotspot_y))
					if response_func.is_valid():
						response_func.call(him)

		return b_did_hit
	return false


static func ais_special_collision_to_response(s: TSprite, ss_targets: SpriteSet, response_func: Callable,
		b_include_whap: bool, b_pole_shield: bool, b_delete_projectile: bool,
		special_func_1: Callable, special_func_2: Callable) -> bool:
	# Special collision that can trigger different behaviors (beer, pizza)
	# Always hits only one frosh (the frontmost one)
	var ss_hit: SpriteSet = AISupport.ais_get_targets_in_scr_range(
		ss_targets,
		s.n_scr_x - 10, s.n_scr_y - 10,
		s.n_scr_x + 10, s.n_scr_y + 10,
		false  # INCLUDEALLFROSH
	)

	var n: int = ss_hit.get_number_of_sprites()
	if n > 0:
		# Only hit the frontmost frosh (highest Y value)
		ss_hit.order_by_y()
		var him: TSprite = ss_hit.get_sprite(n - 1)

		# Check pole shield
		if not (b_pole_shield and him.n_y < AIDefine.D_POLE_Y \
				and s.n_scr_x > (AIDefine.D_POLE_X - AIDefine.D_POLE_WIDTH) \
				and s.n_scr_x < (AIDefine.D_POLE_X + AIDefine.D_POLE_WIDTH)):

			# Check if frosh is already eating/drinking (behavior 11 or 10)
			if him.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == 11 or him.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == 10:
				s.b_deleted = true
				s.n_x += 640  # Move off screen
				if special_func_1.is_valid():
					special_func_1.call(him)
			else:
				# Decide which behavior to apply (50/50 chance)
				if AIMethods.R.randi() % 2 == 0 and special_func_1.is_valid():
					special_func_1.call(him)
				elif special_func_2.is_valid():
					special_func_2.call(him)
				elif response_func.is_valid():
					response_func.call(him)

			if b_delete_projectile:
				s.b_deleted = true
			return true
	return false
