using Godot;

public partial class Main : Node2D
{
    private GodotTimerService _timerService;
    private bool _initialized = false;

    public override void _Ready()
    {
        // Hide the system mouse cursor (we render a custom cursor sprite)
        Input.MouseMode = Input.MouseModeEnum.Hidden;

        GD.Print("=== Legend of the Greasepole - Godot Port ===");
        GD.Print("Initializing services...");

        // Create the service factory and set it as the singleton
        var factory = new GodotServiceFactory(this);

        // Initialize all global services
        Globals.Initialize();

        // Keep a reference to the timer service so we can update it
        _timerService = (GodotTimerService)Globals.GameTimerService;

        // Initialize the sound service (sets up stub sound arrays)
        Globals.SoundService.Initialize();

        // Hook up debug output
        Globals.DebugStringReceived += OnDebugString;
        Globals.AnalyticsStringReceived += OnAnalyticsString;

        GD.Print("Services initialized. Starting game initialization...");

        // Initialize the game
        if (Globals.myGameLoop.InitializeGame())
        {
            GD.Print("Game initialized successfully!");
            _initialized = true;
        }
        else
        {
            GD.PrintErr("Failed to initialize game!");
        }
    }

    public override void _Process(double delta)
    {
        if (!_initialized)
            return;

        // Update the timer with this frame's delta
        _timerService.SetDelta(delta);

        // Update input state
        Globals.InputService.Update();

        // ProcessAI handles its own timing internally via GetAdditionalUpdateCount()
        // Just call it once per Godot frame - it will run the right number of AI updates
        if (Globals.myGameLoop.IsGameRunning())
        {
            Globals.myGameLoop.ProcessAI();
        }

        // Update sound service
        Globals.SoundService.Update();

        // Render the frame
        if (Globals.myGameLoop.IsGameRunning())
        {
            Globals.myGameLoop.RenderFrame();
        }

        // Check for exit
        if (!Globals.myGameLoop.IsGameRunning())
        {
            GD.Print("Game loop ended. Quitting...");
            GetTree().Quit();
        }
    }

    private void OnDebugString(object sender, StringEventArgs e)
    {
        GD.Print($"[DEBUG] {e.Value}");
    }

    private void OnAnalyticsString(object sender, StringEventArgs e)
    {
        GD.Print($"[ANALYTICS] {e.Value}");
    }

    public override void _ExitTree()
    {
        Globals.DebugStringReceived -= OnDebugString;
        Globals.AnalyticsStringReceived -= OnAnalyticsString;
    }

    public override void _Draw()
    {
        // Perform actual sprite rendering
        var renderingService = Globals.RenderingService as GodotRenderingService;
        renderingService?.PerformDraw(this);
    }

    public override void _Input(InputEvent @event)
    {
        // Handle mouse button events directly for more reliable detection
        // This event-driven approach catches clicks that might be missed by polling
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            var inputService = Globals.InputService as GodotInputService;

            if (mouseEvent.ButtonIndex == MouseButton.Left)
            {
                inputService?.NotifyLeftClick();
            }
            else if (mouseEvent.ButtonIndex == MouseButton.Right)
            {
                // Mark as handled so Godot doesn't show context menu
                GetViewport().SetInputAsHandled();
                inputService?.NotifyRightClick();
            }
        }
    }
}
