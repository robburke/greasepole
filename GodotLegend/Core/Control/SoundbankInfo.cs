public static class SoundbankInfo
{
    // Sound level info
//    public const int panONX(s) (s->nX - 320)/32 -- created method in AI classes.
public const int panLEFT = -100;
public const int panCENTER = 0;
public const int panRIGHT = 100;

public const int vol10 = 100;
public const int vol9 = 98;
public const int vol8 = 96;
public const int vol7 = 94;
public const int vol6 = 92;
public const int vol5 = 90;
public const int vol4 = 88;
public const int vol3 = 86;
public const int vol2 = 84;
public const int vol1 = 82;

public const int volFULL = 100; //vol10
//public const int volHALF = 90; //vol5
//public const int volQUARTER = 82; //vol1
public const int volDEFAULT = 94; //vol7

public const int volCROWDSHOUT = 92; //vol6
public const int volCROWD = 90; //vol5
public const int volMUSIC = 90; //vol5
public const int volHOLLAR = 100; //vol10
public const int volNORMAL = 94; //vol7




    // From short sound list
    public const int nsndCLARKFINISH = 4;
    public const int nsndEFFECTS_WHOOSH = 2;
    public const int nsndTAUNTS = 9;
    public const int nsndEFFECTS_SMACK = 2;



    // From long sound list

    public const int nsndFROSH_CLARKFINISH = 2;
    public const int nsndWHOOSH = 2;
    public const int nsndWHAP = 2;
    public const int nsndEXAM_TOSS = 2;
    public const int nsndFRECS_PROGRESS = 4;
    public const int nsndFRECS_REWARD = 6;
    public const int nsndARTSCI_MALE_TAUNT = 7;
    public const int nsndARTSCI_FEMALE_TAUNT = 6;
    public const int nsndCOMMIE_MALE_TAUNT = 2;
    public const int nsndCOMMIE_FEMALE_TAUNT = 3;
    public const int nsndFRECS_ROAR = 2;
    public const int nsndFRECS_CHEER = 1;
    public const int nsndFRECS_HITAPPLE = 3;
    public const int nsndFRECS_HITEXAM = 1;
}

public enum ASLList
{
    lsndAPPLES_OFFER1,
    lsndAPPLES_OFFER2,
    lsndAPPLES_OFFER3,
    lsndAPPLES_OFFER4,
    lsndAPPLES_OFFER5,
    lsndAPPLES_OFFER6,
    lsndAPPLES_OFFER7,
    lsndAPPLES_OFFER8,
    lsndAPPLES_OFFER9,
    lsndAPPLES_OFFER10,
    lsndAPPLES_OFFERR1,
    lsndAPPLES_OFFERR2,
    lsndARTSCI_FEMALE_HIT1,
    lsndARTSCI_FEMALE_HIT2,
    lsndARTSCI_FEMALE_HIT3,
    lsndARTSCI_FEMALE_HIT4,
    lsndARTSCI_FEMALE_HIT5,
    lsndARTSCI_FEMALE_PUSH1,
    lsndARTSCI_FEMALE_TAUNT1,
    lsndARTSCI_FEMALE_TAUNT2,
    lsndARTSCI_FEMALE_TAUNT3,
    lsndARTSCI_FEMALE_TAUNT4,
    lsndARTSCI_FEMALE_TAUNT5,
    lsndARTSCI_FEMALE_TAUNT6,
    lsndARTSCI_MALE_HIT1,
    lsndARTSCI_MALE_HIT2,
    lsndARTSCI_MALE_HIT3,
    lsndARTSCI_MALE_HIT4,
    lsndARTSCI_MALE_HIT5,
    lsndARTSCI_MALE_HITR1,
    lsndARTSCI_MALE_HITR2,
    lsndARTSCI_MALE_HITR3,
    lsndARTSCI_MALE_PUSH1,
    lsndARTSCI_MALE_PUSH2,
    lsndARTSCI_MALE_PUSH3,
    lsndARTSCI_MALE_PUSH4,
    lsndARTSCI_MALE_TAUNT1,
    lsndARTSCI_MALE_TAUNT2,
    lsndARTSCI_MALE_TAUNT3,
    lsndARTSCI_MALE_TAUNT4,
    lsndARTSCI_MALE_TAUNT5,
    lsndARTSCI_MALE_TAUNT6,
    lsndARTSCI_MALE_TAUNT7,
    lsndARTSCI_MALE_TAUNTR1,
    lsndCLARK_OFFER1,
    lsndCLARK_OFFER2,
    lsndCLARK_OFFER3,
    lsndCLARK_OFFER4,
    lsndCLARK_OFFER5,
    lsndCLARK_OFFER6,
    lsndCLARK_OFFERR1,
    lsndCLARK_OFFERR2,
    lsndCLARK_OFFERR3,
    lsndCLARK_OFFERR4,
    lsndCOMMIE_FEMALE_HIT1,
    lsndCOMMIE_FEMALE_HIT2,
    lsndCOMMIE_FEMALE_HIT3,
    lsndCOMMIE_FEMALE_HIT4,
    lsndCOMMIE_FEMALE_PUSH1,
    lsndCOMMIE_FEMALE_TAUNT1,
    lsndCOMMIE_FEMALE_TAUNT2,
    lsndCOMMIE_FEMALE_TAUNT3,
    lsndCOMMIE_FEMALE_TAUNT4,
    lsndCOMMIE_MALE_HIT1,
    lsndCOMMIE_MALE_HIT2,
    lsndCOMMIE_MALE_HIT3,
    lsndCOMMIE_MALE_HIT4,
    lsndCOMMIE_MALE_HIT5,
    lsndCOMMIE_MALE_HIT6,
    lsndCOMMIE_MALE_HIT7,
    lsndCOMMIE_MALE_PHONE1,
    lsndCOMMIE_MALE_PHONE2,
    lsndCOMMIE_MALE_PUSH1,
    lsndCOMMIE_MALE_PUSH2,
    lsndCOMMIE_MALE_PUSH3,
    lsndCOMMIE_MALE_PUSH4,
    lsndCOMMIE_MALE_TAUNT1,
    lsndCOMMIE_MALE_TAUNT2,
    lsndCOMMIE_MALE_TAUNTR1,
    lsndCOMMIE_MALE_TAUNTR2,
    lsndCOMMIE_MALE_TAUNTR3,
    lsndDISCIPLINES_APPLE,
    lsndDISCIPLINES_CHEM,
    lsndDISCIPLINES_CIVIL,
    lsndDISCIPLINES_DEFAULT,
    lsndDISCIPLINES_ELEC,
    lsndDISCIPLINES_ENGCHEM,
    lsndDISCIPLINES_ENGPHYS,
    lsndDISCIPLINES_GEO,
    lsndDISCIPLINES_MECH,
    lsndDISCIPLINES_METALS,
    lsndDISCIPLINES_MINING,
    lsndDISCIPLINES_RITUAL,
    lsndEXAM_OFFER1,
    lsndEXAM_OFFER2,
    lsndEXAM_OFFER3,
    lsndEXAM_OFFER4,
    lsndEXAM_OFFER5,
    lsndEXAM_TOSS1,
    lsndFRECS_BOO1,
    lsndFRECS_BOO2,
    lsndFRECS_BOO3,
    lsndFRECS_CHANT1,
    lsndFRECS_CHANTR1,
    lsndFRECS_CHANTR2,
    lsndFRECS_CHEER1,
    lsndFRECS_HITAPPLE1,
    lsndFRECS_HITAPPLE2,
    lsndFRECS_HITAPPLE3,
    lsndFRECS_HITEXAM1,
    lsndFRECS_HOWHIGHTHEPOLE,
    lsndFRECS_PROGRESS1,
    lsndFRECS_PROGRESS2,
    lsndFRECS_PROGRESS3,
    lsndFRECS_PROGRESS4,
    lsndFRECS_PROGRESSR1,
    lsndFRECS_REWARD1,
    lsndFRECS_REWARD2,
    lsndFRECS_REWARD3,
    lsndFRECS_REWARD4,
    lsndFRECS_REWARD5,
    lsndFRECS_REWARD6,
    lsndFRECS_REWARDR1,
    lsndFRECS_SLAM,
    lsndFRECS_STEPHTOP1, lsndFRECS_STEPHTOP2, lsndFROSH_APPLEHIT1, lsndFROSH_APPLEHIT2,
    lsndFROSH_CLARKFINISH1,
    lsndFROSH_CLARKFINISH2,
    lsndFROSH_CLARKFINISH3,
    lsndFROSH_ALPRAISE1,
    lsndFROSH_ALPRAISE2,
    lsndFROSH_ATTOP1,
    lsndFROSH_ATTOP2,
    lsndFROSH_ATTOP3,
    lsndFROSH_ATTOP4,
    lsndFROSH_ATTOP5,
    lsndFROSH_GOTTAM1,
    lsndFROSH_GOTTAM2,
    lsndFROSH_GOTTAM3,
    lsndFROSH_SHEEP1,
    lsndFROSH_SHEEP2,
    lsndFROSH_COW1,
    lsndFROSH_COW2,
    lsndHOSE_OFFER1,
    lsndHOSE_OFFER2,
    lsndHOSE_OFFER3,
    lsndHOSE_OFFERR1,
    lsndHOSE_TAKE1,
    lsndHOSE_TAKE2,
    lsndMUSIC_SCOTLAND,
    lsndMUSIC_TITLEINIT,
    lsndNARRATOR_CONGRATS,
    lsndNARRATOR_GRAPHICSWARN,
    lsndNARRATOR_JACKETINIT,
    lsndNARRATOR_OPTIONINIT,
    lsndNARRATOR_RITUALWARN,
    lsndNARRATOR_STARTDELAY2,
    lsndNARRATOR_STARTDELAY1,
    lsndPIZZA_OFFER1,
    lsndPIZZA_OFFER2,
    lsndPIZZA_OFFER3,
    lsndPIZZA_OFFERR1,
    lsndPOPBOY_ADVICE1,
    lsndPOPBOY_ADVICE2,
    lsndPOPBOY_ADVICE3,
    lsndPOPBOY_ADVICE4,
    lsndPOPBOY_ADVICE5,
    lsndPOPBOY_ADVICE6,
    lsndPOPBOY_APPLE1,
    lsndPOPBOY_APPLER1,
    lsndPOPBOY_BEER1,
    lsndPOPBOY_BEERR2,
    lsndPOPBOY_BEERR3,
    lsndPOPBOY_CHEER1,
    lsndPOPBOY_CHEER2,
    lsndPOPBOY_EXAM1,
    lsndPOPBOY_EXAM2,
    lsndPOPBOY_EXAM3,
    lsndPOPBOY_GOTTAM1,
    lsndPOPBOY_GREETING1,
    lsndPOPBOY_GREETING2,
    lsndPOPBOY_HIPPO1,
    lsndPOPBOY_HIPPOR1,
    lsndPOPBOY_PIZZA1,
    lsndPOPBOY_PIZZA2,
    lsndPREZ_HITAPPLE1,
    lsndPREZ_HITAPPLE2,
    lsndPREZ_HITAPPLE3,
    lsndPREZ_HITAPPLE4,
    lsndPREZ_HITAPPLER4,
    lsndPREZ_HITAPPLE5,
    lsndPREZ_HITCLARK1,
    lsndPREZ_HITCLARK2,
    lsndPREZ_HITCLARK3,
    lsndPREZ_HITEXAM,
    lsndPREZ_HITHOSE1,
    lsndPREZ_HITHOSE2,
    lsndPREZ_HITHOSER2,
    lsndPREZ_HITHOSER3,
    lsndPREZ_HITHOSER4,
    lsndPREZ_HITPIZZA1,
    lsndPREZ_HITPIZZA2,
    lsndPREZ_HITPIZZAR2,
    lsndPREZ_ENCOURAGE1,
    lsndPREZ_ENCOURAGE2,
    lsndPREZ_ENCOURAGE3,
    lsndPREZ_ENCOURAGE4,
    lsndPREZ_ENCOURAGE5,
    lsndPREZ_POPBOY1_1,
    lsndPREZ_POPBOY1_2,
    lsndPREZ_POPBOY1_3,
    lsndPREZ_POPBOY2_1,
    lsndPREZ_POPBOY2_2,
    lsndRING_SWING,
    lsndRING_DING,
    lsndRING_PRESS,
    lsndRING_RISE,
    lsndRING_ZAP1,
    lsndRING_ZAP2,
    lsndRING_ZAP3,
    lsndSCICONF_HitApples,
    lsndSCICONF_HitBeer,
    lsndSCICONF_HitExam,
    lsndSCICONF_HitMisc,
    lsndSCICONF_HitPizza,
    lsndSCICONF_PopUp1,
    lsndSCICONF_PopUp2,
    lsndSCICONF_PopUp3,
    lsndSCICONF_PopUp4,
    lsndSCICONM_HitApples,
    lsndSCICONM_HitBeer,
    lsndSCICONM_HitExam,
    lsndSCICONM_HitMisc,
    lsndSCICONM_HitPizza,
    lsndSCICONM_PopUp1,
    lsndSCICONM_PopUp2,
    lsndSCICONM_PopUp3
}






public enum ASSList
{
    ssndEFFECTS_ACHIEVEMENTUNLOCKED,
    ssndEFFECTS_ACHIEVEMENTUNLOCKED2,
    ssndEFFECTS_CROWDMURMUR,
    ssndEFFECTS_CROWDROAR1,
    ssndEFFECTS_CROWDROAR2,
    ssndEFFECTS_BIGJACKETWHOOSH,
    ssndEFFECTS_BIGJACKETSLAM,
    ssndEFFECTS_CHUG,
    ssndEFFECTS_CHUGLASTDROP,
    ssndEFFECTS_ICONIN,
    ssndEFFECTS_ICONOUT,
    ssndEFFECTS_KABOOM,
    ssndEFFECTS_PIZZAEAT,
    ssndEFFECTS_PIZZAREADY,
    ssndEFFECTS_POUR,
    ssndEFFECTS_PUNCH,
    ssndEFFECTS_PUSH1,
    ssndEFFECTS_SMACK1,
    ssndEFFECTS_SMACK2,
    ssndEFFECTS_SNATCH1,
    ssndEFFECTS_SPLATTER,
    ssndEFFECTS_TOPPLE,
    ssndEFFECTS_TOSSBEER,
    ssndEFFECTS_TOSSPIZZA,
    ssndEFFECTS_WHOOSH1,
    ssndEFFECTS_WHOOSH2,
    ssndMENU_DECORATEREPEAT,
    ssndMENU_DROP,
    ssndMENU_GAMEINIT,
    ssndMENU_LOADREPEAT,
    ssndMENU_PLACEBAR,
    ssndMENU_SELECT,
    ssndMENU_TITLEREPEAT,
    ssndMENU_TOGGLE,
    ssndWATER_DRIP,
    ssndWATER_RIPPLE,
    ssndWATER_SPLASHBIG,
    ssndWATER_SPLASHMID,
    ssndWATER_SPLASHSMALL,
    ssndWATER_HOSE
}
