class_name SoundbankInfo

# SoundbankInfo.gd - Sound constants for volume and panning
# Ported from SoundbankInfo.cs
# This class exposes constants from Enums.gd for easier access

# Sound panning
const PAN_LEFT: int = Enums.PAN_LEFT
const PAN_CENTER: int = Enums.PAN_CENTER
const PAN_RIGHT: int = Enums.PAN_RIGHT

# Volume levels
const VOL_10: int = Enums.VOL_10
const VOL_9: int = Enums.VOL_9
const VOL_8: int = Enums.VOL_8
const VOL_7: int = Enums.VOL_7
const VOL_6: int = Enums.VOL_6
const VOL_5: int = Enums.VOL_5
const VOL_4: int = Enums.VOL_4
const VOL_3: int = Enums.VOL_3
const VOL_2: int = Enums.VOL_2
const VOL_1: int = Enums.VOL_1

const VOL_FULL: int = Enums.VOL_FULL
const VOL_DEFAULT: int = Enums.VOL_DEFAULT
const VOL_CROWD_SHOUT: int = Enums.VOL_CROWD_SHOUT
const VOL_CROWD: int = Enums.VOL_CROWD
const VOL_MUSIC: int = Enums.VOL_MUSIC
const VOL_HOLLAR: int = Enums.VOL_HOLLAR
const VOL_NORMAL: int = Enums.VOL_NORMAL

# Short sound list counts
const NSND_CLARK_FINISH: int = Enums.NSND_CLARK_FINISH
const NSND_EFFECTS_WHOOSH: int = Enums.NSND_EFFECTS_WHOOSH
const NSND_TAUNTS: int = Enums.NSND_TAUNTS
const NSND_EFFECTS_SMACK: int = Enums.NSND_EFFECTS_SMACK

# Long sound list counts
const NSND_FROSH_CLARK_FINISH: int = Enums.NSND_FROSH_CLARK_FINISH
const NSND_WHOOSH: int = Enums.NSND_WHOOSH
const NSND_WHAP: int = Enums.NSND_WHAP
const NSND_EXAM_TOSS: int = Enums.NSND_EXAM_TOSS
const NSND_FRECS_PROGRESS: int = Enums.NSND_FRECS_PROGRESS
const NSND_FRECS_REWARD: int = Enums.NSND_FRECS_REWARD
const NSND_ARTSCI_MALE_TAUNT: int = Enums.NSND_ARTSCI_MALE_TAUNT
const NSND_ARTSCI_FEMALE_TAUNT: int = Enums.NSND_ARTSCI_FEMALE_TAUNT
const NSND_COMMIE_MALE_TAUNT: int = Enums.NSND_COMMIE_MALE_TAUNT
const NSND_COMMIE_FEMALE_TAUNT: int = Enums.NSND_COMMIE_FEMALE_TAUNT
const NSND_FRECS_ROAR: int = Enums.NSND_FRECS_ROAR
const NSND_FRECS_CHEER: int = Enums.NSND_FRECS_CHEER
const NSND_FRECS_HIT_APPLE: int = Enums.NSND_FRECS_HIT_APPLE
const NSND_FRECS_HIT_EXAM: int = Enums.NSND_FRECS_HIT_EXAM

# Sprite counts for sound effects (also from original SoundbankInfo)
const NSPR_APPLE1: int = AIDefine.NSPR_APPLE1
const NSPR_APPLE2: int = AIDefine.NSPR_APPLE2
const NSPR_APPLE3: int = AIDefine.NSPR_APPLE3
const NSPR_APPLE4: int = AIDefine.NSPR_APPLE4
const NSPR_APPLE5: int = AIDefine.NSPR_APPLE5
const NSPR_APPLE6: int = AIDefine.NSPR_APPLE6
const NSPR_APPLE7: int = AIDefine.NSPR_APPLE7
const NSPR_WHAP: int = AIDefine.NSPR_WHAP

const NSPR_PIZZA1: int = AIDefine.NSPR_PIZZA1
const NSPR_PIZZA2: int = AIDefine.NSPR_PIZZA2
const NSPR_PIZZA3: int = AIDefine.NSPR_PIZZA3
const NSPR_PIZZA4: int = AIDefine.NSPR_PIZZA4
const NSPR_PIZZA5: int = AIDefine.NSPR_PIZZA5
const NSPR_PIZZA6: int = AIDefine.NSPR_PIZZA6
const NSPR_PIZZA7: int = AIDefine.NSPR_PIZZA7

const NSPR_CLARK1: int = AIDefine.NSPR_CLARK1
const NSPR_CLARK2: int = AIDefine.NSPR_CLARK2
const NSPR_CLARK3: int = AIDefine.NSPR_CLARK3
const NSPR_CLARK4: int = AIDefine.NSPR_CLARK4
const NSPR_CLARK5A: int = AIDefine.NSPR_CLARK5A
const NSPR_CLARK5B: int = AIDefine.NSPR_CLARK5B
const NSPR_CLARK6: int = AIDefine.NSPR_CLARK6
const NSPR_CLARK7: int = AIDefine.NSPR_CLARK7

const NSPR_EXAM1: int = AIDefine.NSPR_EXAM1
const NSPR_EXAM2: int = AIDefine.NSPR_EXAM2
const NSPR_EXAM3: int = AIDefine.NSPR_EXAM3
const NSPR_EXAM4: int = AIDefine.NSPR_EXAM4

# Popup sprite counts
const NSPR_ARTSCIF_FALL: int = AIDefine.NSPR_ARTSCIF_FALL
const NSPR_ARTSCIF_POPUP: int = AIDefine.NSPR_ARTSCIF_POPUP
const NSPR_ARTSCIF_WADE: int = AIDefine.NSPR_ARTSCIF_WADE

const NSPR_ARTSCIM_FALL: int = AIDefine.NSPR_ARTSCIM_FALL
const NSPR_ARTSCIM_POPUP: int = AIDefine.NSPR_ARTSCIM_POPUP
const NSPR_ARTSCIM_WADE: int = AIDefine.NSPR_ARTSCIM_WADE

const NSPR_COMMIEF_FALL: int = AIDefine.NSPR_COMMIEF_FALL
const NSPR_COMMIEF_POPUP: int = AIDefine.NSPR_COMMIEF_POPUP
const NSPR_COMMIEF_WADE: int = AIDefine.NSPR_COMMIEF_WADE

const NSPR_COMMIEM_FALL: int = AIDefine.NSPR_COMMIEM_FALL
const NSPR_COMMIEM_POPUP: int = AIDefine.NSPR_COMMIEM_POPUP
const NSPR_COMMIEM_WADE: int = AIDefine.NSPR_COMMIEM_WADE

# Alias for compatibility
const NSND_FROSH_CLARKFINISH: int = NSND_FROSH_CLARK_FINISH
