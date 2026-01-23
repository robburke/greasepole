using System;

public static class Globals
{
    public static IGameSettingsService GameSettingsPersistance;
    public static IInputService InputService;
    public static IRenderingService RenderingService;
    public static ISoundService SoundService;
    public static IGameTimerService GameTimerService;

    public static GameLoop myGameLoop;
    public static GameConditions myGameConditions;
    public static Layers myLayers;

    /// <summary>
    /// Initialize all services from the ServiceFactory. Call this after setting ServiceFactory.Instance.
    /// </summary>
    public static void Initialize()
    {
        GameSettingsPersistance = ServiceFactory.Instance.ProduceGameSettingsService();
        InputService = ServiceFactory.Instance.ProduceInputService();
        RenderingService = ServiceFactory.Instance.ProduceRenderingService();
        SoundService = ServiceFactory.Instance.ProduceSoundService();
        GameTimerService = ServiceFactory.Instance.ProduceGameTimerService();

        myGameLoop = new GameLoop();
        myGameConditions = new GameConditions();
        myLayers = new Layers();
    }

    public static void Debug(string debugString)
    {
        if (DebugStringReceived != null) DebugStringReceived(null, new StringEventArgs(debugString));
    }

    public static void Analytic(string analyticDesc)
    {
        if (AnalyticsStringReceived != null) AnalyticsStringReceived(null, new StringEventArgs(analyticDesc));
    }

    public static event StringEventHandler DebugStringReceived;

    public static event StringEventHandler AnalyticsStringReceived;

}

public delegate void StringEventHandler(object sender, StringEventArgs e);

public class StringEventArgs : EventArgs
{
    public StringEventArgs(string eventString)
    {
        _EventString = eventString;
    }
    private string _EventString;
    public string Value { get { return _EventString; } }
}

