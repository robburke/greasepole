using Godot;

public class GodotTimerService : IGameTimerService
{
    private const double TARGET_FRAME_TIME_MS = 1000.0 / 25.0; // Original game ran at 25 FPS (see AIDefine.cs)
    private double _accumulatedTime = 0;
    private double _deltaMs = 0;
    private bool _isPaused = false;
    private double _gameTimeScoreMs = 0;

    /// <summary>
    /// Call this from Godot's _Process with the delta time to update the timer.
    /// </summary>
    public void SetDelta(double deltaSec)
    {
        _deltaMs = deltaSec * 1000.0;

        if (!_isPaused)
        {
            _gameTimeScoreMs += _deltaMs;
        }
    }

    public void Update()
    {
        if (!_isPaused)
        {
            _accumulatedTime += _deltaMs;
        }
    }

    public int GetAdditionalUpdateCount()
    {
        // Calculate how many game frames should run based on accumulated time
        int additionalUpdates = 0;

        while (_accumulatedTime >= TARGET_FRAME_TIME_MS)
        {
            _accumulatedTime -= TARGET_FRAME_TIME_MS;
            additionalUpdates++;
        }

        // Cap to prevent spiral of death
        if (additionalUpdates > 4)
            additionalUpdates = 4;

        return additionalUpdates;
    }

    public void PauseUpdateCountTimer()
    {
        _isPaused = true;
    }

    public void ResumeUpdateCountTimer()
    {
        _isPaused = false;
        _accumulatedTime = 0;
    }

    public void ResetGameTimeScore()
    {
        _gameTimeScoreMs = 0;
    }

    public double GetCurrentGameTimeScoreMilliseconds()
    {
        return _gameTimeScoreMs;
    }
}
