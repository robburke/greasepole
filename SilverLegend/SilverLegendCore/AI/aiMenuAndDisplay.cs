using System;
public static partial class AIMethods
{
    /// MOUSE CURSOR STUFF

    public static int dMouseOffset = 20;
    public static int[] dMouseOffsetShifts = {0, 5, 8, 10, 11, 11, 11, 10, 8, 5, 
0, -5, -8, -10, -11, -11, -11, -10, -8, -5};
    public const int dNUMOFFSETSHIFTS = 20;
    public const int NUM_JBARSPOTS = 4;
    public static int[] nJSpotX = { 181, 184, 187, 190 };
    public static int[] nJSpotY = { 269, 304, 339, 373 };

    public const int dNUMOFFTOGGLEBUTTONS = 5;
    public static int[] nButtonSpotX = { 85, 400, 85, 400, 259 };
    public static int[] nButtonSpotY = { 105, 105, 280, 280, 404 };


    public static void aiMouseCursorMenu(TSprite s)
    {
        s.nX = Globals.InputService.GetMouseX();
        s.nY = Globals.InputService.GetMouseY();
        if (Globals.InputService.LeftButtonPressed() || Globals.InputService.LeftButtonDown())
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMOU_SELECT2)]);
        else if (Globals.InputService.RightButtonPressed())
            s.bAttrib[1] = !s.bAttrib[1];
        else
            s.SetFrame(frm[s.bAttrib[1] ? ((int)GameBitmapEnumeration.bmpMOU_SELECT1) : ((int)GameBitmapEnumeration.bmpMOU_SELECT3)]);

    }


    public const int nTITLESTARTGLOW = 3;

    public static void aiMenuStartButton(TSprite s)
    {
        if (!s.bAttrib[((int)battrMenuStartButtonAttributes.attrDoNotActivate)])
        {
            if (0 == ((s.nCC - (7 * 23)) % (40 * 23)) && 0 == (ssJacketSlam.GetNumberOfSprites()))
                lSound[((int)ASLList.lsndNARRATOR_STARTDELAY1)].Play(SoundbankInfo.volHOLLAR);
            if (0 == ((s.nCC - (15 * 23)) % (40 * 23)) && 0 == (ssJacketSlam.GetNumberOfSprites()))
                lSound[((int)ASLList.lsndNARRATOR_STARTDELAY2)].Play(SoundbankInfo.volHOLLAR);
        }

        if (Globals.InputService.StartButtonPressed())
        {
            Globals.myGameLoop.ChangeGameState(((int)GameStates.STATEDECORATE));
        }

        if (aisMouseOver(s))
        {
            switch (((s.nCC - s.nAttrib[6]) / 2) % 8)
            {
                case 0: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLESTARTGLOW1)]); break;
                case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLESTARTGLOW2)]); break;
                case 2: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLESTARTGLOW3)]); break;
                case 7: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLESTARTGLOW2)]); break;
            }

            if (Globals.InputService.LeftButtonPressed() && !s.bAttrib[((int)battrMenuStartButtonAttributes.attrDoNotActivate)])
            {
                sSound[((int)ASSList.ssndMENU_SELECT)].Play(SoundbankInfo.volFULL);
                Globals.myGameLoop.ChangeGameState(((int)GameStates.STATEDECORATE));
            }
        }
        else
        {
            s.nAttrib[6] = s.nCC;
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLESTART)]);
        }
        if (!(lSound[((int)ASLList.lsndMUSIC_TITLEINIT)].IsPlaying())
            && !(sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].IsPlaying())
            && !(lSound[((int)ASLList.lsndMUSIC_SCOTLAND)].IsPlaying())
            && !(s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)]))
        {
            s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)] = true;
            sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].Loop(SoundbankInfo.volMUSIC);
            sSound[((int)ASSList.ssndMENU_DECORATEREPEAT)].Stop();
        }
    }

    public const int nccBASEVALUE = 5680;
    public const int nccTRIGGERDEMO = 6300;

    public static int nOldMouseX = 0, nOldMouseY = 0;

    public static bool aiMouseHasMoved()
    {
        int nMouseX = Globals.InputService.GetMouseX();
        int nMouseY = Globals.InputService.GetMouseY();
        bool bAnswer = (nOldMouseX != nMouseX) || (nOldMouseY != nMouseY);
        nOldMouseX = nMouseX; nOldMouseY = nMouseY;
        return bAnswer;
    }


    public static void aiMenuExitButton(TSprite s)
    {
        if (Globals.InputService.BackButtonPressed())
        {
            sSound[((int)ASSList.ssndMENU_TOGGLE)].Play(SoundbankInfo.volFULL, panONX(s));
            Globals.myGameLoop.ChangeGameState(((int)GameStates.STATEEXIT));
        }
        if (s.nCC < nccBASEVALUE || s.nCC > nccTRIGGERDEMO)
            s.nCC = nccBASEVALUE;
        if (aiMouseHasMoved())
            s.nCC = nccBASEVALUE;
        if (aisMouseOver(s) || s.nCC == nccTRIGGERDEMO)
        {
            if (Globals.InputService.LeftButtonPressed() || s.nCC == nccTRIGGERDEMO)
            {
                //if (Globals.InputService.GetMouseX() < s.nX - 10 && 0 == (s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)]))
                //{
                //    sSound[((int)ASSList.ssndMENU_TOGGLE)].Play(SoundbankInfo.volFULL, panONX(s));
                //    Globals.myGameLoop.ChangeGameState(((int)GameStates.STATEEXIT));
                //}
                //else
                { // Cue the Credits
                    if (s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)] == 0 || s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)] == 7)
                        sSound[((int)ASSList.ssndEFFECTS_ICONOUT)].Play();
                    // If this is the last credit and it's being triggered activate the game in
                    // demo mode
                    if (s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)] == 7 && s.nCC == nccTRIGGERDEMO)
                        Globals.myGameLoop.ChangeGameState(((int)GameStates.STATEDEMO));
                    int j = 0;
                    while (j < ssIcons.GetNumberOfSprites())
                    {
                        if (ssIcons.GetSprite(j).SpriteType == (SpriteType.sprmnuTITLESTART)
                            || ssIcons.GetSprite(j).SpriteType == (SpriteType.sprmnuTITLEOPTIONS))
                            ssIcons.GetSprite(j).bAttrib[((int)battrMenuStartButtonAttributes.attrDoNotActivate)] = true;
                        if (ssIcons.GetSprite(j).SpriteType == (SpriteType.sprmnuTITLEBACK) && 0 == (s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)]))
                            aiInitFlyInAndOut2(ssIcons.GetSprite(j), aiInanimate, ssIcons.GetSprite(j).nX, ssIcons.GetSprite(j).nY, ssIcons.GetSprite(j).nX, ssIcons.GetSprite(j).nY - 40, 1, 1);
                        j++;
                    }
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    aiInitFlyInAndOut2(s, aiFlyBackInCredits, s.nX, s.nY, s.nX, s.nY + 300, 1, 1);
                }
                if (s.nCC == nccTRIGGERDEMO)
                    s.nCC = nccTRIGGERDEMO - 100;
                else
                    s.nCC = nccBASEVALUE;

            }
        }
        else
        {
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLEEXIT)]);
            s.nAttrib[3] = s.nCC;
        }

    }

    public static void aiFlyBackInCredits(TSprite s)
    {
        s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)]++;

        if (s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)] == 1 || s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)] == 8)
            sSound[((int)ASSList.ssndEFFECTS_ICONIN)].Play();
        if (s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)] > 7)
        {
            s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)] = 0;
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLEEXIT)]);
            s.nX = 109;
            aiInitFlyInAndOut2(s, aiMenuExitButton, 109, s.nY, 109, 31, 1, 1);
            int j = 0;
            while (j < ssIcons.GetNumberOfSprites())
            {
                if (ssIcons.GetSprite(j).SpriteType == (SpriteType.sprmnuTITLESTART)
                    || ssIcons.GetSprite(j).SpriteType == (SpriteType.sprmnuTITLEOPTIONS))
                    ssIcons.GetSprite(j).bAttrib[((int)battrMenuStartButtonAttributes.attrDoNotActivate)] = false;
                if (ssIcons.GetSprite(j).SpriteType == (SpriteType.sprmnuTITLEBACK))
                    aiInitFlyInAndOut2(ssIcons.GetSprite(j), aiInanimate, ssIcons.GetSprite(j).nX, ssIcons.GetSprite(j).nY, ssIcons.GetSprite(j).nX, ssIcons.GetSprite(j).nY + 40, 1, 1);
                j++;
            }
            for (int k = 0; k < ssIcons.GetNumberOfSprites(); k++)
            {
                TSprite optSprite = ssIcons.GetSprite(k);
                if (optSprite.SpriteType == (SpriteType.sprmnuTITLEOPTIONS))
                {
                    ssIcons.Remove(optSprite);
                    ssIcons.Include(optSprite);
                }
            }
            for (int k = 0; k < ssIcons.GetNumberOfSprites(); k++)
            {
                TSprite optSprite = ssIcons.GetSprite(k);
                if (optSprite.SpriteType == (SpriteType.sprmnuMOUSECURSOR))
                {
                    ssIcons.Remove(optSprite);
                    ssIcons.Include(optSprite);
                }
            }

        }
        else
        {
            for (int k = 0; k < ssIcons.GetNumberOfSprites(); k++)
            {
                TSprite optSprite = ssIcons.GetSprite(k);
                if (optSprite == s)
                {
                    ssIcons.Remove(optSprite);
                    ssIcons.Include(optSprite);
                    break;
                }
            }
            for (int k = 0; k < ssIcons.GetNumberOfSprites(); k++)
            {
                TSprite optSprite = ssIcons.GetSprite(k);
                if (optSprite.SpriteType == (SpriteType.sprmnuMOUSECURSOR))
                {
                    ssIcons.Remove(optSprite);
                    ssIcons.Include(optSprite);
                }
            }

            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLECREDITS1) - 1 + s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)]]);
            s.nX = 0;
            aiInitFlyInAndOut2(s, aiMenuExitButton, 0, s.nY, 0, 230, 3, 3);
        }
    }

    public static void aiMenuOptionsButton(TSprite s)
    {
        if (aisMouseOver(s))
        {
            switch (((s.nCC - s.nAttrib[3]) / 2) % 8)
            {
                case 0: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLEOPTIONSGLOW1)]); break;
                case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLEOPTIONSGLOW2)]); break;
                case 2: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLEOPTIONSGLOW3)]); break;
                case 7: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLEOPTIONSGLOW2)]); break;
            }

            if (Globals.InputService.LeftButtonPressed() && !s.bAttrib[((int)battrMenuStartButtonAttributes.attrDoNotActivate)])
            {
                sSound[((int)ASSList.ssndMENU_TOGGLE)].Play(SoundbankInfo.volFULL, panONX(s));
                Globals.myGameLoop.ChangeGameState(((int)GameStates.STATEOPTIONS));
            }
        }
        else
        {
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTITLEOPTIONS)]);
            s.nAttrib[3] = s.nCC;
        }
    }

    public const int nPASSCRESTGLOW = 8;

    public static void aisClickedOnPassCrest(TSprite s)
    {
        sSound[((int)ASSList.ssndMENU_SELECT)].Play(SoundbankInfo.volMUSIC, panONX(s));
        Globals.myGameLoop.ChangeGameState(((int)GameStates.STATEGAME));
    }

    public static void aiMenuGlowingPassCrest(TSprite s)
    {
        if (0 == ((s.nCC - (22 * 45)) % (23 * 45)))
            lSound[((int)ASLList.lsndNARRATOR_JACKETINIT)].Play(SoundbankInfo.volHOLLAR);

        if (Globals.InputService.StartButtonPressed())
        {
            aisClickedOnPassCrest(s);
        }

        if (aisMouseOver(s))
        {
            if (s.nCC > 4000) s.nCC = 0;
            switch (((s.nCC / 2) % nPASSCRESTGLOW))
            {
                case 0: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMENUPASSCREST1)]); break;
                case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMENUPASSCREST2)]); break;
                case 2: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMENUPASSCREST3)]); break;
                case 7: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMENUPASSCREST2)]); break;
            }

            if (Globals.InputService.LeftButtonPressed())
            {
                aisClickedOnPassCrest(s);
            }
        }
        else
        {
            s.nCC = 4000;
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMENUPASSCREST1)]);
        }
        if (//!(sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].IsPlaying()) &&
            !(lSound[((int)ASLList.lsndMUSIC_SCOTLAND)].IsPlaying()) &&
            !(sSound[((int)ASSList.ssndMENU_DECORATEREPEAT)].IsPlaying()) &&
            !(lSound[((int)ASLList.lsndMUSIC_TITLEINIT)].IsPlaying()) &&
            !(s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)]))
        {
            s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)] = true;
            sSound[((int)ASSList.ssndMENU_DECORATEREPEAT)].Loop(SoundbankInfo.volMUSIC);
            sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].Stop();
        }
    }

    // BUTTONS

    public static void aiInitToggleButton(TSprite s, int nButtonNumber)
    {
        s.nAttrib[((int)attrBar.attrOnScreenX)] = 320;
        s.nAttrib[((int)attrBar.attrOnScreenY)] = 480;

        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpBTNTOGGLE)]);
        s.nAttrib[((int)attrToggleButton.attrButtonNumber)] = nButtonNumber;

        switch (nButtonNumber)
        {
            case 0:
                s.nAttrib[((int)attrToggleButton.attrOnScreenPosition)] = Globals.myGameConditions.GetFroshLameness();
                break;
            case 1:
                s.nAttrib[((int)attrToggleButton.attrOnScreenPosition)] = Globals.myGameConditions.GetSound();
                break;
            case 2:
                s.nAttrib[((int)attrToggleButton.attrOnScreenPosition)] = Globals.myGameConditions.GetEnhancedGraphics();
                break;
            case 3:
                s.nAttrib[((int)attrToggleButton.attrOnScreenPosition)] = Globals.myGameConditions.GetRMBFunction();
                break;
        }
        s.bAttrib[((int)battrBar.battrBeingDragged)] = false;

        if (s.nAttrib[((int)attrToggleButton.attrOnScreenPosition)] == 1)  // Go right
            aiInitFlyInAndOut2(s, aiToggleButton, s.nX, s.nY, nButtonSpotX[s.nAttrib[((int)attrToggleButton.attrButtonNumber)]] + 150, nButtonSpotY[s.nAttrib[((int)attrToggleButton.attrButtonNumber)]], 1, 1);
        else
            aiInitFlyInAndOut2(s, aiToggleButton, s.nX, s.nY, nButtonSpotX[s.nAttrib[((int)attrToggleButton.attrButtonNumber)]], nButtonSpotY[s.nAttrib[((int)attrToggleButton.attrButtonNumber)]], 1, 1);
    }

    public static void aiToggleButton(TSprite s)
    {

        if (!(s.bAttrib[((int)battrBar.battrBeingDragged)]))
        {
            if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s))
            {
                s.nCC = 0;
                s.bAttrib[((int)battrBar.battrBeingDragged)] = true;
                sSound[((int)ASSList.ssndMENU_TOGGLE)].Play(SoundbankInfo.volFULL, panONX(s));
            }
        }
        else
        { // being dragged
            s.nX = Globals.InputService.GetMouseX();
            s.nY = Globals.InputService.GetMouseY();
            if (!Globals.InputService.LeftButtonDown())
            {
                int nValueSelected = (s.nX > nButtonSpotX[s.nAttrib[((int)attrToggleButton.attrButtonNumber)]] + 100) ? 1 : 0;
                sSound[((int)ASSList.ssndMENU_DROP)].Play(SoundbankInfo.volFULL, panONX(s));
                s.bAttrib[((int)battrBar.battrBeingDragged)] = false;
                if (0 != nValueSelected)  // Go right
                    aiInitFlyInAndOut2(s, aiToggleButton, s.nX, s.nY, nButtonSpotX[s.nAttrib[((int)attrToggleButton.attrButtonNumber)]] + 150, nButtonSpotY[s.nAttrib[((int)attrToggleButton.attrButtonNumber)]], 1, 1);
                else
                    aiInitFlyInAndOut2(s, aiToggleButton, s.nX, s.nY, nButtonSpotX[s.nAttrib[((int)attrToggleButton.attrButtonNumber)]], nButtonSpotY[s.nAttrib[((int)attrToggleButton.attrButtonNumber)]], 1, 1);

                switch (s.nAttrib[((int)attrToggleButton.attrButtonNumber)])
                {
                    case 0:
                        Globals.myGameConditions.SetFroshLameness(nValueSelected); break;
                    case 1:
                        Globals.myGameConditions.SetSound(nValueSelected); break;
                    case 2:
                        Globals.myGameConditions.SetEnhancedGraphics(nValueSelected); break;
                    case 3:
                        Globals.myGameConditions.SetRMBFunction(nValueSelected); break;
                    //case 4:
                    //    Globals.myGameConditions.SetColourDepth(nValueSelected);
                    //    if (nValueSelected == 1)
                    //        lSound[((int)ASLList.lsndNARRATOR_GRAPHICSWARN)].Play(SoundbankInfo.volHOLLAR);
                    //    break;
                }
            }
        }
    }




    //////// BARS




    public const int BAROFFJACKET = -1;

    public static void aiInitBar(TSprite s, int nBarNumber)
    {
        s.nAttrib[((int)attrBar.attrBarGroup)] = nBarNumber / 10;
        // No longer "cloak" the Ritual bar by default.
        //if (nBarNumber == 19)
        //    s.nAttrib[((int)attrBar.attrBarGroup)] = -42; // Cloak it.
        s.nAttrib[((int)attrBar.attrOnJacketPosition)] = BAROFFJACKET;
        s.nAttrib[((int)attrBar.attrOnScreenX)] = 553;
        s.nAttrib[((int)attrBar.attrOnScreenY)] = ((nBarNumber % 10) * 40) + 40;

        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpBAR1) + nBarNumber]); // This will change

        s.nAttrib[((int)attrBar.attrAssociatedDiscipline)] = nBarNumber;

        switch (nBarNumber)
        {
            case 0:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_APPLE); break;
            case 1:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_CHEM); break;
            case 2:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_CIVIL); break;
            case 3:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_ELEC); break;
            case 4:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_ENGPHYS); break;
            case 5:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_ENGCHEM); break;
            case 6:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_GEO); break;
            case 7:
            case 17:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_METALS); break;
            case 8:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_MECH); break;
            case 9:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_MINING); break;
            case 15:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_MECH); break;
            case 19:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_RITUAL); break;
            default:
                s.nAttrib[((int)attrBar.attrAssociatedSound)] = ((int)ASLList.lsndDISCIPLINES_DEFAULT); break;
        }
        s.bAttrib[((int)battrBar.battrBeingDragged)] = false;

        bool bAlreadyOnJacket = false;
        for (int i = 0; i < NUM_JBARSPOTS; i++)
        {
            if (Globals.myGameConditions.GetJBar(i) == nBarNumber)
            {
                bAlreadyOnJacket = true;
                s.nAttrib[((int)attrBar.attrOnJacketPosition)] = i;
                s.nX = 800;
                s.nY = nJSpotY[i];
                aiInitFlyInAndOut2(s, aiBar, s.nX, s.nY, nJSpotX[i], nJSpotY[i], 1, 1);

            }
        }
        if (!bAlreadyOnJacket)
        {
            s.nX = 840 + (nBarNumber / 10) * 20; s.nY = s.nAttrib[((int)attrBar.attrOnScreenY)];
        }

    }

    public const int NUM_BAR_GROUPS = 2;
    public const int NUM_ACHIEVEMENT_GROUPS = 3;

    public static void aiNextBarScreen(TSprite s)
    {
        if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s))
        {
            Globals.myGameConditions.SetBarGroup((Globals.myGameConditions.GetBarGroup() + 1) % NUM_BAR_GROUPS);
        }
    }

    public static void aiPrevBarScreen(TSprite s)
    {
        if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s))
        {
            Globals.myGameConditions.SetBarGroup((Globals.myGameConditions.GetBarGroup() - 1 + NUM_BAR_GROUPS) % NUM_BAR_GROUPS);
        }

        if (Globals.InputService.ToggleForwardButtonPressed())
        {
            Globals.myGameConditions.SetBarGroup(
                (Globals.myGameConditions.GetBarGroup() + 1) % NUM_BAR_GROUPS);
        }
        if (Globals.InputService.ToggleBackButtonPressed())
        {
            Globals.myGameConditions.SetBarGroup(
                (Globals.myGameConditions.GetBarGroup() - 1 + NUM_BAR_GROUPS) % NUM_BAR_GROUPS);
        }

    }

    public static void aiNextAchievementScreen(TSprite s)
    {
        if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s))
        {
            Globals.myGameConditions.SetAchievementGroup(
                (Globals.myGameConditions.GetAchievementGroup() + 1) % NUM_ACHIEVEMENT_GROUPS);
        }
        if (Globals.InputService.ToggleForwardButtonPressed())
        {
            Globals.myGameConditions.SetAchievementGroup(
                (Globals.myGameConditions.GetAchievementGroup() + 1) % NUM_ACHIEVEMENT_GROUPS);
        }
        if (Globals.InputService.ToggleBackButtonPressed())
        {
            Globals.myGameConditions.SetAchievementGroup(
                (Globals.myGameConditions.GetAchievementGroup() - 1 + NUM_ACHIEVEMENT_GROUPS) % NUM_ACHIEVEMENT_GROUPS);
        }
    }



    public static void aiDecorateReturn(TSprite s)
    {
        if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s))
        {
            Globals.myGameLoop.ChangeGameState(((int)GameStates.STATETITLE));
        }
        if (Globals.InputService.BackButtonDown())
        {
            Globals.myGameLoop.ChangeGameState(((int)GameStates.STATETITLE));
        }

    }

    public static void aiOptionsReturn(TSprite s)
    {
        if (!(lSound[((int)ASLList.lsndMUSIC_TITLEINIT)].IsPlaying()) && 
            !(sSound[((int)ASSList.ssndMENU_DECORATEREPEAT)].IsPlaying()) &&
//            !(sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].IsPlaying()) &&
            !(lSound[((int)ASLList.lsndMUSIC_SCOTLAND)].IsPlaying()) &&
            !(s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)]))
        {
            s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)] = true;
            sSound[((int)ASSList.ssndMENU_TITLEREPEAT)].Stop();
            sSound[((int)ASSList.ssndMENU_DECORATEREPEAT)].Loop(SoundbankInfo.volMUSIC);
        }

        if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s))
        {
            Globals.myGameLoop.ChangeGameState(((int)GameStates.STATETITLE));
        }
        if (Globals.InputService.BackButtonDown())
        {
            Globals.myGameLoop.ChangeGameState(((int)GameStates.STATETITLE));
        }
    }


    public static bool aisPickJacketBarSpot(TSprite s)
    {

        int[] nDistance = new int[NUM_JBARSPOTS];
        int i;
        int nSpotToUse = 0;
        for (i = 0; i < NUM_JBARSPOTS; i++)
        {
            nDistance[i] = (Math.Abs(s.nX - nJSpotX[i])) + (Math.Abs(s.nY - nJSpotY[i]));
        }

        for (i = 0; i < NUM_JBARSPOTS; i++)
        {
            if (Globals.myGameConditions.GetJBar(nSpotToUse) != NO_BAR)
                nSpotToUse = i;
            if (nDistance[i] < nDistance[nSpotToUse] && Globals.myGameConditions.GetJBar(i) == NO_BAR)
                nSpotToUse = i;
        }

        if (nDistance[nSpotToUse] > 100)
        {
            for (i = NUM_JBARSPOTS - 1; i >= 0; i--)
            {
                if (Globals.myGameConditions.GetJBar(i) == NO_BAR)
                    nSpotToUse = i;
            }
        }
        else
        {
            i = 1000;
        }


        if (Globals.myGameConditions.GetJBar(nSpotToUse) != NO_BAR)
            return false;

        s.nAttrib[((int)attrBar.attrOnJacketPosition)] = nSpotToUse;
        Globals.myGameConditions.SetJBar(nSpotToUse, s.nAttrib[((int)attrBar.attrAssociatedDiscipline)]);
        aiInitFlyInAndOut2(s, aiBar, s.nX, s.nY, nJSpotX[nSpotToUse], nJSpotY[nSpotToUse], 1, 1);

        return true;
    }


    public static void aiAchievementText(TSprite s)
    {
        bool bMyBarGroupOnScreen = s.nAttrib[((int)attrBar.attrBarGroup)] == Globals.myGameConditions.GetAchievementGroup();
        bool bXOnScreen = s.nX >= 0 && s.nX <= 640;

        if (!(s.bAttrib[((int)battrBar.battrBeingDragged)]))
        {
            if (!bMyBarGroupOnScreen && bXOnScreen)
                aiInitFlyInAndOut2(s, aiAchievementText, s.nX, s.nY, 700, s.nY, 1, 1); // Fly out
            if (bMyBarGroupOnScreen && !bXOnScreen)
                aiInitFlyInAndOut2(s, aiAchievementText, s.nX, s.nY, s.nAttrib[((int)attrBar.attrOnScreenX)], s.nAttrib[((int)attrBar.attrOnScreenY)], 1, 1); // Fly in
            if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s))
            {
                //s.nCC = 0;
                //s.bAttrib[((int)battrBar.battrBeingDragged)] = true;
                //if (s.nAttrib[((int)attrBar.attrOnJacketPosition)] != BAROFFJACKET)
                //{
                //    Globals.myGameConditions.SetJBar(s.nAttrib[((int)attrBar.attrOnJacketPosition)], NO_BAR);
                //    s.nAttrib[((int)attrBar.attrOnJacketPosition)] = BAROFFJACKET;
                //}
                //if (!lSound[s.nAttrib[((int)attrBar.attrAssociatedSound)]].IsPlaying())
                //    lSound[s.nAttrib[((int)attrBar.attrAssociatedSound)]].Play(SoundbankInfo.volHOLLAR);
            }
        }
    }


    public static void aiBar(TSprite s)
    {
        bool bMyBarGroupOnScreen = s.nAttrib[((int)attrBar.attrBarGroup)] == Globals.myGameConditions.GetBarGroup();
        bool bOnJacket = !(s.nAttrib[((int)attrBar.attrOnJacketPosition)] == BAROFFJACKET);
        bool bXOnScreen = s.nX >= 0 && s.nX <= 640;

        if (bOnJacket) AIMethods.aisUnlockAchievement(2002);

        if (!(s.bAttrib[((int)battrBar.battrBeingDragged)]))
        {
            if (!bMyBarGroupOnScreen && !bOnJacket && bXOnScreen)
                aiInitFlyInAndOut2(s, aiBar, s.nX, s.nY, 700, s.nY, 1, 1); // Fly out
            if (bMyBarGroupOnScreen && !bOnJacket && !bXOnScreen)
                aiInitFlyInAndOut2(s, aiBar, s.nX, s.nY, s.nAttrib[((int)attrBar.attrOnScreenX)], s.nAttrib[((int)attrBar.attrOnScreenY)], 1, 1); // Fly in
            if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s))
            {
                s.nCC = 0;
                s.bAttrib[((int)battrBar.battrBeingDragged)] = true;
                if (s.nAttrib[((int)attrBar.attrOnJacketPosition)] != BAROFFJACKET)
                {
                    Globals.myGameConditions.SetJBar(s.nAttrib[((int)attrBar.attrOnJacketPosition)], NO_BAR);
                    s.nAttrib[((int)attrBar.attrOnJacketPosition)] = BAROFFJACKET;
                }
                if (!lSound[s.nAttrib[((int)attrBar.attrAssociatedSound)]].IsPlaying())
                    lSound[s.nAttrib[((int)attrBar.attrAssociatedSound)]].Play(SoundbankInfo.volHOLLAR);
            }
        }
        else
        { // being dragged
            s.nX = Globals.InputService.GetMouseX();
            s.nY = Globals.InputService.GetMouseY();
            if (!Globals.InputService.LeftButtonDown())
            {
                s.bAttrib[((int)battrBar.battrBeingDragged)] = false;
                if (s.nCC < 7 //&& s.nAttrib[((int)attrBar.attrOnJacketPosition)] != BAROFFJACKET 
                    || s.nCC >= 7 && Math.Abs(s.nX - s.nAttrib[((int)attrBar.attrOnScreenX)]) > 160)
                {
                    if (!aisPickJacketBarSpot(s))
                    {
                        if (bMyBarGroupOnScreen)
                            aiInitFlyInAndOut2(s, aiBar, s.nX, s.nY, s.nAttrib[((int)attrBar.attrOnScreenX)], s.nAttrib[((int)attrBar.attrOnScreenY)], 1, 1); // Fly in
                        else
                            aiInitFlyInAndOut2(s, aiBar, s.nX, s.nY, 700, s.nY, 1, 1); // Fly out
                        //			aiInitFlyInAndOut2(s, aiBar, s.nX, s.nY, (s.nX < 320) ? -150 : 700, s.nY, 1, 1); // Fly out
                    }
                }
                else
                {
                    if (bMyBarGroupOnScreen)
                        aiInitFlyInAndOut2(s, aiBar, s.nX, s.nY, s.nAttrib[((int)attrBar.attrOnScreenX)], s.nAttrib[((int)attrBar.attrOnScreenY)], 1, 1); // Fly in
                    else
                        aiInitFlyInAndOut2(s, aiBar, s.nX, s.nY, 700, s.nY, 1, 1); // Fly out
                    //			aiInitFlyInAndOut2(s, aiBar, s.nX, s.nY, (s.nX < 320) ? -150 : 700, s.nY, 1, 1); // Fly out
                }
            }
        }
    }

    public static void aiMouseCursorTL(TSprite s)
    {
        //  Moves a site to the position of the mouse cursor
        s.nX = Globals.InputService.GetMouseX() - dMouseOffset - dMouseOffsetShifts[(s.nCC % dNUMOFFSETSHIFTS)];
        s.nY = Globals.InputService.GetMouseY() - dMouseOffset - dMouseOffsetShifts[(s.nCC % dNUMOFFSETSHIFTS)];
        if (Globals.myGameConditions.IsDemo())
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
        else
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMOU_TARTL)]);
    }
    public static void aiMouseCursorTR(TSprite s)
    {
        //  Moves a site to the position of the mouse cursor
        s.nX = Globals.InputService.GetMouseX() + dMouseOffset + dMouseOffsetShifts[(s.nCC % dNUMOFFSETSHIFTS)];
        s.nY = Globals.InputService.GetMouseY() - dMouseOffset - dMouseOffsetShifts[(s.nCC % dNUMOFFSETSHIFTS)];
        if (Globals.myGameConditions.IsDemo())
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
        else
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMOU_TARTR)]);
    }
    public static void aiMouseCursorBL(TSprite s)
    {
        //  Moves a site to the position of the mouse cursor
        s.nX = Globals.InputService.GetMouseX() - dMouseOffset - dMouseOffsetShifts[(s.nCC % dNUMOFFSETSHIFTS)];
        s.nY = Globals.InputService.GetMouseY() + dMouseOffset + dMouseOffsetShifts[(s.nCC % dNUMOFFSETSHIFTS)];
        if (Globals.myGameConditions.IsDemo())
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
        else
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMOU_TARBL)]);
    }
    public static void aiMouseCursorBR(TSprite s)
    {
        //  Moves a site to the position of the mouse cursor
        if (Globals.myGameConditions.IsDemo())
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
        else
        {
            s.nX = Globals.InputService.GetMouseX() + dMouseOffset + dMouseOffsetShifts[(s.nCC % dNUMOFFSETSHIFTS)];
            s.nY = Globals.InputService.GetMouseY() + dMouseOffset + dMouseOffsetShifts[(s.nCC % dNUMOFFSETSHIFTS)];
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpMOU_TARBR)]);
        }
    }


    public static FrameDesc TensDigitFrame(int number)
    {
        if (number < 0)
            return frm[((int)GameBitmapEnumeration.bmpDIG_0)];
        if (number >= 100)
            number = 99;
        return frm[((int)GameBitmapEnumeration.bmpDIG_0) + (number / 10)];
    }

    public static FrameDesc OnesDigitFrame(int number)
    {
        if (number < 0)
            return frm[((int)GameBitmapEnumeration.bmpDIG_0)];
        if (number >= 100)
            number = 99;
        return frm[((int)GameBitmapEnumeration.bmpDIG_0) + (number % 10)];
    }


    public static int aisGetPointsBasedOn(int nAttrButtonType)
    {
        int nTemp = 0;
        switch (nAttrButtonType)
        {
            case ((int)Buttons.buttonAPPLE):
                nTemp = Globals.myGameConditions.GetPlayerApples();
                break;
            case ((int)Buttons.buttonPIZZA):
                nTemp = Globals.myGameConditions.GetPlayerPizza();
                break;
            case ((int)Buttons.buttonCLARK):
                nTemp = Globals.myGameConditions.GetPlayerClark();
                break;
            case ((int)Buttons.buttonEXAM):
                nTemp = Globals.myGameConditions.GetPlayerExam();
                break;
        }
        return nTemp;
    }


    public static void aiPointsTens(TSprite s)
    {
        int nTemp;
        nTemp = aisGetPointsBasedOn(s.nAttrib[((int)attrIcon.attrButtonType)]);
        if (nTemp < 10)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
        else
            s.SetFrame(TensDigitFrame(nTemp));
    }

    public static void aiPointsOnes(TSprite s)
    {
        int nTemp;
        nTemp = aisGetPointsBasedOn(s.nAttrib[((int)attrIcon.attrButtonType)]);
        if (nTemp <= 0)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
        else
            s.SetFrame(OnesDigitFrame(nTemp));
    }

    public static void aiPitTimeSTens(TSprite s)
    {
        if (gbShowFPS)
        {
            sprFPS0.SetFrame(TensDigitFrame(Globals.RenderingService.GetFps()));
            sprFPS1.SetFrame(OnesDigitFrame(Globals.RenderingService.GetFps()));
        }
        else
        {
            sprFPS0.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
            sprFPS1.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
        }
        s.SetFrame(TensDigitFrame(gnPitTimeS));
    }

    public static void aiPitTimeSOnes(TSprite s)
    {
        s.SetFrame(OnesDigitFrame(gnPitTimeS));
    }

    public static void aiPitTimeMTens(TSprite s)
    {
        s.SetFrame(TensDigitFrame(gnPitTimeM));
    }

    public static void aiPitTimeMOnes(TSprite s)
    {
        s.SetFrame(OnesDigitFrame(gnPitTimeM));
    }

    //#define PERFORMANCEBOOSTTIME 
    public const int BOOSTTIMEKEEN = 330;
    public const int BOOSTTIMENORMAL = 660;
    public static void aiPitTimeHTens(TSprite s)
    {

        // BOOST THE PERFORMANCE OF THE FROSH EVERY 1320 Frames (1 minute)
        if (0 == (s.nCC % (0 != Globals.myGameConditions.GetFroshLameness() ? BOOSTTIMEKEEN : BOOSTTIMENORMAL)))
            Globals.myGameConditions.PerformanceBoost();

        s.SetFrame(TensDigitFrame(gnPitTimeH));
        if (gnPitTimeH >= 10) AIMethods.aisUnlockAchievement(5);
        if (gnPitTimeH >= 20) AIMethods.aisUnlockAchievement(5);
        if (gnPitTimeH >= 5 && !HasTossed114Exam) AIMethods.aisUnlockAchievement(13);
    }

    internal static bool HasTossed114Exam = false;

    public static void aiPitTimeHOnes(TSprite s)
    {
        s.SetFrame(OnesDigitFrame(gnPitTimeH));
    }

    //*******************************************************
    // Button Intelligence (ordered alphabetically)
    //*******************************************************
    public static void aiIconIntoPosition(TSprite s)
    {

        switch (s.nAttrib[((int)attrIcon.attrButtonType)])
        {
            case ((int)Buttons.buttonAPPLE):
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOAPPLE1)]); break;
            case ((int)Buttons.buttonPIZZA):
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOPIZZA1)]); break;
            case ((int)Buttons.buttonCLARK):
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOCLARK1)]); break;
            case ((int)Buttons.buttonEXAM):
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOEXAM1)]); break;
        }

        if (s.nX < s.nDestX)
        {
            s.nX += s.nvX;
            s.nvX += 1;
        }
        else
        {
            s.nX = s.nDestX;  // "SLAM!"
            s.nvX = 5;
            s.nAttrib[((int)attrIcon.attrIconStatus)] = 1;
        }
    }

    public static void aiIconOutOfPosition(TSprite s)
    {
        switch (s.nAttrib[((int)attrIcon.attrButtonType)])
        {
            case ((int)Buttons.buttonAPPLE):
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOAPPLE1)]); break;
            case ((int)Buttons.buttonPIZZA):
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOPIZZA1)]); break;
            case ((int)Buttons.buttonCLARK):
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOCLARK1)]); break;
            case ((int)Buttons.buttonEXAM):
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOEXAM1)]); break;
        }
        if (s.nX > -90)
        {
            s.nX -= s.nvX;
            s.nvX += 1;
        }
        if (0 != aisGetPointsBasedOn(s.nAttrib[((int)attrIcon.attrButtonType)]))
        {
            s.nAttrib[((int)attrIcon.attrIconStatus)] = 0;
            sSound[((int)ASSList.ssndEFFECTS_ICONIN)].Play(SoundbankInfo.volFULL, panONX(s));
            s.nvX = 5;
        }
    }

    public static void aiDeleteMe(TSprite s)
    {
        s.bDeleted = true;
    }

    public static void aiRing(TSprite s)
    {
        if (aisMouseOver(s))
        {
            s.nCC += 2;
            if (Globals.InputService.LeftButtonPressed())
            {
                lSound[((int)ASLList.lsndRING_RISE)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armIRONRING); sprArm.nCC = 0;
                aiInitFlyInAndOut2(s, aiDeleteMe, s.nX, s.nY, sprArm.nX, 540, 1, 1);
                Globals.myGameConditions.ReleaseRingSpot(s.nAttrib[0]);
                Globals.myGameConditions.LoseRing();
            }
        }
        switch ((s.nCC / 3) % 5)
        {
            case 0: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICORING1)]); break;
            case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICORING2)]); break;
            case 2: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICORING3)]); break;
            case 3: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICORING4)]); break;
            case 4: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICORING5)]); break;
        }
    }


    public static void aiIcon(TSprite s)
    {
        bool bMouseOver = aisMouseOver(s);
        switch (s.nAttrib[((int)attrIcon.attrIconStatus)])
        {
            case 0:
                if (0 == aisGetPointsBasedOn(s.nAttrib[((int)attrIcon.attrButtonType)]))
                {
                    sSound[((int)ASSList.ssndEFFECTS_ICONOUT)].Play(SoundbankInfo.volFULL, panONX(s));
                    s.nAttrib[((int)attrIcon.attrIconStatus)] = 2;
                }
                aiIconIntoPosition(s);
                break;
            case 1:
                if (!bMouseOver)
                {
                    switch (s.nAttrib[((int)attrIcon.attrButtonType)])
                    {
                        case ((int)Buttons.buttonAPPLE):
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOAPPLE2)]); break;
                        case ((int)Buttons.buttonPIZZA):
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOPIZZA2)]); break;
                        case ((int)Buttons.buttonCLARK):
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOCLARK2)]); break;
                        case ((int)Buttons.buttonEXAM):
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOEXAM2)]); break;
                    }
                }
                switch (s.nAttrib[((int)attrIcon.attrButtonType)])
                {
                    case ((int)Buttons.buttonAPPLE):
                        if (bMouseOver || sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armAPPLE)
                            || (sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armAPPLE) && sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING))
                            || (sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armOTHROW)))
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOAPPLE3)]); break;
                    case ((int)Buttons.buttonPIZZA):
                        if (bMouseOver || sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armPIZZA)
                            || (sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armPIZZA) && sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING))
                            || (sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armSTHROW)))
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOPIZZA3)]); break;
                    case ((int)Buttons.buttonCLARK):
                        if (bMouseOver || sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCLARK)
                            || (sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armCLARK) && sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING))
                            || (sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armSTHROW2)))
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOCLARK3)]); break;
                    case ((int)Buttons.buttonEXAM):
                        if (bMouseOver || sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armEXAM)
                            || (sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armEXAM) && sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING))
                            || (sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armSTHROW3)))
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpICOEXAM3)]); break;
                }

                if (0 == aisGetPointsBasedOn(s.nAttrib[((int)attrIcon.attrButtonType)]))
                {
                    sSound[((int)ASSList.ssndEFFECTS_ICONOUT)].Play(SoundbankInfo.volFULL, panONX(s));
                    s.nAttrib[((int)attrIcon.attrIconStatus)] = 2;
                }

                if (bMouseOver && Globals.InputService.LeftButtonPressed())
                {
                    if (!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING))
                        && !(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armIRONRING)))
                    {
                        sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                        sprArm.nCC = 0;
                        switch (s.nAttrib[((int)attrIcon.attrButtonType)])
                        {
                            case ((int)Buttons.buttonTAUNT):
                                // LAAAAME FROSH!
                                sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armAPPLE); // armTAUNTING
                                break;
                            case ((int)Buttons.buttonAPPLE):
                                sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armAPPLE);
                                break;
                            case ((int)Buttons.buttonPIZZA):
                                sSound[((int)ASSList.ssndEFFECTS_PIZZAREADY)].Play(SoundbankInfo.volNORMAL, panONX(s));
                                sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armPIZZA);
                                break;
                            case ((int)Buttons.buttonCLARK):
                                sSound[((int)ASSList.ssndEFFECTS_POUR)].Play(SoundbankInfo.volNORMAL, panONX(s));
                                sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armCLARK);
                                break;
                            case ((int)Buttons.buttonEXAM):
                                sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armEXAM);
                                break;
                            case ((int)Buttons.buttonGREASE):
                                sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armGREASE);
                                break;
                            case ((int)Buttons.buttonRING):
                                sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armIRONRING);
                                break;
                        }

                    }
                }
                break;
            case 2:
                aiIconOutOfPosition(s);
                break;
        }
    }

    public static void aiTrackerLR(TSprite s)
    {
        if (!(ssFr.GetSprite(0).frmFrame == null))
        {
            s.nY = ssFr.GetBottomMostScrPointOnSprite(0);
            s.nX = ssFr.GetRightMostScrPointOnSprite(0);
        }
    }

    public static void aiTrackerUL(TSprite s)
    {
        if (!(ssFr.GetSprite(0).frmFrame == null))
        {
            s.nY = ssFr.GetTopMostScrPointOnSprite(0);
            s.nX = ssFr.GetLeftMostScrPointOnSprite(0);
        }
    }

    public static void aiSlamJacket(TSprite s)
    {
        switch (s.nCC)
        {
            case 1:
                sSound[((int)ASSList.ssndEFFECTS_BIGJACKETWHOOSH)].Play(SoundbankInfo.volNORMAL);
                s.nX = 320; s.nY = 480 + 150;
                break;
            case 3:
                s.nX = 320; s.nY = 480 + 100;
                break;
            case 4:
                s.nX = 320; s.nY = 480 + 50;
                break;
            case 5:
                s.nX = 320; s.nY = 480;
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSLAMJACKET2)]);
                break;
            case 7:
                sSound[((int)ASSList.ssndEFFECTS_BIGJACKETSLAM)].Play(SoundbankInfo.volFULL);
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSLAMJACKET3)]);
                break;
            case 16:
                if (s.nAttrib[((int)attrJacketSlam.attrNextState)] == ((int)GameStates.STATEEXIT))
                    Globals.myGameLoop.TerminateApplication();
                else
                    Globals.myGameLoop.SetGameState(s.nAttrib[((int)attrJacketSlam.attrNextState)]);
                break;
            case 20:
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSLAMJACKET4)]);
                sSound[((int)ASSList.ssndEFFECTS_BIGJACKETWHOOSH)].Play(SoundbankInfo.volFULL);
                break;
            case 24:
                s.bDeleted = true;
                break;

        }
    }

    public const int AIFLYINANDOUTINCREMENT_NUMERATOR = 2;
    public const int AIFLYINANDOUTINCREMENT_DEMONINATOR = 3;
    public const int AIFLYINANDOUTSQUEEZEDISTANCE = 5;
    public static void aiFlyInAndOut(TSprite s)
    {

        aisMoveTowardsDestination(s);

        if (Math.Abs(s.nX - s.nDestX) <= AIFLYINANDOUTSQUEEZEDISTANCE)
        {
            s.nX = s.nDestX; s.nvX = 0;
        }
        else
        {
            s.nvX = s.nvX *
                AIFLYINANDOUTINCREMENT_NUMERATOR / AIFLYINANDOUTINCREMENT_DEMONINATOR;
            if (Math.Abs(s.nvX) < AIFLYINANDOUTSQUEEZEDISTANCE)
                if (s.nvX > 0)
                    s.nvX = AIFLYINANDOUTSQUEEZEDISTANCE;
                else
                    s.nvX = -AIFLYINANDOUTSQUEEZEDISTANCE;
        }

        if (Math.Abs(s.nY - s.nDestY) <= AIFLYINANDOUTSQUEEZEDISTANCE)
        {
            s.nY = s.nDestY; s.nvY = 0;
        }
        else
        {
            s.nvY = s.nvY *
                AIFLYINANDOUTINCREMENT_NUMERATOR / AIFLYINANDOUTINCREMENT_DEMONINATOR;
            if (Math.Abs(s.nvY) < AIFLYINANDOUTSQUEEZEDISTANCE)
                if (s.nvY > 0)
                    s.nvY = AIFLYINANDOUTSQUEEZEDISTANCE;
                else
                    s.nvY = -AIFLYINANDOUTSQUEEZEDISTANCE;
        }
        if (s.nX == s.nDestX && s.nY == s.nDestY)
            s.SwitchToSecondaryBehavior();
    }

    public static void aiInitFlyInAndOut(TSprite s, AIMethod fSecondaryBehavior,
                           int nStartX, int nStartY,
                           int nGoingToX, int nGoingToY,
                           int alphaX, int alphaY)
    {
        int i, n, dX, dY;
        float increment, t1X = 0, t1Y = 0;
        n = 23;
        increment = AIFLYINANDOUTINCREMENT_NUMERATOR;
        increment /= AIFLYINANDOUTINCREMENT_DEMONINATOR;
        // X Part
        dX = Math.Abs(nStartX - nGoingToX);
        if (0 != dX)
        {
            t1X = (float)alphaX;
            for (i = 0; i < (n - 1); i++)
                t1X *= (1 / increment);
            if (nStartX < nGoingToX)
                dX = -(int)((float)t1X / (float)(1 - increment)); // approx
            else
                dX = (int)((float)t1X / (float)(1 - increment)); // approx
        }
        // Y Part
        dY = Math.Abs(nStartY - nGoingToY);
        if (0 != dY)
        {
            t1Y = (float)alphaY;
            for (i = 0; i < (n - 1); i++)
                t1Y *= (1 / increment);
            if (nStartY < nGoingToY)
                dY = -(int)(t1Y / (1 - increment)); // approx
            else
                dY = (int)(t1Y / (1 - increment)); // approx
        }


        s.nX = nGoingToX + dX; s.nY = nGoingToY + dY;
        s.nvX = (int)t1X; s.nvY = (int)t1Y;
        s.nDestX = nGoingToX; s.nDestY = nGoingToY;

        // Set up the secondary behavior
        s.SetSecondaryBehavior(fSecondaryBehavior);
        s.SetBehavior(aiFlyInAndOut);

    }

    public static void aiFlyInAndOut2(TSprite s)
    {
        aisMoveTowardsDestination(s);

        if (Math.Abs(s.nX - s.nDestX) <= AIFLYINANDOUTSQUEEZEDISTANCE)
        {
            s.nX = s.nDestX; s.nvX = 0;
        }
        else
        {
            s.nvX = s.nvX *
                (AIFLYINANDOUTINCREMENT_DEMONINATOR - 1) / AIFLYINANDOUTINCREMENT_DEMONINATOR;
            if (Math.Abs(s.nvX) < AIFLYINANDOUTSQUEEZEDISTANCE)
                s.nvX = AIFLYINANDOUTSQUEEZEDISTANCE;
        }

        if (Math.Abs(s.nY - s.nDestY) <= AIFLYINANDOUTSQUEEZEDISTANCE)
        {
            s.nY = s.nDestY; s.nvY = 0;
        }
        else
        {
            s.nvY = s.nvY *
                (AIFLYINANDOUTINCREMENT_DEMONINATOR - 1) / AIFLYINANDOUTINCREMENT_DEMONINATOR;
            if (Math.Abs(s.nvY) < AIFLYINANDOUTSQUEEZEDISTANCE)
                s.nvY = AIFLYINANDOUTSQUEEZEDISTANCE;
        }
        if (s.nX == s.nDestX && s.nY == s.nDestY)
            s.SwitchToSecondaryBehavior();
    }

    public static void aiInitFlyInAndOut2(TSprite s, AIMethod fSecondaryBehavior,
                           int nStartX, int nStartY,
                           int nGoingToX, int nGoingToY,
                           int alphaX, int alphaY)
    {
        //int n = 23;
        float increment;
        increment = AIFLYINANDOUTINCREMENT_NUMERATOR;
        increment /= AIFLYINANDOUTINCREMENT_DEMONINATOR;

        //s.nX = GoingToX + dX; s.nY = nGoingToY + dY;
        s.nvX = Math.Abs(nGoingToX - nStartX) / AIFLYINANDOUTINCREMENT_DEMONINATOR;
        s.nvY = Math.Abs(nGoingToY - nStartY) / AIFLYINANDOUTINCREMENT_DEMONINATOR;
        s.nDestX = nGoingToX; s.nDestY = nGoingToY;

        // Set up the secondary behavior
        s.SetSecondaryBehavior(fSecondaryBehavior);
        s.SetBehavior(aiFlyInAndOut2);

    }

    public static void aiConsole(TSprite s)
    {
        //&& 
    }

    public static void aiGrid(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGRID1) + ((s.nCC / 7) % 3)]);
    }

    public static void aiRingMeter(TSprite s)
    {
        if (s.bDeleted != true)
        {
            if (s.nY < 16)
                s.nY = 16;
            if (s.nY > 92)
                s.bAttrib[1] = true;

            if (!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armIRONRING)))
                s.bAttrib[1] = true;
            else if (0 == (s.nCC % 7) && sprArm.nAttrib[((int)attrArm.attrArmAction)] == 1)
                s.nY++;
            if (s.bAttrib[1] == true)
            {
                if (0 == (s.nCC % 14))
                {
                    s.nY++;
                }
                if (s.nY > 92)
                {
                    s.bDeleted = true;
                    if (sprRingMeter == s) sprRingMeter = null;
                }
            }
        }
    }


    public static void aiWaterMeter(TSprite s)
    {
        if (s.bDeleted != true)
        {
            if (s.nY < 16)
                s.nY = 16;
            if (s.nCC == 25)
                lSound[((int)ASLList.lsndHOSE_TAKE1) + R.Next(2)].Play();
            if (s.nY > 92)
                s.bAttrib[1] = true;

            if (!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armGREASE)))
                s.bAttrib[1] = true;
            else if (0 == (s.nCC % 7) && sprArm.nAttrib[((int)attrArm.attrArmAction)] == 1)
                s.nY++;
            if (s.bAttrib[1] == true)
            {
                if (0 == (s.nCC % 14))
                {
                    s.nY++;
                }
                if (s.nY > 92)
                {
                    if (sprWaterMeter == s)
                    {
                        sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                        sprArm.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                    }
                    sprArm.nCC = 0;
                    sSound[((int)ASSList.ssndWATER_HOSE)].Stop();
                    s.bDeleted = true;
                    if (sprWaterMeter == s) sprWaterMeter = null;
                }
            }
        }
    }


    public static void aiPowerMeter(TSprite s)
    {
        if (s.nY < 14)
            s.nY = 14;

        // If the arm unexpectedly changes to another item don't leave the power bar there.
        if (sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armAPPLE))
        {
            s.bDeleted = true;
            if (sprPowerMeter == s) sprPowerMeter = null;
        }

        // If the power meter is on the decline...
        if (s.bAttrib[1] == true)
        {
            s.nY += 4;
            if (s.nY > 95)
            {
                s.bDeleted = true;
                if (sprPowerMeter == s) sprPowerMeter = null;
            }
        }
        else // The power meter is on the rise
            s.nY -= 4;
    }

    public static void aiHighScore(TSprite s)
    {
        if (s.nCC < 100)
        {
            if (0 != s.nCC / 6 % 2)
                s.nY = -40;
            else
                s.nY = dHIGHSCORESTARTHEIGHT;
        }
        else
        {
            s.nY = dHIGHSCORESTARTHEIGHT;
            aiInitFlyInAndOut2(s, aiDeleteMe, s.nX, s.nY, s.nX, -40, 1, 1);
        }
    }

    public static void aiAchievementUnlockedNotice(TSprite s)
    {
        if (s.nCC == 30)
        {
            if (s.SpriteText == SpriteTextType.None)
            {
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpACHIEVEMENTUNLOCKED2)]);
                sSound[((int)ASSList.ssndEFFECTS_ACHIEVEMENTUNLOCKED2)].Play(SoundbankInfo.volFULL, panONX(s));
            }
            else
            {
                s.nA = 255;
            }
        }
        if (s.nCC < 140)
        {
            //if (0 != s.nCC / 6 % 2)
            //    s.nY = -40;
            //else
            //    s.nY = dHIGHSCORESTARTHEIGHT;
        }
        else
        {
            //s.nY = dHIGHSCORESTARTHEIGHT;
            aiInitFlyInAndOut2(s, aiDeleteMe, s.nX, s.nY, s.nX, -40, 1, 1);
        }
    }




    public static void aiPopBoy(TSprite s)
    {
        switch (s.nAttrib[((int)nattrFrosh.attrBehavior)])
        {
            case 1: // Leaping into pit
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY5_1)]);
                aisPlummet(s);
                if (s.nZ <= 0)
                {
                    ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), s.nX, s.nY));
                    NOSPEECHFOR(200);
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY2_1)]);
                    lSound[((int)ASLList.lsndPOPBOY_GREETING1) + R.Next(2)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                    s.nAttrib[((int)nattrFrosh.attrBehavior)] = 2; s.nCC = 0; s.nZ = 0;
                }
                break;
            case 2: // Beckoning (Greeting)
                switch (s.nCC % 40)
                {
                    case 1:
                        //			aisThinkForAl(s);
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY2_1)]); break;
                    case 4:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY2_2)]); break;
                    case 11:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY2_1)]); break;
                    case 14:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY2_2)]); break;
                    case 21:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY2_4)]); break;
                    case 24:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY2_3)]); break;
                    case 31:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY2_4)]); break;
                    case 34:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY2_3)]); break;
                }
                if (!lSound[((int)ASLList.lsndPOPBOY_GREETING1)].IsPlaying() &&
                    !lSound[((int)ASLList.lsndPOPBOY_GREETING2)].IsPlaying())
                {
                    s.nAttrib[((int)nattrFrosh.attrBehavior)] = 5; s.nCC = 0;
                    s.nAttrib[((int)nattrFrosh.attrMotivation)] = R.Next(3);
                }
                break;
            case 3: // Walking to his spot
                switch (s.nCC % 40)
                {
                    case 1:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY7_2)]); break;
                    case 8:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY7_1)]); break;
                    case 21:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY7_3)]); break;
                    case 28:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY7_4)]); break;
                }
                if (s.nX < dPOLEX) s.nX += 2; else if (s.nX > dPOLEX) s.nX -= 2;
                if (s.nY < dPOLEY + 1) s.nY++; else if (s.nY > dPOLEY + 1) s.nY--;

                if (s.nX == dPOLEX && s.nY == dPOLEY + 1)
                {
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY6_1)]);
                    s.nAttrib[((int)nattrFrosh.attrBehavior)] = 4; s.nCC = 0;
                }
                break;
            case 4: // Base of Pole
                if ((0 == R.Next(70)) && s.nCC > 30)
                {
                    s.nCC = 0;
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY6_1) + R.Next(2)]);
                }
                break;
            case 5: // Teaching
                switch (s.nCC % 46)
                {
                    case 1:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY4_2)]); break;
                    case 6:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY4_1)]); break;
                    case 21:
                        s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpPOPBOY4_2)]); break;
                    case 26:
                        s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpPOPBOY4_1)]); break;
                }
                if (s.nCC == 12)
                {
                    lSound[((int)ASLList.lsndPOPBOY_ADVICE1) + s.nAttrib[((int)nattrFrosh.attrMotivation)]].Play(SoundbankInfo.volHOLLAR, panONX(s));
                    lSound[((int)ASLList.lsndEXAM_TOSS1)].Stop();
                    s.nAttrib[((int)nattrFrosh.attrMotivation)]++; s.nAttrib[((int)nattrFrosh.attrMotivation)] %= 6;
                    NOSPEECHFOR(102);
                    aisThinkForAl(s);
                }
                else if (s.nCC > 24 && !lSound[((int)ASLList.lsndPOPBOY_ADVICE1)].IsPlaying() &&
                    !lSound[((int)ASLList.lsndPOPBOY_ADVICE2)].IsPlaying() &&
                    !lSound[((int)ASLList.lsndPOPBOY_ADVICE3)].IsPlaying() &&
                    !lSound[((int)ASLList.lsndPOPBOY_ADVICE4)].IsPlaying() &&
                    !lSound[((int)ASLList.lsndPOPBOY_ADVICE5)].IsPlaying() &&
                    !lSound[((int)ASLList.lsndPOPBOY_ADVICE6)].IsPlaying())
                {
                    s.nAttrib[((int)nattrFrosh.attrBehavior)] = 3; s.nCC = 0;
                }
                break;
            case 6: // Booze up
                switch (s.nCC)
                {
                    case 1:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY3_4)]); break;
                    case 6:
                        sSound[((int)ASSList.ssndEFFECTS_CHUG)].Play(SoundbankInfo.volFULL, panONX(s));
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY3_3)]); break;
                    case 27:
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY3_1)]);
                        lSound[((int)ASLList.lsndFROSH_CLARKFINISH1) + R.Next(SoundbankInfo.nsndFROSH_CLARKFINISH)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                        break;
                    case 43:
                        Globals.Analytic("PopBoyBeer");
                        s.nAttrib[((int)nattrFrosh.attrMindSet)]++; s.nAttrib[((int)nattrFrosh.attrMindSet)] %= (Globals.myGameConditions.IsRitual() ? 1 : 3);
                        lSound[((int)ASLList.lsndPOPBOY_BEER1) + s.nAttrib[((int)nattrFrosh.attrMindSet)]].Play(SoundbankInfo.volHOLLAR, panONX(s));
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY3_2)]);
                        NOSPEECHFOR(80);
                        break;
                    case 60:
                        s.nAttrib[((int)nattrFrosh.attrBehavior)] = 3; s.nCC = 0;
                        break;
                }
                break;
            case 7:
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY7_1)]);
                break;
        }

    }

    public const int timePOPBOYJUMP = 70;
    public const int timeALCROUCH = 12;
    public static void aiPopBoyInCrowd(TSprite s)
    {
        if (s.nCC < timePOPBOYJUMP || lSound[((int)ASLList.lsndPOPBOY_CHEER1)].IsPlaying())
        {
            switch (s.nCC % 51)
            {
                case 2: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY1_2)]); break; // YEAH!!
                case 4: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY1_1)]);
                    lSound[((int)ASLList.lsndEXAM_TOSS1)].Stop(); break;
                case 12: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY1_2)]);
                    lSound[((int)ASLList.lsndEXAM_TOSS1)].Stop(); break; // YEAH!!
                case 14: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpPOPBOY1_3)]); break;
                case 22: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY1_3)]); break;
                case 24: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpPOPBOY1_2)]); break; // YEAH!!
                case 26: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpPOPBOY1_1)]); break;
                case 34: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpPOPBOY1_2)]); break;
                case 41: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY1_3)]); break;
                case 43: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpPOPBOY1_3)]); break;
                case 51: s.nCC = 0; break;
            }
        }
        else
        {
            if (s.nCC == timePOPBOYJUMP - 10)
            {
                lSound[((int)ASLList.lsndPOPBOY_CHEER1)].Play(SoundbankInfo.volCROWD, panONX(s));
            }
            else if (s.nCC == timePOPBOYJUMP)
            {
                aisThinkForAl(s, false);
            }
            else
            { // It is time.	
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY5_2)]);
                if (s.nCC < 1000)
                    s.nCC = 1000;
                if (s.nCC == 1002)
                    lSound[((int)ASLList.lsndPOPBOY_CHEER2)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                if (s.nCC == 1000 + timeALCROUCH)
                {
                    s.bDeleted = true;
                    Globals.myGameConditions.PopBoyJumpsIn();
                    sprPole.frmFrame.nZ2 -= 127; // HACKUS SUPREMUS
                    sprPopBoy = SpriteInit.CreateSprite((SpriteType.sprPOPBOY), s.nX, 0);
                    AIMethods.aisUnlockAchievement(10);

                    s.nX += 20; s.nY += 60;
                    aisThinkForAl(s);
                    s.nX -= 20; s.nY -= 60;
                    //				sprPopBoy.nZ = s.nY - dSKYLINEYTOPITY;
                    sprPopBoy.nZ = 26; // This was originally calculated with the above.
                    sprPopBoy.nvZ = 16; sprPopBoy.nvX = 2; sprPopBoy.nvY = 6;
                    ssPit.Include(sprPopBoy);
                }
            }


        }
    }
}