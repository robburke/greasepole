using Godot;
using System;

public class GodotInputService : IInputService
{
    private Vector2 _mousePosition;
    private bool _leftDown;
    private bool _rightDown;
    private bool _backDown;
    private bool _startDown;

    // Click flags - set by event handlers, cleared after AI processes
    // These use a "latch" pattern: once set true, they stay true until OnAIFrameComplete()
    private bool _leftClickPending;
    private bool _rightClickPending;
    private bool _backPending;
    private bool _startPending;

    // Previous frame's down state for edge detection (backup for polling)
    private bool _prevLeftDown;
    private bool _prevRightDown;
    private bool _prevBackDown;
    private bool _prevStartDown;

    // Cheat key edge detection - track previous state for single-fire
    private GreasepoleKeys _lastCheatKey = GreasepoleKeys.None;

    public int GetMouseX()
    {
        return (int)_mousePosition.X;
    }

    public int GetMouseY()
    {
        return (int)_mousePosition.Y;
    }

    public bool LeftButtonPressed()
    {
        return _leftClickPending;
    }

    public bool LeftButtonDown()
    {
        return _leftDown;
    }

    public bool RightButtonPressed()
    {
        return _rightClickPending;
    }

    public bool RightButtonDown()
    {
        return _rightDown;
    }

    public bool BackButtonPressed()
    {
        return _backPending;
    }

    public bool BackButtonDown()
    {
        return _backDown;
    }

    public void ConsumeBackInput()
    {
        _backPending = false;
        // Do NOT clear _backDown - user must physically release the key to trigger again
    }

    public bool StartButtonPressed()
    {
        return _startPending;
    }

    public bool StartButtonDown()
    {
        return _startDown;
    }

    public bool ToggleForwardButtonPressed()
    {
        return Input.IsActionJustPressed("ui_right");
    }

    public bool ToggleForwardButtonDown()
    {
        return Input.IsActionPressed("ui_right");
    }

    public bool ToggleBackButtonPressed()
    {
        return Input.IsActionJustPressed("ui_left");
    }

    public bool ToggleBackButtonDown()
    {
        return Input.IsActionPressed("ui_left");
    }

    public void Update()
    {
        // Mouse position
        var viewport = Engine.GetMainLoop() as SceneTree;
        if (viewport?.Root != null)
        {
            _mousePosition = viewport.Root.GetMousePosition();
        }

        // Get current down states via polling (backup for events that might be missed)
        bool currentLeftDown = Input.IsMouseButtonPressed(MouseButton.Left);
        bool currentRightDown = Input.IsMouseButtonPressed(MouseButton.Right);
        bool currentBackDown = Input.IsActionPressed("ui_back") || Input.IsKeyPressed(Key.Escape);
        bool currentStartDown = Input.IsActionPressed("ui_accept") || Input.IsKeyPressed(Key.Enter);

        // Detect rising edge via polling as a backup
        // The event-based NotifyLeftClick/NotifyRightClick are the primary detection
        // but polling catches cases where events might be missed
        if (currentLeftDown && !_prevLeftDown)
            _leftClickPending = true;
        if (currentRightDown && !_prevRightDown)
            _rightClickPending = true;
        if (currentBackDown && !_prevBackDown)
            _backPending = true;
        if (currentStartDown && !_prevStartDown)
            _startPending = true;

        // Store current down states
        _leftDown = currentLeftDown;
        _rightDown = currentRightDown;
        _backDown = currentBackDown;
        _startDown = currentStartDown;

        // Remember for next frame's edge detection
        _prevLeftDown = currentLeftDown;
        _prevRightDown = currentRightDown;
        _prevBackDown = currentBackDown;
        _prevStartDown = currentStartDown;
    }

    /// <summary>
    /// Called from Main._Input when a left-click is detected.
    /// Ensures clicks are captured even if they happen between Update() calls.
    /// </summary>
    public void NotifyLeftClick()
    {
        _leftClickPending = true;
    }

    /// <summary>
    /// Called from Main._Input when a right-click is detected.
    /// Ensures clicks are captured even if they happen between Update() calls.
    /// </summary>
    public void NotifyRightClick()
    {
        _rightClickPending = true;
    }

    /// <summary>
    /// Called by the game loop after AI processing completes.
    /// Clears pending flags so each click is only seen for exactly one AI cycle.
    /// </summary>
    public void OnAIFrameComplete()
    {
        _leftClickPending = false;
        _rightClickPending = false;
        _backPending = false;
        _startPending = false;
    }

    public GreasepoleKeys GetKeyboardInput()
    {
        // Use the latched/consumable state for Escape, just like the menu logic does.
        // This prevents the key from being handled twice (once by menu, once by HandleKeyDown)
        // in the same frame or subsequent frames if the proper consumption methods are used.
        if (BackButtonPressed())
             return GreasepoleKeys.Back;

        // Detect which cheat key is currently pressed
        GreasepoleKeys currentKey = GreasepoleKeys.None;
        if (Input.IsKeyPressed(Key.M))
            currentKey = GreasepoleKeys.IncreaseMunitions;
        else if (Input.IsKeyPressed(Key.F))
            currentKey = GreasepoleKeys.ShowFPS;
        else if (Input.IsKeyPressed(Key.R))
            currentKey = GreasepoleKeys.BuildRingEnergy;
        else if (Input.IsKeyPressed(Key.B))
            currentKey = GreasepoleKeys.GWBalloon;
        else if (Input.IsKeyPressed(Key.A))
            currentKey = GreasepoleKeys.PopupArtsciorCommie;
        else if (Input.IsKeyPressed(Key.S))
            currentKey = GreasepoleKeys.PopupScicon;
        else if (Input.IsKeyPressed(Key.O))
            currentKey = GreasepoleKeys.PopupHose;
        else if (Input.IsKeyPressed(Key.J))
            currentKey = GreasepoleKeys.Mosh;
        else if (Input.IsKeyPressed(Key.D))
            currentKey = GreasepoleKeys.StartDemo;
        else if (Input.IsKeyPressed(Key.C))
            currentKey = GreasepoleKeys.ClarkC;
        else if (Input.IsKeyPressed(Key.H))
            currentKey = GreasepoleKeys.ClarkH;
        else if (Input.IsKeyPressed(Key.P))
            currentKey = GreasepoleKeys.ClarkP;

        // Edge detection: only return key on initial press, not while held
        GreasepoleKeys result = GreasepoleKeys.None;
        if (currentKey != GreasepoleKeys.None && currentKey != _lastCheatKey)
        {
            result = currentKey;
        }
        _lastCheatKey = currentKey;

        return result;
    }
}
