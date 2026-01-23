class_name GameLoop

var b_game_terminate: bool = false
var b_exit_on_escape: bool = true
var current_game_state: int = Enums.GameStates.STATENOCHANGE

var b_processing_ai: bool = false
var b_updating_frame: bool = false

# Cheat code state machine: 0=inactive, 1=C pressed, 2=C-H pressed (cheats active)
var n_ritual_state_machine: int = 0

# Title music intro plays only once per session
var b_play_riff_only_once: bool = false

var b_frame_set_loaded = [] # bool array

# Sprite Sets
var ss_menu: SpriteSet
var ss_game: SpriteSet # Placeholder

func _init():
	b_game_terminate = false
	b_frame_set_loaded.resize(7) # MAXFRAMESETS
	for i in range(7):
		b_frame_set_loaded[i] = false

	ss_menu = SpriteSet.new(0)
	ss_game = SpriteSet.new(0)

	Globals.GameLoop = self

	# Load saved settings (jacket bars, options, high scores, achievements)
	Globals.myGameConditions.load_settings_from_storage()

	print("[GameLoop] Initialized")

func change_game_state(new_state: int):
	# This initiates a state transition via the jacket slam animation
	# The actual state change happens in ai_slam_jacket at frame 16
	match new_state:
		Enums.GameStates.STATETITLE, Enums.GameStates.STATEEXIT:
			b_exit_on_escape = true
		Enums.GameStates.STATELOADING, Enums.GameStates.STATEDECORATE, Enums.GameStates.STATEDEMO, Enums.GameStates.STATEOPTIONS, Enums.GameStates.STATEGAME:
			b_exit_on_escape = false

	# Create the jacket slam sprite to handle the transition
	_init_transition_sprites(new_state)

	# Set state to STATENOCHANGE while transition plays
	# The actual state will be set in ai_slam_jacket at frame 16
	current_game_state = Enums.GameStates.STATENOCHANGE


func set_game_state(new_state: int):
	# Called by ai_slam_jacket at frame 16 to actually initialize the new state
	_init_state_sprites(new_state)
	current_game_state = new_state


func _init_transition_sprites(next_state: int):
	# Create the jacket slam sprite if not already transitioning
	if AIMethods.ss_jacket_slam.get_number_of_sprites() == 0:
		# Load trans bitmaps if not already loaded
		Globals.RenderingService.load_bitmap_set(Enums.BitmapSets.bmpTransBitmaps)
		var tmp_sprite: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_SLAM_JACKET, 0, 0)
		tmp_sprite.n_attrib[Enums.AttrJacketSlam.ATTR_NEXT_STATE] = next_state
		AIMethods.ss_jacket_slam.include(tmp_sprite)


func _init_state_sprites(state: int):
	# Initialize sprites for the given state (called after jacket slam at frame 16)
	match state:
		Enums.GameStates.STATETITLE:
			print("[GameLoop] Init Title Screen")

			# Save settings when returning to title (preserves jacket bars, options, achievements)
			Globals.myGameConditions.save_settings_to_storage()

			Globals.RenderingService.load_bitmap_set(Enums.BitmapSets.bmpMenuBitmaps)

			# Flush all game sprite sets (preserving jacket slam)
			print("[GameLoop] Flushing Spritesets")
			_flush_sprite_sets(true)

			ss_menu.flush()

			# Reset scroll position to bottom (title screen is at scroll 0)
			Globals.myLayers.reset_for_menu()

			# Stop other menu music first
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_DECORATEREPEAT].stop()
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_LOADREPEAT].stop()
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDMURMUR].stop()
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_TITLEREPEAT].stop()

			# Title music intro plays only once per session
			# After playing, aiMenuAndDisplay will start the loop
			if not b_play_riff_only_once:
				AIMethods.l_sound[Enums.ASLList.LSND_MUSIC_TITLEINIT].play(SoundbankInfo.VOL_MUSIC)
				b_play_riff_only_once = true

			# Create Title Sprites
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_TITLEBACK))
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_TITLESTART))
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_TITLEEXIT))
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_TITLEOPTIONS))

			# Mouse cursor (added last to render on top)
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_MOUSE_CURSOR))

		Enums.GameStates.STATEOPTIONS:
			print("[GameLoop] Init Options/Achievements Screen")
			Globals.RenderingService.load_bitmap_set(Enums.BitmapSets.bmpMenuBitmaps)

			# Mark title buttons as deleted (don't flush - keep TITLEBACK for background)
			var j: int = 0
			while j < ss_menu.get_number_of_sprites():
				var spr: TSprite = ss_menu.get_sprite(j)
				if spr != null:
					if spr.sprite_type == Enums.SpriteType.SPRMNU_TITLESTART \
						or spr.sprite_type == Enums.SpriteType.SPRMNU_TITLEOPTIONS \
						or spr.sprite_type == Enums.SpriteType.SPRMNU_TITLEEXIT:
						spr.b_deleted = true
				j += 1

			# Background elements (grayed out title buttons)
			var spr_tmp: TSprite
			spr_tmp = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, 0, 266)
			spr_tmp.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLESTART])
			ss_menu.include(spr_tmp)
			spr_tmp = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, 0, 346)
			spr_tmp.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEOPTIONSGLOW3])
			ss_menu.include(spr_tmp)
			spr_tmp = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, 45, 27)
			spr_tmp.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEEXIT])
			ss_menu.include(spr_tmp)

			# Options background
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_OPTIONS_BACK))

			# Return button (added before achievement text per C# order)
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_OPTIONS_RETURN))

			Globals.myGameConditions.set_achievement_group(0)
			var achievements_per_screen: int = 6
			var current_achievement_group: int = 0
			var achievements_this_screen: int = 0

			for i in range(PoleGameAchievement.list.size()):
				var achievement = PoleGameAchievement.list[i]

				var text_height: int = 24
				var text_x: int = 125
				var points_x: int = 80
				var frosh_head_x: int = 46
				var text_y: int = 60
				var target_y: int = text_y + int(float(text_height) * 2.4) * achievements_this_screen

				# Achievement Name
				var text_sprite: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_ACHIEVEMENT_TEXT)
				text_sprite.n_x = 650
				text_sprite.n_attrib[Enums.AttrBar.ATTR_BAR_GROUP] = current_achievement_group
				text_sprite.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X] = text_x
				text_sprite.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y] = target_y
				text_sprite.text = achievement.achievement_name if not (achievement.secret and not achievement.achieved) else "Secret Achievement"
				text_sprite.b_attrib[5] = achievement.achieved
				AIFlyInAndOut.ai_init_fly_in_and_out(text_sprite, Callable(AIMenuAndDisplay, "ai_achievement_text"), 640, 490, 700, target_y, 1, 1)
				ss_menu.include(text_sprite)

				# Achievement Description
				var text_sprite2: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_ACHIEVEMENT_TEXT)
				text_sprite2.n_x = 650
				text_sprite2.n_attrib[Enums.AttrBar.ATTR_BAR_GROUP] = current_achievement_group
				text_sprite2.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X] = text_x
				text_sprite2.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y] = target_y + text_height
				text_sprite2.text = achievement.description if not (achievement.secret and not achievement.achieved) else "Keep stalling the frosh to discover this achievement!"
				text_sprite2.b_attrib[5] = achievement.achieved
				AIFlyInAndOut.ai_init_fly_in_and_out(text_sprite2, Callable(AIMenuAndDisplay, "ai_achievement_text"), 640, 490, 700, target_y + text_height, 1, 1)
				ss_menu.include(text_sprite2)

				# Achievement Points
				var text_sprite3: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_ACHIEVEMENT_TEXT)
				text_sprite3.n_x = 650
				text_sprite3.n_attrib[Enums.AttrBar.ATTR_BAR_GROUP] = current_achievement_group
				text_sprite3.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X] = points_x
				text_sprite3.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y] = target_y + (text_height / 2)
				text_sprite3.text = str(achievement.value) if not (achievement.secret and not achievement.achieved) else ""
				text_sprite3.b_attrib[5] = achievement.achieved
				AIFlyInAndOut.ai_init_fly_in_and_out(text_sprite3, Callable(AIMenuAndDisplay, "ai_achievement_text"), 640, 490, 700, target_y + (text_height / 2), 1, 1)
				ss_menu.include(text_sprite3)

				# Achievement unlocked indicator (frosh head)
				if achievement.achieved:
					var achievement_unlocked: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_BTN_TOGGLE0)
					achievement_unlocked.n_x = 650
					achievement_unlocked.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X] = frosh_head_x
					achievement_unlocked.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y] = target_y + (text_height / 2) + 2
					achievement_unlocked.n_attrib[Enums.AttrBar.ATTR_BAR_GROUP] = current_achievement_group
					AIFlyInAndOut.ai_init_fly_in_and_out(achievement_unlocked, Callable(AIMenuAndDisplay, "ai_achievement_text"), 640, 490, 700, target_y + (text_height / 2) + 2, 1, 1)
					ss_menu.include(achievement_unlocked)

				achievements_this_screen += 1
				if (i + 1) % achievements_per_screen == 0:
					achievements_this_screen = 0
					current_achievement_group += 1

			# Achievement screen header text
			var header_sprite: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_ACHIEVEMENT_ADDITIONAL_TEXT, 565, 414)
			header_sprite.text = "More"
			ss_menu.include(header_sprite)

			# Achievement stats text - calculate totals
			var n_achieved: int = 0
			var n_total: int = 0
			var pts_achieved: int = 0
			var pts_total: int = 0
			for a in PoleGameAchievement.list:
				n_total += 1
				pts_total += a.value
				if a.achieved:
					n_achieved += 1
					pts_achieved += a.value
			var stats_sprite: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_ACHIEVEMENT_ADDITIONAL_TEXT, 125, 414)
			stats_sprite.set_behavior(Callable(AIMenuAndDisplay, "ai_inanimate"))
			stats_sprite.text = "Total: " + str(n_achieved) + "/" + str(n_total) + " achieved (" + str(pts_achieved) + "/" + str(pts_total) + " points)"
			ss_menu.include(stats_sprite)

			# Next achievement screen button
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_AI_NEXT_ACHIEVEMENT_SCREEN, 480, 423))

			# Mouse cursor (added last per C# order)
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_MOUSE_CURSOR))

		Enums.GameStates.STATEDECORATE:
			print("[GameLoop] Init Decorate Screen")
			Globals.RenderingService.load_bitmap_set(Enums.BitmapSets.bmpMenuBitmaps)

			# Reset bar group to 0 (first page of discipline crests)
			Globals.myGameConditions.set_bar_group(0)

			# Flush all game sprite sets (preserving jacket slam)
			print("[GameLoop] Flushing Spritesets")
			_flush_sprite_sets(true)

			ss_menu.flush()

			# Start decorate music (stop other menu music first)
			# Only start if TITLEINIT not playing - aiMenuAndDisplay will start loop when intro finishes
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_TITLEREPEAT].stop()
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_LOADREPEAT].stop()
			if not AIMethods.l_sound[Enums.ASLList.LSND_MUSIC_TITLEINIT].is_playing():
				AIMethods.s_sound[Enums.ASSList.SSND_MENU_DECORATEREPEAT].loop(SoundbankInfo.VOL_MUSIC)

			# Background with title screen
			var tmp_sprite: TSprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_CLOUDS, 0, 0)
			tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEBACK])
			ss_menu.include(tmp_sprite)

			# Grayed out title buttons
			var spr_tmp: TSprite
			spr_tmp = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, -85, 266)
			spr_tmp.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLESTARTGLOW3])
			ss_menu.include(spr_tmp)
			spr_tmp = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, 109, 31)
			spr_tmp.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEEXIT])
			ss_menu.include(spr_tmp)
			spr_tmp = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, -75, 346)
			spr_tmp.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEOPTIONS])
			ss_menu.include(spr_tmp)

			# Main decorate screen sprites
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_JACKET_BACK))
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_MENU_JACKET))
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_MENU_PASS_CREST))
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_DECORATE_RETURN))
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_TXT_SELECT))
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_AI_PREV_BAR_SCREEN, 480, 423))

			# 20 decoration bars (discipline crests)
			for i in range(20):
				ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_BAR1 + i))

			# Mouse cursor (added last to render on top)
			ss_menu.include(SpriteInit.create_sprite(Enums.SpriteType.SPRMNU_MOUSE_CURSOR))

		Enums.GameStates.STATEGAME:
			_init_game(false)

		Enums.GameStates.STATEDEMO:
			_init_game(true)


func _flush_sprite_sets(b_save_jacket_slam: bool) -> bool:
	# Flush all game sprite sets
	AIMethods.ss_clouds.flush()
	AIMethods.ss_balloon.flush()
	AIMethods.ss_skyline.flush()
	AIMethods.ss_trees.flush()
	AIMethods.ss_frecs.flush()
	AIMethods.ss_water.flush()
	AIMethods.ss_fr.flush(SpriteSet.SS_DO_NOT_DELETE)
	AIMethods.ss_pit.flush()
	AIMethods.ss_tossed.flush()
	AIMethods.ss_console.flush()
	AIMethods.ss_icons.flush()
	AIMethods.ss_mouse.flush()
	if not b_save_jacket_slam:
		AIMethods.ss_jacket_slam.flush()
	AIMethods.ss_fr.flush()
	return true


func _init_game(b_demo: bool) -> void:
	print("[GameLoop] Init Game (demo=%s)" % b_demo)

	Globals.TimerService.pause_update_count_timer()
	Globals.myGameConditions.reset(b_demo)

	# Stop menu music
	AIMethods.s_sound[Enums.ASSList.SSND_MENU_DECORATEREPEAT].stop()
	AIMethods.s_sound[Enums.ASSList.SSND_MENU_TITLEREPEAT].stop()
	AIMethods.s_sound[Enums.ASSList.SSND_MENU_LOADREPEAT].stop()
	AIMethods.s_sound[Enums.ASSList.SSND_MENU_GAMEINIT].play(SoundbankInfo.VOL_HOLLAR)

	# Load game bitmaps
	Globals.RenderingService.load_bitmap_set(Enums.BitmapSets.bmpGameBitmaps)

	# Initialize pole position system
	PolePosition.initialize_pole_positions()

	# Start crowd murmur
	AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDMURMUR].loop(SoundbankInfo.VOL_CROWD)

	# Flush all sprite sets
	print("[GameLoop] Flushing Spritesets")
	_flush_sprite_sets(true)

	# Initialize game sprites
	print("[GameLoop] Initializing Game Sprites")
	if not _init_game_sprites(b_demo):
		print("[GameLoop] Couldn't init Game sprites")

	# Crowd roar
	AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDROAR1 + AIMethods.R.randi() % 2].play(SoundbankInfo.VOL_CROWD)

	# Resume timers and reset game time
	Globals.TimerService.resume_update_count_timer()
	Globals.TimerService.reset_game_time_score()
	if not b_demo:
		Globals.myLayers.reset_for_game()


func _init_game_sprites(b_demo: bool) -> bool:
	# Initialize the sprites required in various SpriteSets in game
	# AIMethods.has_tossed_114_exam = b_demo

	# Clear special sprite references
	AIMethods.spr_alien = null
	AIMethods.spr_gw_balloon = null
	AIMethods.spr_gw_hippo = null
	AIMethods.spr_pop_boy = null
	AIMethods.spr_power_meter = null
	AIMethods.spr_water_meter = null
	AIMethods.spr_ring_meter = null

	var tmp_sprite: TSprite
	var SCRW: int = 640

	# === BACKGROUND ===
	# Clouds
	AIMethods.ss_clouds.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_CLOUDS, 0, 285))
	# Trees
	AIMethods.ss_trees.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_TREES, 0, 0))

	# === GEORGE (Prez) ===
	AIMethods.spr_prez = SpriteInit.create_sprite(Enums.SpriteType.SPR_PREZ, 200, -180)
	AIMethods.ss_skyline.include(AIMethods.spr_prez)

	# === PODIUM, BACKDROP ===
	AIMethods.ss_skyline.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_BACKDROP, 0, 0))

	AIMethods.spr_random_event_generator = SpriteInit.create_sprite(Enums.SpriteType.SPR_PODIUM, 180, -23)
	AIMethods.ss_skyline.include(AIMethods.spr_random_event_generator)

	# === FRECS (Engineering students) ===
	AIMethods.spr_frecs_l = SpriteInit.create_sprite(Enums.SpriteType.SPR_FREC_GROUP, 55, -14)
	AIMethods.spr_frecs_c = SpriteInit.create_sprite(Enums.SpriteType.SPR_FREC_ACTION, 299, -8)
	AIMethods.spr_frecs_r = SpriteInit.create_sprite(Enums.SpriteType.SPR_FREC_GROUP, 547, -14)
	AIMethods.ss_frecs.include(AIMethods.spr_frecs_c)  # Center first
	AIMethods.ss_frecs.include(AIMethods.spr_frecs_l)
	AIMethods.ss_frecs.include(AIMethods.spr_frecs_r)

	# === WATER ===
	AIMethods.ss_water.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_WATER, 0, 0))

	# === POLE ===
	AIMethods.spr_pole = SpriteInit.create_sprite(Enums.SpriteType.SPR_POLE, SCRW / 2, 80)
	AIMethods.ss_pit.include(AIMethods.spr_pole)

	# === TAM ===
	AIMethods.spr_tam = SpriteInit.create_sprite(Enums.SpriteType.SPR_TAM, SCRW / 2, 81)
	AIMethods.spr_tam.n_z = 630
	AIMethods.ss_pit.include(AIMethods.spr_tam)

	# === FROSH ===
	# Skin tones
	var skin_tone_default: Array = [248, 208, 152]      # Light/Peach
	var skin_tone_pale: Array = [255, 223, 191]         # Very Pale
	var skin_tone_tan: Array = [222, 156, 89]           # Tan/Olive
	var skin_tone_medium_brown: Array = [172, 116, 68]  # Medium Brown
	var skin_tone_darker: Array = [134, 84, 39]         # Darker Brown
	var skin_tone_darkest: Array = [80, 51, 25]         # Darkest Brown
	var skin_tone_rosy: Array = [235, 180, 160]         # Rosy/Reddish

	var skin_tones: Array = [
		skin_tone_default, skin_tone_pale, skin_tone_tan,
		skin_tone_medium_brown, skin_tone_darker, skin_tone_darkest, skin_tone_rosy
	]

	# Shirt colors
	var shirt_tone_default: Array = [95, 0, 95]         # Purple
	var shirt_tone_2: Array = [66, 10, 69]              # Dark Purple
	var shirt_tone_3: Array = [136, 31, 142]            # Magenta
	var shirt_tone_4: Array = [109, 51, 112]            # Light Purple
	var shirt_deep_violet: Array = [75, 0, 130]
	var shirt_dark_orchid: Array = [153, 50, 204]
	var shirt_blue_violet: Array = [138, 43, 226]
	var shirt_plum: Array = [160, 90, 160]
	var shirt_indigo: Array = [85, 0, 110]
	var shirt_medium_purple: Array = [130, 80, 190]

	var shirt_colors: Array = [
		shirt_tone_default, shirt_tone_2, shirt_tone_3, shirt_tone_4,
		shirt_deep_violet, shirt_dark_orchid, shirt_blue_violet,
		shirt_plum, shirt_indigo, shirt_medium_purple
	]

	for i in range(AIDefine.GN_NUM_FROSH_IN_PIT):
		# Set up the Frosh personalities and include them in the special ss_fr spriteset
		tmp_sprite = SpriteInit.create_sprite(
			Enums.SpriteType.SPR_FROSH,
			AIMethods.R.randi() % 40,
			AIDefine.D_PIT_MIN_Y + (AIMethods.R.randi() % (AIDefine.D_PIT_MAX_Y - AIDefine.D_PIT_MIN_Y))
		)
		tmp_sprite.n_tag = i  # Add a tag to the Frosh - they're numbered now

		# Assign personality based on index
		if i < AIDefine.GN_NUM_START_HEAVYWEIGHT:
			tmp_sprite.n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY] = Enums.Personalities.PERS_HEAVYWEIGHT
		elif i < AIDefine.GN_NUM_START_HEAVYWEIGHT + AIDefine.GN_NUM_START_CLIMBER:
			tmp_sprite.n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY] = Enums.Personalities.PERS_CLIMBER
		elif i < AIDefine.GN_NUM_START_HEAVYWEIGHT + AIDefine.GN_NUM_START_CLIMBER + AIDefine.GN_NUM_START_HOISTER:
			tmp_sprite.n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY] = Enums.Personalities.PERS_HOISTER
		else:
			tmp_sprite.n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY] = Enums.Personalities.PERS_GOOFY

		# Randomize skin or shirt color (sprite supports only one RGB replacement)
		var chosen_skin: Array = skin_tones[AIMethods.R.randi() % skin_tones.size()]
		var chosen_shirt: Array = shirt_colors[AIMethods.R.randi() % shirt_colors.size()]

		# 50% chance for skin tint, 50% chance for shirt tint
		if AIMethods.R.randi() % 2 == 0:
			# Skin tint
			if chosen_skin != skin_tone_default:
				tmp_sprite.replace_rgb = skin_tone_default
				tmp_sprite.substitute_rgb = chosen_skin
		else:
			# Shirt tint
			if chosen_shirt != shirt_tone_default:
				tmp_sprite.replace_rgb = shirt_tone_default
				tmp_sprite.substitute_rgb = chosen_shirt

		AIMethods.ss_pit.include(tmp_sprite)
		AIMethods.ss_fr.include(tmp_sprite)

	# === CONSOLE AND ICONS ===
	var BARTOP: int = 108
	var ICO1HT: int = 50
	var ICO2HT: int = 36
	var ICO3HT: int = 51

	AIMethods.ss_console.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_GRID, 0, BARTOP))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_CONSOLE, 0, BARTOP))

	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, 0, BARTOP)
	tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOBAR])
	AIMethods.ss_icons.include(tmp_sprite)

	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_APPLE_ICON, 0, BARTOP))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PIZZA_ICON, 0, BARTOP + ICO1HT))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_CLARK_ICON, 0, BARTOP + ICO1HT + ICO2HT))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_EXAM_ICON, 0, BARTOP + ICO1HT + ICO2HT + ICO3HT))

	# Points and timer indicators
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_APPLE_TENS, 30, BARTOP + 24))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_APPLE_ONES, 50, BARTOP + 24))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PIZZA_TENS, 30, BARTOP + (24 * 3)))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PIZZA_ONES, 50, BARTOP + (24 * 3)))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_CLARK_TENS, 30, BARTOP + (24 * 5)))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_CLARK_ONES, 50, BARTOP + (24 * 5)))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_EXAM_TENS, 30, BARTOP + (24 * 7)))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_EXAM_ONES, 50, BARTOP + (24 * 7)))

	# Pit time display
	var D_CURRENT_SCORE_HEIGHT: int = 25
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PITTIME_HTENS, 475, D_CURRENT_SCORE_HEIGHT))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PITTIME_HONES, 495, D_CURRENT_SCORE_HEIGHT))
	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, 515, D_CURRENT_SCORE_HEIGHT)
	tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpALP_COLON])
	AIMethods.ss_icons.include(tmp_sprite)
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PITTIME_MTENS, 535, D_CURRENT_SCORE_HEIGHT))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PITTIME_MONES, 555, D_CURRENT_SCORE_HEIGHT))
	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, 575, D_CURRENT_SCORE_HEIGHT)
	tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpALP_PERIOD])
	AIMethods.ss_icons.include(tmp_sprite)
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PITTIME_STENS, 595, D_CURRENT_SCORE_HEIGHT))
	AIMethods.ss_icons.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_PITTIME_SONES, 615, D_CURRENT_SCORE_HEIGHT))

	# FPS display (debug)
	AIMethods.spr_fps_0 = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, 10, 290)
	AIMethods.ss_icons.include(AIMethods.spr_fps_0)
	AIMethods.spr_fps_1 = SpriteInit.create_sprite(Enums.SpriteType.SPR_INANIMATE, 30, 290)
	AIMethods.ss_icons.include(AIMethods.spr_fps_1)

	# === FORGE ===
	AIMethods.spr_forge = SpriteInit.create_sprite(Enums.SpriteType.SPR_FORGE, 0, 480)
	AIMethods.ss_icons.include(AIMethods.spr_forge)

	# === ARM ===
	if not b_demo:
		AIMethods.spr_arm = SpriteInit.create_sprite(Enums.SpriteType.SPR_ARM, 0, 0)
		AIMethods.ss_icons.include(AIMethods.spr_arm)
	else:
		AIMethods.spr_arm = SpriteInit.create_sprite(Enums.SpriteType.SPR_ARM, 0, 0)
		AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_DEMO
		AIMethods.ss_icons.include(AIMethods.spr_arm)

	# === MOUSE CURSOR ===
	AIMethods.ss_mouse.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_MOUSE_CURSOR_TL, 0, 0))
	AIMethods.ss_mouse.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_MOUSE_CURSOR_TR, 0, 0))
	AIMethods.ss_mouse.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_MOUSE_CURSOR_BL, 0, 0))
	AIMethods.ss_mouse.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_MOUSE_CURSOR_BR, 0, 0))

	# === HIGH SCORE DISPLAY ===
	var high_score: int = Globals.myGameConditions.get_high_score(Globals.myGameConditions.get_frosh_lameness())
	var high_score_hours: int = high_score / 1000 / 60
	var high_score_minutes: int = (high_score / 1000) % 60
	var high_score_seconds: int = (high_score / 10) % 100

	var START_OFFSET: int = 40

	# Hours tens
	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_HIGH_SCORE, 275 - START_OFFSET, AIDefine.D_HIGH_SCORE_START_HEIGHT)
	if high_score_hours < 10:
		tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	else:
		tmp_sprite.set_frame(_tens_digit_frame(high_score_hours))
	AIMethods.ss_icons.include(tmp_sprite)

	# Hours ones
	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_HIGH_SCORE, 295 - START_OFFSET, AIDefine.D_HIGH_SCORE_START_HEIGHT)
	if high_score_hours <= 0:
		tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
	else:
		tmp_sprite.set_frame(_ones_digit_frame(high_score_hours))
	AIMethods.ss_icons.include(tmp_sprite)

	# Colon
	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_HIGH_SCORE, 315 - START_OFFSET, AIDefine.D_HIGH_SCORE_START_HEIGHT)
	tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpALP_COLON])
	AIMethods.ss_icons.include(tmp_sprite)

	# Minutes tens
	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_HIGH_SCORE, 335 - START_OFFSET, AIDefine.D_HIGH_SCORE_START_HEIGHT)
	if high_score_minutes < 10:
		tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
	else:
		tmp_sprite.set_frame(_tens_digit_frame(high_score_minutes))
	AIMethods.ss_icons.include(tmp_sprite)

	# Minutes ones
	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_HIGH_SCORE, 355 - START_OFFSET, AIDefine.D_HIGH_SCORE_START_HEIGHT)
	if high_score_minutes < 0:
		tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	else:
		tmp_sprite.set_frame(_ones_digit_frame(high_score_minutes))
	AIMethods.ss_icons.include(tmp_sprite)

	# Period
	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_HIGH_SCORE, 375 - START_OFFSET, AIDefine.D_HIGH_SCORE_START_HEIGHT)
	tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpALP_PERIOD])
	AIMethods.ss_icons.include(tmp_sprite)

	# Seconds tens
	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_HIGH_SCORE, 395 - START_OFFSET, AIDefine.D_HIGH_SCORE_START_HEIGHT)
	if high_score_seconds < 10:
		tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
	else:
		tmp_sprite.set_frame(_tens_digit_frame(high_score_seconds))
	AIMethods.ss_icons.include(tmp_sprite)

	# Seconds ones
	tmp_sprite = SpriteInit.create_sprite(Enums.SpriteType.SPR_HIGH_SCORE, 415 - START_OFFSET, AIDefine.D_HIGH_SCORE_START_HEIGHT)
	if high_score_seconds <= 0:
		tmp_sprite.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0])
	else:
		tmp_sprite.set_frame(_ones_digit_frame(high_score_seconds))
	AIMethods.ss_icons.include(tmp_sprite)

	# Tri-pub ban chance
	if 0 == AIMethods.R.randi() % 3:
		Globals.myGameConditions.gb_tri_pub_ban = false

	return true


func _tens_digit_frame(value: int) -> Variant:
	var tens: int = (value / 10) % 10
	return AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0 + tens]


func _ones_digit_frame(value: int) -> Variant:
	var ones: int = value % 10
	return AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0 + ones]

func render_frame():
	# 1. Begin Frame
	Globals.RenderingService.begin_frame()

	# 2. Draw Sprites based on state
	# Note: During STATENOCHANGE (jacket slam transition), we still draw the current sprites
	# so the background is visible while the jacket slams down
	match current_game_state:
		Enums.GameStates.STATETITLE, Enums.GameStates.STATEOPTIONS, Enums.GameStates.STATEDECORATE:
			ss_menu.draw()
		Enums.GameStates.STATEGAME, Enums.GameStates.STATEDEMO:
			# Draw game sprites in correct layer order (back to front)
			# Order must match C#: clouds, balloon, trees, skyline, frecs, water, pit, tossed, console/icons, mouse
			AIMethods.ss_clouds.draw()     # Sky/clouds (furthest back)
			AIMethods.ss_balloon.draw()    # GW balloon (in sky)
			AIMethods.ss_trees.draw()      # Trees (behind skyline)
			AIMethods.ss_skyline.draw()    # Skyline, podium, George (in front of trees)
			AIMethods.ss_frecs.draw()      # Frec groups
			AIMethods.ss_water.draw()      # Water surface
			AIMethods.ss_pit.order_by_y()  # Sort pit sprites by Y before drawing
			AIMethods.ss_pit.draw()        # Pit (pole, tam, frosh)
			AIMethods.ss_tossed.draw()     # Tossed items
			AIMethods.ss_console.draw()    # Console grid
			AIMethods.ss_icons.draw()      # Icons, buttons, arm
			AIMethods.ss_mouse.draw()      # Mouse cursor (top layer)
		Enums.GameStates.STATENOCHANGE:
			# During transition, keep drawing current sprites as background
			# Check if we were in game mode by seeing if ss_pit has sprites
			if AIMethods.ss_pit.get_number_of_sprites() > 0:
				AIMethods.ss_clouds.draw()
				AIMethods.ss_balloon.draw()
				AIMethods.ss_trees.draw()
				AIMethods.ss_skyline.draw()
				AIMethods.ss_frecs.draw()
				AIMethods.ss_water.draw()
				AIMethods.ss_pit.order_by_y()
				AIMethods.ss_pit.draw()
				AIMethods.ss_tossed.draw()
				AIMethods.ss_console.draw()
				AIMethods.ss_icons.draw()
				AIMethods.ss_mouse.draw()
			else:
				ss_menu.draw()
		_:
			pass  # STATELOADING, STATEEXIT

	# 3. Draw jacket slam transition on top of everything (if active)
	AIMethods.ss_jacket_slam.draw()

	# 4. End Frame
	Globals.RenderingService.end_frame()

func process_ai():
	if b_processing_ai:
		return
	b_processing_ai = true

	# Update timer and get number of AI frames to process (fixed 25 FPS timestep)
	Globals.TimerService.update()
	var n_ai_queue: int = Globals.TimerService.get_additional_update_count()

	# Process each queued AI frame
	while n_ai_queue > 0:
		n_ai_queue -= 1

		# Input Handling - check once per AI frame
		var key = Globals.InputService.get_keyboard_input()
		if key != Enums.GreasepoleKeys.None:
			handle_key_down(key)

		# Think - process AI based on current state
		match current_game_state:
			Enums.GameStates.STATETITLE, Enums.GameStates.STATEOPTIONS, Enums.GameStates.STATEDECORATE:
				ss_menu.compact()
				ss_menu.think()
				ss_menu.calculate_screen_coordinates()
			Enums.GameStates.STATEGAME, Enums.GameStates.STATEDEMO:
				# Update pit time display values
				var temp: float = Globals.TimerService.get_current_game_time_score_milliseconds()
				if not Globals.myGameConditions.is_game_over():
					AIMethods.gn_pit_time_s = int(temp / 10) % 100
					AIMethods.gn_pit_time_m = int(temp / 1000) % 60
					AIMethods.gn_pit_time_h = int(temp / 1000 / 60)

				# Compact all sprite sets first (matches C# order)
				AIMethods.ss_clouds.compact()
				AIMethods.ss_balloon.compact()
				AIMethods.ss_skyline.compact()
				AIMethods.ss_trees.compact()
				AIMethods.ss_frecs.compact()
				AIMethods.ss_water.compact()
				AIMethods.ss_pit.compact()
				AIMethods.ss_tossed.compact()
				AIMethods.ss_icons.compact()
				AIMethods.ss_console.compact()
				AIMethods.ss_mouse.compact()

				# Think all sprite sets (matches C# order)
				AIMethods.ss_clouds.think()
				AIMethods.ss_balloon.think()
				AIMethods.ss_skyline.think()
				AIMethods.ss_trees.think()
				AIMethods.ss_frecs.think()
				AIMethods.ss_water.think()
				AIMethods.ss_pit.think()
				AIMethods.ss_tossed.think()
				AIMethods.ss_icons.think()
				AIMethods.ss_console.think()
				AIMethods.ss_mouse.think()

				# Scroll the screen based on mouse position
				Globals.myLayers.scroll_screen()

				# Calculate screen coordinates for all sprite sets (matches C# order)
				AIMethods.ss_clouds.calculate_screen_coordinates()
				AIMethods.ss_balloon.calculate_screen_coordinates()
				AIMethods.ss_skyline.calculate_screen_coordinates()
				AIMethods.ss_trees.calculate_screen_coordinates()
				AIMethods.ss_frecs.calculate_screen_coordinates()
				AIMethods.ss_water.calculate_screen_coordinates()
				AIMethods.ss_pit.calculate_screen_coordinates()
				AIMethods.ss_tossed.calculate_screen_coordinates()
				AIMethods.ss_icons.calculate_screen_coordinates()
				AIMethods.ss_console.calculate_screen_coordinates()
				AIMethods.ss_mouse.calculate_screen_coordinates()

				# Sort Frosh by X position for proper overlap
				AIMethods.ss_fr.compact()
				AIMethods.ss_fr.sort_by_x()
			Enums.GameStates.STATENOCHANGE:
				# During transition, still need to update sprites for background
				if AIMethods.ss_pit.get_number_of_sprites() > 0:
					# Was in game mode - use same order as C#
					# Compact all
					AIMethods.ss_clouds.compact()
					AIMethods.ss_balloon.compact()
					AIMethods.ss_skyline.compact()
					AIMethods.ss_trees.compact()
					AIMethods.ss_frecs.compact()
					AIMethods.ss_water.compact()
					AIMethods.ss_pit.compact()
					AIMethods.ss_tossed.compact()
					AIMethods.ss_icons.compact()
					AIMethods.ss_console.compact()
					AIMethods.ss_mouse.compact()
					# Think all
					AIMethods.ss_clouds.think()
					AIMethods.ss_balloon.think()
					AIMethods.ss_skyline.think()
					AIMethods.ss_trees.think()
					AIMethods.ss_frecs.think()
					AIMethods.ss_water.think()
					AIMethods.ss_pit.think()
					AIMethods.ss_tossed.think()
					AIMethods.ss_icons.think()
					AIMethods.ss_console.think()
					AIMethods.ss_mouse.think()
					# Scroll
					Globals.myLayers.scroll_screen()
					# Calculate screen coordinates all
					AIMethods.ss_clouds.calculate_screen_coordinates()
					AIMethods.ss_balloon.calculate_screen_coordinates()
					AIMethods.ss_skyline.calculate_screen_coordinates()
					AIMethods.ss_trees.calculate_screen_coordinates()
					AIMethods.ss_frecs.calculate_screen_coordinates()
					AIMethods.ss_water.calculate_screen_coordinates()
					AIMethods.ss_pit.calculate_screen_coordinates()
					AIMethods.ss_tossed.calculate_screen_coordinates()
					AIMethods.ss_icons.calculate_screen_coordinates()
					AIMethods.ss_console.calculate_screen_coordinates()
					AIMethods.ss_mouse.calculate_screen_coordinates()
					# Sort Frosh
					AIMethods.ss_fr.compact()
					AIMethods.ss_fr.sort_by_x()
				else:
					ss_menu.compact()
					ss_menu.think()
					ss_menu.calculate_screen_coordinates()

		# Update jacket slam transition sprites
		AIMethods.ss_jacket_slam.compact()
		AIMethods.ss_jacket_slam.think()
		AIMethods.ss_jacket_slam.calculate_screen_coordinates()

		# Signal to input service that ONE AI frame completed
		# This clears pending click flags so each click is seen for exactly one AI frame
		Globals.InputService.on_ai_frame_complete()

	b_processing_ai = false

func handle_key_down(key: int):
	match key:
		Enums.GreasepoleKeys.Back:
			if b_exit_on_escape:
				kill_game()
			else:
				change_game_state(Enums.GameStates.STATETITLE)
		Enums.GreasepoleKeys.IncreaseMunitions:
			if n_ritual_state_machine == 2:
				Globals.myGameConditions.get_apples(5)
				Globals.myGameConditions.get_pizzas(4)
				Globals.myGameConditions.get_clarks(3)
				Globals.myGameConditions.get_exams(1)
		Enums.GreasepoleKeys.ShowFPS:
			AIMethods.gb_show_fps = not AIMethods.gb_show_fps
		Enums.GreasepoleKeys.BuildRingEnergy:
			if n_ritual_state_machine == 2:
				if AIMethods.spr_forge != null:
					AIMethods.spr_forge.n_attrib[Enums.NAttrForge.ATTR_FORGE_ENERGY] = AIDefine.ENERGY_SWING
		Enums.GreasepoleKeys.GWBalloon:
			if n_ritual_state_machine == 2:
				if AIMethods.spr_gw_balloon == null:
					AIMethods.spr_gw_balloon = SpriteInit.create_sprite(Enums.SpriteType.SPR_GW_BALLOON)
					AIMethods.ss_balloon.include(AIMethods.spr_gw_balloon)
		Enums.GreasepoleKeys.PopupArtsciorCommie:
			if n_ritual_state_machine == 2:
				if AIMethods.spr_alien == null:
					var rand_val = AIMethods.R.randi() % 4
					match rand_val:
						0: AIMethods.spr_alien = SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_ARTSCIF)
						1: AIMethods.spr_alien = SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_ARTSCIM)
						2: AIMethods.spr_alien = SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_COMMIEF)
						3: AIMethods.spr_alien = SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_COMMIEM)
					AIMethods.ss_pit.include(AIMethods.spr_alien)
		Enums.GreasepoleKeys.PopupScicon:
			if n_ritual_state_machine == 2:
				var sprite_type = Enums.SpriteType.SPR_SCICON_F if AIMethods.R.randi() % 2 == 1 else Enums.SpriteType.SPR_SCICON_M
				AIMethods.ss_pit.include(SpriteInit.create_sprite(sprite_type))
		Enums.GreasepoleKeys.PopupHose:
			if n_ritual_state_machine == 2:
				AIMethods.ss_pit.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POPUP_HOSE))
		Enums.GreasepoleKeys.Mosh:
			if n_ritual_state_machine == 2:
				AISupport.ais_start_a_mosh()
		Enums.GreasepoleKeys.StartDemo:
			change_game_state(Enums.GameStates.STATEDEMO)
		Enums.GreasepoleKeys.ClarkC:
			n_ritual_state_machine = 1
		Enums.GreasepoleKeys.ClarkH:
			if n_ritual_state_machine == 1:
				n_ritual_state_machine = 2
		Enums.GreasepoleKeys.ClarkP:
			if n_ritual_state_machine == 2:
				n_ritual_state_machine = 0
				AIMethods.l_sound[Enums.ASLList.LSND_DISCIPLINES_RITUAL].play(SoundbankInfo.VOL_HOLLAR)
				for i in range(AIMethods.ss_icons.get_number_of_sprites()):
					if AIMethods.ss_icons.get_sprite(i).sprite_type == Enums.SpriteType.SPRMNU_BAR20:
						AIMethods.ss_icons.get_sprite(i).n_attrib[Enums.AttrBar.ATTR_BAR_GROUP] = 1

func kill_game():
	print("[GameLoop] Kill Game")
	Globals.get_tree().quit() # Wait, GameLoop is not a Node, so use Globals.get_tree() or pass usage
	# Globals is a Node
	Globals.get_tree().quit()
