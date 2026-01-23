using System;

public static partial class AIMethods
{

    public static void aisProjectileRebound(TSprite s)
    {
        // Alter the behaviour of a Projectile as it goes "on the rebound."
        switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
        {
            // For an apple, create a whap and go flying.
            case ((int)projTypes.projApple):
            case ((int)projTypes.projClark):
            case ((int)projTypes.projPizza):
            case ((int)projTypes.projExam):
                s.nAttrib[((int)attrProjectile.attrHitTarget)] = ((int)attrAppleHitTargetConstants.attrFlyingRebounding);
                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprWHAP),
                    s.nScrx + s.frmFrame.nHotSpotX,
                    s.nScry + s.frmFrame.nHotSpotY));
                if (s.nX < dPOLEX)
                    s.nvX = randintin(-4, 0) * 20;
                else
                    s.nvX = randintin(0, 4) * 20;
                s.nvY = 0;
                s.nvZ = (R.Next(40) + 10);
                break;
            case ((int)projTypes.projGrease):
                aisCreateHoseWhap(s.nScrx + s.frmFrame.nHotSpotX, s.nScry + s.frmFrame.nHotSpotY, 2);
                s.bDeleted = true;
                // Send the grease gooping down.
                break;
        }
    }

    public static void aisCreateHoseWhap(int nX, int nY, int nDistance)
    {
        TSprite sprTmp;
        sprTmp = SpriteInit.CreateSprite((SpriteType.sprHOSEWHAP), nX, nY);
        switch (nDistance)
        {
            case 1: sprTmp.SetFrame(frm[((int)GameBitmapEnumeration.bmpHSPLASH1_1) + R.Next(5)]); break;
            case 2: sprTmp.SetFrame(frm[((int)GameBitmapEnumeration.bmpHSPLASH2_1) + R.Next(5)]); break;
            default: sprTmp.SetFrame(frm[((int)GameBitmapEnumeration.bmpHSPLASH3_1) + R.Next(5)]); break;
        }
        ssConsole.Include(sprTmp);
    }

    public static void aisRunAwayFrom(TSprite s)
    {
        // Make the Frosh bolt from s's nDestx-position
        int i;
        int n = ssFr.GetNumberOfSprites();
        for (i = 0; i < n; i++)
        {
            if (ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)] == 4
                || ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)] == 7)
            {
                aisSendFroshFlying(ssFr.GetSprite(i));
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalTHINK);
                ssFr.GetSprite(i).bAttrib[((int)battrFrosh.attrExcited)] = true;
                aiInit4(ssFr.GetSprite(i));
                if (s.nDestX < ssFr.GetSprite(i).nX)
                    ssFr.GetSprite(i).nDestX = ssFr.GetSprite(i).nX + (dPITMAXX - dPITMINX) + 40;
                else
                    ssFr.GetSprite(i).nDestX = ssFr.GetSprite(i).nX - (dPITMAXX - dPITMINX) - 40;
                if (ssFr.GetSprite(i).nDestX > dPITMAXX + 100)
                    ssFr.GetSprite(i).nDestX = dPITMAXX + 100;
                if (ssFr.GetSprite(i).nDestX < -100)
                    ssFr.GetSprite(i).nDestX = -100;
            }
        }
    }

    public static int nTemp;
    public static void aiProjectile(TSprite s)
    {

        switch (s.nAttrib[((int)attrProjectile.attrHitTarget)])
        {
            case ((int)attrAppleHitTargetConstants.attrFlyingRebounding):
                aisPlummet(s);
                switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                {
                    case ((int)projTypes.projApple):
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAPPLE5_1) + R.Next(nsprAPPLE5)]); break;
                    case ((int)projTypes.projExam):
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpEXAM4_1) + R.Next(nsprEXAM4)]); break;
                    case ((int)projTypes.projClark):
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLARK5B_1) + R.Next(nsprCLARK5B)]); break;
                    case ((int)projTypes.projPizza):
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPIZZA4_1) + R.Next(nsprPIZZA4)]); break;
                    case ((int)projTypes.projGrease):
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHSPLASH1_1)]); break;
                }
                if (s.nScry > 480)
                    s.bDeleted = true;
                break;
            case ((int)attrAppleHitTargetConstants.attrFlyingTowardTarget):
                switch (s.nCC)
                {
                    case 1:
                        if (s.nAttrib[((int)attrProjectile.attrProjectileType)] == ((int)projTypes.projGrease))
                        {
                            s.nX = s.nAttrib[((int)attrProjectile.attrStartX)];
                            //s.nY = s.nAttrib[((int)attrProjectile.attrStartY)];
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHOSEBALL1)]);
                        }
                        break;
                    case 3:
                        // Frame two
                        s.nX = s.nDestX - ((s.nDestX - s.nAttrib[((int)attrProjectile.attrStartX)]) * 2 / 3);
                        s.nY = s.nDestY + Math.Abs((s.nDestY - s.nAttrib[((int)attrProjectile.attrStartY)]) / 4);
                        switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                        {
                            case ((int)projTypes.projApple):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAPPLE3_1) + R.Next(nsprAPPLE3)]); break;
                            case ((int)projTypes.projPizza):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPIZZA3_1) + R.Next(nsprPIZZA3)]); break;
                            case ((int)projTypes.projClark):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLARK3_1) + R.Next(nsprCLARK3)]); break;
                            case ((int)projTypes.projGrease):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHOSEBALL1)]); break;
                            case ((int)projTypes.projExam):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpEXAM3_1) + R.Next(nsprEXAM3)]);
                                aisRunAwayFrom(s);
                                if (!(lSound[((int)ASLList.lsndEXAM_TOSS1)].IsPlaying()))
                                    lSound[((int)ASLList.lsndEXAM_TOSS1)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                                break;
                        }
                        break;
                    case 4:
                        // Frame two.5
                        s.nX = s.nDestX - ((s.nDestX - s.nAttrib[((int)attrProjectile.attrStartX)]) * 3 / 10);
                        s.nY = s.nDestY;
                        break;
                    case 5:
                        // Frame three
                        s.nX = s.nDestX - ((s.nDestX - s.nAttrib[((int)attrProjectile.attrStartX)]) / 5);
                        s.nY = s.nDestY - Math.Abs((s.nDestY - s.nAttrib[((int)attrProjectile.attrStartY)]) / 4);
                        switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                        {
                            case ((int)projTypes.projApple):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAPPLE3_1) + R.Next(nsprAPPLE3)]); break;
                            case ((int)projTypes.projPizza):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPIZZA3_1) + R.Next(nsprPIZZA3)]); break;
                            case ((int)projTypes.projClark):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLARK3_1) + R.Next(nsprCLARK3)]); break;
                            case ((int)projTypes.projGrease):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHOSEBALL2)]); break;
                            case ((int)projTypes.projExam):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpEXAM3_1) + R.Next(nsprEXAM3)]); break;
                        }
                        break;
                    case 6:
                        // Frame four
                        s.nX = s.nDestX - ((s.nDestX - s.nAttrib[((int)attrProjectile.attrStartX)]) / 10);
                        s.nY = s.nDestY - Math.Abs((s.nDestY - s.nAttrib[((int)attrProjectile.attrStartY)]) / 8);
                        switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                        {
                            case ((int)projTypes.projApple):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAPPLE4_1) + R.Next(nsprAPPLE4)]); break;
                            case ((int)projTypes.projPizza):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPIZZA4_1) + R.Next(nsprPIZZA4)]); break;
                            case ((int)projTypes.projClark):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLARK4_1) + R.Next(nsprCLARK4)]); break;
                            case ((int)projTypes.projGrease):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHOSEBALL3)]); break;
                            case ((int)projTypes.projExam):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpEXAM4_1) + R.Next(nsprEXAM4)]); break;
                        }
                        break;
                    case 7:
                        // Frame four.5
                        s.nX = s.nDestX; s.nY = s.nDestY; break;
                    case 8:
                        // Frame five
                        switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                        {
                            // TEST FOR COLLISION WITH ALIENS
                            case ((int)projTypes.projApple):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAPPLE5_1) + R.Next(nsprAPPLE5)]);
                                if (sprAlien != null && aisScrPointInside(sprAlien, s.nScrx, s.nScry))
                                {
                                    AIMethods.aisUnlockAchievement(99);
                                    sprAlien.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;
                                    if (aisForgeTrick(1, 2000) && Globals.myGameConditions.IsRitual()) // Whip apples at John
                                        lSound[((int)ASLList.lsndFROSH_APPLEHIT2)].Play(SoundbankInfo.volHOLLAR, SoundbankInfo.panCENTER);

                                    aisProjectileRebound(s);
                                }
                                break;
                            case ((int)projTypes.projPizza):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPIZZA5_1) + R.Next(nsprPIZZA5)]);
                                if (sprAlien != null && aisScrPointInside(sprAlien, s.nScrx, s.nScry))
                                {
                                    sprAlien.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;
                                    aisForgeTrick(5, 300);  // Pizza at Alien
                                    aisProjectileRebound(s);
                                }
                                break;
                            case ((int)projTypes.projClark):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLARK5A_1) + R.Next(nsprCLARK5A)]);
                                if (sprAlien != null && aisScrPointInside(sprAlien, s.nScrx, s.nScry))
                                {
                                    sprAlien.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;
                                    aisForgeTrick(4, 300); // Beer at Alien
                                    aisProjectileRebound(s);
                                }
                                break;
                            case ((int)projTypes.projGrease):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHOSEBALL4)]);
                                if (sprAlien != null && aisScrPointInside(sprAlien, s.nScrx, s.nScry))
                                {
                                    sprAlien.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;
                                    aisForgeTrick(2, 800); // Hose at Alien
                                    aisProjectileRebound(s);
                                }
                                break;
                            case ((int)projTypes.projExam):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpEXAM4_1) + R.Next(nsprEXAM4)]);
                                if (sprAlien != null && aisScrPointInside(sprAlien, s.nScrx, s.nScry))
                                {
                                    sprAlien.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;
                                    aisForgeTrick(3, 800); // Exam at Alien
                                    aisProjectileRebound(s);
                                }
                                break;
                        }
                        break;
                    case 9:
                        // Frame six

                        // TEST FOR COLLISION WITH HIPPO AND FROSH AND POPBOY
                        switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                        {
                            case ((int)projTypes.projApple):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAPPLE6_1) + R.Next(nsprAPPLE6)]);
                                if (sprGWHippo != null && aisScrPointInside(sprGWHippo, s.nScrx, s.nScry))
                                {
                                    sprGWHippo.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;
                                    if (sprGWHippo.nZ > 100) s.bDeleted = true;
                                    else aisProjectileRebound(s);
                                    Globals.myGameConditions.AddEnergy(50);
                                }
                                else if (aisCollisionToResponse(s, ssFr, aisSendFroshReallyFlying,
                                    INCLUDEWHAP, POLEACTSASSHIELD))
                                {
                                    aisProjectileRebound(s);
                                    Globals.myGameConditions.AddEnergy(25);
                                }
                                else if (sprPopBoy != null && aisScrPointInside(sprPopBoy, s.nScrx, s.nScry))
                                {
                                    aisForgeTrick(10, -500); // Hit Al with apples
                                    aisProjectileRebound(s);
                                    if (lSound[((int)ASLList.lsndPOPBOY_APPLE1)].IsPlaying() && Globals.myGameConditions.IsRitual())
                                    {
                                        lSound[((int)ASLList.lsndPOPBOY_APPLE1)].Stop();
                                        lSound[((int)ASLList.lsndPOPBOY_APPLER1)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                                    }
                                    else
                                    {
                                        if (!lSound[((int)ASLList.lsndPOPBOY_APPLE1)].IsPlaying() && !lSound[((int)ASLList.lsndPOPBOY_APPLER1)].IsPlaying())
                                        {
                                            for (int i = ((int)ASLList.lsndPOPBOY_ADVICE1); i < ((int)ASLList.lsndPOPBOY_PIZZA2); i++)
                                                lSound[i].Stop();
                                            if (Globals.myGameConditions.IsRitual() && 0 != R.Next(2))
                                                lSound[((int)ASLList.lsndPOPBOY_APPLER1)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                                            else
                                                lSound[((int)ASLList.lsndPOPBOY_APPLE1)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                                            Globals.myGameConditions.AddEnergy(50);
                                        }
                                    }
                                }
                                break;
                            case ((int)projTypes.projPizza):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPIZZA6_1) + R.Next(nsprPIZZA6)]);
                                if (sprGWHippo != null && aisScrPointInside(sprGWHippo, s.nScrx, s.nScry))
                                {
                                    sprGWHippo.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;

                                    if (sprGWHippo.nZ > 100) s.bDeleted = true;
                                    else aisProjectileRebound(s);
                                    Globals.myGameConditions.AddEnergy(50);
                                }
                                else if (aisSpecialCollisionToResponse(s, ssFr, aisSendFroshReallyFlying,
                                    NOWHAP, POLEACTSASSHIELD, true, aiInit11D, aiInit5A))
                                {
                                    Globals.myGameConditions.AddEnergy(20);
                                }
                                else if (sprPopBoy != null && aisScrPointInside(sprPopBoy, s.nScrx, s.nScry))
                                {
                                    aisForgeTrick(11, 500); // Feed Al pizza
                                    s.bDeleted = true;
                                    Globals.myGameConditions.AddEnergy(50);
                                    if (!lSound[((int)ASLList.lsndPOPBOY_PIZZA1)].IsPlaying() && !lSound[((int)ASLList.lsndPOPBOY_PIZZA2)].IsPlaying())
                                    {
                                        for (int i = ((int)ASLList.lsndPOPBOY_ADVICE1); i < ((int)ASLList.lsndPOPBOY_HIPPOR1); i++)
                                            lSound[i].Stop();
                                        lSound[((int)ASLList.lsndPOPBOY_PIZZA1) + R.Next(2)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                                    }
                                }
                                break;
                            case ((int)projTypes.projClark):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLARK6_1) + R.Next(nsprCLARK6)]);
                                if (sprGWHippo != null && aisScrPointInside(sprGWHippo, s.nScrx, s.nScry))
                                {
                                    sprGWHippo.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;
                                    //aisProjectileRebound(s); 
                                    if (sprGWHippo.nZ > 100) s.bDeleted = true;
                                    Globals.myGameConditions.AddEnergy(70);
                                }
                                else if (aisSpecialCollisionToResponse(s, ssFr, aisSendFroshReallyFlying,
                                    NOWHAP, POLEACTSASSHIELD, true, aiInit11E, aiInit5B))
                                {
                                    Globals.myGameConditions.AddEnergy(20);
                                }
                                else if (sprPopBoy != null && sprPopBoy.nZ < 2 && aisScrPointInside(sprPopBoy, s.nScrx, s.nScry))
                                {
                                    AIMethods.aisUnlockAchievement(1999);

                                    if (!aisForgeTrick(12, 2000)) // Feed Al Beer first time
                                        aisForgeTrick(13, 1000); // Feed Al Beer second time
                                    s.bDeleted = true;
                                    Globals.myGameConditions.AddEnergy(50);
                                    if (!lSound[((int)ASLList.lsndPOPBOY_BEER1)].IsPlaying() && !lSound[((int)ASLList.lsndPOPBOY_BEERR2)].IsPlaying() && !lSound[((int)ASLList.lsndPOPBOY_BEERR3)].IsPlaying())
                                    {
                                        for (int i = ((int)ASLList.lsndPOPBOY_ADVICE1); i < ((int)ASLList.lsndPOPBOY_PIZZA2); i++)
                                            lSound[i].Stop();
                                        sprPopBoy.nAttrib[((int)nattrFrosh.attrBehavior)] = 6; sprPopBoy.nCC = 0;

                                    }
                                }
                                break;
                            case ((int)projTypes.projGrease):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHOSEBALL5)]);
                                if (sprGWHippo != null && aisScrPointInside(sprGWHippo, s.nScrx, s.nScry))
                                {
                                    //sprGWHippo.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;

                                    if (sprGWHippo.nZ > 100) s.bDeleted = true;
                                    else aisCreateHoseWhap(s.nScrx + s.frmFrame.nHotSpotX, s.nScry + s.frmFrame.nHotSpotY, 1);
                                    Globals.myGameConditions.AddEnergy(3);

                                }
                                else if (aisCollisionToResponse(s, ssFr, aisSendFroshReallyFlying,
                                    NOWHAP, POLEACTSASSHIELD, false))
                                {
                                    aisCreateHoseWhap(s.nScrx + s.frmFrame.nHotSpotX, s.nScry + s.frmFrame.nHotSpotY, 1);
                                    s.bDeleted = true;
                                    Globals.myGameConditions.AddEnergy(3);
                                }
                                break;
                            case ((int)projTypes.projExam):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpEXAM4_1) + R.Next(nsprEXAM4)]);
                                if (sprGWHippo != null && aisScrPointInside(sprGWHippo, s.nScrx, s.nScry))
                                {
                                    sprGWHippo.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;
                                    if (sprGWHippo.nZ > 100) s.bDeleted = true;
                                    else aisProjectileRebound(s);
                                    Globals.myGameConditions.AddEnergy(150);
                                }
                                else if (sprPopBoy != null && aisScrPointInside(sprPopBoy, s.nScrx, s.nScry))
                                {
                                    aisProjectileRebound(s);
                                    Globals.myGameConditions.AddEnergy(50);
                                    if (!lSound[((int)ASLList.lsndPOPBOY_EXAM1)].IsPlaying() && !lSound[((int)ASLList.lsndPOPBOY_EXAM2)].IsPlaying() && !lSound[((int)ASLList.lsndPOPBOY_EXAM3)].IsPlaying())
                                    {
                                        for (int i = ((int)ASLList.lsndPOPBOY_ADVICE1); i < ((int)ASLList.lsndPOPBOY_HIPPOR1); i++)
                                            lSound[i].Stop();
                                        sprPopBoy.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)]++; sprPopBoy.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] %= 3;
                                        lSound[((int)ASLList.lsndEXAM_TOSS1)].Stop();
                                        lSound[((int)ASLList.lsndPOPBOY_EXAM1) + sprPopBoy.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)]].Play(SoundbankInfo.volHOLLAR, panONX(s));
                                    }
                                }
                                else if (aisCollisionToResponse(s, ssFr, aisSendFroshReallyFlying,
                                    INCLUDEWHAP, POLEACTSASSHIELD))
                                {
                                    aisProjectileRebound(s);
                                    Globals.myGameConditions.AddEnergy(70);
                                }
                                break;
                        }
                        break;
                    case 10:
                        // Frame seven

                        // Test for collision with FRECS and POLE and PREZ
                        if (s.nX > (dPOLEX - dPOLEWIDTH) && s.nX < (dPOLEX + dPOLEWIDTH)
                            && s.nScry > sprTam.nScry && s.nScry < (sprTam.nScry + dPOLEHEIGHT))
                            if (s.nAttrib[((int)attrProjectile.attrProjectileType)] != ((int)projTypes.projGrease))
                                aisProjectileRebound(s);  // POLE
                            else
                            {
                                aisCreateHoseWhap(s.nScrx + s.frmFrame.nHotSpotX, s.nScry + s.frmFrame.nHotSpotY, 1);
                                s.bDeleted = true;
                            }
                        else
                        { // FRECS
                            s.CalculateScreenCoordinates(Globals.myLayers.GetOffset(((int)LayerNames.LAYERPIT)));
                            // FRECS LEFT
                            if (aisScrPointInside(sprFrecsL, s.nScrx, s.nScry))
                            {
                                switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                                {
                                    case ((int)projTypes.projApple):
                                        sprForge.nAttrib[((int)nattrForge.attrForgeEnergy)] -= 600;
                                        aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faBlocking));
                                        lSound[((int)ASLList.lsndFRECS_HITAPPLE1) + R.Next(SoundbankInfo.nsndFRECS_HITAPPLE)].Play(SoundbankInfo.volHOLLAR, (sprFrecsL.nX - 320) / 32);
                                        aisProjectileRebound(s); break;
                                    case ((int)projTypes.projClark):
                                        if (!aisForgeTrick(9, 200)) // Beer at crowd 1
                                            aisForgeTrick(15, 300); // Beer at crowd 2
                                        sprFrecsL.nAttrib[((int)nattrCrowd.attrFEnergy)] += 160;
                                        if (sprFrecsL.nAttrib[((int)nattrCrowd.attrFAction)] == ((int)attrCrowdActions.faSlamming))
                                        {
                                            AIMethods.aisUnlockAchievement(2000);
                                            aisForgeTrick(16, 2000); // Beer at crowd HAMMERED
                                            //														aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faStayinAlive));
                                        }
                                        nTemp = R.Next(SoundbankInfo.nsndFRECS_ROAR);
                                        if (!sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].IsPlaying())
                                            sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].Play(SoundbankInfo.volCROWD, (sprFrecsL.nX - 320) / 32);
                                        s.bDeleted = true; break;
                                    case ((int)projTypes.projPizza):
                                        sprFrecsL.nAttrib[((int)nattrCrowd.attrFEnergy)] += 60;
                                        nTemp = R.Next(SoundbankInfo.nsndFRECS_CHEER);
                                        if (!lSound[((int)ASLList.lsndFRECS_CHEER1) + nTemp].IsPlaying())
                                            lSound[((int)ASLList.lsndFRECS_CHEER1) + nTemp].Play(SoundbankInfo.volHOLLAR, (sprFrecsL.nX - 320) / 32);
                                        s.bDeleted = true; break;
                                    case ((int)projTypes.projGrease):
                                        sprFrecsL.nAttrib[((int)nattrCrowd.attrFEnergy)] += 3;
                                        aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faBlocking));
                                        //lSound[((int)ASLList.lsndFRECS_HITAPPLE1) + R.Next(SoundbankInfo.nsndFRECS_HITAPPLE)].Play(SoundbankInfo.volHOLLAR, (sprFrecsL.nX - 320)/32);
                                        aisCreateHoseWhap(s.nScrx + s.frmFrame.nHotSpotX, s.nScry + s.frmFrame.nHotSpotY, 1);

                                        s.bDeleted = true; break;
                                    case ((int)projTypes.projExam):
                                        sprFrecsL.nAttrib[((int)nattrCrowd.attrFEnergy)] += 50;
                                        aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faBlocking));
                                        lSound[((int)ASLList.lsndEXAM_TOSS1)].Stop();
                                        lSound[((int)ASLList.lsndFRECS_HITEXAM1) + R.Next(SoundbankInfo.nsndFRECS_HITEXAM)].Play(SoundbankInfo.volHOLLAR, (sprFrecsL.nX - 320) / 32);
                                        aisProjectileRebound(s); break;
                                }
                            }
                            // FRECS CENTER
                            if (aisScrPointInside(sprFrecsC, s.nScrx, s.nScry))
                            {
                                switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                                {
                                    case ((int)projTypes.projApple):
                                        sprForge.nAttrib[((int)nattrForge.attrForgeEnergy)] -= 600;
                                        aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faBlocking));
                                        lSound[((int)ASLList.lsndFRECS_HITAPPLE1) + R.Next(SoundbankInfo.nsndFRECS_HITAPPLE)].Play(SoundbankInfo.volHOLLAR, (sprFrecsC.nX - 320) / 32);
                                        aisProjectileRebound(s); break;
                                    case ((int)projTypes.projClark):
                                        if (!aisForgeTrick(9, 200)) // Beer at crowd 1
                                            aisForgeTrick(15, 300); // Beer at crowd 2
                                        sprFrecsC.nAttrib[((int)nattrCrowd.attrFEnergy)] += 160;
                                        if (sprFrecsC.nAttrib[((int)nattrCrowd.attrFAction)] == ((int)attrCrowdActions.faSlamming))
                                        {
                                            AIMethods.aisUnlockAchievement(2000);
                                            aisForgeTrick(17, 2000); // Beer at crowd HAMMERED
                                            //														aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faStayinAlive));
                                        }

                                        nTemp = R.Next(SoundbankInfo.nsndFRECS_ROAR);
                                        if (!sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].IsPlaying())
                                            sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].Play(SoundbankInfo.volCROWD, (sprFrecsC.nX - 320) / 32);
                                        s.bDeleted = true; break;
                                    case ((int)projTypes.projPizza):
                                        sprFrecsC.nAttrib[((int)nattrCrowd.attrFEnergy)] += 60;
                                        nTemp = R.Next(SoundbankInfo.nsndFRECS_CHEER);
                                        if (!lSound[((int)ASLList.lsndFRECS_CHEER1) + nTemp].IsPlaying())
                                            lSound[((int)ASLList.lsndFRECS_CHEER1) + nTemp].Play(SoundbankInfo.volHOLLAR, (sprFrecsC.nX - 320) / 32);
                                        s.bDeleted = true; break;
                                    case ((int)projTypes.projGrease):
                                        sprFrecsC.nAttrib[((int)nattrCrowd.attrFEnergy)] += 10;
                                        aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faBlocking));
                                        //lSound[((int)ASLList.lsndFRECS_HITAPPLE1) + R.Next(SoundbankInfo.nsndFRECS_HITAPPLE)].Play(SoundbankInfo.volHOLLAR, (sprFrecsC.nX - 320)/32);
                                        aisCreateHoseWhap(s.nScrx + s.frmFrame.nHotSpotX, s.nScry + s.frmFrame.nHotSpotY, 1);
                                        s.bDeleted = true; break;
                                    case ((int)projTypes.projExam):
                                        sprFrecsC.nAttrib[((int)nattrCrowd.attrFEnergy)] += 50;
                                        aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faBlocking));
                                        lSound[((int)ASLList.lsndEXAM_TOSS1)].Stop();
                                        lSound[((int)ASLList.lsndFRECS_HITEXAM1) + R.Next(SoundbankInfo.nsndFRECS_HITEXAM)].Play(SoundbankInfo.volHOLLAR, (sprFrecsC.nX - 320) / 32);
                                        aisProjectileRebound(s); break;
                                }
                            }
                            // FRECS RIGHt
                            if (aisScrPointInside(sprFrecsR, s.nScrx, s.nScry))
                            {
                                switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                                {
                                    case ((int)projTypes.projApple):
                                        sprForge.nAttrib[((int)nattrForge.attrForgeEnergy)] -= 600;
                                        aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faBlocking));
                                        lSound[((int)ASLList.lsndFRECS_HITAPPLE1) + R.Next(SoundbankInfo.nsndFRECS_HITAPPLE)].Play(SoundbankInfo.volHOLLAR, (sprFrecsR.nX - 320) / 32);
                                        aisProjectileRebound(s); break;
                                    case ((int)projTypes.projClark):
                                        if (!(aisForgeTrick(9, 200)))
                                            aisForgeTrick(15, 300); // Beer at crowd
                                        sprFrecsR.nAttrib[((int)nattrCrowd.attrFEnergy)] += 160;
                                        if (sprFrecsR.nAttrib[((int)nattrCrowd.attrFAction)] == ((int)attrCrowdActions.faSlamming))
                                        {
                                            AIMethods.aisUnlockAchievement(2000);
                                            aisForgeTrick(16, 2000); // Beer at crowd HAMMERED
                                            //														aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faStayinAlive));
                                        }

                                        nTemp = R.Next(SoundbankInfo.nsndFRECS_ROAR);
                                        if (!sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].IsPlaying())
                                            sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].Play(SoundbankInfo.volCROWD, (sprFrecsR.nX - 320) / 32);
                                        s.bDeleted = true; break;
                                    case ((int)projTypes.projPizza):
                                        sprFrecsR.nAttrib[((int)nattrCrowd.attrFEnergy)] += 60;
                                        nTemp = R.Next(SoundbankInfo.nsndFRECS_CHEER);
                                        if (!lSound[((int)ASLList.lsndFRECS_CHEER1) + nTemp].IsPlaying())
                                            lSound[((int)ASLList.lsndFRECS_CHEER1) + nTemp].Play(SoundbankInfo.volHOLLAR, (sprFrecsR.nX - 320) / 32);
                                        s.bDeleted = true; break;
                                    case ((int)projTypes.projGrease):
                                        sprFrecsR.nAttrib[((int)nattrCrowd.attrFEnergy)] += 10;
                                        aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faBlocking));
                                        //lSound[((int)ASLList.lsndFRECS_HITAPPLE1) + R.Next(SoundbankInfo.nsndFRECS_HITAPPLE)].Play(SoundbankInfo.volHOLLAR, (sprFrecsR.nX - 320)/32);
                                        aisCreateHoseWhap(s.nScrx + s.frmFrame.nHotSpotX, s.nScry + s.frmFrame.nHotSpotY, 1);
                                        s.bDeleted = true; break;
                                    case ((int)projTypes.projExam):
                                        sprFrecsR.nAttrib[((int)nattrCrowd.attrFEnergy)] += 50;
                                        aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faBlocking));
                                        lSound[((int)ASLList.lsndEXAM_TOSS1)].Stop();
                                        lSound[((int)ASLList.lsndFRECS_HITEXAM1) + R.Next(SoundbankInfo.nsndFRECS_HITEXAM)].Play(SoundbankInfo.volHOLLAR, (sprFrecsR.nX - 320) / 32);
                                        aisProjectileRebound(s); break;
                                }
                            }
                            // PREZ
                            if (aisScrPointInside(sprPrez, s.nScrx, s.nScry) || (aisScrPointInside(sprPrez, s.nScrx + 30, s.nScry)))
                            {
                                int nHitNumber = Globals.myGameConditions.GetPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                //aisProjectileRebound(s);
                                switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                                {
                                    case ((int)projTypes.projApple):
                                        aisProjectileRebound(s);
                                        sprForge.nAttrib[((int)nattrForge.attrForgeEnergy)] -= 2500;
                                        if ((sprPrez.nAttrib[((int)attrPrez.attrPrezAction)]) != 4)
                                            switch (nHitNumber)
                                            {
                                                case 0:
                                                    if (!lSound[((int)ASLList.lsndPREZ_HITAPPLER4)].IsPlaying() && !lSound[((int)ASLList.lsndPREZ_HITAPPLE4)].IsPlaying())
                                                    {
                                                        lSound[((int)ASLList.lsndPREZ_HITAPPLE1)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                                        Globals.myGameConditions.AddPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                                    }
                                                    break;
                                                case 1:
                                                    if (!lSound[((int)ASLList.lsndPREZ_HITAPPLE1)].IsPlaying())
                                                    {
                                                        lSound[((int)ASLList.lsndPREZ_HITAPPLE2)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                                        Globals.myGameConditions.AddPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                                    }
                                                    break;
                                                case 2:
                                                    if (!lSound[((int)ASLList.lsndPREZ_HITAPPLE2)].IsPlaying())
                                                    {
                                                        lSound[((int)ASLList.lsndPREZ_HITAPPLE3)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                                        Globals.myGameConditions.AddPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                                    }
                                                    break;
                                                case 3:
                                                    if (!lSound[((int)ASLList.lsndPREZ_HITAPPLE3)].IsPlaying())
                                                    {
                                                        lSound[Globals.myGameConditions.IsRitual() ? ((int)ASLList.lsndPREZ_HITAPPLER4) : ((int)ASLList.lsndPREZ_HITAPPLE4)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                                        Globals.myGameConditions.AddPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                                    }
                                                    break;
                                                default:
                                                    if (!lSound[((int)ASLList.lsndPREZ_HITAPPLER4)].IsPlaying() && !lSound[((int)ASLList.lsndPREZ_HITAPPLE4)].IsPlaying())
                                                    {
                                                        lSound[((int)ASLList.lsndPREZ_HITAPPLE5)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                                        Globals.myGameConditions.ResetPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                                        sprPole.SetBehavior(aiPushSciConM);
                                                        sprPole.nCC = 43;
                                                    }
                                                    break;

                                            }
                                        /*aisProjectileRebound(s);*/
                                        break;
                                    case ((int)projTypes.projClark):
                                        aisForgeTrick(6, 1000); // Beer at George
                                        AIMethods.aisUnlockAchievement(98);
                                        if ((sprPrez.nAttrib[((int)attrPrez.attrPrezAction)]) != 4)
                                            switch (nHitNumber)
                                            {
                                                case 0:
                                                    if (!lSound[((int)ASLList.lsndPREZ_HITCLARK2)].IsPlaying() && !lSound[((int)ASLList.lsndPREZ_HITCLARK3)].IsPlaying())
                                                    {
                                                        lSound[Globals.myGameConditions.IsRitual() ? ((int)ASLList.lsndPREZ_HITCLARK1) : ((int)ASLList.lsndPREZ_HITCLARK1)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                                        Globals.myGameConditions.AddPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                                    }
                                                    break;
                                                case 1:
                                                    if (!lSound[((int)ASLList.lsndPREZ_HITCLARK1)].IsPlaying())
                                                    {
                                                        lSound[Globals.myGameConditions.IsRitual() ? ((int)ASLList.lsndPREZ_HITCLARK2) : ((int)ASLList.lsndPREZ_HITCLARK2)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                                        Globals.myGameConditions.AddPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                                    }
                                                    break;
                                                default:
                                                    if (!lSound[((int)ASLList.lsndPREZ_HITCLARK2)].IsPlaying())
                                                    {
                                                        lSound[Globals.myGameConditions.IsRitual() ? ((int)ASLList.lsndPREZ_HITCLARK3) : ((int)ASLList.lsndPREZ_HITCLARK3)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                                        Globals.myGameConditions.ResetPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                                    }
                                                    break;
                                            }
                                        s.bDeleted = true; break;
                                    case ((int)projTypes.projPizza):
                                        AIMethods.aisUnlockAchievement(98);
                                        aisForgeTrick(14, 800);  // Pizza at George
                                        if ((sprPrez.nAttrib[((int)attrPrez.attrPrezAction)]) != 4)
                                            switch (nHitNumber)
                                            {
                                                case 0:
                                                    if (!lSound[((int)ASLList.lsndPREZ_HITPIZZA2)].IsPlaying() && !lSound[((int)ASLList.lsndPREZ_HITPIZZAR2)].IsPlaying())
                                                    {
                                                        lSound[Globals.myGameConditions.IsRitual() ? ((int)ASLList.lsndPREZ_HITPIZZA1) : ((int)ASLList.lsndPREZ_HITPIZZA1)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                                        Globals.myGameConditions.AddPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                                    }
                                                    break;
                                                default:
                                                    if (!lSound[((int)ASLList.lsndPREZ_HITPIZZA1)].IsPlaying())
                                                    {
                                                        lSound[Globals.myGameConditions.IsRitual() ? ((int)ASLList.lsndPREZ_HITPIZZAR2) : ((int)ASLList.lsndPREZ_HITPIZZA2)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                                        Globals.myGameConditions.ResetPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                                    }
                                                    break;
                                            }
                                        s.bDeleted = true;
                                        break;
                                    case ((int)projTypes.projGrease):
                                        if ((sprPrez.nAttrib[((int)attrPrez.attrPrezAction)]) != 4)
                                        {
                                            Globals.myGameConditions.AddPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]);
                                            switch (nHitNumber)
                                            {
                                                case 1: sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4; lSound[((int)ASLList.lsndPREZ_HITHOSE1)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32); break;
                                                case 2: sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4; lSound[Globals.myGameConditions.IsRitual() ? ((int)ASLList.lsndPREZ_HITHOSER2) : ((int)ASLList.lsndPREZ_HITHOSE2)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32); lSound[((int)ASLList.lsndPREZ_HITHOSE1)].Stop(); break;
                                                case 3: sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4; lSound[Globals.myGameConditions.IsRitual() ? ((int)ASLList.lsndPREZ_HITHOSER3) : ((int)ASLList.lsndPREZ_HITHOSE2)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32); lSound[((int)ASLList.lsndPREZ_HITHOSER2)].Stop(); lSound[((int)ASLList.lsndPREZ_HITHOSE2)].Stop(); break;
                                                case 4: sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4; lSound[Globals.myGameConditions.IsRitual() ? ((int)ASLList.lsndPREZ_HITHOSER4) : ((int)ASLList.lsndPREZ_HITHOSE2)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32); lSound[((int)ASLList.lsndPREZ_HITHOSER3)].Stop(); lSound[((int)ASLList.lsndPREZ_HITHOSE2)].Stop();
                                                    Globals.myGameConditions.ResetPrezHit(s.nAttrib[((int)attrProjectile.attrProjectileType)]); break;
                                            }
                                        }
                                        aisCreateHoseWhap(s.nScrx + s.frmFrame.nHotSpotX, s.nScry + s.frmFrame.nHotSpotY, 2);
                                        aisProjectileRebound(s); s.bDeleted = true; break;
                                    case ((int)projTypes.projExam):
                                        sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                                        lSound[((int)ASLList.lsndPREZ_HITEXAM)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32); s.bDeleted = true;
                                        lSound[((int)ASLList.lsndEXAM_TOSS1)].Stop(); NOSPEECHFOR(100);
                                        break;
                                }
                            }

                        }

                        switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
                        {
                            case ((int)projTypes.projApple):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAPPLE7_1) + R.Next(nsprAPPLE7)]); break;
                            case ((int)projTypes.projClark):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLARK7_1) + R.Next(nsprCLARK7)]); break;
                            case ((int)projTypes.projPizza):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPIZZA7_1) + R.Next(nsprPIZZA7)]); break;
                            case ((int)projTypes.projGrease):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHOSEBALL6)]); break;
                            case ((int)projTypes.projExam):
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpEXAM4_1) + R.Next(nsprEXAM4)]); break;
                        }
                        break;

                    case 11:
                        // Frame eight
                        if (s.nScry > Globals.myLayers.GetOffset(((int)LayerNames.LAYERPIT)))
                        {
                            // THE PROJECTILE JUST HIT THE WATER.
                            ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), s.nX, s.nY));
                            if (s.nAttrib[((int)attrProjectile.attrProjectileType)] == ((int)projTypes.projPizza))
                                aiInitFloatingPizza(s);// Create a floating pizza sprite in the pit
                            else if (s.nAttrib[((int)attrProjectile.attrProjectileType)] == ((int)projTypes.projClark))
                                aiInitFloatingClark(s);// Create a floating Clark mug in the pit
                            else
                                s.bDeleted = true;
                        }
                        else if (s.nScry > (Globals.myLayers.GetOffset(((int)LayerNames.LAYERPIT)) - dBANKHEIGHT))
                        {
                            // THE PROJECTILE JUST HIT THE BANK OPPOSITE
                            aisProjectileRebound(s);
                        }
                        break;
                    case 12:
                        s.nY += 4; break;
                    case 13:
                        s.nY += 7; break;
                    case 14:
                        s.nY += 10; s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGLINT_1) + R.Next(2)]); break;
                    case 15:
                        s.bDeleted = true; break;
                }
                break;
        }
    }


    public static void aiFloatingProjectile(TSprite s)
    {
        aisBobUpAndDown(s, (timeAVERAGEBOBTIME / 3));
        switch (s.nAttrib[((int)attrProjectile.attrProjectileType)])
        {
            case ((int)projTypes.projPizza):
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpPIZZA5_1) + R.Next(nsprPIZZA5)]);
                break;
            case ((int)projTypes.projClark):
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLARK5A_1) + ((s.nCC % (nsprCLARK5A * timeCLARKMUGFLOATROTATION)) / timeCLARKMUGFLOATROTATION)]);
                break;
        }

        if (s.nCC > 500)
            s.bDeleted = true;
    }

    public static void aiInitFloatingPizza(TSprite s)
    {// Create a floating pizza sprite in the pit  
        s.nCC = 0;
        s.SetBehavior(aiFloatingProjectile);
        aisChasePizza(s);
    }

    public static void aiInitFloatingClark(TSprite s)
    {// Create a floating pizza sprite in the pit  
        s.nCC = 0;
        s.SetBehavior(aiFloatingProjectile);
        aisChaseClark(s);
    }

    public static void aiCloseUpBeer(TSprite s)
    {
        if (aisForgeTrick(7, 200)) // Guzzle Beer first time
            Globals.myGameConditions.AddEnergy(50);
        switch (s.nCC)
        {
            case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLOSEUPBEER1)]); break;
            case 5: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpCLOSEUPBEER2)]); break;
            case 9: s.bDeleted = true; break;
        }
    }

    public static void aiArmRing1(TSprite s)
    {
        if (s.nY > 327) s.nY -= 10;
        else
        {
            if (s.nX == 44 && s.nCC == 40) s.bDeleted = true;
            else aiInitFlyInAndOut2(s, aiArmRing1, s.nX, s.nY, 44, s.nY, 1, 1);
        }

    }
    public static void aiArmRing2(TSprite s)
    {
        if (s.nY > 299) s.nY -= 10;
        else
        {
            if (s.nX == 224 && s.nCC == 40) s.bDeleted = true;
            else aiInitFlyInAndOut2(s, aiArmRing2, s.nX, s.nY, 224, s.nY, 1, 1);
        }
    }
    public static void aiArmRing3(TSprite s)
    {
        if (s.nY > 227) s.nY -= 10;
        else
        {
            if (s.nX == 245 && s.nCC == 40)
            {
                s.bDeleted = true; sprArm.nCC = 300; sprArm.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND7_7)]);
                sprArm.nX = 320; sprArm.nY = 700;
            }
            else aiInitFlyInAndOut2(s, aiArmRing3, s.nX, s.nY, 245, s.nY, 1, 1);
        }
    }

    public static int aisChangeArm()
    {
        int nCurrent = sprArm.nAttrib[((int)attrArm.attrArmStatus)];
        int bOK = 0;
        int nApples = Globals.myGameConditions.GetPlayerApples();
        int nPizza = Globals.myGameConditions.GetPlayerPizza();
        int nClark = Globals.myGameConditions.GetPlayerClark();
        int nExams = Globals.myGameConditions.GetPlayerExam();
        if (nCurrent == ((int)ArmPositions.armGREASE) || nCurrent == ((int)ArmPositions.armIRONRING))
            nCurrent = ((int)ArmPositions.armAPPLE);
        else
            nCurrent++;

        if (0 == (nApples + nPizza + nClark + nExams))
            return ((int)ArmPositions.armNOTHING);

        do
        {
            switch (nCurrent)
            {
                case ((int)ArmPositions.armAPPLE):
                    if (0 != nApples) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armPIZZA);
                    break;
                case ((int)ArmPositions.armPIZZA):
                    if (0 != nPizza) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armCLARK);
                    break;
                case ((int)ArmPositions.armCLARK):
                    if (0 != nClark) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armEXAM);
                    break;
                case ((int)ArmPositions.armEXAM):
                    if (0 != nExams) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armGREASE);
                    break;
                case ((int)ArmPositions.armGREASE):
                    if (sprWaterMeter != null) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armIRONRING);
                    break;
                case ((int)ArmPositions.armIRONRING):
                    if (sprRingMeter != null) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armAPPLE);
                    break;
                default:
                    nCurrent = ((int)ArmPositions.armAPPLE); break;
            }
        } while (0 == bOK);


        switch (nCurrent)
        {
            case ((int)ArmPositions.armAPPLE):
                if (0 != nApples) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
//                break;
            case ((int)ArmPositions.armPIZZA):
                if (0 != nPizza) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
//                break;
            case ((int)ArmPositions.armCLARK):
                if (0 != nClark) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
//                break;
            case ((int)ArmPositions.armEXAM):
                if (0 != nExams) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
//                break;
            case ((int)ArmPositions.armGREASE):
                if (sprWaterMeter != null) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
//                break;
            case ((int)ArmPositions.armIRONRING):
                if (sprRingMeter != null) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
 //               break;
            default:
                return nCurrent; //break;

        }
//        return nCurrent;
    }

    public static int aisChangeArmBackwards()
    {
        int nCurrent = sprArm.nAttrib[((int)attrArm.attrArmStatus)];
        int bOK = 0;
        int nApples = Globals.myGameConditions.GetPlayerApples();
        int nPizza = Globals.myGameConditions.GetPlayerPizza();
        int nClark = Globals.myGameConditions.GetPlayerClark();
        int nExams = Globals.myGameConditions.GetPlayerExam();
        if (nCurrent == ((int)ArmPositions.armGREASE) || nCurrent == ((int)ArmPositions.armIRONRING))
            nCurrent = ((int)ArmPositions.armAPPLE);
        else
            nCurrent--;

        if (0 == (nApples + nPizza + nClark + nExams))
            return ((int)ArmPositions.armNOTHING);

        do
        {
            switch (nCurrent)
            {
                case ((int)ArmPositions.armAPPLE):
                    if (0 != nApples) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armIRONRING);
                    break;
                case ((int)ArmPositions.armPIZZA):
                    if (0 != nPizza) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armAPPLE);
                    break;
                case ((int)ArmPositions.armCLARK):
                    if (0 != nClark) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armPIZZA);
                    break;
                case ((int)ArmPositions.armEXAM):
                    if (0 != nExams) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armCLARK);
                    break;
                case ((int)ArmPositions.armGREASE):
                    if (sprWaterMeter != null) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armEXAM);
                    break;
                case ((int)ArmPositions.armIRONRING):
                    if (sprRingMeter != null) bOK = 1;
                    else nCurrent = ((int)ArmPositions.armGREASE);
                    break;
                default:
                    nCurrent = ((int)ArmPositions.armIRONRING); break;
            }
        } while (0 == bOK);


        switch (nCurrent)
        {
            case ((int)ArmPositions.armAPPLE):
                if (0 != nApples) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
            //                break;
            case ((int)ArmPositions.armPIZZA):
                if (0 != nPizza) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
            //                break;
            case ((int)ArmPositions.armCLARK):
                if (0 != nClark) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
            //                break;
            case ((int)ArmPositions.armEXAM):
                if (0 != nExams) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
            //                break;
            case ((int)ArmPositions.armGREASE):
                if (sprWaterMeter != null) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
            //                break;
            case ((int)ArmPositions.armIRONRING):
                if (sprRingMeter != null) return nCurrent;
                else return ((int)ArmPositions.armPIZZA);
            //               break;
            default:
                return nCurrent; //break;

        }
        //        return nCurrent;
    }


    public static int nDemoOldMouseX = 0;
    public static int nDemoOldMouseY = 0;
    public static int nMouseX;
    public static int nMouseY;
    public static bool aisDemoMouseHasMoved() {
        nMouseX = Globals.InputService.GetMouseX();
        nMouseY = Globals.InputService.GetMouseY();
	  bool bAnswer = (nDemoOldMouseX != nMouseX) || (nDemoOldMouseY != nMouseY);
	  nDemoOldMouseX = nMouseX; nDemoOldMouseY = nMouseY;
	  return bAnswer;
  }
    public const int RINGUPSTARTTIME = 375;

    public static int nTossPower;
    public static void aiArm(TSprite s)
    {
        s.nX = Globals.InputService.GetMouseX() + ((640 - Globals.InputService.GetMouseX()) / 6);
        s.nY = 480 + ((Globals.InputService.GetMouseY()) / 5);
        // If the user clicks the left mouse button, set up a toss

        switch (s.nAttrib[((int)attrArm.attrArmStatus)])
        {
            case ((int)ArmPositions.armDEMO): // The demo hand is invisible and determines if it's time to exit the demo
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
                if (aisDemoMouseHasMoved() && s.nCC > 10)
                {
                    Globals.myGameLoop.ChangeGameState(((int)GameStates.STATETITLE));
                }
                break;
            case ((int)ArmPositions.armNOTHING):
                switch (s.nAttrib[((int)attrArm.attrArmAction)])
                {
                    case 0:                          // HAND, NOTHING IN IT
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND0_1)]);
                        if (0 == R.Next(10))
                        {
                            // TRY TO SWITCH TO WEAPONS
                            if (aisChangeArm() != ((int)ArmPositions.armNOTHING))
                            {
                                s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                                s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                                s.nCC = 0;
                            }
                        }
                        if (0 == R.Next(150))
                            s.nAttrib[((int)attrArm.attrArmAction)] = 1; s.nCC = 0;
                        break;
                    case 1: // Unknown
                        switch (s.nCC)
                        {
                            case 7:
                                s.nAttrib[((int)attrArm.attrArmAction)] = 0;
                                break;
                        }
                        break;
                    case 2: // Pushing
                        switch (s.nCC)
                        {
                            case 1:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND0_2)]);
                                break;
                            case 9:
                                s.nAttrib[((int)attrArm.attrArmAction)] = 0;
                                break;
                        }
                        break;
                    case 3: // Snatching
                        switch (s.nCC)
                        {
                            case 1:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND0_3)]);
                                break;
                            case 9:
                                s.nAttrib[((int)attrArm.attrArmAction)] = 0;
                                break;
                        }
                        break;
                }
                break;                             /* ArmNOTHING */

            case ((int)ArmPositions.armAPPLE):
                switch (s.nAttrib[((int)attrArm.attrArmAction)])
                {
                    case 0:                          // HOLDING AN APPLE
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_1)]);
                        if (s.nCC > 100 && 0 == R.Next(50))
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 0 != R.Next(2) ? 1 : 3; s.nCC = 0; // Small Bob
                        }
                        if (Globals.InputService.LeftButtonPressed())
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 2; s.nCC = 0;
                        }
                        if (Globals.InputService.RightButtonPressed())
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 0 != R.Next(2) ? 1 : 3; s.nCC = 0;
                        }
                        if (Globals.InputService.ToggleForwardButtonPressed())
                        {
                            // SWITCH
                            s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                            s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                            s.nCC = 0;
                        }
                        else if (Globals.InputService.ToggleBackButtonPressed())
                        {
                            // SWITCH
                            s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArmBackwards();
                            s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                            s.nCC = 0;
                        }
                        break;
                    case 1:                          // BOBBING AN APPLE: SMALL
                        switch (s.nCC)
                        {
                            case 1:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_2)]);
                                break;
                            case 5:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_3)]);
                                break;
                            case 7:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_2)]);
                                break;
                            case 9:
                                s.nAttrib[((int)attrArm.attrArmAction)] = 0;
                                break;
                        }
                        break;
                    case 3:                          // BOBBING AN APPLE: BIG
                        switch (s.nCC)
                        {
                            case 1:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_2)]);
                                break;
                            case 5:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_3)]);
                                break;
                            case 7:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_4)]);
                                break;
                            case 16:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_3)]);
                                break;
                            case 18:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_2)]);
                                break;
                            case 19:
                                s.nAttrib[((int)attrArm.attrArmAction)] = 0;
                                break;
                        }
                        break;

                    case 2:                          // TOSSING AN APPLE
                        switch (s.nCC)
                        {
                            case 1: // Left arm windup
                                nTossPower = 1; s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_5)]); break;
                            case 3: s.nX -= 390;
                                if (s.nX > -75) s.nX = -75; break;
                            case 4: s.nX -= 390;
                                sprPowerMeter = SpriteInit.CreateSprite((SpriteType.sprPOWERMETER), 26, 95);
                                ssConsole.Include(sprPowerMeter);
                                if (s.nX > -70) s.nX = -70; break;
                            case 5: s.nX -= 430; if (s.nX > -75) s.nX = -75;
                                if (Globals.InputService.LeftButtonDown())
                                {
                                    s.nCC--; nTossPower++;
                                }
                                break;
                            case 6:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_6)]);
                                break;
                            case 7:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_7)]);
                                break;
                            case 8:
                                // CREATE AN APPLE
                                if (sprPowerMeter != null)
                                    sprPowerMeter.bAttrib[1] = true;
                                aisCreateProjectile(s.nX + 20, s.nY, (SpriteType.sprAPPLE), nTossPower);
                                if (nTossPower > 23) aisUnlockAchievement(543);
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_8)]);
                                Globals.myGameConditions.LoseApple();
                                s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armOTHROW);
                                s.nAttrib[((int)attrArm.attrArmAction)] = (Globals.myGameConditions.GetPlayerApples() > 0 ? ((int)ArmPositions.armAPPLE) : aisChangeArm());
                                s.nCC = 0;
                                break;
                        }
                        break;
                }
                break;                             /* ((int)ArmPositions.armAPPLE) */

            case ((int)ArmPositions.armPIZZA):
                s.nX = Globals.InputService.GetMouseX() - ((Globals.InputService.GetMouseX()) / 6);
                switch (s.nAttrib[((int)attrArm.attrArmAction)])
                {
                    case 0:                          // HOLDING A SLICE OF PIZZA
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND2_1)]);
                        if (Globals.InputService.LeftButtonPressed())
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 2; s.nCC = 0;
                        }
                        break;
                    case 1:                          // WAVING A PIZZA (REMOVED)
                        s.nAttrib[((int)attrArm.attrArmAction)] = 0;
                        break;
                    case 2:                          // TOSSING A PIZZA
                        switch (s.nCC)
                        {
                            case 1:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND2_2)]);
                                break;
                            case 4:
                                // CREATE PIZZA
                                aisCreateProjectile(s.nX - 30, s.nY, (SpriteType.sprPIZZA));
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND9_1)]);
                                Globals.myGameConditions.LosePizza();
                                s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armSTHROW);
                                s.nAttrib[((int)attrArm.attrArmAction)] = (Globals.myGameConditions.GetPlayerPizza() > 0 ? ((int)ArmPositions.armPIZZA) : aisChangeArm());
                                s.nCC = 0;
                                break;
                        }
                        break;
                }
                if (Globals.InputService.ToggleForwardButtonPressed())
                {
                    // SWITCH
                    s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                    s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                    s.nCC = 0;
                }
                else if (Globals.InputService.ToggleBackButtonPressed())
                {
                    // SWITCH
                    s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArmBackwards();
                    s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                    s.nCC = 0;
                }

                break;                             /* ((int)ArmPositions.armPIZZA) */
            case ((int)ArmPositions.armCLARK):
                s.nX = Globals.InputService.GetMouseX() - ((Globals.InputService.GetMouseX()) / 6);

                switch (s.nAttrib[((int)attrArm.attrArmAction)])
                {
                    case 0:                          // HOLDING A CLARK MUG
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND3_1)]);
                        if (s.nCC > 450 && 0 == R.Next(150))
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 1; s.nCC = 0;
                        }
                        if (Globals.InputService.LeftButtonPressed())
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 2; s.nCC = 0;
                        }
                        if (Globals.InputService.RightButtonPressed())
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 1; s.nCC = 0;
                        }
                        if (Globals.InputService.ToggleForwardButtonPressed())
                        {
                            // SWITCH
                            s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                            s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                            s.nCC = 0;
                        }
                        else if (Globals.InputService.ToggleBackButtonPressed())
                        {
                            // SWITCH
                            s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArmBackwards();
                            s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                            s.nCC = 0;
                        }
                        break;
                    case 1:                          // PULLIN' THE CLARK MUG BACK
                        s.nX = 640; s.nY = 480;
                        switch (s.nCC)
                        {
                            case 1:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND3_2)]);
                                break;
                            case 3:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND3_3)]);
                                break;
                            case 5:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND3_4)]);
                                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprCLOSEUPBEER), s.nX, s.nY));
                                break;
                            // Create a chugging bitmap
                            case 27:
                                AIMethods.aisUnlockAchievement(50);
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND3_3)]);
                                break;
                            case 29:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND3_2)]);
                                break;
                            case 31:
                                s.nAttrib[((int)attrArm.attrArmAction)] = 0;
                                break;
                        }
                        break;
                    case 2:                          // TOSSING A CLARK MUG
                        switch (s.nCC)
                        {
                            case 1:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND3_5)]);
                                break;
                            case 4:
                                // CREATE A FLYING BEERMUG
                                aisCreateProjectile(s.nX - 30, s.nY, (SpriteType.sprCLARK), 3);
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND9_1)]);
                                Globals.myGameConditions.LoseClark();
                                s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armSTHROW2);
                                s.nAttrib[((int)attrArm.attrArmAction)] = (Globals.myGameConditions.GetPlayerClark() > 0 ? ((int)ArmPositions.armCLARK) : aisChangeArm());
                                s.nCC = 0;
                                break;
                        }
                        break;
                }
                break;                             /* ((int)ArmPositions.armCLARK) */
            case ((int)ArmPositions.armEXAM):
                s.nX = Globals.InputService.GetMouseX() - ((Globals.InputService.GetMouseX()) / 6);
                switch (s.nAttrib[((int)attrArm.attrArmAction)])
                {
                    case 0:                          // HOLDING A 114 EXAM
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND4_1)]);
                        if (0 == R.Next(250))
                            s.nAttrib[((int)attrArm.attrArmAction)] = 1; s.nCC = 0;
                        if (Globals.InputService.LeftButtonPressed())
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 2; s.nCC = 0;
                        }
                        if (Globals.InputService.RightButtonPressed())
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 1; s.nCC = 0;
                        }
                        if (Globals.InputService.ToggleForwardButtonPressed())
                        {
                            // SWITCH
                            s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                            s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                            s.nCC = 0;
                        }
                        else if(Globals.InputService.ToggleBackButtonPressed())
                        {
                            // SWITCH
                            s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArmBackwards();
                            s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                            s.nCC = 0;
                        }
                        break;
                    case 1:                          // TAKING A LOOK AT THE EXAM
                        s.nY -= 15;
                        if (Globals.InputService.RightButtonPressed())
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 0; s.nCC = 0;
                        }
                        if (Globals.InputService.ToggleForwardButtonPressed())
                        {
                            // SWITCH
                            s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                            s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                            s.nCC = 0;
                        }
                        else if (Globals.InputService.ToggleBackButtonPressed())
                        {
                            // SWITCH
                            s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArmBackwards();
                            s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                            s.nCC = 0;
                        }
                        switch (s.nCC)
                        {
                            case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND4_2)]); break;
                            case 2: s.nY += 10; break;
                            case 3: s.nY += 5; break;
                            case 78: s.nY += 5; break;
                            case 79: s.nY += 10; break;
                            case 80: s.nAttrib[((int)attrArm.attrArmAction)] = 0; break;
                        }
                        if (Globals.InputService.LeftButtonPressed())
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = 2; s.nCC = 0;
                        }
                        break; /* Taking a look at the exam */
                    case 2:                          // TOSSING A 114 EXAM
                        AIMethods.aisUnlockAchievement(114);
                        HasTossed114Exam = true;
                        switch (s.nCC)
                        {
                            case 1:
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND4_3)]);
                                break;
                            case 4:
                                // CREATE A 114 EXAM
                                aisCreateProjectile(s.nX - 30, s.nY, (SpriteType.sprEXAM), 150);
                                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND9_1)]);
                                Globals.myGameConditions.LoseExam();
                                s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armSTHROW3);
                                s.nAttrib[((int)attrArm.attrArmAction)] = (Globals.myGameConditions.GetPlayerExam() > 0 ? ((int)ArmPositions.armEXAM) : aisChangeArm());
                                s.nCC = 0;
                                break;
                        }
                        break;
                }
                break;                         /* ((int)ArmPositions.armEXAM) */
            case ((int)ArmPositions.armGREASE):
                s.nX = Globals.InputService.GetMouseX() + s.nAttrib[((int)attrArm.attrKickback)];
                sprWaterMeter.bAttrib[1] = true;
                if (Globals.InputService.ToggleForwardButtonPressed())
                {
                    // SWITCH
                    s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                    s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                    s.nCC = 0;
                }
                else if (Globals.InputService.ToggleBackButtonPressed())
                {
                    // SWITCH
                    s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArmBackwards();
                    s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                    s.nCC = 0;
                }

                switch (s.nAttrib[((int)attrArm.attrArmAction)])
                {
                    case 0:  // HOLDING THE FIRE HOSE
                        s.nAttrib[((int)attrArm.attrKickback)] /= 2;

                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND5_1)]);
                        if (Globals.InputService.LeftButtonDown())
                        {
                            sSound[((int)ASSList.ssndWATER_HOSE)].Loop(SoundbankInfo.volNORMAL);
                            s.nAttrib[((int)attrArm.attrArmAction)] = 1; s.nCC = 0;
                        }
                        break;
                    case 1:                          // HOSING
                        AIMethods.aisUnlockAchievement(69);
                        //if (s.nX < 320)
                        s.nAttrib[((int)attrArm.attrKickback)] += (s.nX > 320 ? (2 + R.Next(2)) : -(2 + R.Next(2)));
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND5_1) + ((s.nCC / 2) % 3)]);

                        TSprite newProjectile;

                        // Note conversion between MISC layer and PIT layer.
                        newProjectile = SpriteInit.CreateSprite((SpriteType.sprGREASE), s.nX + R.Next(48) - 24, s.nY + R.Next(48) - 24 - dPALMHEIGHT + dMISCYTOPITY() + 125);
                        newProjectile.nDestX = s.nX + R.Next(12) - 6;
                        newProjectile.nDestY = Globals.InputService.GetMouseY() + dMISCYTOPITY() + R.Next(12) - 6;
                        newProjectile.nAttrib[((int)attrProjectile.attrStartX)] = s.nX;
                        newProjectile.nAttrib[((int)attrProjectile.attrStartY)] = s.nY - dPALMHEIGHT + dMISCYTOPITY() + 125;
                        newProjectile.nAttrib[((int)attrProjectile.attrPowerOfThrow)] = 15;
                        ssTossed.Include(newProjectile);

                        //Globals.myGameConditions.LoseHose();
                        if (!(Globals.InputService.LeftButtonDown()))
                        {
                            sSound[((int)ASSList.ssndWATER_HOSE)].Stop();
                            s.nAttrib[((int)attrArm.attrArmAction)] = 0; s.nCC = 0;
                        }
                        break;
                    default:
                        Globals.Debug("ARMGREASE IS MESSED UP");
                        break;
                }
                break;                             /* ((int)ArmPositions.armGREASE) (the hose) */
            case ((int)ArmPositions.armPUSH):
                break;
            case ((int)ArmPositions.armIRONRING):
                if (s.nCC < 300)
                {
                    sSound[((int)ASSList.ssndEFFECTS_CROWDMURMUR)].Stop();
                }
                //#define RINGUPSTARTTIME 375
                else if (s.nCC < 375)
                { // Force the screen to scroll up
                    if (s.nCC == 310)
                    {
                        sprFrecsL.nAttrib[((int)nattrCrowd.attrFEnergy)] = 1;
                        sprFrecsC.nAttrib[((int)nattrCrowd.attrFEnergy)] = 1;
                        sprFrecsR.nAttrib[((int)nattrCrowd.attrFEnergy)] = 1;
                    }
                    s.nX = 320; s.nY = 480;
                    Globals.myLayers.ForceScroll((s.nCC - 335) / 2);
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND7_7)]);
                    if (s.nCC >= RINGUPSTARTTIME - 20)
                        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND7_6)]);

                    s.nY -= ((s.nCC - 380) * 4);
                    if (s.nY < 480)
                        s.nY = 480;
                }
                else if (s.nCC < RINGUPSTARTTIME + 31)
                { // Shouts with fist
                    if (s.nCC == RINGUPSTARTTIME)
                    {
                        aisShoutDiscipline();
                        sprRingMeter = SpriteInit.CreateSprite((SpriteType.sprRINGMETER), 26, 17);
                        ssConsole.Include(sprRingMeter);
                    }
                    if (s.nCC == RINGUPSTARTTIME + 30)
                    {
                        sprFrecsL.nAttrib[((int)nattrCrowd.attrFEnergy)] = energySlam - 50;
                        sprFrecsC.nAttrib[((int)nattrCrowd.attrFEnergy)] = energySlam - 50;
                        sprFrecsR.nAttrib[((int)nattrCrowd.attrFEnergy)] = energySlam - 50;
                        sSound[((int)ASSList.ssndEFFECTS_CROWDMURMUR)].Loop(SoundbankInfo.volCROWD);
                    }

                    s.nX = 320; s.nY = 480;
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND7_6)]);
                }
                else if (s.nCC < RINGUPSTARTTIME + 33)
                { // Shouts with fist
                    s.nX = (320 + Globals.InputService.GetMouseX() + ((640 - Globals.InputService.GetMouseX()) / 6)) / 2;
                    s.nY = (1110 /*480 + 150 + 480 */+ ((Globals.InputService.GetMouseY()) / 5)) / 2;
                    s.nAttrib[((int)attrArm.attrArmAction)] = 0;
                    sprRingMeter.bAttrib[1] = true;
                }
                else
                { // Attack Frosh
                    AIMethods.aisUnlockAchievement(7);
                    Globals.myGameConditions.AddEnergy(4);
                    s.nX = Globals.InputService.GetMouseX() - ((Globals.InputService.GetMouseX()) / 6);
                    if (0 != s.nAttrib[((int)attrArm.attrArmAction)])
                    {
                        s.nY += 100;
                        if (s.nCC == 1005)
                        {
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND7_6)]);
                            s.nCC = 1000;
                            s.nAttrib[((int)attrArm.attrArmAction)] = 0;
                        }
                    }
                    else
                    {
                        s.nY += 150;
                        if (sprRingMeter == null)
                        {
                            s.nAttrib[((int)attrArm.attrArmAction)] = aisChangeArm();
                            s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                        }
                        else if (Globals.InputService.LeftButtonPressed())
                        {
                            lSound[((int)ASLList.lsndRING_ZAP1)].Stop();
                            lSound[((int)ASLList.lsndRING_ZAP2)].Stop();
                            lSound[((int)ASLList.lsndRING_ZAP3)].Stop();
                            lSound[((int)ASLList.lsndRING_ZAP1) + R.Next(3)].Play(SoundbankInfo.volHOLLAR, panONX(s));
                            aisIronRingZap();
                            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND7_4)]);
                            s.nAttrib[((int)attrArm.attrArmAction)] = 1;
                            s.nCC = 1000;
                        }
                    }
                }
                break;
            case ((int)ArmPositions.armOTHROW):
                switch (s.nCC)
                {
                    case 1: s.nX -= 20; s.nY += 24; break;
                    case 2: s.nX -= 70; s.nY += 60; break;
                    case 3: s.nX -= 140; s.nY += 120; break;
                    case 4: s.nX -= 200; s.nY += 140; break;
                    case 5: s.nX -= 300; s.nY += 160; break;
                    case 6:
                        s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                        s.nCC = 4; // Skip the first "dropping down" part
                        break;
                }
                break;                             /* ((int)ArmPositions.armOTHROW) */
            case ((int)ArmPositions.armSTHROW):
            case ((int)ArmPositions.armSTHROW2):
            case ((int)ArmPositions.armSTHROW3):
                switch (s.nCC)
                {
                    case 1: s.nX += 20; s.nY += 24; break;
                    case 2: s.nX += 40; s.nY += 48; break;
                    case 3: s.nX += 60; s.nY += 60; break;
                    case 4: s.nX += 72; s.nY += 72; break;
                    case 5: s.nX += 80; s.nY += 84; break;
                    case 6:
                        s.nAttrib[((int)attrArm.attrArmStatus)] = ((int)ArmPositions.armCHANGING);
                        s.nCC = 4; // Skip the first "dropping down" part
                        break;
                }
                break;
            case ((int)ArmPositions.armCHANGING):
                switch (s.nCC)
                {
                    case 1:
                        s.nY += 15; break;
                    case 10:
                        s.nAttrib[((int)attrArm.attrArmStatus)] = s.nAttrib[((int)attrArm.attrArmAction)];
                        s.nAttrib[((int)attrArm.attrArmAction)] = 0; break;
                    case 2:
                    case 9:
                        s.nY += 30; break;
                    case 3:
                    case 8:
                        s.nY += 60; break;
                    case 4:
                    case 7:
                        s.nY += 120; break;
                    case 5:
                        s.nY += 240; break;
                    case 6:
                        s.nY += 480;
                        sSound[((int)ASSList.ssndWATER_HOSE)].Stop();
                        switch (s.nAttrib[((int)attrArm.attrArmAction)])
                        {
                            case ((int)ArmPositions.armNOTHING): s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND0_1)]); break;
                            case ((int)ArmPositions.armAPPLE): s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_1)]); break;
                            case ((int)ArmPositions.armPIZZA): s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND2_1)]); break;
                            case ((int)ArmPositions.armCLARK): s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND3_1)]); break;
                            case ((int)ArmPositions.armEXAM): s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND4_1)]); break;
                            case ((int)ArmPositions.armGREASE): s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND5_1)]); break;
                            case ((int)ArmPositions.armPUSH): s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND0_1)]); break;
                            case ((int)ArmPositions.armIRONRING): s.SetFrame(frm[((int)GameBitmapEnumeration.bmpINVISIBLE)]);
                                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprARMRING2), 224 - 100, 299 + 100));
                                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprARMRING3), 245 + 100, 227 + 100));
                                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprARMRING1), 44 - 100, 327 + 100));
                                break;
                            case ((int)ArmPositions.armOTHROW): s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND1_8)]); break;
                            case ((int)ArmPositions.armSTHROW):
                            case ((int)ArmPositions.armSTHROW2):
                            case ((int)ArmPositions.armSTHROW3): s.SetFrame(frm[((int)GameBitmapEnumeration.bmpHAND9_1)]); break;
                        }
                        break;
                    default:
                        if (s.nCC > 40)
                            s.nCC = 0;
                        break;

                }
                break;
        }

    }


    public static void aiWhap(TSprite s)
    {
        // Create a "WHAP!" noise.
        if (s.nCC > timeWHAP)
            s.bDeleted = true;
    }

    public static void aiWordBubble(TSprite s)
    {
        // Create a "WHAP!" noise.
        if (s.nCC > timeBUBBLE)
            s.bDeleted = true;
    }
}