public static partial class AIMethods
{


    // ALL OF THE CRAZY CROWD BUSINESS

    public static void aisSetFrecAction(TSprite s, int nNewAction)
    {
        s.nCC = 0;
        if (s.nAttrib[((int)nattrCrowd.attrFAction)] != ((int)attrCrowdActions.faPart))
            s.nAttrib[((int)nattrCrowd.attrFAction)] = nNewAction;
    }

    public static void aiFrecGroupBlock(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_AppleBlock)]);
        if (s.nCC > 40)
            aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
    }
    public static void aiFrecActionBlock(TSprite s)
    {
        aiFrecGroupBlock(s);
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_AppleBlock)]);
    }
    //    public static int nTemp;
    public static void aiFrecGroupMill(TSprite s)
    {
        nTemp = R.Next(SoundbankInfo.nsndFRECS_ROAR);
        if (0 == R.Next(600) && s == sprFrecsR)
            aisSetFrecAction(s, ((int)attrCrowdActions.faWave));

        if (0 != nFroshLevel[6])
            aisSetFrecAction(s, ((int)attrCrowdActions.faLookUp));


        if ((0 == R.Next(40)) || s.nCC == 1)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Milling1) + R.Next(3)]);
        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] > energyCheer) && 0 != R.Next(2))
        {
            if (!sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].IsPlaying())
                sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].Play(SoundbankInfo.volCROWD, panONX(s));
            aisSetFrecAction(s, ((int)attrCrowdActions.faCheering));
        }
    }
    public static void aiFrecActionMill(TSprite s)
    {
        if (0 == R.Next(600) && s == sprFrecsR)
            aisSetFrecAction(s, ((int)attrCrowdActions.faWave));

        if (0 != nFroshLevel[6])
            aisSetFrecAction(s, ((int)attrCrowdActions.faLookUp));

        if ((0 == R.Next(40)) || s.nCC == 1)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Milling1) + R.Next(2)]);
        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] > energyCheer) && 0 != R.Next(2))
        {
            //		static int nTemp;
            nTemp = R.Next(SoundbankInfo.nsndFRECS_ROAR);
            if (!sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].IsPlaying())
                sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].Play(SoundbankInfo.volCROWD, panONX(s));
            aisSetFrecAction(s, ((int)attrCrowdActions.faCheering));
        }

    }

    public static void aiFrecGroupCheer(TSprite s)
    {
        if (s.nCC > 16)
            s.nCC = 0;
        if (s.nCC == 1)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Cheer1)]);
        if (s.nCC == 5)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Cheer2)]);
        if (s.nCC == 9)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Cheer3)]);
        if (s.nCC == 12)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Cheer4)]);

        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] < energyCheer) && (0 == R.Next(5)))
            aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] > energySlam))
        {
            if (!lSound[((int)ASLList.lsndFRECS_SLAM)].IsPlaying())
                lSound[((int)ASLList.lsndFRECS_SLAM)].Play(SoundbankInfo.volCROWD);
            aisSetFrecAction(s, ((int)attrCrowdActions.faSlamming));
            //		aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faSlamming));
            //		aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faSlamming));
        }
    }

    public static void aiFrecActionCheer(TSprite s)
    {
        if (s.nCC > 10)
            s.nCC = 0;
        if (s.nCC == 1)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Shout1)]);
        if (s.nCC == 5)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Shout2)]);
        if (s.nCC == 7)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Shout3)]);
        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] < energyCheer) && (0 == R.Next(5)))
            aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] > energySlam))
        {
            if (!lSound[((int)ASLList.lsndFRECS_SLAM)].IsPlaying())
                lSound[((int)ASLList.lsndFRECS_SLAM)].Play(SoundbankInfo.volCROWD);
            aisSetFrecAction(s, ((int)attrCrowdActions.faSlamming));

            //		lSound[((int)ASLList.lsndFRECS_SLAM)].Play(SoundbankInfo.volCROWD);
            //		aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faSlamming));
            //		aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faSlamming));
            //		aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faSlamming));
        }

    }

    public static void aiFrecGroupSlam(TSprite s)
    {
        if (s.nAttrib[((int)nattrCrowd.attrFEnergy)] > energySlam + 300)
            s.nAttrib[((int)nattrCrowd.attrFEnergy)] = energySlam + 100;
        switch (s.nCC)
        {
            case 4: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Slam2)]); break;
            case 12: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Slam3)]); break;
            case 15: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Slam1)]); break;
            case 20:
                if (s.nAttrib[((int)nattrCrowd.attrFEnergy)] < energySlam)
                    aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
                else
                {
                    s.nCC = 0;
                    if (!lSound[((int)ASLList.lsndFRECS_SLAM)].IsPlaying())
                        lSound[((int)ASLList.lsndFRECS_SLAM)].Play(SoundbankInfo.volCROWD);
                }
                break;
        }
        //	Globals.myGameConditions.AddEnergy(-2);
    }
    public static void aiFrecActionSlam(TSprite s)
    {
        if (s.nAttrib[((int)nattrCrowd.attrFEnergy)] > energySlam + 300)
            s.nAttrib[((int)nattrCrowd.attrFEnergy)] = energySlam + 100;
        switch (s.nCC)
        {
            case 4: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Slam2)]); break;
            case 12: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Slam3)]); break;
            case 15: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Slam1)]); break;
            case 20:
                if (s.nAttrib[((int)nattrCrowd.attrFEnergy)] < energySlam)
                    aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
                else
                {
                    s.nCC = 0;
                    if (!lSound[((int)ASLList.lsndFRECS_SLAM)].IsPlaying())
                        lSound[((int)ASLList.lsndFRECS_SLAM)].Play(SoundbankInfo.volCROWD);
                }
                break;
        }


    }

    public static void aiFrecGroupLookUp(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_LookUp1)]);
        if (s.nCC > 50 && 0 == R.Next(6))
            aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] > energyCheer) && 0 != R.Next(2))
            aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
    }
    public static void aiFrecActionLookUp(TSprite s)
    {
        s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_LookUp1)]);
        if (s.nCC > 50 && 0 == R.Next(6))
            aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] > energyCheer) && 0 != R.Next(2))
            aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
    }
    public static void aiFrecGroupStayinAlive(TSprite s)
    {
        if (s.nCC > 300)
        {
            aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
        }
        else
        {
            switch (sprPole.nCC % 69)
            {
                case 4:
                case 24:
                case 40:
                case 56:
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_StayinAlive1)]);
                    break;
                case 16:
                case 32:
                case 48:
                case 64:
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_StayinAlive2)]);
                    break;
            }
        }
    }
    public static void aiFrecActionStayinAlive(TSprite s)
    {
        if (s.nCC > 500)
        {
            aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
        }
        else
        {
            switch (sprPole.nCC % 69)
            {
                case 4:
                case 24:
                case 40:
                case 56:
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_StayinAlive2)]);
                    break;
                case 16:
                case 32:
                case 48:
                case 64:
                    s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_StayinAlive1)]);
                    break;
            }
        }
    }
    public static void aiFrecActionPart(TSprite s)
    {
        // HUH?
        aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
    }
    public static void aiFrecGroupPart(TSprite s)
    {
        switch (s.nCC)
        {
            case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Milling1)]); break;
            case 57: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Part1)]); break;
            case 65: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Part2)]); break;
            case 137: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Part1)]); break;
            case 145: s.nAttrib[((int)nattrCrowd.attrFAction)] = ((int)attrCrowdActions.faMilling); break;
        }
    }
    public static void aiFrecGroupBoo(TSprite s)
    {
        switch (s.nCC)
        {
            case 1: lSound[((int)ASLList.lsndFRECS_BOO1) + R.Next(3)].Play(SoundbankInfo.volCROWD, panONX(s));
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Boo1)]); break;
            case 15: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Boo2)]); break;
            case 30: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Boo3)]); break;
            case 45: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Boo2)]); break;
            case 60: aisSetFrecAction(s, ((int)attrCrowdActions.faMilling)); break;
        }
    }
    public static void aiFrecActionBoo(TSprite s)
    {
        switch (s.nCC)
        {
            case 1: lSound[((int)ASLList.lsndFRECS_BOO1) + R.Next(3)].Play(SoundbankInfo.volCROWD, panONX(s));
                s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Boo1)]); break;
            case 15: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Boo2)]); break;
            case 30: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Boo3)]); break;
            case 45: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Boo2)]); break;
            case 60: aisSetFrecAction(s, ((int)attrCrowdActions.faMilling)); break;
        }
    }

    public static void aiFrecActionShout(TSprite s)
    {
        aisSetFrecAction(s, ((int)attrCrowdActions.faCheering));
    }

    public static void aiFrecGroupShout(TSprite s)
    {
        if (s.nCC > 20)
            s.nCC = 0;
        if (s.nCC == 1)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Shout1)]);
        if (s.nCC == 10)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Shout2)]);
        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] < energyCheer) && (0 == R.Next(5)))
            aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] > energySlam))
        {
            lSound[((int)ASLList.lsndFRECS_SLAM)].Play(SoundbankInfo.volCROWD);
            aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faSlamming));
            aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faSlamming));
            aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faSlamming));
        }
    }

    public const int SWITCHTIME1 = 100;
    public const int SWITCHTIME2 = 250;
    public const int SWITCHTIME3 = 370;

    public static void aiFrecGroupLookURL(TSprite s)
    {
        if (s.nCC < 0) s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_LookULR3)]);
        switch (s.nCC)
        {
            case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_LookULR3)]); break;
            case SWITCHTIME1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_LookULR2)]); break;
            case SWITCHTIME2: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_LookULR1)]); break;
            case SWITCHTIME3: aisSetFrecAction(s, ((int)attrCrowdActions.faMilling)); break;
        }
    }


    public static void aiFrecGroupLookULR(TSprite s)
    {
        if (s.nCC < 0) s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_LookULR1)]);
        switch (s.nCC)
        {
            case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_LookULR1)]); break;
            case SWITCHTIME1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_LookULR2)]); break;
            case SWITCHTIME2: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_LookULR3)]); break;
            case SWITCHTIME3: aisSetFrecAction(s, ((int)attrCrowdActions.faMilling)); break;
        }
    }

    public static void aiFrecActionLookURL(TSprite s)
    {
        if (s.nCC < 0) s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_LookULR3)]);
        switch (s.nCC)
        {
            case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_LookULR3)]); break;
            case SWITCHTIME1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_LookULR2)]); break;
            case SWITCHTIME2: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_LookULR1)]); break;
            case SWITCHTIME3: aisSetFrecAction(s, ((int)attrCrowdActions.faMilling)); break;
        }
    }

    public static void aiFrecActionLookULR(TSprite s)
    {
        if (s.nCC < 0) s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_LookULR1)]);
        switch (s.nCC)
        {
            case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_LookULR1)]); break;
            case SWITCHTIME1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_LookULR2)]); break;
            case SWITCHTIME2: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_LookULR3)]); break;
            case SWITCHTIME3: aisSetFrecAction(s, ((int)attrCrowdActions.faMilling)); break;
        }
    }

    public static void aiFrecGroupWave(TSprite s)
    {
        switch (s.nCC)
        {
            case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Wave1)]); break;
            case 3: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Wave2)]); break;
            case 6: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpGF_Wave3)]); break;
            case 9: aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
                if (s == sprFrecsR)
                    aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faWave));
                if (s == sprFrecsC)
                    aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faWave));
                break;
        }
    }

    public static void aiFrecActionWave(TSprite s)
    {
        switch (s.nCC)
        {
            case 1: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Wave1)]); break;
            case 3: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Wave2)]); break;
            case 6: s.SetFrame(frm[((int)GameBitmapEnumeration.bmpAF_Wave3)]); break;
            case 9: aisSetFrecAction(s, ((int)attrCrowdActions.faMilling));
                if (s == sprFrecsR)
                    aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faWave));
                if (s == sprFrecsC)
                    aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faWave));
                break;
        }
    }


    public static void aiFrecGroup(TSprite s)
    {
        // LET THE CENTER FRECS ACT AS THE ENERGY GIVERS
        if (s == sprFrecsC)
        {
            int nTemp = Globals.myGameConditions.GetEnergy();
            sprFrecsL.nAttrib[((int)nattrCrowd.attrFEnergy)] += nTemp;
            sprFrecsC.nAttrib[((int)nattrCrowd.attrFEnergy)] += nTemp;
            sprFrecsR.nAttrib[((int)nattrCrowd.attrFEnergy)] += nTemp;
            Globals.myGameConditions.ResetEnergy();
        }


        switch (s.nAttrib[((int)nattrCrowd.attrFAction)])
        {
            case ((int)attrCrowdActions.faMilling): aiFrecGroupMill(s); break;
            case ((int)attrCrowdActions.faCheering): aiFrecGroupCheer(s); break;
            case ((int)attrCrowdActions.faSlamming): aiFrecGroupSlam(s); break;
            case ((int)attrCrowdActions.faBooing): aiFrecGroupBoo(s); break;
            case ((int)attrCrowdActions.faShouting): aiFrecGroupShout(s); break;
            case ((int)attrCrowdActions.faBlocking): aiFrecGroupBlock(s); break;
            case ((int)attrCrowdActions.faLookUp): aiFrecGroupLookUp(s); break;
            case ((int)attrCrowdActions.faLookULR): aiFrecGroupLookULR(s); break;
            case ((int)attrCrowdActions.faLookURL): aiFrecGroupLookURL(s); break;
            case ((int)attrCrowdActions.faStayinAlive): aiFrecGroupStayinAlive(s); break;
            case ((int)attrCrowdActions.faWave): aiFrecGroupWave(s); break;
            case ((int)attrCrowdActions.faPart): aiFrecGroupPart(s); break;
        }

        s.nAttrib[((int)nattrCrowd.attrFEnergy)]--;
        if (s.nAttrib[((int)nattrCrowd.attrFEnergy)] < 0)
            s.nAttrib[((int)nattrCrowd.attrFEnergy)] = 0;

    }

    public static void aiFrecAction(TSprite s)
    {
        // LET THE CENTER FRECS ACT AS THE ENERGY GIVERS
        if (s == sprFrecsC)
        {
            int nTemp = Globals.myGameConditions.GetEnergy();
            sprFrecsL.nAttrib[((int)nattrCrowd.attrFEnergy)] += nTemp;
            sprFrecsC.nAttrib[((int)nattrCrowd.attrFEnergy)] += nTemp;
            sprFrecsR.nAttrib[((int)nattrCrowd.attrFEnergy)] += nTemp;
            sprForge.nAttrib[((int)nattrForge.attrForgeEnergy)] += nTemp;
            Globals.myGameConditions.ResetEnergy();
        }

        switch (s.nAttrib[((int)nattrCrowd.attrFAction)])
        {
            case ((int)attrCrowdActions.faMilling): aiFrecActionMill(s); break;
            case ((int)attrCrowdActions.faCheering): aiFrecActionCheer(s); break;
            case ((int)attrCrowdActions.faSlamming): aiFrecActionSlam(s); break;
            case ((int)attrCrowdActions.faBooing): aiFrecActionBoo(s); break;
            case ((int)attrCrowdActions.faShouting): aiFrecActionShout(s); break;
            case ((int)attrCrowdActions.faBlocking): aiFrecActionBlock(s); break;
            case ((int)attrCrowdActions.faLookUp): aiFrecActionLookUp(s); break;
            case ((int)attrCrowdActions.faLookULR): aiFrecActionLookULR(s); break;
            case ((int)attrCrowdActions.faLookURL): aiFrecActionLookURL(s); break;
            case ((int)attrCrowdActions.faStayinAlive): aiFrecActionStayinAlive(s); break;
            case ((int)attrCrowdActions.faWave): aiFrecActionWave(s); break;
            case ((int)attrCrowdActions.faPart): aiFrecActionPart(s); break;
        }

        s.nAttrib[((int)nattrCrowd.attrFEnergy)]--;
        if (s.nAttrib[((int)nattrCrowd.attrFEnergy)] < 0)
            s.nAttrib[((int)nattrCrowd.attrFEnergy)] = 0;
    }

    public const int timeBETWEENCHEERS = 250;

    public static int panONX(TSprite s)
    {
        return (int)((s.nX - 320) / 3.2);
    }

    public static void aiFrecBoring(TSprite s)
    {
        s.nAttrib[((int)nattrCrowd.attrFEnergy)] += Globals.myGameConditions.GetEnergy();
        Globals.myGameConditions.ResetEnergy();

        if ((s.nAttrib[((int)nattrCrowd.attrFEnergy)] > energyCheer) && s.nCC > timeBETWEENCHEERS)
        {
            s.nCC = 0;
            //		static int nTemp;
            nTemp = R.Next(SoundbankInfo.nsndFRECS_ROAR);
            if (!sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].IsPlaying())
                sSound[((int)ASSList.ssndEFFECTS_CROWDROAR1) + nTemp].Play(SoundbankInfo.volCROWD, panONX(s));
        }

        s.nAttrib[((int)nattrCrowd.attrFEnergy)]--;
        if (s.nAttrib[((int)nattrCrowd.attrFEnergy)] < 0)
            s.nAttrib[((int)nattrCrowd.attrFEnergy)] = 0;
    }
}