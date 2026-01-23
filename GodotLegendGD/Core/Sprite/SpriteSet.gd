class_name SpriteSet

const MAX_SPRITES_PER_SET = 600
const SS_DO_NOT_DELETE = true

var sprites: Array[TSprite] = []
var n: int = 0
var my_layer_index: int = -1  # Layer index for dynamic offset lookup (-1 = no layer, use 0)

func _init(layer_index: int = -1):
	my_layer_index = layer_index
	sprites.resize(MAX_SPRITES_PER_SET) # Pre-allocation for fixed size behavior matching C#
	n = 0

func set_layer(layer_index: int):
	my_layer_index = layer_index

func get_layer_y() -> int:
	# Dynamically get the current layer offset from Globals.myLayers
	if my_layer_index < 0:
		return 0
	return Globals.myLayers.get_offset(my_layer_index)

func include(sprite: TSprite):
	if n >= MAX_SPRITES_PER_SET:
		print("[SpriteSet] Too many sprites!")
	else:
		sprites[n] = sprite
		n += 1

func get_sprite(i: int) -> TSprite:
	return sprites[i]

func remove(sprite: TSprite):
	var i = 0
	while i < n:
		if sprites[i] == sprite:
			n -= 1
			while i < n:
				sprites[i] = sprites[i + 1]
				i += 1
		else:
			i += 1

func remove_all():
	n = 0

func flush(do_not_delete: bool = false):
	if do_not_delete:
		n = 0
	else:
		for i in range(n):
			sprites[i].b_deleted = true
		compact()

func compact():
	var n_free = 0
	for n_test in range(n):
		if sprites[n_test].b_deleted:
			# Dispose sprite
			sprites[n_test].dispose()
			sprites[n_test] = null # Clear ref
		else:
			sprites[n_free] = sprites[n_test]
			n_free += 1
	n = n_free

func bubble_sort_y():
	var temp: TSprite
	for i in range(n):
		for j in range(i + 1, n):
			if sprites[i].n_y > sprites[j].n_y:
				temp = sprites[i]
				sprites[i] = sprites[j]
				sprites[j] = temp
			elif sprites[i].n_y == sprites[j].n_y:
				if sprites[i].n_z < sprites[j].n_z:
					temp = sprites[i]
					sprites[i] = sprites[j]
					sprites[j] = temp
				elif sprites[i].n_z == sprites[j].n_z:
					if sprites[i].n_tag > sprites[j].n_tag:
						temp = sprites[i]
						sprites[i] = sprites[j]
						sprites[j] = temp

func order_by_y():
	bubble_sort_y()


func bubble_sort_x():
	var temp: TSprite
	for i in range(n):
		for j in range(i + 1, n):
			if sprites[i].n_x > sprites[j].n_x:
				temp = sprites[i]
				sprites[i] = sprites[j]
				sprites[j] = temp


func sort_by_x():
	bubble_sort_x()

func think():
	var n_temp = n
	for i in range(n_temp):
		if sprites[i] != null and not sprites[i].b_deleted:
			sprites[i].think()

func calculate_screen_coordinates():
	var layer_y = get_layer_y()
	for i in range(n):
		if sprites[i] != null and not sprites[i].b_deleted:
			sprites[i].calculate_screen_coordinates(layer_y)

func draw():
	for i in range(n):
		if sprites[i] != null and not sprites[i].b_deleted:
			sprites[i].draw()


func get_number_of_sprites() -> int:
	return n


# === BOUNDING BOX QUERIES (for collision/range detection) ===

func get_left_most_scr_point_on_sprite(i: int) -> int:
	var s: TSprite = sprites[i]
	if s.frm_frame == null:
		return s.n_scr_x
	return s.n_scr_x + s.frm_frame.hotspot_x - s.frm_frame.n_x1


func get_right_most_scr_point_on_sprite(i: int) -> int:
	var s: TSprite = sprites[i]
	if s.frm_frame == null:
		return s.n_scr_x
	return s.n_scr_x + s.frm_frame.hotspot_x + s.frm_frame.n_x2


func get_top_most_scr_point_on_sprite(i: int) -> int:
	var s: TSprite = sprites[i]
	if s.frm_frame == null:
		return s.n_scr_y
	return s.n_scr_y + s.frm_frame.hotspot_y - s.frm_frame.n_z1


func get_bottom_most_scr_point_on_sprite(i: int) -> int:
	var s: TSprite = sprites[i]
	if s.frm_frame == null:
		return s.n_scr_y
	return s.n_scr_y + s.frm_frame.hotspot_y + s.frm_frame.n_z2


# World-space versions (using n_x, n_y instead of screen coords)
func get_left_most_point_on_sprite(i: int) -> int:
	var s: TSprite = sprites[i]
	if s.frm_frame == null:
		return s.n_x
	return s.n_x - s.frm_frame.n_x1


func get_right_most_point_on_sprite(i: int) -> int:
	var s: TSprite = sprites[i]
	if s.frm_frame == null:
		return s.n_x
	return s.n_x + s.frm_frame.n_x2


func get_top_most_point_on_sprite(i: int) -> int:
	var s: TSprite = sprites[i]
	if s.frm_frame == null:
		return s.n_y
	return s.n_y - s.frm_frame.n_z1


func get_bottom_most_point_on_sprite(i: int) -> int:
	var s: TSprite = sprites[i]
	if s.frm_frame == null:
		return s.n_y
	return s.n_y + s.frm_frame.n_z2
