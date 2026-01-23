class_name AICrowd

# aiCrowd.gd - Static class with FREC crowd behavior
# Ported from aiCrowd.cs
#
# Per porting_rules.md:
# - This is a static class with logic functions only
# - All shared state accessed via AIMethods autoload
# - Cross-file calls use class qualification

# === CONSTANTS ===
const SWITCHTIME1: int = 100
const SWITCHTIME2: int = 250
const SWITCHTIME3: int = 370
const TIME_BETWEEN_CHEERS: int = 250

# === HELPER FUNCTIONS ===

static func ais_set_frec_action(s: TSprite, n_new_action: int) -> void:
	s.n_cc = 0
	if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ACTION] != Enums.CrowdActions.FA_PART:
		s.n_attrib[Enums.NAttrCrowd.ATTR_F_ACTION] = n_new_action


static func pan_on_x(s: TSprite) -> int:
	return int((s.n_x - 320) / 3.2)


# === BLOCK ACTIONS ===

static func ai_frec_group_block(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_AppleBlock])
	if s.n_cc > 40:
		ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)


static func ai_frec_action_block(s: TSprite) -> void:
	ai_frec_group_block(s)
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_AppleBlock])


# === MILLING ACTIONS ===

static func ai_frec_group_mill(s: TSprite) -> void:
	var n_temp: int = AIMethods.R.randi() % SoundbankInfo.NSND_FRECS_ROAR
	if 0 == AIMethods.R.randi() % 600 and s == AIMethods.spr_frecs_r:
		ais_set_frec_action(s, Enums.CrowdActions.FA_WAVE)

	if 0 != AIMethods.n_frosh_level[6]:
		ais_set_frec_action(s, Enums.CrowdActions.FA_LOOK_UP)

	if (0 == AIMethods.R.randi() % 40) or s.n_cc == 1:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Milling1 + AIMethods.R.randi() % 3])

	if (s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] > AIDefine.ENERGY_CHEER) and 0 != AIMethods.R.randi() % 2:
		if not AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDROAR1 + n_temp].is_playing():
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDROAR1 + n_temp].play(SoundbankInfo.VOL_CROWD, pan_on_x(s))
		ais_set_frec_action(s, Enums.CrowdActions.FA_CHEERING)


static func ai_frec_action_mill(s: TSprite) -> void:
	if 0 == AIMethods.R.randi() % 600 and s == AIMethods.spr_frecs_r:
		ais_set_frec_action(s, Enums.CrowdActions.FA_WAVE)

	if 0 != AIMethods.n_frosh_level[6]:
		ais_set_frec_action(s, Enums.CrowdActions.FA_LOOK_UP)

	if (0 == AIMethods.R.randi() % 40) or s.n_cc == 1:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Milling1 + AIMethods.R.randi() % 2])

	if (s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] > AIDefine.ENERGY_CHEER) and 0 != AIMethods.R.randi() % 2:
		var n_temp: int = AIMethods.R.randi() % SoundbankInfo.NSND_FRECS_ROAR
		if not AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDROAR1 + n_temp].is_playing():
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDROAR1 + n_temp].play(SoundbankInfo.VOL_CROWD, pan_on_x(s))
		ais_set_frec_action(s, Enums.CrowdActions.FA_CHEERING)


# === CHEERING ACTIONS ===

static func ai_frec_group_cheer(s: TSprite) -> void:
	if s.n_cc > 16:
		s.n_cc = 0
	if s.n_cc == 1:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Cheer1])
	if s.n_cc == 5:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Cheer2])
	if s.n_cc == 9:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Cheer3])
	if s.n_cc == 12:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Cheer4])

	if (s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] < AIDefine.ENERGY_CHEER) and (0 == AIMethods.R.randi() % 5):
		ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
	if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] > AIDefine.ENERGY_SLAM:
		if not AIMethods.l_sound[Enums.ASLList.LSND_FRECS_SLAM].is_playing():
			AIMethods.l_sound[Enums.ASLList.LSND_FRECS_SLAM].play(SoundbankInfo.VOL_CROWD)
		ais_set_frec_action(s, Enums.CrowdActions.FA_SLAMMING)


static func ai_frec_action_cheer(s: TSprite) -> void:
	if s.n_cc > 10:
		s.n_cc = 0
	if s.n_cc == 1:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Shout1])
	if s.n_cc == 5:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Shout2])
	if s.n_cc == 7:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Shout3])

	if (s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] < AIDefine.ENERGY_CHEER) and (0 == AIMethods.R.randi() % 5):
		ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
	if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] > AIDefine.ENERGY_SLAM:
		if not AIMethods.l_sound[Enums.ASLList.LSND_FRECS_SLAM].is_playing():
			AIMethods.l_sound[Enums.ASLList.LSND_FRECS_SLAM].play(SoundbankInfo.VOL_CROWD)
		ais_set_frec_action(s, Enums.CrowdActions.FA_SLAMMING)


# === SLAMMING ACTIONS ===

static func ai_frec_group_slam(s: TSprite) -> void:
	if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] > AIDefine.ENERGY_SLAM + 300:
		s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = AIDefine.ENERGY_SLAM + 100

	match s.n_cc:
		4: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Slam2])
		12: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Slam3])
		15: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Slam1])
		20:
			if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] < AIDefine.ENERGY_SLAM:
				ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
			else:
				s.n_cc = 0
				if not AIMethods.l_sound[Enums.ASLList.LSND_FRECS_SLAM].is_playing():
					AIMethods.l_sound[Enums.ASLList.LSND_FRECS_SLAM].play(SoundbankInfo.VOL_CROWD)


static func ai_frec_action_slam(s: TSprite) -> void:
	if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] > AIDefine.ENERGY_SLAM + 300:
		s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = AIDefine.ENERGY_SLAM + 100

	match s.n_cc:
		4: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Slam2])
		12: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Slam3])
		15: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Slam1])
		20:
			if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] < AIDefine.ENERGY_SLAM:
				ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
			else:
				s.n_cc = 0
				if not AIMethods.l_sound[Enums.ASLList.LSND_FRECS_SLAM].is_playing():
					AIMethods.l_sound[Enums.ASLList.LSND_FRECS_SLAM].play(SoundbankInfo.VOL_CROWD)


# === LOOK UP ACTIONS ===

static func ai_frec_group_look_up(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_LookUp1])
	if s.n_cc > 50 and 0 == AIMethods.R.randi() % 6:
		ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
	if (s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] > AIDefine.ENERGY_CHEER) and 0 != AIMethods.R.randi() % 2:
		ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)


static func ai_frec_action_look_up(s: TSprite) -> void:
	s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_LookUp1])
	if s.n_cc > 50 and 0 == AIMethods.R.randi() % 6:
		ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
	if (s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] > AIDefine.ENERGY_CHEER) and 0 != AIMethods.R.randi() % 2:
		ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)


# === STAYIN' ALIVE ACTIONS ===

static func ai_frec_group_stayin_alive(s: TSprite) -> void:
	if s.n_cc > 300:
		ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
	else:
		match AIMethods.spr_pole.n_cc % 69:
			4, 24, 40, 56:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_StayinAlive1])
			16, 32, 48, 64:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_StayinAlive2])


static func ai_frec_action_stayin_alive(s: TSprite) -> void:
	if s.n_cc > 500:
		ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
	else:
		match AIMethods.spr_pole.n_cc % 69:
			4, 24, 40, 56:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_StayinAlive2])
			16, 32, 48, 64:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_StayinAlive1])


# === PART ACTIONS ===

static func ai_frec_action_part(s: TSprite) -> void:
	# HUH?
	ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)


static func ai_frec_group_part(s: TSprite) -> void:
	match s.n_cc:
		1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Milling1])
		57: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Part1])
		65: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Part2])
		137: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Part1])
		145: s.n_attrib[Enums.NAttrCrowd.ATTR_F_ACTION] = Enums.CrowdActions.FA_MILLING


# === BOO ACTIONS ===

static func ai_frec_group_boo(s: TSprite) -> void:
	match s.n_cc:
		1:
			AIMethods.l_sound[Enums.ASLList.LSND_FRECS_BOO1 + AIMethods.R.randi() % 3].play(SoundbankInfo.VOL_CROWD, pan_on_x(s))
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Boo1])
		15: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Boo2])
		30: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Boo3])
		45: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Boo2])
		60: ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)


static func ai_frec_action_boo(s: TSprite) -> void:
	match s.n_cc:
		1:
			AIMethods.l_sound[Enums.ASLList.LSND_FRECS_BOO1 + AIMethods.R.randi() % 3].play(SoundbankInfo.VOL_CROWD, pan_on_x(s))
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Boo1])
		15: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Boo2])
		30: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Boo3])
		45: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Boo2])
		60: ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)


# === SHOUT ACTIONS ===

static func ai_frec_action_shout(s: TSprite) -> void:
	ais_set_frec_action(s, Enums.CrowdActions.FA_CHEERING)


static func ai_frec_group_shout(s: TSprite) -> void:
	if s.n_cc > 20:
		s.n_cc = 0
	if s.n_cc == 1:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Shout1])
	if s.n_cc == 10:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Shout2])

	if (s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] < AIDefine.ENERGY_CHEER) and (0 == AIMethods.R.randi() % 5):
		ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
	if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] > AIDefine.ENERGY_SLAM:
		AIMethods.l_sound[Enums.ASLList.LSND_FRECS_SLAM].play(SoundbankInfo.VOL_CROWD)
		ais_set_frec_action(AIMethods.spr_frecs_l, Enums.CrowdActions.FA_SLAMMING)
		ais_set_frec_action(AIMethods.spr_frecs_c, Enums.CrowdActions.FA_SLAMMING)
		ais_set_frec_action(AIMethods.spr_frecs_r, Enums.CrowdActions.FA_SLAMMING)


# === LOOK ULR/URL ACTIONS ===

static func ai_frec_group_look_url(s: TSprite) -> void:
	if s.n_cc < 0:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_LookULR3])
	match s.n_cc:
		1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_LookULR3])
		SWITCHTIME1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_LookULR2])
		SWITCHTIME2: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_LookULR1])
		SWITCHTIME3: ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)


static func ai_frec_group_look_ulr(s: TSprite) -> void:
	if s.n_cc < 0:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_LookULR1])
	match s.n_cc:
		1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_LookULR1])
		SWITCHTIME1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_LookULR2])
		SWITCHTIME2: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_LookULR3])
		SWITCHTIME3: ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)


static func ai_frec_action_look_url(s: TSprite) -> void:
	if s.n_cc < 0:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_LookULR3])
	match s.n_cc:
		1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_LookULR3])
		SWITCHTIME1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_LookULR2])
		SWITCHTIME2: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_LookULR1])
		SWITCHTIME3: ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)


static func ai_frec_action_look_ulr(s: TSprite) -> void:
	if s.n_cc < 0:
		s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_LookULR1])
	match s.n_cc:
		1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_LookULR1])
		SWITCHTIME1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_LookULR2])
		SWITCHTIME2: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_LookULR3])
		SWITCHTIME3: ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)


# === WAVE ACTIONS ===

static func ai_frec_group_wave(s: TSprite) -> void:
	match s.n_cc:
		1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Wave1])
		3: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Wave2])
		6: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpGF_Wave3])
		9:
			ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
			if s == AIMethods.spr_frecs_r:
				ais_set_frec_action(AIMethods.spr_frecs_c, Enums.CrowdActions.FA_WAVE)
			if s == AIMethods.spr_frecs_c:
				ais_set_frec_action(AIMethods.spr_frecs_l, Enums.CrowdActions.FA_WAVE)


static func ai_frec_action_wave(s: TSprite) -> void:
	match s.n_cc:
		1: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Wave1])
		3: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Wave2])
		6: s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpAF_Wave3])
		9:
			ais_set_frec_action(s, Enums.CrowdActions.FA_MILLING)
			if s == AIMethods.spr_frecs_r:
				ais_set_frec_action(AIMethods.spr_frecs_c, Enums.CrowdActions.FA_WAVE)
			if s == AIMethods.spr_frecs_c:
				ais_set_frec_action(AIMethods.spr_frecs_l, Enums.CrowdActions.FA_WAVE)


# === MAIN FREC GROUP BEHAVIOR ===

static func ai_frec_group(s: TSprite) -> void:
	# Let the center frecs act as the energy givers
	if s == AIMethods.spr_frecs_c:
		var n_temp: int = Globals.myGameConditions.get_energy()
		AIMethods.spr_frecs_l.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += n_temp
		AIMethods.spr_frecs_c.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += n_temp
		AIMethods.spr_frecs_r.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += n_temp
		Globals.myGameConditions.reset_energy()

	match s.n_attrib[Enums.NAttrCrowd.ATTR_F_ACTION]:
		Enums.CrowdActions.FA_MILLING: ai_frec_group_mill(s)
		Enums.CrowdActions.FA_CHEERING: ai_frec_group_cheer(s)
		Enums.CrowdActions.FA_SLAMMING: ai_frec_group_slam(s)
		Enums.CrowdActions.FA_BOOING: ai_frec_group_boo(s)
		Enums.CrowdActions.FA_SHOUTING: ai_frec_group_shout(s)
		Enums.CrowdActions.FA_BLOCKING: ai_frec_group_block(s)
		Enums.CrowdActions.FA_LOOK_UP: ai_frec_group_look_up(s)
		Enums.CrowdActions.FA_LOOK_ULR: ai_frec_group_look_ulr(s)
		Enums.CrowdActions.FA_LOOK_URL: ai_frec_group_look_url(s)
		Enums.CrowdActions.FA_STAYIN_ALIVE: ai_frec_group_stayin_alive(s)
		Enums.CrowdActions.FA_WAVE: ai_frec_group_wave(s)
		Enums.CrowdActions.FA_PART: ai_frec_group_part(s)

	s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] -= 1
	if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] < 0:
		s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = 0


# === MAIN FREC ACTION BEHAVIOR ===

static func ai_frec_action(s: TSprite) -> void:
	# Let the center frecs act as the energy givers
	if s == AIMethods.spr_frecs_c:
		var n_temp: int = Globals.myGameConditions.get_energy()
		AIMethods.spr_frecs_l.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += n_temp
		AIMethods.spr_frecs_c.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += n_temp
		AIMethods.spr_frecs_r.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += n_temp
		AIMethods.spr_forge.n_attrib[Enums.NAttrForge.ATTR_FORGE_ENERGY] += n_temp
		Globals.myGameConditions.reset_energy()

	match s.n_attrib[Enums.NAttrCrowd.ATTR_F_ACTION]:
		Enums.CrowdActions.FA_MILLING: ai_frec_action_mill(s)
		Enums.CrowdActions.FA_CHEERING: ai_frec_action_cheer(s)
		Enums.CrowdActions.FA_SLAMMING: ai_frec_action_slam(s)
		Enums.CrowdActions.FA_BOOING: ai_frec_action_boo(s)
		Enums.CrowdActions.FA_SHOUTING: ai_frec_action_shout(s)
		Enums.CrowdActions.FA_BLOCKING: ai_frec_action_block(s)
		Enums.CrowdActions.FA_LOOK_UP: ai_frec_action_look_up(s)
		Enums.CrowdActions.FA_LOOK_ULR: ai_frec_action_look_ulr(s)
		Enums.CrowdActions.FA_LOOK_URL: ai_frec_action_look_url(s)
		Enums.CrowdActions.FA_STAYIN_ALIVE: ai_frec_action_stayin_alive(s)
		Enums.CrowdActions.FA_WAVE: ai_frec_action_wave(s)
		Enums.CrowdActions.FA_PART: ai_frec_action_part(s)

	s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] -= 1
	if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] < 0:
		s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = 0


# === BORING FREC BEHAVIOR ===

static func ai_frec_boring(s: TSprite) -> void:
	s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] += Globals.myGameConditions.get_energy()
	Globals.myGameConditions.reset_energy()

	if (s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] > AIDefine.ENERGY_CHEER) and s.n_cc > TIME_BETWEEN_CHEERS:
		s.n_cc = 0
		var n_temp: int = AIMethods.R.randi() % SoundbankInfo.NSND_FRECS_ROAR
		if not AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDROAR1 + n_temp].is_playing():
			AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_CROWDROAR1 + n_temp].play(SoundbankInfo.VOL_CROWD, pan_on_x(s))

	s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] -= 1
	if s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] < 0:
		s.n_attrib[Enums.NAttrCrowd.ATTR_F_ENERGY] = 0
