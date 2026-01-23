public static partial class AIMethods
{

    public static void aiInanimate(TSprite s) { ;}
    // (Earth-shattering, no?)

    public static void aiShowCurrentAchievementScreen(TSprite s)
    {
        int achievementGroup = Globals.myGameConditions.GetAchievementGroup();
        s.Text = (achievementGroup+1).ToString() + " of 3";
    }

    public static void aisUnlockAchievement(int achievementNumber)
    {
        foreach (PoleGameAchievement a in PoleGameAchievement.List)
        {
            if (a.AchievementGuid == achievementNumber)
            {
                if (a.Achieved) return;
                a.Achieved = true;
                Globals.Analytic("AchUnl;" + achievementNumber.ToString());
                a.WhenAchieved = System.DateTime.Now;
                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprACHIEVEMENTUNLOCKED)));
                TSprite text1 = SpriteInit.CreateSprite((SpriteType.sprACHIEVEMENTUNLOCKEDTEXT1));
                text1.SpriteText = SpriteTextType.Small;
                text1.Text = "Achievement Unlocked";
                ssIcons.Include(text1);
                TSprite text2 = SpriteInit.CreateSprite((SpriteType.sprACHIEVEMENTUNLOCKEDTEXT2));
                text2.SpriteText = SpriteTextType.Small;
                text2.Text = a.Name;
                text2.nA = 0;
                ssIcons.Include(text2);
                Globals.myGameConditions.SaveSettingsToStorage();
            }
        }
    }

    public static void aiTam(TSprite s)
    {
        if (Globals.InputService.BackButtonPressed())
        {
            Globals.myGameLoop.ChangeGameState(((int)GameStates.STATETITLE));
        }

        if (0 == (s.nAttrib[((int)attrTam.attrTamStatus)]))
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpTAM0_1)]);
        else
        {
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
            if (s.nAttrib[((int)attrTam.attrTamStatus)] == 1)
                s.nAttrib[((int)attrTam.attrTamStatus)] = 0;
        }
    }

    // Added to J Section Release 3 to get a bit more fun crowd noise.
    // Modfied in J Section Release 4 to keep things under control
    public static int nNextCheer = 3;

    public static void aisRandomCrowdNoise()
    {
        nNextCheer++;
        if (Globals.myGameConditions.IsRitual())
            nNextCheer %= 3;
        else
        {
            nNextCheer %= 2;
        }

        int nNoise = Globals.myGameConditions.GetNoiseCount();
        Globals.myGameConditions.SetNoiseCount(nNoise + 1);
        if (nNoise > 1280 && (SPEECHOK()))
        {
            NOSPEECHFOR(48);
            Globals.myGameConditions.SetNoiseCount(0);
            if (nFroshAbove1 < 6 && 0 == R.Next(3))
            {
                if (Globals.myGameConditions.GetNoiseCount() > 1000 && !(lSound[((int)ASLList.lsndFRECS_REWARD5)].IsPlaying() || lSound[((int)ASLList.lsndFRECS_REWARD6)].IsPlaying() || lSound[((int)ASLList.lsndFRECS_REWARDR1)].IsPlaying()))
                {
                    Globals.myGameConditions.SetNoiseCount(0);
                    lSound[((int)ASLList.lsndFRECS_REWARD5) + nNextCheer].Play(SoundbankInfo.volCROWD, randintin(SoundbankInfo.panLEFT / 2, SoundbankInfo.panRIGHT / 2));
                }
            }
            else if (nFroshAbove1 > 20 && 0 != R.Next(2))
                lSound[((int)ASLList.lsndFRECS_PROGRESS2) + R.Next(Globals.myGameConditions.IsRitual() ? SoundbankInfo.nsndFRECS_PROGRESS + 1 - 1 : SoundbankInfo.nsndFRECS_PROGRESS - 1)].Play(SoundbankInfo.volCROWDSHOUT, randintin(SoundbankInfo.panLEFT / 2, SoundbankInfo.panRIGHT / 2));
            else if (0 != R.Next(3))
            {
                if (0 != R.Next(2))
                {
                    if (Globals.myGameConditions.IsRitual() && 0 != R.Next(2))
                        lSound[((int)ASLList.lsndFRECS_CHANT1) + R.Next(3)].Play(SoundbankInfo.volCROWD, randintin(SoundbankInfo.panLEFT / 2, SoundbankInfo.panRIGHT / 2));
                    else
                        lSound[((int)ASLList.lsndFRECS_CHANT1)].Play(SoundbankInfo.volCROWD, randintin(SoundbankInfo.panLEFT / 2, SoundbankInfo.panRIGHT / 2));
                }
                else
                    lSound[((int)ASLList.lsndFRECS_HOWHIGHTHEPOLE)].Play(SoundbankInfo.volCROWDSHOUT, randintin(SoundbankInfo.panLEFT / 2, SoundbankInfo.panRIGHT / 2));
            }
        }
    }

    public static void aiRandomEventGenerator(TSprite s)
    {
        // For now, the Podium is a "source" of random events.
        aisRandomEvents(s);
        aisRandomCrowdNoise();

        // This code is responsible for preventing annoying overlap of voices.
        // If a certain time has elapsed, something happens.
        if (!Globals.myGameConditions.IsDemo())
        {
            if (s.nCC > timeBETWEENEVENTS && (0 == R.Next(100))
                && sprTam.nAttrib[((int)attrTam.attrTamStatus)] != 2)
            {
                s.nCC = 0;

                int nTotalWeapons = Globals.myGameConditions.GetPlayerApples()
                    + Globals.myGameConditions.GetPlayerClark()
                    + Globals.myGameConditions.GetPlayerPizza()
                    + Globals.myGameConditions.GetPlayerExam();

                if ((sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armGREASE)
                    && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armIRONRING))
                    && ((sprAlien == null && 0 != R.Next(3)) ||
                    (nTotalWeapons < 6)))
                {
                    switch (R.Next(13))
                    {
                        case 0:
                            if (nTotalWeapons > 10) ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprPOPUP_HOSE)));
                            else ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprPOPUP_BEER)));
                            break;
                        case 1:
                        case 2:
                            if (nTotalWeapons > 10) ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprPOPUP_EXAM)));
                            else ssPit.Include(SpriteInit.CreateSprite(((0 != R.Next(2)) ? (SpriteType.sprPOPUP_PIZZA) : (SpriteType.sprPOPUP_BEER))));
                            break;
                        case 3:
                        case 4:
                        case 5: ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprPOPUP_BEER))); break;
                        case 6:
                        case 7:
                        case 8: ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprPOPUP_PIZZA))); break;
                        default: ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprPOPUP_APPLES))); break;
                    }
                }
                else if ((0 != R.Next(5)))
                {
                    if (sprAlien == null && (0 != R.Next(5)) && (0 == ((s.nCC) % (24 * 5)))
                        && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armGREASE)
                        && sprArm.nAttrib[((int)attrArm.attrArmStatus)] != ((int)ArmPositions.armIRONRING))
                    {
                        if ((0 != R.Next(4)) || Globals.GameTimerService.GetCurrentGameTimeScoreMilliseconds() < 180)
                        {
                            switch (R.Next(4))
                            {
                                case 0: sprAlien = SpriteInit.CreateSprite((SpriteType.sprPOPUP_ARTSCIF)); break;
                                case 1: sprAlien = SpriteInit.CreateSprite((SpriteType.sprPOPUP_ARTSCIM)); break;
                                case 2: sprAlien = SpriteInit.CreateSprite((SpriteType.sprPOPUP_COMMIEF)); break;
                                case 3: sprAlien = SpriteInit.CreateSprite((SpriteType.sprPOPUP_COMMIEM)); break;
                            }
                            ssPit.Include(sprAlien);
                        }
                        else
                        {
                            // J4 tweak: SciCons don't come in the first minute of play
                            // UNTWEAKED in J5
                            ssPit.Include(SpriteInit.CreateSprite(0 != R.Next(2) ? (SpriteType.sprSCICONF) : (SpriteType.sprSCICONM)));
                        }
                    }
                }
                else
                {
                    if (sprGWBalloon == null && 0 == R.Next(3))
                    {
                        sprGWBalloon = SpriteInit.CreateSprite((SpriteType.sprGWBALLOON));
                        ssBalloon.Include(sprGWBalloon);
                    }
                }
            }
            else
            {
                // If it IS the demo, create some GW balloons for effect.
                if (sprGWBalloon == null && s.nCC > timeBETWEENEVENTS * 10
                    && (0 == R.Next(1000)))
                {
                    sprGWBalloon = SpriteInit.CreateSprite((SpriteType.sprGWBALLOON));
                    ssBalloon.Include(sprGWBalloon);
                }
            }
        }
    }

}