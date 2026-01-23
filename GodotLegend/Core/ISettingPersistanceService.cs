using System;
using System.Collections.Generic;

public interface IGameSettingsService
{
    /// <summary>
    /// Saves settings to persistent storage.
    /// </summary>
    void SaveSettings(List<PoleGameSetting> settings);
    /// <summary>
    /// Returns a list of game settings, or an empty list of none are available.  Never returns null.
    /// </summary>
    List<PoleGameSetting> LoadSettings();

    /// <summary>
    /// Delete the game settings if they exist
    /// </summary>
    void ResetSettings();

}


// Note: Conditionally compile the Serializable attribute
// so that the Silverlight implementation can compile.
#if POLEGAMESETTINGS_SERIALIZABLE
[Serializable]
#endif
public class PoleGameSetting
{
    public PoleGameSetting() { }
    public PoleGameSetting(string name, int value)
    {
        Name = name; Value = value;
    }
    public string Name
    {
        get { return _Name; }
        set { _Name = value; }
    }
    private string _Name;

    public int Value
    {
        get { return _Value; }
        set { _Value = value; }
    }
    private int _Value;

}
