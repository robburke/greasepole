class_name GameConditions

# GameConditions.gd - Game state and conditions manager
# Ported from GameConditions.cs

const NUM_BAR_GROUPS: int = 2
const NO_BAR: int = -1

var gb_tri_pub_ban: bool = false

var _n_noise_count: int = 0
var _n_topples: int = 0
var _n_booster: Array[int] = []
var _n_booster_clock: int = 0
var _n_high_score: Array[int] = [0, 0]
var _b_pop_boy_in_pit: bool = false
var _b_ritual: bool = false
var _n_apples: int = 0
var _n_pizza: int = 0
var _n_clark: int = 0
var _n_exam: int = 0
var _n_hose: int = 0
var _n_ring: int = 0
var _b_ring_spot: Array[bool] = []
var _b_special_ring_trick: Array[bool] = []
var _b_is_demo: bool = false
var _b_game_over: bool = false

var _n_achievement_group: int = 0
var _n_bar_group: int = 0
var _n_j_bar: Array[int] = []
var _n_frosh_lameness: int = 0
var _n_sound: int = 0
var _n_enhanced_graphics: int = 0
var _n_rmb_function: int = 0
var _n_delta_energy: int = 0
var _n_prez_hit: Array[int] = [0, 0, 0, 0, 0]  # Apple, Grease, Clark, Pizza, Exam

# Performance boost arrays - index values from C#
var n_start_value: Array[int] = [1, 30, 80, 0, 0, 0, 6, 0]
var n_increment: Array[int] = [1, 30, -10, 1, 1, 1, -1, 1]
var n_final_value: Array[int] = [20, 1500, 50, 1, 1, 1, 2, 1]
var n_start_time: Array[int] = [0, 0, 0, 3, 4, 5, 5, 7]


func _init() -> void:
	# Initialize arrays
	_n_booster.resize(AIDefine.NUM_PERFORMANCE_BOOSTS)
	_b_ring_spot.resize(AIDefine.NUM_RING_SPOTS)
	_b_special_ring_trick.resize(AIDefine.NUM_TRICKS)
	_n_j_bar.resize(AIDefine.NUM_JBAR_SPOTS)

	_n_bar_group = 0

	for i in range(AIDefine.NUM_JBAR_SPOTS):
		_n_j_bar[i] = AIDefine.NO_BAR

	reset(false)
	_n_frosh_lameness = 0
	_n_sound = 1
	_n_enhanced_graphics = 0
	_n_rmb_function = 0
	_b_ritual = false


# === TOPPLE TRACKING ===
func count_topples() -> int:
	return _n_topples

func topple() -> void:
	_n_topples += 1

func is_game_over() -> bool:
	return _b_game_over

func is_demo() -> bool:
	return _b_is_demo

func game_over() -> void:
	_b_game_over = true


# === RING TRICKS ===
func use_trick(n_index: int) -> bool:
	if _b_special_ring_trick[n_index]:
		return false
	else:
		_b_special_ring_trick[n_index] = true
		return true

func release_trick(n_index: int) -> void:
	_b_special_ring_trick[n_index] = false

func is_trick_used(n_index: int) -> bool:
	return _b_special_ring_trick[n_index]


# === RING SPOTS ===
func take_ring_spot(n_index: int) -> void:
	_b_ring_spot[n_index] = true

func release_ring_spot(n_index: int) -> void:
	_b_ring_spot[n_index] = false

func is_ring_spot_open(n_index: int) -> bool:
	return not _b_ring_spot[n_index]


# === BOOSTERS ===
func get_booster(n_index: int) -> int:
	return _n_booster[n_index]


# === NOISE COUNT ===
func get_noise_count() -> int:
	return _n_noise_count

func set_noise_count(n_new_count: int) -> void:
	_n_noise_count = n_new_count


# === ENERGY ===
func add_energy(n_more: int) -> void:
	_n_delta_energy += n_more

func get_energy() -> int:
	return _n_delta_energy

func reset_energy() -> void:
	_n_delta_energy = 0


# === INVENTORY MANAGEMENT ===
func lose_apple() -> void:
	_n_apples -= 1
	if _n_apples < 0:
		_n_apples = 0

func lose_clark() -> void:
	_n_clark -= 1

func lose_pizza() -> void:
	_n_pizza -= 1
	if _n_pizza < 0:
		_n_pizza = 0

func lose_exam() -> void:
	_n_exam -= 1

func lose_hose() -> void:
	_n_hose -= 1

func lose_ring() -> void:
	_n_ring = 0

func get_apples(n_more: int) -> void:
	_n_apples += n_more
	if _n_apples > 99:
		_n_apples = 99
	if _n_apples < 0:
		_n_apples = 0

func get_clarks(n_more: int) -> void:
	_n_clark += n_more
	if _n_clark > 99:
		_n_clark = 99
	if _n_clark < 0:
		_n_clark = 0

func get_pizzas(n_more: int) -> void:
	_n_pizza += n_more
	if _n_pizza > 99:
		_n_pizza = 99
	if _n_pizza < 0:
		_n_pizza = 0

func get_exams(n_more: int) -> void:
	_n_exam += n_more
	if _n_exam > 14:
		_n_exam = 14
	if _n_exam < 0:
		_n_exam = 0

func get_hose(n_more: int) -> void:
	_n_hose += n_more

func get_ring(n_more: int) -> void:
	_n_ring += n_more


# === BAR AND ACHIEVEMENT GROUP ===
func set_bar_group(n_new_bar_group: int) -> void:
	_n_bar_group = n_new_bar_group

func set_achievement_group(n_new_achievement_group: int) -> void:
	_n_achievement_group = n_new_achievement_group


# === POP BOY ===
func is_pop_boy_in_pit() -> bool:
	return _b_pop_boy_in_pit

func pop_boy_jumps_in() -> void:
	_b_pop_boy_in_pit = true


# === PREZ HIT ===
func get_prez_hit(n_weapon: int) -> int:
	return _n_prez_hit[n_weapon]

func add_prez_hit(n_weapon: int) -> void:
	_n_prez_hit[n_weapon] += 1

func reset_prez_hit(n_weapon: int) -> void:
	_n_prez_hit[n_weapon] = 0


# === FROSH LAMENESS / OPTIONS ===
func set_frosh_lameness(n_new: int) -> void:
	_n_frosh_lameness = n_new

func get_frosh_lameness() -> int:
	return _n_frosh_lameness

func set_sound(n_new: int) -> void:
	_n_sound = n_new

func get_sound() -> int:
	return _n_sound

func set_enhanced_graphics(n_new: int) -> void:
	_n_enhanced_graphics = n_new

func get_enhanced_graphics() -> int:
	return _n_enhanced_graphics

func set_rmb_function(n_new: int) -> void:
	_n_rmb_function = n_new

func get_rmb_function() -> int:
	return _n_rmb_function


# === JACKET BAR ===
func get_j_bar(n_bar_spot: int) -> int:
	return _n_j_bar[n_bar_spot]

func set_j_bar(n_bar_spot: int, n_new_bar: int) -> void:
	_n_j_bar[n_bar_spot] = n_new_bar
	_b_ritual = _n_j_bar[0] == 19 or _n_j_bar[1] == 19 \
		or _n_j_bar[2] == 19 or _n_j_bar[3] == 19


# === RITUAL ===
func is_ritual() -> bool:
	return _b_ritual


# === PLAYER INVENTORY GETTERS ===
func get_player_apples() -> int:
	return _n_apples

func get_player_clark() -> int:
	return _n_clark

func get_player_pizza() -> int:
	return _n_pizza

func get_player_exam() -> int:
	return _n_exam

func get_player_hose() -> int:
	return _n_hose

func get_player_ring() -> int:
	return _n_ring

func get_bar_group() -> int:
	return _n_bar_group

func get_achievement_group() -> int:
	return _n_achievement_group


# === HIGH SCORE ===
func set_high_score(n_index: int, n_score: int) -> void:
	_n_high_score[n_index] = n_score

func get_high_score(n_index: int) -> int:
	return _n_high_score[n_index]


# === SETTINGS PERSISTENCE ===
func save_settings_to_storage() -> void:
	var settings: Dictionary = {}
	settings["JacketBar1"] = _n_j_bar[0]
	settings["JacketBar2"] = _n_j_bar[1]
	settings["JacketBar3"] = _n_j_bar[2]
	settings["JacketBar4"] = _n_j_bar[3]

	# Store the Option Button conditions
	settings["OptionButton0"] = _n_frosh_lameness
	settings["OptionButton1"] = _n_sound
	settings["OptionButton2"] = _n_enhanced_graphics
	settings["OptionButton3"] = _n_rmb_function

	# Store the high scores
	settings["HighScore0"] = _n_high_score[0]
	settings["HighScore1"] = _n_high_score[1]

	# Store achievements
	for i in range(PoleGameAchievement.list.size()):
		var achievement: PoleGameAchievement = PoleGameAchievement.list[i]
		settings["A" + str(achievement.achievement_guid)] = achievement.achieved_code if achievement.achieved else 0

	if Globals.SettingsService != null:
		Globals.SettingsService.save_settings(settings)


func _snag_setting(settings: Dictionary, key: String, default_value: int) -> int:
	if settings.has(key):
		return settings[key]
	return default_value


func load_settings_from_storage() -> void:
	var settings: Dictionary = {}
	if Globals.SettingsService != null:
		settings = Globals.SettingsService.load_settings()

	set_j_bar(0, _snag_setting(settings, "JacketBar1", -1))
	set_j_bar(1, _snag_setting(settings, "JacketBar2", -1))
	set_j_bar(2, _snag_setting(settings, "JacketBar3", -1))
	set_j_bar(3, _snag_setting(settings, "JacketBar4", -1))
	set_high_score(0, _snag_setting(settings, "HighScore0", 0))
	set_high_score(1, _snag_setting(settings, "HighScore1", 0))
	_n_frosh_lameness = _snag_setting(settings, "OptionButton0", 0)
	_n_sound = _snag_setting(settings, "OptionButton1", 1)
	_n_enhanced_graphics = _snag_setting(settings, "OptionButton2", 1)
	_n_rmb_function = _snag_setting(settings, "OptionButton3", 1)

	for i in range(PoleGameAchievement.list.size()):
		var a: PoleGameAchievement = PoleGameAchievement.list[i]
		a.achieved = _snag_setting(settings, "A" + str(a.achievement_guid), 0) == a.achieved_code


# === RESET ===
func reset(b_is_this_a_demo: bool) -> void:
	_b_game_over = false
	_b_is_demo = b_is_this_a_demo

	_n_topples = 0
	_n_noise_count = 800

	for j in range(AIDefine.NUM_RING_SPOTS):
		_b_ring_spot[j] = false

	reset_energy()

	_n_booster_clock = -1
	for i in range(AIDefine.NUM_PERFORMANCE_BOOSTS):
		_n_booster[i] = n_start_value[i]

	for k in range(AIDefine.NUM_TRICKS):
		_b_special_ring_trick[k] = false

	_b_pop_boy_in_pit = false
	_n_prez_hit[0] = 0
	_n_prez_hit[1] = 0
	_n_prez_hit[2] = 0
	_n_prez_hit[3] = 0
	_n_prez_hit[4] = 0

	if _n_frosh_lameness == 1:
		_n_apples = 0
		_n_pizza = 5
		_n_clark = 0
		_n_exam = 0
		_n_hose = 0
		_n_ring = 0
	else:
		_n_apples = 5
		_n_pizza = 10
		_n_clark = 10
		_n_exam = 1
		_n_hose = 0
		_n_ring = 0


# === PERFORMANCE BOOST ===
func performance_boost() -> void:
	_n_booster_clock += 1

	for i in range(8):
		if n_start_time[i] <= _n_booster_clock and _n_booster[i] != n_final_value[i]:
			_n_booster[i] += n_increment[i]
