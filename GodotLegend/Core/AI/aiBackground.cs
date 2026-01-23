using System;

public static partial class AIMethods
{

    public static void aiFrecGroupInit(TSprite s)
    {
        s.nAttrib[((int)nattrCrowd.attrFAction)] = ((int)attrCrowdActions.faMilling);
        s.nAttrib[((int)nattrCrowd.attrFEnergy)] = energyStart;
        s.nAttrib[((int)nattrCrowd.attrFCanStartWave)] = (s.nX > 320) ? 1 : 0;
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Milling1)]);

        // Change for silverlight version - make this exciting even if the graphics are not enhanced.
        s.SetBehavior(aiFrecGroup);
        
        // There should only be one Frec Group or FrecBoring object.
        //if (0 != Globals.myGameConditions.GetEnhancedGraphics())
        //    s.SetBehavior(aiFrecGroup);
        //else
        //    s.SetBehavior(aiFrecBoring);
    }

    public static void aiFrecActionInit(TSprite s)
    {
        s.nAttrib[((int)nattrCrowd.attrFAction)] = ((int)attrCrowdActions.faMilling);
        s.nAttrib[((int)nattrCrowd.attrFEnergy)] = energyStart;
        s.nAttrib[((int)nattrCrowd.attrFCanStartWave)] = 0;
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Milling1)]);
        if (0 != Globals.myGameConditions.GetEnhancedGraphics())
            s.SetBehavior(aiFrecAction);
        else
            s.SetBehavior(aiInanimate);

    }

    public static void aiSciConMInit(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSCICONM1)]);
        s.SetBehavior(aiSciConM);
    }
    public static void aiSciConM(TSprite s) { }

    public static void aiSciConFInit(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSCICONF1)]);
        s.SetBehavior(aiSciConF);
    }
    public static void aiSciConF(TSprite s) { }


    public static void aiUpperYearInit(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOPBOY1_1)]);
        s.SetBehavior(aiUpperYear);
    }
    public static void aiUpperYear(TSprite s) { }

    public static void aiPoofInit(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOOF1)]);
        s.SetBehavior(aiPoof);
    }
    public static void aiPoof(TSprite s)
    {
        if (s.nCC == 3)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOOF2)]);
        if (s.nCC == 5)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOOF3)]);
        if (s.nCC == 7)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOOF4)]);
        if (s.nCC == 9)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPOOF1)]);
        if (s.nCC == 10)
            s.bDeleted = true;
    }


    public const int BALLOONWIDTH = 100;
    public static void aiGWBalloonInit(TSprite s)
    {

        // Pop Boy announces the balloon's arrival (with varying levels of couthness)
        if (Globals.myGameConditions.IsPopBoyInPit())
        {
            if (Globals.myGameConditions.IsRitual())
                lSound[((int)ASLList.lsndPOPBOY_HIPPOR1)].Play(SoundbankInfo.volHOLLAR);
            else
                lSound[((int)ASLList.lsndPOPBOY_HIPPO1)].Play(SoundbankInfo.volHOLLAR);
        }

        if (0 != R.Next(2))
        {
            aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faLookULR)); sprFrecsL.nCC -= 120;
            aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faLookULR)); sprFrecsC.nCC -= 100;
            aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faLookULR));
            s.nAttrib[0] = 0;
            s.nX = 640 + BALLOONWIDTH;
            s.nY = 0;
            s.nZ = 25;
            s.nvX = -2;
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGWBALLOONL)]);
        }
        else
        {
            aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faLookURL));
            aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faLookURL)); sprFrecsC.nCC -= 100;
            aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faLookURL)); sprFrecsR.nCC -= 120;
            s.nAttrib[0] = 1;
            s.nX = 0 - BALLOONWIDTH;
            s.nY = 0;
            s.nZ = 25;
            s.nvX = 2;
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGWBALLOONR)]);
        }
        s.SetBehavior(aiGWBalloon);
    }

    public const int dBalloonOffset = 20;
    public static readonly int[] dBalloonOffsetShifts = {0, 5, 8, 10, 11, 11, 11, 10, 8, 5, 
0, -5, -8, -10, -11, -11, -11, -10, -8, -5};
    public const int dNUMBALLOONOFFSETSHIFTS = 20;


    public static void aiGWBalloon(TSprite s)
    {
        if (s.nCC > 100 && (s.nX < 0 - BALLOONWIDTH || s.nX > 640 + BALLOONWIDTH))
        {
            s.bDeleted = true;
            sprGWBalloon = null;
        }
        if (s.nCC == 125 ) //  && 0 != R.Next(2)) MORE HIPPO!!
        {
            sSound[((int)ASSList.ssndEFFECTS_KABOOM)].Play(SoundbankInfo.volHOLLAR);
            if (0 != s.nAttrib[0])
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGWBALLOONRBANG)]);
            else
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGWBALLOONLBANG)]);
            if (sprGWHippo == null)
            {
                Globals.Analytic("HippoLaunched");
                sprGWHippo = SpriteInit.CreateSprite((SpriteType.sprGWHIPPO), s.nX, dPOLEY + 50);
                sprGWHippo.nZ = s.nZ - (dSKYYTOPITY()) + 30 + dPOLEY + 50;
                ssPit.Include(sprGWHippo);
            }
        }
        if (s.nCC == 150)
        {
            if (0 != s.nAttrib[0])
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGWBALLOONR)]);
            else
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGWBALLOONL)]);
        }

        //s.nY += (dBalloonOffsetShifts[((s.nCC / 3)% dNUMBALLOONOFFSETSHIFTS)] / 3);
        s.nX += s.nvX;
    }

    public static void aiGWHippoInit(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHIPPO0_1)]);
        s.SetBehavior(aiGWHippo);
        s.nAttrib[0] = 0;
        s.nvX = 0;
        s.nvY = 0;
        s.nvZ = 20;

    }
    public static void aiGWHippo(TSprite s)
    {
        switch (s.nAttrib[0])
        {
            case 0: // in air
                if (s.nZ < 0)
                {
                    ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), s.nX, s.nY));
                    s.nZ = 0; s.nvX = 16; s.nvY = 4; s.nDestX = randintin(dPITMINX, dPITMAXX - 80);
                    s.nDestY = randintin(dPOLEY, dPITMAXY - 30);
                    s.nAttrib[0] = 1;
                    s.nCC = 0;
                }
                else
                {
                    aisPlummet(s);
                    if (s.nCC == 7) s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHIPPO0_2)]);
                    if (s.nCC >= 10 && 0 == (s.nCC % 3))
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHIPPO1_1) + R.Next(3)]);
                }
                break;
            case 1:  // Wade towards a target in the pit
                if (!((Math.Abs(s.nX - s.nDestX) <= s.nvX) && (Math.Abs(s.nY - s.nDestY) <= s.nvY)))
                {
                    if (s.nCC < 20)
                        s.SetFrame((s.nDestX > s.nX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO2_1)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO2_1)]);
                    else if (s.nCC < 50)
                        s.SetFrame((s.nDestX > s.nX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO2_2)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO2_2)]);
                    else
                        s.SetFrame((s.nDestX > s.nX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO2_3)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO2_3)]);
                    s.nAttrib[((int)attrProjectile.attrPowerOfThrow)] = 100;
                    aisCollisionToResponse(s, ssFr, aisSendFroshReallyFlying,
                        NOWHAP, NOPOLESHIELDING, false);
                    if (0 == (s.nCC % 4))
                        aisMoveTowardsDestination(s);
                }
                else
                {
                    if (s.nDestX == dPOLEX && s.nDestY == dPOLEY)
                    {
                        s.nAttrib[0] = 2;
                    }
                    else
                    {
                        if (0 == R.Next(4) || s.nCC > 250)
                        {
                            s.nDestX = dPOLEX; s.nDestY = dPOLEY;
                        }
                        else
                        {
                            s.nCC = 0;
                            s.nAttrib[0] = 11;
                            ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprGWWORDBUBBLE),
                            s.nScrx + s.frmFrame.nHotSpotX + 20,
                            s.nScry + s.frmFrame.nHotSpotY - 60));

                            s.nDestX = randintin(dPITMINX + 30, dPITMAXX - 80);
                            s.nDestY = randintin(dPOLEY, dPITMAXY - 30);
                        }
                    }
                }
                break;
            case 11: // WORD BUBBLE DELAY
                if (s.nCC > 40)
                {
                    s.nAttrib[0] = 1;
                    s.nCC = 400;
                }
                break;
            case 2: // CLIMB THE POLE
                if (0 == (s.nCC % 4)) s.nX = dPOLEX - 1 + R.Next(3);

                s.nY = dPOLEY + 1;
                s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO3_1)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO3_1)]);
                if (s.nZ < dTAMZ + 110)
                {
                    if (0 == (s.nCC % 5))
                        s.nZ += 30 + R.Next(10);
                }
                else
                {
                    s.nAttrib[0] = 3;
                    s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = false;
                    s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO3_2)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO3_2)]);
                    s.nZ = dTAMZ + 140;
                    s.nY = dPOLEY + 2;
                }

                break;
            case 3:  // Sitting on tam
                //s.bDeleted = true;
                if (s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)])
                {
                    s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = false;
                    s.nCC = 0;
                    s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO4_2)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO4_2)]);
                    s.nAttrib[0] = 31;
                    AIMethods.aisUnlockAchievement(96);
                    aisForgeTrick(0, 2500); // Hit Hippo with Stuff
                }
                else if (0 == (s.nCC % 10) && 0 == R.Next(5))
                {
                    if (0 == R.Next(6))
                    {
                        s.nAttrib[0] = 4; s.nCC = 0;
                        TSprite temp;
                        temp = SpriteInit.CreateSprite((SpriteType.sprGWWORDBUBBLE), s.nScrx + s.frmFrame.nHotSpotX + 60, s.nScry + 60);
                        temp.SetFrame(frm[((int)GameBitmapEnumeration.bmpHIPPOWORDS1)]);
                        if (s.nX < dPOLEX) s.nvX = 7; else s.nvX = -7;
                        s.nvY = 0; s.nvZ = 7;
                        ssIcons.Include(temp);
                    }
                    else
                    {
                        s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO3_2) + R.Next(2)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO3_2 )+ R.Next(2)]);
                    }
                }

                break;
            case 31:
                if (s.nCC > 5)
                {
                    if (0 == (s.nCC % 3))
                    {
                        switch ((s.nCC % 24) % 8)
                        {
                            case 0: s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO3_2)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO3_2)]); break;
                            case 1: s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO4_1)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO4_1)]); break;
                            case 2: s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO3_2)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO3_2)]); break;
                            case 3: s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO4_1)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO4_1)]); break;
                            case 4: s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO3_3)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO3_3)]); break;
                            case 5: s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO4_2)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO4_2)]); break;
                            case 6: s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO3_3)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO3_3)]); break;
                            case 7: s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO4_2)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO4_2)]); break;
                        }
                    }
                    if (s.nCC > 50)
                    {
                        s.nAttrib[0] = 3;
                        s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO3_3)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO3_3)]);
                    }

                }
                break;
            case 4:  // Jeronimo
                if (s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)])
                {
                    s.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = false;
                    s.nCC = 0;
                    s.SetFrame((s.nX < dPOLEX) ? frm[((int)GameBitmapEnumeration.bmpHIPPO4_2)] : frmM[((int)GameBitmapEnumeration.bmpHIPPO4_2)]);
                    s.nAttrib[0] = 31;
                }

                if (s.nCC > 30)
                {
                    aisPlummet(s);
                    if (s.nZ < 0)
                    {
                        s.bDeleted = true;
                        sprGWHippo = null;
                    }
                    if (s.nCC >= 10 && 0 == (s.nCC % 3))
                        s.SetFrame((s.nX < dPOLEX) ? (frm[((int)GameBitmapEnumeration.bmpHIPPO1_1) + R.Next(3)]) : (frmM[((int)GameBitmapEnumeration.bmpHIPPO1_1 )+ R.Next(3)]));

                }
                break;
        }
    }

    public const int PREZLEFT = 175;
    public const int PREZRIGHT = 240;

    public static void aiPrezInit(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ1_2)]);
        s.nAttrib[((int)attrPrez.attrPrezAction)] = 1;
        s.nvX = -2;
        s.SetBehavior(aiPrez);
    }
    public static void aiPrez(TSprite s)
    {
        switch (s.nAttrib[((int)attrPrez.attrPrezAction)])
        {
            case 1:  // Chillin'
                if (s.nCC > 15 && (0 == R.Next(100)))
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ1_1) + R.Next(3)]);
                if (s.nCC > 40 && (0 == R.Next(500)))
                {
                    s.nAttrib[((int)attrPrez.attrPrezAction)] = 3; s.nCC = 0;
                }
                if (s.nCC > 40 && (0 == R.Next(500)))
                {
                    s.nAttrib[((int)attrPrez.attrPrezAction)] = 2; s.nCC = 0; s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ2_1) + R.Next(3)]);
                }
                break;
            case 2:  // Motioning
                if (s.nCC > 15 && (0 == R.Next(50)))
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ2_1) + R.Next(3)]);
                if (s.nCC > 40 && (0 == R.Next(50)))
                {
                    s.nAttrib[((int)attrPrez.attrPrezAction)] = 1; s.nCC = 0; s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ1_1) + R.Next(3)]);
                }
                break;
            case 3:  // Walkin'
                s.nX += s.nvX;
                if (s.nCC == 1 || 0 == (s.nCC % 10))
                {
                    if (s.nvX < 0)
                        s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpPREZ3_1) + ((0 == (s.nCC % 20)) ? 1 : 0)]);
                    else
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ3_1) + ((0 == (s.nCC % 20)) ? 1 : 0)]);
                }

                if ((s.nvX < 0 && s.nX < PREZLEFT) || (s.nvX > 0 && s.nX > PREZRIGHT))
                {
                    s.nvX = -s.nvX;
                    if (s.nvX < 0)
                        s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpPREZ3_1) + ((0 == (s.nCC % 20)) ? 1 : 0)]);
                    else
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ3_1) + ((0 == (s.nCC % 20)) ? 1 : 0)]);

                }
                if ((s.nCC > 15 && (0 == R.Next(300))) || s.nCC > 80)
                {
                    s.nAttrib[((int)attrPrez.attrPrezAction)] = 1; s.nCC = 0; s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ1_1) + R.Next(3)]);
                }
                break;
            case 4:  // Speaking
                if (s.nCC < 10000)
                {
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ4_1) + R.Next(2)]);
                    s.nCC = 10000;
                }
                if (0 == (s.nCC % 24) && 0 != R.Next(4))
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ4_1) + R.Next(2)]);
                if (s.nCC > 10075)
                {
                    s.nCC = 0;
                    s.nAttrib[((int)attrPrez.attrPrezAction)] = 1; s.nCC = 0; s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ1_1) + R.Next(3)]);
                }
                break;
        }
    }

    public static void aiForgeInit(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
        s.nAttrib[((int)nattrForge.attrForgeSwing)] = 0;
        s.nAttrib[((int)nattrForge.attrForgeMotion)] = 0;
        s.SetBehavior(aiForge);
    }
    //[3][10] 
    public static readonly int[,] IRSequences =
{ 
    {((int)GameBitmapEnumeration.bmpFORGE1_1), ((int)GameBitmapEnumeration.bmpFORGE1_2), ((int)GameBitmapEnumeration.bmpFORGE1_3), ((int)GameBitmapEnumeration.bmpFORGE1_4), ((int)GameBitmapEnumeration.bmpFORGEDOWN), ((int)GameBitmapEnumeration.bmpFORGE2_5), ((int)GameBitmapEnumeration.bmpFORGE2_4), ((int)GameBitmapEnumeration.bmpFORGE2_3), ((int)GameBitmapEnumeration.bmpFORGE2_2), ((int)GameBitmapEnumeration.bmpFORGE1_1)}, 
    {((int)GameBitmapEnumeration.bmpFORGE1_1), ((int)GameBitmapEnumeration.bmpFORGE2_2), ((int)GameBitmapEnumeration.bmpFORGE2_3), ((int)GameBitmapEnumeration.bmpFORGE2_4), ((int)GameBitmapEnumeration.bmpFORGEDOWN), ((int)GameBitmapEnumeration.bmpFORGE3_5), ((int)GameBitmapEnumeration.bmpFORGE3_4), ((int)GameBitmapEnumeration.bmpFORGE3_3), ((int)GameBitmapEnumeration.bmpFORGE3_2), ((int)GameBitmapEnumeration.bmpFORGE1_1)}, 
    {((int)GameBitmapEnumeration.bmpFORGE1_1), ((int)GameBitmapEnumeration.bmpFORGE3_2), ((int)GameBitmapEnumeration.bmpFORGE3_3), ((int)GameBitmapEnumeration.bmpFORGE3_4), ((int)GameBitmapEnumeration.bmpFORGEDOWN), ((int)GameBitmapEnumeration.bmpFORGE4_5), ((int)GameBitmapEnumeration.bmpFORGE4_4), ((int)GameBitmapEnumeration.bmpFORGE4_3), ((int)GameBitmapEnumeration.bmpFORGE4_2), ((int)GameBitmapEnumeration.bmpFORGE1_1)}};

    public static void aiForge(TSprite s)
    {
        switch (s.nAttrib[((int)nattrForge.attrForgeMotion)])
        {
            case 0: // Forge is not on screen
                if (s.nAttrib[((int)nattrForge.attrForgeEnergy)] >= energySwing)
                {
                    s.nAttrib[((int)nattrForge.attrForgeEnergy)] = 0;
                    s.nAttrib[((int)nattrForge.attrForgeMotion)] = 1;
                    s.nCC = 0;
                }
                break;
            case 1: // Forge is swinging
                switch (s.nCC)
                {
                    // Swing out
                    case 1: s.SetFrame(frm[IRSequences[s.nAttrib[((int)nattrForge.attrForgeSwing)], 0]]);
                        lSound[((int)ASLList.lsndRING_SWING)].Play(SoundbankInfo.volHOLLAR, panONX(s)); break;
                    case 4: s.SetFrame(frm[IRSequences[s.nAttrib[((int)nattrForge.attrForgeSwing)], 1]]); break;
                    case 7: s.SetFrame(frm[IRSequences[s.nAttrib[((int)nattrForge.attrForgeSwing)], 2]]); break;
                    case 10: s.SetFrame(frm[IRSequences[s.nAttrib[((int)nattrForge.attrForgeSwing)], 3]]); break;
                    // WHAM!
                    case 29: s.SetFrame(frm[IRSequences[s.nAttrib[((int)nattrForge.attrForgeSwing)], 4]]);
                        lSound[((int)ASLList.lsndRING_PRESS)].Play(SoundbankInfo.volHOLLAR, panONX(s)); break;
                    case 52: s.SetFrame(frm[IRSequences[s.nAttrib[((int)nattrForge.attrForgeSwing)], 5]]);
                        lSound[((int)ASLList.lsndRING_DING)].Play(SoundbankInfo.volHOLLAR, panONX(s)); break;
                    case 55: s.SetFrame(frm[IRSequences[s.nAttrib[((int)nattrForge.attrForgeSwing)], 6]]); break;
                    case 91: s.SetFrame(frm[IRSequences[s.nAttrib[((int)nattrForge.attrForgeSwing)], 7]]);
                        lSound[((int)ASLList.lsndRING_SWING)].Play(SoundbankInfo.volNORMAL, panONX(s)); break;
                    case 94: s.SetFrame(frm[IRSequences[s.nAttrib[((int)nattrForge.attrForgeSwing)], 8]]); break;
                    case 97: s.SetFrame(frm[IRSequences[s.nAttrib[((int)nattrForge.attrForgeSwing)], 9]]); break;
                    case 100: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
                        int nTmp = 0;
                        s.nAttrib[((int)nattrForge.attrForgeSwing)]++; s.nAttrib[((int)nattrForge.attrForgeSwing)] %= 3; s.nAttrib[((int)nattrForge.attrForgeMotion)] = 0;
                        TSprite sTmp;
                        if (0 == s.nAttrib[((int)nattrForge.attrForgeSwing)])
                        { // If we are resetting the forge,
                            // The player gets themselves an iron ring!
                            while (!(Globals.myGameConditions.IsRingSpotOpen(nTmp)))
                                nTmp++;
                            Globals.myGameConditions.TakeRingSpot(nTmp);
                            if (nTmp != 0) aisUnlockAchievement(7777);
                            sTmp = SpriteInit.CreateSprite((SpriteType.sprRINGICON), 36 + (40 * (nTmp / 4)),
                                320 + (42 * (nTmp % 4)));
                            sTmp.nAttrib[0] = nTmp;
                            Globals.myGameConditions.GetRing(1);
                            ssIcons.Include(sTmp);
                        }
                        break;
                }
                break;
        }
    }
}