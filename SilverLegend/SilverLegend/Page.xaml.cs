using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using SilverLegend.Services;
using System.Windows.Media.Imaging;
using System.Net;
using System.IO;
using System.Windows.Resources;
using System.Windows.Browser;

namespace SilverLegend
{
    public partial class Page : UserControl
    {
        public Page()
        {
            InitializeComponent();
            m_Instance = this;

            App.Current.Host.Content.Resized += ContentResized;
            App.Current.Host.Content.FullScreenChanged += ContentResized;
        }
        #region Transform Canvas to stretch it to full window size
        void ContentResized(object sender, EventArgs e)
        {
            double actualHeight = App.Current.Host.Content.ActualHeight;
            double actualWidth = App.Current.Host.Content.ActualWidth;

            if (!App.Current.Host.Content.IsFullScreen)
            {
                actualHeight = this.LayoutRoot.ActualHeight;
                actualWidth = this.LayoutRoot.ActualWidth;
            }

            double scaleY = actualHeight / 480;
            double scaleX = actualWidth / 640;
            double scl = Math.Min(scaleX, scaleY);
            if (scl <= 0) return;
            GameCanvasTranslation.X = ((actualWidth) - 640d * scl) / 2d;
            GameCanvasTranslation.Y = ((actualHeight) - 480d * scl) / 2d;
            GameCanvasScale.ScaleX = scl;
            GameCanvasScale.ScaleY = scl;
            GameCanvasClipRect.Rect = new Rect(0, 0, 640, 480);
            FullScreenToggleControl.Visibility = 
                App.Current.Host.Content.IsFullScreen ? Visibility.Collapsed : Visibility.Visible;
        }
        #endregion 

        public static Page Instance { get { return m_Instance; } }
        private static Page m_Instance;

        public bool ToggleMuteWithFocus { get; set; } 

        public bool IsMuted
        {
            get
            {
                return m_IsMuted;
            }
            set
            {
                m_IsMuted = value;
                if (LoadingSoundMediaElement != null)
                {
                    LoadingSoundMediaElement.Volume = value ? 0d : 1d;
                }
                if (MenuAssetsProgressPercentage == 100 && Globals.SoundService != null)
                {
                    ((SoundServiceSilverlight)Globals.SoundService).SetMute(value);
                }
                if (IsMutedChanged != null) IsMutedChanged(this, new EventArgs());
            }
        }
        private bool m_IsMuted;
        public event EventHandler IsMutedChanged;

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            Application.Current.Host.Settings.EnableFrameRateCounter = false;
            //System.Windows.Interop.Settings
            IsMuted = IsMuted;
            ServiceFactory.Instance = new ServiceFactorySilverlight();
            Globals.DebugStringReceived += new StringEventHandler(GlobalsDebugStringReceived);
            Globals.AnalyticsStringReceived += new StringEventHandler(GlobalsAnalyticsStringReceived);

            Globals.myGameConditions.SetEnhancedGraphics(0);

            // BEGIN TEST TO GET IMAGE LOADING
            //Image i = new Image();
            //Uri uri = new Uri("/SilverlightLegendCore;component/Graphics/TRANS_SLAMJACKET4.BMP", UriKind.Relative);
            //BitmapImage bi = new BitmapImage(uri);
            //i.Source = bi;
            //i.Width = 300;
            //i.Height = 300;
            //GameCanvasRoot.Children.Add(i);
            // END TEST TO GET IMAGE LOADING
            Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpTransBitmaps, 100); 
            Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpMenuBitmaps, 100);
            Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpLoadBitmaps, 100);

            LoadingSoundMediaElement.Source = new Uri("/SilverLegend;component/Sound/LoadRepeat.mp3", UriKind.Relative);
            LoadingSoundMediaElement.MediaEnded += 
            delegate { 
                if (MenuAssetsProgressPercentage < 100)
                {
                    LoadingSoundMediaElement.Position = TimeSpan.Zero;
                    LoadingSoundMediaElement.Volume = IsMuted ? 0 : 1;
                    LoadingSoundMediaElement.Play();
                }
            };
            LoadingScreenCanvas.Children.Add(LoadingSoundMediaElement);
            LoadingSoundMediaElement.Play();

            DownloadMenuAssetsAssembly();

            string browserInfo = GetBrowserInformation();
            if (browserInfo.Length > 49)
            {
                browserInfo = browserInfo.Substring(0, 49);
            }
            m_AnalyticsService.YoAsync(App.s_SessionId, "BrowserInfo", browserInfo);


        }


        private string GetBrowserInformation()
        {
            string biString = "";
            BrowserInformation bi = HtmlPage.BrowserInformation;
            if (bi.ProductName != null)
                biString += (bi.ProductName + ";");
            else
                biString += "?;";
            if (bi.ProductVersion != null)
                biString += (bi.ProductVersion + ";");
            else
                biString += "?;";
            if (bi.Name != null)
                biString += (bi.Name + ";");
            else
                biString += "?;";
            if (bi.Platform != null)
                biString += (bi.Platform);
            else
                biString += "?";
            return biString;
            
        }
        MediaElement LoadingSoundMediaElement = new MediaElement();


        private void DownloadMenuAssetsAssembly()
        {
            MenuAssetsWebClient = new WebClient();
            MenuAssetsWebClient.DownloadProgressChanged += delegate(object _sender, DownloadProgressChangedEventArgs dpce) { MenuAssetsProgressPercentage = dpce.ProgressPercentage; UpdateLoadingFroshPosition(dpce.ProgressPercentage); };
            MenuAssetsWebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(MenuAssetsDownloaded);
            MenuAssetsWebClient.OpenReadAsync(new Uri("SilverLegendAssetsMenu.dll", UriKind.Relative));
        }

        private void UpdateLoadingFroshPosition(int p)
        {
            Canvas.SetLeft(FroshOnLoadingScreenImage, 50 + p * 5);
        }
        public int MenuAssetsProgressPercentage { get; private set; }
        public int GameAssetsProgressPercentage { get; private set; }
        private WebClient MenuAssetsWebClient;
        private WebClient GameAssetsWebClient;
        private bool ReadyForGame = false;

        void MenuAssetsDownloaded(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null && e.Cancelled == false)
            {
                MenuAssetsProgressPercentage = 100;

                AssemblyPart assemblyPart = new AssemblyPart();
                assemblyPart.Load(e.Result);


                m_AnalyticsService.YoAsync(App.s_SessionId, "DownloadSuccess", "MenuAssets");

                // BEGIN TEST TO GET IMAGE LOADING
                //Image i = new Image();
                //string transJacketPath = "/SilverLegendAssetsMenu;component/Graphics/TRANS_SLAMJACKET4.BMP";
                //Uri uri = new Uri(transJacketPath, UriKind.Relative);
                //BitmapImage bi = new BitmapImage(uri);
                //i.Source = bi;
                //i.Width = 300;
                //i.Height = 300;
                //Page.Instance.GameCanvas.Children.Add(i);
                // END TEST TO GET IMAGE LOADING

                DownloadGameAssetsAssembly();

                Globals.myGameLoop.InitializeGame();

                GameLoop.Updated += new EventHandler<SilverLegend.Util.EventArgs<TimeSpan>>(GameLoop_Updated);
                GameLoop.Attach(this);


            }
            else
            {
                MenuAssetsProgressPercentage = 0;
                MenuAssetsWebClient.OpenReadAsync(new Uri("SilverLegendAssetsMenu.dll", UriKind.Relative));
            }
        }

        void GameAssetsDownloaded(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null && e.Cancelled == false)
            {
                GameAssetsProgressPercentage = 100;

                AssemblyPart assemblyPart = new AssemblyPart();
                assemblyPart.Load(e.Result);

                m_AnalyticsService.YoAsync(App.s_SessionId, "DownloadSuccess", "GameAssets");

                ReadyForGame = true;
            }
            else
            {
                GameAssetsProgressPercentage = 0;
                GameAssetsWebClient.OpenReadAsync(new Uri("SilverLegendAssetsGame.dll", UriKind.Relative));
            }
        }

        private void DownloadGameAssetsAssembly()
        {
            GameAssetsWebClient = new WebClient();
            GameAssetsWebClient.DownloadProgressChanged += delegate(object _sender, DownloadProgressChangedEventArgs dpce) { GameAssetsProgressPercentage = dpce.ProgressPercentage; if (StallingForGameAssets)  UpdateLoadingFroshPosition(dpce.ProgressPercentage); };
            GameAssetsWebClient.OpenReadCompleted += new OpenReadCompletedEventHandler(GameAssetsDownloaded);
            GameAssetsWebClient.OpenReadAsync(new Uri("SilverLegendAssetsGame.dll", UriKind.Relative));
        }

        private Util.GameLoop GameLoop = new SilverLegend.Util.GameLoop();
        private DateTime PageCreateTime = DateTime.Now;

        private bool StallingForGameAssets = false;
        
        private GameStates m_PreviouslyCommunicatedGameState = GameStates.STATENOCHANGE;
        private AnalyticsSvcRef.AnalyticsSvcClient m_AnalyticsService = new AnalyticsSvcRef.AnalyticsSvcClient();

        void GameLoop_Updated(object sender, SilverLegend.Util.EventArgs<TimeSpan> e)
        {
            try
            {
                double seconds = ((TimeSpan)(DateTime.Now - PageCreateTime)).TotalSeconds;
                //Globals.Debug("Begin Tick: " + seconds);
                
#if DEBUG
                TestRect.SetValue(Canvas.TopProperty, (double)0);
                TestRect.SetValue(Canvas.LeftProperty, 320 + 300 * Math.Sin(seconds));
#endif

                GameStates currentGameState = Globals.myGameLoop.CurrentGameState;
                if (currentGameState != m_PreviouslyCommunicatedGameState)
                {
                    bool shouldNotSend = 
                        m_PreviouslyCommunicatedGameState == GameStates.STATEDEMO
                        && currentGameState == GameStates.STATEGAME;
                    m_PreviouslyCommunicatedGameState = currentGameState;
                    if (!shouldNotSend)
                    {
                        m_AnalyticsService.YoAsync(App.s_SessionId, "GameStateChange", currentGameState.ToString());
                    }
                }
                // Test to see if not ReadyForGame and is playing game stall for time.
                if (currentGameState == GameStates.STATEGAME || currentGameState == GameStates.STATEDEMO)
                {
                    if (!ReadyForGame)
                    {
                        StallingForGameAssets = true;
                        LoadingScreenCanvas.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        StallingForGameAssets = false;
                    }
                }
                if (!StallingForGameAssets)
                {
                    OnUpdate();
                }
                if (!StallingForGameAssets)
                {
                    OnDraw();
                }

                if ((!StallingForGameAssets) && currentGameState != GameStates.STATELOADING)
                {
                    LoadingScreenCanvas.Visibility = Visibility.Collapsed;
                    LoadingSoundMediaElement.Stop();
                }

                double seconds2 = ((TimeSpan)(DateTime.Now - PageCreateTime)).TotalSeconds;
                //Globals.Debug("Begin Tick: " + seconds + " End: " + seconds2);
            }
            catch (Exception ex)
            {
                ProgressText.Text = "EX (tell Rob): " + ex.ToString();
                DebugCanvas.Visibility = Visibility.Visible;
            }


        }

        private void OnUpdate()
        {
            Globals.myGameLoop.ProcessAI();
            Globals.SoundService.Update();
        }

        private void OnDraw()
        {
            Globals.myGameLoop.RenderFrame();
        }

        void GlobalsDebugStringReceived(object sender, StringEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(e.Value);
            this.ProgressText.Text = e.Value;
        }

        void GlobalsAnalyticsStringReceived(object sender, StringEventArgs e)
        {
            if (this.m_AnalyticsService != null)
            {
                if (!string.IsNullOrEmpty(e.Value))
                {
                    m_AnalyticsService.YoAsync(App.s_SessionId, "GameAnalytic", e.Value);
                }
            }
        }


        private void UserControl_GotFocus(object sender, RoutedEventArgs e)
        {
            if (ToggleMuteWithFocus) IsMuted = false;
        }

        private void UserControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ToggleMuteWithFocus) IsMuted = true;
        }


    }
}
