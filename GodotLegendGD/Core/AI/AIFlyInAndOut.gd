class_name AIFlyInAndOut

# AIFlyInAndOut.gd - Static class with fly-in-and-out animation functions
# Ported from aiMenuAndDisplay.cs (the aiInitFlyInAndOut functions)
#
# Per porting_rules.md:
# - This is a static class with logic functions only
# - All shared state accessed via AIMethods autoload

# === FLY IN AND OUT ANIMATION (Type 1) ===
# Uses deceleration as sprite approaches destination

static func ai_fly_in_and_out(s: TSprite) -> void:
	AISupport.ais_move_towards_destination(s)

	if abs(s.n_x - s.n_dest_x) <= AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE:
		s.n_x = s.n_dest_x
		s.nv_x = 0
	else:
		s.nv_x = int(s.nv_x * AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_NUMERATOR / AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_DENOMINATOR)
		if abs(s.nv_x) < AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE:
			if s.nv_x > 0:
				s.nv_x = AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE
			else:
				s.nv_x = -AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE

	if abs(s.n_y - s.n_dest_y) <= AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE:
		s.n_y = s.n_dest_y
		s.nv_y = 0
	else:
		s.nv_y = int(s.nv_y * AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_NUMERATOR / AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_DENOMINATOR)
		if abs(s.nv_y) < AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE:
			if s.nv_y > 0:
				s.nv_y = AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE
			else:
				s.nv_y = -AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE

	if s.n_x == s.n_dest_x and s.n_y == s.n_dest_y:
		s.switch_to_secondary_behavior()


static func ai_init_fly_in_and_out(s: TSprite, behavior: Callable, start_x: int, start_y: int,
		dest_x: int, dest_y: int, alpha_x: int, alpha_y: int) -> void:
	var n: int = 23  # Magic number - preserved from C#
	var increment: float = float(AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_NUMERATOR) / float(AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_DENOMINATOR)

	var d_x: int = abs(start_x - dest_x)
	var t1_x: float = 0.0
	if d_x != 0:
		t1_x = float(alpha_x)
		for i in range(n - 1):
			t1_x *= (1.0 / increment)
		if start_x < dest_x:
			d_x = -int(t1_x / (1.0 - increment))
		else:
			d_x = int(t1_x / (1.0 - increment))

	var d_y: int = abs(start_y - dest_y)
	var t1_y: float = 0.0
	if d_y != 0:
		t1_y = float(alpha_y)
		for i in range(n - 1):
			t1_y *= (1.0 / increment)
		if start_y < dest_y:
			d_y = -int(t1_y / (1.0 - increment))
		else:
			d_y = int(t1_y / (1.0 - increment))

	s.n_x = dest_x + d_x
	s.n_y = dest_y + d_y
	s.nv_x = int(t1_x)
	s.nv_y = int(t1_y)
	s.n_dest_x = dest_x
	s.n_dest_y = dest_y

	s.set_secondary_behavior(behavior)
	s.set_behavior(Callable(AIFlyInAndOut, "ai_fly_in_and_out"))


# === FLY IN AND OUT ANIMATION (Type 2) ===
# Uses different deceleration curve

static func ai_fly_in_and_out_2(s: TSprite) -> void:
	AISupport.ais_move_towards_destination(s)

	if abs(s.n_x - s.n_dest_x) <= AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE:
		s.n_x = s.n_dest_x
		s.nv_x = 0
	else:
		s.nv_x = int(s.nv_x * (AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_DENOMINATOR - 1) / AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_DENOMINATOR)
		if abs(s.nv_x) < AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE:
			s.nv_x = AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE

	if abs(s.n_y - s.n_dest_y) <= AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE:
		s.n_y = s.n_dest_y
		s.nv_y = 0
	else:
		s.nv_y = int(s.nv_y * (AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_DENOMINATOR - 1) / AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_DENOMINATOR)
		if abs(s.nv_y) < AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE:
			s.nv_y = AIDefine.AI_FLY_IN_AND_OUT_SQUEEZE_DISTANCE

	if s.n_x == s.n_dest_x and s.n_y == s.n_dest_y:
		s.switch_to_secondary_behavior()


static func ai_init_fly_in_and_out_2(s: TSprite, behavior: Callable, start_x: int, start_y: int,
		dest_x: int, dest_y: int, alpha_x: int, alpha_y: int) -> void:
	s.nv_x = abs(dest_x - start_x) / AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_DENOMINATOR
	s.nv_y = abs(dest_y - start_y) / AIDefine.AI_FLY_IN_AND_OUT_INCREMENT_DENOMINATOR

	s.n_x = start_x
	s.n_y = start_y
	s.n_dest_x = dest_x
	s.n_dest_y = dest_y

	s.set_secondary_behavior(behavior)
	s.set_behavior(Callable(AIFlyInAndOut, "ai_fly_in_and_out_2"))
