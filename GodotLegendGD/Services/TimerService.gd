extends Node

# TimerService.gd - Game timing and frame rate control
# Ported from GodotTimerService.cs

# Original game ran at 25 FPS (see AIDefine.cs)
const TARGET_FRAME_TIME_MS: float = 1000.0 / 25.0

var _accumulated_time: float = 0.0
var _delta_ms: float = 0.0
var _is_paused: bool = false
var _game_time_score_ms: float = 0.0


func _ready():
	Globals.TimerService = self
	print("[TimerService] Initialized - Target frame time: %.2f ms (25 FPS)" % TARGET_FRAME_TIME_MS)


func set_delta(delta_sec: float) -> void:
	"""Call this from Godot's _process with the delta time to update the timer."""
	_delta_ms = delta_sec * 1000.0

	if not _is_paused:
		_game_time_score_ms += _delta_ms


func update() -> void:
	if not _is_paused:
		_accumulated_time += _delta_ms


func get_additional_update_count() -> int:
	"""Calculate how many game frames should run based on accumulated time."""
	var additional_updates: int = 0

	while _accumulated_time >= TARGET_FRAME_TIME_MS:
		_accumulated_time -= TARGET_FRAME_TIME_MS
		additional_updates += 1

	# Cap to prevent spiral of death
	if additional_updates > 4:
		additional_updates = 4

	return additional_updates


func pause_update_count_timer() -> void:
	_is_paused = true


func resume_update_count_timer() -> void:
	_is_paused = false
	_accumulated_time = 0.0


func reset_game_time_score() -> void:
	_game_time_score_ms = 0.0


func get_current_game_time_score_milliseconds() -> float:
	return _game_time_score_ms


func get_current_game_time_score_seconds() -> float:
	return _game_time_score_ms / 1000.0
