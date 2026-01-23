class_name AIMisc

# aiMisc.gd - Static class with miscellaneous AI behaviors
# Ported from aiMisc.cs
#
# Per porting_rules.md:
# - This is a static class with logic functions only
# - All shared state accessed via AIMethods autoload
# - Cross-file calls use class qualification

# === STATIC STATE ===
static var n_next_cheer: int = 3


# === INANIMATE (does nothing) ===

static func ai_inanimate(s: TSprite) -> void:
	# Earth-shattering, no?
	pass


# === DELETE ME (marks sprite for deletion) ===

static func ai_delete_me(s: TSprite) -> void:
	s.b_deleted = true


# === ACHIEVEMENT SCREEN ===

static func ai_show_current_achievement_screen(s: TSprite) -> void:
	var achievement_group: int = Globals.myGameConditions.get_achievement_group()
	s.text = str(achievement_group + 1) + " of 3"


# === UNLOCK ACHIEVEMENT ===

static func ais_unlock_achievement(achievement_number: int) -> void:
	for a in PoleGameAchievement.list:
		if a.achievement_guid == achievement_number:
			if a.achieved:
				return
			a.achieved = true
			Globals.Analytic("AchUnl;" + str(achievement_number))
			a.when_achieved = Time.get_unix_time_from_system()

			# Determine which sprite set to use based on current game state
			var target_ss: SpriteSet
			var current_state: int = Globals.GameLoop.current_game_state
			if current_state == Enums.GameStates.STATEGAME:
				target_ss = AIMethods.ss_icons
			else:
				# For menu states (Title, Options, Decorate, Demo), use ss_menu
				target_ss = Globals.GameLoop.ss_menu

			target_ss.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_ACHIEVEMENT_UNLOCKED))
			var text1: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_ACHIEVEMENT_UNLOCKED_TEXT1)
			text1.sprite_text = TSprite.SpriteTextType.Small
			text1.text = "Achievement Unlocked"
			target_ss.include(text1)
			var text2: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_ACHIEVEMENT_UNLOCKED_TEXT2)
			text2.sprite_text = TSprite.SpriteTextType.Small
			text2.text = a.achievement_name
			text2.n_a = 0
			target_ss.include(text2)
			Globals.myGameConditions.save_settings_to_storage()


# === TAM BEHAVIOR ===

static func ai_tam(s: TSprite) -> void:
	if Globals.InputService.back_button_pressed():
		Globals.GameLoop.change_game_state(Enums.GameStates.STATETITLE)

	if 0 == s.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS]:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTAM0_1])
	else:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
		if s.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] == 1:
			s.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] = 0


# === RANDOM CROWD NOISE ===
# Added to J Section Release 3 to get a bit more fun crowd noise.
# Modified in J Section Release 4 to keep things under control

static func ais_random_crowd_noise() -> void:
	n_next_cheer += 1
	if Globals.myGameConditions.is_ritual():
		n_next_cheer = n_next_cheer % 3
	else:
		n_next_cheer = n_next_cheer % 2

	var n_noise: int = Globals.myGameConditions.get_noise_count()
	Globals.myGameConditions.set_noise_count(n_noise + 1)

	if n_noise > 1280 and AIMethods.SPEECHOK():
		AIMethods.NOSPEECHFOR(48)
		Globals.myGameConditions.set_noise_count(0)

		if AIMethods.n_frosh_above_1 < 6 and 0 == AIMethods.R.randi() % 3:
			if Globals.myGameConditions.get_noise_count() > 1000 and not (
				AIMethods.l_sound[Enums.ASLList.LSND_FRECS_REWARD5].is_playing() or
				AIMethods.l_sound[Enums.ASLList.LSND_FRECS_REWARD6].is_playing() or
				AIMethods.l_sound[Enums.ASLList.LSND_FRECS_REWARDR1].is_playing()
			):
				Globals.myGameConditions.set_noise_count(0)
				AIMethods.l_sound[Enums.ASLList.LSND_FRECS_REWARD5 + n_next_cheer].play(
					SoundbankInfo.VOL_CROWD,
					AIMethods.randintin(SoundbankInfo.PAN_LEFT / 2, SoundbankInfo.PAN_RIGHT / 2)
				)
		elif AIMethods.n_frosh_above_1 > 20 and 0 != AIMethods.R.randi() % 2:
			var progress_range: int = SoundbankInfo.NSND_FRECS_PROGRESS - 1
			if Globals.myGameConditions.is_ritual():
				progress_range = SoundbankInfo.NSND_FRECS_PROGRESS + 1 - 1
			AIMethods.l_sound[Enums.ASLList.LSND_FRECS_PROGRESS2 + AIMethods.R.randi() % progress_range].play(
				SoundbankInfo.VOL_CROWD_SHOUT,
				AIMethods.randintin(SoundbankInfo.PAN_LEFT / 2, SoundbankInfo.PAN_RIGHT / 2)
			)
		elif 0 != AIMethods.R.randi() % 3:
			if 0 != AIMethods.R.randi() % 2:
				if Globals.myGameConditions.is_ritual() and 0 != AIMethods.R.randi() % 2:
					AIMethods.l_sound[Enums.ASLList.LSND_FRECS_CHANT1 + AIMethods.R.randi() % 3].play(
						SoundbankInfo.VOL_CROWD,
						AIMethods.randintin(SoundbankInfo.PAN_LEFT / 2, SoundbankInfo.PAN_RIGHT / 2)
					)
				else:
					AIMethods.l_sound[Enums.ASLList.LSND_FRECS_CHANT1].play(
						SoundbankInfo.VOL_CROWD,
						AIMethods.randintin(SoundbankInfo.PAN_LEFT / 2, SoundbankInfo.PAN_RIGHT / 2)
					)
			else:
				AIMethods.l_sound[Enums.ASLList.LSND_FRECS_HOWHIGHTHEPOLE].play(
					SoundbankInfo.VOL_CROWD_SHOUT,
					AIMethods.randintin(SoundbankInfo.PAN_LEFT / 2, SoundbankInfo.PAN_RIGHT / 2)
				)


# === RANDOM EVENT GENERATOR ===

static func ai_random_event_generator(s: TSprite) -> void:
	# For now, the Podium is a "source" of random events.
	ais_random_events(s)
	ais_random_crowd_noise()

	# This code is responsible for preventing annoying overlap of voices.
	# If a certain time has elapsed, something happens.
	if not Globals.myGameConditions.is_demo():
		if s.n_cc > AIDefine.TIME_BETWEEN_EVENTS and (0 == AIMethods.R.randi() % 100) \
			and AIMethods.spr_tam.n_attrib[Enums.AttrTam.ATTR_TAM_STATUS] != 2:
			s.n_cc = 0

			var n_total_weapons: int = Globals.myGameConditions.get_player_apples() \
				+ Globals.myGameConditions.get_player_clark() \
				+ Globals.myGameConditions.get_player_pizza() \
				+ Globals.myGameConditions.get_player_exam()

			if (AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_GREASE \
				and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING) \
				and ((AIMethods.spr_alien == null and 0 != AIMethods.R.randi() % 3) or (n_total_weapons < 6)):
				match AIMethods.R.randi() % 13:
					0:
						if n_total_weapons > 10:
							AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_HOSE))
						else:
							AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_BEER))
					1, 2:
						if n_total_weapons > 10:
							AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_EXAM))
						else:
							if 0 != AIMethods.R.randi() % 2:
								AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_PIZZA))
							else:
								AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_BEER))
					3, 4, 5:
						AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_BEER))
					6, 7, 8:
						AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_PIZZA))
					_:
						AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_APPLES))

			elif 0 != AIMethods.R.randi() % 5:
				if AIMethods.spr_alien == null and (0 != AIMethods.R.randi() % 5) and (0 == (s.n_cc % (24 * 5))) \
					and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_GREASE \
					and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING:
					if (0 != AIMethods.R.randi() % 4) or Globals.TimerService.get_current_game_time_score_milliseconds() < 180:
						match AIMethods.R.randi() % 4:
							0: AIMethods.spr_alien = SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_ARTSCIF)
							1: AIMethods.spr_alien = SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_ARTSCIM)
							2: AIMethods.spr_alien = SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_COMMIEF)
							3: AIMethods.spr_alien = SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_COMMIEM)
						AIMethods.ss_pit.include(AIMethods.spr_alien)
					else:
						# J4 tweak: SciCons don't come in the first minute of play
						# UNTWEAKED in J5
						if 0 != AIMethods.R.randi() % 2:
							AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SCICON_F))
						else:
							AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_SCICON_M))
			else:
				if AIMethods.spr_gw_balloon == null and 0 == AIMethods.R.randi() % 3:
					AIMethods.spr_gw_balloon = SpriteInit.create_sprite(Enums.SpriteType.SPR_GW_BALLOON)
					AIMethods.ss_balloon.include(AIMethods.spr_gw_balloon)
		else:
			# If it IS the demo, create some GW balloons for effect.
			if AIMethods.spr_gw_balloon == null and s.n_cc > AIDefine.TIME_BETWEEN_EVENTS * 10 \
				and (0 == AIMethods.R.randi() % 1000):
				AIMethods.spr_gw_balloon = SpriteInit.create_sprite(Enums.SpriteType.SPR_GW_BALLOON)
				AIMethods.ss_balloon.include(AIMethods.spr_gw_balloon)


# === RANDOM EVENTS ===

static func ais_random_events(s: TSprite) -> void:
	# Obtain stats on what's going on in the game
	if 0 == (AIMethods.spr_pole.n_cc % AIDefine.TIME_RANDOM_EVENT_INTERVAL):
		# Reset level counts
		for i in range(1, 7):
			AIMethods.n_frosh_level_l[i] = 0
			AIMethods.n_frosh_level_r[i] = 0
		AIMethods.n_frosh_tam = 0
		AIMethods.n_frosh_thinking = 0
		AIMethods.n_frosh_bitter = 0

		var n: int = AIMethods.ss_fr.get_number_of_sprites()

		for i in range(n):
			var spr_tmp: TSprite = AIMethods.ss_fr.get_sprite(i)
			var pyramid_level: int = spr_tmp.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL]
			if spr_tmp.n_x < AIDefine.D_POLE_X:
				AIMethods.n_frosh_level_l[pyramid_level] += 1
			else:
				AIMethods.n_frosh_level_r[pyramid_level] += 1

			if 0 == spr_tmp.n_attrib[Enums.NAttrFrosh.ATTR_MOTIVATION]:
				AIMethods.n_frosh_bitter += 1
			if spr_tmp.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == 6 \
				and spr_tmp.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] == Enums.Goals.GOAL_THINK:
				AIMethods.n_frosh_thinking += 1

		AIMethods.n_frosh_tam = AIMethods.n_frosh_level_l[6] + AIMethods.n_frosh_level_r[6]
		AIMethods.n_frosh_above_3 = AIMethods.n_frosh_tam \
			+ AIMethods.n_frosh_level_l[5] + AIMethods.n_frosh_level_r[5] \
			+ AIMethods.n_frosh_level_l[4] + AIMethods.n_frosh_level_r[4]
		AIMethods.n_frosh_above_1 = AIMethods.n_frosh_above_3 \
			+ AIMethods.n_frosh_level_l[3] + AIMethods.n_frosh_level_r[3] \
			+ AIMethods.n_frosh_level_l[2] + AIMethods.n_frosh_level_r[2]
		AIMethods.n_frosh_total_l = AIMethods.n_frosh_level_l[1] + AIMethods.n_frosh_level_l[2] + AIMethods.n_frosh_level_l[3] \
			+ AIMethods.n_frosh_level_l[4] + AIMethods.n_frosh_level_l[5] + AIMethods.n_frosh_level_l[6]
		AIMethods.n_frosh_total_r = AIMethods.n_frosh_level_r[1] + AIMethods.n_frosh_level_r[2] + AIMethods.n_frosh_level_r[3] \
			+ AIMethods.n_frosh_level_r[4] + AIMethods.n_frosh_level_r[5] + AIMethods.n_frosh_level_r[6]

		if Globals.myGameConditions.is_pop_boy_in_pit():
			AIMethods.n_frosh_level_l[1] += 2
			AIMethods.n_frosh_level_r[1] += 2

		for i in range(7):
			AIMethods.n_frosh_level[i] = AIMethods.n_frosh_level_l[i] + AIMethods.n_frosh_level_r[i]

		if AIMethods.n_frosh_thinking > 20:
			AISupport.ais_regroup()

		# If the Pyramid is topheavy, it gets toppled
		if AIMethods.n_frosh_above_1 > AIMethods.n_frosh_level[1] and AIMethods.n_frosh_above_1 > 10:
			AISupport.ais_topple_pyramid()
			Globals.myGameConditions.add_energy(170)
		if AIMethods.n_frosh_level[3] > AIMethods.n_frosh_level[2] + 5:
			AISupport.ais_topple_pyramid()
			Globals.myGameConditions.add_energy(170)
		if AIMethods.n_frosh_level[4] > AIMethods.n_frosh_level[3] + 4:
			AISupport.ais_topple_pyramid()
			Globals.myGameConditions.add_energy(170)
		if AIMethods.n_frosh_level[5] > AIMethods.n_frosh_level[4] + 3:
			AISupport.ais_topple_pyramid()
			Globals.myGameConditions.add_energy(150)
