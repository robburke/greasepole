using Godot;

/// <summary>
/// Godot implementation of IStaticSound for short sound effects.
/// Uses AudioStreamPlayer for 2D audio playback.
/// </summary>
public class GodotStaticSound : IStaticSound
{
    private AudioStreamPlayer _player;
    private AudioStream _stream;
    private string _soundPath;
    private bool _isLoaded = false;

    public GodotStaticSound(Node parent, string soundPath)
    {
        _soundPath = soundPath;

        // Create AudioStreamPlayer
        _player = new AudioStreamPlayer();
        parent.AddChild(_player);

        // Try to load the audio stream
        if (ResourceLoader.Exists(soundPath))
        {
            _stream = ResourceLoader.Load<AudioStream>(soundPath);
            if (_stream != null)
            {
                _player.Stream = _stream;
                _isLoaded = true;
            }
        }

        if (!_isLoaded)
        {
            GD.PrintErr($"[GodotStaticSound] Failed to load: {soundPath}");
        }
    }

    public void Play(int volume, int pan)
    {
        if (!_isLoaded) return;

        // Convert volume (0-100) to Godot's dB scale (-80 to 0)
        // volume 100 = 0 dB, volume 0 = -80 dB
        float volumeDb = (volume / 100f) * 80f - 80f;
        _player.VolumeDb = volumeDb;

        // Pan not implemented for simple AudioStreamPlayer
        // Would need AudioStreamPlayer2D for spatial audio

        _player.Play();
    }

    public void Play(int volume)
    {
        Play(volume, 0);
    }

    public void Play()
    {
        Play(100, 0);
    }

    public void Loop(int volume)
    {
        if (!_isLoaded) return;

        float volumeDb = (volume / 100f) * 80f - 80f;
        _player.VolumeDb = volumeDb;

        // For looping, we need to handle it manually or use AudioStreamPlayer's built-in loop
        // MP3s in Godot can be set to loop in the import settings
        // For runtime, we connect to the finished signal
        if (!_player.IsConnected("finished", Callable.From(OnLoopFinished)))
        {
            _player.Connect("finished", Callable.From(OnLoopFinished));
        }

        _player.Play();
    }

    private void OnLoopFinished()
    {
        if (_player != null && _isLoaded)
        {
            _player.Play();
        }
    }

    public void Stop()
    {
        if (_player != null)
        {
            _player.Stop();
        }
    }

    public bool IsPlaying()
    {
        return _player != null && _player.Playing;
    }

    public void Dispose()
    {
        if (_player != null)
        {
            _player.Stop();
            _player.QueueFree();
            _player = null;
        }
    }
}

/// <summary>
/// Godot implementation of IStreamedSound for longer audio (music, voice).
/// Same implementation as GodotStaticSound since Godot handles streaming automatically.
/// </summary>
public class GodotStreamedSound : GodotStaticSound, IStreamedSound
{
    public GodotStreamedSound(Node parent, string soundPath) : base(parent, soundPath)
    {
    }
}
