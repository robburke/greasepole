public static partial class AIMethods
{

    public const int SCREENBOTTOM = 205;

    public const int MOREAPPLESEASY = 12;
    public const int MOREAPPLESHARD = 10;
    public const int MOREPIZZAEASY = 10;
    public const int MOREPIZZAHARD = 5;
    public const int MORECLARKEASY = 10;
    public const int MORECLARKHARD = 5;

    public static void aisCheckForGeneralistAchievement()
    {
        if (Globals.myGameConditions.GetPlayerApples() > 0
            && Globals.myGameConditions.GetPlayerClark() > 0
            && Globals.myGameConditions.GetPlayerPizza() > 0
            && Globals.myGameConditions.GetPlayerExam() > 0)
            AIMethods.aisUnlockAchievement(404);
    }

    public static void aiGetApples(TSprite s)
    {
        if (s.nCC == 1)
            sSound[((int)ASSList.ssndEFFECTS_SNATCH1)].Play(SoundbankInfo.volNORMAL, panONX(s));
        else if (s.nCC == 5
            && (!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING) && sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armIRONRING)))
            && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armIRONRING))
        {
            sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
            sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armAPPLE);
            sprArm.nCC = 0;
        }
        else if (s.nCC >= MOREAPPLESEASY || (s.nCC >= MOREAPPLESHARD && 0 != Globals.myGameConditions.GetFroshLameness()))
            s.bDeleted = true;
        s.nY += s.nCC + Globals.myGameConditions.GetFroshLameness();

        s.nX += ((sprArm.nX - s.nX) / 5);

        TSprite sprBounce;
        sprBounce = SpriteInit.CreateSprite((SpriteType.sprAPPLE), s.nX, s.nY);
        sprBounce.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingRebounding);
        sprBounce.nvX = randintin(-8, 8);
        sprBounce.nvY = 0;
        sprBounce.nvZ = (R.Next(30));
        Globals.myGameConditions.GetApples(1);
        if (Globals.myGameConditions.GetPlayerApples() >= 99) aisUnlockAchievement(399);
        ssPit.Include(sprBounce);
        aisCheckForGeneralistAchievement();
    }

    public static void aiGetPizza(TSprite s)
    {
        if (s.nCC == 1)
            sSound[((int)ASSList.ssndEFFECTS_SNATCH1)].Play(SoundbankInfo.volNORMAL, panONX(s));
        else if (s.nCC == 5
            && (!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING) && sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armIRONRING)))
            && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armIRONRING))
        {
            sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
            sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armPIZZA);
            sprArm.nCC = 0;
        }
        else if (s.nCC >= MOREPIZZAEASY || (s.nCC >= MOREPIZZAHARD && 0 != Globals.myGameConditions.GetFroshLameness()))
            s.bDeleted = true;
        s.nY += s.nCC + Globals.myGameConditions.GetFroshLameness();

        s.nX += ((sprArm.nX - s.nX) / 5);

        TSprite sprBounce;
        sprBounce = SpriteInit.CreateSprite((SpriteType.sprPIZZA), s.nX, s.nY);
        sprBounce.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingRebounding);
        sprBounce.nvX = randintin(-8, 8);
        sprBounce.nvY = 0;
        sprBounce.nvZ = (R.Next(30));
        Globals.myGameConditions.GetPizzas(1);
        if (Globals.myGameConditions.GetPlayerPizza() >= 99) aisUnlockAchievement(399);

        ssPit.Include(sprBounce);
        aisCheckForGeneralistAchievement();
    }

    public static void aiGetClark(TSprite s)
    {
        if (s.nCC == 1)
            sSound[((int)ASSList.ssndEFFECTS_SNATCH1)].Play(SoundbankInfo.volNORMAL, panONX(s));
        else if (s.nCC == 5
            && (!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING) && sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armIRONRING)))
            && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armIRONRING))
        {
            sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
            sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armCLARK);
            sprArm.nCC = 0;
        }
        else if (s.nCC >= MORECLARKEASY || (s.nCC >= MORECLARKHARD && 0 != Globals.myGameConditions.GetFroshLameness()))
            s.bDeleted = true;
        s.nY += s.nCC + Globals.myGameConditions.GetFroshLameness();

        s.nX += ((sprArm.nX - s.nX) / 5);

        TSprite sprBounce;
        sprBounce = SpriteInit.CreateSprite((SpriteType.sprCLARK), s.nX, s.nY);
        sprBounce.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingRebounding);
        sprBounce.nvX = randintin(-8, 8);
        sprBounce.nvY = 0;
        sprBounce.nvZ = (R.Next(30));
        Globals.myGameConditions.GetClarks(1);
        if (Globals.myGameConditions.GetPlayerClark() >= 99) aisUnlockAchievement(399);

        ssPit.Include(sprBounce);
        aisCheckForGeneralistAchievement();
    }

    public static void aiGetExam(TSprite s)
    {
        if (s.nCC == 1)
        {
            Globals.myGameConditions.GetExams(1);
            sSound[((int)ASSList.ssndEFFECTS_SNATCH1)].Play(SoundbankInfo.volNORMAL, panONX(s));
        }
        else if (s.nCC == 5
            && (!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING) && sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armIRONRING)))
            && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armIRONRING))
        {
            sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
            sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armEXAM);
            sprArm.nCC = 0;
        }
        else if (s.nCC >= 20)
            s.bDeleted = true;

        s.nY += s.nCC + 2 + Globals.myGameConditions.GetFroshLameness();
        s.nX += ((sprArm.nX - s.nX) / 5);
        aisCheckForGeneralistAchievement();
    }

    public static void aiGetHose(TSprite s)
    {
        sSound[((int)ASSList.ssndEFFECTS_SNATCH1)].Play(SoundbankInfo.volNORMAL, panONX(s));

        {
            if ((!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING) && sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armIRONRING)))
            && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armIRONRING))
                sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
            sprArm.nAttrib[((int)attrArm.attrArmAction)] = ((int)ArmPositions.armGREASE);
            sprArm.nAttrib[((int)attrArm.attrKickback)] = 0;
            sprArm.nCC = 0;
            s.bDeleted = true;

            sprWaterMeter = SpriteInit.CreateSprite((SpriteType.sprWATERMETER), 26, 17);
            ssConsole.Include(sprWaterMeter);
        }
        //Globals.myGameConditions.GetHose(50);
    }


    public static void aisAlienInPit(TSprite s, int nFirstFrame, int nAnimationBitmaps)
    {
        s.nZ = 0;
        int nTotalCycledFrames = nAnimationBitmaps * 2 - 1;
        int nCurrentFrame = (s.nCC / 5) % nTotalCycledFrames;
        int nCurrentBitmap =
            (nCurrentFrame < nAnimationBitmaps)
            ? nCurrentFrame : nTotalCycledFrames - nCurrentFrame - 1;

        if (s.nCC < 50)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
        else
        {
            if (s.nX < dPOLEX)
            {
                s.SetFrame(frmM[nFirstFrame + nCurrentBitmap]);
                s.nX -= 2;
            }
            else
            {
                s.SetFrame(frm[nFirstFrame + nCurrentBitmap]);
                s.nX += 2;
            }
            if (s.nX < -60 || s.nX > 640 + 60)
            {
                if (s == sprAlien)
                    sprAlien = null;  // DO THIS WHENEVER THE ALIEN IS DELETED
                s.bDeleted = true;
            }
        }
    }

    public static void aiArtSciMInPit(TSprite s)
    {
        if (s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)])
        {
            if (0 == R.Next(40))
                s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = false;
            else
            {
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpARTSCIM_HIT1) + ((s.nCC / 40) % 2)]);
                if (0 == (s.nCC % 40))
                    if (Globals.myGameConditions.IsRitual())
                        lSound[((int)ASLList.lsndARTSCI_MALE_HIT1) + ((s.nCC / 40) % 8)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                    else
                        lSound[((int)ASLList.lsndARTSCI_MALE_HIT1) + ((s.nCC / 40) % 5)].Play(SoundbankInfo.volHOLLAR, panONX(s));
            }
        }
        else
            aisAlienInPit(s, ((int)GameBitmapEnumeration.bmpARTSCIM_WADE1), nsprARTSCIM_WADE);

    }

    public static void aiArtSciFInPit(TSprite s)
    {
        if (s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)])
        {
            if (0 == R.Next(40))
                s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = false;
            else
            {
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpARTSCIF_HIT1)]);
                if (0 == (s.nCC % 40))
                    lSound[((int)ASLList.lsndARTSCI_FEMALE_HIT1) + ((s.nCC / 40) % 5)].Play(SoundbankInfo.volHOLLAR, panONX(s));
            }
        }
        else
            aisAlienInPit(s, ((int)GameBitmapEnumeration.bmpARTSCIF_WADE1), nsprARTSCIF_WADE);
    }
    public static void aiCommieMInPit(TSprite s)
    {
        if (s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)])
        {
            if (0 == R.Next(40))
                s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = false;
            else
            {
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCOMMIEM_HIT1)]);
                if (0 == (s.nCC % 50))
                    lSound[((int)ASLList.lsndCOMMIE_MALE_HIT1) + ((s.nCC / 50) % 7)].Play(SoundbankInfo.volHOLLAR, panONX(s));
            }
        }
        else
            aisAlienInPit(s, ((int)GameBitmapEnumeration.bmpCOMMIEM_WADE1), nsprCOMMIEM_WADE);
    }
    public static void aiCommieFInPit(TSprite s)
    {
        if (s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)])
        {
            if (0 == R.Next(40))
                s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = false;
            else
            {
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCOMMIEF_HIT1) + ((s.nCC / 40) % 2)]);
                if (0 == (s.nCC % 75))
                    lSound[((int)ASLList.lsndCOMMIE_FEMALE_HIT1) + ((s.nCC / 75) % 4)].Play(SoundbankInfo.volHOLLAR, panONX(s));
            }
        }
        else
            aisAlienInPit(s, ((int)GameBitmapEnumeration.bmpCOMMIEF_WADE1), nsprCOMMIEF_WADE);
    }

    public const int timeFALLINGFRAMES = 20;

    public static void aisPushIntoPit(TSprite s, int nFirstFrame, int nAnimationBitmaps)
    {
        aisForgeTrick(8, 200); // Push Alien into pit first time
        AIMethods.aisUnlockAchievement(4);
        Globals.myGameConditions.AddEnergy(50);

        if (s.nCC <= 10)
        {
            if (s.nCC > 4)
                s.SetFrame(frm[nFirstFrame]);
            s.nY = SCREENBOTTOM; s.nvZ = 25; s.nvX = -3; s.nvY = -1;
            if (s.nCC == 1)
                sSound[((int)ASSList.ssndEFFECTS_PUSH1)].Play(SoundbankInfo.volNORMAL, panONX(s));
            s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = false;
        }
        else
        {
            aisPlummet(s);
            if (s.nvZ > -10)
                s.SetFrame(frm[nFirstFrame + 1]);
            else
                s.SetFrame(frm[nFirstFrame + 2]);
        }
    }


    public static void aiPushArtSciMIntoPit(TSprite s)
    {
        AIMethods.aisUnlockAchievement(4);
        // Push sound effect
        int i;
        if (s.nCC == 1)
        {
            for (i = 0; i < 7; i++)
                lSound[((int)ASLList.lsndARTSCI_MALE_TAUNT1) + i].Stop();
            lSound[((int)ASLList.lsndARTSCI_MALE_TAUNTR1)].Stop();
            lSound[((int)ASLList.lsndARTSCI_MALE_PUSH1) + R.Next(4)].Play(SoundbankInfo.volHOLLAR, panONX(s));
        }
        // Be pushed into the pit 
        aisPushIntoPit(s, ((int)GameBitmapEnumeration.bmpARTSCIM_FALL1), nsprARTSCIM_FALL);
        if ((s.nZ <= 0 && s.nvZ < 0))
        {
            // Change to the function in Male ArtSci in Pit
            ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), s.nX, s.nY));
            s.SetBehavior(aiArtSciMInPit);
            aisChaseAlien(true); // Beat the livinshit out of John
        }
    }
    public static void aiPushArtSciFIntoPit(TSprite s)
    {
        AIMethods.aisUnlockAchievement(4);
        // Push sound effect
        int i;
        if (s.nCC == 1)
        {
            for (i = 0; i < 6; i++)
                lSound[((int)ASLList.lsndARTSCI_FEMALE_TAUNT1) + i].Stop();
            lSound[((int)ASLList.lsndARTSCI_FEMALE_PUSH1)].Play(SoundbankInfo.volHOLLAR, panONX(s));
        }
        // Be pushed into the put 
        aisPushIntoPit(s, ((int)GameBitmapEnumeration.bmpARTSCIF_FALL1), nsprARTSCIF_FALL);
        if ((s.nZ <= 0 && s.nvZ < 0))
        {  // Change to the function in Male ArtSci in Pit
            ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), s.nX, s.nY));
            s.SetBehavior(aiArtSciFInPit);
            aisChaseAlien(false);
        }
    }
    public static void aiPushCommieMIntoPit(TSprite s)
    {
        AIMethods.aisUnlockAchievement(4);
        // Push sound effect
        int i;
        if (s.nCC == 1)
        {
            for (i = 0; i < 2; i++)
                lSound[((int)ASLList.lsndCOMMIE_MALE_TAUNT1) + i].Stop();
            lSound[((int)ASLList.lsndCOMMIE_MALE_TAUNTR1)].Stop();
            lSound[((int)ASLList.lsndCOMMIE_MALE_TAUNTR2)].Stop();
            lSound[((int)ASLList.lsndCOMMIE_MALE_TAUNTR3)].Stop();
            lSound[((int)ASLList.lsndCOMMIE_MALE_PUSH1) + R.Next(4)].Play(SoundbankInfo.volHOLLAR, panONX(s));
        }
        // Be pushed into the put 
        aisPushIntoPit(s, ((int)GameBitmapEnumeration.bmpCOMMIEM_FALL1), nsprCOMMIEM_FALL);
        if ((s.nZ <= 0 && s.nvZ < 0))
        {
            // Change to the function in Male ArtSci in Pit
            ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), s.nX, s.nY));
            s.SetBehavior(aiCommieMInPit);
            aisChaseAlien(true);
        }
    }
    public static void aiPushCommieFIntoPit(TSprite s)
    {
        AIMethods.aisUnlockAchievement(4);
        // Push sound effect
        int i;
        if (s.nCC == 1)
        {
            for (i = 0; i < 3; i++)
                lSound[((int)ASLList.lsndCOMMIE_FEMALE_TAUNT1) + i].Stop();
            lSound[((int)ASLList.lsndCOMMIE_FEMALE_PUSH1)].Play(SoundbankInfo.volHOLLAR, panONX(s));
        }
        // Be pushed into the put 
        aisPushIntoPit(s, ((int)GameBitmapEnumeration.bmpCOMMIEF_FALL1), nsprCOMMIEF_FALL);
        if ((s.nZ <= 0 && s.nvZ < 0))
        {
            // Change to the function in Male ArtSci in Pit
            ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), s.nX, s.nY));
            s.SetBehavior(aiCommieFInPit);
            aisChaseAlien(true);
        }
    }
    public const int timeTPBM = 45;

    public static void aiPushSciConM(TSprite s)
    {
        int i;
        switch (s.nCC)
        {
            case 1:
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSCICONM2)]);
                NOSPEECHFOR(450);
                for (i = ((int)ASLList.lsndAPPLES_OFFER1); i < ((int)ASLList.lsndRING_ZAP3); i++)
                    lSound[i].Stop();
                lSound[((int)ASLList.lsndSCICONM_HitMisc)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                break;
            case 25:
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSCICONM3)]);
                AIMethods.aisUnlockAchievement(11);
                break;
            case 45:
                Globals.myGameConditions.gbTriPubBan = true;
                lSound[((int)ASLList.lsndFRECS_HITAPPLE2)].Play(SoundbankInfo.volHOLLAR);
                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprTRI)));
                break;
            case timeTPBM + 15:
                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPUB)));
                break;
            case timeTPBM + 30:
                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprBAN)));
                break;

            case 136:
                Globals.myGameLoop.ChangeGameState(((int)GameStates.STATETITLE));
                if (Globals.myGameConditions.IsRitual())
                    lSound[((int)ASLList.lsndFRECS_REWARDR1)].Play(SoundbankInfo.volHOLLAR);
                else
                    lSound[((int)ASLList.lsndFRECS_BOO3)].Play(SoundbankInfo.volHOLLAR);
                break;
        }
    }
    public static void aiPushSciConF(TSprite s)
    {
        int i;
        switch (s.nCC)
        {
            case 1:
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSCICONF2)]);
                NOSPEECHFOR(450);
                for (i = ((int)ASLList.lsndAPPLES_OFFER1); i < ((int)ASLList.lsndRING_ZAP3); i++)
                    lSound[i].Stop();
                lSound[((int)ASLList.lsndSCICONF_HitMisc)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                break;
            //#define timeTPBF 85
            case 25:
                AIMethods.aisUnlockAchievement(11);
                break;
            case 85:
                Globals.myGameConditions.gbTriPubBan = true;
                lSound[((int)ASLList.lsndFRECS_HITAPPLE3)].Play(SoundbankInfo.volHOLLAR);
                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprTRI)));
                break;
            case 85 + 15:
                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprPUB)));
                break;
            case 85 + 30:
                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprBAN)));
                break;
            case 170:
                Globals.myGameLoop.ChangeGameState(((int)GameStates.STATETITLE));
                if (Globals.myGameConditions.IsRitual())
                    lSound[((int)ASLList.lsndFRECS_REWARDR1)].Play(SoundbankInfo.volHOLLAR);
                else
                    lSound[((int)ASLList.lsndFRECS_BOO3)].Play(SoundbankInfo.volHOLLAR);
                break;
        }
    }



    public static void aiInitForeGroundEntry(TSprite s)
    {
        switch (s.nAttrib[((int)attrForeGroundPopUpDudes.attrEntryType)])
        {
            case 1: // Moving up
                s.nX = randintin(15, 625);
                s.nY = SCREENBOTTOM + 200; //s.frmFrame.nHeight + 200;
                s.nDestX = s.nX;
                s.nDestY = SCREENBOTTOM;
                break;
            case 2: // Moving right
                s.nX = -200;
                s.nY = SCREENBOTTOM;
                s.nDestX = 250;
                s.nDestY = s.nY;
                break;
            case 3: // Moving left
                s.nX = 640 + 200;
                s.nY = SCREENBOTTOM;
                s.nDestX = 640 - 250;
                s.nDestY = s.nY;
                break;
        }
        s.SetBehavior(aiForeGroundEntry);
    }


    public static void aiForeGroundEntry(TSprite s)
    {
        switch (s.nAttrib[((int)attrForeGroundPopUpDudes.attrEntryType)])
        {
            case 1: // Moving up
                if (s.nY <= s.nDestY + 5)
                {
                    if (s.bAttrib[((int)battrForeGroundPopUpDudes.attrSciConBehavior)])
                        aiInitSciConForegroundOnScreen(s);
                    else
                        aiInitForegroundOnScreen(s);
                }
                else
                {
                    s.nY += (s.nDestY - s.nY) / s.nAttrib[((int)attrForeGroundPopUpDudes.attrRelativeSpeed)];
                }
                break;
            case 2: // Moving right
                if (s.nX >= s.nDestX - 5)
                {
                    if (s.bAttrib[((int)battrForeGroundPopUpDudes.attrSciConBehavior)])
                        aiInitSciConForegroundOnScreen(s);
                    else
                        aiInitForegroundOnScreen(s);
                }
                else
                {
                    s.nX += (s.nDestX - s.nX) / s.nAttrib[((int)attrForeGroundPopUpDudes.attrRelativeSpeed)];
                }
                break;
            case 3: // Moving left
                if (s.nX <= s.nDestX + 5)
                {
                    if (s.bAttrib[((int)battrForeGroundPopUpDudes.attrSciConBehavior)])
                        aiInitSciConForegroundOnScreen(s);
                    else
                        aiInitForegroundOnScreen(s);
                }
                else
                {
                    s.nX += (s.nDestX - s.nX) / s.nAttrib[((int)attrForeGroundPopUpDudes.attrRelativeSpeed)];
                }
                break;
        }
        if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s)
            && (!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING) && sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armIRONRING)))
            && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armIRONRING))
        {
            sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armNOTHING);
            sprArm.nAttrib[((int)attrArm.attrArmAction)] = 1; sprArm.nCC = 0;
            s.SwitchToSecondaryBehavior();
            s.nCC = 0;
        }
    }

    public static void aiInitForegroundOnScreen(TSprite s)
    {
        s.nX = s.nDestX;
        s.nY = s.nDestY;
        s.SetBehavior(aiForegroundOnScreen);
        s.nCC = 0;
    }

    public static void aiForegroundOnScreen(TSprite s)
    {
        if (s.nCC > (0 != Globals.myGameConditions.GetFroshLameness() ? 15 : 25))
            aiInitForeGroundExit(s);
        if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s)
            && (!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING) && sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armIRONRING)))
            && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armIRONRING))
        {
            sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armNOTHING);
            sprArm.nAttrib[((int)attrArm.attrArmAction)] = 1; sprArm.nCC = 0;
            s.SwitchToSecondaryBehavior();
            s.nCC = 0;
        }
    }

    public static void aiInitSciConForegroundOnScreen(TSprite s)
    {
        s.nX = s.nDestX;
        s.nY = s.nDestY;
        s.nAttrib[((int)attrForeGroundPopUpDudes.attrWeaponBeingTaken)] = ((int)ArmPositions.armNOTHING);
        s.SetBehavior(aiSciConForegroundOnScreen);
        s.nCC = 0;
    }


    public static void aiSciConForegroundOnScreen(TSprite s)
    {
        TSprite sprBounce;
        int nTmp;
        switch (s.nAttrib[((int)attrForeGroundPopUpDudes.attrWeaponBeingTaken)])
        {
            case ((int)ArmPositions.armNOTHING):
                break;
            case ((int)ArmPositions.armAPPLE):
                nTmp = Globals.myGameConditions.GetPlayerApples();
                if (0 != nTmp)
                {
                    sprBounce = SpriteInit.CreateSprite((SpriteType.sprAPPLE), sprArm.nX, s.nY);
                    sprBounce.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingRebounding);
                    sprBounce.SetFrame(frm[((int)GameBitmapEnumeration.bmpAPPLE5_1) + R.Next(nsprAPPLE5)]);
                    sprBounce.nvX = randintin(-8, 8);
                    sprBounce.nvY = 0;
                    sprBounce.nvZ = (R.Next(30));
                    ssPit.Include(sprBounce);
                    if (nTmp > 15)
                        Globals.myGameConditions.GetApples(-5);
                    else
                        Globals.myGameConditions.GetApples(-1);
                    s.nCC--;
                }
                else
                {
                    // SWITCH
                    s.nAttrib[((int)attrForeGroundPopUpDudes.attrWeaponBeingTaken)] = ((int)ArmPositions.armNOTHING);
                    sprArm.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                    sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                    sprArm.nCC = 0;
                }
                break;
            case ((int)ArmPositions.armPIZZA):
                nTmp = Globals.myGameConditions.GetPlayerPizza();
                if (0 != nTmp)
                {
                    sprBounce = SpriteInit.CreateSprite((SpriteType.sprPIZZA), sprArm.nX, s.nY);
                    sprBounce.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingRebounding);
                    sprBounce.SetFrame(frm[((int)GameBitmapEnumeration.bmpPIZZA5_1) + R.Next(nsprPIZZA5)]);
                    sprBounce.nvX = randintin(-8, 8);
                    sprBounce.nvY = 0;
                    sprBounce.nvZ = (R.Next(30));
                    ssPit.Include(sprBounce);
                    if (nTmp > 15)
                        Globals.myGameConditions.GetPizzas(-5);
                    else
                        Globals.myGameConditions.GetPizzas(-1);
                    s.nCC--;
                }
                else
                {
                    // SWITCH
                    s.nAttrib[((int)attrForeGroundPopUpDudes.attrWeaponBeingTaken)] = ((int)ArmPositions.armNOTHING);
                    sprArm.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                    sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                    sprArm.nCC = 0;
                }
                break;
            case ((int)ArmPositions.armCLARK):
                nTmp = Globals.myGameConditions.GetPlayerClark();
                if (0 != nTmp)
                {
                    sprBounce = SpriteInit.CreateSprite((SpriteType.sprCLARK), sprArm.nX, s.nY);
                    sprBounce.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingRebounding);
                    sprBounce.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLARK5A_1) + R.Next(nsprCLARK5A)]);
                    sprBounce.nvX = randintin(-8, 8);
                    sprBounce.nvY = 0;
                    sprBounce.nvZ = (R.Next(30));
                    ssPit.Include(sprBounce);
                    if (nTmp > 15)
                        Globals.myGameConditions.GetClarks(-5);
                    else
                        Globals.myGameConditions.GetClarks(-1);
                    s.nCC--;
                }
                else
                {
                    // SWITCH
                    s.nAttrib[((int)attrForeGroundPopUpDudes.attrWeaponBeingTaken)] = ((int)ArmPositions.armNOTHING);
                    sprArm.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                    sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                    sprArm.nCC = 0;
                }
                break;
            case ((int)ArmPositions.armEXAM):
                nTmp = Globals.myGameConditions.GetPlayerExam();
                if (0 != nTmp)
                {
                    sprBounce = SpriteInit.CreateSprite((SpriteType.sprEXAM), sprArm.nX, s.nY);
                    sprBounce.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingRebounding);
                    sprBounce.SetFrame(frm[((int)GameBitmapEnumeration.bmpEXAM4_1) + R.Next(nsprEXAM4)]);
                    sprBounce.nvX = randintin(-8, 8);
                    sprBounce.nvY = 0;
                    sprBounce.nvZ = (R.Next(30));
                    ssPit.Include(sprBounce);
                    Globals.myGameConditions.GetExams(-1);
                    s.nCC--;
                }
                else
                {
                    // SWITCH
                    s.nAttrib[((int)attrForeGroundPopUpDudes.attrWeaponBeingTaken)] = ((int)ArmPositions.armNOTHING);
                    sprArm.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                    sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                    sprArm.nCC = 0;
                }
                break;
        }

        if (Globals.InputService.LeftButtonPressed())
        {  // IF THE PLAYER IS ATTACKING
            s.nCC = 0;
            if (aisMouseOver(s))
            { // TRI PUB BAN!
                sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armNOTHING);
                sprArm.nAttrib[((int)attrArm.attrArmAction)] = 1; sprArm.nCC = 0;
                s.SwitchToSecondaryBehavior();
            }
            else if (sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armNOTHING))
            { // LOSS OF WEAPONRY
                s.SetFrame(frm[s.bAttrib[((int)battrForeGroundPopUpDudes.attrMale)] ? ((int)GameBitmapEnumeration.bmpSCICONM2) : ((int)GameBitmapEnumeration.bmpSCICONF2)]);
                if (s.bAttrib[((int)battrForeGroundPopUpDudes.attrMale)] && (sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armAPPLE) || sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCLARK)))
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSCICONM3)]);
                sSound[((int)ASSList.ssndEFFECTS_SNATCH1)].Play(SoundbankInfo.volNORMAL, panONX(s));
                s.nAttrib[((int)attrForeGroundPopUpDudes.attrWeaponBeingTaken)] = sprArm.nAttrib[((int)attrArm.attrArmStatus)];

                switch (sprArm.nAttrib[((int)attrArm.attrArmStatus)])
                {
                    case ((int)ArmPositions.armNOTHING):
                        //s.bDeleted = true;
                        //				lSound[s.bAttrib[((int)battrForeGroundPopUpDudes.attrMale)] ? ((int)ASLList.lsndSCICONM_HitApples) : ((int)ASLList.lsndSCICONF_HitApples)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                        break;
                    case ((int)ArmPositions.armAPPLE):
                        lSound[s.bAttrib[((int)battrForeGroundPopUpDudes.attrMale)] ? ((int)ASLList.lsndSCICONM_HitApples) : ((int)ASLList.lsndSCICONF_HitApples)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                        break;
                    case ((int)ArmPositions.armPIZZA):
                        lSound[s.bAttrib[((int)battrForeGroundPopUpDudes.attrMale)] ? ((int)ASLList.lsndSCICONM_HitPizza) : ((int)ASLList.lsndSCICONF_HitPizza)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                        break;
                    case ((int)ArmPositions.armCLARK):
                        lSound[s.bAttrib[((int)battrForeGroundPopUpDudes.attrMale)] ? ((int)ASLList.lsndSCICONM_HitBeer) : ((int)ASLList.lsndSCICONF_HitBeer)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                        break;
                    case ((int)ArmPositions.armEXAM):
                        lSound[s.bAttrib[((int)battrForeGroundPopUpDudes.attrMale)] ? ((int)ASLList.lsndSCICONM_HitExam) : ((int)ASLList.lsndSCICONF_HitExam)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                        break;
                    default:
                        break;
                }
            }
        }

        if (s.nCC > (0 != Globals.myGameConditions.GetFroshLameness() ? 45 : 25))
            aiInitForeGroundExit(s);
    }

    public static void aiInitForeGroundExit(TSprite s)
    {
        switch (s.nAttrib[((int)attrForeGroundPopUpDudes.attrExitType)])
        {
            case 1: // Moving down
                s.nDestX = s.nX;
                s.nDestY = SCREENBOTTOM + 200;
                break;
            case 2: // Moving left
                s.nDestX = -200;
                s.nDestY = s.nY;
                break;
            case 3: // Moving right
                s.nDestX = 640 + 200;
                s.nDestY = s.nY;
                break;
        }
        s.SetBehavior(aiForeGroundExit);

    }

    public static void aiForeGroundExit(TSprite s)
    {
        switch (s.nAttrib[((int)attrForeGroundPopUpDudes.attrEntryType)])
        {
            case 1: // Moving up
                if (s.nY >= s.nDestY - 5)
                {
                    if (s == sprAlien)
                        sprAlien = null;  // DO THIS WHENEVER THE ALIEN IS DELETED
                    s.bDeleted = true;
                }
                else
                {
                    s.nY += (s.nDestY - s.nY) / s.nAttrib[((int)attrForeGroundPopUpDudes.attrRelativeSpeed)];
                }
                break;
            case 2: // Moving left
                if (s.nX <= s.nDestX + 5)
                {
                    if (s == sprAlien)
                        sprAlien = null;  // DO THIS WHENEVER THE ALIEN IS DELETED
                    s.bDeleted = true;
                }
                else
                {
                    s.nX += (s.nDestX - s.nX) / s.nAttrib[((int)attrForeGroundPopUpDudes.attrRelativeSpeed)];
                }
                break;
            case 3: // Moving right
                if (s.nX >= s.nDestX - 5)
                {
                    if (s == sprAlien)
                        sprAlien = null;  // DO THIS WHENEVER THE ALIEN IS DELETED
                    s.bDeleted = true;
                }
                else
                {
                    s.nX += (s.nDestX - s.nX) / s.nAttrib[((int)attrForeGroundPopUpDudes.attrRelativeSpeed)];
                }
                break;
        }
        if (Globals.InputService.LeftButtonPressed() && aisMouseOver(s)
            && (!(sprArm.nAttrib[((int)attrArm.attrArmStatus)] == ((int)ArmPositions.armCHANGING) && sprArm.nAttrib[((int)attrArm.attrArmAction)] == ((int)ArmPositions.armIRONRING)))
            && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armIRONRING))
        {
            sprArm.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armNOTHING);
            sprArm.nAttrib[((int)attrArm.attrArmAction)] = 1; sprArm.nCC = 0;
            s.SwitchToSecondaryBehavior();
            s.nCC = 0;
        }
    }
    public static void aiInitPopUp(TSprite s, FrameDesc newFrame, int nSpeed,
                      int nEntryType, int nOnScreenType, int nExitType,
                      int nVoiceEffect, AIMethod fSecondaryBehavior)
    {
        aiInitPopUp(s, newFrame, nSpeed,
                      nEntryType, nOnScreenType, nExitType,
                      nVoiceEffect, fSecondaryBehavior, false);
    }

    public static void aiInitPopUp(TSprite s, FrameDesc newFrame, int nSpeed,
                      int nEntryType, int nOnScreenType, int nExitType,
                      int nVoiceEffect, AIMethod fSecondaryBehavior,
                      bool bSciConEffect)
    {
        // This is the pop up

        s.nAttrib[((int)attrForeGroundPopUpDudes.attrRelativeSpeed)] = nSpeed;
        s.nAttrib[((int)attrForeGroundPopUpDudes.attrEntryType)] = nEntryType;
        s.nAttrib[((int)attrForeGroundPopUpDudes.attrOnScreenType)] = nOnScreenType;
        s.nAttrib[((int)attrForeGroundPopUpDudes.attrExitType)] = nExitType;
        s.bAttrib[((int)battrForeGroundPopUpDudes.attrSciConBehavior)] = bSciConEffect;

        s.SetSecondaryBehavior(fSecondaryBehavior);

        s.SetFrame(newFrame);
        aiInitForeGroundEntry(s);
        lSound[nVoiceEffect].Play(SoundbankInfo.volHOLLAR, panONX(s));
    }
}