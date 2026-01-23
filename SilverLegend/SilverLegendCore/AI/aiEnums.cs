
/////////////////////////////////////////////////////////////////////////
// Attributes
/////////////////////////////////////////////////////////////////////////
// Integer attributes -- Frosh
public enum nattrCrowd
{
    attrFAction,
    attrFEnergy,
    attrFCanStartWave
}

public enum attrCrowdActions
{
    faMilling,
    faCheering,
    faSlamming,
    faBooing,
    faBlocking,
    faShouting,
    faLookUp,
    faLookULR,
    faLookURL,
    faStayinAlive,
    faWave,
    faPart
}

public enum nattrFrosh
{
    attrBehavior,
    attrGoal,
    attrMotivation,
    attrStr,
    attrFrame,
    attrPersonality,
    attrUpperLevelGoal,
    attrMindSet,
    attrPyramidLevel,
    attrHeightOfFall
}

public enum skintones
{
    skinWhite,
    skinBrown,
    skinBlack,
    skinYellow,
    skinPale
}
public enum motivationlevels
{
    motivationLow,
    motivationSkeptical,
    motivationHigh
}
public enum mindsets
{
    mindsetMotivated,
    mindsetGullible,
    mindsetHungry,
    mindsetThirsty,
    mindsetDrunk
}

// Boolean Attributes -- Frosh
public enum battrFrosh
{
    attrExcited,
    attrLookingLeft,
    attrLookingAtScreen,
    attrWeightOnShoulders,
    attrThirsty,
    attrHungry
}
// Integer attributes -- Frecs
public enum nattrFrecs
{
    attrExcitement
}
// Integer attributes -- Arm
public enum attrArm
{
    attrArmStatus, // Nothing, apple, Pizza, Mug, exam, grease, push, ring, end throwx
    attrArmAction, // Nothing, bobbing, throwing
    attrUnknown1,
    attrUnknown2,
    attrUnknown3,
    attrKickback
}
// Integer attributes -- Apple
public enum attrProjectile
{
    attrStartX,
    attrStartY,
    attrProjectileType,
    attrHitTarget,
    attrPowerOfThrow
}
public enum projTypes
{
    projApple,
    projGrease,
    projClark,
    projPizza,
    projExam
}
public enum battrMenuStartButtonAttributes
{
    attrMakeTitleSoundPlay,
    attrDoNotActivate

}
public enum nattrExitAndCredits
{
    attrCreditsScreen
}
public enum attrAppleHitTargetConstants
{
    attrFlyingTowardTarget,
    attrFlyingRebounding,
    attrFlyingFloatingInPit
}

public enum attrPrez
{
    attrPrezAction
}

// Integer attributes -- FlyInAndOut
public enum attrFlyInAndOut
{
    attrUnused1,
    attrUnused2,
    attrUnused3,
    attrUnused4,
    attrUnused5,
    attrUnused6,
    attrVelocityX,
    attrVelocityY
}

public enum attrBar
{
    attrBarGroup,
    attrOnJacketPosition,
    attrOnScreenX,
    attrOnScreenY,
    attrAssociatedSound,
    attrAssociatedDiscipline
}
public enum battrBar
{
    battrBeingDragged
}
public enum attrToggleButton
{
    attrButtonNumber,
    attrOnScreenPosition
}

public enum attrJacketSlam
{
    attrNextState
}

public enum nattrForge
{
    attrForgeSwing,
    attrForgeMotion,
    attrForgeEnergy
}


// Integer attributes -- Icon
public enum attrIcon
{
    attrIconStatus,
    attrPointsRequired,
    attrButtonType
}
// Integer attributes -- Tam
public enum attrTam
{
    attrNailsInTam,
    attrTamStatus  // Sitting, Being Yanked, Flying
}
public enum attrForeGroundPopUpDudes
{
    attrRelativeSpeed,
    attrEntryType,
    attrOnScreenType,
    attrExitType,
    attrWeaponBeingTaken
}
public enum battrForeGroundPopUpDudes
{
    attrBeingAttacked,
    attrSciConBehavior,
    attrMale
}

/////////////////////////////////////////////////////////////////////////
// Goals
/////////////////////////////////////////////////////////////////////////
public enum Goals
{
    goalMINDLESS_WANDERING,
    goalPYRAMID_SPOT,  // Moving towards a pyramid spot
    goalCLIMBING_UP,  // Climbing to next level
    goalBOOSTING_UP,  // Providing a boost up
    goalBOOSTED_UP,  // About to be boosted up
    goalCLARK,  // Clark Mug o' Beer
    goalPIZZA,  // 'Za
    goalARTSCI,  // ArtSci
    goalCOMMIE,// Commie
    goalTHINK,// Think about what's going wrong.
    goalMOSH
}  // We be jammin'.

public enum UpperLevelGoals
{
    upperGoalCling,   // Cling to the Pole if you can get there
    upperGoalClimb,   // Climb over the shoulders of the first person you meet
    upperGoalSupport
}// Raise your arms so that people can climb over you

/////////////////////////////////////////////////////////////////////////
// Misc
/////////////////////////////////////////////////////////////////////////
public enum Personalities
{
    persGoofy, persHeavyweight, persHoister, persClimber
}

public enum ArmPositions
{
    armNOTHING, armAPPLE, armPIZZA, armCLARK, armEXAM, armGREASE,
    armPUSH, armIRONRING, armOTHROW, armSTHROW, armSTHROW2, armSTHROW3, armCHANGING, armDEMO
}

public enum Buttons
{
    buttonTAUNT, buttonAPPLE, buttonPIZZA, buttonCLARK, buttonEXAM,
    buttonGREASE, buttonRING
}




public enum SpriteType
{
    // GAME
    sprgame_START, // MARKER
    sprRIPPLE, sprCLOUDS, sprTREES, sprBACKDROP, sprPREZ, sprFORGE,
    sprPODIUM, sprPOLE, sprFROSH, sprPOWERMETER, sprWATERMETER, sprRINGMETER, sprCONSOLE, sprGRID,
    sprMOUSECURSORTL, sprMOUSECURSORTR, sprMOUSECURSORBR, sprMOUSECURSORBL, sprAPPLEICON, sprPIZZAICON, sprGREASEICON, sprCLARKICON,
    sprEXAMICON, sprRINGICON, sprTAUNTBUTTON, sprPOINTSONES, sprWATER,
    sprAPPLE_TENS, sprAPPLE_ONES, sprPIZZA_TENS, sprPIZZA_ONES,
    sprCLARK_TENS, sprCLARK_ONES, sprEXAM_TENS, sprEXAM_ONES,
    sprARM, sprARMRING1, sprARMRING2, sprARMRING3, sprCLOSEUPBEER, sprAPPLE, sprGREASE, sprTAM, sprTRACKERUL, sprTRACKERLR, sprWHAP,
    sprEXAM, sprCLARK, sprPIZZA, sprSPLASHS, sprSPLASHM, sprSPLASHML, sprSPLASHL, sprINANIMATE,
    sprFRECGROUP, sprFRECACTION, sprSCICONM, sprSCICONF, sprUPPERYEAR, sprPOOF,
    sprGWBALLOON, sprGWHIPPO, sprHOSEWHAP, sprGWWORDBUBBLE, sprHIGHSCORE, sprPOPBOYINCROWD, sprPOPBOY,
    sprPITTIME_HONES, sprPITTIME_HTENS, sprPITTIME_MONES, sprPITTIME_MTENS,
    sprPITTIME_SONES, sprPITTIME_STENS,
    sprPOPUP_APPLES, sprPOPUP_PIZZA, sprPOPUP_BEER, sprPOPUP_EXAM, sprPOPUP_HOSE, sprPOPUP_ARTSCIM,
    sprPOPUP_ARTSCIF, sprPOPUP_COMMIEM, sprPOPUP_COMMIEF, sprPOPUP_ENGINEER, sprACHIEVEMENTUNLOCKED, sprACHIEVEMENTUNLOCKEDTEXT1, sprACHIEVEMENTUNLOCKEDTEXT2, sprTRI, sprPUB, sprBAN,
    sprgame_END, // MARKER

    // TRANS
    sprtrans_START, // MARKER
    sprSLAMJACKET,
    sprtrans_END, // MARKER

    // MENU
    sprmnu_START, // MARKER

    sprmnuJACKETBACK, sprmnuTITLEBACK, sprmnuOPTIONSBACK, sprmnuOPTIONSRETURN,

    sprmnuDECORATERETURN,
    sprmnuTITLEOPTIONS, sprmnuTITLESTART, sprmnuTITLEEXIT,

    sprmnuMOUSECURSOR,
    sprmnuBTNTOGGLE0, sprmnuBTNTOGGLE1, sprmnuBTNTOGGLE2,
    sprmnuBTNTOGGLE3, sprmnuBTNTOGGLE4,
    sprmnuTXTSELECT, sprmnuMENUJACKET,
    sprmnuMENUPASSCREST,
    sprmnuBAR1, sprmnuBAR2, sprmnuBAR3, sprmnuBAR4, sprmnuBAR5, sprmnuBAR6,
    sprmnuBAR7, sprmnuBAR8, sprmnuBAR9, sprmnuBAR10, sprmnuBAR11,
    sprmnuBAR12, sprmnuBAR13, sprmnuBAR14, sprmnuBAR15,
    sprmnuBAR16, sprmnuBAR17, sprmnuBAR18, sprmnuBAR19,
    sprmnuBAR20, sprmnuAIPREVBARSCREEN, sprmnuAINEXTBARSCREEN, sprmnuAINEXTACHIEVEMENTSCREEN,
    sprmnuACHIEVEMENTTEXT, sprmnuACHIEVEMENTADDITIONALTEXT,

    sprmnu_END // MARKER

}

