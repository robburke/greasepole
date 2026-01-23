extends Node2D

var game_loop: GameLoop

func _ready():
	print("[WindowWrapper] Starting GameLoop...")

	# Load shared bitmaps (toggle buttons, digits, mouse cursors, etc.)
	Globals.RenderingService.load_bitmap_set(Enums.BitmapSets.bmpLoadBitmaps)

	game_loop = GameLoop.new()
	# Use set_game_state for initial startup (no jacket slam transition)
	game_loop.set_game_state(Enums.GameStates.STATETITLE)

func _process(delta):
	if game_loop:
		# Update timer with this frame's delta
		Globals.TimerService.set_delta(delta)

		# Process AI using fixed timestep (25 FPS / 40ms per game frame)
		# This may run 0, 1, or more AI iterations depending on accumulated time
		game_loop.process_ai()

		# Render happens once per Godot frame (at display refresh rate)
		game_loop.render_frame()
