using System; 
public static partial class AIMethods
{
    public static Random R = new Random();

    // Returns a random integer in range lowest..highest
    public static int randintin(int lowest, int highest)
    {
        return R.Next(lowest, highest + 1);
//        return R.Next(highest - lowest + 1) + lowest;
    }




    public const int MAX_SSOUNDS = 500;
    public const int MAX_LSOUNDS = 500;


    public const bool INCLUDEONLYPYRAMIDFROSH = true;
    public const bool INCLUDEALLFROSH = false;

    public const bool INCLUDEWHAP = true;
    public const bool NOWHAP = false;
    public const bool POLEACTSASSHIELD = true;
    public const bool NOPOLESHIELDING = false;



    // GLOBAL VARIABLES
    public static bool gbShowFPS = false;
    public static bool gbStartAtGame = false;

    // These are NOT saved to the Registry.
    public static int gnPitTimeS = 0;   // Pit Time, Seconds
    public static int gnPitTimeM = 0;   // Pit Time, Minutes
    public static int gnPitTimeH = 0;   // Pit Time, Hours

    public static int[] nFroshLevelL = { 0, 0, 0, 0, 0, 0, 0 };
    public static int[] nFroshLevelR = { 0, 0, 0, 0, 0, 0, 0 };
    public static int[] nFroshLevel = { 0, 0, 0, 0, 0, 0, 0 };
    public static int[] nFroshTarget = {0/*unused*/, 
                       40/*base*/, 
					   15/*level 2*/, 
					   7 /* level 3 */, 
					   5 /* level 4 */, 
					   3 /* level 5 */, 
					   1 /* tam */};
    public static int nFroshTam = 0;
    public static int nFroshThinking = 0; public static int nFroshBitter = 0;
    public static int nFroshAbove1 = 0; public static int nFroshAbove3 = 0;
    public static int nFroshTotalL = 0; public static int nFroshTotalR = 0;

    // Direction convention: LEFT indicates GOING / LOOKING LEFT

    // Pointers to The SpriteSets used in the game
    public static SpriteSet ssClouds;
    public static SpriteSet ssBalloon;
    public static SpriteSet ssSkyline;
    public static SpriteSet ssTrees;
    public static SpriteSet ssFrecs;
    public static SpriteSet ssWater;
    public static SpriteSet ssPit;  // The pole, frosh, water spashing, etc.
    public static SpriteSet ssTossed; // Tossed items.
    public static SpriteSet ssConsole; public static SpriteSet ssIcons;  // Icons and the hand
    public static SpriteSet ssMouse;  // The mouse cursor
    public static SpriteSet ssJacketSlam; // The jacket ka-blam
    // Special spriteset which contains ALL and ONLY the Frosh
    public static SpriteSet ssFr;  // Ordered by X

    // Pointer to the Arm, Tam, and Pole sprites (so other sprites can change its behavior.)
    public static TSprite sprArm = null;
    public static TSprite sprTam = null;
    public static TSprite sprPole = null;
    public static TSprite sprPowerMeter = null;
    public static TSprite sprRingMeter = null;
    public static TSprite sprWaterMeter = null;
    public static TSprite sprFrecsL = null;
    public static TSprite sprFrecsC = null;
    public static TSprite sprFrecsR = null;
    public static TSprite sprAlien = null;
    public static TSprite sprPrez = null;
    public static TSprite sprPopBoy = null;
    public static TSprite sprForge = null;
    public static TSprite sprRandomEventGenerator = null;
    public static TSprite sprGWBalloon = null;
    public static TSprite sprGWHippo = null;
    public static TSprite sprFPS1 = null;
    public static TSprite sprFPS0 = null;

    // Pointers to the frames that contain the game graphics and sounds
    public static FrameDesc[] frm = new FrameDesc[(int)GameBitmapEnumeration.bmpENDOFBITMAPS];
    public static FrameDesc[] frmM = new FrameDesc[(int)GameBitmapEnumeration.bmpENDOFBITMAPS];
    public static IStaticSound[] sSound = new IStaticSound[MAX_SSOUNDS];
    public static IStreamedSound[] lSound = new IStreamedSound[MAX_LSOUNDS];


    //*******************************************************
    // Support Function Definitions
    //*******************************************************
    private static SpriteSet TargetsInScrRange = null;

    // Uses define INCLUDEONLYPYRAMIDFROSH and INCLUDEALLFROSH 
    public static SpriteSet GetTargetsInScrRange(SpriteSet ssTargets, int nLeftOfRange, int nTopOfRange, int nRightOfRange, int nBotOfRange, bool bPoleOnly)
    {
        // Returns the Frosh within the SCREENspace outlined by the two points passed in.
        // VERY IMPORANT: Use delete FroshInRange when you're finished with the function.
        // Forgetting to do so will waste memory.
        int nMidYOfRange = (nTopOfRange + nBotOfRange) / 2;
        int nMidXOfRange = (nLeftOfRange + nRightOfRange) / 2;
        int nTopOfSprite, nBotOfSprite, nLeftOfSprite, nRightOfSprite;

        if (TargetsInScrRange == null)
        {
            TargetsInScrRange = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERPIT)));
        }
        else
        {
            TargetsInScrRange.RemoveAll();
        }

        int i = 0;
        int n = ssTargets.GetNumberOfSprites();
        TSprite him;

        // Precondition: All Frosh < i can't possibly be in the set.
        // Invariant: All Frosh < i that should be in the set ARE in the set.
        while (i < n)
        {
            // Add the Frosh to the set
            him = ssTargets.GetSprite(i);
            nLeftOfSprite = ssTargets.GetLeftMostScrPointOnSprite(i);
            nTopOfSprite = ssTargets.GetTopMostScrPointOnSprite(i);
            nRightOfSprite = ssTargets.GetRightMostScrPointOnSprite(i);
            nBotOfSprite = ssTargets.GetBottomMostScrPointOnSprite(i);
            if ((nTopOfRange > nTopOfSprite && nTopOfRange < nBotOfSprite)
                || (nMidYOfRange > nTopOfSprite && nMidYOfRange < nBotOfSprite)
                || (nBotOfRange > nTopOfSprite && nBotOfRange < nBotOfSprite))
                if ((nLeftOfRange > nLeftOfSprite && nLeftOfRange < nRightOfSprite)
                    || (nMidXOfRange > nLeftOfSprite && nMidXOfRange < nRightOfSprite)
                    || (nRightOfRange > nLeftOfSprite && nRightOfRange < nRightOfSprite))
                    if ((!bPoleOnly) || !(him.nAttrib[((int)nattrFrosh.attrBehavior)] < 7))
                        TargetsInScrRange.Include(him);
            i++;
        }

        return TargetsInScrRange;
    }


    public static SpriteSet GetFroshInRange(int nLeftOfRange, int nTopOfRange,
                                int nRightOfRange, int nBotOfRange)
    {
        return GetSpritesInRange(nLeftOfRange, nTopOfRange, nRightOfRange, nBotOfRange, ssFr);
    }

    public static SpriteSet GetSpritesInRange(int nRangeL, int nRangeT,
                              int nRangeR, int nRangeB,
                              SpriteSet ssTarget)
    {
        return GetSpritesInRange(nRangeL, nRangeT, nRangeR, nRangeB, ssTarget, true); // Defaults to rigorous = true
    }

    private static SpriteSet SpritesInRange;

    // THIS FUNCTION ISN"T GOOD AT FINDING SMALL THINGS
    public static SpriteSet GetSpritesInRange(int nRangeL, int nRangeT,
                                  int nRangeR, int nRangeB,
                                  SpriteSet ssTarget, bool bRigorous)
    {
        // Returns the Sprites within the FROSHspace outlined by the two points passed in.
        // VERY IMPORANT: Use delete FroshInRange when you're finished with the function.
        // Forgetting to do so will waste memory.
        int nRangeMX = (nRangeL + nRangeR) / 2;
        int nRangeMY = (nRangeT + nRangeB) / 2;

        int nSpriteL, nSpriteT, nSpriteR, nSpriteB;
        int nSpriteMX;// = (nSpriteL + nSpriteR) / 2;
        int nSpriteMY;// = (nSpriteT + nSpriteB) / 2;

        if (SpritesInRange == null)
        {
            SpritesInRange = new SpriteSet(Globals.myLayers.GetLayer(((int)LayerNames.LAYERPIT)));
        }
        else
        {
            SpritesInRange.RemoveAll();
        }

        int i = 0;
        int n = ssTarget.GetNumberOfSprites();
        TSprite him;

        // Precondition: All Frosh < i can't possibly be in the set.
        // Invariant: All Frosh < i that should be in the set ARE in the set.
        while (i < n)
        {
            // Add the Frosh to the set
            him = ssTarget.GetSprite(i);
            nSpriteL = ssTarget.GetLeftMostPointOnSprite(i);
            nSpriteT = ssTarget.GetTopMostPointOnSprite(i);
            nSpriteR = ssTarget.GetRightMostPointOnSprite(i);
            nSpriteB = ssTarget.GetBottomMostPointOnSprite(i);
            nSpriteMX = (nSpriteL + nSpriteR) / 2;
            nSpriteMY = (nSpriteT + nSpriteB) / 2;

            if ((nRangeT > nSpriteT && nRangeT < nSpriteB)
                || (nRangeMY > nSpriteT && nRangeMY < nSpriteB)
                || (nRangeB > nSpriteT && nRangeB < nSpriteB))
            {
                if ((nRangeL > nSpriteL && nRangeL < nSpriteR)
                    || (nRangeMX > nSpriteL && nRangeMX < nSpriteR)
                    || (nRangeR > nSpriteL && nRangeR < nSpriteR))
                {
                    SpritesInRange.Include(him);
                }
            }
            if (bRigorous)
                if ((nSpriteT > nRangeT && nSpriteT < nRangeB)
                    || (nSpriteMY > nRangeT && nSpriteMY < nRangeB)
                    || (nSpriteB > nRangeT && nSpriteB < nRangeB))
                {
                    if ((nSpriteL > nRangeL && nSpriteL < nRangeR)
                        || (nSpriteMX > nRangeL && nSpriteMX < nRangeR)
                        || (nSpriteR > nRangeL && nSpriteR < nRangeR))
                    {
                        SpritesInRange.Include(him);
                    }
                }
            i++;
        }

        return SpritesInRange;
    }


    public static bool aisScrPointInside(TSprite s, int x, int y)
    {
        // Determines if a screen coordinate (x, y) is inside the area of the
        // screen on which this sprite is displayed.

        int nTopLeftX = s.nScrx + s.frmFrame.nHotSpotX - s.frmFrame.nX1;
        int nTopLeftY = s.nScry + s.frmFrame.nHotSpotY - s.frmFrame.nZ1;
        int nBottomRightX = s.nScrx + s.frmFrame.nHotSpotX + s.frmFrame.nX2;
        int nBottomRightY = s.nScry + s.frmFrame.nHotSpotY + s.frmFrame.nZ2;

        if (x >= nTopLeftX && x <= nBottomRightX &&
            y >= nTopLeftY && y <= nBottomRightY)
            return true;
        return false;
    }

    public static bool aisMouseOver(TSprite s)
    {
        return aisScrPointInside(s, Globals.InputService.GetMouseX(), Globals.InputService.GetMouseY());
    }

    public static void aisSendFroshFlying(TSprite s)
    {
        s.SetBehavior(aiAct1);

        s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 0;
        s.SetGoal(((int)Goals.goalMINDLESS_WANDERING));
        s.nDestX = randintin(dPITMINX, dPITMAXX);
        s.nDestY = randintin(dPITMINY, dPITMAXY);
    }

    public static void aisSendFroshReallyFlying(TSprite s)
    {
        aisSendFroshFlying(s);
        aiInit1(s);
        s.nAttrib[(int)nattrFrosh.attrHeightOfFall] = s.nZ;
    }

    public static bool aisConsume(TSprite s)
    {
        return aisConsume(s, false); // beer defaults to false. What was I thinking?!
    }
    public static bool aisConsume(TSprite s, bool bIsBeer)
    {
        SpriteSet ssTossedObjectsL;
        SpriteSet ssTossedObjectsR;

        ssTossedObjectsL = GetSpritesInRange(s.nX - dPIZZAEATINGOFFSETX - 3, s.nY - 2,
            s.nX - dPIZZAEATINGOFFSETX + 3, s.nY + 6, ssTossed);
        ssTossedObjectsR = GetSpritesInRange(s.nX + dPIZZAEATINGOFFSETX - 3, s.nY - 2,
            s.nX + dPIZZAEATINGOFFSETX + 3, s.nY + 6, ssTossed);

        int nL = ssTossedObjectsL.GetNumberOfSprites();
        int nR = ssTossedObjectsR.GetNumberOfSprites();

        bool bResult;

        if (nL + nR > 0)
        {
            bResult = true;
            if (nL > 0)
            {
                s.bAttrib[((int)battrFrosh.attrLookingLeft)] = true;
                if (bIsBeer)
                    aisAbandonClark(ssTossedObjectsL.GetSprite(0));
                else
                    aisAbandonPizza(ssTossedObjectsL.GetSprite(0));
                ssTossedObjectsL.GetSprite(0).bDeleted = true;
            }
            else
            {
                s.bAttrib[((int)battrFrosh.attrLookingLeft)] = false;
                ssTossedObjectsR.GetSprite(0).bDeleted = true;
                if (bIsBeer)
                    aisAbandonClark(ssTossedObjectsR.GetSprite(0));
                else
                    aisAbandonPizza(ssTossedObjectsR.GetSprite(0));
            }
        }
        else
            bResult = false;

        //delete ssTossedObjectsL;
        //delete ssTossedObjectsR;
        return bResult;
    }


    //#define INCLUDEWHAP true, NOWHAP false, POLEACTSASSHIELD true, NOPOLESHIELDING false
    public static void aisIronRingZap()
    {
        SpriteSet ssHitFrosh = GetTargetsInScrRange(ssFr,
            Globals.InputService.GetMouseX() - 15, Globals.InputService.GetMouseY() - 15,
            Globals.InputService.GetMouseX() + 15, Globals.InputService.GetMouseY() + 15, INCLUDEALLFROSH);

        int n = ssHitFrosh.GetNumberOfSprites();

        TSprite him;
        int i;
        if (n > 0)
        {
            for (i = 0; i < n; i++)
            {
                him = ssHitFrosh.GetSprite(i);
                if (R.Next(2) == 1)
                    aiInit6E(him);
                else
                    aiInit6F(him);
            }
        }
        //delete ssHitFrosh;
    }

    public static bool aisCollisionToResponse(TSprite projectile, SpriteSet ssTargets,
                                AIMethod pfAIResponse, bool bIncludeWhap,
                                bool bPoleActsAsShield)
    {
        return aisCollisionToResponse(projectile, ssTargets,
                                pfAIResponse, bIncludeWhap,
                                bPoleActsAsShield, true);
    }

    //#define INCLUDEWHAP true, NOWHAP false, POLEACTSASSHIELD true, NOPOLESHIELDING false
    public static bool aisCollisionToResponse(TSprite projectile, SpriteSet ssTargets,
                                AIMethod pfAIResponse, bool bIncludeWhap,
                                bool bPoleActsAsShield, bool bHitOnlyOne)
    {
        bool bDidIHitAnything = false;
        projectile.CalculateScreenCoordinates(Globals.myLayers.GetOffset(((int)LayerNames.LAYERPIT)));
        SpriteSet ssHitFrosh = GetTargetsInScrRange(ssTargets,
            projectile.nScrx + projectile.frmFrame.nHotSpotX - projectile.frmFrame.nX1,
            projectile.nScry + projectile.frmFrame.nHotSpotY - projectile.frmFrame.nZ1,
            projectile.nScrx + projectile.frmFrame.nHotSpotX + projectile.frmFrame.nX2,
            projectile.nScry + projectile.frmFrame.nHotSpotY + projectile.frmFrame.nZ2,
            INCLUDEALLFROSH);

        int n = ssHitFrosh.GetNumberOfSprites();

        TSprite him;
        int i;
        if (n > 0)
        {
            if (bHitOnlyOne)
            {
                ssHitFrosh.OrderByY();
                him = ssHitFrosh.GetSprite(n - 1);

                if (!(bPoleActsAsShield && him.nY < dPOLEY
                    && projectile.nScrx > (dPOLEX - dPOLEWIDTH)
                    && projectile.nScrx < (dPOLEX + dPOLEWIDTH)))
                {

                    bDidIHitAnything = true;
                    if (bIncludeWhap)
                        ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprWHAP),
                        projectile.nScrx + projectile.frmFrame.nHotSpotX,
                        projectile.nScry + projectile.frmFrame.nHotSpotY));
                    // PERFORMANCE BOOST 0: Do AIResponse to the Frosh
                    if (projectile.nAttrib[((int)attrProjectile.attrPowerOfThrow)] > R.Next(Globals.myGameConditions.GetBooster(0)))
                        pfAIResponse(him);
                    //else
                    //    ;/// OOH!  THE FROSH WAS TOO STRONG!
                    if (him.nZ <= dBELLYBUTTONZ)
                        ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), him.nX, him.nY));
                }
            }
            else
            {
                for (i = 0; i < n; i++)
                {
                    him = ssHitFrosh.GetSprite(i);
                    if (!(bPoleActsAsShield && him.nY < dPOLEY
                        && projectile.nScrx > (dPOLEX - dPOLEWIDTH)
                        && projectile.nScrx < (dPOLEX + dPOLEWIDTH)))
                    {

                        bDidIHitAnything = true;
                        if (bIncludeWhap)
                            ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprWHAP),
                            projectile.nScrx + projectile.frmFrame.nHotSpotX,
                            projectile.nScry + projectile.frmFrame.nHotSpotY));
                        // PERFORMANCE BOOST 0: Do AIResponse to the Frosh
                        if (projectile.nAttrib[((int)attrProjectile.attrPowerOfThrow)] > R.Next(Globals.myGameConditions.GetBooster(0)))
                            pfAIResponse(him);
                        //else
                        //    ;/// OOH!  THE FROSH WAS TOO STRONG!
                    }

                }
            }
        }

        //delete ssHitFrosh;
        return bDidIHitAnything;
    }

    public static bool aisSpecialCollisionToResponse(TSprite projectile, SpriteSet ssTargets,
                                AIMethod pfAIResponse, bool bIncludeWhap,
                                bool bPoleActsAsShield, bool bHitOnlyOne, AIMethod pfAISpecialResponse1,
                                AIMethod pfAISpecialResponse2)
    {
        bool bDidIHitAnything = false;
        projectile.CalculateScreenCoordinates(Globals.myLayers.GetOffset(((int)LayerNames.LAYERPIT)));
        SpriteSet ssHitFrosh = GetTargetsInScrRange(ssTargets,
            projectile.nScrx + projectile.frmFrame.nHotSpotX - projectile.frmFrame.nX1,
            projectile.nScry + projectile.frmFrame.nHotSpotY - projectile.frmFrame.nZ1,
            projectile.nScrx + projectile.frmFrame.nHotSpotX + projectile.frmFrame.nX2,
            projectile.nScry + projectile.frmFrame.nHotSpotY + projectile.frmFrame.nZ2,
            INCLUDEALLFROSH);

        int n = ssHitFrosh.GetNumberOfSprites();

        TSprite him;
        int i;
        if (n > 0)
        {
            if (bHitOnlyOne)
            {
                ssHitFrosh.OrderByY();
                him = ssHitFrosh.GetSprite(n - 1);

                if (!(bPoleActsAsShield && him.nY < dPOLEY
                    && projectile.nScrx > (dPOLEX - dPOLEWIDTH)
                    && projectile.nScrx < (dPOLEX + dPOLEWIDTH)))
                {

                    bDidIHitAnything = true;
                    // PERFORMANCE BOOST 0: Do AIResponse to the Frosh
                    if (him.nAttrib[((int)nattrFrosh.attrBehavior)] == 11 || him.nAttrib[((int)nattrFrosh.attrBehavior)] == 10)
                    {
                        projectile.bDeleted = true; projectile.nX += 640;
                        pfAISpecialResponse1(him);
                    }
                    else if (him.nAttrib[((int)nattrFrosh.attrBehavior)] == 4)
                    {
                        projectile.bDeleted = true; projectile.nX += 640;
                        pfAISpecialResponse2(him);
                    }
                    else
                    {
                        if (bIncludeWhap)
                            ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprWHAP),
                            projectile.nScrx + projectile.frmFrame.nHotSpotX,
                            projectile.nScry + projectile.frmFrame.nHotSpotY));
                        aisProjectileRebound(projectile);
                        if (projectile.nAttrib[((int)attrProjectile.attrPowerOfThrow)] > R.Next(Globals.myGameConditions.GetBooster(0)))
                            pfAIResponse(him);
                        //else
                        //    ;/// OOH!  THE FROSH WAS TOO STRONG!
                    }
                    if (him.nZ <= dBELLYBUTTONZ)
                        ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), him.nX, him.nY));
                }
            }
            else
            {
                for (i = 0; i < n; i++)
                {
                    him = ssHitFrosh.GetSprite(i);
                    if (!(bPoleActsAsShield && him.nY < dPOLEY
                        && projectile.nScrx > (dPOLEX - dPOLEWIDTH)
                        && projectile.nScrx < (dPOLEX + dPOLEWIDTH)))
                    {

                        bDidIHitAnything = true;
                        // PERFORMANCE BOOST 0: Do AIResponse to the Frosh
                        if (him.nAttrib[((int)nattrFrosh.attrBehavior)] == 11 || him.nAttrib[((int)nattrFrosh.attrBehavior)] == 10)
                        {
                            projectile.bDeleted = true; projectile.nX += 640;
                            pfAISpecialResponse1(him);
                        }
                        if (him.nAttrib[((int)nattrFrosh.attrBehavior)] == 4)
                        {
                            projectile.bDeleted = true; projectile.nX += 640;
                            pfAISpecialResponse2(him);
                        }
                        else
                        {
                            if (bIncludeWhap)
                                ssIcons.Include(SpriteInit.CreateSprite((SpriteType.sprWHAP),
                                projectile.nScrx + projectile.frmFrame.nHotSpotX,
                                projectile.nScry + projectile.frmFrame.nHotSpotY));
                            if (projectile.nAttrib[((int)attrProjectile.attrPowerOfThrow)] > R.Next(Globals.myGameConditions.GetBooster(0)))
                                pfAIResponse(him);
                            //else
                            //    ;/// OOH!  THE FROSH WAS TOO STRONG!
                        }
                    }
                }
            }
        }

        //delete ssHitFrosh;
        return bDidIHitAnything;
    }


    public static bool aisForgeTrick(int nTrick, int nEnergy)
    {
        if (Globals.myGameConditions.UseTrick(nTrick))
        {
            sprForge.nAttrib[((int)nattrForge.attrForgeEnergy)] += nEnergy;
            return true;
        }
        else return false;
    }


    public static void aisShoutDiscipline()
    {
        // HOLLAR
        int i;
        int nNextBar;
        int nThingToShout = ((int)ASLList.lsndDISCIPLINES_DEFAULT);

        for (i = NUM_JBARSPOTS - 1; i >= 0; i--)
        {
            nNextBar = Globals.myGameConditions.GetJBar(i);
            switch (nNextBar)
            {
                case 0: nThingToShout = ((int)ASLList.lsndDISCIPLINES_APPLE); break;
                case 1: nThingToShout = ((int)ASLList.lsndDISCIPLINES_CHEM);
                    if (i > 0 && Globals.myGameConditions.GetJBar(i - 1) == 10)
                        nThingToShout = ((int)ASLList.lsndDISCIPLINES_ENGCHEM);
                    break;
                case 2: nThingToShout = ((int)ASLList.lsndDISCIPLINES_CIVIL); break;
                case 3: nThingToShout = ((int)ASLList.lsndDISCIPLINES_ELEC); break;
                case 5: nThingToShout = ((int)ASLList.lsndDISCIPLINES_ENGCHEM); break;
                case 4: nThingToShout = ((int)ASLList.lsndDISCIPLINES_ENGPHYS); break;
                case 6: nThingToShout = ((int)ASLList.lsndDISCIPLINES_GEO); break;
                case 7: nThingToShout = ((int)ASLList.lsndDISCIPLINES_METALS); break;
                case 8:
                case 15: nThingToShout = ((int)ASLList.lsndDISCIPLINES_MECH); break;
                case 9: nThingToShout = ((int)ASLList.lsndDISCIPLINES_MINING); break;
                case 17: nThingToShout = ((int)ASLList.lsndDISCIPLINES_METALS); break;
            }
        }
        lSound[nThingToShout].Play(SoundbankInfo.volHOLLAR);
    }

    public static void aisChooseNewPersonality(TSprite s)
    {
        // Choose a new personality for this frosh.
        s.nAttrib[((int)nattrFrosh.attrPersonality)] = R.Next(nFROSHPERSONALITIES);
    }


    public static void aisGameXYtoPitXY(int nGameX, int nGameY, out int nPitX, out int nPitY)
    {
        nPitX = nGameX;
        nPitY = nGameY + Globals.myLayers.GetOffset(((int)LayerNames.LAYERPIT));
    }

    public static bool aisPickPyramidSpot(TSprite s)
    {
        // Chooses a Pyramid Spot and assigns it as the destination in s.nDestX,Y
        // Returns FALSE if no pyramid spot is available
        int nChain = R.Next(PolePosition.nPPChains);
        int nDebugCheck = 0;

        PolePosition ppMySpot = null;
        while (ppMySpot == null)
        {
            ppMySpot = PolePosition.PoleChains[nChain].FirstFreeChild();
            nChain++;
            nChain %= PolePosition.nPPChains;
            nDebugCheck++;
            if (nDebugCheck > PolePosition.nPPChains)
            {
                //			Globals.Debug("aisPickPyramidSpot: You need more Pole Positions.");
                return false;
            }
        }

        s.ppChosen = ppMySpot;
        s.ppChosen.SetClaim(s);
        s.nDestX = ppMySpot.GetX(); s.nDestY = ppMySpot.GetY();
        return true;

    }

    public static bool aisPickCloserPyramidSpot(TSprite s)
    {
        // Takes one look for a Pyramid Spot closer than the one you're at and assigns it as the 
        // destination in s.nDestX,Y.

        PolePosition ppPotentialSpot = s.ppChosen.AdjacentChain().FirstFreeChild();
        if (ppPotentialSpot == null)
        {
            return false;  // All the spots in the adjacent chain are, in fact, TAKEN.
        }

        if (ppPotentialSpot.nOrdinal < s.ppChosen.nOrdinal)
        { // the next spot is closer to the pole 
            //		Globals.Debug("Aha!  I can move closer to the pole.");
            s.ppChosen.ReleaseClaim();
            s.ppChosen = ppPotentialSpot;
            ppPotentialSpot.SetClaim(s);
            s.nDestX = ppPotentialSpot.GetX(); s.nDestY = ppPotentialSpot.GetY();
            return true;
        }
        return false;
    }



    public static void aisPickHoistingSpot(TSprite s)
    {
    }

    public static bool aisPickClimbingSpot(TSprite s)
    {
        // Chooses a Climbing Spot and assigns it as the destination in s.nDestX,Y
        // Returns FALSE if no spot is available
        int nChain = R.Next(PolePosition.nPPChains);
        int nDebugCheck = 0;

        PolePosition ppMySpot = null;
        while (ppMySpot == null)
        {
            ppMySpot = PolePosition.PoleChains[R.Next(PolePosition.nPPChains)].LastTakenChild();
            nChain++;
            nChain %= PolePosition.nPPChains;
            nDebugCheck++;
            if (nDebugCheck > PolePosition.nPPChains)
            {
                //			Globals.Debug("aisPickClimbingSpot: You need more Pole Positions.");
                return false;
            }
        }

        s.nDestX = ppMySpot.GetX();
        s.nDestY = ppMySpot.GetY() + (ppMySpot.GetY() > dPOLEY ? 3 : (-3));
        return true;
    }


    public static void aisChooseFroshPitGoal(TSprite s)
    {
        // Choose a new goal for a Frosh wading in the pit.

        s.SetGoal(((int)Goals.goalMINDLESS_WANDERING));

        // On occasion, change the personality of the Frosh
        if ((0 == R.Next(10)))
            aisChooseNewPersonality(s);

        switch (s.nAttrib[((int)nattrFrosh.attrPersonality)])
        {
            case ((int)Personalities.persGoofy):
                TSprite targetFrosh;
                targetFrosh = ssFr.GetSprite(R.Next(gnNumFroshInPit));
                s.nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalMINDLESS_WANDERING);
                s.nDestX = targetFrosh.nX - dSPLASHINGDISTANCE;
                s.nDestY = targetFrosh.nY;
                break;

            case ((int)Personalities.persHeavyweight):
                if (aisPickPyramidSpot(s))
                    s.SetGoal(((int)Goals.goalPYRAMID_SPOT));
                break;

            case ((int)Personalities.persHoister):
            case ((int)Personalities.persClimber):
                if (aisPickClimbingSpot(s))
                    s.SetGoal(((int)Goals.goalCLIMBING_UP));
                break;
        }
    }

    public static void aisChooseFroshUpperLevelGoal(TSprite s)
    {
        // Assign an "upper-level goal" for this frosh.  Done typically in aiInit10, just as the Frosh
        // begin walking along a new level
        if (s.nZ > 240)
        {
            s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] = ((int)UpperLevelGoals.upperGoalClimb);
        }
        else
        {
            if (R.Next(15) < 11)
                s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] = ((int)UpperLevelGoals.upperGoalClimb);
            else if (R.Next(2) != 0)
                s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] = ((int)UpperLevelGoals.upperGoalCling);
            else
                s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] = ((int)UpperLevelGoals.upperGoalSupport);
        }
    }


    public static void aisWeightOnShoulders(TSprite s)
    {
        if (0 == R.Next(20))
        {
            s.bAttrib[((int)battrFrosh.attrWeightOnShoulders)] = true;
            if (s.nZ < 30)
            {
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR7B_1)]);
                if (0 ==R.Next(5))
                {
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR7B_2)]);
                    // PERFORMANCE BOOST: STRENGTH: FROSH CAN HANDLE WEIGHT ON THEIR SHOULDERS
                    // START AT 10, GROW TOWARDS 1000+ AS t.INFINITY
                    if (0 == R.Next(Globals.myGameConditions.GetBooster(1)))
                    {
                        aisSendFroshFlying(s);
                    }
                }
            }
        }
    }

    public static void aisCreateProjectile(int nX, int nY, SpriteType sprTYPE)
    {
        aisCreateProjectile(nX, nY, sprTYPE, 1);
    }

    public static void aisCreateProjectile(int nX, int nY, SpriteType sprTYPE, int nTossPower)
    {
        TSprite newProjectile;

        // Note conversion between MISC layer and PIT layer.
        newProjectile = SpriteInit.CreateSprite(sprTYPE, nX, nY - dPALMHEIGHT + dMISCYTOPITY());
        newProjectile.nDestX = Globals.InputService.GetMouseX();
        newProjectile.nDestY = Globals.InputService.GetMouseY() + dMISCYTOPITY();
        newProjectile.nAttrib[((int)attrProjectile.attrStartX)] = newProjectile.nX;
        newProjectile.nAttrib[((int)attrProjectile.attrStartY)] = newProjectile.nY;
        newProjectile.nAttrib[((int)attrProjectile.attrPowerOfThrow)] = nTossPower;
        ssTossed.Include(newProjectile);

    }


    public static void aisKeepInPitX(TSprite s)
    {
        // Keeps a sprite within (dPITMINX..dPITMAXX), and reverses the x-velocity if a boundary has been hit.
        if (s.nX > dPITMAXXPLUS50)
        {
            s.nX = dPITMAXXPLUS50;
            s.nvX = -(s.nvX);
        }
        if (s.nX < dPITMINXMINUS50)
        {
            s.nX = dPITMINXMINUS50;
            s.nvX = -(s.nvX);
        }
    }

    public static void aisKeepInPitY(TSprite s)
    {
        // Keeps a sprite within (dPITMINY..dPITMAXY), and reverses the y-velocity if a boundary has been hit.
        if (s.nY > dPITMAXYPLUS30)
        {
            s.nY = dPITMAXYPLUS30;
            s.nvY = -(s.nvY);
        }
        if (s.nY < dPITMINY)
        {
            s.nY = dPITMINY;
            s.nvY = -(s.nvY);
        }
    }

    public static void aisPlummet(TSprite s)
    {
        // Move
        s.nX += s.nvX; s.nY += s.nvY; s.nZ += s.nvZ;
        // Accelerate in the z-direction
        s.nvZ -= dGRAVCONST;
    }

    public static void aisMoveTowardsDestination(TSprite s)
    {
        // Move the sprite at its velocity towards its destination

        if (Math.Abs(s.nX - s.nDestX) > s.nvX)
            if (s.nX < s.nDestX)
                s.nX += s.nvX;
            else
                s.nX -= s.nvX;

        if (Math.Abs(s.nY - s.nDestY) > s.nvY)
            if (s.nY < s.nDestY)
                s.nY += s.nvY;
            else
                s.nY -= s.nvY;
    }

    public const int topplePOPBOYLAME = 2;
    public const int topplePOPBOYKEEN = 1;

    public static void aisTopplePyramid()
    {
        // Send all of the frosh flying
        int i;
        Globals.myGameConditions.Topple();
        if (Globals.myGameConditions.GetNoiseCount() > 1000)
            Globals.myGameConditions.SetNoiseCount(4000);

        // The actual toppling stuff
        int nTemp;
        TSprite s = null;
        int n = ssFr.GetNumberOfSprites();
        sSound[((int)ASSList.ssndEFFECTS_TOPPLE)].Play(SoundbankInfo.volHOLLAR);
        for (i = 0; i < n; i++)
        {
            nTemp = ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)];
            if (nTemp == 7 || nTemp == 10 || nTemp == 11)
            {
                s = ssFr.GetSprite(i);
                aisSendFroshFlying(s);
                // One in every ten will leap
                if (0 == R.Next(10))
                {
                    aiInit2(s);
                    if (s.nX < dPOLEX)
                    {
                        s.nvX = -s.nvX;
                        s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR2_1) + R.Next(nsprFR2)]);
                    }
                }
                // One in every six will look shocked
                else if (0 == R.Next(6))
                {
                    s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR1_1) + R.Next(nsprFR1)]);
                }

                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalTHINK);
                if (0 == (R.Next(300) % 3))
                    ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrPersonality)] = ((int)Personalities.persHeavyweight);
            }
        }

        // Decide if Pop Boy is coming in and usher him in if so.
        if (Globals.myGameConditions.CountTopples() == ((0 != Globals.myGameConditions.GetFroshLameness()) ? topplePOPBOYKEEN : topplePOPBOYLAME))
        {
            for (i = ((int)ASLList.lsndPREZ_HITAPPLE1); i < ((int)ASLList.lsndPREZ_ENCOURAGE5); i++)
                lSound[i].Stop();

            lSound[((int)ASLList.lsndEXAM_TOSS1)].Stop();
            NOSPEECHFOR(160);
            for (i = ((int)ASLList.lsndAPPLES_OFFER1); i < ((int)ASLList.lsndRING_ZAP3); i++)
                lSound[i].Stop();
            sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
            lSound[((int)ASLList.lsndPREZ_POPBOY1_1) + R.Next(3)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
            aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faPart));
            ssSkyline.Include(SpriteInit.CreateSprite((SpriteType.sprPOPBOYINCROWD), 530, 0));
        }
        else
        {
            if (Globals.myGameConditions.IsPopBoyInPit() && (0 != R.Next(2)) && !lSound[((int)ASLList.lsndPOPBOY_EXAM1)].IsPlaying() && !lSound[((int)ASLList.lsndPOPBOY_EXAM2)].IsPlaying() && !lSound[((int)ASLList.lsndPOPBOY_EXAM3)].IsPlaying())
            {
                // Pop Boy has his say
                aisThinkForAl(s);
                sprPopBoy.nAttrib[((int)nattrFrosh.attrBehavior)] = 5; sprPopBoy.nCC = 0;
            }
            else
            {
                // George gets his say
                if (!(lSound[((int)ASLList.lsndEXAM_TOSS1)].IsPlaying()) && SPEECHOK()
                    && sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] != 4 && (0 != Globals.myGameConditions.CountTopples() % 2))
                {

                    NOSPEECHFOR(120);
                    sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 4;
                    for (i = ((int)ASLList.lsndPREZ_HITAPPLE1); i < ((int)ASLList.lsndPREZ_HITPIZZAR2); i++)
                        lSound[i].Stop();
                    lSound[((int)ASLList.lsndPREZ_ENCOURAGE1) + (((Globals.myGameConditions.CountTopples() - 1) / 2) % 5)].Play(SoundbankInfo.volHOLLAR, (sprPrez.nX - 320) / 32);
                }
            }
        }
    }

    public static void aisStartAMosh()
    {
        if (!(sprAlien == null))
            return;
        // Send all of the frosh flying
        int i;
        int nTemp;
        int n = ssFr.GetNumberOfSprites();

        lSound[((int)ASLList.lsndMUSIC_SCOTLAND)].Play(SoundbankInfo.volFULL);

        aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faStayinAlive));
        aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faStayinAlive));
        aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faStayinAlive));

        for (i = 0; i < n; i++)
        {
            nTemp = ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)];
            if (nTemp == 7 || nTemp == 10 || nTemp == 11)
            {
                aisSendFroshFlying(ssFr.GetSprite(i));
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalMOSH);
            }
            else
            {
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalMOSH);
                ssFr.GetSprite(i).nDestX = ssFr.GetSprite(i).nX;
                ssFr.GetSprite(i).nDestY = ssFr.GetSprite(i).nY;
                if (ssFr.GetSprite(i).nZ < 5)
                {
                    ssFr.GetSprite(i).nZ = 0;
                    aiInit4(ssFr.GetSprite(i));
                }
                else
                    aisSendFroshFlying(ssFr.GetSprite(i));
            }
            ssFr.GetSprite(i).nCC = 0;
        }
    }

    public static void aisAbandonAlien()
    {
        // THIS IS A BLAST OF AISABANDONPIZZA
        int i;
        int nBehavior;
        int nGoal;
        int n = ssFr.GetNumberOfSprites();
        int nNewGoal;
        if (R.Next(2) != 0)
            nNewGoal = ((int)Goals.goalMINDLESS_WANDERING);
        else
            nNewGoal = ((int)Goals.goalTHINK);

        for (i = 0; i < n; i++)
        {
            nBehavior = ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)];
            nGoal = ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)];
            if (nGoal == ((int)Goals.goalARTSCI) || nGoal == ((int)Goals.goalCOMMIE))
            {
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrPersonality)] = ((int)Personalities.persHeavyweight);
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = nNewGoal;
                ssFr.GetSprite(i).nDestX = ssFr.GetSprite(i).nX;
                ssFr.GetSprite(i).nDestY = ssFr.GetSprite(i).nY;
            }
        }
    }


    public static void aisChaseAlien(bool bIsCommie)
    {
        // THIS IS A BLAST OF AISCHASEPIZZA
        int i;
        int nTemp;
        int nMotivationLevel;
        int n = ssFr.GetNumberOfSprites();

        for (i = 0; i < n; i++)
        {
            // First, assess if the Frosh is in the chasing mood
            nMotivationLevel = ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrMotivation)];
            if (nMotivationLevel == 2
                || (nMotivationLevel == 1 && (0 == R.Next(4)))
                || /* nMotivationLevel == 0 && */ (0 == R.Next(10)))
            {
                nTemp = ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)];
                if ((0 != nMotivationLevel) && (0 == R.Next(20)))
                    ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)]--;
                if (nTemp == 7 || nTemp == 10 || nTemp == 11)
                {
                    if (bIsCommie)
                        ssFr.GetSprite(i).nDestX = ((sprAlien.nX < ssFr.GetSprite(i).nX) ? sprAlien.nX - dCOMMIEPUNCHINGOFFSETX : sprAlien.nX + dCOMMIEPUNCHINGOFFSETX);
                    else
                        ssFr.GetSprite(i).nDestX = ((sprAlien.nX < ssFr.GetSprite(i).nX) ? sprAlien.nX - dARTSCISPLASHINGOFFSETX : sprAlien.nX + dARTSCISPLASHINGOFFSETX);
                    ssFr.GetSprite(i).nDestY = sprAlien.nY + randintin(-3, 6);
                    if (ssFr.GetSprite(i).nvY < 3)
                        ssFr.GetSprite(i).nvY = 3;
                    aisSendFroshFlying(ssFr.GetSprite(i));
                    if (Globals.myGameConditions.IsRitual())
                        ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = bIsCommie ? ((int)Goals.goalCOMMIE) : ((int)Goals.goalARTSCI);
                    else
                        ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalARTSCI);
                }
                else if (nTemp == 4)
                {
                    if (bIsCommie)
                        ssFr.GetSprite(i).nDestX = ((sprAlien.nX < ssFr.GetSprite(i).nX) ? sprAlien.nX - dCOMMIEPUNCHINGOFFSETX : sprAlien.nX + dCOMMIEPUNCHINGOFFSETX);
                    else
                        ssFr.GetSprite(i).nDestX = ((sprAlien.nX < ssFr.GetSprite(i).nX) ? sprAlien.nX - dARTSCISPLASHINGOFFSETX : sprAlien.nX + dARTSCISPLASHINGOFFSETX);
                    ssFr.GetSprite(i).nDestY = sprAlien.nY;
                    if (Globals.myGameConditions.IsRitual())
                        ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = bIsCommie ? ((int)Goals.goalCOMMIE) : ((int)Goals.goalARTSCI);
                    else
                        ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalARTSCI);
                    if (ssFr.GetSprite(i).nvY < 3)
                        ssFr.GetSprite(i).nvY = 3;
                    if (ssFr.GetSprite(i).nZ < 5)
                    {
                        ssFr.GetSprite(i).nZ = 0;
                        aiInit4(ssFr.GetSprite(i));
                    }
                    else
                        aisSendFroshFlying(ssFr.GetSprite(i));
                }
            }
        }
    }


    public static void aisAbandonClark(TSprite sprClark)
    {
        int i;
        int nBehavior;
        int nGoal;
        int nDestX;
        int nDestY;
        int n = ssFr.GetNumberOfSprites();
        int nNewGoal;
        if (0 != R.Next(2))
            nNewGoal = ((int)Goals.goalMINDLESS_WANDERING);
        else
            nNewGoal = ((int)Goals.goalTHINK);

        TSprite tmp;
        for (i = 0; i < n; i++)
        {
            tmp = ssFr.GetSprite(i);
            nBehavior = tmp.nAttrib[((int)nattrFrosh.attrBehavior)];
            nGoal = tmp.nAttrib[((int)nattrFrosh.attrGoal)];
            nDestX = tmp.nDestX;
            nDestY = tmp.nDestY;

            if (nGoal == ((int)Goals.goalCLARK) && Math.Abs(nDestX - sprClark.nX) == dPIZZAEATINGOFFSETX && Math.Abs(nDestY - sprClark.nY) < 3)
            {
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrPersonality)] = ((int)Personalities.persHeavyweight);
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = nNewGoal;
                ssFr.GetSprite(i).nDestX = ssFr.GetSprite(i).nX;
                ssFr.GetSprite(i).nDestY = ssFr.GetSprite(i).nY;
            }
        }
    }

    public static void aisChaseClark(TSprite sprClark)
    {
        // THIS IS A BLAST OF AISCHASEPIZZA
        int i;
        int nTemp;
        int n = ssFr.GetNumberOfSprites();

        for (i = 0; i < n; i++)
        {
            if ((0 == R.Next((Globals.myGameConditions.GetBooster(0) / 3) + 1)))
            {
                nTemp = ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)];
                if (nTemp == 7 || nTemp == 10 || nTemp == 11)
                {
                    ssFr.GetSprite(i).nDestX = ((sprClark.nX < ssFr.GetSprite(i).nX) ? sprClark.nX - dPIZZAEATINGOFFSETX : sprClark.nX + dPIZZAEATINGOFFSETX);
                    ssFr.GetSprite(i).nDestY = sprClark.nY;
                    aisSendFroshFlying(ssFr.GetSprite(i));
                    ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalCLARK);
                }
                else if (nTemp == 4)
                {
                    ssFr.GetSprite(i).nDestX = ((sprClark.nX < ssFr.GetSprite(i).nX) ? sprClark.nX - dPIZZAEATINGOFFSETX : sprClark.nX + dPIZZAEATINGOFFSETX);
                    ssFr.GetSprite(i).nDestY = sprClark.nY;
                    ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalCLARK);
                    if (ssFr.GetSprite(i).nZ < 5)
                    {
                        ssFr.GetSprite(i).nZ = 0;
                        aiInit4(ssFr.GetSprite(i));
                    }
                    else
                        aisSendFroshFlying(ssFr.GetSprite(i));
                }
            }
        }
    }

    public static void aisAbandonPizza(TSprite sprPizza)
    {
        int i;
        int nBehavior;
        int nGoal;
        int nDestX;
        int nDestY;
        int n = ssFr.GetNumberOfSprites();
        int nNewGoal;
        if (0 != R.Next(2))
            nNewGoal = ((int)Goals.goalMINDLESS_WANDERING);
        else
            nNewGoal = ((int)Goals.goalTHINK);

        TSprite tmp;
        for (i = 0; i < n; i++)
        {
            tmp = ssFr.GetSprite(i);
            nBehavior = tmp.nAttrib[((int)nattrFrosh.attrBehavior)];
            nGoal = tmp.nAttrib[((int)nattrFrosh.attrGoal)];
            nDestX = tmp.nDestX;
            nDestY = tmp.nDestY;

            if (nGoal == ((int)Goals.goalPIZZA) && Math.Abs(nDestX - sprPizza.nX) == dPIZZAEATINGOFFSETX && Math.Abs(nDestY - sprPizza.nY) < 3)
            {
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrPersonality)] = ((int)Personalities.persHeavyweight);
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = nNewGoal;
                ssFr.GetSprite(i).nDestX = ssFr.GetSprite(i).nX;
                ssFr.GetSprite(i).nDestY = ssFr.GetSprite(i).nY;
            }
        }
    }

    public static void aisChasePizza(TSprite sprPizza)
    {
        // Send all of the frosh chasing after a pizza
        int i;
        int nTemp;
        int n = ssFr.GetNumberOfSprites();

        for (i = 0; i < n; i++)
        {
            if ((0 == R.Next((Globals.myGameConditions.GetBooster(0) / 3) + 1)))
            {
                nTemp = ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)];
                if (nTemp == 7 || nTemp == 10 || nTemp == 11)
                {
                    ssFr.GetSprite(i).nDestX = ((sprPizza.nX < ssFr.GetSprite(i).nX) ? sprPizza.nX - dPIZZAEATINGOFFSETX : sprPizza.nX + dPIZZAEATINGOFFSETX);
                    ssFr.GetSprite(i).nDestY = sprPizza.nY;
                    aisSendFroshFlying(ssFr.GetSprite(i));
                    ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalPIZZA);
                }
                else if (nTemp == 4)
                {
                    ssFr.GetSprite(i).nDestX = ((sprPizza.nX < ssFr.GetSprite(i).nX) ? sprPizza.nX - dPIZZAEATINGOFFSETX : sprPizza.nX + dPIZZAEATINGOFFSETX);
                    ssFr.GetSprite(i).nDestY = sprPizza.nY;
                    ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalPIZZA);
                    if (ssFr.GetSprite(i).nZ < 5)
                    {
                        ssFr.GetSprite(i).nZ = 0;
                        aiInit4(ssFr.GetSprite(i));
                    }
                    else
                        aisSendFroshFlying(ssFr.GetSprite(i));
                }
            }
        }
    }


    public static void aisRegroup()
    {
        // Cause the thinking frosh to regroup
        int i;
        int n = ssFr.GetNumberOfSprites();
        /*
        */
        for (i = 0; i < n; i++)
        {
            if (ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)] == 6)
            {
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalMINDLESS_WANDERING);
                ssFr.GetSprite(i).nAttrib[((int)nattrFrosh.attrPersonality)] = ((int)Personalities.persHeavyweight);
                aiInit4(ssFr.GetSprite(i));
            }
        }
    }

    public static void aisBobUpAndDown(TSprite s)
    {
        aisBobUpAndDown(s, AIMethods.timeAVERAGEBOBTIME);
    }
    public static void aisBobUpAndDown(TSprite s, int nDelay)
    {
        if (0 == R.Next(nDelay))
        {
            s.nZ = (s.nZ == 0 ? -1 : 0);
            if ((0 == R.Next(3)) && (0 != Globals.myGameConditions.GetEnhancedGraphics()))
                ssWater.Include(SpriteInit.CreateSprite((SpriteType.sprRIPPLE), s.nX, s.nY));
        }
    }

    public const int timeRANDOMEVENTINTERVAL = 4;
    public static void aisRandomEvents(TSprite s)
    {
        // Obtain stats on what's going on in the game
        if ((0 == (sprPole.nCC % timeRANDOMEVENTINTERVAL)))
        {
            int i;
            nFroshLevelL[1] = 0; nFroshLevelR[1] = 0;
            nFroshLevelL[2] = 0; nFroshLevelR[2] = 0;
            nFroshLevelL[3] = 0; nFroshLevelR[3] = 0;
            nFroshLevelL[4] = 0; nFroshLevelR[4] = 0;
            nFroshLevelL[5] = 0; nFroshLevelR[5] = 0;
            nFroshLevelL[6] = 0; nFroshLevelR[6] = 0;
            nFroshTam = 0;
            nFroshThinking = 0; nFroshBitter = 0;

            int n = ssFr.GetNumberOfSprites();
            TSprite sprTmp;

            for (i = 0; i < n; i++)
            {
                sprTmp = ssFr.GetSprite(i);
                if (sprTmp.nX < dPOLEX)
                    nFroshLevelL[sprTmp.nAttrib[((int)nattrFrosh.attrPyramidLevel)]]++;
                else
                    nFroshLevelR[sprTmp.nAttrib[((int)nattrFrosh.attrPyramidLevel)]]++;

                if (0 == (sprTmp.nAttrib[((int)nattrFrosh.attrMotivation)]))
                    nFroshBitter++;
                if (sprTmp.nAttrib[((int)nattrFrosh.attrBehavior)] == 6 &&
                    sprTmp.nAttrib[((int)nattrFrosh.attrGoal)] == ((int)Goals.goalTHINK))
                    nFroshThinking++;
            }
            nFroshTam = nFroshLevelL[6] + nFroshLevelR[6];
            nFroshAbove3 = nFroshTam
                + nFroshLevelL[5] + nFroshLevelR[5]
                + nFroshLevelL[4] + nFroshLevelR[4];
            nFroshAbove1 = nFroshAbove3
                + nFroshLevelL[3] + nFroshLevelR[3]
                + nFroshLevelL[2] + nFroshLevelR[2];
            nFroshTotalL = nFroshLevelL[1] + nFroshLevelL[2] + nFroshLevelL[3]
                + nFroshLevelL[4] + nFroshLevelL[5] + nFroshLevelL[6];
            nFroshTotalR = nFroshLevelR[1] + nFroshLevelR[2] + nFroshLevelR[3]
                + nFroshLevelR[4] + nFroshLevelR[5] + nFroshLevelR[6];

            if (Globals.myGameConditions.IsPopBoyInPit())
            {
                nFroshLevelL[1] += 2;
                nFroshLevelR[1] += 2;
            }

            for (i = 0; i < 7; i++)
                nFroshLevel[i] = nFroshLevelL[i] + nFroshLevelR[i];

            if (nFroshThinking > 20)
            {
                aisRegroup();
            }


            // If the Pyramid is topheavy, it gets toppled.
            if ((nFroshAbove1 > (nFroshLevel[1]) && nFroshAbove1 > 10))
            {
                Globals.Debug("Pyramid failed because there were too many frosh above level one.");
                aisTopplePyramid();
                Globals.myGameConditions.AddEnergy(170);
            }
            if (nFroshLevel[3] > nFroshLevel[2] + 5)
            {
                Globals.Debug("Pyramid failed because there were more frosh on level 3 than 2.");
                aisTopplePyramid();
                Globals.myGameConditions.AddEnergy(170);
            }
            if (nFroshLevel[4] > nFroshLevel[3] + 4)
            {
                Globals.Debug("Pyramid failed because there were more frosh on level 4 than 3.");
                aisTopplePyramid();
                Globals.myGameConditions.AddEnergy(170);
            }
            if (nFroshLevel[5] > nFroshLevel[4] + 3)
            {
                Globals.Debug("Pyramid failed because there were more frosh on level 5 than 4.");
                aisTopplePyramid();
                Globals.myGameConditions.AddEnergy(150);

            }
        }
    }
    public static void aisThinkForAl(TSprite s)
    {
        aisThinkForAl(s, true);
    }

    public static void aisThinkForAl(TSprite s, bool bCircle)
    {
        // Cause the frosh to think
        int i;
        int nTmp;
        TSprite sprTmp;
        int nDisplace = s.nX > dPOLEX ? 80 : -80;
        int n = ssFr.GetNumberOfSprites();
        for (i = 0; i < n; i++)
        {
            sprTmp = ssFr.GetSprite(i);
            if (sprTmp.nZ < 2)
            {
                aiInit6D(sprTmp);
                if (bCircle)
                {
                    nTmp = Math.Abs(s.nY - sprTmp.nDestY) / 8;
                    if (nTmp > 7)
                        nTmp = 7;
                    if (sprTmp.nDestX < s.nX)
                        sprTmp.nDestX = s.nX - (int)Math.Sqrt(6400 - 128 * (nTmp) * (nTmp)) - R.Next(100);
                    else
                        sprTmp.nDestX = s.nX + (int)Math.Sqrt(6400 - 128 * (nTmp) * (nTmp)) + R.Next(100);
                }
            }
            else
            {
                aiInit2(sprTmp);
                if (sprTmp.nX < dPOLEX)
                {
                    sprTmp.nvX = -sprTmp.nvX;
                    sprTmp.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR2_1) + R.Next(nsprFR2)]);
                }
            }
        }
    }
}