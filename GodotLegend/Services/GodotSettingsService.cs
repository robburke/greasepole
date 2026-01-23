using Godot;
using System.Collections.Generic;

/// <summary>
/// Settings service using Godot's ConfigFile for persistent storage.
/// </summary>
public class GodotSettingsService : IGameSettingsService
{
    private const string SETTINGS_PATH = "user://greasepole_settings.cfg";
    private const string SECTION = "settings";

    public void SaveSettings(List<PoleGameSetting> settings)
    {
        var config = new ConfigFile();

        foreach (var setting in settings)
        {
            config.SetValue(SECTION, setting.Name, setting.Value);
        }

        var error = config.Save(SETTINGS_PATH);
        if (error != Error.Ok)
        {
            GD.PrintErr($"[GodotSettingsService] Failed to save settings: {error}");
        }
        else
        {
            GD.Print($"[GodotSettingsService] Saved {settings.Count} settings");
        }
    }

    public List<PoleGameSetting> LoadSettings()
    {
        var settings = new List<PoleGameSetting>();
        var config = new ConfigFile();

        var error = config.Load(SETTINGS_PATH);
        if (error != Error.Ok)
        {
            GD.Print("[GodotSettingsService] No settings file found, returning empty list");
            return settings;
        }

        if (config.HasSection(SECTION))
        {
            foreach (string key in config.GetSectionKeys(SECTION))
            {
                var value = (int)config.GetValue(SECTION, key, 0);
                settings.Add(new PoleGameSetting(key, value));
            }
        }

        GD.Print($"[GodotSettingsService] Loaded {settings.Count} settings");
        return settings;
    }

    public void ResetSettings()
    {
        var dir = DirAccess.Open("user://");
        if (dir != null && dir.FileExists("greasepole_settings.cfg"))
        {
            dir.Remove("greasepole_settings.cfg");
            GD.Print("[GodotSettingsService] Settings file deleted");
        }
    }
}
