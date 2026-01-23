using System;

// Bitmap loading 
public enum GameStates
{
    STATENOCHANGE, STATELOADING, STATEEXIT,
    STATETITLE, STATEDECORATE, STATEOPTIONS, STATEGAME, STATEDEMO
}
public enum BitmapSets { bmpLoadBitmaps, bmpGameBitmaps, bmpMenuBitmaps, bmpTransBitmaps }

public enum GreasepoleKeys
{
    None,
    Back,
    IncreaseMunitions,
    ShowFPS,
    BuildRingEnergy,
    GWBalloon,
    PopupArtsciorCommie,
    PopupScicon,
    PopupHose,
    Mosh,
    StartDemo,
    ClarkC,
    ClarkH,
    ClarkP
}

public class GameLoop
{
    private Random R = new Random();
    public const int nMAXFRAMESETS = 7;
    public bool IsGameRunning() { return !bGameTerminate; }
    public void KillGame() { bGameTerminate = true; }

    public bool bExitOnEscape;

    public bool bGameReset;
    public bool bMenuReset;
    public bool bOptionsReset;
    public bool bTitleReset;
    public bool bTransReset;

    private int nGameState;

    public GameStates CurrentGameState { get { return _CurrentGameState; } }
    private GameStates _CurrentGameState = GameStates.STATELOADING;

    // Initialize states of the game.
    public int nAIQueue;


    public bool bProcessingAI;
    public bool bUpdatingFrame;

    public bool bGameTerminate;

    // Frame loading / unloading functionality
    public bool[] bFrameSetLoaded = new bool[nMAXFRAMESETS];
    // end Frame loading / unloading functionality

    public GameLoop()
    {
        bExitOnEscape = true;
        nGameState = ((int)GameStates.STATENOCHANGE);
        bProcessingAI = false;
        bUpdatingFrame = false;
        nAIQueue = 0;

        int i;
        for (i = 0; i < ((int)GameBitmapEnumeration.bmpENDOFBITMAPS); i++)
        {
            AIMethods.frm[i] = new FrameDesc();
        }
        for (i = 0; i < ((int)GameBitmapEnumeration.bmpENDOFBITMAPS); i++)
        {
            AIMethods.frmM[i] = new FrameDesc();
        }
    }

    // ********************************************************
    // Main Game Loop Functions
    // ********************************************************


    public void ChangeGameState(int nState)
    {
        switch (nState)
        {
            case ((int)GameStates.STATETITLE):
            case ((int)GameStates.STATEEXIT):
                bExitOnEscape = true;
                InitTransition(nState);
                break;

            case ((int)GameStates.STATELOADING):
            case ((int)GameStates.STATEDECORATE):
            case ((int)GameStates.STATEDEMO):
            case ((int)GameStates.STATEOPTIONS):
            case ((int)GameStates.STATEGAME):
                bExitOnEscape = false;
                InitTransition(nState);
                break;
        }
        _CurrentGameState = (GameStates)nState;
    }

    public void SetGameState(int nState)
    {
        nGameState = nState;
    }
    public int GetGameState()
    { 
        return nGameState;
    }

    public bool bGameNotOver = true;
    public bool bInMainLoop = false;


    public int nRitualStateMachine = 0;

    public void TerminateApplication()
    {
        if (ApplicationTerminated != null) ApplicationTerminated(this, new EventArgs());
    }
    public event EventHandler ApplicationTerminated;

    public void HandleKeyDown(GreasepoleKeys key)
    {
        switch (key)
        {

            case GreasepoleKeys.Back:
                if (!Globals.myGameLoop.bExitOnEscape)
                //    TerminateApplication();
                //else
                {
                    if (Globals.myGameLoop.CurrentGameState == GameStates.STATEGAME)
                    {
                        int nNewHighScore = (int)(Globals.GameTimerService.GetCurrentGameTimeScoreMilliseconds());

                        Globals.Analytic("GameEndEsc;" + nNewHighScore.ToString());


                        if (nNewHighScore > Globals.myGameConditions.GetHighScore(Globals.myGameConditions.GetFroshLameness()))
                        {
                            AIMethods.lSound[((int)ASLList.lsndNARRATOR_CONGRATS)].Play(SoundbankInfo.volHOLLAR);
                            Globals.myGameConditions.SetHighScore(Globals.myGameConditions.GetFroshLameness(), nNewHighScore);
                        }
                    }
                    Globals.myGameLoop.ChangeGameState(((int)GameStates.STATETITLE));
                }
                break;
            case GreasepoleKeys.IncreaseMunitions:
                if (nRitualStateMachine == 2)
                {
                    Globals.myGameConditions.GetApples(5);
                    Globals.myGameConditions.GetPizzas(4);
                    Globals.myGameConditions.GetClarks(3);
                    Globals.myGameConditions.GetExams(1);
                }
                break;
            case GreasepoleKeys.ShowFPS: // FPS Counter
                AIMethods.gbShowFPS = !(AIMethods.gbShowFPS);
                break;
            case GreasepoleKeys.BuildRingEnergy: // Energy for Iron Ring
                if (nRitualStateMachine == 2)
                    AIMethods.sprForge.nAttrib[((int)nattrForge.attrForgeEnergy)] = AIMethods.energySwing;
                break;
            case GreasepoleKeys.GWBalloon: // GW Balloon
                if (nRitualStateMachine == 2)
                    if (AIMethods.sprGWBalloon == null)
                    {
                        AIMethods.sprGWBalloon = SpriteInit.CreateSprite((SpriteType.sprGWBALLOON));
                        AIMethods.ssBalloon.Include(AIMethods.sprGWBalloon);
                    }
                break;
            case GreasepoleKeys.PopupArtsciorCommie: // Popup Artscis and Commies
                if (nRitualStateMachine == 2)
                    if (AIMethods.sprAlien == null)
                    {
                        switch (R.Next(4))
                        {
                            case 0: AIMethods.sprAlien = SpriteInit.CreateSprite((SpriteType.sprPOPUP_ARTSCIF)); break;
                            case 1: AIMethods.sprAlien = SpriteInit.CreateSprite((SpriteType.sprPOPUP_ARTSCIM)); break;
                            case 2: AIMethods.sprAlien = SpriteInit.CreateSprite((SpriteType.sprPOPUP_COMMIEF)); break;
                            case 3: AIMethods.sprAlien = SpriteInit.CreateSprite((SpriteType.sprPOPUP_COMMIEM)); break;
                        }
                        AIMethods.ssPit.Include(AIMethods.sprAlien);
                    }
                break;
            case GreasepoleKeys.PopupScicon: // Popup SciCons
                if (nRitualStateMachine == 2)
                    AIMethods.ssPit.Include(SpriteInit.CreateSprite(1 == R.Next(2) ? (SpriteType.sprSCICONF) : (SpriteType.sprSCICONM)));
                break;
            case GreasepoleKeys.PopupHose: // Hose
                if (nRitualStateMachine == 2)
                    AIMethods.ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprPOPUP_HOSE)));
                break;
            case GreasepoleKeys.Mosh: // Mosh
                if (nRitualStateMachine == 2)
                    AIMethods.aisStartAMosh();
                break;
            case GreasepoleKeys.StartDemo: // Invoke Demo
                Globals.myGameLoop.ChangeGameState(((int)GameStates.STATEDEMO));
                break;
            case GreasepoleKeys.ClarkC:
                nRitualStateMachine = 1;
                break;

            case GreasepoleKeys.ClarkH:
                if (nRitualStateMachine == 1)
                    nRitualStateMachine = 2;
                break;

            case GreasepoleKeys.ClarkP:
                if (nRitualStateMachine == 2)
                {
                    nRitualStateMachine = 0;
                    // RITUAL VERSION COMMENTED OUT NO MORE ;]
                    AIMethods.lSound[((int)ASLList.lsndDISCIPLINES_RITUAL)].Play(SoundbankInfo.volHOLLAR);
                    for (int i = 0; i < AIMethods.ssIcons.GetNumberOfSprites(); i++)
                    {
                        if (AIMethods.ssIcons.GetSprite(i).SpriteType == (SpriteType.sprmnuBAR20))
                            AIMethods.ssIcons.GetSprite(i).nAttrib[((int)attrBar.attrBarGroup)] = 1;
                    }
                }
                break;

        }
    }

    private bool LoadedLongSounds = false;
    private bool LoadedGameBitmaps = false;
    private bool LoadedTransBitmaps = false;
    private bool LoadedMenuBitmaps = false;
    private bool SplashscreenLoadingFinished = false;
    private int splashscreenLoadingPercentage;

    /// <summary>
    /// Call after everything is ready - automatically called in XNA version by ProcessAI.
    /// </summary>
    public void SplashscreenLoadingFinalize()
    {
        SplashscreenLoadingFinished = true;
        Globals.myGameConditions.LoadSettingsFromStorage();
        Globals.GameTimerService.ResumeUpdateCountTimer();
        Globals.GameTimerService.ResetGameTimeScore();
        if (AIMethods.gbStartAtGame)
        {
            Globals.Debug("Debug feature activated: Starting Pole Game directly");
            InitGame();
        }
        else
        {
            Globals.Debug("Calling Up Pole Game Main Menu");
            InitTitle();
        }

    }

    public int SplashScreenLoadingPercentage
    {
        get { return Math.Max(splashscreenLoadingPercentage, 100); }
    }
	

    public bool ProcessAI()
    {
        if (!SplashscreenLoadingFinished)
        {
            if (!LoadedLongSounds)
            {
                splashscreenLoadingPercentage += 10;
                Globals.Debug("Initializing 'Long' Sounds.");
                if (Globals.SoundService.LoadLongSounds(25))
                    LoadedLongSounds = true;
            }
            else if (!LoadedGameBitmaps)
            {
                if (splashscreenLoadingPercentage < 25) splashscreenLoadingPercentage+=2;
                else
                {
                    splashscreenLoadingPercentage += 5;
                    Globals.Debug("Initializing 'Game' Bitmaps.");
                    if (Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpGameBitmaps, 25))
                        LoadedGameBitmaps = true;
                }
            }
            else if (!LoadedTransBitmaps)
            {
                if (splashscreenLoadingPercentage < 50) splashscreenLoadingPercentage+=2;
                else
                {
                    splashscreenLoadingPercentage += 5;
                    Globals.Debug("Initializing 'Trans' Bitmaps.");
                    if (Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpTransBitmaps, 100))
                        LoadedTransBitmaps = true;
                }
            }
            else if (!LoadedMenuBitmaps)
            {
                if (splashscreenLoadingPercentage < 75) splashscreenLoadingPercentage+=2;
                else
                {
                    splashscreenLoadingPercentage += 5;
                    Globals.Debug("Initializing 'Menu' Bitmaps.");
                    if (Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpMenuBitmaps, 100))
                        LoadedMenuBitmaps = true;
                }
            }
            else
            {
                if (splashscreenLoadingPercentage < 100) splashscreenLoadingPercentage += 2;
                else SplashscreenLoadingFinished = true;
            }
            if (SplashscreenLoadingFinished)
            {
                SplashscreenLoadingFinalize();
            }
            return true;

        }



        // Compact spritesets and process the Artificial Intelligence for the game.
        if (bUpdatingFrame)
        {
            Globals.Debug("ATTEMPT TO PROCESS AI WHILE UPDATING FRAME");
            return false;
        }

        bProcessingAI = true;
        

        Globals.GameTimerService.Update();
        nAIQueue += Globals.GameTimerService.GetAdditionalUpdateCount();
        while (nAIQueue > 0)
        {
            nAIQueue--;
            GreasepoleKeys currentKey = Globals.InputService.GetKeyboardInput();
            if (currentKey != GreasepoleKeys.None) HandleKeyDown(currentKey);

            if (!(nGameState == ((int)GameStates.STATENOCHANGE)))
            {
                switch (nGameState)
                {
                    case ((int)GameStates.STATELOADING):
                        InitTransition(((int)GameStates.STATETITLE));
                        break;
                    case ((int)GameStates.STATETITLE):
                        InitTitle();
                        break;
                    case ((int)GameStates.STATEDECORATE):
                        InitDecorate();
                        break;
                    case ((int)GameStates.STATEOPTIONS):
                        InitOptions();
                        break;
                    case ((int)GameStates.STATEEXIT):
                        InitTransition(((int)GameStates.STATEEXIT));
                        break;
                    case ((int)GameStates.STATEGAME):
                        InitGame();
                        break;
                    case ((int)GameStates.STATEDEMO):
                        InitGame(true);
                        break;
                }
                nAIQueue = 0;
            }

            double temp = (Globals.GameTimerService.GetCurrentGameTimeScoreMilliseconds());
            if (!Globals.myGameConditions.IsGameOver())
            {
                AIMethods.gnPitTimeS = (int)(temp / 10) % 100;
                AIMethods.gnPitTimeM = (int)(temp) / 1000 % 60;
                AIMethods.gnPitTimeH = (int)(temp) / 1000 / 60;
            }

            // Compact any of the SpriteSets that can have deleted sprites in them.
            AIMethods.ssClouds.Compact(); // The clouds and sky
            AIMethods.ssBalloon.Compact(); // The clouds and sky
            AIMethods.ssSkyline.Compact(); // The skyline
            AIMethods.ssTrees.Compact(); // The skyline
            AIMethods.ssFrecs.Compact();
            AIMethods.ssWater.Compact();
            AIMethods.ssPit.Compact();    // Frosh behind the pole
            AIMethods.ssTossed.Compact();
            AIMethods.ssIcons.Compact(); AIMethods.ssConsole.Compact();
            AIMethods.ssTossed.Compact();
            AIMethods.ssMouse.Compact();
            AIMethods.ssJacketSlam.Compact();

            // Think.
            AIMethods.ssClouds.Think(); // The background
            AIMethods.ssBalloon.Think();
            AIMethods.ssSkyline.Think();
            AIMethods.ssTrees.Think();
            AIMethods.ssFrecs.Think();
            AIMethods.ssWater.Think();
            AIMethods.ssPit.Think();    // Frosh behind the pole
            AIMethods.ssTossed.Think();
            AIMethods.ssIcons.Think(); AIMethods.ssConsole.Think();
            AIMethods.ssMouse.Think();
            AIMethods.ssJacketSlam.Think();

            Globals.InputService.Update();
            if (Globals.myGameConditions.IsDemo())
                Globals.myLayers.ForceScroll(DemoScroll());
            Globals.myLayers.ScrollScreen();

            // Arrange.
            AIMethods.ssClouds.CalculateScreenCoordinates(); // The clouds and sky
            AIMethods.ssBalloon.CalculateScreenCoordinates(); // The clouds and sky
            AIMethods.ssSkyline.CalculateScreenCoordinates(); // The skyline
            AIMethods.ssTrees.CalculateScreenCoordinates(); // The skyline
            AIMethods.ssFrecs.CalculateScreenCoordinates();
            AIMethods.ssWater.CalculateScreenCoordinates();
            AIMethods.ssPit.CalculateScreenCoordinates();    // Frosh behind the pole
            AIMethods.ssTossed.CalculateScreenCoordinates();
            AIMethods.ssIcons.CalculateScreenCoordinates();
            AIMethods.ssConsole.CalculateScreenCoordinates();
            AIMethods.ssMouse.CalculateScreenCoordinates();
            AIMethods.ssJacketSlam.CalculateScreenCoordinates();

        }
        bProcessingAI = false;

        return true;
    } /* ProcessAI */

    public int DemoScroll()
    {
        int n = AIMethods.ssFr.GetNumberOfSprites();
        int nScroll = 0;
        int nHighest;
        bool bDoNotScrollDown = false;

        for (int i = 0; i < n; i++)
        {
            nHighest = AIMethods.ssFr.GetSprite(i).nScry;
            if (nHighest <= 10)
            {
                nScroll += 1;
                bDoNotScrollDown = true;
            }
            if (nHighest <= 18)
                bDoNotScrollDown = true;
        }
        return (bDoNotScrollDown ? nScroll : nScroll - 1);

    }
    private static byte[] BlackBytes = new byte[3];
    private static TSprite SplashScreenSprite1 = null;
    private static TSprite SplashScreenSprite2 = null;

    private void RenderSplashScreen()
    {
        AIMethods.frm[(int)GameBitmapEnumeration.bmpSPLASHSCREEN].Draw(SplashScreenSprite1, 0, 0, BlackBytes, BlackBytes);
        AIMethods.frm[(int)GameBitmapEnumeration.bmpBTNTOGGLE].Draw(SplashScreenSprite2, (int)((float)SplashScreenLoadingPercentage * 5.8), 400, BlackBytes, BlackBytes); 
    }

    public void RenderFrame()
    {
        Globals.RenderingService.Drawing();
        if (!SplashscreenLoadingFinished)
        {
            RenderSplashScreen();
        }
        else
        {

            // P/re-condition: Ready to generate another frame
            // Post-condition: Next frame ready to be flipped to the screen.
            if (bUpdatingFrame)
                Globals.Debug("ATTEMPT TO UPDATE FRAME WHILE UPDATING FRAME");
            if (bProcessingAI)
                Globals.Debug("ATTEMPT TO UPDATE FRAME WHILE PROCESSING AI");

            if ((!bProcessingAI) && (!bUpdatingFrame) && (!bGameTerminate))
            {
                bUpdatingFrame = true;
                AIMethods.ssClouds.Draw(); // The background
                AIMethods.ssBalloon.Draw(); // The trees
                AIMethods.ssTrees.Draw(); // The trees
                AIMethods.ssSkyline.Draw(); // The U-Hurl, etc.
                AIMethods.ssFrecs.Draw();
                AIMethods.ssWater.Draw();
                AIMethods.ssPit.OrderByY();
                AIMethods.ssPit.DrawMultiCultural();       // Frosh and pole
                AIMethods.ssTossed.Draw();    // Tossed items
                AIMethods.ssConsole.Draw(); AIMethods.ssIcons.Draw();    // Console and Icons
                AIMethods.ssMouse.Draw();   // Mouse	
                AIMethods.ssJacketSlam.Draw();   // Mouse	

            }
        }

        Globals.RenderingService.RenderToScreen();
        bUpdatingFrame = false;
        Globals.RenderingService.Drawn();

    } /* updateFrame */



    /// <summary>
    /// Initialize Globals, Renderer, Sound and prepare for first entry into update/render loop.
    /// </summary>
    /// <returns></returns>
    public bool InitializeGame()
    {
        // Initialize Globals
        Globals.Debug("Initializing Globals");
        if (!Initialize_Globals())
        {
            Globals.Debug("Failed!");
            return false;
        }



        // Initialize the graphics engine
        Globals.Debug("Initializing Rendering Service");
        if (!Globals.RenderingService.Initialize())
        {
            Globals.Debug("Failed!");
            return false;
        }

        Globals.Debug("Initializing Sound Service");
        Globals.SoundService.Initialize();

        Globals.Debug("Initializing Once-Per-Game Actions:");
        Globals.Debug("Initializing Layers");
        if (!Initialize_Layers())
            return false;
        Globals.Debug("Initializing SpriteSets");
        if (!Initialize_SpriteSets())
            return false;
        Globals.Debug("Initializing Pole Positions");
        if (!PolePosition.Initialize_PolePositions())
            return false;

        Globals.Debug("Initializing 'Short' Sounds");
        if (!Globals.SoundService.LoadShortSounds())
            return false;
#if SILVERLIGHT
#else
        if (AIMethods.sSound[((int)ASSList.ssndMENU_LOADREPEAT)] != null)
            AIMethods.sSound[((int)ASSList.ssndMENU_LOADREPEAT)].Loop(SoundbankInfo.volMUSIC);
#endif
        Globals.Debug("Initializing 'Load' Bitmaps");
        if (!Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpLoadBitmaps, 100))
            return false;

        Globals.Debug("...Pole Game Initialized.  Now for the good stuff.");


        //MainGameLoop();

        return true;
    }


    public bool Initialize_Globals()
    {
        // Reset bFrameSetLoaded array
        for (int i = 0; i < nMAXFRAMESETS; i++)
            bFrameSetLoaded[i] = false;

        bGameTerminate = false;
        bGameReset = false;
        bTransReset = false;
        bMenuReset = false;

        return true;
    }


    public bool Initialize_Layers()
    {
        // Initialize the Layers and set up their default values.
        Globals.myLayers = new Layers();
        return true;
    }

    public bool Flush_SpriteSets(bool bSaveJacketSlam)
    {
        AIMethods.ssClouds.Flush();
        AIMethods.ssBalloon.Flush();
        AIMethods.ssSkyline.Flush();
        AIMethods.ssTrees.Flush();
        AIMethods.ssTrees.Flush();
        AIMethods.ssFrecs.Flush();
        AIMethods.ssWater.Flush();
        AIMethods.ssFr.Flush(SpriteSet.ssDONOTDELETE);
        AIMethods.ssPit.Flush();
        AIMethods.ssTossed.Flush();
        AIMethods.ssConsole.Flush(); AIMethods.ssIcons.Flush();
        AIMethods.ssMouse.Flush();
        if (!bSaveJacketSlam)
            AIMethods.ssJacketSlam.Flush();

        AIMethods.ssFr.Flush();
        return true;
    }


    public bool Initialize_SpriteSets()
    {
        // Initialize the SpriteSets and set up their Layers.

        AIMethods.ssClouds = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERSKY)));
        AIMethods.ssBalloon = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERSKY)));
        AIMethods.ssSkyline = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERSKYLINE)));
        AIMethods.ssTrees = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERTREE)));
        AIMethods.ssFrecs = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERFREC)));
        AIMethods.ssWater = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERPIT)));
        AIMethods.ssFr = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERPIT)));
        AIMethods.ssPit = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERPIT)));
        AIMethods.ssTossed = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERPIT)));
        AIMethods.ssConsole = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERMISC))); AIMethods.ssIcons = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERMISC)));
        AIMethods.ssMouse = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERMISC)));
        AIMethods.ssJacketSlam = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERMISC))); // The jacket ka-blam

        AIMethods.ssFr = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERPIT)));
        return true;
    }


    public const int NUMBEROFSOUNDS = 240;






    // GAMEFLOWINIT

    public void InitTransition(int nNextState)
    {
        //Globals.GameTimerService.PauseUpdateCountTimer();

        //Globals.Debug("Initializing Jacket Slam.");
        //if (!Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpTransBitmaps))
        //{
        //    Globals.Debug("Can't initialize Transitional bitmaps!"); return;
        //}

        Initialize_TransitionSprites(nNextState);
        //Globals.GameTimerService.ResumeUpdateCountTimer();

        nGameState = ((int)GameStates.STATENOCHANGE);
    }

    public static int bPlayRiffOnlyOnce = 0;

    public void InitTitle()
    {
        Globals.GameTimerService.PauseUpdateCountTimer();
        
        Globals.myGameConditions.SaveSettingsToStorage();

        //Globals.Debug("Initializing 'Menu' Bitmaps for TITLE screen.");
        //if (!Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpMenuBitmaps))
        //{
        //    Globals.Debug("Can't initialize Menu bitmaps!"); return;
        //}


        if (!(AIMethods.sSound[((int)ASSList.ssndMENU_DECORATEREPEAT)].IsPlaying())
            && !(AIMethods.sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].IsPlaying())
            && 0 == bPlayRiffOnlyOnce)
        {
            AIMethods.lSound[((int)ASLList.lsndMUSIC_TITLEINIT)].Play(SoundbankInfo.volMUSIC);
            AIMethods.sSound[((int)ASSList.ssndMENU_DECORATEREPEAT)].Stop();
#if SILVERLIGHT
#else
            AIMethods.sSound[((int)ASSList.ssndMENU_LOADREPEAT)].Stop();
#endif
        }

        bPlayRiffOnlyOnce = 1;

        Globals.Debug("Flushing Spritesets");
        Flush_SpriteSets(true);

        Globals.Debug("Initializing Title Sprites");
        if (!Initialize_TitleSprites())
            Globals.Debug("Couldn't init Title sprites!");

        Globals.GameTimerService.ResumeUpdateCountTimer();
        nGameState = ((int)GameStates.STATENOCHANGE);
        _CurrentGameState = GameStates.STATETITLE;
    }

    public void InitDecorate()
    {
        Globals.GameTimerService.PauseUpdateCountTimer();

        //Globals.Debug("Initializing 'Menu' Bitmaps for DECORATE screen.");
        //if (!Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpMenuBitmaps))
        //{
        //    Globals.Debug("Can't initialize Menu bitmaps!");
        //    return;
        //}
        #if SILVERLIGHT
#else

        AIMethods.sSound[((int)ASSList.ssndMENU_LOADREPEAT)].Stop();
#endif

        Globals.Debug("Flushing Spritesets");
        Flush_SpriteSets(true);

        AIMethods.lSound[((int)ASLList.lsndNARRATOR_JACKETINIT)].Play(SoundbankInfo.volHOLLAR);

        Globals.Debug("Initializing Jacket Decorating Sprites");
        if (!Initialize_DecorateSprites())
            Globals.Debug("Couldn't init Jacket Decorating sprites");

        nGameState = ((int)GameStates.STATENOCHANGE);
        _CurrentGameState = GameStates.STATEDECORATE;
        Globals.GameTimerService.ResumeUpdateCountTimer();
    }
    public void InitOptions()
    {
        Globals.GameTimerService.PauseUpdateCountTimer();
        //Globals.Debug("Initializing 'Menu' Bitmaps for OPTIONS screen.");
        //if (!Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpMenuBitmaps))
        //{
        //    Globals.Debug("Can't initialize Menu bitmaps!");
        //    return;
        //}

//        Globals.Debug("Initializing Options Screen Sprites");
        Globals.Debug("Initializing Achievements Screen Sprites");
        if (!Initialize_OptionSprites())
            Globals.Debug("Couldn't init Options sprites");

//        AIMethods.lSound[((int)ASLList.lsndNARRATOR_OPTIONINIT)].Play(SoundbankInfo.volHOLLAR);
        AIMethods.lSound[((int)ASLList.lsndNARRATOR_STARTDELAY1)].Stop();
        AIMethods.lSound[((int)ASLList.lsndNARRATOR_STARTDELAY2)].Stop();
        Globals.GameTimerService.ResumeUpdateCountTimer();
        _CurrentGameState = GameStates.STATEOPTIONS;
        nGameState = ((int)GameStates.STATENOCHANGE);

    }

    public void InitGame()
    {
        InitGame(false);
    }


    public void InitGame(bool bDemo)
    {

        Globals.GameTimerService.PauseUpdateCountTimer();
        Globals.myGameConditions.Reset(bDemo);

        // Loading music
        AIMethods.sSound[((int)ASSList.ssndMENU_DECORATEREPEAT)].Stop();
        AIMethods.sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].Stop();
#if SILVERLIGHT
#else
        AIMethods.sSound[((int)ASSList.ssndMENU_LOADREPEAT)].Stop();
#endif
        AIMethods.sSound[((int)ASSList.ssndMENU_GAMEINIT)].Play(SoundbankInfo.volHOLLAR);

        // START UP THE GAME
        //Globals.Debug("Initializing 'Game' Bitmaps");
        //if (!Globals.RenderingService.LoadBitmapSet(BitmapSets.bmpGameBitmaps))
        //{
        //    Globals.Debug("Can't initialize Game bitmaps!");
        //    return;
        //}

        AIMethods.sSound[((int)ASSList.ssndEFFECTS_CROWDMURMUR)].Loop(SoundbankInfo.volCROWD);

        Globals.Debug("Flushing Spritesets");
        Flush_SpriteSets(true);

        Globals.Debug("Initializing Game Sprites");
        if (!Initialize_GameSprites(bDemo))
            Globals.Debug("Couldn't init Game sprites");

        AIMethods.sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + R.Next(2)].Play(SoundbankInfo.volCROWD);

        // "Game" Resetting stuff
        Globals.GameTimerService.ResumeUpdateCountTimer();
        Globals.GameTimerService.ResetGameTimeScore();
        if (!bDemo) Globals.myLayers.ResetForGame();

        nGameState = ((int)GameStates.STATENOCHANGE);
        _CurrentGameState = GameStates.STATEGAME;
    }

    ///////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////
    // Sprite Init ////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////


    public bool Initialize_TransitionSprites(int nNextState)
    {
        if (AIMethods.ssJacketSlam.n == 0)
        {
            TSprite tmpSprite;
            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprSLAMJACKET), 0, 0);
            tmpSprite.nAttrib[((int)attrJacketSlam.attrNextState)] = nNextState;
            AIMethods.ssJacketSlam.Include(tmpSprite);
        }
        return true;
    }
    public bool Initialize_TitleSprites()
    {
        TSprite tmpSprite; // Add a Splashscreen sunken into the background
        tmpSprite = SpriteInit.CreateSprite((SpriteType.sprCLOUDS), 0, 0);
        tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpSPLASHSCREEN)]);
        AIMethods.ssIcons.Include(tmpSprite);

        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuTITLEBACK)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuTITLESTART)));

        //int nTemp;

        int highScore = Globals.myGameConditions.GetHighScore(Globals.myGameConditions.GetFroshLameness());

        int STOFFSET = -180;
        int dHIGHSCORETITLEHEIGHT = 415;
        if (0 != highScore)
        {
            int highScoreHours = (int)(highScore) / 1000 / 60;
            int highScoreMinutes = (int)(highScore) / 1000 % 60; 
            int highScoreSeconds = (int)(highScore / 10) % 100;

            // HOURS
            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 275 - STOFFSET, dHIGHSCORETITLEHEIGHT);

            if (highScoreHours < 10) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]); else tmpSprite.SetFrame(AIMethods.TensDigitFrame(highScoreHours));
            AIMethods.ssIcons.Include(tmpSprite);

            AIMethods.aiInitFlyInAndOut(tmpSprite, AIMethods.aiInanimate, tmpSprite.nX, tmpSprite.nY - 480, tmpSprite.nX, tmpSprite.nY, 2, 2);

            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 295 - STOFFSET, dHIGHSCORETITLEHEIGHT);
            if (highScoreHours <= 0) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]); else tmpSprite.SetFrame(AIMethods.OnesDigitFrame(highScoreHours));
            AIMethods.ssIcons.Include(tmpSprite);
            AIMethods.aiInitFlyInAndOut(tmpSprite, AIMethods.aiInanimate, tmpSprite.nX, tmpSprite.nY - 480, tmpSprite.nX, tmpSprite.nY, 2, 2);

            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 315 - STOFFSET, dHIGHSCORETITLEHEIGHT);
            tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpALP_COLON)]);
            AIMethods.ssIcons.Include(tmpSprite);
            AIMethods.aiInitFlyInAndOut(tmpSprite, AIMethods.aiInanimate, tmpSprite.nX, tmpSprite.nY - 480, tmpSprite.nX, tmpSprite.nY, 2, 2);

            // MINUTES
            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 335 - STOFFSET, dHIGHSCORETITLEHEIGHT);
            if (highScoreMinutes < 10) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]); else tmpSprite.SetFrame(AIMethods.TensDigitFrame(highScoreMinutes));
            AIMethods.ssIcons.Include(tmpSprite);
            AIMethods.aiInitFlyInAndOut(tmpSprite, AIMethods.aiInanimate, tmpSprite.nX, tmpSprite.nY - 480, tmpSprite.nX, tmpSprite.nY, 2, 2);

            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 355 - STOFFSET, dHIGHSCORETITLEHEIGHT);
            if (highScoreMinutes < 0) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]); else tmpSprite.SetFrame(AIMethods.OnesDigitFrame(highScoreMinutes));
            AIMethods.ssIcons.Include(tmpSprite);
            AIMethods.aiInitFlyInAndOut(tmpSprite, AIMethods.aiInanimate, tmpSprite.nX, tmpSprite.nY - 480, tmpSprite.nX, tmpSprite.nY, 2, 2);

            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 375 - STOFFSET, dHIGHSCORETITLEHEIGHT);
            tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpALP_PERIOD)]);
            AIMethods.ssIcons.Include(tmpSprite);
            AIMethods.aiInitFlyInAndOut(tmpSprite, AIMethods.aiInanimate, tmpSprite.nX, tmpSprite.nY - 480, tmpSprite.nX, tmpSprite.nY, 2, 2);

            // SECONDS
            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 395 - STOFFSET, dHIGHSCORETITLEHEIGHT);
            if (highScoreSeconds < 10) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]); else tmpSprite.SetFrame(AIMethods.TensDigitFrame(highScoreSeconds));
            AIMethods.ssIcons.Include(tmpSprite);
            AIMethods.aiInitFlyInAndOut(tmpSprite, AIMethods.aiInanimate, tmpSprite.nX, tmpSprite.nY - 480, tmpSprite.nX, tmpSprite.nY, 2, 2);

            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 415 - STOFFSET, dHIGHSCORETITLEHEIGHT);
            if (highScoreSeconds <= 0) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]); else tmpSprite.SetFrame(AIMethods.OnesDigitFrame(highScoreSeconds));
            AIMethods.ssIcons.Include(tmpSprite);
            AIMethods.aiInitFlyInAndOut(tmpSprite, AIMethods.aiInanimate, tmpSprite.nX, tmpSprite.nY - 480, tmpSprite.nX, tmpSprite.nY, 2, 2);
        }

        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuTITLEEXIT)));

        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuTITLEOPTIONS)));

        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuMOUSECURSOR)));

        if (Globals.myGameConditions.gbTriPubBan)
        {
            AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprTRI)));
            AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPUB)));
            AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprBAN)));
        }

        nGameState = ((int)GameStates.STATENOCHANGE);

        return true;
    }

    public bool Initialize_OptionSprites()
    {
        int j = 0;
        while (j < AIMethods.ssIcons.GetNumberOfSprites())
        {
            if (AIMethods.ssIcons.GetSprite(j).SpriteType == (SpriteType.sprmnuTITLESTART)
                || AIMethods.ssIcons.GetSprite(j).SpriteType == (SpriteType.sprmnuTITLEOPTIONS)
                || AIMethods.ssIcons.GetSprite(j).SpriteType == (SpriteType.sprmnuTITLEEXIT))
                AIMethods.ssIcons.GetSprite(j).bDeleted = true;
            j++;
        }

        TSprite sprTmp;
        sprTmp = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 0, 266);
        sprTmp.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLESTART)]);
        AIMethods.ssIcons.Include(sprTmp);
        sprTmp = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 0, 346);
        sprTmp.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLEOPTIONSGLOW3)]);
        AIMethods.ssIcons.Include(sprTmp);
        sprTmp = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 45, 27);
        sprTmp.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLEEXIT)]);
        AIMethods.ssIcons.Include(sprTmp);

        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuOPTIONSBACK)));

        // For change to the achievements screen
        //for (int i = 0; i < 5; i++)
        //{
        //    if (i != 1)
        //        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuBTNTOGGLE0) + i));
        //}

        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuOPTIONSRETURN)));

        Globals.myGameConditions.SetAchievementGroup(0);

        int achievementsPerScreen = 6;
        int currentAchievementGroup = 0;
        int achievementsThisScreen = 0;
        for (int i = 0; i < PoleGameAchievement.List.Count; i++)
        {
            PoleGameAchievement achievement = PoleGameAchievement.List[i];

            int textHeight = 24;
            int textX = 125;
            int pointsX = 80;
            int froshHeadX = 46;
            int textY = 60;
            int targetY = textY + (int)((double)textHeight * 2.4) * achievementsThisScreen;

            // Name
            TSprite textSprite = SpriteInit.CreateSprite((SpriteType.sprmnuACHIEVEMENTTEXT));
            textSprite.nX = 650; 
            textSprite.nAttrib[((int)attrBar.attrBarGroup)] = currentAchievementGroup;
            textSprite.nAttrib[((int)attrBar.attrOnScreenX)] = textX;
            textSprite.nAttrib[((int)attrBar.attrOnScreenY)] = targetY;            
            textSprite.Text = (!(achievement.Secret && !achievement.Achieved)) ? achievement.Name : "Secret Achievement";
            textSprite.bAttrib[5] = achievement.Achieved;
            AIMethods.aiInitFlyInAndOut(textSprite, AIMethods.aiAchievementText, 640, 490, 700, textSprite.nAttrib[((int)attrBar.attrOnScreenY)], 1, 1);
            AIMethods.ssIcons.Include(textSprite);

            // Description
            TSprite textSprite2 = SpriteInit.CreateSprite((SpriteType.sprmnuACHIEVEMENTTEXT));
            textSprite2.nX = 650;
            textSprite2.nAttrib[((int)attrBar.attrBarGroup)] = currentAchievementGroup;
            textSprite2.nAttrib[((int)attrBar.attrOnScreenX)] = textX;
            textSprite2.nAttrib[((int)attrBar.attrOnScreenY)] = targetY + textHeight;
            textSprite2.Text = (!(achievement.Secret && !achievement.Achieved)) ? achievement.Description : "Keep stalling the frosh to discover this achievement!";
            textSprite2.bAttrib[5] = achievement.Achieved;
            AIMethods.aiInitFlyInAndOut(textSprite2, AIMethods.aiAchievementText, 640, 490, 700, textSprite2.nAttrib[((int)attrBar.attrOnScreenY)], 1, 1);
            AIMethods.ssIcons.Include(textSprite2);

            // Points value
            TSprite textSprite3 = SpriteInit.CreateSprite((SpriteType.sprmnuACHIEVEMENTTEXT));
            textSprite3.nX = 650;
            textSprite3.nAttrib[((int)attrBar.attrBarGroup)] = currentAchievementGroup;
            textSprite3.nAttrib[((int)attrBar.attrOnScreenX)] = pointsX;
            textSprite3.nAttrib[((int)attrBar.attrOnScreenY)] = targetY + (textHeight / 2);
            textSprite3.Text = (!(achievement.Secret && !achievement.Achieved)) ? achievement.Value.ToString() : "";
            textSprite3.bAttrib[5] = achievement.Achieved;
            AIMethods.aiInitFlyInAndOut(textSprite3, AIMethods.aiAchievementText, 640, 490, 700, textSprite3.nAttrib[((int)attrBar.attrOnScreenY)], 1, 1);
            AIMethods.ssIcons.Include(textSprite3);

            // Froshhead
            if (achievement.Achieved)
            {
                TSprite achievementUnlocked = SpriteInit.CreateSprite((SpriteType.sprmnuBTNTOGGLE0));
                achievementUnlocked.nX = 650;
                achievementUnlocked.nAttrib[((int)attrBar.attrOnScreenX)] = froshHeadX;
                achievementUnlocked.nAttrib[((int)attrBar.attrOnScreenY)] = targetY + (textHeight / 2) + 2;
                achievementUnlocked.nAttrib[((int)attrBar.attrBarGroup)] = currentAchievementGroup;
                AIMethods.aiInitFlyInAndOut(achievementUnlocked, AIMethods.aiAchievementText, 640, 490, 700, achievementUnlocked.nAttrib[((int)attrBar.attrOnScreenY)], 1, 1);
                AIMethods.ssIcons.Include(achievementUnlocked);
            }

            achievementsThisScreen++;
            if ((i + 1) % achievementsPerScreen == 0)
            {
                achievementsThisScreen = 0;
                currentAchievementGroup++;
            }
        }

        int nAchieved = 0;
        int nTotal = 0;
        int ptsAchieved = 0;
        int ptsTotal = 0;
        foreach (PoleGameAchievement a in PoleGameAchievement.List)
        {
            nTotal++;
            ptsTotal += a.Value;
            if (a.Achieved) { nAchieved++; ptsAchieved += a.Value; }

        }

        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuACHIEVEMENTADDITIONALTEXT), 565, 414));
        TSprite stats = SpriteInit.CreateSprite((SpriteType.sprmnuACHIEVEMENTADDITIONALTEXT), 125, 414);
        stats.SetBehavior(AIMethods.aiInanimate);
        stats.Text = "Total: " + nAchieved + "/" + nTotal + " achieved (" + ptsAchieved + "/" + ptsTotal + " points)";
        AIMethods.ssIcons.Include(stats);


        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuAINEXTACHIEVEMENTSCREEN), 480, 423));


        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuMOUSECURSOR)));


        nGameState = ((int)GameStates.STATENOCHANGE);
        return true;
    }



    public bool Initialize_DecorateSprites()
    {
        // Initialize the sprites required in various SpriteSets in menu
        TSprite tmpSprite; // Add a Title Screen sunken into the background
        tmpSprite = SpriteInit.CreateSprite((SpriteType.sprCLOUDS), 0, 0);
        tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLEBACK)]);
        AIMethods.ssIcons.Include(tmpSprite);
        TSprite sprTmp;
        sprTmp = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), -85, 266);
        sprTmp.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLESTARTGLOW3)]);
        AIMethods.ssIcons.Include(sprTmp);
        sprTmp = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 109, 31);
        sprTmp.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLEEXIT)]);
        AIMethods.ssIcons.Include(sprTmp);
        sprTmp = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), -75, 346);
        sprTmp.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLEOPTIONS)]);
        AIMethods.ssIcons.Include(sprTmp);

        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuJACKETBACK)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuMENUJACKET)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuMENUPASSCREST)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuDECORATERETURN)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuTXTSELECT)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuAIPREVBARSCREEN), 480, 423));
        for (int i = 0; i < 20; i++)
            AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuBAR1) + i));

        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprmnuMOUSECURSOR)));
        return true;

    }
    public const int SCRW = 640;
    public const int SCRH = 480;
    public bool Initialize_GameSprites(bool bDemo)
    {
        // Initialize the sprites required in various SpriteSets in game
        AIMethods.HasTossed114Exam = bDemo;

        if (!(AIMethods.sprAlien == null)) AIMethods.sprAlien = null; // The memory will have already been freed.
        if (!(AIMethods.sprGWBalloon == null)) AIMethods.sprGWBalloon = null; // The memory will have already been freed.
        if (!(AIMethods.sprGWHippo == null)) AIMethods.sprGWHippo = null; // The memory will have already been freed.
        if (!(AIMethods.sprPopBoy == null)) AIMethods.sprPopBoy = null; // The memory will have already been freed.
        if (!(AIMethods.sprPowerMeter == null)) AIMethods.sprPowerMeter = null; // The memory will have already been freed.
        if (!(AIMethods.sprWaterMeter == null)) AIMethods.sprWaterMeter = null; // The memory will have already been freed.
        if (!(AIMethods.sprRingMeter == null)) AIMethods.sprRingMeter = null; // The memory will have already been freed.

        // CHECK THIS FOR MEMORY LEAKS.  BEFORE EACH CREATESPRITE FOR A 
        // SPECIAL SPRITE, SOMETHING SHOULD BE DELETED.
        // Background
        AIMethods.ssClouds.Include(SpriteInit.CreateSprite((SpriteType.sprCLOUDS), 0, 285));
        AIMethods.ssTrees.Include(SpriteInit.CreateSprite((SpriteType.sprTREES), 0, 0));

        // George
        AIMethods.sprPrez = SpriteInit.CreateSprite((SpriteType.sprPREZ), 200, -180);
        AIMethods.ssSkyline.Include(AIMethods.sprPrez);

        // Podium, trucks, etc.
        AIMethods.ssSkyline.Include(SpriteInit.CreateSprite((SpriteType.sprBACKDROP), 0, 0));

        AIMethods.sprRandomEventGenerator = SpriteInit.CreateSprite((SpriteType.sprPODIUM), 180, -23);
        AIMethods.ssSkyline.Include(AIMethods.sprRandomEventGenerator);
        AIMethods.sprFrecsL = SpriteInit.CreateSprite((SpriteType.sprFRECGROUP), 55, -14);
        AIMethods.sprFrecsC = SpriteInit.CreateSprite((SpriteType.sprFRECACTION), 299, -8);
        AIMethods.sprFrecsR = SpriteInit.CreateSprite((SpriteType.sprFRECGROUP), 547, -14);
        AIMethods.ssFrecs.Include(AIMethods.sprFrecsC); // Center first
        AIMethods.ssFrecs.Include(AIMethods.sprFrecsL);
        AIMethods.ssFrecs.Include(AIMethods.sprFrecsR);

        AIMethods.ssWater.Include(SpriteInit.CreateSprite((SpriteType.sprWATER), 0, 0));

        // Pit (Frosh and Pole)

        // Pole
        AIMethods.sprPole = SpriteInit.CreateSprite((SpriteType.sprPOLE), SCRW / 2, 80);
        AIMethods.ssPit.Include(AIMethods.sprPole);

        // Tam
        AIMethods.sprTam = SpriteInit.CreateSprite((SpriteType.sprTAM), SCRW / 2, 81);
        AIMethods.sprTam.nZ = 630;
        AIMethods.ssPit.Include(AIMethods.sprTam);


        // FROSH
        int i;
        TSprite tmpSprite;
        byte[] skinToneDefault = new byte[] {248, 208, 152};
        byte[] skinToneDarker = new byte[] { 134, 84, 39 };
        byte[] skinToneDarkest = new byte[] { 134, 84, 39 }; // rem darkest - didn't work w shader
//        byte[] skinToneDarkest = new byte[] { 80, 51, 25 };
        byte[] skinToneTan = new byte[] { 222, 156, 89 };
        byte[] skinTonePale = new byte[] { 255, 223, 191 };
        byte[] shirtToneDefault = new byte[] { 95, 0, 95 };
        byte[] shirtTone2 = new byte[] { 66, 10, 69 };
//        byte[] shirtTone3 = new byte[] { 163, 10, 171 };
        byte[] shirtTone3 = new byte[] { 136, 31, 142 };
        byte[] shirtTone4 = new byte[] { 109, 51, 112 };

        for (i = 0; i < AIMethods.gnNumFroshInPit; i++)
        {
            // Set up the Frosh personalities and include them in the special ssFr spriteset.
            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprFROSH), R.Next() % 40, AIMethods.dPITMINY + (R.Next() % (AIMethods.dPITMAXY - AIMethods.dPITMINY)));
            tmpSprite.nTag = i; // Add a tag to the Frosh.  They're numbered now.
            if (i < AIMethods.gnNumStartHeavyweight)
                tmpSprite.nAttrib[((int)nattrFrosh.attrPersonality)] = ((int)Personalities.persHeavyweight);
            else if (i < AIMethods.gnNumStartHeavyweight + AIMethods.gnNumStartClimber)
                tmpSprite.nAttrib[((int)nattrFrosh.attrPersonality)] = ((int)Personalities.persClimber);
            else if (i < AIMethods.gnNumStartHeavyweight + AIMethods.gnNumStartClimber + AIMethods.gnNumStartHoister)
                tmpSprite.nAttrib[((int)nattrFrosh.attrPersonality)] = ((int)Personalities.persHoister);
            else
                tmpSprite.nAttrib[((int)nattrFrosh.attrPersonality)] = ((int)Personalities.persGoofy);

            // And now for some ethnic diversity
            if (i < 7)
            {
//                tmpSprite.nAttrib[((int)nattrFrosh.attrEthnicity)] = ((int)skintones.skinBlack);
                tmpSprite.ReplaceRGB = skinToneDefault; tmpSprite.SubstituteRGB = skinToneDarkest;
            }
            else if (i < 14)
            {
//                tmpSprite.nAttrib[((int)nattrFrosh.attrEthnicity)] = ((int)skintones.skinBrown);
                tmpSprite.ReplaceRGB = skinToneDefault; tmpSprite.SubstituteRGB = skinToneDarker;
            }
            else if (i < 25)
            {
//                tmpSprite.nAttrib[((int)nattrFrosh.attrEthnicity)] = ((int)skintones.skinYellow);
                tmpSprite.ReplaceRGB = skinToneDefault; tmpSprite.SubstituteRGB = skinToneTan;
            }
            else if (i < 35)
            {
//                tmpSprite.nAttrib[((int)nattrFrosh.attrEthnicity)] = ((int)skintones.skinPale);
                tmpSprite.ReplaceRGB = skinToneDefault; tmpSprite.SubstituteRGB = skinTonePale;
            }
            else if (i < 50)
            {
//                tmpSprite.nAttrib[((int)nattrFrosh.attrEthnicity)] = ((int)skintones.skinWhite);
                tmpSprite.ReplaceRGB = shirtToneDefault; tmpSprite.SubstituteRGB = shirtTone2;
            }
            else if (i < 60)
            {
//                tmpSprite.nAttrib[((int)nattrFrosh.attrEthnicity)] = ((int)skintones.skinWhite);
                tmpSprite.ReplaceRGB = shirtToneDefault; tmpSprite.SubstituteRGB = shirtTone3;
            }
            else if (i < 70)
            {
//                tmpSprite.nAttrib[((int)nattrFrosh.attrEthnicity)] = ((int)skintones.skinWhite);
                tmpSprite.ReplaceRGB = shirtToneDefault; tmpSprite.SubstituteRGB = shirtTone4;
            }
            else // There are gnNumFroshInPit frosh - currently 85.
            {
//                tmpSprite.nAttrib[((int)nattrFrosh.attrEthnicity)] = ((int)skintones.skinWhite);
                // No replacement.
            }

            AIMethods.ssPit.Include(tmpSprite);
            AIMethods.ssFr.Include(tmpSprite);
        }

        // Icons and buttons
        int BARTOP = 108;
        int ICO1HT = 50;
        int ICO2HT = 36;
        int ICO3HT = 51;
        //int ICO4HT = 39;

        AIMethods.ssConsole.Include(SpriteInit.CreateSprite((SpriteType.sprGRID), 0, BARTOP));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprCONSOLE), 0, BARTOP));
        tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 0, BARTOP);
        tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpICOBAR)]);
        AIMethods.ssIcons.Include(tmpSprite);
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprAPPLEICON), 0, BARTOP));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPIZZAICON), 0, BARTOP + ICO1HT));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprCLARKICON), 0, BARTOP + ICO1HT + ICO2HT));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprEXAMICON), 0, BARTOP + ICO1HT + ICO2HT + ICO3HT));

        // Points and timer indicators
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprAPPLE_TENS), 30, BARTOP + 24));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprAPPLE_ONES), 50, BARTOP + 24));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPIZZA_TENS), 30, BARTOP + (24 * 3)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPIZZA_ONES), 50, BARTOP + (24 * 3)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprCLARK_TENS), 30, BARTOP + (24 * 5)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprCLARK_ONES), 50, BARTOP + (24 * 5)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprEXAM_TENS), 30, BARTOP + (24 * 7)));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprEXAM_ONES), 50, BARTOP + (24 * 7)));

        int dCURRENTSCOREHEIGHT = 25;

        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPITTIME_HTENS), 475, dCURRENTSCOREHEIGHT));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPITTIME_HONES), 495, dCURRENTSCOREHEIGHT));
        tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 515, dCURRENTSCOREHEIGHT);
        tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpALP_COLON)]);
        AIMethods.ssIcons.Include(tmpSprite);
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPITTIME_MTENS), 535, dCURRENTSCOREHEIGHT));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPITTIME_MONES), 555, dCURRENTSCOREHEIGHT));
        tmpSprite = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 575, dCURRENTSCOREHEIGHT);
        tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpALP_PERIOD)]);
        AIMethods.ssIcons.Include(tmpSprite);
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPITTIME_STENS), 595, dCURRENTSCOREHEIGHT));
        AIMethods.ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPITTIME_SONES), 615, dCURRENTSCOREHEIGHT));


        AIMethods.sprFPS0 = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 10, 290); AIMethods.ssIcons.Include(AIMethods.sprFPS0);
        AIMethods.sprFPS1 = SpriteInit.CreateSprite((SpriteType.sprINANIMATE), 30, 290); AIMethods.ssIcons.Include(AIMethods.sprFPS1);


        // Forge
        AIMethods.sprForge = SpriteInit.CreateSprite((SpriteType.sprFORGE), 0, 480); AIMethods.ssIcons.Include(AIMethods.sprForge);

        // Arm
        if (!bDemo)
        {
            AIMethods.sprArm = SpriteInit.CreateSprite((SpriteType.sprARM), 0, 0); AIMethods.ssIcons.Include(AIMethods.sprArm);
        }
        else
        {
            AIMethods.sprArm = SpriteInit.CreateSprite((SpriteType.sprARM), 0, 0);
            AIMethods.sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armDEMO);
            AIMethods.ssIcons.Include(AIMethods.sprArm);
        }


        // The Mouse Cursor
        AIMethods.ssMouse.Include(SpriteInit.CreateSprite((SpriteType.sprMOUSECURSORTL), 0, 0));
        AIMethods.ssMouse.Include(SpriteInit.CreateSprite((SpriteType.sprMOUSECURSORTR), 0, 0));
        AIMethods.ssMouse.Include(SpriteInit.CreateSprite((SpriteType.sprMOUSECURSORBL), 0, 0));
        AIMethods.ssMouse.Include(SpriteInit.CreateSprite((SpriteType.sprMOUSECURSORBR), 0, 0));

        // HIGH SCORE
        //int nTemp;
        int highScore = Globals.myGameConditions.GetHighScore(Globals.myGameConditions.GetFroshLameness());

        //if (0 != highScore)
        {
            int highScoreHours = (int)(highScore) / 1000 / 60;
            int highScoreMinutes = (int)(highScore) / 1000 % 60;
            int highScoreSeconds = (int)(highScore / 10) % 100;


            int STARTOFFSET = 40;
            // HOURS
            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprHIGHSCORE), 275 - STARTOFFSET, AIMethods.dHIGHSCORESTARTHEIGHT);
            if (highScoreHours < 10) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]); else tmpSprite.SetFrame(AIMethods.TensDigitFrame(highScoreHours));
            AIMethods.ssIcons.Include(tmpSprite);

            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprHIGHSCORE), 295 - STARTOFFSET, AIMethods.dHIGHSCORESTARTHEIGHT);
            if (highScoreHours <= 0) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]); else tmpSprite.SetFrame(AIMethods.OnesDigitFrame(highScoreHours));
            AIMethods.ssIcons.Include(tmpSprite);

            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprHIGHSCORE), 315 - STARTOFFSET, AIMethods.dHIGHSCORESTARTHEIGHT);
            tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpALP_COLON)]);
            AIMethods.ssIcons.Include(tmpSprite);

            // MINUTES
            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprHIGHSCORE), 335 - STARTOFFSET, AIMethods.dHIGHSCORESTARTHEIGHT);
            if (highScoreMinutes < 10) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]); else tmpSprite.SetFrame(AIMethods.TensDigitFrame(highScoreMinutes));
            AIMethods.ssIcons.Include(tmpSprite);

            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprHIGHSCORE), 355 - STARTOFFSET, AIMethods.dHIGHSCORESTARTHEIGHT);
            if (highScoreMinutes < 0) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]); else tmpSprite.SetFrame(AIMethods.OnesDigitFrame(highScoreMinutes));
            AIMethods.ssIcons.Include(tmpSprite);

            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprHIGHSCORE), 375 - STARTOFFSET, AIMethods.dHIGHSCORESTARTHEIGHT);
            tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpALP_PERIOD)]);
            AIMethods.ssIcons.Include(tmpSprite);

            // SECONDS
            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprHIGHSCORE), 395 - STARTOFFSET, AIMethods.dHIGHSCORESTARTHEIGHT);
            if (highScoreSeconds < 10) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]); else tmpSprite.SetFrame(AIMethods.TensDigitFrame(highScoreSeconds));
            AIMethods.ssIcons.Include(tmpSprite);

            tmpSprite = SpriteInit.CreateSprite((SpriteType.sprHIGHSCORE), 415 - STARTOFFSET, AIMethods.dHIGHSCORESTARTHEIGHT);
            if (highScoreSeconds <= 0) tmpSprite.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]); else tmpSprite.SetFrame(AIMethods.OnesDigitFrame(highScoreSeconds));
            AIMethods.ssIcons.Include(tmpSprite);
        }

        // Once in a blue moon you get over the tri-pub ban.
        if (0 == R.Next(3))
            Globals.myGameConditions.gbTriPubBan = false;

        return true;
    }













}

