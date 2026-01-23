using System;
using System.Text;

public interface IInputService
{
	int GetMouseX();
	int GetMouseY(); 

    bool LeftButtonPressed(); 
	bool LeftButtonDown(); 
	bool RightButtonPressed();
    bool RightButtonDown();

    bool BackButtonPressed();
    bool BackButtonDown();
    void ConsumeBackInput();
    bool StartButtonPressed();
    bool StartButtonDown();

    bool ToggleForwardButtonPressed();
    bool ToggleForwardButtonDown();
    bool ToggleBackButtonPressed();
    bool ToggleBackButtonDown();

    void Update();

    /// <summary>
    /// Called after AI frame processes to signal input was consumed.
    /// Default implementation does nothing (for non-Godot implementations).
    /// </summary>
    void OnAIFrameComplete() { }

    GreasepoleKeys GetKeyboardInput();
}
