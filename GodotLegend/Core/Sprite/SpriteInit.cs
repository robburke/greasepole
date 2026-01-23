using System;
public static class SpriteInit
{
    public static TSprite CreateSprite(SpriteType SpriteType)
    {
        return CreateSprite(SpriteType, 0, 0);
    }
    private static Random R = new Random(); 
    
    public static TSprite CreateSprite(SpriteType SpriteType, int nX, int nY)
    {
        // Returns a new sprite, initialized as per SpriteType's specifications.
        TSprite newSprite = new TSprite();
        newSprite.bDeleted = false;
        newSprite.nCC = 0;
        newSprite.nX = nX; newSprite.nY = nY;

        // Call InitSprite to initialize the specific sprite.
        InitSprite(newSprite, SpriteType);

        return newSprite;
    }

    public static void DeleteSprite(TSprite aSprite)
    {
        aSprite.Dispose();
    }


    public static void InitSprite(TSprite s, SpriteType spriteType)
    {

        // This function initializes a new sprite.  Occasionally it cues up an 
        // appropriate sound effect to go along with the sprite's creation, too.

        // This is split up into a few Switch statements for organizational purposes.
        // Sprites are ordered such that elements created within the game come first.

        s.SpriteType = spriteType;

        ///////////////////////////////////////////////////////////////
        // MENU /////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        if (spriteType > (SpriteType.sprmnu_START) && spriteType < (SpriteType.sprmnu_END))
        {
            switch (spriteType)
            {
                // MENU
                case (SpriteType.sprmnuMOUSECURSOR):
                    s.bAttrib[1] = true;
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpMOU_SELECT1)]);
                    s.SetBehavior(AIMethods.aiMouseCursorMenu);
                    break;

                case (SpriteType.sprmnuOPTIONSBACK):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpOPTIONSBACK)]);
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiInanimate, 0, 480, 0, 0, 1, 1);
                    break;


                case (SpriteType.sprmnuTITLEBACK):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLEBACK)]);
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiInanimate, 640, 0, 0, 0, 2, 0);
                    break;
                case (SpriteType.sprmnuTITLESTART):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLESTART)]);
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)] = false;
                    s.bAttrib[((int)battrMenuStartButtonAttributes.attrDoNotActivate)] = false;
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiMenuStartButton, -640, 0, -95, 266, 2, 0);
                    break;
                case (SpriteType.sprmnuTITLEOPTIONS):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLEOPTIONS)]);
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    s.bAttrib[((int)battrMenuStartButtonAttributes.attrDoNotActivate)] = false;
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiMenuOptionsButton, -640, 0, -77, 347, 2, 0);
                    break;

                case (SpriteType.sprmnuTITLEEXIT):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTITLEEXIT)]);
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    s.nAttrib[((int)nattrExitAndCredits.attrCreditsScreen)] = 0;
                    s.bAttrib[((int)battrMenuStartButtonAttributes.attrDoNotActivate)] = false;
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiMenuExitButton, 45, 480 + 27, 109, 31, 0, 2);
                    break;

                case (SpriteType.sprmnuBTNTOGGLE0):
                case (SpriteType.sprmnuBTNTOGGLE1):
                case (SpriteType.sprmnuBTNTOGGLE2):
                case (SpriteType.sprmnuBTNTOGGLE3):
                case (SpriteType.sprmnuBTNTOGGLE4):
                    s.SetBehavior(AIMethods.aiToggleButton);
                    AIMethods.aiInitToggleButton(s, spriteType - (SpriteType.sprmnuBTNTOGGLE0));
                    break;
                case (SpriteType.sprmnuTXTSELECT):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTXTSELECT)]);
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiInanimate, 0, -200, 15, 15, 1, 1);
                    break;
                case (SpriteType.sprmnuJACKETBACK):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpJACKETBACK)]);
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiInanimate, 640, 0, 0, 0, 2, 0);
                    break;
                case (SpriteType.sprmnuMENUJACKET):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpMENUJACKET)]);
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiInanimate, -850, 0, 0, 0, 1, 1);
                    break;
                case (SpriteType.sprmnuMENUPASSCREST):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpMENUPASSCREST1)]);
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)] = false;
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiMenuGlowingPassCrest, -721, 128, 129, 128, 1, 1);
                    break;

                case (SpriteType.sprmnuAIPREVBARSCREEN):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpMENUPREVBARSCREEN)]);
                    s.SetBehavior(AIMethods.aiPrevBarScreen);
                    break;
                case (SpriteType.sprmnuAINEXTBARSCREEN):
                    s.SetFrame(AIMethods.frmM[((int)GameBitmapEnumeration.bmpMENUPREVBARSCREEN)]);
                    s.SetBehavior(AIMethods.aiNextBarScreen);
                    break;
                case (SpriteType.sprmnuAINEXTACHIEVEMENTSCREEN):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpMENUPREVBARSCREEN)]);
                    s.SetBehavior(AIMethods.aiNextAchievementScreen);
                    break;
                case (SpriteType.sprmnuOPTIONSRETURN):
                    s.bAttrib[((int)battrMenuStartButtonAttributes.attrMakeTitleSoundPlay)] = false;
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpBTNRETURN)]);
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiOptionsReturn, 640, 392, 10, 423, 1, 1);
                    break;

                case (SpriteType.sprmnuDECORATERETURN):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpBTNRETURN)]);
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiDecorateReturn, 640, 392, 10, 423, 1, 1);
                    break;

                case (SpriteType.sprmnuACHIEVEMENTTEXT):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
                    s.SpriteText = SpriteTextType.Small;
                    s.nR = 255;
                    s.nG = 255;
                    s.nB = 255;
                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiAchievementText, 640, 392, 10, 423, 1, 1);
                    s.Text = "";
                    break;
                case (SpriteType.sprmnuACHIEVEMENTADDITIONALTEXT):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
                    s.SpriteText = SpriteTextType.Small;
                    s.nR = 255;
                    s.nG = 255;
                    s.nB = 255;
                    s.SetBehavior(AIMethods.aiShowCurrentAchievementScreen);
//                    AIMethods.aiInitFlyInAndOut(s, AIMethods.aiAchievementText, 640, 392, 10, 423, 1, 1);
                    s.Text = "";
                    break;
                case (SpriteType.sprmnuBAR1):
                case (SpriteType.sprmnuBAR2):
                case (SpriteType.sprmnuBAR3):
                case (SpriteType.sprmnuBAR4):
                case (SpriteType.sprmnuBAR5):
                case (SpriteType.sprmnuBAR6):
                case (SpriteType.sprmnuBAR7):
                case (SpriteType.sprmnuBAR8):
                case (SpriteType.sprmnuBAR9):
                case (SpriteType.sprmnuBAR10):
                case (SpriteType.sprmnuBAR11):
                case (SpriteType.sprmnuBAR12):
                case (SpriteType.sprmnuBAR13):
                case (SpriteType.sprmnuBAR14):
                case (SpriteType.sprmnuBAR15):
                case (SpriteType.sprmnuBAR16):
                case (SpriteType.sprmnuBAR17):
                case (SpriteType.sprmnuBAR18):
                case (SpriteType.sprmnuBAR19):
                case (SpriteType.sprmnuBAR20):
                    s.SetBehavior(AIMethods.aiBar);
                    AIMethods.aiInitBar(s, spriteType - (SpriteType.sprmnuBAR1));
                    break;
            }
        }
        ///////////////////////////////////////////////////////////////
        // GAME ///////////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////

        if (spriteType > (SpriteType.sprgame_START) && spriteType < (SpriteType.sprgame_END))
        {
            switch (spriteType)
            {
                case (SpriteType.sprRIPPLE):
                    if (!AIMethods.sSound[((int)ASSList.ssndWATER_RIPPLE)].IsPlaying())
                        AIMethods.sSound[((int)ASSList.ssndWATER_RIPPLE)].Play(SoundbankInfo.vol4, AIMethods.panONX(s));
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpRIPPLE1)]);
                    s.SetBehavior(AIMethods.aiRipple);
                    break;
                case (SpriteType.sprSPLASHL):
                    if (!AIMethods.sSound[((int)ASSList.ssndWATER_SPLASHBIG)].IsPlaying())
                        AIMethods.sSound[((int)ASSList.ssndWATER_SPLASHBIG)].Play(SoundbankInfo.vol7, AIMethods.panONX(s));
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpSPLASHL1)]);
                    s.SetBehavior(AIMethods.aiSplashL);
                    break;
                case (SpriteType.sprSPLASHM):
                    if (!AIMethods.sSound[((int)ASSList.ssndWATER_SPLASHMID)].IsPlaying())
                        AIMethods.sSound[((int)ASSList.ssndWATER_SPLASHMID)].Play(SoundbankInfo.vol6, AIMethods.panONX(s));
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpSPLASHM1)]);
                    s.SetBehavior(AIMethods.aiSplashM);
                    break;
                case (SpriteType.sprSPLASHML):
                    if (!AIMethods.sSound[((int)ASSList.ssndWATER_SPLASHMID)].IsPlaying())
                        AIMethods.sSound[((int)ASSList.ssndWATER_SPLASHMID)].Play(SoundbankInfo.vol6, AIMethods.panONX(s));
                    s.SetFrame(AIMethods.frmM[((int)GameBitmapEnumeration.bmpSPLASHM1)]);
                    s.SetBehavior(AIMethods.aiSplashML);
                    break;
                case (SpriteType.sprSPLASHS):
                    if (!AIMethods.sSound[((int)ASSList.ssndWATER_SPLASHSMALL)].IsPlaying())
                        AIMethods.sSound[((int)ASSList.ssndWATER_SPLASHSMALL)].Play(SoundbankInfo.vol5, AIMethods.panONX(s));
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpSPLASHS1)]);
                    s.SetBehavior(AIMethods.aiSplashS);
                    break;
                case (SpriteType.sprCONSOLE):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpCONSOLE)]);
                    s.SetBehavior(AIMethods.aiConsole);
                    break;
                case (SpriteType.sprGRID):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpGRID1)]);
                    s.SetBehavior(AIMethods.aiGrid);
                    break;
                case (SpriteType.sprPOWERMETER):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpPOWERBAR)]);
                    s.SetBehavior(AIMethods.aiPowerMeter);
                    break;
                case (SpriteType.sprWATERMETER):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpWATERBAR)]);
                    s.SetBehavior(AIMethods.aiWaterMeter);
                    break;
                case (SpriteType.sprRINGMETER):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpRINGBAR)]);
                    s.SetBehavior(AIMethods.aiRingMeter);
                    break;
                case (SpriteType.sprCLOSEUPBEER):
                    AIMethods.sSound[((int)ASSList.ssndEFFECTS_CHUG)].Play(SoundbankInfo.volHOLLAR, AIMethods.panONX(s));
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpCLOSEUPBEER1)]);
                    s.SetBehavior(AIMethods.aiCloseUpBeer);
                    break;
                case (SpriteType.sprAPPLE):
                    AIMethods.sSound[((int)ASSList.ssndEFFECTS_WHOOSH1) + R.Next(SoundbankInfo.nsndEFFECTS_WHOOSH)].Play(SoundbankInfo.volNORMAL, AIMethods.panONX(s));
                    s.nAttrib[((int)attrProjectile.attrProjectileType)] = ((int)projTypes.projApple);
                    s.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingTowardTarget);
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpAPPLE1_1) + R.Next(AIMethods.nsprAPPLE1)]);
                    s.SetBehavior(AIMethods.aiProjectile);
                    break;
                case (SpriteType.sprPIZZA):
                    AIMethods.sSound[((int)ASSList.ssndEFFECTS_TOSSPIZZA)].Play(SoundbankInfo.volNORMAL, AIMethods.panONX(s));
                    s.nAttrib[((int)attrProjectile.attrProjectileType)] = ((int)projTypes.projPizza);
                    s.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingTowardTarget);
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpPIZZA1_1) + R.Next(AIMethods.nsprPIZZA1)]);
                    s.SetBehavior(AIMethods.aiProjectile);
                    break;
                case (SpriteType.sprCLARK):
                    AIMethods.sSound[((int)ASSList.ssndEFFECTS_TOSSBEER)].Play(SoundbankInfo.volNORMAL, AIMethods.panONX(s));
                    s.nAttrib[((int)attrProjectile.attrProjectileType)] = ((int)projTypes.projClark);
                    s.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingTowardTarget);
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpCLARK2_1) + R.Next(AIMethods.nsprCLARK2)]);
                    s.SetBehavior(AIMethods.aiProjectile);
                    break;
                case (SpriteType.sprEXAM):
                    s.nAttrib[((int)attrProjectile.attrProjectileType)] = ((int)projTypes.projExam);
                    s.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingTowardTarget);
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpEXAM1_1) + R.Next(AIMethods.nsprEXAM1)]);
                    s.SetBehavior(AIMethods.aiProjectile);
                    break;
                case (SpriteType.sprGREASE):
                    s.nAttrib[((int)attrProjectile.attrProjectileType)] = ((int)projTypes.projGrease);
                    s.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingTowardTarget);
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpSPRAY1) + R.Next(2)]);
                    s.SetBehavior(AIMethods.aiProjectile);
                    break;
                case (SpriteType.sprPOPUP_APPLES):
                    // relative speed [2..5], entry [1..3], onscreen [1..?], exit [1..3],
                    // sound [bitename], hit_behavior [aifunction]
                    AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpOFFERAPPLES)],
                        6, 1, 1, 1, ((int)ASLList.lsndAPPLES_OFFER1) + R.Next(Globals.myGameConditions.IsRitual() ? 12 : 10),
                        AIMethods.aiGetApples);
                    break;
                case (SpriteType.sprPOPUP_PIZZA):
                    //		case (SpriteType.sprPOPUP_BEER): 
                    AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpOFFERPIZZA)],
                        6, 1, 1, 1, ((int)ASLList.lsndPIZZA_OFFER1) + R.Next((Globals.myGameConditions.IsRitual() ? 4 : 3)),
                        AIMethods.aiGetPizza);
                    break;
                case (SpriteType.sprPOPUP_BEER):
                    AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpOFFERCLARK)],
                        6, 1, 1, 1, ((int)ASLList.lsndCLARK_OFFER1) + R.Next((Globals.myGameConditions.IsRitual() ? 10 : 6)),
                        AIMethods.aiGetClark);
                    break;
                case (SpriteType.sprPOPUP_EXAM):
                    AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpOFFEREXAM)],
                        6, 1, 1, 1, ((int)ASLList.lsndEXAM_OFFER1) + R.Next(5),
                        AIMethods.aiGetExam);
                    break;
                case (SpriteType.sprPOPUP_HOSE):
                    AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpOFFERHOSE)],
                        6, 1, 1, 1, ((int)ASLList.lsndHOSE_OFFER1) + R.Next(Globals.myGameConditions.IsRitual() ? 4 : 3),
                        AIMethods.aiGetHose);
                    break;
                case (SpriteType.sprPOPUP_ARTSCIM):
                    if (Globals.myGameConditions.IsRitual() && (0 == R.Next(4)))
                        AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpARTSCIM_POPUP5)],
                            4, 1, 1, 1, ((int)ASLList.lsndARTSCI_MALE_TAUNTR1), AIMethods.aiPushArtSciMIntoPit);
                    else
                        AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpARTSCIM_POPUP1) + R.Next(AIMethods.nsprARTSCIM_POPUP - 1)],
                            4, 1, 1, 1, ((int)ASLList.lsndARTSCI_MALE_TAUNT1) + R.Next(SoundbankInfo.nsndARTSCI_MALE_TAUNT), AIMethods.aiPushArtSciMIntoPit);
                    break;
                case (SpriteType.sprPOPUP_ARTSCIF):
                    AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpARTSCIF_POPUP1) + R.Next(AIMethods.nsprARTSCIF_POPUP)],
                        4, 1, 1, 1, ((int)ASLList.lsndARTSCI_FEMALE_TAUNT1) + R.Next(SoundbankInfo.nsndARTSCI_FEMALE_TAUNT), AIMethods.aiPushArtSciFIntoPit);
                    break;
                case (SpriteType.sprPOPUP_COMMIEM):
                    if (0 != R.Next(2))
                        AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpCOMMIEM_POPUP1) + (AIMethods.nsprCOMMIEM_POPUP - 1)],
                            4, 1, 1, 1, ((int)ASLList.lsndCOMMIE_MALE_PHONE1) + R.Next(2), AIMethods.aiPushCommieMIntoPit);
                    else if (Globals.myGameConditions.IsRitual() && (0 == R.Next(2)))
                        AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpCOMMIEM_POPUP1) + R.Next(AIMethods.nsprCOMMIEM_POPUP - 1)],
                            4, 1, 1, 1, ((int)ASLList.lsndCOMMIE_MALE_TAUNTR1) + R.Next(3), AIMethods.aiPushCommieMIntoPit);
                    else
                        AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpCOMMIEM_POPUP1) + R.Next(AIMethods.nsprCOMMIEM_POPUP - 1)],
                            4, 1, 1, 1, ((int)ASLList.lsndCOMMIE_MALE_TAUNT1) + R.Next(SoundbankInfo.nsndCOMMIE_MALE_TAUNT), AIMethods.aiPushCommieMIntoPit);
                    break;
                case (SpriteType.sprPOPUP_COMMIEF):
                    AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpCOMMIEF_POPUP1) + R.Next(AIMethods.nsprCOMMIEF_POPUP)],
                        4, 1, 1, 1, ((int)ASLList.lsndCOMMIE_FEMALE_TAUNT1) + R.Next(4), AIMethods.aiPushCommieFIntoPit);
                    break;
                case (SpriteType.sprSCICONM):
                    if (0 != R.Next(2))
                        AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpSCICONM1)],
                            6, 2, 2, 2, ((int)ASLList.lsndSCICONM_PopUp1) + R.Next(3), AIMethods.aiPushSciConM, true);
                    else
                        AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpSCICONM1)],
                            6, 3, 3, 3, ((int)ASLList.lsndSCICONM_PopUp1) + R.Next(3), AIMethods.aiPushSciConM, true);
                    s.bAttrib[((int)battrForeGroundPopUpDudes.attrMale)] = true;
                    break;
                case (SpriteType.sprSCICONF):
                    if (0 != R.Next(2))
                        AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpSCICONF1)],
                            6, 2, 2, 2, ((int)ASLList.lsndSCICONF_PopUp1) + R.Next(4), AIMethods.aiPushSciConF, true);
                    else
                        AIMethods.aiInitPopUp(s, AIMethods.frm[((int)GameBitmapEnumeration.bmpSCICONF1)],
                            6, 3, 3, 3, ((int)ASLList.lsndSCICONF_PopUp1) + R.Next(4), AIMethods.aiPushSciConF, true);
                    s.bAttrib[((int)battrForeGroundPopUpDudes.attrMale)] = false;
                    break;
                case (SpriteType.sprPOPUP_ENGINEER):
                    break;
                case (SpriteType.sprCLOUDS):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpCLOUDS)]);
                    s.SetBehavior(AIMethods.aiInanimate);
                    break;
                case (SpriteType.sprTREES):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTREES)]);
                    s.SetBehavior(AIMethods.aiInanimate);
                    break;
                case (SpriteType.sprBACKDROP):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpBACKDROP)]);
                    s.SetBehavior(AIMethods.aiInanimate);
                    break;
                case (SpriteType.sprFRECGROUP):
                    AIMethods.aiFrecGroupInit(s);
                    break;
                case (SpriteType.sprFRECACTION):
                    AIMethods.aiFrecActionInit(s);
                    break;
                case (SpriteType.sprUPPERYEAR):
                    AIMethods.aiUpperYearInit(s);
                    break;
                case (SpriteType.sprPOOF):
                    AIMethods.aiPoofInit(s);
                    break;
                case (SpriteType.sprGWBALLOON):
                    AIMethods.aiGWBalloonInit(s);
                    break;
                case (SpriteType.sprGWHIPPO):
                    AIMethods.aiGWHippoInit(s);
                    break;
                case (SpriteType.sprPREZ):
                    AIMethods.aiPrezInit(s);
                    break;
                case (SpriteType.sprFORGE):
                    s.nAttrib[((int)nattrForge.attrForgeMotion)] = 0;
                    s.nAttrib[((int)nattrForge.attrForgeSwing)] = 0;
                    s.nAttrib[((int)nattrForge.attrForgeEnergy)] = 0;
                    AIMethods.aiForgeInit(s);
                    break;
                case (SpriteType.sprPODIUM):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpPODIUM)]);
                    s.SetBehavior(AIMethods.aiRandomEventGenerator);
                    break;
                case (SpriteType.sprINANIMATE):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
                    s.SetBehavior(AIMethods.aiInanimate);
                    break;
                case (SpriteType.sprPOLE):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpPOLE)]);
                    s.SetBehavior(AIMethods.aiInanimate);
                    break;
                case (SpriteType.sprWATER):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpWATER)]);
                    s.SetBehavior(AIMethods.aiInanimate);
                    break;
                case (SpriteType.sprFROSH):
                    // Set the Behavior to diving into the pit
                    s.nZ = 30;

                    // Goal: Mindless wandering somewhere in the pit
                    s.SetGoal(((int)Goals.goalMINDLESS_WANDERING));
                    s.ppChosen = null;
                    s.nDestX = AIMethods.randintin(AIMethods.dPITMINX, AIMethods.dPITMAXX);
                    s.nDestY = AIMethods.randintin(AIMethods.dPITMINY, AIMethods.dPITMAXY);
                    s.nDestZ = 0;
                    s.nvX = 0; s.nvY = 0; s.nvZ = 0;
                    // Assign default Attribute values.
                    s.nAttrib[((int)nattrFrosh.attrMindSet)] = ((int)mindsets.mindsetGullible); // They'll fall for anything
                    s.nAttrib[((int)nattrFrosh.attrMotivation)] = ((int)motivationlevels.motivationHigh);
                    s.nAttrib[((int)nattrFrosh.attrStr)] = AIMethods.gnDefaultStrength + AIMethods.randintin(-3, 3);
                    s.bAttrib[((int)battrFrosh.attrExcited)] = false;
                    s.bAttrib[((int)battrFrosh.attrThirsty)] = true;
                    s.bAttrib[((int)battrFrosh.attrHungry)] = true;
                    s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 0;
                    s.SetBehavior(AIMethods.aiInit2);

                    break;
                case (SpriteType.sprMOUSECURSORBL):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpMOU_TARBL)]);
                    s.SetBehavior(AIMethods.aiMouseCursorBL);
                    break;
                case (SpriteType.sprMOUSECURSORBR):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpMOU_TARBR)]);
                    s.SetBehavior(AIMethods.aiMouseCursorBR);
                    break;
                case (SpriteType.sprMOUSECURSORTL):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpMOU_TARTL)]);
                    s.SetBehavior(AIMethods.aiMouseCursorTL);
                    break;
                case (SpriteType.sprMOUSECURSORTR):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpMOU_TARTR)]);
                    s.SetBehavior(AIMethods.aiMouseCursorTR);
                    break;
                case (SpriteType.sprARM):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpHAND0_1)]);
                    s.SetBehavior(AIMethods.aiArm);
                    s.nAttrib[((int)attrArm.attrArmStatus)] = 0;
                    s.nAttrib[((int)attrArm.attrArmAction)] = 0;
                    break;
                case (SpriteType.sprARMRING1):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpHAND7_3)]);
                    s.SetBehavior(AIMethods.aiArmRing1);
                    break;
                case (SpriteType.sprARMRING2):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpHAND7_2)]);
                    s.SetBehavior(AIMethods.aiArmRing2);
                    break;
                case (SpriteType.sprARMRING3):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpHAND7_1)]);
                    s.SetBehavior(AIMethods.aiArmRing3);
                    break;
                case (SpriteType.sprAPPLEICON):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpICOAPPLE1)]);
                    s.SetBehavior(AIMethods.aiIcon); s.nvY = 5;
                    s.nDestX = s.nX; s.nDestY = s.nY;
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonAPPLE);
                    s.nAttrib[((int)attrIcon.attrIconStatus)] = 0;
                    break;
                case (SpriteType.sprPIZZAICON):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpICOPIZZA1)]);
                    s.SetBehavior(AIMethods.aiIcon); s.nvY = 5;
                    s.nDestX = s.nX; s.nDestY = s.nY;
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonPIZZA);
                    s.nAttrib[((int)attrIcon.attrIconStatus)] = 0;
                    break;
                case (SpriteType.sprCLARKICON):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpICOCLARK1)]);
                    s.SetBehavior(AIMethods.aiIcon); s.nvY = 5;
                    s.nDestX = s.nX; s.nDestY = s.nY;
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonCLARK);
                    s.nAttrib[((int)attrIcon.attrIconStatus)] = 0;
                    break;
                case (SpriteType.sprEXAMICON):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpICOEXAM1)]);
                    s.SetBehavior(AIMethods.aiIcon); s.nvY = 5;
                    s.nDestX = s.nX; s.nDestY = s.nY;
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonEXAM);
                    s.nAttrib[((int)attrIcon.attrIconStatus)] = 0;
                    break;
                case (SpriteType.sprRINGICON):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpICORING1)]);
                    s.nvY = 5; s.nvX = 5;
                    s.nDestX = s.nX; s.nDestY = s.nY;
                    s.nX = -40;
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonRING);
                    s.nAttrib[((int)attrIcon.attrIconStatus)] = 0;
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut2(s, AIMethods.aiRing, -40, s.nY, s.nDestX, s.nY, 1, 1);

                    AIMethods.lSound[((int)ASLList.lsndRING_DING)].Play(SoundbankInfo.volHOLLAR, 0);

                    break;
                case (SpriteType.sprAPPLE_ONES):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]);
                    s.SetBehavior(AIMethods.aiPointsOnes);
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonAPPLE);
                    break;
                case (SpriteType.sprAPPLE_TENS):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]);
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonAPPLE);
                    s.SetBehavior(AIMethods.aiPointsTens);
                    break;
                case (SpriteType.sprCLARK_ONES):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]);
                    s.SetBehavior(AIMethods.aiPointsOnes);
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonCLARK);
                    break;
                case (SpriteType.sprCLARK_TENS):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]);
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonCLARK);
                    s.SetBehavior(AIMethods.aiPointsTens);
                    break;
                case (SpriteType.sprEXAM_ONES):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]);
                    s.SetBehavior(AIMethods.aiPointsOnes);
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonEXAM);
                    break;
                case (SpriteType.sprEXAM_TENS):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]);
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonEXAM);
                    s.SetBehavior(AIMethods.aiPointsTens);
                    break;
                case (SpriteType.sprPIZZA_ONES):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]);
                    s.SetBehavior(AIMethods.aiPointsOnes);
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonPIZZA);
                    break;
                case (SpriteType.sprPIZZA_TENS):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpDIG_0)]);
                    s.nAttrib[((int)attrIcon.attrButtonType)] = ((int)Buttons.buttonPIZZA);
                    s.SetBehavior(AIMethods.aiPointsTens);
                    break;

                case (SpriteType.sprPITTIME_HONES):
                    s.SetBehavior(AIMethods.aiPitTimeHOnes); break;
                case (SpriteType.sprPITTIME_HTENS):
                    s.SetBehavior(AIMethods.aiPitTimeHTens); break;
                case (SpriteType.sprPITTIME_MONES):
                    s.SetBehavior(AIMethods.aiPitTimeMOnes); break;
                case (SpriteType.sprPITTIME_MTENS):
                    s.SetBehavior(AIMethods.aiPitTimeMTens); break;
                case (SpriteType.sprPITTIME_SONES):
                    s.SetBehavior(AIMethods.aiPitTimeSOnes); break;
                case (SpriteType.sprPITTIME_STENS):
                    s.SetBehavior(AIMethods.aiPitTimeSTens); break;
                case (SpriteType.sprTAM):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTAM0_1)]);
                    s.SetBehavior(AIMethods.aiTam);
                    s.nAttrib[((int)attrTam.attrNailsInTam)] = (0 != Globals.myGameConditions.GetFroshLameness()) ? 10 : 20;
                    s.nAttrib[((int)attrTam.attrTamStatus)] = 0;
                    break;
                case (SpriteType.sprTRACKERLR):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpWHITE_DOT)]);
                    s.SetBehavior(AIMethods.aiTrackerLR);
                    break;
                case (SpriteType.sprTRACKERUL):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpWHITE_DOT)]);
                    s.SetBehavior(AIMethods.aiTrackerUL);
                    break;
                case (SpriteType.sprWHAP):
                    AIMethods.sSound[((int)ASSList.ssndEFFECTS_SMACK1) + R.Next(SoundbankInfo.nsndEFFECTS_SMACK)].Play(SoundbankInfo.volNORMAL, AIMethods.panONX(s));
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpWHAP_1) + R.Next(AIMethods.nsprWHAP)]);
                    s.SetBehavior(AIMethods.aiWhap);
                    break;
                case (SpriteType.sprHOSEWHAP):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpWHAP_1) + R.Next(AIMethods.nsprWHAP)]);
                    s.SetBehavior(AIMethods.aiWhap);
                    break;
                case (SpriteType.sprGWWORDBUBBLE):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpHIPPOWORDS1) + R.Next(6)]);
                    s.SetBehavior(AIMethods.aiWordBubble);
                    break;
                case (SpriteType.sprHIGHSCORE):
                    s.SetBehavior(AIMethods.aiHighScore);
                    break;
                case (SpriteType.sprPOPBOY):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpPOPBOY1_1)]);
                    s.SetBehavior(AIMethods.aiPopBoy);
                    s.nAttrib[((int)nattrFrosh.attrBehavior)] = 1;
                    s.nAttrib[((int)nattrFrosh.attrMotivation)] = 0;
                    s.nAttrib[((int)nattrFrosh.attrMindSet)] = -1;
                    s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] = -1;
                    break;
                case (SpriteType.sprPOPBOYINCROWD):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpPOPBOY1_2)]);
                    s.SetBehavior(AIMethods.aiPopBoyInCrowd);
                    break;

                case (SpriteType.sprTRI):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpTRI)]);
                    s.nvY = 5; s.nvX = 5;
                    s.nDestX = 100; s.nDestY = 165;
                    s.nX = -160; s.nY = s.nDestY;
                    AIMethods.sSound[((int)ASSList.ssndEFFECTS_ICONOUT)].Play(SoundbankInfo.volHOLLAR, AIMethods.panONX(s));
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut2(s, AIMethods.aiInanimate, s.nX, s.nY, s.nDestX, s.nY, 1, 1);
                    break;
                case (SpriteType.sprPUB):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpPUB)]);
                    s.nvY = 5; s.nvX = 5;
                    s.nDestX = 320 - 15; s.nDestY = 165;
                    s.nX = s.nDestX; s.nY = -250;
                    AIMethods.sSound[((int)ASSList.ssndEFFECTS_ICONOUT)].Play(SoundbankInfo.volHOLLAR, AIMethods.panONX(s));
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut2(s, AIMethods.aiInanimate, s.nX, s.nY, s.nX, s.nDestY, 1, 1);
                    break;
                case (SpriteType.sprBAN):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpBAN)]);
                    s.nvY = 5; s.nvX = 5;
                    s.nDestX = 480 + 60 - 15; s.nDestY = 165;
                    s.nX = 800; s.nY = s.nDestY;
                    AIMethods.sSound[((int)ASSList.ssndEFFECTS_ICONOUT)].Play(SoundbankInfo.volHOLLAR, AIMethods.panONX(s));
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut2(s, AIMethods.aiInanimate, s.nX, s.nY, s.nDestX, s.nY, 1, 1);
                    break;
                case (SpriteType.sprACHIEVEMENTUNLOCKED):
                case (SpriteType.sprACHIEVEMENTUNLOCKEDTEXT1):
                case (SpriteType.sprACHIEVEMENTUNLOCKEDTEXT2):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpACHIEVEMENTUNLOCKED)]);
                    s.nR = s.nG = s.nB = 255;
                    s.nvY = 1; s.nvX = 1;
                    s.nDestX = 150; s.nDestY = 480 - 90;
//                    s.nX = s.nDestX; s.nY = -200;
                    s.nX = -200; s.nY = s.nDestY;
                    if (spriteType != SpriteType.sprACHIEVEMENTUNLOCKED) { s.nDestY += 13; s.nY += 13; s.nX += 60; s.nDestX += 60; }
                    if (spriteType == SpriteType.sprACHIEVEMENTUNLOCKEDTEXT2) { s.nDestY += 22; s.nY += 22; s.nX += 10; s.nDestX += 10; }
                    if (spriteType == SpriteType.sprACHIEVEMENTUNLOCKED) 
                        AIMethods.sSound[((int)ASSList.ssndEFFECTS_ACHIEVEMENTUNLOCKED)].Play(SoundbankInfo.volHOLLAR, AIMethods.panONX(s));
                    s.SetBehavior(AIMethods.aiFlyInAndOut);
                    // s, StartXandY, GoingToXandY, FlipToXandY, VelocityXandY
                    AIMethods.aiInitFlyInAndOut2(s, AIMethods.aiAchievementUnlockedNotice, s.nX, s.nY - 400, s.nDestX, s.nDestY, 1, 1);
                    break;
            }
        }
        ///////////////////////////////////////////////////////////////
        // TRANSITION /////////////////////////////////////////////////
        ///////////////////////////////////////////////////////////////
        if (spriteType > (SpriteType.sprtrans_START) && spriteType < (SpriteType.sprtrans_END))
        {
            switch (spriteType)
            {
                case (SpriteType.sprSLAMJACKET):
                    s.SetFrame(AIMethods.frm[((int)GameBitmapEnumeration.bmpSLAMJACKET1)]);
                    s.SetBehavior(AIMethods.aiSlamJacket);
                    break;
            }
        }
    }

}