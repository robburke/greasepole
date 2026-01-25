class_name AIMenuAndDisplay

# aiMenuAndDisplay.gd - Static class with menu and display logic
# Ported from aiMenuAndDisplay.cs
#
# Per porting_rules.md:
# - This is a static class with logic functions only
# - All shared state accessed via AIMethods autoload
# - Cross-file calls use class qualification

# === CONSTANTS ===
const D_MOUSE_OFFSET: int = 20
const D_MOUSE_OFFSET_SHIFTS: Array[int] = [0, 5, 8, 10, 11, 11, 11, 10, 8, 5, 0, -5, -8, -10, -11, -11, -11, -10, -8, -5]
const D_NUM_OFFSET_SHIFTS: int = 20
const NUM_JBAR_SPOTS: int = 4
const N_JSPOT_X: Array[int] = [181, 184, 187, 190]
const N_JSPOT_Y: Array[int] = [269, 304, 339, 373]

const D_NUM_TOGGLE_BUTTONS: int = 5
const N_BUTTON_SPOT_X: Array[int] = [85, 400, 85, 400, 259]
const N_BUTTON_SPOT_Y: Array[int] = [105, 105, 280, 280, 404]

const N_TITLE_START_GLOW: int = 3
const NCC_BASE_VALUE: int = 5680
const NCC_TRIGGER_DEMO: int = 6300
const N_PASS_CREST_GLOW: int = 8
const BAR_OFF_JACKET: int = -1

# === STATIC VARIABLES (stored in AIMethods for shared state) ===
# These are tracked per-session, accessed via AIMethods
# n_old_mouse_x, n_old_mouse_y - tracked for mouse movement detection

static var n_old_mouse_x: int = 0
static var n_old_mouse_y: int = 0

# === MOUSE CURSOR ===

static func ai_mouse_cursor_menu(s: TSprite) -> void:
	s.n_x = Globals.InputService.get_mouse_x()
	s.n_y = Globals.InputService.get_mouse_y()
	if Globals.InputService.left_button_pressed() or Globals.InputService.left_button_down():
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMOU_SELECT2])
	elif Globals.InputService.right_button_pressed():
		s.b_attrib[1] = not s.b_attrib[1]
	else:
		var frame_id: int = Enums.GameBitmapEnumeration.bmpMOU_SELECT1 if s.b_attrib[1] else Enums.GameBitmapEnumeration.bmpMOU_SELECT3
		s.set_frame(AIMethods.frm[frame_id])


static func ai_mouse_has_moved() -> bool:
	var n_mouse_x: int = Globals.InputService.get_mouse_x()
	var n_mouse_y: int = Globals.InputService.get_mouse_y()
	var b_answer: bool = (n_old_mouse_x != n_mouse_x) or (n_old_mouse_y != n_mouse_y)
	n_old_mouse_x = n_mouse_x
	n_old_mouse_y = n_mouse_y
	return b_answer


# === MENU BUTTONS ===

static func ai_menu_start_button(s: TSprite) -> void:
	if not s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_DO_NOT_ACTIVATE]:
		# Magic timing: (7 * 23), (40 * 23), (15 * 23) - preserved from C#
		if 0 == ((s.n_cc - (7 * 23)) % (40 * 23)) and 0 == AIMethods.ss_jacket_slam.get_number_of_sprites():
			AIMethods.l_sound[Enums.ASLList.LSND_NARRATOR_STARTDELAY1].play(SoundbankInfo.VOL_HOLLAR)
		if 0 == ((s.n_cc - (15 * 23)) % (40 * 23)) and 0 == AIMethods.ss_jacket_slam.get_number_of_sprites():
			AIMethods.l_sound[Enums.ASLList.LSND_NARRATOR_STARTDELAY2].play(SoundbankInfo.VOL_HOLLAR)

	if Globals.InputService.start_button_pressed():
		Globals.GameLoop.change_game_state(Enums.GameStates.STATEDECORATE)

	if AISupport.ais_mouse_over(s):
		match ((s.n_cc - s.n_attrib[6]) / 2) % 8:
			0: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLESTARTGLOW1])
			1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLESTARTGLOW2])
			2: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLESTARTGLOW3])
			7: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLESTARTGLOW2])

		if Globals.InputService.left_button_pressed() and not s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_DO_NOT_ACTIVATE]:
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_SELECT].play(SoundbankInfo.VOL_FULL)
			Globals.GameLoop.change_game_state(Enums.GameStates.STATEDECORATE)
	else:
		s.n_attrib[6] = s.n_cc
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLESTART])

	# Music handling - check if title music should loop
	if not AIMethods.l_sound[Enums.ASLList.LSND_MUSIC_TITLEINIT].is_playing() \
		and not AIMethods.s_sound[Enums.ASSList.SSND_MENU_TITLEREPEAT].is_playing() \
		and not AIMethods.l_sound[Enums.ASLList.LSND_MUSIC_SCOTLAND].is_playing() \
		and not s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_MAKE_TITLE_SOUND_PLAY]:
		s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_MAKE_TITLE_SOUND_PLAY] = true
		AIMethods.s_sound[Enums.ASSList.SSND_MENU_TITLEREPEAT].loop(SoundbankInfo.VOL_MUSIC)
		AIMethods.s_sound[Enums.ASSList.SSND_MENU_DECORATEREPEAT].stop()


static func ai_menu_options_button(s: TSprite) -> void:
	if AISupport.ais_mouse_over(s):
		match ((s.n_cc - s.n_attrib[3]) / 2) % 8:
			0: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEOPTIONSGLOW1])
			1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEOPTIONSGLOW2])
			2: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEOPTIONSGLOW3])
			7: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEOPTIONSGLOW2])

		if Globals.InputService.left_button_pressed() and not s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_DO_NOT_ACTIVATE]:
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_TOGGLE].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
			Globals.GameLoop.change_game_state(Enums.GameStates.STATEOPTIONS)
	else:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEOPTIONS])
		s.n_attrib[3] = s.n_cc


static func ai_menu_exit_button(s: TSprite) -> void:
	if Globals.InputService.back_button_pressed():
		AIMethods.s_sound[Enums.ASSList.SSND_MENU_TOGGLE].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
		Globals.GameLoop.change_game_state(Enums.GameStates.STATEEXIT)

	if s.n_cc < NCC_BASE_VALUE or s.n_cc > NCC_TRIGGER_DEMO:
		s.n_cc = NCC_BASE_VALUE
	if ai_mouse_has_moved():
		s.n_cc = NCC_BASE_VALUE

	if AISupport.ais_mouse_over(s) or s.n_cc == NCC_TRIGGER_DEMO:
		if Globals.InputService.left_button_pressed() or s.n_cc == NCC_TRIGGER_DEMO:
			# Handle credits sequence
			if s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN] == 0 or s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN] == 7:
				AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ICONOUT].play(SoundbankInfo.VOL_FULL)

			# Demo mode trigger
			if s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN] == 7 and s.n_cc == NCC_TRIGGER_DEMO:
				Globals.GameLoop.change_game_state(Enums.GameStates.STATEDEMO)

			# Mark other buttons as inactive during credits
			# Also move TITLEBACK up by 40 pixels when credits start
			var j: int = 0
			var ss = Globals.GameLoop.ss_menu
			while j < ss.get_number_of_sprites():
				var spr: TSprite = ss.get_sprite(j)
				if spr == null:
					j += 1
					continue
				if spr.sprite_type == Enums.SpriteType.SPRMNU_TITLESTART or spr.sprite_type == Enums.SpriteType.SPRMNU_TITLEOPTIONS:
					spr.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_DO_NOT_ACTIVATE] = true
				if spr.sprite_type == Enums.SpriteType.SPRMNU_TITLEBACK and s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN] == 0:
					AIFlyInAndOut.ai_init_fly_in_and_out_2(spr, Callable(AIMenuAndDisplay, "ai_inanimate"), spr.n_x, spr.n_y, spr.n_x, spr.n_y - 40, 1, 1)
				j += 1

			# Fly out animation
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_fly_back_in_credits"),
				s.n_x, s.n_y, s.n_x, s.n_y + 300, 1, 1)

			if s.n_cc == NCC_TRIGGER_DEMO:
				s.n_cc = NCC_TRIGGER_DEMO - 100
			else:
				s.n_cc = NCC_BASE_VALUE
	else:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEEXIT])
		s.n_attrib[3] = s.n_cc


static func ai_fly_back_in_credits(s: TSprite) -> void:
	s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN] += 1

	if s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN] == 1 or s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN] == 8:
		AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ICONIN].play(SoundbankInfo.VOL_FULL)

	if s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN] > 7:
		# Reset credits
		s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN] = 0
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLEEXIT])
		s.n_x = 109
		AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_menu_exit_button"),
			109, s.n_y, 109, 31, 1, 1)

		# Re-enable other buttons and move TITLEBACK back down
		var j: int = 0
		var ss = Globals.GameLoop.ss_menu
		while j < ss.get_number_of_sprites():
			var spr: TSprite = ss.get_sprite(j)
			if spr == null:
				j += 1
				continue
			if spr.sprite_type == Enums.SpriteType.SPRMNU_TITLESTART or spr.sprite_type == Enums.SpriteType.SPRMNU_TITLEOPTIONS:
				spr.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_DO_NOT_ACTIVATE] = false
			if spr.sprite_type == Enums.SpriteType.SPRMNU_TITLEBACK:
				AIFlyInAndOut.ai_init_fly_in_and_out_2(spr, Callable(AIMenuAndDisplay, "ai_inanimate"), spr.n_x, spr.n_y, spr.n_x, spr.n_y + 40, 1, 1)
			j += 1

		# Reorder TITLEOPTIONS to draw after credits (for z-order)
		for k in range(ss.get_number_of_sprites()):
			var opt_sprite: TSprite = ss.get_sprite(k)
			if opt_sprite != null and opt_sprite.sprite_type == Enums.SpriteType.SPRMNU_TITLEOPTIONS:
				ss.remove(opt_sprite)
				ss.include(opt_sprite)
				break

		# Reorder mouse cursor to draw last (for z-order)
		for k in range(ss.get_number_of_sprites()):
			var cursor_sprite: TSprite = ss.get_sprite(k)
			if cursor_sprite != null and cursor_sprite.sprite_type == Enums.SpriteType.SPRMNU_MOUSE_CURSOR:
				ss.remove(cursor_sprite)
				ss.include(cursor_sprite)
				break
	else:
		# During credits, reorder this sprite (the credits image) to draw on top
		var ss = Globals.GameLoop.ss_menu
		for k in range(ss.get_number_of_sprites()):
			var opt_sprite: TSprite = ss.get_sprite(k)
			if opt_sprite != null and opt_sprite == s:
				ss.remove(opt_sprite)
				ss.include(opt_sprite)
				break

		# Also ensure mouse cursor is on top
		for k in range(ss.get_number_of_sprites()):
			var cursor_sprite: TSprite = ss.get_sprite(k)
			if cursor_sprite != null and cursor_sprite.sprite_type == Enums.SpriteType.SPRMNU_MOUSE_CURSOR:
				ss.remove(cursor_sprite)
				ss.include(cursor_sprite)
				break

		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpTITLECREDITS1 - 1 + s.n_attrib[Enums.NAttrExitAndCredits.ATTR_CREDITS_SCREEN]])
		s.n_x = 0
		AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_menu_exit_button"),
			0, s.n_y, 0, 230, 3, 3)


# === PASS CREST (DECORATE SCREEN) ===

static func ais_clicked_on_pass_crest(s: TSprite) -> void:
	AIMethods.s_sound[Enums.ASSList.SSND_MENU_SELECT].play(SoundbankInfo.VOL_MUSIC, AICrowd.pan_on_x(s))
	Globals.GameLoop.change_game_state(Enums.GameStates.STATEGAME)


static func ai_menu_glowing_pass_crest(s: TSprite) -> void:
	# Magic timing: (22 * 45), (23 * 45) - preserved from C#
	if 0 == ((s.n_cc - (22 * 45)) % (23 * 45)):
		AIMethods.l_sound[Enums.ASLList.LSND_NARRATOR_JACKETINIT].play(SoundbankInfo.VOL_HOLLAR)

	if Globals.InputService.start_button_pressed():
		ais_clicked_on_pass_crest(s)

	if AISupport.ais_mouse_over(s):
		if s.n_cc > 4000:
			s.n_cc = 0
		match ((s.n_cc / 2) % N_PASS_CREST_GLOW):
			0: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMENUPASSCREST1])
			1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMENUPASSCREST2])
			2: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMENUPASSCREST3])
			7: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMENUPASSCREST2])

		if Globals.InputService.left_button_pressed():
			ais_clicked_on_pass_crest(s)
	else:
		s.n_cc = 4000
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMENUPASSCREST1])

	# Music handling - check if decorate music should loop
	if not AIMethods.l_sound[Enums.ASLList.LSND_MUSIC_SCOTLAND].is_playing() \
		and not AIMethods.s_sound[Enums.ASSList.SSND_MENU_DECORATEREPEAT].is_playing() \
		and not AIMethods.l_sound[Enums.ASLList.LSND_MUSIC_TITLEINIT].is_playing() \
		and not s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_MAKE_TITLE_SOUND_PLAY]:
		s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_MAKE_TITLE_SOUND_PLAY] = true
		AIMethods.s_sound[Enums.ASSList.SSND_MENU_DECORATEREPEAT].loop(SoundbankInfo.VOL_MUSIC)
		AIMethods.s_sound[Enums.ASSList.SSND_MENU_TITLEREPEAT].stop()


# === TOGGLE BUTTONS (OPTIONS) ===

static func ai_init_toggle_button(s: TSprite, n_button_number: int) -> void:
	s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X] = 320
	s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y] = 480

	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpBTNTOGGLE])
	s.n_attrib[Enums.AttrToggleButton.ATTR_BUTTON_NUMBER] = n_button_number

	match n_button_number:
		0:
			s.n_attrib[Enums.AttrToggleButton.ATTR_ON_SCREEN_POSITION] = Globals.myGameConditions.get_frosh_lameness()
		1:
			s.n_attrib[Enums.AttrToggleButton.ATTR_ON_SCREEN_POSITION] = Globals.myGameConditions.get_sound()
		2:
			s.n_attrib[Enums.AttrToggleButton.ATTR_ON_SCREEN_POSITION] = Globals.myGameConditions.get_enhanced_graphics()
		3:
			s.n_attrib[Enums.AttrToggleButton.ATTR_ON_SCREEN_POSITION] = Globals.myGameConditions.get_rmb_function()

	s.b_attrib[Enums.BAttrBar.BATTR_BEING_DRAGGED] = false

	var btn_num: int = s.n_attrib[Enums.AttrToggleButton.ATTR_BUTTON_NUMBER]
	if s.n_attrib[Enums.AttrToggleButton.ATTR_ON_SCREEN_POSITION] == 1:  # Go right
		AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_toggle_button"),
			s.n_x, s.n_y, N_BUTTON_SPOT_X[btn_num] + 150, N_BUTTON_SPOT_Y[btn_num], 1, 1)
	else:
		AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_toggle_button"),
			s.n_x, s.n_y, N_BUTTON_SPOT_X[btn_num], N_BUTTON_SPOT_Y[btn_num], 1, 1)


static func ai_toggle_button(s: TSprite) -> void:
	if not s.b_attrib[Enums.BAttrBar.BATTR_BEING_DRAGGED]:
		if Globals.InputService.left_button_pressed() and AISupport.ais_mouse_over(s):
			s.n_cc = 0
			s.b_attrib[Enums.BAttrBar.BATTR_BEING_DRAGGED] = true
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_TOGGLE].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
	else:  # being dragged
		s.n_x = Globals.InputService.get_mouse_x()
		s.n_y = Globals.InputService.get_mouse_y()
		if not Globals.InputService.left_button_down():
			var btn_num: int = s.n_attrib[Enums.AttrToggleButton.ATTR_BUTTON_NUMBER]
			var n_value_selected: int = 1 if s.n_x > N_BUTTON_SPOT_X[btn_num] + 100 else 0
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_DROP].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
			s.b_attrib[Enums.BAttrBar.BATTR_BEING_DRAGGED] = false

			if n_value_selected != 0:  # Go right
				AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_toggle_button"),
					s.n_x, s.n_y, N_BUTTON_SPOT_X[btn_num] + 150, N_BUTTON_SPOT_Y[btn_num], 1, 1)
			else:
				AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_toggle_button"),
					s.n_x, s.n_y, N_BUTTON_SPOT_X[btn_num], N_BUTTON_SPOT_Y[btn_num], 1, 1)

			match btn_num:
				0: Globals.myGameConditions.set_frosh_lameness(n_value_selected)
				1: Globals.myGameConditions.set_sound(n_value_selected)
				2: Globals.myGameConditions.set_enhanced_graphics(n_value_selected)
				3: Globals.myGameConditions.set_rmb_function(n_value_selected)


# === RETURN BUTTONS ===

static func ai_options_return(s: TSprite) -> void:
	# Music handling - check if decorate music should loop
	if not AIMethods.l_sound[Enums.ASLList.LSND_MUSIC_TITLEINIT].is_playing() \
		and not AIMethods.s_sound[Enums.ASSList.SSND_MENU_DECORATEREPEAT].is_playing() \
		and not AIMethods.l_sound[Enums.ASLList.LSND_MUSIC_SCOTLAND].is_playing() \
		and not s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_MAKE_TITLE_SOUND_PLAY]:
		s.b_attrib[Enums.BAttrMenuStartButtonAttributes.ATTR_MAKE_TITLE_SOUND_PLAY] = true
		AIMethods.s_sound[Enums.ASSList.SSND_MENU_TITLEREPEAT].stop()
		AIMethods.s_sound[Enums.ASSList.SSND_MENU_DECORATEREPEAT].loop(SoundbankInfo.VOL_MUSIC)

	if AISupport.ais_mouse_over(s):
		if Globals.InputService.left_button_pressed():
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_SELECT].play(SoundbankInfo.VOL_FULL)
			Globals.GameLoop.change_game_state(Enums.GameStates.STATETITLE)
	if Globals.InputService.back_button_pressed():
		AIMethods.s_sound[Enums.ASSList.SSND_MENU_SELECT].play(SoundbankInfo.VOL_FULL)
		Globals.GameLoop.change_game_state(Enums.GameStates.STATETITLE)


static func ai_decorate_return(s: TSprite) -> void:
	if AISupport.ais_mouse_over(s):
		if Globals.InputService.left_button_pressed():
			AIMethods.s_sound[Enums.ASSList.SSND_MENU_SELECT].play(SoundbankInfo.VOL_FULL)
			Globals.GameLoop.change_game_state(Enums.GameStates.STATETITLE)
	if Globals.InputService.back_button_pressed():
		AIMethods.s_sound[Enums.ASSList.SSND_MENU_SELECT].play(SoundbankInfo.VOL_FULL)
		Globals.GameLoop.change_game_state(Enums.GameStates.STATETITLE)


# === INANIMATE (does nothing) ===

static func ai_inanimate(s: TSprite) -> void:
	# Does nothing - used as placeholder behavior
	pass


# === CONSOLE AND GRID ===

static func ai_console(s: TSprite) -> void:
	# Console does nothing (static display element)
	pass


static func ai_grid(s: TSprite) -> void:
	# Animate the radar grid with 3 frames
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGRID1 + ((s.n_cc / 7) % 3)])


# === ICON HELPER FUNCTIONS ===

static func ais_get_points_based_on(n_attr_button_type: int) -> int:
	var n_temp: int = 0
	match n_attr_button_type:
		Enums.Buttons.BUTTON_APPLE:
			n_temp = Globals.myGameConditions.get_player_apples()
		Enums.Buttons.BUTTON_PIZZA:
			n_temp = Globals.myGameConditions.get_player_pizza()
		Enums.Buttons.BUTTON_CLARK:
			n_temp = Globals.myGameConditions.get_player_clark()
		Enums.Buttons.BUTTON_EXAM:
			n_temp = Globals.myGameConditions.get_player_exam()
	return n_temp


static func ai_points_tens(s: TSprite) -> void:
	var n_temp: int = ais_get_points_based_on(s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE])
	if n_temp < 10:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	else:
		s.set_frame(tens_digit_frame(n_temp))


static func ai_points_ones(s: TSprite) -> void:
	var n_temp: int = ais_get_points_based_on(s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE])
	if n_temp <= 0:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	else:
		s.set_frame(ones_digit_frame(n_temp))


# === ICON BUTTON BEHAVIOR ===

static func ai_icon_into_position(s: TSprite) -> void:
	match s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]:
		Enums.Buttons.BUTTON_APPLE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOAPPLE1])
		Enums.Buttons.BUTTON_PIZZA:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOPIZZA1])
		Enums.Buttons.BUTTON_CLARK:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOCLARK1])
		Enums.Buttons.BUTTON_EXAM:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOEXAM1])

	if s.n_x < s.n_dest_x:
		s.n_x += s.nv_x
		s.nv_x += 1
	else:
		s.n_x = s.n_dest_x  # "SLAM!"
		s.nv_x = 5
		s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 1


static func ai_icon_out_of_position(s: TSprite) -> void:
	match s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]:
		Enums.Buttons.BUTTON_APPLE:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOAPPLE1])
		Enums.Buttons.BUTTON_PIZZA:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOPIZZA1])
		Enums.Buttons.BUTTON_CLARK:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOCLARK1])
		Enums.Buttons.BUTTON_EXAM:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOEXAM1])

	if s.n_x > -90:
		s.n_x -= s.nv_x
		s.nv_x += 1

	if ais_get_points_based_on(s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]) != 0:
		s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 0


static func ai_icon(s: TSprite) -> void:
	var b_mouse_over: bool = AISupport.ais_mouse_over(s)
	match s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS]:
		0:  # Into position
			if ais_get_points_based_on(s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]) == 0:
				AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ICONOUT].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
				s.n_attrib[Enums.AttrIcon.ATTR_ICON_STATUS] = 2
			ai_icon_into_position(s)
		1:  # In position
			if not b_mouse_over:
				match s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]:
					Enums.Buttons.BUTTON_APPLE:
						s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOAPPLE2])
					Enums.Buttons.BUTTON_PIZZA:
						s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOPIZZA2])
					Enums.Buttons.BUTTON_CLARK:
						s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOCLARK2])
					Enums.Buttons.BUTTON_EXAM:
						s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOEXAM2])

			# Check for arm status to highlight icon
			var arm_status: int = 0
			var arm_action: int = 0
			if AIMethods.spr_arm != null:
				arm_status = AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS]
				arm_action = AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION]

			match s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]:
				Enums.Buttons.BUTTON_APPLE:
					if b_mouse_over or arm_status == Enums.ArmPositions.ARM_APPLE \
							or (arm_action == Enums.ArmPositions.ARM_APPLE and arm_status == Enums.ArmPositions.ARM_CHANGING) \
							or arm_status == Enums.ArmPositions.ARM_OTHROW:
						s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOAPPLE3])
				Enums.Buttons.BUTTON_PIZZA:
					if b_mouse_over or arm_status == Enums.ArmPositions.ARM_PIZZA \
							or (arm_action == Enums.ArmPositions.ARM_PIZZA and arm_status == Enums.ArmPositions.ARM_CHANGING) \
							or arm_status == Enums.ArmPositions.ARM_STHROW:
						s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOPIZZA3])
				Enums.Buttons.BUTTON_CLARK:
					if b_mouse_over or arm_status == Enums.ArmPositions.ARM_CLARK \
							or (arm_action == Enums.ArmPositions.ARM_CLARK and arm_status == Enums.ArmPositions.ARM_CHANGING) \
							or arm_status == Enums.ArmPositions.ARM_STHROW2:
						s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOCLARK3])
				Enums.Buttons.BUTTON_EXAM:
					if b_mouse_over or arm_status == Enums.ArmPositions.ARM_EXAM \
							or (arm_action == Enums.ArmPositions.ARM_EXAM and arm_status == Enums.ArmPositions.ARM_CHANGING) \
							or arm_status == Enums.ArmPositions.ARM_STHROW3:
						s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICOEXAM3])

			# Handle click on icon
			if Globals.InputService.left_button_pressed() and b_mouse_over:
				if AIMethods.spr_arm != null and arm_status != Enums.ArmPositions.ARM_CHANGING:
					AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ICONIN].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
					AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
					AIMethods.spr_arm.n_cc = 0
					match s.n_attrib[Enums.AttrIcon.ATTR_BUTTON_TYPE]:
						Enums.Buttons.BUTTON_APPLE:
							AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_APPLE
						Enums.Buttons.BUTTON_PIZZA:
							AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_PIZZAREADY].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
							AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_PIZZA
						Enums.Buttons.BUTTON_CLARK:
							AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_POUR].play(SoundbankInfo.VOL_NORMAL, AICrowd.pan_on_x(s))
							AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_CLARK
						Enums.Buttons.BUTTON_EXAM:
							AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_EXAM
		2:  # Out of position
			ai_icon_out_of_position(s)


# === HIGH SCORE DISPLAY ===

static func ai_high_score(s: TSprite) -> void:
	if s.n_cc < 100:
		if (s.n_cc / 6) % 2 != 0:
			s.n_y = -40
		else:
			s.n_y = AIDefine.D_HIGH_SCORE_START_HEIGHT
	else:
		s.n_y = AIDefine.D_HIGH_SCORE_START_HEIGHT
		AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMisc, "ai_delete_me"), s.n_x, s.n_y, s.n_x, -40, 1, 1)


# === TRACKER SPRITES (follow first frosh position) ===

static func ai_tracker_lr(s: TSprite) -> void:
	if AIMethods.ss_fr.get_sprite(0) != null and AIMethods.ss_fr.get_sprite(0).frm_frame != null:
		s.n_y = AIMethods.ss_fr.get_bottom_most_scr_point_on_sprite(0)
		s.n_x = AIMethods.ss_fr.get_right_most_scr_point_on_sprite(0)


static func ai_tracker_ul(s: TSprite) -> void:
	if AIMethods.ss_fr.get_sprite(0) != null and AIMethods.ss_fr.get_sprite(0).frm_frame != null:
		s.n_y = AIMethods.ss_fr.get_top_most_scr_point_on_sprite(0)
		s.n_x = AIMethods.ss_fr.get_left_most_scr_point_on_sprite(0)


# === GAME MOUSE CURSORS (targeting crosshairs) ===

static func ai_mouse_cursor_tl(s: TSprite) -> void:
	s.n_x = Globals.InputService.get_mouse_x() - D_MOUSE_OFFSET - D_MOUSE_OFFSET_SHIFTS[s.n_cc % D_NUM_OFFSET_SHIFTS]
	s.n_y = Globals.InputService.get_mouse_y() - D_MOUSE_OFFSET - D_MOUSE_OFFSET_SHIFTS[s.n_cc % D_NUM_OFFSET_SHIFTS]
	if Globals.myGameConditions.is_demo():
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	else:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMOU_TARTL])


static func ai_mouse_cursor_tr(s: TSprite) -> void:
	s.n_x = Globals.InputService.get_mouse_x() + D_MOUSE_OFFSET + D_MOUSE_OFFSET_SHIFTS[s.n_cc % D_NUM_OFFSET_SHIFTS]
	s.n_y = Globals.InputService.get_mouse_y() - D_MOUSE_OFFSET - D_MOUSE_OFFSET_SHIFTS[s.n_cc % D_NUM_OFFSET_SHIFTS]
	if Globals.myGameConditions.is_demo():
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	else:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMOU_TARTR])


static func ai_mouse_cursor_bl(s: TSprite) -> void:
	s.n_x = Globals.InputService.get_mouse_x() - D_MOUSE_OFFSET - D_MOUSE_OFFSET_SHIFTS[s.n_cc % D_NUM_OFFSET_SHIFTS]
	s.n_y = Globals.InputService.get_mouse_y() + D_MOUSE_OFFSET + D_MOUSE_OFFSET_SHIFTS[s.n_cc % D_NUM_OFFSET_SHIFTS]
	if Globals.myGameConditions.is_demo():
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	else:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMOU_TARBL])


static func ai_mouse_cursor_br(s: TSprite) -> void:
	if Globals.myGameConditions.is_demo():
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	else:
		s.n_x = Globals.InputService.get_mouse_x() + D_MOUSE_OFFSET + D_MOUSE_OFFSET_SHIFTS[s.n_cc % D_NUM_OFFSET_SHIFTS]
		s.n_y = Globals.InputService.get_mouse_y() + D_MOUSE_OFFSET + D_MOUSE_OFFSET_SHIFTS[s.n_cc % D_NUM_OFFSET_SHIFTS]
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpMOU_TARBR])


# === METER DISPLAYS ===

static func ai_power_meter(s: TSprite) -> void:
	if s.n_y < 14:
		s.n_y = 14

	# If the arm unexpectedly changes to another item don't leave the power bar there
	if AIMethods.spr_arm == null or AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_APPLE:
		s.b_deleted = true
		if AIMethods.spr_power_meter == s:
			AIMethods.spr_power_meter = null

	# If the power meter is on the decline...
	if s.b_attrib[1]:
		s.n_y += 4
		if s.n_y > 95:
			s.b_deleted = true
			if AIMethods.spr_power_meter == s:
				AIMethods.spr_power_meter = null
	else:
		# The power meter is on the rise
		s.n_y -= 4


static func ai_water_meter(s: TSprite) -> void:
	if s.b_deleted:
		return

	if s.n_y < 16:
		s.n_y = 16
	if s.n_cc == 25:
		AIMethods.l_sound[Enums.ASLList.LSND_HOSE_TAKE1 + AIMethods.R.randi() % 2].play(SoundbankInfo.VOL_HOLLAR)
	if s.n_y > 92:
		s.b_attrib[1] = true

	if AIMethods.spr_arm == null or AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_GREASE:
		s.b_attrib[1] = true
	elif (s.n_cc % 7) == 0 and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] == 1:
		s.n_y += 1

	if s.b_attrib[1]:
		if (s.n_cc % 14) == 0:
			s.n_y += 1
		if s.n_y > 92:
			if AIMethods.spr_water_meter == s:
				AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
				AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = ais_change_arm()
			AIMethods.spr_arm.n_cc = 0
			AIMethods.s_sound[Enums.ASSList.SSND_WATER_HOSE].stop()
			s.b_deleted = true
			if AIMethods.spr_water_meter == s:
				AIMethods.spr_water_meter = null


static func ai_ring_meter(s: TSprite) -> void:
	if s.b_deleted:
		return

	if s.n_y < 16:
		s.n_y = 16
	if s.n_y > 92:
		s.b_attrib[1] = true

	if AIMethods.spr_arm == null or AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] != Enums.ArmPositions.ARM_IRON_RING:
		s.b_attrib[1] = true
	elif (s.n_cc % 7) == 0 and AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] == 1:
		s.n_y += 1

	if s.b_attrib[1]:
		if (s.n_cc % 14) == 0:
			s.n_y += 1
		if s.n_y > 92:
			s.b_deleted = true
			if AIMethods.spr_ring_meter == s:
				AIMethods.spr_ring_meter = null


static func ais_change_arm() -> int:
	# Returns the arm action the arm should change to (placeholder implementation)
	return Enums.ArmPositions.ARM_NOTHING


# === ACHIEVEMENT UNLOCKED NOTICE ===

static func ai_achievement_unlocked_notice(s: TSprite) -> void:
	if s.n_cc == 30:
		if s.sprite_text == TSprite.SpriteTextType.None:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpACHIEVEMENTUNLOCKED2])
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_ACHIEVEMENTUNLOCKED2].play(SoundbankInfo.VOL_FULL, AICrowd.pan_on_x(s))
		else:
			s.n_a = 255

	if s.n_cc >= 140:
		AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMisc, "ai_delete_me"), s.n_x, s.n_y, s.n_x, -40, 1, 1)


# === BAR INITIALIZATION ===

static func ai_init_bar(s: TSprite, n_bar_number: int) -> void:
	s.n_attrib[Enums.AttrBar.ATTR_BAR_GROUP] = n_bar_number / 10
	s.n_attrib[Enums.AttrBar.ATTR_ON_JACKET_POSITION] = BAR_OFF_JACKET
	s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X] = 553
	s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y] = (n_bar_number % 10) * 40 + 40

	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpBAR1 + n_bar_number])

	s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_DISCIPLINE] = n_bar_number

	# Set associated sound based on bar number
	match n_bar_number:
		0: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_APPLE
		1: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_CHEM
		2: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_CIVIL
		3: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_ELEC
		4: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_ENGPHYS
		5: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_ENGCHEM
		6: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_GEO
		7, 17: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_METALS
		8: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_MECH
		9: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_MINING
		15: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_MECH
		19: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_RITUAL
		_: s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND] = Enums.ASLList.LSND_DISCIPLINES_DEFAULT

	s.b_attrib[Enums.BAttrBar.BATTR_BEING_DRAGGED] = false

	# Check if bar is already on jacket (from saved game state)
	var b_already_on_jacket: bool = false
	for i in range(NUM_JBAR_SPOTS):
		if Globals.myGameConditions.get_j_bar(i) == n_bar_number:
			b_already_on_jacket = true
			s.n_attrib[Enums.AttrBar.ATTR_ON_JACKET_POSITION] = i
			s.n_x = 800
			s.n_y = N_JSPOT_Y[i]
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_bar"), s.n_x, s.n_y, N_JSPOT_X[i], N_JSPOT_Y[i], 1, 1)
			break

	if not b_already_on_jacket:
		# Start off-screen, staggered by bar group
		s.n_x = 840 + (n_bar_number / 10) * 20
		s.n_y = s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y]


# === ACHIEVEMENT TEXT ===

static func ai_achievement_text(s: TSprite) -> void:
	var b_my_bar_group_on_screen: bool = s.n_attrib[Enums.AttrBar.ATTR_BAR_GROUP] == Globals.myGameConditions.get_achievement_group()
	var b_x_on_screen: bool = s.n_x >= 0 and s.n_x <= 640

	if not s.b_attrib[Enums.BAttrBar.BATTR_BEING_DRAGGED]:
		if not b_my_bar_group_on_screen and b_x_on_screen:
			# Fly out
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_achievement_text"), s.n_x, s.n_y, 700, s.n_y, 1, 1)
		if b_my_bar_group_on_screen and not b_x_on_screen:
			# Fly in
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_achievement_text"), s.n_x, s.n_y, s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X], s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y], 1, 1)


# === NEXT ACHIEVEMENT SCREEN ===

static func ai_next_achievement_screen(s: TSprite) -> void:
	var max_groups: int = (PoleGameAchievement.list.size() + 5) / 6  # 6 achievements per screen
	if Globals.InputService.left_button_pressed() and AISupport.ais_mouse_over(s):
		Globals.myGameConditions.set_achievement_group((Globals.myGameConditions.get_achievement_group() + 1) % max_groups)
	if Globals.InputService.toggle_forward_button_pressed():
		Globals.myGameConditions.set_achievement_group((Globals.myGameConditions.get_achievement_group() + 1) % max_groups)
	if Globals.InputService.toggle_back_button_pressed():
		Globals.myGameConditions.set_achievement_group((Globals.myGameConditions.get_achievement_group() - 1 + max_groups) % max_groups)


# === PREV BAR SCREEN ===

static func ai_prev_bar_screen(s: TSprite) -> void:
	if Globals.InputService.left_button_pressed() and AISupport.ais_mouse_over(s):
		Globals.myGameConditions.set_bar_group((Globals.myGameConditions.get_bar_group() - 1 + Globals.myGameConditions.NUM_BAR_GROUPS) % Globals.myGameConditions.NUM_BAR_GROUPS)

	if Globals.InputService.toggle_forward_button_pressed():
		Globals.myGameConditions.set_bar_group((Globals.myGameConditions.get_bar_group() + 1) % Globals.myGameConditions.NUM_BAR_GROUPS)
	if Globals.InputService.toggle_back_button_pressed():
		Globals.myGameConditions.set_bar_group((Globals.myGameConditions.get_bar_group() - 1 + Globals.myGameConditions.NUM_BAR_GROUPS) % Globals.myGameConditions.NUM_BAR_GROUPS)


static func ai_next_bar_screen(s: TSprite) -> void:
	if Globals.InputService.left_button_pressed() and AISupport.ais_mouse_over(s):
		Globals.myGameConditions.set_bar_group((Globals.myGameConditions.get_bar_group() + 1) % Globals.myGameConditions.NUM_BAR_GROUPS)


# === PICK JACKET BAR SPOT ===
# Automatically picks the best jacket spot for a bar (used for quick-click placement)

static func ais_pick_jacket_bar_spot(s: TSprite) -> bool:
	var n_distance: Array[int] = []
	n_distance.resize(NUM_JBAR_SPOTS)

	# Calculate distance from bar to each jacket spot
	for i in range(NUM_JBAR_SPOTS):
		n_distance[i] = absi(s.n_x - N_JSPOT_X[i]) + absi(s.n_y - N_JSPOT_Y[i])

	# Find closest empty spot
	var n_spot_to_use: int = 0
	for i in range(NUM_JBAR_SPOTS):
		if Globals.myGameConditions.get_j_bar(n_spot_to_use) != Globals.myGameConditions.NO_BAR:
			n_spot_to_use = i
		if n_distance[i] < n_distance[n_spot_to_use] and Globals.myGameConditions.get_j_bar(i) == Globals.myGameConditions.NO_BAR:
			n_spot_to_use = i

	# If closest spot is too far (>100), find any empty spot
	if n_distance[n_spot_to_use] > 100:
		for i in range(NUM_JBAR_SPOTS - 1, -1, -1):
			if Globals.myGameConditions.get_j_bar(i) == Globals.myGameConditions.NO_BAR:
				n_spot_to_use = i

	# If no empty spot available, return false
	if Globals.myGameConditions.get_j_bar(n_spot_to_use) != Globals.myGameConditions.NO_BAR:
		return false

	# Place bar on jacket
	s.n_attrib[Enums.AttrBar.ATTR_ON_JACKET_POSITION] = n_spot_to_use
	Globals.myGameConditions.set_j_bar(n_spot_to_use, s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_DISCIPLINE])
	AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_bar"), s.n_x, s.n_y, N_JSPOT_X[n_spot_to_use], N_JSPOT_Y[n_spot_to_use], 1, 1)

	return true


# === BAR (DISCIPLINE CREST) ===

static func ai_bar(s: TSprite) -> void:
	var b_my_bar_group_on_screen: bool = s.n_attrib[Enums.AttrBar.ATTR_BAR_GROUP] == Globals.myGameConditions.get_bar_group()
	var b_on_jacket: bool = s.n_attrib[Enums.AttrBar.ATTR_ON_JACKET_POSITION] != BAR_OFF_JACKET
	var b_x_on_screen: bool = s.n_x >= 0 and s.n_x <= 640

	# Achievement for placing bar on jacket
	if b_on_jacket:
		AISupport.ais_unlock_achievement(2002)

	if not s.b_attrib[Enums.BAttrBar.BATTR_BEING_DRAGGED]:
		# Not on jacket, wrong group, but still on screen -> fly out
		if not b_my_bar_group_on_screen and not b_on_jacket and b_x_on_screen:
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_bar"), s.n_x, s.n_y, 700, s.n_y, 1, 1)
		# Correct group, not on jacket, off screen -> fly in
		if b_my_bar_group_on_screen and not b_on_jacket and not b_x_on_screen:
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_bar"), s.n_x, s.n_y, s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X], s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y], 1, 1)
		# Click to pick up
		if Globals.InputService.left_button_pressed() and AISupport.ais_mouse_over(s):
			s.n_cc = 0
			s.b_attrib[Enums.BAttrBar.BATTR_BEING_DRAGGED] = true
			# If was on jacket, remove it
			if s.n_attrib[Enums.AttrBar.ATTR_ON_JACKET_POSITION] != BAR_OFF_JACKET:
				Globals.myGameConditions.set_j_bar(s.n_attrib[Enums.AttrBar.ATTR_ON_JACKET_POSITION], Globals.myGameConditions.NO_BAR)
				s.n_attrib[Enums.AttrBar.ATTR_ON_JACKET_POSITION] = BAR_OFF_JACKET
			# Play discipline sound
			if not AIMethods.l_sound[s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND]].is_playing():
				AIMethods.l_sound[s.n_attrib[Enums.AttrBar.ATTR_ASSOCIATED_SOUND]].play(SoundbankInfo.VOL_HOLLAR)
	else:
		# Being dragged - follow mouse
		s.n_x = Globals.InputService.get_mouse_x()
		s.n_y = Globals.InputService.get_mouse_y()

		if not Globals.InputService.left_button_down():
			s.b_attrib[Enums.BAttrBar.BATTR_BEING_DRAGGED] = false

			# C# logic: Quick click (nCC < 7) OR dragged far from original position -> try auto-place on jacket
			if s.n_cc < 7 or (s.n_cc >= 7 and absi(s.n_x - s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X]) > 160):
				if not ais_pick_jacket_bar_spot(s):
					# Couldn't auto-place, fly back
					if b_my_bar_group_on_screen:
						AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_bar"), s.n_x, s.n_y, s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X], s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y], 1, 1)
					else:
						AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_bar"), s.n_x, s.n_y, 700, s.n_y, 1, 1)
			else:
				# Dragged but not far enough - fly back to screen position
				if b_my_bar_group_on_screen:
					AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_bar"), s.n_x, s.n_y, s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_X], s.n_attrib[Enums.AttrBar.ATTR_ON_SCREEN_Y], 1, 1)
				else:
					AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMenuAndDisplay, "ai_bar"), s.n_x, s.n_y, 700, s.n_y, 1, 1)


# === JACKET SLAM TRANSITION ===

static func ai_slam_jacket(s: TSprite) -> void:
	match s.n_cc:
		1:
			# Play whoosh sound, position jacket below screen
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_BIGJACKETWHOOSH].play(SoundbankInfo.VOL_NORMAL)
			s.n_x = 320
			s.n_y = 480 + 150
		3:
			s.n_x = 320
			s.n_y = 480 + 100
		4:
			s.n_x = 320
			s.n_y = 480 + 50
		5:
			s.n_x = 320
			s.n_y = 480
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSLAMJACKET2])
		7:
			# Slam sound at full volume, show impact frame
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_BIGJACKETSLAM].play(SoundbankInfo.VOL_FULL)
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSLAMJACKET3])
		16:
			# STATE TRANSITION HAPPENS HERE
			var next_state: int = s.n_attrib[Enums.AttrJacketSlam.ATTR_NEXT_STATE]
			if next_state == Enums.GameStates.STATEEXIT:
				Globals.GameLoop.kill_game()
			else:
				Globals.GameLoop.set_game_state(next_state)
		20:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpSLAMJACKET4])
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_BIGJACKETWHOOSH].play(SoundbankInfo.VOL_FULL)
		24:
			s.b_deleted = true


# === DIGIT HELPER FUNCTIONS ===

static func tens_digit_frame(number: int) -> FrameDesc:
	if number < 0:
		return AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0]
	if number >= 100:
		number = 99
	return AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0 + (number / 10)]


static func ones_digit_frame(number: int) -> FrameDesc:
	if number < 0:
		return AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0]
	if number >= 100:
		number = 99
	return AIMethods.frm[Enums.GameBitmapEnumeration.bmpDIG_0 + (number % 10)]


# === PIT TIME DISPLAY ===

const BOOST_TIME_KEEN: int = 330
const BOOST_TIME_NORMAL: int = 660

static func ai_pit_time_s_tens(s: TSprite) -> void:
	# Also handles FPS display
	if AIMethods.gb_show_fps:
		AIMethods.spr_fps_0.set_frame(tens_digit_frame(Globals.RenderingService.get_fps()))
		AIMethods.spr_fps_1.set_frame(ones_digit_frame(Globals.RenderingService.get_fps()))
	else:
		AIMethods.spr_fps_0.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
		AIMethods.spr_fps_1.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpINVISIBLE])
	s.set_frame(tens_digit_frame(AIMethods.gn_pit_time_s))


static func ai_pit_time_s_ones(s: TSprite) -> void:
	s.set_frame(ones_digit_frame(AIMethods.gn_pit_time_s))


static func ai_pit_time_m_tens(s: TSprite) -> void:
	s.set_frame(tens_digit_frame(AIMethods.gn_pit_time_m))


static func ai_pit_time_m_ones(s: TSprite) -> void:
	s.set_frame(ones_digit_frame(AIMethods.gn_pit_time_m))


static func ai_pit_time_h_tens(s: TSprite) -> void:
	# Boost the performance of the frosh every ~1 minute
	var boost_time: int = BOOST_TIME_NORMAL if Globals.myGameConditions.get_frosh_lameness() == 0 else BOOST_TIME_KEEN
	if s.n_cc > 0 and (s.n_cc % boost_time) == 0:
		Globals.myGameConditions.performance_boost()

	s.set_frame(tens_digit_frame(AIMethods.gn_pit_time_h))

	# Achievement checks
	if AIMethods.gn_pit_time_h >= 10:
		AISupport.ais_unlock_achievement(5)
	if AIMethods.gn_pit_time_h >= 20:
		AISupport.ais_unlock_achievement(5)
	if AIMethods.gn_pit_time_h >= 5 and not AIProjectile.has_tossed_114_exam:
		AISupport.ais_unlock_achievement(13)


static func ai_pit_time_h_ones(s: TSprite) -> void:
	s.set_frame(ones_digit_frame(AIMethods.gn_pit_time_h))


# === RING ICON ===

static func ai_ring(s: TSprite) -> void:
	if AISupport.ais_mouse_over(s):
		s.n_cc += 2
		if Globals.InputService.left_button_pressed():
			AIMethods.l_sound[Enums.ASLList.LSND_RING_RISE].play(SoundbankInfo.VOL_HOLLAR, AICrowd.pan_on_x(s))
			AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_STATUS] = Enums.ArmPositions.ARM_CHANGING
			AIMethods.spr_arm.n_attrib[Enums.AttrArm.ATTR_ARM_ACTION] = Enums.ArmPositions.ARM_IRON_RING
			AIMethods.spr_arm.n_cc = 0
			AIFlyInAndOut.ai_init_fly_in_and_out_2(s, Callable(AIMisc, "ai_delete_me"), s.n_x, s.n_y, AIMethods.spr_arm.n_x, 540, 1, 1)
			Globals.myGameConditions.release_ring_spot(s.n_attrib[0])
			Globals.myGameConditions.lose_ring()

	match (s.n_cc / 3) % 5:
		0: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICORING1])
		1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICORING2])
		2: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICORING3])
		3: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICORING4])
		4: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpICORING5])
