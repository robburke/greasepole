class_name FrameDesc

var bitmap_name: String
var hotspot_x: int
var hotspot_y: int
# Bounding box offsets from hotspot (matching C# naming)
var n_x1: int  # Left offset
var n_z1: int  # Top offset (Z is vertical in pit coords)
var n_x2: int  # Right offset
var n_z2: int  # Bottom offset
# Aliases for compatibility
var size_x1: int:
	get: return n_x1
	set(v): n_x1 = v
var size_y1: int:
	get: return n_z1
	set(v): n_z1 = v
var size_x2: int:
	get: return n_x2
	set(v): n_x2 = v
var size_y2: int:
	get: return n_z2
	set(v): n_z2 = v
var is_mirror: bool
var mirror_source: FrameDesc = null
var bitmap_width: int
var bitmap_height: int

func _init():
	pass

func init_frame(name: String, hx: int, hy: int, sx1: int, sy1: int, sx2: int, sy2: int, mirror: bool, source: FrameDesc = null):
	bitmap_name = name
	hotspot_x = hx
	hotspot_y = hy
	n_x1 = sx1
	n_z1 = sy1
	n_x2 = sx2
	n_z2 = sy2
	is_mirror = mirror
	mirror_source = source
