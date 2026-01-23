class_name AIDefine

# AIDefine.gd - Static class with all game constants
# Ported from AIDefine.cs
# These are the magic numbers - DO NOT CHANGE without testing!

# === FROSH COUNTS ===
const GN_NUM_FROSH_IN_PIT: int = 85
const GN_NUM_START_HEAVYWEIGHT: int = 55
const GN_NUM_START_CLIMBER: int = 28
const GN_NUM_START_HOISTER: int = 2
const GN_DEFAULT_NAILS_IN_TAM: int = 5

# === DEFAULT STATS ===
const GN_DEFAULT_MORALE: int = 1
const GN_DEFAULT_STRENGTH: int = 7
const GN_DEFAULT_INTELLIGENCE: int = 7

# === FRAME LIMITS ===
const MAX_FRAMES: int = 750  # Maximum number of bitmaps used by the game

# === PIT COORDINATES ===
# Pit's coordinates relative to the Pit layer
const D_PIT_MIN_X: int = 0
const D_PIT_MIN_X_MINUS_50: int = -50
const D_PIT_MAX_X: int = 640
const D_PIT_MAX_X_PLUS_50: int = 690
const D_PIT_MIN_Y: int = 30
const D_PIT_MAX_Y: int = 200
const D_PIT_MAX_Y_PLUS_30: int = 230
const D_POLE_WIDTH: int = 20
const D_POLE_HEIGHT: int = 640

# === FROSH PROPERTIES ===
const N_FROSH_PERSONALITIES: int = 4
const N_PIZZA_MUNCH_AVERAGE: int = 15

# === DISTANCE CONSTANTS (in pixels) ===
const D_ONE: int = 1
const D_TWO: int = 2
const D_THREE: int = 3
const D_SIX: int = 6
const D_TEN: int = 10
const D_GRAV_CONST: int = 2         # Gravitational constant
const D_SPEED_FOR_BIG_SPLASH: int = 8
const D_BELLY_BUTTON_Z: int = 15    # Height of bellybutton above water in Z
const D_SPLASHING_DISTANCE: int = 100   # Distance you should be away to splash
const D_UPPER_LEVEL_DISTANCE_FROM_POLE: int = 15  # Distance a frosh stands from the pole
const D_FROSH_ARM_LINK_OFFSET_X: int = 45   # Must be float in calculations
const D_FROSH_ARM_LINK_OFFSET_Y: int = 20   # Must be float in calculations
const D_FROSH_WIDTH_Y: int = 6      # How "fat" in the Y-dir are the frosh?
const D_BANK_HEIGHT: int = 40       # Height of the opposite bank of the pit
const D_ICON_WIDTH: int = 60        # Width of the icons in the game
const D_PALM_HEIGHT: int = 200      # Height of palm above base of hand
const D_TAM_Z: int = 490
const D_ARM_WIDTH: int = 45
const D_STAGGER: int = 20
const D_POLE_X: int = 320
const D_POLE_Y: int = 80
const D_CLIMBING_SPEED: int = 5
const D_SWIM_SPEED: int = 4
const D_SWIM_DISTANCE: int = 400
const D_SWIM_FRAME_RATE: int = 10
const D_PIZZA_EATING_OFFSET_X: int = 40
const D_COMMIE_PUNCHING_OFFSET_X: int = 30
const D_ARTSCI_SPLASHING_OFFSET_X: int = 130  # A bit more than D_SPLASHING_DISTANCE
const D_HIGH_SCORE_START_HEIGHT: int = 400

# === GAME CONDITIONS CONSTANTS ===
const NUM_PERFORMANCE_BOOSTS: int = 8
const NO_BAR: int = -1
const NUM_RING_SPOTS: int = 100
const NUM_TRICKS: int = 19
const NUM_JBAR_SPOTS: int = 4  # From aiMenuAndDisplay.cs

const BEER_DRINKING_SPEED: int = 3
const AI_KEEP_POLE_POSITION: bool = true

# === TIME CONSTANTS (in frames, running at ~25fps) ===
const TIME_WHAP: int = 2               # Time for a "WHAP!" to be onscreen
const TIME_BUBBLE: int = 50            # Time for a GW Word Bubble to be onscreen
const TIME_PIZZA_MUNCH: int = 3
const TIME_REACH_FOR_CLING: int = 20
const TIME_DROP_FROM_CLINGING: int = 450
const TIME_THINKING_TIME: int = 100
const TIME_AVERAGE_BOB_TIME: int = 35
const TIME_CLARK_MUG_FLOAT_ROTATION: int = 5
const TIME_BETWEEN_EVENTS: int = 100
const TIME_RANDOM_EVENT_INTERVAL: int = 4

# === NUMBER OF SPRITES IN SEQUENCES ===
const NSPR_FR1: int = 6      # Number of falling frosh sprites
const NSPR_FR1B: int = 1     # Number of splashing frosh
const NSPR_FR2: int = 2      # Number of leaping/diving frosh
const NSPR_FR3: int = 2      # Number of underwater sprites
const NSPR_FR4: int = 4      # Number of standard wading frosh
const NSPR_FR4E: int = 2     # Number of excited wading frosh

# === APPLE SPRITE COUNTS ===
const NSPR_APPLE1: int = 1   # Number of apples, depth 1
const NSPR_APPLE2: int = 1   # Number of apples, depth 2
const NSPR_APPLE3: int = 1   # Number of apples, depth 3
const NSPR_APPLE4: int = 1   # Number of apples, depth 4
const NSPR_APPLE5: int = 4   # Number of apples, depth 5
const NSPR_APPLE6: int = 1
const NSPR_APPLE7: int = 1
const NSPR_WHAP: int = 1     # Number of WHAP!'s

# === PIZZA SPRITE COUNTS ===
const NSPR_PIZZA1: int = 1
const NSPR_PIZZA2: int = 1
const NSPR_PIZZA3: int = 1
const NSPR_PIZZA4: int = 8
const NSPR_PIZZA5: int = 1
const NSPR_PIZZA6: int = 1
const NSPR_PIZZA7: int = 1

# === CLARK MUG SPRITE COUNTS ===
const NSPR_CLARK1: int = 1
const NSPR_CLARK2: int = 1
const NSPR_CLARK3: int = 1
const NSPR_CLARK4: int = 1
const NSPR_CLARK5A: int = 4  # Floating
const NSPR_CLARK5B: int = 1  # Miss
const NSPR_CLARK6: int = 1
const NSPR_CLARK7: int = 1

# === EXAM SPRITE COUNTS ===
const NSPR_EXAM1: int = 1
const NSPR_EXAM2: int = 1
const NSPR_EXAM3: int = 1
const NSPR_EXAM4: int = 4

# === GREASE SPRITE COUNTS ===
const NSPR_GREASE1: int = 1
const NSPR_GREASE2: int = 1
const NSPR_GREASE3: int = 1
const NSPR_GREASE4: int = 1
const NSPR_GREASE5A: int = 2  # Splatter
const NSPR_GREASE5B: int = 1  # Miss
const NSPR_GREASE6: int = 1
const NSPR_GREASE7: int = 1

# === ARTSCI SPRITE COUNTS ===
const NSPR_ARTSCIF_FALL: int = 3
const NSPR_ARTSCIF_HIT: int = 1
const NSPR_ARTSCIF_POPUP: int = 2
const NSPR_ARTSCIF_WADE: int = 4

const NSPR_ARTSCIM_FALL: int = 3
const NSPR_ARTSCIM_HIT: int = 2
const NSPR_ARTSCIM_POPUP: int = 5
const NSPR_ARTSCIM_WADE: int = 4

# === COMMIE SPRITE COUNTS ===
const NSPR_COMMIEF_FALL: int = 3
const NSPR_COMMIEF_HIT: int = 2
const NSPR_COMMIEF_POPUP: int = 5
const NSPR_COMMIEF_WADE: int = 4

const NSPR_COMMIEM_FALL: int = 3
const NSPR_COMMIEM_HIT: int = 1
const NSPR_COMMIEM_POPUP: int = 5
const NSPR_COMMIEM_WADE: int = 3

# === FREC SPRITE COUNTS ===
const NSPR_FRECGJ: int = 8   # Number of joyous frec frames
const NSPR_FRECGN: int = 8   # Number of normal frec frames

# === ARM SPRITE COUNTS ===
const NSPR_ARM_OHEAD: int = 4  # Number of overhead throw shots

# === ENERGY LEVELS ===
const ENERGY_SWING: int = 4000
const ENERGY_CHEER: int = 150
const ENERGY_START: int = 250
const ENERGY_SLAM: int = 700

# === COLLISION FLAGS ===
const INCLUDE_ONLY_PYRAMID_FROSH: bool = true
const INCLUDE_ALL_FROSH: bool = false
const INCLUDE_WHAP: bool = true
const NO_WHAP: bool = false
const POLE_ACTS_AS_SHIELD: bool = true
const NO_POLE_SHIELDING: bool = false

# === POP BOY CONSTANTS ===
const TOPPLE_POPBOY_LAME: int = 2
const TOPPLE_POPBOY_KEEN: int = 1

# === FLY IN AND OUT CONSTANTS ===
const AI_FLY_IN_AND_OUT_INCREMENT_NUMERATOR: int = 2
const AI_FLY_IN_AND_OUT_INCREMENT_DENOMINATOR: int = 3
const AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE: int = 5

# === COLLISION ALIASES (for code compatibility) ===
const NOWHAP: bool = NO_WHAP
const NOPOLESHIELDING: bool = NO_POLE_SHIELDING
