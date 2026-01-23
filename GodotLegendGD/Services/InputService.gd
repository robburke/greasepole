extends Node



var _mouse_position: Vector2
var _left_down: bool
var _right_down: bool
var _back_down: bool
var _start_down: bool

# Click flags - set by event handlers, cleared after AI processes
var _left_click_pending: bool
var _right_click_pending: bool
var _back_pending: bool
var _start_pending: bool

# Previous frame's down state for edge detection
var _prev_left_down: bool
var _prev_right_down: bool
var _prev_back_down: bool
var _prev_start_down: bool

# Cheat key edge detection - track previous state for single-fire
var _last_cheat_key: Enums.GreasepoleKeys = Enums.GreasepoleKeys.None

func _ready():
	Globals.InputService = self
	# Hide system cursor - game uses custom in-game cursor sprites
	Input.set_mouse_mode(Input.MOUSE_MODE_HIDDEN)
	print("[InputService] Initialized")

func get_mouse_x() -> int:
	return int(_mouse_position.x)

func get_mouse_y() -> int:
	return int(_mouse_position.y)

func left_button_pressed() -> bool:
	return _left_click_pending

func left_button_down() -> bool:
	return _left_down

func right_button_pressed() -> bool:
	return _right_click_pending

func right_button_down() -> bool:
	return _right_down

func back_button_pressed() -> bool:
	return _back_pending

func back_button_down() -> bool:
	return _back_down

func consume_back_input():
	_back_pending = false

func start_button_pressed() -> bool:
	return _start_pending

func start_button_down() -> bool:
	return _start_down

func toggle_forward_button_pressed() -> bool:
	return Input.is_action_just_pressed("ui_right")

func toggle_forward_button_down() -> bool:
	return Input.is_action_pressed("ui_right")

func toggle_back_button_pressed() -> bool:
	return Input.is_action_just_pressed("ui_left")

func toggle_back_button_down() -> bool:
	return Input.is_action_pressed("ui_left")

func _process(delta):
	# Mouse position
	_mouse_position = get_viewport().get_mouse_position()
	
	# Get current down states via polling
	var current_left_down = Input.is_mouse_button_pressed(MOUSE_BUTTON_LEFT)
	var current_right_down = Input.is_mouse_button_pressed(MOUSE_BUTTON_RIGHT)
	var current_back_down = Input.is_action_pressed("ui_back") or Input.is_key_pressed(KEY_ESCAPE)
	var current_start_down = Input.is_action_pressed("ui_accept") or Input.is_key_pressed(KEY_ENTER)
	
	# Detect rising edge via polling as backup
	if current_left_down and not _prev_left_down:
		_left_click_pending = true
	if current_right_down and not _prev_right_down:
		_right_click_pending = true
	if current_back_down and not _prev_back_down:
		_back_pending = true
	if current_start_down and not _prev_start_down:
		_start_pending = true
		
	# Store current down states
	_left_down = current_left_down
	_right_down = current_right_down
	_back_down = current_back_down
	_start_down = current_start_down
	
	# Remember for next frame's edge detection
	_prev_left_down = current_left_down
	_prev_right_down = current_right_down
	_prev_back_down = current_back_down
	_prev_start_down = current_start_down

func _input(event):
	# Capture clicks from events
	if event is InputEventMouseButton:
		if event.button_index == MOUSE_BUTTON_LEFT and event.pressed:
			_left_click_pending = true
		elif event.button_index == MOUSE_BUTTON_RIGHT and event.pressed:
			_right_click_pending = true

func on_ai_frame_complete():
	_left_click_pending = false
	_right_click_pending = false
	_back_pending = false
	_start_pending = false

func get_keyboard_input() -> Enums.GreasepoleKeys:
	if back_button_pressed():
		return Enums.GreasepoleKeys.Back

	# Detect which cheat key is currently pressed
	var current_key: Enums.GreasepoleKeys = Enums.GreasepoleKeys.None
	if Input.is_key_pressed(KEY_M):
		current_key = Enums.GreasepoleKeys.IncreaseMunitions
	elif Input.is_key_pressed(KEY_F):
		current_key = Enums.GreasepoleKeys.ShowFPS
	elif Input.is_key_pressed(KEY_R):
		current_key = Enums.GreasepoleKeys.BuildRingEnergy
	elif Input.is_key_pressed(KEY_B):
		current_key = Enums.GreasepoleKeys.GWBalloon
	elif Input.is_key_pressed(KEY_A):
		current_key = Enums.GreasepoleKeys.PopupArtsciorCommie
	elif Input.is_key_pressed(KEY_S):
		current_key = Enums.GreasepoleKeys.PopupScicon
	elif Input.is_key_pressed(KEY_O):
		current_key = Enums.GreasepoleKeys.PopupHose
	elif Input.is_key_pressed(KEY_J):
		current_key = Enums.GreasepoleKeys.Mosh
	elif Input.is_key_pressed(KEY_D):
		current_key = Enums.GreasepoleKeys.StartDemo
	elif Input.is_key_pressed(KEY_C):
		current_key = Enums.GreasepoleKeys.ClarkC
	elif Input.is_key_pressed(KEY_H):
		current_key = Enums.GreasepoleKeys.ClarkH
	elif Input.is_key_pressed(KEY_P):
		current_key = Enums.GreasepoleKeys.ClarkP

	# Edge detection: only return key on initial press, not while held
	var result: Enums.GreasepoleKeys = Enums.GreasepoleKeys.None
	if current_key != Enums.GreasepoleKeys.None and current_key != _last_cheat_key:
		result = current_key
	_last_cheat_key = current_key

	return result
