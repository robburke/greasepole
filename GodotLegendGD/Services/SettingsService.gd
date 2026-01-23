extends Node

# SettingsService.gd - Settings persistence using Godot's ConfigFile
# Ported from GodotSettingsService.cs

const SETTINGS_PATH: String = "user://greasepole_settings.cfg"
const SECTION: String = "settings"


func _ready():
	Globals.SettingsService = self
	print("[SettingsService] Initialized")


func save_settings(settings: Dictionary) -> void:
	var config := ConfigFile.new()

	for key in settings.keys():
		config.set_value(SECTION, key, settings[key])

	var error := config.save(SETTINGS_PATH)
	if error != OK:
		push_error("[SettingsService] Failed to save settings: " + str(error))
	else:
		print("[SettingsService] Saved %d settings" % settings.size())


func load_settings() -> Dictionary:
	var settings: Dictionary = {}
	var config := ConfigFile.new()

	var error := config.load(SETTINGS_PATH)
	if error != OK:
		print("[SettingsService] No settings file found, returning empty dictionary")
		return settings

	if config.has_section(SECTION):
		for key in config.get_section_keys(SECTION):
			settings[key] = config.get_value(SECTION, key, 0)

	print("[SettingsService] Loaded %d settings" % settings.size())
	return settings


func reset_settings() -> void:
	var dir := DirAccess.open("user://")
	if dir != null and dir.file_exists("greasepole_settings.cfg"):
		dir.remove("greasepole_settings.cfg")
		print("[SettingsService] Settings file deleted")
