class_name AISupport

# aiSupport.gd - Static class with support/helper functions
# Ported from aiSupport.cs
#
# Per porting_rules.md:
# - This is a static class with logic functions only
# - All shared state accessed via AIMethods autoload
# - Cross-file calls use class qualification: AISupport.ais_mouse_over(s)

# === POINT COLLISION DETECTION ===

static func ais_scr_point_inside(s: TSprite, x: int, y: int) -> bool:
	# Determines if a screen coordinate (x, y) is inside the area of the
	# screen on which this sprite is displayed.
	if s.frm_frame == null:
		return false

	var n_top_left_x: int = s.n_scr_x + s.frm_frame.hotspot_x - s.frm_frame.n_x1
	var n_top_left_y: int = s.n_scr_y + s.frm_frame.hotspot_y - s.frm_frame.n_z1
	var n_bottom_right_x: int = s.n_scr_x + s.frm_frame.hotspot_x + s.frm_frame.n_x2
	var n_bottom_right_y: int = s.n_scr_y + s.frm_frame.hotspot_y + s.frm_frame.n_z2

	if x >= n_top_left_x and x <= n_bottom_right_x and y >= n_top_left_y and y <= n_bottom_right_y:
		return true
	return false


static func ais_mouse_over(s: TSprite) -> bool:
	# Check if mouse is over sprite
	return ais_scr_point_inside(s, Globals.InputService.get_mouse_x(), Globals.InputService.get_mouse_y())


# === MOVEMENT FUNCTIONS ===

static func ais_plummet(s: TSprite) -> void:
	# Move and apply gravity
	s.n_x += s.nv_x
	s.n_y += s.nv_y
	s.n_z += s.nv_z
	# Accelerate in the z-direction (gravity)
	s.nv_z -= AIDefine.D_GRAV_CONST


static func ais_move_towards_destination(s: TSprite) -> void:
	# Move the sprite at its velocity towards its destination
	if abs(s.n_x - s.n_dest_x) > s.nv_x:
		if s.n_x < s.n_dest_x:
			s.n_x += s.nv_x
		else:
			s.n_x -= s.nv_x

	if abs(s.n_y - s.n_dest_y) > s.nv_y:
		if s.n_y < s.n_dest_y:
			s.n_y += s.nv_y
		else:
			s.n_y -= s.nv_y


static func ais_keep_in_pit_x(s: TSprite) -> void:
	# Keeps a sprite within (dPITMINX..dPITMAXX), and reverses x-velocity if boundary hit
	if s.n_x > AIDefine.D_PIT_MAX_X_PLUS_50:
		s.n_x = AIDefine.D_PIT_MAX_X_PLUS_50
		s.nv_x = -s.nv_x
	if s.n_x < AIDefine.D_PIT_MIN_X_MINUS_50:
		s.n_x = AIDefine.D_PIT_MIN_X_MINUS_50
		s.nv_x = -s.nv_x


static func ais_keep_in_pit_y(s: TSprite) -> void:
	# Keeps a sprite within (dPITMINY..dPITMAXY), and reverses y-velocity if boundary hit
	if s.n_y > AIDefine.D_PIT_MAX_Y_PLUS_30:
		s.n_y = AIDefine.D_PIT_MAX_Y_PLUS_30
		s.nv_y = -s.nv_y
	if s.n_y < AIDefine.D_PIT_MIN_Y:
		s.n_y = AIDefine.D_PIT_MIN_Y
		s.nv_y = -s.nv_y


# === BOBBING ANIMATION ===

static func ais_bob_up_and_down(s: TSprite, n_delay: int = AIDefine.TIME_AVERAGE_BOB_TIME) -> void:
	if 0 == AIMethods.R.randi() % n_delay:
		s.n_z = -1 if s.n_z == 0 else 0
		if (0 == AIMethods.R.randi() % 3) and (0 != Globals.myGameConditions.get_enhanced_graphics()):
			AIMethods.ss_water.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_RIPPLE, s.n_x, s.n_y))


# === COORDINATE CONVERSION ===

static func ais_game_xy_to_pit_xy(n_game_x: int, n_game_y: int) -> Vector2i:
	# Convert game coordinates to pit coordinates
	var n_pit_x: int = n_game_x
	var n_pit_y: int = n_game_y + Globals.myLayers.get_offset(Enums.LayerNames.LAYER_PIT)
	return Vector2i(n_pit_x, n_pit_y)


# === FROSH STATE CHANGES ===

static func ais_send_frosh_flying(s: TSprite) -> void:
	s.set_behavior(Callable(AIFrosh, "ai_act_1"))
	s.n_attrib[Enums.NAttrFrosh.ATTR_PYRAMID_LEVEL] = 0
	s.set_goal(Enums.Goals.GOAL_MINDLESS_WANDERING)
	s.n_dest_x = AIMethods.randintin(AIDefine.D_PIT_MIN_X, AIDefine.D_PIT_MAX_X)
	s.n_dest_y = AIMethods.randintin(AIDefine.D_PIT_MIN_Y, AIDefine.D_PIT_MAX_Y)


static func ais_send_frosh_really_flying(s: TSprite) -> void:
	ais_send_frosh_flying(s)
	AIFrosh.ai_init_1(s)
	s.n_attrib[Enums.NAttrFrosh.ATTR_HEIGHT_OF_FALL] = s.n_z


static func ais_choose_new_personality(s: TSprite) -> void:
	# Choose a new personality for this frosh
	s.n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY] = AIMethods.R.randi() % AIDefine.N_FROSH_PERSONALITIES


# === PROJECTILE CREATION ===

static func ais_create_projectile(n_x: int, n_y: int, spr_type: int, n_toss_power: int = 1) -> void:
	var new_projectile: TSprite

	# Note conversion between MISC layer and PIT layer
	new_projectile = SpriteInit.create_sprite(spr_type, n_x, n_y - AIDefine.D_PALM_HEIGHT + AIMethods.d_misc_y_to_pit_y())
	new_projectile.n_dest_x = Globals.InputService.get_mouse_x()
	new_projectile.n_dest_y = Globals.InputService.get_mouse_y() + AIMethods.d_misc_y_to_pit_y()
	new_projectile.n_attrib[Enums.AttrProjectile.ATTR_START_X] = new_projectile.n_x
	new_projectile.n_attrib[Enums.AttrProjectile.ATTR_START_Y] = new_projectile.n_y
	new_projectile.n_attrib[Enums.AttrProjectile.ATTR_POWER_OF_THROW] = n_toss_power
	AIMethods.ss_tossed.include(new_projectile)


# === WEIGHT ON SHOULDERS ===

static func ais_weight_on_shoulders(s: TSprite) -> void:
	if 0 == AIMethods.R.randi() % 20:
		s.b_attrib[Enums.BAttrFrosh.ATTR_WEIGHT_ON_SHOULDERS] = true
		if s.n_z < 30:
			s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR7B_1])
			if 0 == AIMethods.R.randi() % 5:
				s.set_frame(AIMethods.frm[Enums.GameBitmapEnumeration.bmpFR7B_2])
				# PERFORMANCE BOOST: STRENGTH: FROSH CAN HANDLE WEIGHT ON THEIR SHOULDERS
				# START AT 10, GROW TOWARDS 1000+ AS t.INFINITY
				if 0 == AIMethods.R.randi() % Globals.myGameConditions.get_booster(1):
					ais_send_frosh_flying(s)


# === FORGE TRICK ===

static func ais_forge_trick(n_trick: int, n_energy: int) -> bool:
	if Globals.myGameConditions.use_trick(n_trick):
		AIMethods.spr_forge.n_attrib[Enums.NAttrForge.ATTR_FORGE_ENERGY] += n_energy
		return true
	else:
		return false


# === FREC ACTIONS ===

static func ais_set_frec_action(spr_frecs: TSprite, n_action: int) -> void:
	if spr_frecs != null:
		spr_frecs.n_cc = 0
		if spr_frecs.n_attrib[Enums.NAttrCrowd.ATTR_F_ACTION] != Enums.CrowdActions.FA_PART:
			spr_frecs.n_attrib[Enums.NAttrCrowd.ATTR_F_ACTION] = n_action


# === UNLOCK ACHIEVEMENT ===

static func ais_unlock_achievement(achievement_id: int) -> void:
	# Delegate to the real implementation in AIMisc
	AIMisc.ais_unlock_achievement(achievement_id)


# === IRON RING ZAP ===

static func ais_iron_ring_zap() -> void:
	var ss_hit_frosh: SpriteSet = ais_get_targets_in_scr_range(
		AIMethods.ss_fr,
		Globals.InputService.get_mouse_x() - 15, Globals.InputService.get_mouse_y() - 15,
		Globals.InputService.get_mouse_x() + 15, Globals.InputService.get_mouse_y() + 15,
		AIDefine.INCLUDE_ALL_FROSH
	)

	var n: int = ss_hit_frosh.get_number_of_sprites()

	if n > 0:
		for i in range(n):
			var him: TSprite = ss_hit_frosh.get_sprite(i)
			if AIMethods.R.randi() % 2 == 1:
				AIFrosh.ai_init_6e(him)
			else:
				AIFrosh.ai_init_6f(him)


# === RANGE QUERIES ===

static func ais_get_targets_in_scr_range(ss_targets: SpriteSet, n_left_of_range: int, n_top_of_range: int,
		n_right_of_range: int, n_bot_of_range: int, b_pole_only: bool) -> SpriteSet:
	# Returns the Frosh within the SCREENspace outlined by the two points passed in.
	var n_mid_y_of_range: int = (n_top_of_range + n_bot_of_range) / 2
	var n_mid_x_of_range: int = (n_left_of_range + n_right_of_range) / 2

	if AIMethods._targets_in_scr_range == null:
		AIMethods._targets_in_scr_range = SpriteSet.new(Enums.LayerNames.LAYER_PIT)
	else:
		AIMethods._targets_in_scr_range.remove_all()

	var n: int = ss_targets.get_number_of_sprites()

	for i in range(n):
		var him: TSprite = ss_targets.get_sprite(i)
		var n_left_of_sprite: int = ss_targets.get_left_most_scr_point_on_sprite(i)
		var n_top_of_sprite: int = ss_targets.get_top_most_scr_point_on_sprite(i)
		var n_right_of_sprite: int = ss_targets.get_right_most_scr_point_on_sprite(i)
		var n_bot_of_sprite: int = ss_targets.get_bottom_most_scr_point_on_sprite(i)

		if ((n_top_of_range > n_top_of_sprite and n_top_of_range < n_bot_of_sprite) or
			(n_mid_y_of_range > n_top_of_sprite and n_mid_y_of_range < n_bot_of_sprite) or
			(n_bot_of_range > n_top_of_sprite and n_bot_of_range < n_bot_of_sprite)):
			if ((n_left_of_range > n_left_of_sprite and n_left_of_range < n_right_of_sprite) or
				(n_mid_x_of_range > n_left_of_sprite and n_mid_x_of_range < n_right_of_sprite) or
				(n_right_of_range > n_left_of_sprite and n_right_of_range < n_right_of_sprite)):
				if (not b_pole_only) or not (him.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] < 7):
					AIMethods._targets_in_scr_range.include(him)

	return AIMethods._targets_in_scr_range


static func ais_get_frosh_in_range(n_left_of_range: int, n_top_of_range: int,
		n_right_of_range: int, n_bot_of_range: int) -> SpriteSet:
	return ais_get_sprites_in_range(n_left_of_range, n_top_of_range, n_right_of_range, n_bot_of_range, AIMethods.ss_fr, true)


static func ais_get_sprites_in_range(n_range_l: int, n_range_t: int, n_range_r: int, n_range_b: int,
		ss_target: SpriteSet, b_rigorous: bool = true) -> SpriteSet:
	# Returns the Sprites within the FROSHspace outlined by the two points passed in.
	# NOTE: THIS FUNCTION ISN'T GOOD AT FINDING SMALL THINGS
	var n_range_mx: int = (n_range_l + n_range_r) / 2
	var n_range_my: int = (n_range_t + n_range_b) / 2

	if AIMethods._sprites_in_range == null:
		AIMethods._sprites_in_range = SpriteSet.new(Enums.LayerNames.LAYER_PIT)
	else:
		AIMethods._sprites_in_range.remove_all()

	var n: int = ss_target.get_number_of_sprites()

	for i in range(n):
		var him: TSprite = ss_target.get_sprite(i)
		var n_sprite_l: int = ss_target.get_left_most_point_on_sprite(i)
		var n_sprite_t: int = ss_target.get_top_most_point_on_sprite(i)
		var n_sprite_r: int = ss_target.get_right_most_point_on_sprite(i)
		var n_sprite_b: int = ss_target.get_bottom_most_point_on_sprite(i)
		var n_sprite_mx: int = (n_sprite_l + n_sprite_r) / 2
		var n_sprite_my: int = (n_sprite_t + n_sprite_b) / 2

		if ((n_range_t > n_sprite_t and n_range_t < n_sprite_b) or
			(n_range_my > n_sprite_t and n_range_my < n_sprite_b) or
			(n_range_b > n_sprite_t and n_range_b < n_sprite_b)):
			if ((n_range_l > n_sprite_l and n_range_l < n_sprite_r) or
				(n_range_mx > n_sprite_l and n_range_mx < n_sprite_r) or
				(n_range_r > n_sprite_l and n_range_r < n_sprite_r)):
				AIMethods._sprites_in_range.include(him)

		if b_rigorous:
			if ((n_sprite_t > n_range_t and n_sprite_t < n_range_b) or
				(n_sprite_my > n_range_t and n_sprite_my < n_range_b) or
				(n_sprite_b > n_range_t and n_sprite_b < n_range_b)):
				if ((n_sprite_l > n_range_l and n_sprite_l < n_range_r) or
					(n_sprite_mx > n_range_l and n_sprite_mx < n_range_r) or
					(n_sprite_r > n_range_l and n_sprite_r < n_range_r)):
					AIMethods._sprites_in_range.include(him)

	return AIMethods._sprites_in_range


# === PYRAMID SPOT SELECTION ===

static func ais_pick_pyramid_spot(s: TSprite) -> bool:
	# Chooses a Pyramid Spot and assigns it as the destination
	# Returns false if no pyramid spot is available
	var n_chain: int = AIMethods.R.randi() % PolePosition.N_PP_CHAINS
	var n_debug_check: int = 0

	var pp_my_spot: PolePosition = null
	while pp_my_spot == null:
		pp_my_spot = PolePosition.pole_chains[n_chain].first_free_child()
		n_chain += 1
		n_chain = n_chain % PolePosition.N_PP_CHAINS
		n_debug_check += 1
		if n_debug_check > PolePosition.N_PP_CHAINS:
			return false

	s.pp_chosen = pp_my_spot
	s.pp_chosen.set_claim(s)
	s.n_dest_x = pp_my_spot.get_x()
	s.n_dest_y = pp_my_spot.get_y()
	return true


static func ais_pick_closer_pyramid_spot(s: TSprite) -> bool:
	# Takes one look for a Pyramid Spot closer than the one you're at
	var pp_potential_spot: PolePosition = s.pp_chosen.adjacent_chain().first_free_child()
	if pp_potential_spot == null:
		return false  # All the spots in the adjacent chain are taken

	if pp_potential_spot.n_ordinal < s.pp_chosen.n_ordinal:
		# The next spot is closer to the pole
		s.pp_chosen.release_claim()
		s.pp_chosen = pp_potential_spot
		pp_potential_spot.set_claim(s)
		s.n_dest_x = pp_potential_spot.get_x()
		s.n_dest_y = pp_potential_spot.get_y()
		return true
	return false


static func ais_pick_climbing_spot(s: TSprite) -> bool:
	# Chooses a Climbing Spot and assigns it as the destination
	# Returns false if no spot is available
	var n_chain: int = AIMethods.R.randi() % PolePosition.N_PP_CHAINS
	var n_debug_check: int = 0

	var pp_my_spot: PolePosition = null
	while pp_my_spot == null:
		pp_my_spot = PolePosition.pole_chains[AIMethods.R.randi() % PolePosition.N_PP_CHAINS].last_taken_child()
		n_chain += 1
		n_chain = n_chain % PolePosition.N_PP_CHAINS
		n_debug_check += 1
		if n_debug_check > PolePosition.N_PP_CHAINS:
			return false

	s.n_dest_x = pp_my_spot.get_x()
	s.n_dest_y = pp_my_spot.get_y() + (3 if pp_my_spot.get_y() > AIDefine.D_POLE_Y else -3)
	return true


# === FROSH GOAL SELECTION ===

static func ais_choose_frosh_pit_goal(s: TSprite) -> void:
	# Choose a new goal for a Frosh wading in the pit
	s.set_goal(Enums.Goals.GOAL_MINDLESS_WANDERING)

	# On occasion, change the personality of the Frosh
	if 0 == AIMethods.R.randi() % 10:
		ais_choose_new_personality(s)

	match s.n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY]:
		Enums.Personalities.PERS_GOOFY:
			var target_frosh: TSprite = AIMethods.ss_fr.get_sprite(AIMethods.R.randi() % AIDefine.GN_NUM_FROSH_IN_PIT)
			s.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_MINDLESS_WANDERING
			s.n_dest_x = target_frosh.n_x - AIDefine.D_SPLASHING_DISTANCE
			s.n_dest_y = target_frosh.n_y

		Enums.Personalities.PERS_HEAVYWEIGHT:
			if ais_pick_pyramid_spot(s):
				s.set_goal(Enums.Goals.GOAL_PYRAMID_SPOT)

		Enums.Personalities.PERS_HOISTER, Enums.Personalities.PERS_CLIMBER:
			if ais_pick_climbing_spot(s):
				s.set_goal(Enums.Goals.GOAL_CLIMBING_UP)


static func ais_choose_frosh_upper_level_goal(s: TSprite) -> void:
	# Assign an "upper-level goal" for this frosh
	if s.n_z > 240:
		s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] = Enums.UpperLevelGoals.UPPER_GOAL_CLIMB
	else:
		if AIMethods.R.randi() % 15 < 11:
			s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] = Enums.UpperLevelGoals.UPPER_GOAL_CLIMB
		elif AIMethods.R.randi() % 2 != 0:
			s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] = Enums.UpperLevelGoals.UPPER_GOAL_CLING
		else:
			s.n_attrib[Enums.NAttrFrosh.ATTR_UPPER_LEVEL_GOAL] = Enums.UpperLevelGoals.UPPER_GOAL_SUPPORT


# === CONSUME (PIZZA/BEER) ===

static func ais_consume(s: TSprite, b_is_beer: bool = false) -> bool:
	var ss_tossed_objects_l: SpriteSet = ais_get_sprites_in_range(
		s.n_x - AIDefine.D_PIZZA_EATING_OFFSET_X - 3, s.n_y - 2,
		s.n_x - AIDefine.D_PIZZA_EATING_OFFSET_X + 3, s.n_y + 6, AIMethods.ss_tossed)
	var ss_tossed_objects_r: SpriteSet = ais_get_sprites_in_range(
		s.n_x + AIDefine.D_PIZZA_EATING_OFFSET_X - 3, s.n_y - 2,
		s.n_x + AIDefine.D_PIZZA_EATING_OFFSET_X + 3, s.n_y + 6, AIMethods.ss_tossed)

	var n_l: int = ss_tossed_objects_l.get_number_of_sprites()
	var n_r: int = ss_tossed_objects_r.get_number_of_sprites()

	if n_l + n_r > 0:
		if n_l > 0:
			s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = true
			if b_is_beer:
				ais_abandon_clark(ss_tossed_objects_l.get_sprite(0))
			else:
				ais_abandon_pizza(ss_tossed_objects_l.get_sprite(0))
			ss_tossed_objects_l.get_sprite(0).b_deleted = true
		else:
			s.b_attrib[Enums.BAttrFrosh.ATTR_LOOKING_LEFT] = false
			ss_tossed_objects_r.get_sprite(0).b_deleted = true
			if b_is_beer:
				ais_abandon_clark(ss_tossed_objects_r.get_sprite(0))
			else:
				ais_abandon_pizza(ss_tossed_objects_r.get_sprite(0))
		return true
	return false


static func ais_abandon_pizza(spr_pizza: TSprite) -> void:
	# Frosh stop chasing this pizza
	var n: int = AIMethods.ss_fr.get_number_of_sprites()
	var n_new_goal: int
	if 0 != AIMethods.R.randi() % 2:
		n_new_goal = Enums.Goals.GOAL_MINDLESS_WANDERING
	else:
		n_new_goal = Enums.Goals.GOAL_THINK

	for i in range(n):
		var tmp: TSprite = AIMethods.ss_fr.get_sprite(i)
		var n_goal: int = tmp.n_attrib[Enums.NAttrFrosh.ATTR_GOAL]
		var n_dest_x: int = tmp.n_dest_x
		var n_dest_y: int = tmp.n_dest_y

		if n_goal == Enums.Goals.GOAL_PIZZA and abs(n_dest_x - spr_pizza.n_x) == AIDefine.D_PIZZA_EATING_OFFSET_X and abs(n_dest_y - spr_pizza.n_y) < 3:
			AIMethods.ss_fr.get_sprite(i).n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY] = Enums.Personalities.PERS_HEAVYWEIGHT
			AIMethods.ss_fr.get_sprite(i).n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = n_new_goal
			AIMethods.ss_fr.get_sprite(i).n_dest_x = AIMethods.ss_fr.get_sprite(i).n_x
			AIMethods.ss_fr.get_sprite(i).n_dest_y = AIMethods.ss_fr.get_sprite(i).n_y


static func ais_abandon_clark(spr_clark: TSprite) -> void:
	# Frosh stop chasing this clark mug
	var n: int = AIMethods.ss_fr.get_number_of_sprites()
	var n_new_goal: int
	if 0 != AIMethods.R.randi() % 2:
		n_new_goal = Enums.Goals.GOAL_MINDLESS_WANDERING
	else:
		n_new_goal = Enums.Goals.GOAL_THINK

	for i in range(n):
		var tmp: TSprite = AIMethods.ss_fr.get_sprite(i)
		var n_goal: int = tmp.n_attrib[Enums.NAttrFrosh.ATTR_GOAL]
		var n_dest_x: int = tmp.n_dest_x
		var n_dest_y: int = tmp.n_dest_y

		if n_goal == Enums.Goals.GOAL_CLARK and abs(n_dest_x - spr_clark.n_x) == AIDefine.D_PIZZA_EATING_OFFSET_X and abs(n_dest_y - spr_clark.n_y) < 3:
			AIMethods.ss_fr.get_sprite(i).n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY] = Enums.Personalities.PERS_HEAVYWEIGHT
			AIMethods.ss_fr.get_sprite(i).n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = n_new_goal
			AIMethods.ss_fr.get_sprite(i).n_dest_x = AIMethods.ss_fr.get_sprite(i).n_x
			AIMethods.ss_fr.get_sprite(i).n_dest_y = AIMethods.ss_fr.get_sprite(i).n_y


# === MOSH ===

static func ais_start_a_mosh() -> void:
	if AIMethods.spr_alien != null:
		return
	# Send all of the frosh flying
	var n: int = AIMethods.ss_fr.get_number_of_sprites()

	AIMethods.l_sound[Enums.ASLList.LSND_MUSIC_SCOTLAND].play(SoundbankInfo.VOL_FULL)

	AICrowd.ais_set_frec_action(AIMethods.spr_frecs_l, Enums.CrowdActions.FA_STAYIN_ALIVE)
	AICrowd.ais_set_frec_action(AIMethods.spr_frecs_c, Enums.CrowdActions.FA_STAYIN_ALIVE)
	AICrowd.ais_set_frec_action(AIMethods.spr_frecs_r, Enums.CrowdActions.FA_STAYIN_ALIVE)

	for i in range(n):
		var frosh: TSprite = AIMethods.ss_fr.get_sprite(i)
		var n_temp: int = frosh.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR]
		if n_temp == 7 or n_temp == 10 or n_temp == 11:
			ais_send_frosh_flying(frosh)
			frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_MOSH
		else:
			frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_MOSH
			frosh.n_dest_x = frosh.n_x
			frosh.n_dest_y = frosh.n_y
			if frosh.n_z < 5:
				frosh.n_z = 0
				AIFrosh.ai_init_4(frosh)
			else:
				ais_send_frosh_flying(frosh)
		frosh.n_cc = 0


# === REDIRECT FUNCTIONS ===
# These redirect to the actual implementations in other files

static func ais_collision_to_response(projectile: TSprite, ss_targets: SpriteSet, response_func: Callable,
		b_include_whap: bool, b_pole_acts_as_shield: bool, b_hit_only_one: bool = true) -> bool:
	return AIProjectile.ais_collision_to_response(projectile, ss_targets, response_func, b_include_whap, b_pole_acts_as_shield, b_hit_only_one)


static func ais_change_arm() -> int:
	return AIProjectile.ais_change_arm()


static func ais_chase_alien(b_is_commie: bool) -> void:
	# Chase alien targets frosh at the alien sprite
	var n: int = AIMethods.ss_fr.get_number_of_sprites()

	for i in range(n):
		var frosh: TSprite = AIMethods.ss_fr.get_sprite(i)
		var n_motivation_level: int = frosh.n_attrib[Enums.NAttrFrosh.ATTR_MOTIVATION]

		# Check if frosh is in chasing mood
		if n_motivation_level == 2 \
				or (n_motivation_level == 1 and AIMethods.R.randi() % 4 == 0) \
				or (AIMethods.R.randi() % 10 == 0):

			var n_temp: int = frosh.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR]
			if n_motivation_level != 0 and AIMethods.R.randi() % 20 == 0:
				frosh.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] -= 1

			if n_temp == 7 or n_temp == 10 or n_temp == 11:
				if b_is_commie:
					frosh.n_dest_x = AIMethods.spr_alien.n_x - AIDefine.D_COMMIE_PUNCHING_OFFSET_X if AIMethods.spr_alien.n_x < frosh.n_x else AIMethods.spr_alien.n_x + AIDefine.D_COMMIE_PUNCHING_OFFSET_X
				else:
					frosh.n_dest_x = AIMethods.spr_alien.n_x - AIDefine.D_ARTSCI_SPLASHING_OFFSET_X if AIMethods.spr_alien.n_x < frosh.n_x else AIMethods.spr_alien.n_x + AIDefine.D_ARTSCI_SPLASHING_OFFSET_X
				frosh.n_dest_y = AIMethods.spr_alien.n_y + AIMethods.randintin(-3, 6)
				if frosh.nv_y < 3:
					frosh.nv_y = 3
				ais_send_frosh_flying(frosh)
				if Globals.myGameConditions.is_ritual():
					frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_COMMIE if b_is_commie else Enums.Goals.GOAL_ARTSCI
				else:
					frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_ARTSCI
			elif n_temp == 4:
				if b_is_commie:
					frosh.n_dest_x = AIMethods.spr_alien.n_x - AIDefine.D_COMMIE_PUNCHING_OFFSET_X if AIMethods.spr_alien.n_x < frosh.n_x else AIMethods.spr_alien.n_x + AIDefine.D_COMMIE_PUNCHING_OFFSET_X
				else:
					frosh.n_dest_x = AIMethods.spr_alien.n_x - AIDefine.D_ARTSCI_SPLASHING_OFFSET_X if AIMethods.spr_alien.n_x < frosh.n_x else AIMethods.spr_alien.n_x + AIDefine.D_ARTSCI_SPLASHING_OFFSET_X
				frosh.n_dest_y = AIMethods.spr_alien.n_y
				if Globals.myGameConditions.is_ritual():
					frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_COMMIE if b_is_commie else Enums.Goals.GOAL_ARTSCI
				else:
					frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_ARTSCI
				if frosh.nv_y < 3:
					frosh.nv_y = 3
				if frosh.n_z < 5:
					frosh.n_z = 0
					AIFrosh.ai_init_4(frosh)
				else:
					ais_send_frosh_flying(frosh)


# === REGROUP ===

static func ais_regroup() -> void:
	# Cause the thinking frosh to regroup
	var n: int = AIMethods.ss_fr.get_number_of_sprites()

	for i in range(n):
		var frosh: TSprite = AIMethods.ss_fr.get_sprite(i)
		if frosh.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR] == 6:
			frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_MINDLESS_WANDERING
			frosh.n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY] = Enums.Personalities.PERS_HEAVYWEIGHT
			AIFrosh.ai_init_4(frosh)


# === TOPPLE PYRAMID ===

static func ais_topple_pyramid() -> void:
	# Send all of the frosh flying
	Globals.myGameConditions.topple()
	if Globals.myGameConditions.get_noise_count() > 1000:
		Globals.myGameConditions.set_noise_count(4000)

	# The actual toppling stuff
	var n: int = AIMethods.ss_fr.get_number_of_sprites()
	AIMethods.s_sound[Enums.ASSList.SSND_EFFECTS_TOPPLE].play(SoundbankInfo.VOL_HOLLAR, 0)

	for i in range(n):
		var frosh: TSprite = AIMethods.ss_fr.get_sprite(i)
		var n_temp: int = frosh.n_attrib[Enums.NAttrFrosh.ATTR_BEHAVIOR]
		if n_temp == 7 or n_temp == 10 or n_temp == 11:
			ais_send_frosh_flying(frosh)
			# One in every ten will leap
			if 0 == AIMethods.R.randi() % 10:
				AIFrosh.ai_init_2(frosh)
				if frosh.n_x < AIDefine.D_POLE_X:
					frosh.nv_x = -frosh.nv_x
					frosh.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR2_1 + AIMethods.R.randi() % AIDefine.NSPR_FR2])
			# One in every six will look shocked
			elif 0 == AIMethods.R.randi() % 6:
				frosh.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR1_1 + AIMethods.R.randi() % AIDefine.NSPR_FR1])

			frosh.n_attrib[Enums.NAttrFrosh.ATTR_GOAL] = Enums.Goals.GOAL_THINK
			if 0 == AIMethods.R.randi() % 300 % 3:
				frosh.n_attrib[Enums.NAttrFrosh.ATTR_PERSONALITY] = Enums.Personalities.PERS_HEAVYWEIGHT

	# Decide if Pop Boy is coming in and usher him in if so
	var topple_threshold: int = AIDefine.TOPPLE_POPBOY_KEEN if Globals.myGameConditions.get_frosh_lameness() != 0 else AIDefine.TOPPLE_POPBOY_LAME
	if Globals.myGameConditions.count_topples() == topple_threshold:
		# Stop prez voices
		for voice_idx in range(Enums.ASLList.LSND_PREZ_HITAPPLE1, Enums.ASLList.LSND_PREZ_ENCOURAGE5):
			AIMethods.l_sound[voice_idx].stop()

		AIMethods.l_sound[Enums.ASLList.LSND_EXAM_TOSS1].stop()
		AIMethods.NOSPEECHFOR(160)

		# Stop more voices
		for voice_idx in range(Enums.ASLList.LSND_APPLES_OFFER1, Enums.ASLList.LSND_RING_ZAP3):
			AIMethods.l_sound[voice_idx].stop()

		AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
		AIMethods.l_sound[Enums.ASLList.LSND_PREZ_POPBOY1_1 + AIMethods.R.randi() % 3].play(
			SoundbankInfo.VOL_HOLLAR,
			(AIMethods.spr_prez.n_x - 320) / 32
		)
		ais_set_frec_action(AIMethods.spr_frecs_r, Enums.AttrCrowdActions.FA_PART)
		AIMethods.ss_skyline.include(SpriteInit.create_sprite(Enums.SpriteType.SPR_POP_BOY_IN_CROWD, 530, 0))
	else:
		if Globals.myGameConditions.is_pop_boy_in_pit() and (0 != AIMethods.R.randi() % 2) \
			and not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_EXAM1].is_playing() \
			and not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_EXAM2].is_playing() \
			and not AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_EXAM3].is_playing():
			# Pop Boy has his say
			AIMethods.l_sound[Enums.ASLList.LSND_POPBOY_EXAM1 + AIMethods.R.randi() % 3].play(SoundbankInfo.VOL_HOLLAR, 0)
		else:
			# George gets his say
			if not AIMethods.l_sound[Enums.ASLList.LSND_EXAM_TOSS1].is_playing() and AIMethods.SPEECHOK() \
				and AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] != 4 \
				and (0 != Globals.myGameConditions.count_topples() % 2):
				AIMethods.NOSPEECHFOR(120)
				AIMethods.spr_prez.n_attrib[Enums.AttrPrez.ATTR_PREZ_ACTION] = 4
				for voice_idx in range(Enums.ASLList.LSND_PREZ_HITAPPLE1, Enums.ASLList.LSND_PREZ_HITPIZZAR2):
					AIMethods.l_sound[voice_idx].stop()
				AIMethods.l_sound[Enums.ASLList.LSND_PREZ_ENCOURAGE1 + (((Globals.myGameConditions.count_topples() - 1) / 2) % 5)].play(SoundbankInfo.VOL_HOLLAR, (AIMethods.spr_prez.n_x - 320) / 32)


# === THINK FOR AL ===

static func ais_think_for_al(s: TSprite, b_circle: bool = true) -> void:
	# Cause the frosh to think
	var n: int = AIMethods.ss_fr.get_number_of_sprites()

	for i in range(n):
		var spr_tmp: TSprite = AIMethods.ss_fr.get_sprite(i)
		if spr_tmp.n_z < 2:
			AIFrosh.ai_init_6d(spr_tmp)
			if b_circle:
				var n_tmp: int = abs(s.n_y - spr_tmp.n_dest_y) / 8
				if n_tmp > 7:
					n_tmp = 7
				if spr_tmp.n_dest_x < s.n_x:
					spr_tmp.n_dest_x = s.n_x - int(sqrt(6400 - 128 * n_tmp * n_tmp)) - AIMethods.R.randi() % 100
				else:
					spr_tmp.n_dest_x = s.n_x + int(sqrt(6400 - 128 * n_tmp * n_tmp)) + AIMethods.R.randi() % 100
		else:
			AIFrosh.ai_init_2(spr_tmp)
			if spr_tmp.n_x < AIDefine.D_POLE_X:
				spr_tmp.nv_x = -spr_tmp.nv_x
				spr_tmp.set_frame(AIMethods.frm_m[Enums.GameBitmapEnumeration.bmpFR2_1 + AIMethods.R.randi() % AIDefine.NSPR_FR2])
