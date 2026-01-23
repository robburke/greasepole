extends Node

# SoundService.gd - Audio playback service
# Ported from GodotSoundService.cs and GodotSound.cs

const SOUND_PATH: String = "res://Assets/Sound/"
var _short_sounds_loaded: bool = false
var _long_sounds_loaded_count: int = 0


func _ready():
	Globals.SoundService = self
	print("[SoundService] Initialized")
	_initialize_stub_sounds()

	# Load actual sounds after stubs are set up
	# This replaces stubs with real GodotSound instances
	if not AIMethods.NO_SOUND:
		print("[SoundService] Loading sounds...")
		load_short_sounds()
		load_long_sounds(100)  # Load all long sounds
		print("[SoundService] Sound loading complete")


func _initialize_stub_sounds():
	# Initialize all sound slots with stub sounds
	for i in range(AIMethods.MAX_SSOUNDS):
		AIMethods.s_sound[i] = StubSound.new()

	for i in range(AIMethods.MAX_LSOUNDS):
		AIMethods.l_sound[i] = StubSound.new()


func load_short_sounds() -> bool:
	if _short_sounds_loaded:
		return true

	print("[SoundService] LoadShortSounds - loading actual sounds")
	var loaded_count: int = 0
	var failed_count: int = 0

	# Load each short sound based on the ASSList enum
	for sound_enum in Enums.ASSList.values():
		var index: int = sound_enum
		var sound_name: String = _get_short_sound_filename(sound_enum)
		var full_path: String = SOUND_PATH + sound_name + ".mp3"

		if ResourceLoader.exists(full_path):
			AIMethods.s_sound[index] = GodotSound.new(self, full_path)
			loaded_count += 1
		else:
			# Keep stub for missing sounds
			push_error("[SoundService] Short sound not found: " + full_path)
			failed_count += 1

	print("[SoundService] Short sounds: %d loaded, %d failed" % [loaded_count, failed_count])
	_short_sounds_loaded = true
	return true


func load_long_sounds(percentage_to_load: int) -> bool:
	# Calculate how many sounds to load based on percentage
	var total_sounds: int = Enums.ASLList.size()
	var target_count: int = int(total_sounds * percentage_to_load / 100.0)

	# If we've already loaded the target percentage, return true to proceed
	if _long_sounds_loaded_count >= target_count:
		return true

	print("[SoundService] LoadLongSounds: loading %d to %d of %d" % [_long_sounds_loaded_count, target_count, total_sounds])
	var loaded_count: int = 0
	var failed_count: int = 0

	# Load sounds up to the target count
	for sound_enum in Enums.ASLList.values():
		var index: int = sound_enum
		if index >= target_count:
			break
		if index < _long_sounds_loaded_count:
			continue  # Already loaded

		var sound_name: String = _get_long_sound_filename(sound_enum)
		var full_path: String = SOUND_PATH + sound_name + ".mp3"

		if ResourceLoader.exists(full_path):
			AIMethods.l_sound[index] = GodotSound.new(self, full_path)
			loaded_count += 1
		else:
			# Keep stub for missing sounds
			push_error("[SoundService] Long sound not found: " + full_path)
			failed_count += 1

		_long_sounds_loaded_count = index + 1

	print("[SoundService] Long sounds: %d loaded, %d failed" % [loaded_count, failed_count])
	return _long_sounds_loaded_count >= target_count


func shut_up():
	print("[SoundService] ShutUp - stopping all sounds")

	# Stop all short sounds
	for i in range(AIMethods.MAX_SSOUNDS):
		if AIMethods.s_sound[i] != null:
			AIMethods.s_sound[i].stop()

	# Stop all long sounds
	for i in range(AIMethods.MAX_LSOUNDS):
		if AIMethods.l_sound[i] != null:
			AIMethods.l_sound[i].stop()


func _get_short_sound_filename(sound_enum: int) -> String:
	# Enum name like "SSND_EFFECTS_ACHIEVEMENTUNLOCKED" -> "EFFECTS_ACHIEVEMENTUNLOCKED"
	var name: String = Enums.ASSList.keys()[sound_enum]
	if name.begins_with("SSND_"):
		return name.substr(5)
	return name


func _get_long_sound_filename(sound_enum: int) -> String:
	# Enum name like "LSND_APPLES_OFFER1" -> "APPLES_OFFER1"
	var name: String = Enums.ASLList.keys()[sound_enum]
	if name.begins_with("LSND_"):
		return name.substr(5)
	return name


# === STUB SOUND CLASS ===
# Placeholder that does nothing - used before sounds are loaded
class StubSound:
	func play(_volume: int = 100, _pan: int = 0) -> void:
		pass

	func loop(_volume: int = 100) -> void:
		pass

	func stop() -> void:
		pass

	func is_playing() -> bool:
		return false


# === GODOT SOUND CLASS ===
# Actual sound implementation using AudioStreamPlayer
class GodotSound:
	var _player: AudioStreamPlayer
	var _stream: AudioStream
	var _sound_path: String
	var _is_loaded: bool = false
	var _is_looping: bool = false

	func _init(parent: Node, sound_path: String):
		_sound_path = sound_path

		# Create AudioStreamPlayer
		_player = AudioStreamPlayer.new()
		parent.add_child(_player)

		# Try to load the audio stream
		if ResourceLoader.exists(sound_path):
			_stream = load(sound_path)
			if _stream != null:
				_player.stream = _stream
				_is_loaded = true

		if not _is_loaded:
			push_error("[GodotSound] Failed to load: " + sound_path)

		# Connect finished signal for looping
		_player.finished.connect(_on_finished)

	func play(volume: int = 100, pan: int = 0) -> void:
		if not _is_loaded:
			print("[GodotSound] Cannot play - not loaded: " + _sound_path)
			return

		_is_looping = false

		# Convert volume (0-100) to Godot's dB scale (-80 to 0)
		# volume 100 = 0 dB, volume 0 = -80 dB
		var volume_db: float = (volume / 100.0) * 80.0 - 80.0
		_player.volume_db = volume_db

		# Pan not directly supported in AudioStreamPlayer
		# Would need AudioStreamPlayer2D for spatial audio
		# For now, we ignore pan (original game didn't use it extensively)

		print("[GodotSound] Playing: " + _sound_path.get_file() + " at " + str(volume_db) + " dB")
		_player.play()

	func loop(volume: int = 100) -> void:
		if not _is_loaded:
			print("[GodotSound] Cannot loop - not loaded: " + _sound_path)
			return

		_is_looping = true

		var volume_db: float = (volume / 100.0) * 80.0 - 80.0
		_player.volume_db = volume_db

		print("[GodotSound] Looping: " + _sound_path.get_file() + " at " + str(volume_db) + " dB")
		_player.play()

	func _on_finished():
		if _is_looping and _is_loaded:
			_player.play()

	func stop() -> void:
		if _player != null:
			_is_looping = false
			_player.stop()

	func is_playing() -> bool:
		return _player != null and _player.playing
