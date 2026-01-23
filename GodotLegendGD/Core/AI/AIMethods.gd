extends Node

# AIMethods.gd - AUTOLOAD
# This file holds ALL shared state from the C# partial class AIMethods.
# Logic files access state via: AIMethods.variable_name
#
# Per porting_rules.md:
# - This is registered as an Autoload in project.godot
# - All other ai*.gd files are static classes with logic only

# === RANDOM NUMBER GENERATOR ===
var R: RandomNumberGenerator = RandomNumberGenerator.new()

# === GLOBAL FLAGS ===
var gb_show_fps: bool = false
var gb_start_at_game: bool = false
var NO_SOUND: bool = false

# === PIT TIME ===
var gn_pit_time_s: int = 0  # Seconds
var gn_pit_time_m: int = 0  # Minutes
var gn_pit_time_h: int = 0  # Hours

# === FROSH LEVEL TRACKING ===
var n_frosh_level_l: Array[int] = [0, 0, 0, 0, 0, 0, 0]
var n_frosh_level_r: Array[int] = [0, 0, 0, 0, 0, 0, 0]
var n_frosh_level: Array[int] = [0, 0, 0, 0, 0, 0, 0]
var n_frosh_target: Array[int] = [0, 40, 15, 7, 5, 3, 1]  # [unused, base, level2, level3, level4, level5, tam]
var n_frosh_tam: int = 0
var n_frosh_thinking: int = 0
var n_frosh_bitter: int = 0
var n_frosh_above_1: int = 0
var n_frosh_above_3: int = 0
var n_frosh_total_l: int = 0
var n_frosh_total_r: int = 0

# === SPRITE SETS ===
# Pointers to the SpriteSets used in the game
var ss_clouds: SpriteSet = null
var ss_balloon: SpriteSet = null
var ss_skyline: SpriteSet = null
var ss_trees: SpriteSet = null
var ss_frecs: SpriteSet = null
var ss_water: SpriteSet = null
var ss_pit: SpriteSet = null       # The pole, frosh, water splashing, etc.
var ss_tossed: SpriteSet = null    # Tossed items
var ss_console: SpriteSet = null
var ss_icons: SpriteSet = null     # Icons and the hand
var ss_mouse: SpriteSet = null     # The mouse cursor
var ss_jacket_slam: SpriteSet = null  # The jacket ka-blam
var ss_fr: SpriteSet = null        # Special spriteset containing ALL and ONLY the Frosh (ordered by X)

# === KEY SPRITE REFERENCES ===
# Pointers to specific sprites so other sprites can change their behavior
var spr_arm: TSprite = null
var spr_tam: TSprite = null
var spr_pole: TSprite = null
var spr_power_meter: TSprite = null
var spr_ring_meter: TSprite = null
var spr_water_meter: TSprite = null
var spr_frecs_l: TSprite = null
var spr_frecs_c: TSprite = null
var spr_frecs_r: TSprite = null
var spr_alien: TSprite = null
var spr_prez: TSprite = null
var spr_pop_boy: TSprite = null
var spr_forge: TSprite = null
var spr_random_event_generator: TSprite = null
var spr_gw_balloon: TSprite = null
var spr_gw_hippo: TSprite = null
var spr_fps_1: TSprite = null
var spr_fps_0: TSprite = null

# === FRAME ARRAYS ===
# Pointers to frames containing game graphics
# NOTE: These are also in Globals.frames / Globals.frames_mirror
# Keeping reference here for direct access pattern matching C#
var frm: Array = []   # Normal frames - initialized to size bmpENDOFBITMAPS
var frm_m: Array = [] # Mirrored frames

# === SOUND ARRAYS ===
const MAX_SSOUNDS: int = 500
const MAX_LSOUNDS: int = 500
var s_sound: Array = []  # Static sounds
var l_sound: Array = []  # Long/streamed sounds

# === INTERNAL SPRITE SETS FOR RANGE QUERIES ===
var _targets_in_scr_range: SpriteSet = null
var _sprites_in_range: SpriteSet = null

# === INITIALIZATION ===
func _ready():
	R.randomize()

	# Point frame arrays to Globals.frames (populated by RenderingService)
	# Globals is loaded before AIMethods in project.godot autoloads, so it's ready
	frm = Globals.frames
	frm_m = Globals.frames_mirror

	# Initialize sound arrays
	s_sound.resize(MAX_SSOUNDS)
	l_sound.resize(MAX_LSOUNDS)

	# Initialize sprite sets with their layer indices for dynamic parallax scrolling
	# Layer indices: LAYER_SKY=0, LAYER_TREE=1, LAYER_SKYLINE=2, LAYER_FREC=3, LAYER_PIT=4, LAYER_MISC=5
	ss_clouds = SpriteSet.new(Enums.LayerNames.LAYER_SKY)
	ss_balloon = SpriteSet.new(Enums.LayerNames.LAYER_SKY)
	ss_skyline = SpriteSet.new(Enums.LayerNames.LAYER_SKYLINE)
	ss_trees = SpriteSet.new(Enums.LayerNames.LAYER_TREE)
	ss_frecs = SpriteSet.new(Enums.LayerNames.LAYER_FREC)
	ss_water = SpriteSet.new(Enums.LayerNames.LAYER_PIT)
	ss_pit = SpriteSet.new(Enums.LayerNames.LAYER_PIT)
	ss_tossed = SpriteSet.new(Enums.LayerNames.LAYER_PIT)
	ss_console = SpriteSet.new(Enums.LayerNames.LAYER_MISC)
	ss_icons = SpriteSet.new(Enums.LayerNames.LAYER_MISC)
	ss_mouse = SpriteSet.new(Enums.LayerNames.LAYER_MISC)
	ss_jacket_slam = SpriteSet.new(Enums.LayerNames.LAYER_MISC)
	ss_fr = SpriteSet.new(Enums.LayerNames.LAYER_PIT)

# === UTILITY FUNCTIONS ===

# Returns a random integer in range lowest..highest (inclusive)
# Equivalent to C#: R.Next(lowest, highest + 1)
static func randintin(lowest: int, highest: int) -> int:
	return randi_range(lowest, highest)

# === SPEECH CONTROL ===
const TIME_BETWEEN_EVENTS: int = 100

func no_speech_for(no_speech_time: int) -> void:
	if spr_random_event_generator == null:
		return
	if spr_random_event_generator.n_cc > TIME_BETWEEN_EVENTS:
		spr_random_event_generator.n_cc = TIME_BETWEEN_EVENTS
	spr_random_event_generator.n_cc -= no_speech_time

func speech_ok() -> bool:
	if spr_random_event_generator == null:
		return false
	return spr_random_event_generator.n_cc > TIME_BETWEEN_EVENTS

# Uppercase aliases matching C# naming convention
func NOSPEECHFOR(no_speech_time: int) -> void:
	no_speech_for(no_speech_time)

func SPEECHOK() -> bool:
	return speech_ok()

# === LAYER CONVERSION FUNCTIONS ===
# These convert between different layer coordinate systems

func d_sky_y_to_pit_y() -> int:
	return Globals.myLayers.get_offset(Enums.LayerNames.LAYER_SKY) - Globals.myLayers.get_offset(Enums.LayerNames.LAYER_PIT)

func d_skyline_y_to_pit_y() -> int:
	return Globals.myLayers.get_offset(Enums.LayerNames.LAYER_SKYLINE) - Globals.myLayers.get_offset(Enums.LayerNames.LAYER_PIT)

func d_misc_y_to_pit_y() -> int:
	return Globals.myLayers.get_offset(Enums.LayerNames.LAYER_MISC) - Globals.myLayers.get_offset(Enums.LayerNames.LAYER_PIT)

func d_pit_y_to_misc_y() -> int:
	return Globals.myLayers.get_offset(Enums.LayerNames.LAYER_PIT) - Globals.myLayers.get_offset(Enums.LayerNames.LAYER_MISC)

# === PAN CALCULATION ===
# Calculate stereo pan position based on sprite X coordinate
static func pan_on_x(s: TSprite) -> float:
	# Convert X position (0-640) to pan value (-1.0 to 1.0)
	return (s.n_x - 320.0) / 320.0
