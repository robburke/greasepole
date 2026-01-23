using Godot;
using System;

/// <summary>
/// Godot sound service implementation using AudioStreamPlayer.
/// </summary>
public class GodotSoundService : ISoundService
{
    private Node _gameNode;
    private const string SOUND_PATH = "res://Assets/Sound/";
    private bool _shortSoundsLoaded = false;
    private int _longSoundsLoadedCount = 0;

    public GodotSoundService(Node gameNode)
    {
        _gameNode = gameNode;
    }

    public void Initialize()
    {
        GD.Print("[GodotSoundService] Initialize called");

        // Initialize the static sound array with stub sounds (will be replaced when LoadShortSounds is called)
        for (int i = 0; i < AIMethods.MAX_SSOUNDS; i++)
        {
            AIMethods.sSound[i] = new StubStaticSound();
        }

        // Initialize the streamed/long sound array with stub sounds (will be replaced when LoadLongSounds is called)
        for (int i = 0; i < AIMethods.MAX_LSOUNDS; i++)
        {
            AIMethods.lSound[i] = new StubStreamedSound();
        }
    }

    public void Update()
    {
        // Called each frame for audio updates - not needed for Godot's audio system
    }

    public bool LoadShortSounds()
    {
        if (_shortSoundsLoaded) return true;

        GD.Print("[GodotSoundService] LoadShortSounds - loading actual sounds");

        // Load each short sound based on the ASSList enum
        foreach (ASSList soundEnum in Enum.GetValues(typeof(ASSList)))
        {
            int index = (int)soundEnum;
            string soundName = GetShortSoundFileName(soundEnum);
            string fullPath = SOUND_PATH + soundName + ".mp3";

            if (ResourceLoader.Exists(fullPath))
            {
                AIMethods.sSound[index] = new GodotStaticSound(_gameNode, fullPath);
            }
            else
            {
                // Keep stub for missing sounds
                GD.PrintErr($"[GodotSoundService] Short sound not found: {fullPath}");
            }
        }

        _shortSoundsLoaded = true;
        return true;
    }

    public bool LoadLongSounds(int percentageToLoad)
    {
        // Calculate how many sounds to load based on percentage
        int totalSounds = Enum.GetValues(typeof(ASLList)).Length;
        int targetCount = (int)(totalSounds * percentageToLoad / 100.0);

        // If we've already loaded the target percentage, return true to proceed
        if (_longSoundsLoadedCount >= targetCount)
        {
            return true;
        }

        GD.Print($"[GodotSoundService] LoadLongSounds: loading {_longSoundsLoadedCount} to {targetCount} of {totalSounds}");

        // Load sounds up to the target count
        foreach (ASLList soundEnum in Enum.GetValues(typeof(ASLList)))
        {
            int index = (int)soundEnum;
            if (index >= targetCount) break;
            if (index < _longSoundsLoadedCount) continue; // Already loaded

            string soundName = GetLongSoundFileName(soundEnum);
            string fullPath = SOUND_PATH + soundName + ".mp3";

            if (ResourceLoader.Exists(fullPath))
            {
                AIMethods.lSound[index] = new GodotStreamedSound(_gameNode, fullPath);
            }
            else
            {
                // Keep stub for missing sounds
                GD.PrintErr($"[GodotSoundService] Long sound not found: {fullPath}");
            }

            _longSoundsLoadedCount = index + 1;
        }

        return _longSoundsLoadedCount >= targetCount;
    }

    public void ShutUp()
    {
        GD.Print("[GodotSoundService] ShutUp - stopping all sounds");

        // Stop all short sounds
        for (int i = 0; i < AIMethods.MAX_SSOUNDS; i++)
        {
            AIMethods.sSound[i]?.Stop();
        }

        // Stop all long sounds
        for (int i = 0; i < AIMethods.MAX_LSOUNDS; i++)
        {
            AIMethods.lSound[i]?.Stop();
        }
    }

    /// <summary>
    /// Convert ASSList enum to filename (remove ssnd prefix)
    /// </summary>
    private string GetShortSoundFileName(ASSList sound)
    {
        // Enum name like "ssndEFFECTS_ACHIEVEMENTUNLOCKED" -> "EFFECTS_ACHIEVEMENTUNLOCKED"
        string name = sound.ToString();
        if (name.StartsWith("ssnd"))
        {
            return name.Substring(4);
        }
        return name;
    }

    /// <summary>
    /// Convert ASLList enum to filename (remove lsnd prefix)
    /// </summary>
    private string GetLongSoundFileName(ASLList sound)
    {
        // Enum name like "lsndAPPLES_OFFER1" -> "APPLES_OFFER1"
        string name = sound.ToString();
        if (name.StartsWith("lsnd"))
        {
            return name.Substring(4);
        }
        return name;
    }
}

/// <summary>
/// Stub implementation of IStaticSound that does nothing.
/// Used as placeholder before sounds are loaded.
/// </summary>
public class StubStaticSound : IStaticSound
{
    public void Play(int volume, int pan) { }
    public void Play(int volume) { }
    public void Play() { }
    public void Loop(int volume) { }
    public void Stop() { }
    public bool IsPlaying() => false;
}

/// <summary>
/// Stub implementation of IStreamedSound that does nothing.
/// Used as placeholder before sounds are loaded.
/// </summary>
public class StubStreamedSound : IStreamedSound
{
    public void Play(int volume, int pan) { }
    public void Play(int volume) { }
    public void Play() { }
    public void Loop(int volume) { }
    public void Stop() { }
    public bool IsPlaying() => false;
}
