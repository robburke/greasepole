using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverLegend;

namespace SilverLegend.Services
{
    public class InputServiceSilverlight : IInputService
    {
        public InputServiceSilverlight()
        {
            Page pageInstance = Page.Instance;
            pageInstance.GameCanvas.MouseLeftButtonDown += delegate { m_MouseLeftButtonPressed = true; };
            pageInstance.GameCanvas.MouseLeftButtonUp += delegate { m_MouseLeftButtonPressed = false; };
            pageInstance.GameCanvas.MouseMove += delegate(object s, MouseEventArgs mouseMoveEventArgs) { m_MousePoint = mouseMoveEventArgs.GetPosition(pageInstance.GameCanvas); };

            KeyHandler = new SilverLegend.Util.KeyHandler(pageInstance);
        }

        private Util.KeyHandler KeyHandler;

        public bool MouseLeftButtonPressed { get { return m_MouseLeftButtonPressed; } }
        private bool m_MouseLeftButtonPressed = false;

        public Point MousePoint { get { return m_MousePoint; } }
        private Point m_MousePoint = new Point(320, 240);


        #region Getter Implementations
        public int GetMouseX()
        {
            return _MouseX;
        }

        public int GetMouseY()
        {
            return _MouseY;
        }

        public bool LeftButtonPressed()
        {
            return _LeftButtonPressed;
        }

        public bool LeftButtonDown()
        {
            return _LeftButtonDown;
        }

        public bool RightButtonPressed()
        {
            return _RightButtonPressed;
        }

        public bool RightButtonDown()
        {
            return _RightButtonDown;
        }

        public bool BackButtonPressed()
        {
            return _BackButtonPressed;
        }

        public bool BackButtonDown()
        {
            return _BackButtonDown;
        }

        public bool StartButtonPressed()
        {
            return _StartButtonPressed;
        }

        public bool StartButtonDown()
        {
            return _StartButtonDown;
        }

        public bool ToggleForwardButtonPressed()
        {
            return _ToggleForwardButtonPressed;
        }

        public bool ToggleForwardButtonDown()
        {
            return _ToggleForwardButtonDown;
        }

        public bool ToggleBackButtonPressed()
        {
            return _ToggleBackButtonPressed;
        }

        public bool ToggleBackButtonDown()
        {
            return _ToggleBackButtonDown;
        }
        #endregion

        public void Update()
        {
            bool leftButtonDown = false;
            bool rightButtonDown = false;
            bool backButtonDown = false;
            bool startButtonDown = false;
            bool toggleForwardButtonDown = false;
            bool toggleBackButtonDown = false;

            Page pageInstance = Page.Instance;
            _MouseX = (int)(MousePoint.X);
            _MouseY = (int)(MousePoint.Y);
            if (_MouseX < 0) _MouseX = 0; if (_MouseX > 639) _MouseX = 639;
            if (_MouseY < 0) _MouseY = 0; if (_MouseY > 479) _MouseX = 479;

            leftButtonDown = MouseLeftButtonPressed;
            rightButtonDown = rightButtonDown || KeyHandler.IsKeyPressed(Key.Space); 
            backButtonDown = backButtonDown || KeyHandler.IsKeyPressed(Key.Back);
            startButtonDown = startButtonDown || KeyHandler.IsKeyPressed(Key.Enter);
            toggleForwardButtonDown = toggleForwardButtonDown || KeyHandler.IsKeyPressed(Key.Shift)
                || KeyHandler.IsKeyPressed(Key.X)
                || KeyHandler.IsKeyPressed(Key.Tab);
            toggleBackButtonDown = toggleBackButtonDown || KeyHandler.IsKeyPressed(Key.Z);

            _LeftButtonPressed = !_LeftButtonDown && leftButtonDown;
            _RightButtonPressed = !_RightButtonDown && rightButtonDown;
            _BackButtonPressed = !_BackButtonDown && backButtonDown;
            _StartButtonPressed = !_StartButtonDown && startButtonDown;
            _ToggleForwardButtonPressed = !_ToggleForwardButtonDown && toggleForwardButtonDown;
            _ToggleBackButtonPressed = !_ToggleBackButtonDown && toggleBackButtonDown;

            _LeftButtonDown = leftButtonDown;
            _RightButtonDown = rightButtonDown;
            _BackButtonDown = backButtonDown;
            _StartButtonDown = startButtonDown;
            _ToggleForwardButtonDown = toggleForwardButtonDown;
            _ToggleBackButtonDown = toggleBackButtonDown;
        }
        private int _MouseX = 320;
        private int _MouseY = 240;
        private bool _LeftButtonPressed = false;
        private bool _LeftButtonDown = false;
        private bool _RightButtonPressed = false;
        private bool _RightButtonDown = false;
        private bool _BackButtonPressed = false;
        private bool _BackButtonDown = false;
        private bool _StartButtonPressed = false;
        private bool _StartButtonDown = false;
        private bool _ToggleForwardButtonPressed = false;
        private bool _ToggleForwardButtonDown = false;
        private bool _ToggleBackButtonPressed = false;
        private bool _ToggleBackButtonDown = false;

        private GreasepoleKeys LastKeyboardInput = GreasepoleKeys.None;

        public GreasepoleKeys GetKeyboardInput()
        {
            GreasepoleKeys result = GreasepoleKeys.None;
            Util.KeyHandler kh = KeyHandler;
            if (kh.IsKeyPressed(Key.Escape) || kh.IsKeyPressed(Key.Back)) result = GreasepoleKeys.Back;
            if (kh.IsKeyPressed(Key.C)) result = GreasepoleKeys.ClarkC;
            if (kh.IsKeyPressed(Key.H)) result = GreasepoleKeys.ClarkH;
            if (kh.IsKeyPressed(Key.P)) result = GreasepoleKeys.ClarkP;
            if (kh.IsKeyPressed(Key.Up)) result = GreasepoleKeys.IncreaseMunitions;
            if (kh.IsKeyPressed(Key.F1)) result = GreasepoleKeys.ShowFPS;
            if (kh.IsKeyPressed(Key.F2)) result = GreasepoleKeys.BuildRingEnergy;
            if (kh.IsKeyPressed(Key.F3)) result = GreasepoleKeys.GWBalloon;
            if (kh.IsKeyPressed(Key.F4)) result = GreasepoleKeys.PopupArtsciorCommie;
            if (kh.IsKeyPressed(Key.F5)) result = GreasepoleKeys.PopupScicon;
            if (kh.IsKeyPressed(Key.F6)) result = GreasepoleKeys.PopupHose;
            if (kh.IsKeyPressed(Key.F7)) result = GreasepoleKeys.Mosh;
            if (kh.IsKeyPressed(Key.F12)) result = GreasepoleKeys.StartDemo;

            if (result == LastKeyboardInput)
            {
                return GreasepoleKeys.None;
            }
            LastKeyboardInput = result;
            return LastKeyboardInput;
        }

    }
}
