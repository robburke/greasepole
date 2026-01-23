class_name PolePosition

# PolePosition.gd - Pole position chain system
# Ported from poleposn.cs
#
# This class manages the grid of positions where frosh can stand
# on the pyramid around the pole.

# === CONSTANTS ===
const MAX_PP_CHAINS: int = 20
const END_OF_CHAIN: float = 6000.0
const END_OF_DATA: float = 60000.0

# === STATIC DATA ===
static var pole_chains: Array = []  # Array of PolePosition
static var n_pp_chains: int = 0
static var N_PP_CHAINS: int:  # Alias for compatibility
	get: return n_pp_chains
	set(v): n_pp_chains = v

# Pre-defined pole position data (from C#)
static var n_pole_position_data: Array[float] = [
	0, 1,
	0, 2,
	0, 3,
	END_OF_CHAIN, END_OF_CHAIN,
	1, 1,
	1, 2,
	1, 3,
	END_OF_CHAIN, END_OF_CHAIN,
	1, 0,
	2, 1,
	2, 2,
	2, 3,
	END_OF_CHAIN, END_OF_CHAIN,
	2, 0,
	3, 0,
	3, 1,
	3, 2,
	4, 1,
	4, 0,
	END_OF_CHAIN, END_OF_CHAIN,
	2, -1,
	3, -1,
	4, -1,
	3, -2,
	2, -2,
	2, -3,
	END_OF_CHAIN, END_OF_CHAIN,
	1, -1,
	1, -2,
	1, -3,
	END_OF_CHAIN, END_OF_CHAIN,
	0, -1,
	0, -2,
	0, -3,
	END_OF_CHAIN, END_OF_CHAIN,
	-1, -1,
	-1, -2,
	-1, -3,
	END_OF_CHAIN, END_OF_CHAIN,
	-2, -1,
	-3, -1,
	-4, -1,
	-3, -2,
	-2, -2,
	-2, -3,
	END_OF_CHAIN, END_OF_CHAIN,
	-2, 0,
	-3, 0,
	-3, 1,
	-3, 2,
	-4, 1,
	-4, 0,
	END_OF_CHAIN, END_OF_CHAIN,
	-1, 0,
	-2, 1,
	-2, 2,
	-2, 3,
	END_OF_CHAIN, END_OF_CHAIN,
	-1, 1,
	-1, 2,
	-1, 3,
	END_OF_CHAIN, END_OF_DATA
]

# === INSTANCE VARIABLES ===
var f_offset_x: float
var f_offset_y: float
var n_pit_x: int
var n_pit_y: int
var child_position: PolePosition = null   # Position further from the pole
var parent_position: PolePosition = null  # Position closer to the pole
var adjacent_chain_first_position: PolePosition = null  # First position in adjacent chain
var spr_claimer: TSprite = null
var n_ordinal: int = 0  # How close are you to the pole itself?


# === CONSTRUCTOR ===
func _init(pole_position_x: float, pole_position_y: float, parent: PolePosition):
	f_offset_x = pole_position_x
	f_offset_y = pole_position_y
	parent_position = parent
	child_position = null
	calculate_screen_position()
	set_claim(null)


# === GETTERS ===
func get_x() -> int:
	return n_pit_x


func get_y() -> int:
	return n_pit_y


func get_claimer() -> TSprite:
	return spr_claimer


func get_parent() -> PolePosition:
	return parent_position


func get_child() -> PolePosition:
	return child_position


func adjacent_chain() -> PolePosition:
	return adjacent_chain_first_position


# === SETTERS ===
func set_adjacent_chain(pp_chain: PolePosition) -> void:
	adjacent_chain_first_position = pp_chain


func release_claim() -> void:
	spr_claimer = null


func set_claim(spr_new_claimer: TSprite) -> void:
	spr_claimer = spr_new_claimer


# === STATE QUERIES ===
func position_is_taken() -> bool:
	return not position_is_free()


func position_is_free() -> bool:
	return spr_claimer == null


func is_claimed() -> bool:
	return spr_claimer != null


# === CHAIN TRAVERSAL ===
func first_taken_child() -> PolePosition:
	var return_spot: PolePosition = self
	if return_spot.position_is_taken():
		return return_spot
	elif child_position == null:
		return null
	else:
		return child_position.first_taken_child()


func first_free_child() -> PolePosition:
	var return_spot: PolePosition = self
	if return_spot.position_is_free():
		return return_spot
	elif child_position == null:
		return null
	else:
		return child_position.first_free_child()


func last_taken_child() -> PolePosition:
	var return_spot: PolePosition = self
	if return_spot.position_is_free():
		return return_spot.parent_position
	elif child_position == null:
		return null
	else:
		return child_position.last_taken_child()


# === POSITION CALCULATION ===
func calculate_screen_position() -> void:
	n_pit_x = AIDefine.D_POLE_X + int(f_offset_x * AIDefine.D_FROSH_ARM_LINK_OFFSET_X)
	n_pit_y = AIDefine.D_POLE_Y + int(f_offset_y * AIDefine.D_FROSH_ARM_LINK_OFFSET_Y)


func create_child(offset_x: float, offset_y: float) -> PolePosition:
	child_position = PolePosition.new(offset_x, offset_y, self)
	return child_position


# === STATIC INITIALIZATION ===
static func initialize_pole_positions() -> bool:
	var i: int = 0
	var n_position_distance_from_pole: int
	n_pp_chains = 0
	var f_x: float = 0
	var f_y: float = 0
	var pp_current_chain: PolePosition

	# Initialize chains array
	pole_chains.resize(MAX_PP_CHAINS)

	while f_y != END_OF_DATA:
		f_x = n_pole_position_data[i]
		i += 1
		f_y = n_pole_position_data[i]
		i += 1
		pp_current_chain = PolePosition.new(f_x, f_y, null)  # Create a first-level entry
		pole_chains[n_pp_chains] = pp_current_chain

		n_position_distance_from_pole = 0
		pp_current_chain.n_ordinal = n_position_distance_from_pole

		while f_x != END_OF_CHAIN:
			f_x = n_pole_position_data[i]
			i += 1
			f_y = n_pole_position_data[i]
			i += 1
			if f_x != END_OF_CHAIN:
				pp_current_chain = pp_current_chain.create_child(f_x, f_y)
				n_position_distance_from_pole += 1
				pp_current_chain.n_ordinal = n_position_distance_from_pole

		n_pp_chains += 1

	# Set up adjacent chain references
	for j in range(n_pp_chains):
		pp_current_chain = pole_chains[j]
		while pp_current_chain != null:
			pp_current_chain.set_adjacent_chain(pole_chains[(j + 1) % n_pp_chains])
			pp_current_chain = pp_current_chain.get_child()

	return true
