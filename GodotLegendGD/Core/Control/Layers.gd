class_name Layers

# Layers.gd - Parallax layer system for scrolling backgrounds
# Ported from Layers.cs

# === LAYER CLASS ===
class Layer:
	var _n_start_offset: int
	var _f_factor: float
	var _n_y: int = 0

	func _init(n_start_offset: int, f_factor: float) -> void:
		_n_start_offset = n_start_offset
		_f_factor = f_factor

	func set_scroll_distance(n_distance: int) -> void:
		_n_y = n_distance

	func get_y() -> int:
		return _n_start_offset + int(float(_n_y) * _f_factor)


# === LAYER NAMES ENUM ===
# Note: Also defined in Enums.gd but kept here for local reference
enum LayerNames {
	LAYER_SKY, LAYER_TREE, LAYER_SKYLINE, LAYER_FREC, LAYER_PIT, LAYER_MISC
}


# === CONSTANTS ===
const MAX_SCROLL_VELOCITY: int = 4
const D_MAX_GAME_SCROLL_DISTANCE: int = 85
const D_GAME_START_SCROLL_DISTANCE: int = 85
const D_GAME_START_SCROLL_VELOCITY: int = -10
const D_SCROLL_ACCELERATION: int = 1
const NUM_LAYERS: int = 6


# === STATE ===
var _n_scroll_distance: int = 0
var _n_scroll_velocity: int = 0
var _b_force_scroll_to_bottom: bool = false
var _my_layer: Array[Layer] = []


func _init() -> void:
	_n_scroll_distance = 0
	_n_scroll_velocity = 0
	_b_force_scroll_to_bottom = false

	# Initialize layers array
	_my_layer.resize(NUM_LAYERS)

	_my_layer[LayerNames.LAYER_SKY] = Layer.new(0, 3.2)       # Upper-left corner of screen
	_my_layer[LayerNames.LAYER_TREE] = Layer.new(166, 6.1)   # Base-of-trees-level
	_my_layer[LayerNames.LAYER_SKYLINE] = Layer.new(251, 5.6) # Base-of-skyline-level
	_my_layer[LayerNames.LAYER_FREC] = Layer.new(260, 4.9)   # Frecs
	_my_layer[LayerNames.LAYER_PIT] = Layer.new(275, 4.3)    # Water level
	_my_layer[LayerNames.LAYER_MISC] = Layer.new(0, 0.0)     # Screen (typ. 0)


func get_offset(n_layer_name: int) -> int:
	return _my_layer[n_layer_name].get_y()


func set_scroll_distance(n_distance: int) -> void:
	for i in range(NUM_LAYERS):
		_my_layer[i].set_scroll_distance(n_distance)


func force_scroll(n_distance: int) -> void:
	_n_scroll_distance += n_distance

	if _n_scroll_distance <= 0:
		_n_scroll_distance = 0
	elif _n_scroll_distance >= D_MAX_GAME_SCROLL_DISTANCE:
		_n_scroll_distance = D_MAX_GAME_SCROLL_DISTANCE


func scroll_screen() -> void:
	if (not Globals.myGameConditions.is_demo()) and (not _b_force_scroll_to_bottom):
		if Globals.InputService.get_mouse_y() < 40:
			_n_scroll_velocity += D_SCROLL_ACCELERATION
		elif Globals.InputService.get_mouse_y() > 440:
			_n_scroll_velocity -= D_SCROLL_ACCELERATION
		else:
			if _n_scroll_velocity > 0:
				_n_scroll_velocity -= 1
			if _n_scroll_velocity < 0 and not _b_force_scroll_to_bottom:
				_n_scroll_velocity += 1

	if _n_scroll_distance < 30 and _n_scroll_velocity < -2:
		_n_scroll_velocity += 1
	elif _n_scroll_distance > 50 and _n_scroll_velocity > 2:
		_n_scroll_velocity -= 1

	if _b_force_scroll_to_bottom:
		if _n_scroll_distance == 0:
			_b_force_scroll_to_bottom = false
		else:
			_n_scroll_velocity = -(_n_scroll_distance / 16) - 1

	if _n_scroll_velocity > MAX_SCROLL_VELOCITY:
		_n_scroll_velocity = MAX_SCROLL_VELOCITY
	if _n_scroll_velocity < -MAX_SCROLL_VELOCITY:
		_n_scroll_velocity = -MAX_SCROLL_VELOCITY

	_n_scroll_distance += _n_scroll_velocity

	# Don't go over the boundaries
	if _n_scroll_distance <= 0 and _n_scroll_velocity < 0:
		_n_scroll_distance = 0
		_n_scroll_velocity = 0
	elif _n_scroll_distance >= D_MAX_GAME_SCROLL_DISTANCE and _n_scroll_velocity > 0:
		_n_scroll_distance = D_MAX_GAME_SCROLL_DISTANCE
		_n_scroll_velocity = 0

	Globals.myLayers.set_scroll_distance(_n_scroll_distance)


func get_layer(n_layer: int) -> Layer:
	return _my_layer[n_layer]


func reset_to(n_d: int, n_v: int, b_force: bool) -> void:
	# Note: Matching C# behavior - only set layers, not tracking variable
	# This means nScrollDistance stays at 0/previous value, and scroll_screen()
	# will immediately jump to that position on first call
	set_scroll_distance(n_d)  # Propagate to all layers
	_n_scroll_velocity = n_v
	_b_force_scroll_to_bottom = b_force


func reset_for_game() -> void:
	reset_to(D_GAME_START_SCROLL_DISTANCE, D_GAME_START_SCROLL_VELOCITY, true)


func reset_for_menu() -> void:
	reset_to(0, 0, false)
