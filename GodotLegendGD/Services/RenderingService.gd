extends Node2D



# Internal command class for buffering render calls
class RenderCommand:
	var frame: FrameDesc
	var x: int
	var y: int
	var use_active_material: bool
	var replace_color: Color
	var substitute_color: Color
	var tolerance: float
	var is_text: bool
	var text: String
	var color: Color
	var is_super_front: bool  # For jacket slam - draws on top of text

	func _init(p_frame, p_x, p_y, p_use_mat, p_rep, p_sub, p_tol, p_is_text, p_text, p_color, p_super_front: bool = false):
		frame = p_frame
		x = p_x
		y = p_y
		use_active_material = p_use_mat
		replace_color = p_rep
		substitute_color = p_sub
		tolerance = p_tol
		is_text = p_is_text
		text = p_text
		color = p_color
		is_super_front = p_super_front

var _textures = {}
var _bitmap_sets_loaded = {}
var _render_commands = []
var _color_replace_shader: Shader
var _color_replace_materials = {} # Key: Array [r,g,b,r,g,b], Value: ShaderMaterial
var _default_font: Font
var _assets_path = "res://Assets/Graphics/"
var _missing_textures_logged = {}  # Track which missing textures we've already warned about

func _ready():
	Globals.RenderingService = self
	_color_replace_shader = load("res://Shaders/color_replace.gdshader")
	_default_font = ThemeDB.fallback_font
	print("[RenderingService] Initialized and Shader loaded")

func load_bitmap_set(bitmap_set: int, percentage_to_load: int = 100) -> bool:
	if _bitmap_sets_loaded.has(bitmap_set) and _bitmap_sets_loaded[bitmap_set]:
		return true
		
	print("[RenderingService] load_bitmap_set: ", bitmap_set)
	
	var result = BitmapRegistry.get_bitmap_load_info(bitmap_set)
	var frame_number = result["frame_number"]
	var info_list = result["info_list"]
	
	for i in range(info_list.size()):
		var info = info_list[i]
		var index = i + frame_number
		
		# Initialize normal frame
		var frm = Globals.frames[index]
		frm.init_frame(info.bitmap_name, info.hotspot_x, info.hotspot_y, 
			info.size_x1, info.size_y1, info.size_x2, info.size_y2, false)
		frm.bitmap_width = info.width
		frm.bitmap_height = info.height
		
		# Initialize mirrored frame
		var mirrored_hotspot_x = info.width - info.hotspot_x
		var frm_m = Globals.frames_mirror[index]
		frm_m.init_frame(info.bitmap_name, mirrored_hotspot_x, info.hotspot_y,
			info.size_x1, info.size_y1, info.size_x2, info.size_y2, true, frm)
		frm_m.bitmap_width = info.width
		frm_m.bitmap_height = info.height
		
		# Preload texture
		_load_texture(info.bitmap_name)
		
	_bitmap_sets_loaded[bitmap_set] = true
	return true

func _load_texture(bitmap_name: String) -> Texture2D:
	if _textures.has(bitmap_name):
		return _textures[bitmap_name]

	# DEBUG: Log empty bitmap names
	if bitmap_name == "":
		print("[RenderingService] WARNING: Empty bitmap_name requested!")
		return null

	var file_path = _assets_path + bitmap_name + ".png"
	if ResourceLoader.exists(file_path):
		var tex = load(file_path)
		_textures[bitmap_name] = tex
		return tex
	else:
		# Only log once per missing texture to avoid console spam
		if not _missing_textures_logged.has(bitmap_name):
			_missing_textures_logged[bitmap_name] = true
			print("[RenderingService] Texture not found: ", file_path)
		return null

func begin_frame():
	_render_commands.clear()

func end_frame():
	queue_redraw()

func draw_bitmap(frame: FrameDesc, x: int, y: int, use_color_swap: bool = false, replace_rgb: Color = Color.BLACK, substitute_rgb: Color = Color.BLACK):
	if frame == null:
		return

	# Check if color replacement is actually needed (C# logic: non-black replace color and colors differ)
	var needs_color_replace: bool = false
	if use_color_swap:
		var has_replace_color: bool = replace_rgb.r8 != 0 or replace_rgb.g8 != 0 or replace_rgb.b8 != 0
		var colors_differ: bool = replace_rgb.r8 != substitute_rgb.r8 or replace_rgb.g8 != substitute_rgb.g8 or replace_rgb.b8 != substitute_rgb.b8
		needs_color_replace = has_replace_color and colors_differ

	var cmd = RenderCommand.new(frame, x, y, needs_color_replace, replace_rgb, substitute_rgb, 0.02, false, "", Color.WHITE)
	_render_commands.append(cmd)

func draw_bitmap_super_front(frame: FrameDesc, x: int, y: int):
	# Draw on top of everything including text (used for jacket slam transition)
	if frame == null:
		return

	var cmd = RenderCommand.new(frame, x, y, false, Color.BLACK, Color.BLACK, 0.0, false, "", Color.WHITE, true)
	_render_commands.append(cmd)

func draw_text(x: int, y: int, text: String, color: Color):
	var cmd = RenderCommand.new(null, x, y, false, Color.BLACK, Color.BLACK, 0.0, true, text, color)
	_render_commands.append(cmd)

func _exit_tree():
	# Clean up pooled canvas items
	for rid in _canvas_item_pool:
		if rid.is_valid():
			RenderingServer.free_rid(rid)
	# Clean up legacy shader canvas items (if any)
	for rid in _shader_canvas_items.values():
		if rid.is_valid():
			RenderingServer.free_rid(rid)
	if _text_canvas_item.is_valid():
		RenderingServer.free_rid(_text_canvas_item)
	if _super_front_canvas_item.is_valid():
		RenderingServer.free_rid(_super_front_canvas_item)

func _draw():
	# Reset pool index for new frame
	_pool_index = 0

	# Clear text canvas item
	var text_rid = _get_text_canvas_item()
	RenderingServer.canvas_item_clear(text_rid)

	# Clear super front canvas item (jacket slam)
	var super_front_rid = _get_super_front_canvas_item()
	RenderingServer.canvas_item_clear(super_front_rid)

	if _render_commands.size() == 0:
		return

	# Process commands using C# batching approach with DrawIndex
	var cmd_index: int = 0
	while cmd_index < _render_commands.size():
		var first_cmd = _render_commands[cmd_index]

		if first_cmd.is_text:
			# TEXT: batch consecutive text commands
			while cmd_index < _render_commands.size():
				var cmd = _render_commands[cmd_index]
				if not cmd.is_text:
					break
				_draw_text_to_canvas(text_rid, cmd.x, cmd.y, cmd.text, cmd.color)
				cmd_index += 1
		elif first_cmd.is_super_front:
			# SUPER FRONT: draw to super front canvas
			var frame = first_cmd.frame
			var tex = _load_texture(frame.bitmap_name)
			if tex:
				var dest_pos = Vector2(first_cmd.x, first_cmd.y)
				_draw_to_canvas_item(super_front_rid, tex, dest_pos, frame)
			cmd_index += 1
		else:
			# SPRITE: batch by material (matching C# logic)
			var batch_material: ShaderMaterial = null
			if first_cmd.use_active_material:
				batch_material = _get_color_replace_material(first_cmd.replace_color, first_cmd.substitute_color)

			# Get pooled canvas item for this batch
			var batch_rid = _get_pooled_canvas_item()

			# Configure material and draw index
			if batch_material:
				RenderingServer.canvas_item_set_material(batch_rid, batch_material.get_rid())
			else:
				RenderingServer.canvas_item_set_material(batch_rid, RID())
			RenderingServer.canvas_item_set_draw_index(batch_rid, _pool_index)

			# Add all consecutive sprite commands that share this material
			while cmd_index < _render_commands.size():
				var cmd = _render_commands[cmd_index]

				if cmd.is_text or cmd.is_super_front:
					break  # Different command type

				# Check material match
				var cmd_material: ShaderMaterial = null
				if cmd.use_active_material:
					cmd_material = _get_color_replace_material(cmd.replace_color, cmd.substitute_color)

				if cmd_material != batch_material:
					break  # Material changed

				# Add to batch
				var frame = cmd.frame
				var tex = _load_texture(frame.bitmap_name)

				if tex:
					var dest_pos = Vector2(cmd.x, cmd.y)
					_draw_sprite_to_canvas(batch_rid, tex, dest_pos, frame)

				cmd_index += 1

	# Hide unused pool items
	for i in range(_pool_index, _canvas_item_pool.size()):
		RenderingServer.canvas_item_clear(_canvas_item_pool[i])

func _draw_sprite_to_canvas(rid: RID, tex: Texture2D, pos: Vector2, frame: FrameDesc):
	# Draw sprite to a specific canvas item RID
	var src_rect = Rect2(0, 0, tex.get_width(), tex.get_height())
	var target_rect: Rect2

	if frame.is_mirror:
		# Match C# logic: use pos directly with negative width to flip
		# The mirrored hotspot is already calculated correctly in load_bitmap_set
		target_rect = Rect2(pos, Vector2(-src_rect.size.x, src_rect.size.y))
	else:
		target_rect = Rect2(pos, src_rect.size)

	RenderingServer.canvas_item_add_texture_rect(rid, target_rect, tex.get_rid())

# Helper to manage shader drawing
func _get_color_replace_material(rep: Color, sub: Color) -> ShaderMaterial:
	var key = [rep.r8, rep.g8, rep.b8, sub.r8, sub.g8, sub.b8]
	if _color_replace_materials.has(key):
		return _color_replace_materials[key]
	
	var mat = ShaderMaterial.new()
	mat.shader = _color_replace_shader
	mat.set_shader_parameter("replace_color", rep)
	mat.set_shader_parameter("substitute_color", sub)
	mat.set_shader_parameter("tolerance", 0.02)
	_color_replace_materials[key] = mat
	return mat

func _draw_to_canvas_item(rid: RID, tex: Texture2D, pos: Vector2, frame: FrameDesc):
	# Draw to a specific canvas item RID (used for super_front)
	var src_rect = Rect2(0, 0, tex.get_width(), tex.get_height())
	var target_rect: Rect2

	if frame.is_mirror:
		# Match C# logic: use pos directly with negative width to flip
		target_rect = Rect2(pos, Vector2(-src_rect.size.x, src_rect.size.y))
	else:
		target_rect = Rect2(pos, src_rect.size)

	RenderingServer.canvas_item_add_texture_rect(rid, target_rect, tex.get_rid())

var _shader_canvas_items = {} # Map[ShaderMaterial, RID] - legacy, keeping for cleanup
var _text_canvas_item: RID  # Dedicated canvas item for text, always on top
var _super_front_canvas_item: RID  # Dedicated canvas item for jacket slam, on top of everything

# Canvas item pool (matching C# approach)
var _canvas_item_pool: Array[RID] = []
var _pool_index: int = 0

func _get_pooled_canvas_item() -> RID:
	var rid: RID
	if _pool_index < _canvas_item_pool.size():
		rid = _canvas_item_pool[_pool_index]
		RenderingServer.canvas_item_clear(rid)
	else:
		rid = RenderingServer.canvas_item_create()
		RenderingServer.canvas_item_set_parent(rid, get_canvas_item())
		_canvas_item_pool.append(rid)
	_pool_index += 1
	return rid

func _get_canvas_item_for_material(material: ShaderMaterial) -> RID:
	if _shader_canvas_items.has(material):
		return _shader_canvas_items[material]

	var rid = RenderingServer.canvas_item_create()
	RenderingServer.canvas_item_set_parent(rid, get_canvas_item())
	RenderingServer.canvas_item_set_material(rid, material.get_rid())
	# Set z-index lower so text canvas item (z=100) is always on top
	RenderingServer.canvas_item_set_z_index(rid, 0)
	_shader_canvas_items[material] = rid
	return rid

func _get_text_canvas_item() -> RID:
	if _text_canvas_item.is_valid():
		return _text_canvas_item

	_text_canvas_item = RenderingServer.canvas_item_create()
	RenderingServer.canvas_item_set_parent(_text_canvas_item, get_canvas_item())
	# Set high z-index so text is always on top of sprite canvas items
	RenderingServer.canvas_item_set_z_index(_text_canvas_item, 100)
	# High draw index to ensure text draws after sprites
	RenderingServer.canvas_item_set_draw_index(_text_canvas_item, 10000)
	return _text_canvas_item

func _get_super_front_canvas_item() -> RID:
	if _super_front_canvas_item.is_valid():
		return _super_front_canvas_item

	_super_front_canvas_item = RenderingServer.canvas_item_create()
	RenderingServer.canvas_item_set_parent(_super_front_canvas_item, get_canvas_item())
	# Set highest z-index so jacket slam is on top of everything including text
	RenderingServer.canvas_item_set_z_index(_super_front_canvas_item, 200)
	# Highest draw index to ensure super_front draws after everything
	RenderingServer.canvas_item_set_draw_index(_super_front_canvas_item, 20000)
	return _super_front_canvas_item

func _draw_text_to_canvas(rid: RID, x: int, y: int, text: String, color: Color):
	# Use RenderingServer to draw text to a specific canvas item
	var font_rid = _default_font.get_rids()[0] if _default_font.get_rids().size() > 0 else RID()
	if not font_rid.is_valid():
		# Fallback: try to get font from ThemeDB
		font_rid = ThemeDB.fallback_font.get_rids()[0]

	# Draw text using font rendering
	# Font.draw_string takes a canvas_item RID as first parameter
	# Add 14 to Y to match C# version (compensates for baseline positioning)
	# C# uses fontSize 13 for non-bold, 14 for bold - we use 13 as default
	var font_size: int = 13
	_default_font.draw_string(rid, Vector2(x, y + 14), text, HORIZONTAL_ALIGNMENT_LEFT, -1, font_size, color)


		
# Removed _process - canvas item clearing is now handled in _draw() with pooled approach
