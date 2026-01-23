public static partial class AIMethods
{
#if SILVERLIGHT
    public const int gnNumFroshInPit = 80; // should be 85
#else
    public const int gnNumFroshInPit = 85; // should be 85
#endif
    public const int gnNumStartHeavyweight = 55;
    public const int gnNumStartClimber = 28;
    public const int gnNumStartHoister = 2;
    public const int gnDefaultNailsInTam = 5;

    public const int gnDeafultMorale = 1;
    public const int gnDefaultStrength = 7;
    public const int gnDefaultIntelligence = 7;

    public static bool NO_SOUND = false;

    public const int MAX_FRAMES = 750;  // The maximum number of bitmaps used by the game

    // Pit's co-ordinates relative to the Pit layer.
    public const int dPITMINX = 0;
    public const int dPITMINXMINUS50 = -50;
    public const int dPITMAXX = 640;
    public const int dPITMAXXPLUS50 = 690;
    public const int dPITMINY = 30;
    public const int dPITMAXY = 200;
    public const int dPITMAXYPLUS30 = 230;
    public const int dPOLEWIDTH = 20;
    public const int dPOLEHEIGHT = 640;

    public const int nFROSHPERSONALITIES = 4;
    public const int nPIZZAMUNCHAVERAGE = 15;
    // Relative distances in Pixels
    public const int dONE = 1;
    public const int dTWO = 2;
    public const int dTHREE = 3;
    public const int dSIX = 6;
    public const int dTEN = 10;
    public const int dGRAVCONST = 2;      // Gravitational constant
    public const int dSPEEDFORBIGSPLASH = 8;
    public const int dBELLYBUTTONZ = 15;        // Height of Bellybutton above water in z
    public const int dSPLASHINGDISTANCE = 100;  // Distance you should be away to splash.
    public const int dUPPERLEVELDISTANCEFROMPOLE = 15; // Distance a frosh stands from the Pole
    public const int dFROSHARMLINKOFFSETX = 45;  // Must be float.
    public const int dFROSHARMLINKOFFSETY = 20;  // Must be float.
    public const int dFROSHWIDTHY = 6; // How "Fat" in the y-dir are the frosh?
    public const int dBANKHEIGHT = 40; // Height of the opposite bank of the pit
    public const int dICONWIDTH = 60; // Width of the icons in the game
    public const int dPALMHEIGHT = 200; // Height of palm above base of hand
    public const int dTAMZ = 490;
    public const int dARMWIDTH = 45;
    public const int dSTAGGER = 20;
    public const int dPOLEX = 320;
    public const int dPOLEY = 80;
    public const int dCLIMBINGSPEED = 5;
    public const int dSWIMSPEED = 4;
    public const int dSWIMDISTANCE = 400;
    public const int dSWIMFRAMERATE = 10;
    public const int dPIZZAEATINGOFFSETX = 40;
    public const int dCOMMIEPUNCHINGOFFSETX = 30;
    public const int dARTSCISPLASHINGOFFSETX = 130; // a bit more than dSPLASHINGDISTANCE.
    public const int dHIGHSCORESTARTHEIGHT = 400;

    // From GameConditions:
    public const int NUM_PERFORMACEBOOSTS = 8;
    public const int NO_BAR = -1;
    public const int NUM_RINGSPOTS = 100;
    public const int NUM_TRICKS = 19;

    public const int BEER_DRINKING_SPEED = 3;
    public const bool aiKEEPPOLEPOSITION = true;

    // Conversions between layers
    public static int dSKYYTOPITY()
    {
        return (Globals.myLayers.GetOffset(((int)LayerNames.LAYERSKY)) - Globals.myLayers.GetOffset(((int)LayerNames.LAYERPIT)));
    }
    public static int dSKYLINEYTOPITY()
    {
        return (Globals.myLayers.GetOffset(((int)LayerNames.LAYERSKYLINE)) - Globals.myLayers.GetOffset(((int)LayerNames.LAYERPIT)));
    }
    public static int dMISCYTOPITY()
    {
        return (Globals.myLayers.GetOffset(((int)LayerNames.LAYERMISC)) - Globals.myLayers.GetOffset(((int)LayerNames.LAYERPIT)));
    }
    public static int dPITYTOMISCY()
    {
        return (Globals.myLayers.GetOffset(((int)LayerNames.LAYERPIT)) - Globals.myLayers.GetOffset(((int)LayerNames.LAYERMISC)));
    }

    // Relative times in frames (for running at 25fps right now.)
    public const int timeWHAP = 2;   // Time for a "WHAP!" to be onscreen.
    public const int timeBUBBLE = 50;  // Time for a GW Word Bubble to be onscreen.
    public const int timePIZZAMUNCH = 3;
    public const int timeREACHFORCLING = 20;
    public const int timeDROPFROMCLINGING = 450;
    public const int timeTHINKINGTIME = 100;
    public const int timeAVERAGEBOBTIME = 35;
    public const int timeCLARKMUGFLOATROTATION = 5;

    public const int timeBETWEENEVENTS = 100;
    public static void NOSPEECHFOR(int noSpeechTime)
    {
        if (sprRandomEventGenerator.nCC > timeBETWEENEVENTS)
            sprRandomEventGenerator.nCC = timeBETWEENEVENTS;
        sprRandomEventGenerator.nCC -= noSpeechTime;
    }
    public static bool SPEECHOK()
    {
        return (sprRandomEventGenerator.nCC > timeBETWEENEVENTS);
    }

    // (Number of sprites in a sequence)
    public const int nsprFR1 = 6;  // Number of falling frosh sprites
    public const int nsprFR1B = 1;  // Number of splshing frosh
    public const int nsprFR2 = 2;  // Number of leaping/diving frosh
    public const int nsprFR3 = 2;  // Number of underwater sprites
    public const int nsprFR4 = 4;  // Number of standard wading frosh
    public const int nsprFR4E = 2;  // Number of excited wading frosh

    public const int nsprAPPLE1 = 1; // Number of apples, depth 1
    public const int nsprAPPLE2 = 1; // Number of apples, depth 2
    public const int nsprAPPLE3 = 1; // Number of apples, depth 3
    public const int nsprAPPLE4 = 1; // Number of apples, depth 4
    public const int nsprAPPLE5 = 4; // Number of apples, depth 5
    public const int nsprAPPLE6 = 1; // Number of apples, depth 5
    public const int nsprAPPLE7 = 1; // Number of apples, depth 5
    public const int nsprWHAP = 1; // Number of WHAP!'s

    public const int nsprPIZZA1 = 1; // Number of pizzas, depth 1
    public const int nsprPIZZA2 = 1; // Number of pizzas, depth 2
    public const int nsprPIZZA3 = 1; // Number of pizzas, depth 3
    public const int nsprPIZZA4 = 8; // Number of pizzas, depth 4
    public const int nsprPIZZA5 = 1; // Number of pizzas, depth 5
    public const int nsprPIZZA6 = 1; // Number of pizzas, depth 6
    public const int nsprPIZZA7 = 1; // Number of pizzas, depth 7

    public const int nsprCLARK1 = 1; // Number of Clark mugs, depth 1
    public const int nsprCLARK2 = 1; // Number of Clark mugs, depth 2
    public const int nsprCLARK3 = 1; // Number of Clark mugs, depth 3
    public const int nsprCLARK4 = 1; // Number of Clark mugs, depth 4
    public const int nsprCLARK5A = 4;// Number of Clark mugs, depth 5 floating
    public const int nsprCLARK5B = 1;// Number of Clark mugs, depth 5 miss
    public const int nsprCLARK6 = 1; // Number of Clark mugs, depth 6
    public const int nsprCLARK7 = 1; // Number of Clark mugs, depth 7

    public const int nsprEXAM1 = 1; // Number of 114 exams, depth 1
    public const int nsprEXAM2 = 1; // Number of 114 exams, depth 2
    public const int nsprEXAM3 = 1; // Number of 114 exams, depth 3
    public const int nsprEXAM4 = 4; // Number of 114 exams, depth 4

    public const int nsprGREASE1 = 1; // Number of greases, depth 1
    public const int nsprGREASE2 = 1; // Number of greases, depth 2
    public const int nsprGREASE3 = 1; // Number of greases, depth 3
    public const int nsprGREASE4 = 1; // Number of greases, depth 4
    public const int nsprGREASE5A = 2;// Number of greases, depth 5 splatter
    public const int nsprGREASE5B = 1;// Number of greases, depth 5 miss
    public const int nsprGREASE6 = 1; // Number of greases, depth 6
    public const int nsprGREASE7 = 1; // Number of greases, depth 7

    public const int nsprARTSCIF_FALL = 3;
    public const int nsprARTSCIF_HIT = 1;
    public const int nsprARTSCIF_POPUP = 2;
    public const int nsprARTSCIF_WADE = 4;

    public const int nsprARTSCIM_FALL = 3;
    public const int nsprARTSCIM_HIT = 2;
    public const int nsprARTSCIM_POPUP = 5;
    public const int nsprARTSCIM_WADE = 4;

    public const int nsprCOMMIEF_FALL = 3;
    public const int nsprCOMMIEF_HIT = 2;
    public const int nsprCOMMIEF_POPUP = 5;
    public const int nsprCOMMIEF_WADE = 4;

    public const int nsprCOMMIEM_FALL = 3;
    public const int nsprCOMMIEM_HIT = 1;
    public const int nsprCOMMIEM_POPUP = 5;
    public const int nsprCOMMIEM_WADE = 3;

    public const int nsprFRECGJ = 8;  // Number of joyous frec frames
    public const int nsprFRECGN = 8;  // Number of normal frec frames

    public const int nsprARMOHEAD = 4; // Number of overhead throw shots

    // ENERGY LEVELS
    public const int energySwing = 4000;
    public const int energyCheer = 150;
    public const int energyStart = 250;
    public const int energySlam = 700;


}