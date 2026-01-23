using Godot;

public partial class WindowWrapper : Control
{
    private const float GAME_WIDTH = 640.0f;
    private const float GAME_HEIGHT = 480.0f;
    private const float ASPECT_RATIO = GAME_WIDTH / GAME_HEIGHT; // 4:3 = 1.333...

    private SubViewportContainer _viewportContainer;

    public override void _Ready()
    {
        _viewportContainer = GetNode<SubViewportContainer>("ViewportContainer");

        // Connect to resize signal
        GetTree().Root.SizeChanged += OnWindowResized;

        // Initial sizing
        OnWindowResized();
    }

    public override void _ExitTree()
    {
        GetTree().Root.SizeChanged -= OnWindowResized;
    }

    private void OnWindowResized()
    {
        if (_viewportContainer == null)
            return;

        // Get the current window size
        Vector2 windowSize = GetViewportRect().Size;

        // Calculate the largest size that fits while maintaining aspect ratio
        float windowAspect = windowSize.X / windowSize.Y;

        float newWidth, newHeight;

        if (windowAspect > ASPECT_RATIO)
        {
            // Window is wider than 4:3 - fit to height (pillarbox sides)
            newHeight = windowSize.Y;
            newWidth = newHeight * ASPECT_RATIO;
        }
        else
        {
            // Window is taller than 4:3 - fit to width (letterbox top/bottom)
            newWidth = windowSize.X;
            newHeight = newWidth / ASPECT_RATIO;
        }

        // Center the viewport container
        _viewportContainer.Size = new Vector2(newWidth, newHeight);
        _viewportContainer.Position = new Vector2(
            (windowSize.X - newWidth) / 2.0f,
            (windowSize.Y - newHeight) / 2.0f
        );
    }
}
