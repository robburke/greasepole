using Godot;

public class GodotServiceFactory : ServiceFactory
{
    private Node _gameNode;

    public GodotServiceFactory(Node gameNode)
    {
        _gameNode = gameNode;
        Instance = this;
    }

    public override IGameSettingsService ProduceGameSettingsService()
    {
        return new GodotSettingsService();
    }

    public override IInputService ProduceInputService()
    {
        return new GodotInputService();
    }

    public override IRenderingService ProduceRenderingService()
    {
        return new GodotRenderingService(_gameNode);
    }

    public override ISoundService ProduceSoundService()
    {
        return new GodotSoundService(_gameNode);
    }

    public override IGameTimerService ProduceGameTimerService()
    {
        return new GodotTimerService();
    }
}
