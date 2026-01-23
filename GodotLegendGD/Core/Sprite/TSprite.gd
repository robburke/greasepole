class_name TSprite

# Enums
enum SpriteTextType { None, Small, Large }

# Coordinates
var n_x: int = 0
var n_y: int = 0
var n_z: int = 0
var n_scr_x: int = 0
var n_scr_y: int = 0
var n_dest_x: int = 0
var n_dest_y: int = 0
var n_dest_z: int = 0

# Velocity
var nv_x: int = 0
var nv_y: int = 0
var nv_z: int = 0

# Visuals
var frm_frame: FrameDesc = null
var replace_color: Color = Color.BLACK
var substitute_color: Color = Color.BLACK
var use_color_swap: bool = true

# RGB arrays for color replacement (C# compatibility)
# Setting these will update the Color values
var replace_rgb: Array:
	get:
		return [int(replace_color.r8), int(replace_color.g8), int(replace_color.b8)]
	set(value):
		if value != null and value.size() >= 3:
			replace_color = Color8(value[0], value[1], value[2])

var substitute_rgb: Array:
	get:
		return [int(substitute_color.r8), int(substitute_color.g8), int(substitute_color.b8)]
	set(value):
		if value != null and value.size() >= 3:
			substitute_color = Color8(value[0], value[1], value[2])
var n_r: int = 0
var n_g: int = 0
var n_b: int = 0
var n_a: int = 255

# Logic / AI
var pf_ai: Callable
var pf_ai_secondary: Callable
var n_cc: int = 0 # Cycle counter
var n_tag: int = 0
var sprite_type: int = 0  # SpriteType enum value
var b_deleted: bool = false
var b_super_front: bool = false  # Draw on top of everything including text (jacket slam)

# Attributes
var n_attrib: Array = [0, 0, 0, 0, 0, 0, 0, 0, 0, 0]
var b_attrib: Array = [false, false, false, false, false, false]

# Text
var sprite_text: int = SpriteTextType.None
var text: String = ""

# References
var pp_chosen = null # PolePosition placeholder

func _init():
	pass

func dispose():
	pp_abandon()

func set_frame(new_frame: FrameDesc):
	frm_frame = new_frame

func pp_abandon():
	if pp_chosen != null:
		if pp_chosen.get_claimer() == self:
			pp_chosen.release_claim()
			pp_chosen = null

func set_behavior(new_behavior: Callable, keep_position: bool = false):
	pf_ai = new_behavior
	if not keep_position:
		pp_abandon()

func set_secondary_behavior(new_behavior: Callable):
	pf_ai_secondary = new_behavior

func switch_to_secondary_behavior():
	var temp = pf_ai_secondary
	pf_ai_secondary = pf_ai
	pf_ai = temp

func set_goal(new_goal: int):
	n_attrib[1] = new_goal

func calculate_screen_coordinates(viewport_y: int):
	if frm_frame:
		n_scr_x = n_x - frm_frame.hotspot_x
		n_scr_y = n_y + viewport_y - frm_frame.hotspot_y - n_z

func draw():
	if sprite_text == SpriteTextType.None:
		if frm_frame:
			if b_super_front:
				# Draw on top of everything including text (jacket slam)
				Globals.RenderingService.draw_bitmap_super_front(frm_frame, n_scr_x, n_scr_y)
			else:
				# Verify colors are Color objects
				Globals.RenderingService.draw_bitmap(frm_frame, n_scr_x, n_scr_y, use_color_swap, replace_color, substitute_color)
		else:
			pass
			# print("[TSprite] Frame is null for sprite tag: ", n_tag)
	else:
		# Draw Text
		var color = Color8(n_r, n_g, n_b, n_a)
		Globals.RenderingService.draw_text(n_scr_x, n_scr_y, text, color)

func think():
	n_cc += 1
	if pf_ai.is_valid():
		pf_ai.call(self)
