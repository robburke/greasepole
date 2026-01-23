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
    bool StartButtonPressed();
    bool StartButtonDown();

    bool ToggleForwardButtonPressed();
    bool ToggleForwardButtonDown();
    bool ToggleBackButtonPressed();
    bool ToggleBackButtonDown();

    void Update();

    GreasepoleKeys GetKeyboardInput();
}
