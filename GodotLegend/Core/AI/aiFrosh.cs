using System;

public static partial class AIMethods
{
//***********************************************
// (1) Free Falling
//***********************************************
public static void aiAct1(TSprite s)
{
	aisPlummet(s);
	s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 0;

	// Prevent going offscreen
	//aisKeepInPitX(s);
	aisKeepInPitY(s);
	
	if (s.nZ < dBELLYBUTTONZ && s.nvZ < 0) {
		// The frosh has hit the water
		s.nZ = 0;
		if (s.nvZ < -dSPEEDFORBIGSPLASH)
            ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), s.nX, s.nY));
		else
            ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHM), s.nX, s.nY));
        ssWater.Include(SpriteInit.CreateSprite((SpriteType.sprRIPPLE), s.nX, s.nY));
		aiInit3(s);
	}
}

public static void aiInit1(TSprite s) {
	
	// Release the Frosh's claim to a spot.
    if (!(s.ppChosen == null) && s.ppChosen.GetClaimer() == s)
    { // Short circuit important
        s.ppChosen.ReleaseClaim();
        s.ppChosen = null;
    }

	s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 0;
	s.nvX = randintin(- dTHREE, dTHREE);
	s.nvY = randintin(- dONE, dONE);
	s.nvZ = randintin(0, dSIX);
	if (0 == R.Next(6)) { 
		// SEND THE FROSH _REALLY_ FLYING
		s.nvX = randintin(- dTEN, dTEN);
		s.nvY = randintin(- dSIX, dSIX);
		s.nvZ = randintin(0, dTEN * 3);
	}
	
	s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR1_1) + R.Next(nsprFR1)]);
	// Select a falling frosh image
	s.nCC = 0;                          // Increases when frosh hits water
	s.SetBehavior(aiAct1);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 1;
	
	
}

//***********************************************
// (2) Leaping into Pit
//***********************************************
public static void aiAct2(TSprite s)
{
	// Looks distictly like aiAct1...
	s.SetBehavior(aiAct1);
}

public static void aiInit2(TSprite s)
{
	s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 0;
	s.nvX = randintin(dONE, dTEN * 2);
	s.nvY = randintin(dONE, dTEN);
	s.nvZ = randintin(0, dTEN * 4);
	s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR2_1) + R.Next(nsprFR2)]);
	if (0 == R.Next(nsprFR2 + 1))
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR1_1) + R.Next(nsprFR1)]);
	// Select a falling frosh image -- change to a spr2.
	s.nCC = 0;                          // Increases when frosh hits water
	s.SetBehavior(aiAct2);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 2;
}


//***********************************************
// (3) Underwater
//***********************************************
public static void aiInit3(TSprite s) {

    if (s.nAttrib[(int)nattrFrosh.attrHeightOfFall] > (dTAMZ - 10))
        aisUnlockAchievement(747);
    s.nAttrib[(int)nattrFrosh.attrHeightOfFall] = 0;

	s.nvX = randintin(- dSIX, dSIX);
	s.nvY = randintin(- dTHREE, dTHREE);
	s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR3_1) + R.Next(nsprFR3)]);
	// Select a bobbing tam image
	s.SetBehavior(aiAct3);

	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 3;
	
}


public static void aiAct3(TSprite s) {
	if (0 == ((s.nCC) % 2)) {
		s.nX += (s.nvX + randintin(- dTWO, dTWO)) / 2;
		s.nY += (s.nvY + randintin(- dONE, dONE)) / 2;
	}
	
	if (R.Next(50) == 0) {
        ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHS), s.nX, s.nY));
		if (R.Next(10) != 0)
			aiInit4(s);
		else
			aiInit6C(s); // Every once in a while, they swim
	}
	if ((0 == R.Next(30)))                   // Occasionally toggle the image
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR3_1) + R.Next(nsprFR3)]);
	// Select a bobbing tam image
	
	aisKeepInPitX(s);
	aisKeepInPitY(s);    
}

//************************************************
// (4) Wade Through Pit Towards Target -- the hub
//************************************************

public static void aiInit4(TSprite s)  {
	// Initialize the HUB of the AI
	// IMPORTANT: Do NOT set a goal in the init function!

	s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 0;
	s.nZ = 0; s.nvZ = 0;
	
	// Set speed as a function of strength
	// If the frosh is excited, they get a performance boost.
	int nStrengthFudged = s.nAttrib[((int)nattrFrosh.attrStr)] + 1;
	s.nvX = dONE + (nStrengthFudged / 3);
	if (s.bAttrib[((int)battrFrosh.attrExcited)]) {
		s.nvX = s.nvX * 3;
	}

	s.nvY = dONE + (nStrengthFudged / 4);
	
	// Randomly make them excited (for variety).
	s.bAttrib[((int)battrFrosh.attrExcited)] = Math.Abs(s.nvX) > 2;
	
	// Set the frame counter to 0 (the first frame).
	s.nAttrib[((int)nattrFrosh.attrFrame)] = 0;
	// Set to a default Frame (the first one) for safety's sake.
	s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR4A_1)]);
	
	// CycleCounter used to toggle images
	s.nCC = 0;
	
	s.SetBehavior(aiAct4);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 4;

}

public static void aiAct4(TSprite s)  {
	// HUB OF AI
    if (s.nX < dPITMINX - 300) s.nX = dPITMINX - 300;
    if (s.nX > dPITMAXX + 300) s.nX = dPITMAXX + 300;

	// Generate random ripples
	if (Math.Abs(s.nvX) > 4) { 
		if (0 == (R.Next(1000) % (dSIX)) && 0 != Globals.myGameConditions.GetEnhancedGraphics())
            ssWater.Include(SpriteInit.CreateSprite((SpriteType.sprRIPPLE), s.nX, s.nY));
	}
	else {
        if (0 == (R.Next(1000) % (dTEN)) && 0 != Globals.myGameConditions.GetEnhancedGraphics())
            ssWater.Include(SpriteInit.CreateSprite((SpriteType.sprRIPPLE), s.nX, s.nY));
	}
	
	// Move Frosh towards destination + Flying Frosh Bug Correction
	if (s.nDestX < dPITMINXMINUS50) s.nDestX = dPITMINXMINUS50;
	else if (s.nDestX > dPITMAXXPLUS50) s.nDestX = dPITMAXXPLUS50;
	if (s.nDestY < dPITMINY) s.nDestY = dPITMINY;
	else if (s.nDestY > dPITMAXY) s.nDestY = dPITMAXY;
	s.nvZ = 0; s.nZ = 0;
	aisKeepInPitY(s);    
	aisMoveTowardsDestination(s);
	
	if ((Math.Abs(s.nX - s.nDestX) <= s.nvX) && (Math.Abs(s.nY - s.nDestY) <= s.nvY)) { 
		// WHEN THE FROSH REACHES THEIR GOAL
		
		// Force them (near)perfectly into position
		s.nX = s.nDestX; s.nY = s.nDestY;
		
		// Decide what to do, based on their Goal attribute.
		switch (s.nAttrib[((int)nattrFrosh.attrGoal)]) { 
		case ((int)Goals.goalTHINK):
			aiInit6D(s);
			break;
		case ((int)Goals.goalMINDLESS_WANDERING):     // Mindless wandering
			if (0 == R.Next(5))
				aiInit5C(s);             // Splash the dude!
			else
				aisChooseFroshPitGoal(s);      // Have the Frosh choose a new goal.
			break;
		case ((int)Goals.goalPYRAMID_SPOT):           // Moving towards a pyramid spot
			
			// Assess if any other frosh is in this spot
			if (s.ppChosen == null) {
				//Globals.Debug("ppChosen was null.  How did I get here?");
				aisChooseFroshPitGoal(s);      // Have the Frosh choose a new goal.
			}	
			else if (s.ppChosen.GetClaimer() == s) {
				aiInit7A(s);   // If not, switch to pyramid behavior.
			}
			else {
				aisChooseFroshPitGoal(s);      // Have the Frosh choose a new goal.
			}
			break;
		case ((int)Goals.goalCLIMBING_UP):            // Climbing to next level
			// Here's where they climb out of the pit

			// PERFORMANCE BOOST: ?: DON'T LET THEM CLIMB IF IT WILL TOPPLE THE PYRAMID
			if ((0 == (Globals.myGameConditions.GetBooster(5))) 
				|| ((nFroshAbove1) < nFroshLevel[1]))
				aiInit9A(s, (s.nY < dPOLEY)); // Switch to climbing behavior.
			else
				aisChooseFroshPitGoal(s);      // Have the Frosh choose a new goal.
			break;
		case ((int)Goals.goalBOOSTING_UP):            // Providing a boost up
			// Change to boosting behavior.
			// Toggle the behavior of a few frosh from ((int)Goals.goalMINDLESS_WANDERING) to ((int)Goals.goalBOOSTED_UP)
			break;
		case ((int)Goals.goalBOOSTED_UP):             // About to be boosted up
			// Assess if any boosters are in this spot
			// If so, switch them to "breakout" behavior, and switch me to leaping behavior.
			// Else,  aisChooseFroshPitGoal.
			break;
		case ((int)Goals.goalCLARK):              // Clark Mug o' Beer
			// Assess if a Clark Mug is in this spot
			// If so, erase it and switch me to boozin' behavior.
			s.nX = s.nDestX;
			s.nY = s.nDestY;
			if (aisConsume(s, true))
				aiInit5B(s);
			else
				aisChooseFroshPitGoal(s);
			// Else, reset aiAct4
			break;
		case ((int)Goals.goalPIZZA):                  // 'Za
			// Assess if a Pizza is in this spot
			// If so, erase it and switch me to eatin' behavior.
			s.nX = s.nDestX;
			s.nY = s.nDestY;
			if (aisConsume(s)) 
				aiInit5A(s);
			else 
				aisChooseFroshPitGoal(s);
			// Else, reset aiAct4
			break;
		case ((int)Goals.goalARTSCI):                 // ArtSci
			// Assess if an ArtSci is in this spot
			if (sprAlien == null) {
				aisChooseFroshPitGoal(s);
			}
			else {
				// If so, set me to splashing behaviour and make the artsci block
				int nTemp;
				if (s.nX < sprAlien.nX)
					nTemp = Math.Abs((sprAlien.nX -dARTSCISPLASHINGOFFSETX) - s.nX);
				else
					nTemp = Math.Abs((sprAlien.nX +dARTSCISPLASHINGOFFSETX) - s.nX);
				s.bAttrib[((int)battrFrosh.attrLookingLeft)] = sprAlien.nX < s.nX;
				if (nTemp < 20) { 
					sprAlien.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;
					aiInit5C(s);
				}
				else
				// Else,  look for an ArtSci.
					s.nDestX = ((sprAlien.nX < s.nX) ? sprAlien.nX - dARTSCISPLASHINGOFFSETX : sprAlien.nX + dARTSCISPLASHINGOFFSETX);
			}
			break;
		case ((int)Goals.goalCOMMIE):                 // Commie
			// Assess if an ArtSci is in this spot
			if (sprAlien == null) {
				aisChooseFroshPitGoal(s);
			}
			else {
				// If so, set me to splashing behaviour and make the artsci block
				int nTemp;
				if (s.nX < sprAlien.nX)
					nTemp = Math.Abs((sprAlien.nX -dCOMMIEPUNCHINGOFFSETX) - s.nX);
				else
					nTemp = Math.Abs((sprAlien.nX +dCOMMIEPUNCHINGOFFSETX) - s.nX);
				s.bAttrib[((int)battrFrosh.attrLookingLeft)] = sprAlien.nX < s.nX;
				if (nTemp < 20) { 
					sprAlien.bAttrib[((int)battrForeGroundPopUpDudes.attrBeingAttacked)] = true;
					aiInit5D(s);
				}
				else
				// Else,  look for a Commie.
					s.nDestX = ((sprAlien.nX < s.nX) ? sprAlien.nX - dCOMMIEPUNCHINGOFFSETX : sprAlien.nX + dCOMMIEPUNCHINGOFFSETX);
			}
			break;
		case ((int)Goals.goalMOSH):
			aiInit6B(s);
			break;
		default:
			// Gave an goal-less frosh a sense of direction
			aisChooseFroshPitGoal(s);
			break;
		}
	}                                    // { END Frosh has reached goal }
	else { 
		// Frosh is moving towards destination.  Choose an image.
		
		
		// 13 should be a prime number greater than 6
		
		if (0 == (s.nCC % (13 - s.nvX))) {  
			// Toggle image as a function of frosh's speed
			
			s.nAttrib[((int)nattrFrosh.attrFrame)]++;
			
			if ((s.bAttrib[((int)battrFrosh.attrExcited)])) { 
				// If the Frosh is excited
				
				// Cycle through to next frame
				if (s.nAttrib[((int)nattrFrosh.attrFrame)] >= nsprFR4E)
					s.nAttrib[((int)nattrFrosh.attrFrame)] = 0;
				
				if (s.nDestX < s.nX) {
					// Show left-wards excited image
					s.bAttrib[((int)battrFrosh.attrLookingLeft)] = true;
					s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR4B_1) + s.nAttrib[((int)nattrFrosh.attrFrame)]]);
				}
				else {
					// Show right-wards excited image
					s.bAttrib[((int)battrFrosh.attrLookingLeft)] = false;
					s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR4B_1) + s.nAttrib[((int)nattrFrosh.attrFrame)]]);
				}
			}
			else { 
				// Else, the Frosh is NOT excited
				
				// Cycle through to next frame
				if (s.nAttrib[((int)nattrFrosh.attrFrame)] >= nsprFR4)
					s.nAttrib[((int)nattrFrosh.attrFrame)] = 0;
				
				if (s.nDestX < s.nX) {
					// Show left-wards NON-excited image
					s.bAttrib[((int)battrFrosh.attrLookingLeft)] = true;
					s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR4A_1) + s.nAttrib[((int)nattrFrosh.attrFrame)]]);
				}
				else {
					// Show right-wards NON-excited image
					s.bAttrib[((int)battrFrosh.attrLookingLeft)] = false;
					s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR4A_1) + s.nAttrib[((int)nattrFrosh.attrFrame)]]);
				}
			}
		}
	}
}

//************************************************
// (5) Goofin' Around
//************************************************



public static void aiInit5A(TSprite s)  {
	// Initialize Eating Pizza
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 5;
	s.nvX = 0;	s.nvY = 0; s.nvZ = 0;
	s.nCC = 0;
	s.nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalMINDLESS_WANDERING);
	s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5A_1)] : frm[((int)GameBitmapEnumeration.bmpFR5A_1)]);
	s.SetBehavior(aiAct5A);
}

public static void aiAct5A(TSprite s)  {
	// Eat Pizza
	
	if (0 == (s.nCC % timePIZZAMUNCH)) { 
		switch (s.nCC / timePIZZAMUNCH) { 
		case 1:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5A_2)] : frm[((int)GameBitmapEnumeration.bmpFR5A_2)]);
			if (!sSound[((int)ASSList.ssndEFFECTS_PIZZAEAT)].IsPlaying())
				sSound[((int)ASSList.ssndEFFECTS_PIZZAEAT)].Play(SoundbankInfo.volNORMAL, panONX(s));
			break;
		case 5:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5A_2)] : frm[((int)GameBitmapEnumeration.bmpFR5A_2)]);
			if (0 != R.Next(nPIZZAMUNCHAVERAGE))
				s.nCC = timePIZZAMUNCH;
			else
				aiInit4(s);
			break;
		case 2:
		case 4:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5A_3)] : frm[((int)GameBitmapEnumeration.bmpFR5A_3)]);
			break;
		case 3:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5A_4)] : frm[((int)GameBitmapEnumeration.bmpFR5A_4)]);
			break;
		}
	}
}

public static void aiInit5B(TSprite s)  {
	// Initialize Drinking Beer
	// THIS IS A BLAST OF AIINIT5A
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 5;
	s.nvX = 0;	s.nvY = 0; s.nvZ = 0;
	s.nCC = 0;
	s.nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalMINDLESS_WANDERING);
	s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5B_1)] : frm[((int)GameBitmapEnumeration.bmpFR5B_1)]);
	s.SetBehavior(aiAct5B);
}

public static void aiAct5B(TSprite s)  {
	// Drink Beer
	
	if (0 == (s.nCC % 3)) { 
		switch (s.nCC / 3) { 
		case 1:
			sSound[((int)ASSList.ssndEFFECTS_CHUG)].Play(SoundbankInfo.volNORMAL, panONX(s)); 
			NOSPEECHFOR(50);
            break;
			
		case 3:
		case 5:
		case 7:
		case 9:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5B_2)] : frm[((int)GameBitmapEnumeration.bmpFR5B_2)]);	break;
		case 2:
		case 4:
		case 6:
		case 8:
		case 10:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5B_3)] : frm[((int)GameBitmapEnumeration.bmpFR5B_3)]);	break;
		case 12:  // Last chug 1/2 
            if (0 != R.Next(2))
				sSound[((int)ASSList.ssndEFFECTS_CHUGLASTDROP)].Play(SoundbankInfo.volNORMAL, panONX(s));
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5B_4)] : frm[((int)GameBitmapEnumeration.bmpFR5B_4)]);	
			break;
		case 15:  // Last chug 2/2
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5B_5)] : frm[((int)GameBitmapEnumeration.bmpFR5B_5)]);	
			break;
		case 20:  // Holds glass up
            if (0 != R.Next(6))
				lSound[((int)ASLList.lsndFROSH_CLARKFINISH1) + R.Next(SoundbankInfo.nsndFROSH_CLARKFINISH)].Play(SoundbankInfo.volHOLLAR, panONX(s));
			else
				lSound[((int)ASLList.lsndFROSH_CLARKFINISH3)].Play(SoundbankInfo.volHOLLAR, panONX(s));
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR5B_6)] : frm[((int)GameBitmapEnumeration.bmpFR5B_6)]);	
			break;
		case 38:
            if (0 != R.Next(2))
				aiInit6A(s);
            else if (0 != R.Next(3))
				aiInit4(s);
			else
				aiInit6B(s);
			break;
		}
	}
}

public static void aiInit5C(TSprite s)  {
	// Initialize Splashing water
	
	s.nvX = 0;
	s.nvY = 0;
	s.nvZ = 0;
	if (s.bAttrib[((int)battrFrosh.attrLookingLeft)])
		s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR5C_1)]);
	else
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR5C_1)]);
	s.nCC = 0;
	s.SetBehavior(aiAct5C);
}

public static void aiAct5C(TSprite s)  {
	// Splash water (also formerly 6E)
	
//#define WATER_SPLASHING_SPEED 4
	if (0 == (s.nCC % 4)) { 
		switch (s.nCC / 4) { 
		case 1:
		case 3:
		case 5:
		case 7:
			if (s.bAttrib[((int)battrFrosh.attrLookingLeft)]) {
                ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHML), 
					s.nX - 20 + randintin(- 15, 15), s.nY + randintin(- 2, 2)));
				s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR5C_2)]);
			}
			else {
                ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHM), 
					s.nX + 20 + randintin(- 15, 15), s.nY + randintin(- 2, 2)));
				s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR5C_2)]);
			}
			break;
		case 2:
		case 4:
		case 6:
		case 8:
			if (s.bAttrib[((int)battrFrosh.attrLookingLeft)]) {
                ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHML), 
					s.nX - 20 + randintin(- 15, 15), s.nY + randintin(- 2, 2)));
				s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR5C_1)]);
			}
			else {
                ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHM), 
					s.nX + 20 + randintin(- 15, 15), s.nY + randintin(- 2, 2)));
				s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR5C_1)]);
			}
			break;
		case 9:
			aisChooseFroshPitGoal(s);
			aiInit4(s);
			break;
		}
	}
}
public static void aiInit5D(TSprite s)  {
	// Initialize Splashing water
	
	if (s.bAttrib[((int)battrFrosh.attrLookingLeft)])
		s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR5D_1)]);
	else
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR5D_1)]);
	s.nvX = 0;
	s.nvY = 0;
	s.nvZ = 0;
	s.nCC = 0;
	s.SetBehavior(aiAct5D);
}

public static void aiAct5D(TSprite s)  {
	// Punch Commie
	
//#define COMMIE_PUNCHING_SPEED 8
	if (0 == (s.nCC % 8)) { 
		switch (s.nCC / 8) { 
		case 1:	case 3:	case 5:	
			if (!(sSound[((int)ASSList.ssndEFFECTS_PUNCH)].IsPlaying()))
				sSound[((int)ASSList.ssndEFFECTS_PUNCH)].Play(SoundbankInfo.volNORMAL, panONX(s));
			if (s.bAttrib[((int)battrFrosh.attrLookingLeft)]) {
				s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR5D_2)]);
			}
			else {
				s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR5D_2)]);
			}
			break;
		case 2:	case 4:	case 6:	
			if (s.bAttrib[((int)battrFrosh.attrLookingLeft)]) {
				s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR5D_1)]);
			}
			else {
				s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR5D_1)]);
			}
			break;
		case 7:
			aisChooseFroshPitGoal(s);
			aiInit4(s);
			break;
		}
	}
}

public static void aiInit6A(TSprite s)  {
	// Prepare for some drunken singing
	s.nvX = 0;	s.nvY = 0;	s.nvZ = 0;
	s.nX = s.nDestX; s.nY = s.nDestY;
	s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR6A_1)]); s.nCC = 0;
	s.SetBehavior(aiAct6A);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 6;
	NOSPEECHFOR(15);

}
public static void aiAct6A(TSprite s)  {
	// Drunken Singing
	int nTemp = s.nAttrib[((int)attrProjectile.attrPowerOfThrow)]; // attrPowerOfThrow is not a Frosh attribute
	s.nAttrib[((int)attrProjectile.attrPowerOfThrow)] = 10; // Temporarily make the Frosh a doozie to hit
	//aisCollisionToResponse(s, ssFr, aisSendFroshReallyFlying, 
	//	NOWHAP, NOPOLESHIELDING, false);
	s.nAttrib[((int)attrProjectile.attrPowerOfThrow)] = nTemp;
	
//#define SINGING_SPEED 6
	switch (s.nCC / 6) { 
	case 1:	case 3:	case 5:	case 7:	case 9:
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR6A_2)] : frm[((int)GameBitmapEnumeration.bmpFR6A_2)]);	break;
	case 2:	case 4:	case 6:	case 8:	case 10:
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR6A_1)] : frm[((int)GameBitmapEnumeration.bmpFR6A_1)]);	break;
	case 14:
		aiInit4(s);
		break;
	}
}
public static void aiInit6B(TSprite s)  {
	// Prepare to mosh
	s.nvX = 0;	s.nvY = 0;	s.nvZ = 0;
	s.nX = s.nDestX; s.nY = s.nDestY;
	s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR6B_1)]);
	s.SetBehavior(aiAct6B);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 6;
	s.nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalMINDLESS_WANDERING);
	s.bAttrib[((int)battrFrosh.attrLookingLeft)] = true;
}
public const int timeMOSHSPEED = 69;
public static void aiAct6B(TSprite s)  {
	if (s.nCC > 400) {
		aiInit4(s);
	}
	else {
		switch (sprPole.nCC % timeMOSHSPEED) {
		case 4:	case 24:
			s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR6B_2)]);
			break;
		case 16: case 32:
			s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR6B_1)]);
			break;
		case 40: case 56:
			s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR6B_2)]);
			break;
		case 48: case 64:
			s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR6B_1)]);
			break;
		}
	}
}

// Swim through the pit
public static void aiInit6C(TSprite s)  {
	s.nvX = dSWIMSPEED; s.nvY = 0; s.nvZ = 0;
	s.nCC = 0;
	s.SetBehavior(aiAct6C);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 6;
	s.bAttrib[((int)battrFrosh.attrLookingLeft)] = s.nX > dPOLEX;
	s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR6C_1)] : frm[((int)GameBitmapEnumeration.bmpFR6C_1)]);	
	if (s.bAttrib[((int)battrFrosh.attrLookingLeft)])
		s.nDestX = dPITMINX - dSWIMDISTANCE;
	else
		s.nDestX = dPITMAXX + dSWIMDISTANCE;
}
public static void aiAct6C(TSprite s)  {
	aisMoveTowardsDestination(s);
	if (Math.Abs(s.nX - s.nDestX) <= s.nvX) {
        ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHM), s.nX, s.nY));
		aiInit4(s);  
	}

	if (0 == (s.nCC % dSWIMFRAMERATE)) {
        ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHM), 
			s.nX + (s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? - 70 : 70), s.nY + randintin(-2, 2)));

		if (0 == (s.nCC % (dSWIMFRAMERATE * 2)))
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR6C_1)] : frm[((int)GameBitmapEnumeration.bmpFR6C_1)]);
		else
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR6C_2)] : frm[((int)GameBitmapEnumeration.bmpFR6C_2)]);
	}
}


// THINK
public static void aiInit6D(TSprite s)  {
	s.nvX = 2 + R.Next(3);	s.nvY = 1;	s.nvZ = 0;
	if (0 != R.Next(2))
		s.SetFrame(0 != R.Next(2) ? frm[((int)GameBitmapEnumeration.bmpFR6D_1)] : frmM[((int)GameBitmapEnumeration.bmpFR6D_1)]); 
	else
		s.SetFrame(0 != R.Next(2) ? frm[((int)GameBitmapEnumeration.bmpFR6D_2)] : frmM[((int)GameBitmapEnumeration.bmpFR6D_2)]); 
	s.bAttrib[((int)battrFrosh.attrLookingLeft)] = 0 == R.Next(25);
	s.nCC = 0;
	s.SetBehavior(aiAct6D);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 6;
}

public static void aiAct6D(TSprite s)  {
	aisBobUpAndDown(s);
	aisMoveTowardsDestination(s);
	if (s.bAttrib[((int)battrFrosh.attrLookingLeft)]) {
		switch(s.nCC / 4) {
		case 1: case 3: case 5: case 7: case 9: case 11: case 13:
			s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR6D_3)]); break;
		case 2: case 4: case 6: case 8: case 10: case 12: case 14:
			s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR6D_4)]); break;
		}
	}
	if (s.nCC > timeTHINKINGTIME) {
		s.nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalMINDLESS_WANDERING);
		s.bAttrib[((int)battrFrosh.attrExcited)] = false;
		aiInit4(s);
	}
}

public const int timeANIMALIA = 350;

// Cow-eagles
public static void aiInit6E(TSprite s)  {
    s.UseColorSwap = false;
    if (s.nAttrib[((int)nattrFrosh.attrBehavior)] == -6) // Don't do it again
		return;
	s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 0;
	s.nvX = 0;	s.nvY = 0;	s.nvZ = 0; s.nCC = 0;
    ssConsole.Include(SpriteInit.CreateSprite((SpriteType.sprPOOF), s.nScrx, s.nScry));
	s.bAttrib[((int)battrFrosh.attrLookingLeft)] = (0 == R.Next(2));
	s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frm[((int)GameBitmapEnumeration.bmpFR6E_1)] : frmM[((int)GameBitmapEnumeration.bmpFR6E_1)]); 
	s.SetBehavior(aiAct6E);
	s.nAttrib[((int)nattrFrosh.attrStr)] = R.Next(5) + 3;
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = -6;
}

public static void aiAct6E(TSprite s)  {
    if (0 == R.Next(150))
    {
		int nTemp = R.Next(2);
		if (!lSound[((int)ASLList.lsndFROSH_COW1) + nTemp].IsPlaying())
			lSound[((int)ASLList.lsndFROSH_COW1) + nTemp].Play(SoundbankInfo.volNORMAL, panONX(s));
	}
	if (0 == (s.nCC % s.nAttrib[((int)nattrFrosh.attrStr)])) {
		if (0 == (s.nCC % (s.nAttrib[((int)nattrFrosh.attrStr)]*2)))
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frm[((int)GameBitmapEnumeration.bmpFR6E_2)] : frmM[((int)GameBitmapEnumeration.bmpFR6E_2)]); 
		else
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frm[((int)GameBitmapEnumeration.bmpFR6E_1)] : frmM[((int)GameBitmapEnumeration.bmpFR6E_1)]); 
	}
	s.nZ+= R.Next(2) + 1;
	if (0 == R.Next(200))
		s.bAttrib[((int)battrFrosh.attrLookingLeft)] = !(s.bAttrib[((int)battrFrosh.attrLookingLeft)]);
	if (s.bAttrib[((int)battrFrosh.attrLookingLeft)])
		s.nX += R.Next(3);
	else
		s.nX -= R.Next(3);
	if (s.nCC > timeANIMALIA) {
        ssConsole.Include(SpriteInit.CreateSprite((SpriteType.sprPOOF), s.nScrx, s.nScry));
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR1_1)]);
        s.UseColorSwap = true;
		aiInit1(s);
	}
}

public static void aiInit6F(TSprite s)  {
    s.UseColorSwap = false;
    if (s.nAttrib[((int)nattrFrosh.attrBehavior)] == -6) // Don't do it again
		return;
	s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 0;
	s.nvX = 0;	s.nvY = 0;	s.nvZ = 0; s.nCC = 0;
    ssConsole.Include(SpriteInit.CreateSprite((SpriteType.sprPOOF), s.nScrx, s.nScry));
	s.bAttrib[((int)battrFrosh.attrLookingLeft)] = (0 == R.Next(2));
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = -6;
	s.nAttrib[((int)nattrFrosh.attrStr)] = R.Next(5) + 13;
	if (s.nZ > 0)
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frm[((int)GameBitmapEnumeration.bmpFR6F_1)] : frmM[((int)GameBitmapEnumeration.bmpFR6F_1)]); 
	else
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frm[((int)GameBitmapEnumeration.bmpFR6F_2)] : frmM[((int)GameBitmapEnumeration.bmpFR6F_2)]); 
	s.SetBehavior(aiAct6F);
	if ((0 == R.Next(3)))
		s.nAttrib[((int)nattrFrosh.attrStr)] = R.Next(3) + 3;
	else
		s.nAttrib[((int)nattrFrosh.attrStr)] = R.Next(3) + 1;
}

public static void aiAct6F(TSprite s)  {
	if (0 == R.Next(150)) {
		int nTemp = R.Next(2);
		if (!lSound[((int)ASLList.lsndFROSH_SHEEP1) + nTemp].IsPlaying())
			lSound[((int)ASLList.lsndFROSH_SHEEP1) + nTemp].Play(SoundbankInfo.volNORMAL, panONX(s));
	}
	if (s.nZ > 0)
		aisPlummet(s);
	else if (s.nZ < -1)
		s.nZ = 0;
	else {
		s.nY += (0 == R.Next(4) ? 1 : 0);
		if (0 == (s.nCC % s.nAttrib[((int)nattrFrosh.attrStr)])) {
			if (0 == R.Next(10))
				s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frm[((int)GameBitmapEnumeration.bmpFR6F_2)] : frmM[((int)GameBitmapEnumeration.bmpFR6F_2)]); 
			else
				s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frm[((int)GameBitmapEnumeration.bmpFR6F_3)] : frmM[((int)GameBitmapEnumeration.bmpFR6F_3)]); 
		}
		
		//s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frm[((int)GameBitmapEnumeration.bmpFR6F_2)] : frmM[bmpFR6F_2]); 
		aisBobUpAndDown(s);
		if (s.bAttrib[((int)battrFrosh.attrLookingLeft)])
			s.nX += s.nAttrib[((int)nattrFrosh.attrStr)];
		else
			s.nX -= s.nAttrib[((int)nattrFrosh.attrStr)];
	if (0 == R.Next(200))
        s.bAttrib[((int)battrFrosh.attrLookingLeft)] = !(s.bAttrib[((int)battrFrosh.attrLookingLeft)]);
		
		if (s.nCC > timeANIMALIA) {
            ssConsole.Include(SpriteInit.CreateSprite((SpriteType.sprPOOF), s.nScrx, s.nScry));
			s.nAttrib[((int)nattrFrosh.attrGoal)] = ((int)Goals.goalMINDLESS_WANDERING);
			s.bAttrib[((int)battrFrosh.attrExcited)] = false;
			s.nvZ = 2;	s.nZ = 3;
			s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR1_1)]);
            s.UseColorSwap = true;
			aiInit1(s);
		}
	}
}



//************************************************
// (7) In a Pyramid Spot
//************************************************

public static void aiInit7A(TSprite s)  {
	// Initialize Taking up a pyramid spot
	s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 1;
	s.nvX = 3;
	s.nvY = 3;
	s.nvZ = 0;
	s.nX = s.nDestX;
	s.nY = s.nDestY;
	s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR7A_1)]);
	s.nCC = 0;
	s.SetBehavior(aiAct7A, aiKEEPPOLEPOSITION);
	s.bAttrib[((int)battrFrosh.attrWeightOnShoulders)] = false;
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 7;
}


public static void aiAct7A(TSprite s)  {
	if (s.nCC <= 10) {
		switch(s.nCC) {
		case 4:
			s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR7A_2)]);
			break;
		case 10:
			s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR7A_3)]);
			break;
		}
	}
	else
		aisBobUpAndDown(s);
		// Slowly nudge the frosh towards the pole if room opens up
		if (!(s.bAttrib[((int)battrFrosh.attrWeightOnShoulders)]))
			aisPickCloserPyramidSpot(s);
		aisMoveTowardsDestination(s);
		if (!(s.ppChosen.GetParent() == null))
			if (s.ppChosen.GetParent().PositionIsFree()) {
				s.ppChosen.ReleaseClaim();
				s.ppChosen = s.ppChosen.GetParent();
				s.ppChosen.SetClaim(s);
				s.nDestX = s.ppChosen.GetX();
				s.nDestY = s.ppChosen.GetY();
			}
			else {
				// People around the edges can climb up to the next level.
				// This could be changed to allow anyone without a Claimer in their Child position to go for it.
				if ((s.ppChosen.GetChild() == null) 
					// PERFORMANCE BOOST: FROSH WON'T CLIMB IF THERE'S TOO MANY OTHERS ABOVE
					// START BY NOT EVALUATING THIS; THEN EVALUATE AS t.INFINITY
					&&		((0 == (Globals.myGameConditions.GetBooster(3))) 
							|| ((nFroshAbove1) < nFroshLevel[1]))
					&& (0 == R.Next(100)))                        // This requires strength / morale
				{
					// Get this frosh climbing up
					s.SetGoal(((int)Goals.goalCLIMBING_UP));
					s.SetBehavior(aiAct4, aiKEEPPOLEPOSITION);
					TSprite PersonToClimb;
					PersonToClimb = s.ppChosen.GetParent().GetClaimer();
					s.nDestX = PersonToClimb.nX;
					s.nDestY = PersonToClimb.nY + 1;
					s.ppAbandon();
				}
			}
}


//************************************************
// (9) Climbing over shoulders to a higher level
//************************************************

public const bool NOTLOOKINGATSCREEN = false;
public const bool LOOKINGATSCREEN = true;

public static void aiInit9A(TSprite s)  {
    aiInit9A(s, false);
}

public static void aiInit9A(TSprite s, bool bLookingAtScreen)  {
	// Initialize climbing over shoulders to a higher level -- THIS IS THE ONLY WAY UP
	s.nvX = 3;
	s.nvY = 1;
	s.nvZ = 1;
	s.bAttrib[((int)battrFrosh.attrLookingAtScreen)] = bLookingAtScreen;
	Globals.myGameConditions.AddEnergy(2);

	s.nAttrib[((int)nattrFrosh.attrPyramidLevel)]++; // Just to let everyone else know you're going.
	
	s.nDestY --;
	if (s.nDestY < dPOLEY + 2)
		s.nDestY = dPOLEY + 2;
	
	// A Frosh with their arms up is a bit higher than a frosh in the pit
	if (s.nZ > 30)
		s.nDestZ = s.nZ + 120;
	else
		s.nDestZ = s.nZ + 93;
	

	if (s.nDestZ > dTAMZ && SPEECHOK()) {
		lSound[((int)ASLList.lsndFROSH_ATTOP1) + R.Next(5)].Play(SoundbankInfo.volHOLLAR, panONX(s));
		NOSPEECHFOR(15);
	}

	if (s.nZ > dTAMZ) {
		s.nDestX = dPOLEX;
		s.nDestY = dPOLEY + 2;
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR15_1)] : frm[((int)GameBitmapEnumeration.bmpFR15_1)]);
	}
	else
		if (s.bAttrib[((int)battrFrosh.attrLookingAtScreen)])
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR9B_1)] : frm[((int)GameBitmapEnumeration.bmpFR9B_1)]);
		else
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR9A_1)] : frm[((int)GameBitmapEnumeration.bmpFR9A_1)]);
		
		s.nCC = 0;
		s.SetBehavior(aiAct9A);
		s.nAttrib[((int)nattrFrosh.attrBehavior)] = 9;
}

public static void aiAct9A(TSprite s)  {
	if (s.nZ > dTAMZ + 80)
		aiInit16A(s);

	if (s.nZ < s.nDestZ) {
		s.nZ += dCLIMBINGSPEED;
		if (s.nZ > dTAMZ) {
			aisMoveTowardsDestination(s);
			switch(s.nCC) {
			case 4:
				if (s.bAttrib[((int)battrFrosh.attrLookingAtScreen)])
					s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR9B_2)] : frm[((int)GameBitmapEnumeration.bmpFR9B_2)]);
				else
					s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR15_2)] : frm[((int)GameBitmapEnumeration.bmpFR15_2)]);
				break;
			case 10:
				if (s.bAttrib[((int)battrFrosh.attrLookingAtScreen)])
					s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR9B_3)] : frm[((int)GameBitmapEnumeration.bmpFR9B_2)]);
				else
					s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR15_3)] : frm[((int)GameBitmapEnumeration.bmpFR15_2)]);
				break;
			}
		}
		else
			switch(s.nCC) {
			case 4:
				if (s.bAttrib[((int)battrFrosh.attrLookingAtScreen)])
					s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR9B_2)] : frm[((int)GameBitmapEnumeration.bmpFR9B_2)]);
				else
					s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR9A_2)] : frm[((int)GameBitmapEnumeration.bmpFR9A_2)]);
				break;
			case 10:
				if (s.bAttrib[((int)battrFrosh.attrLookingAtScreen)])
					s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR9B_3)] : frm[((int)GameBitmapEnumeration.bmpFR9B_3)]);
				else
					s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR9A_3)] : frm[((int)GameBitmapEnumeration.bmpFR9A_3)]);
				break;
		}
	}
	else {
		// If the frosh is high enough, have them grab the tam.
		if (s.nZ > dTAMZ)
			aiInit16A(s);
		// Otherwise, start them walking across this level.
		else {
			s.nZ = s.nDestZ;	
			aiInit10(s);
		}
	}
}


//************************************************
// (10) Stumble over necks towards pole
//************************************************

public static void aiInit10(TSprite s)  {
	if (s.nZ < 100)
		s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 2; // Tam Level
	else if (s.nZ < 220)
		s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 3; // Tam Level
	else if (s.nZ < 340)
		s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 4; // Tam Level
	else //if (s.nZ < 460)
		s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 5; // Tam Level

	s.nvX = 1;
	s.nvY = 1;
	s.nvZ = 0;
	s.nCC = 0;
	if (s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] == 2) {
		if (s.nX < dPOLEX)
			s.nDestX = dPOLEX - dUPPERLEVELDISTANCEFROMPOLE - R.Next(30);
		else
			s.nDestX = dPOLEX + dUPPERLEVELDISTANCEFROMPOLE + R.Next(30);	
	}
	else {
		if (s.nX < dPOLEX)
			s.nDestX = dPOLEX - dUPPERLEVELDISTANCEFROMPOLE - R.Next(5);
		else
			s.nDestX = dPOLEX + dUPPERLEVELDISTANCEFROMPOLE + R.Next(5);	
	}
	
	s.bAttrib[((int)battrFrosh.attrLookingLeft)] = (s.nDestX < s.nX);
	
	aisChooseFroshUpperLevelGoal(s);  // Give the Frosh a mission - climb, cling, support.
	
//	if (Math.Abs(s.nDestY - (dPOLEY + 6)) > 6)
//		s.nDestY = dPOLEY + 6;
	
	s.SetBehavior(aiAct10);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 10;
}

public static TSprite  aiClosestToPole(TSprite sprFirst, TSprite sprSecond) {
	// Returns a pointer to the sprite closest to the pole in the x-y plane.
	int nFirst = Math.Abs(sprFirst.nX - dPOLEX) + Math.Abs(sprFirst.nY - dPOLEY);
	int nSecond = Math.Abs(sprSecond.nX - dPOLEX) + Math.Abs(sprSecond.nY - dPOLEY);
	if (nFirst < nSecond)
		return sprFirst;
	else
		return sprSecond;
}

public static void aiAct10(TSprite s)  {
	SpriteSet  Support;
	Support = GetFroshInRange(s.nX - 50, s.nY - (dFROSHARMLINKOFFSETY / 2), s.nX + 50, s.nY + (dFROSHARMLINKOFFSETY / 2));
	int n = Support.GetNumberOfSprites();
	
	aisMoveTowardsDestination(s);
	switch(s.nCC) {
	case 1:
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR10_1)] : frm[((int)GameBitmapEnumeration.bmpFR10_1)]);				  
		break;
	case 10:
	case 30:
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR10_2)] : frm[((int)GameBitmapEnumeration.bmpFR10_2)]);				  
		break;
	case 15:
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR10_3)] : frm[((int)GameBitmapEnumeration.bmpFR10_3)]);				  
		break;
	case 33:
		s.nCC = 0;
		break;
	}
	
	// Make sure the Frosh is supported.
	int bSupport = 0;
	// Determine if anyone else is on the same level with you at this spot
	int bSameLevel = 0;
	TSprite ClosestFroshToPole = null;
	// See if someone's weighing you down.
	int bUnderPressure = 0;
	bool bDoNotClimb = false;
	
	for (int i = 0; i < n; i++) {
		if (!(s == Support.GetSprite(i))) {
			if (Math.Abs (s.nZ - Support.GetZ(i) - 90) < 50) {
				bSupport++;
				
				aisWeightOnShoulders(Support.GetSprite(i));
			}
            if (Math.Abs(s.nZ - Support.GetZ(i)) < 50)
            {
				bSameLevel++;
				if (ClosestFroshToPole == null)
					ClosestFroshToPole = Support.GetSprite(i);
				else
					ClosestFroshToPole = aiClosestToPole(ClosestFroshToPole, Support.GetSprite(i));
			}
			if ((Support.GetZ(i) - s.nZ) > 30) {
				if (Support.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)] == 16)
					bDoNotClimb = true;
				bUnderPressure++;
			}
		}
	}
	
	if (!(ClosestFroshToPole == null) && Math.Abs(ClosestFroshToPole.nX - s.nX) > 10)
		bDoNotClimb = true;
	
	if (0 == bSupport) {
		aisSendFroshFlying(s);
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR1_1) + R.Next(nsprFR1)]);
	}
	
	if (bUnderPressure != 0) {
		//	aisSendFroshFlying(s, true);
	}
	
	
	// If they are close enough to the pole, make them into an ARMS-UP SUPPORTER
	if (Math.Abs(s.nX - s.nDestX) < 10 && Math.Abs(s.nY - s.nDestY) < dFROSHWIDTHY) {
		s.nY = s.nDestY;
		aiInit11(s);
	}
	
	// BUT, if there is someone else here that is closer to the pole, you can climb
	// over them.
	TSprite Test;
	if (bSameLevel != 0) {
		Test = aiClosestToPole(s, ClosestFroshToPole);
		if (!(Test == s) && (ClosestFroshToPole.nAttrib[((int)nattrFrosh.attrBehavior)] >= 11)) {
			// If I am not closest to the pole, I can climb over coolio.
			// If I'm above the second level, I will CERTAINLY climb.
			if (s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] == ((int)UpperLevelGoals.upperGoalClimb) && !bDoNotClimb) {
				aisMoveTowardsDestination(s);
				s.nY = ClosestFroshToPole.nY + (s.nY < ClosestFroshToPole.nY ? -1 : 1);
				s.nZ = ClosestFroshToPole.nZ;
				//aiInit9A(s, (ClosestFroshToPole.nY > s.nY));
				aiInit11(s);
			}
			else if (s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] == ((int)UpperLevelGoals.upperGoalSupport)) {
				aiInit11(s);  // This can precipitate a long-distance support.
			}
		}
	}
	
//	delete Support;
}


//************************************************
// (11) Arms up to support 3rd or higher level clingers 
//************************************************

public static void aiInit11(TSprite s)  {
	s.nvX = 0;
	s.nvY = 0;
	s.nvZ = 0;
	s.nCC = 0;
	s.bAttrib[((int)battrFrosh.attrLookingLeft)] = s.nX > dPOLEX;
	if (s.nY >= dPOLEY && s.nY <= (dPOLEY + 2))
		s.nY = dPOLEY + 3;
	s.SetBehavior(aiAct11);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 11;
}

public static int nBooster4;
public static void aiAct11(TSprite s)
{
	SpriteSet  Support;
	// Note that this support set can differ from the one used in aiAct10.
	Support = GetFroshInRange(s.nX - 50, s.nY - (dFROSHARMLINKOFFSETY / 2), s.nX + 50, s.nY + (dFROSHARMLINKOFFSETY / 2));
	

	int nCurrentLevel = s.nAttrib[((int)nattrFrosh.attrPyramidLevel)];

	aisMoveTowardsDestination(s);
	switch(s.nCC) {
	case 1:
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_1)] : frm[((int)GameBitmapEnumeration.bmpFR11_1)]);
		break;
	case 80:
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_2)] : frm[((int)GameBitmapEnumeration.bmpFR11_2)]);
		break;
	case 160:
		s.nCC = 0;
		break;
	}
	
	// GET THREE STATS about this frosh:
	
	// Make sure the Frosh is supported.
	int nSupport = 0;
	// Determine if anyone else is on the same level with you at this spot
	bool bSameLevel = false;
	TSprite ClosestFroshToPole = null;
	// See if someone's weighing you down.
	int bUnderPressure = 0;
	bool bDoNotCling = false;
	int n = Support.GetNumberOfSprites();

	for (int i = 0; i < n; i++) {
		if (!(s == Support.GetSprite(i))) {
            if (Math.Abs(s.nZ - Support.GetZ(i) - 90) < 50)
            {
				nSupport++;
				aisWeightOnShoulders(Support.GetSprite(i));
			}
            if (Math.Abs(s.nZ - Support.GetZ(i)) < 50)
            {
				bSameLevel = true;
				if (ClosestFroshToPole == null)
					ClosestFroshToPole = Support.GetSprite(i);
				else
					ClosestFroshToPole = aiClosestToPole(ClosestFroshToPole, Support.GetSprite(i));
			}
			if ((Support.GetZ(i) - s.nZ) > 30) {
				bUnderPressure++;
			}
		}
	}


	// Every once in a while...
	// PERFORMANCE BOOST: INTELLIGENCE: ABOUT 80 IS SHODDY, ABOUT 50 IS BETTER CLIMBING
	// INCREASES TENDENCY TO CLIMB
	// THIS IS AN INDICATOR OF GENERAL AWARENESS OF HOW TO WORK IN THE GREASEPIT.
	// START AT 80, MOVE TOWARDS 50 AS t.INFINITY
	nBooster4 = Globals.myGameConditions.GetBooster(4);
	if (0 == R.Next(Globals.myGameConditions.GetBooster(2))) {
		// If there aren't enough people above me
		if ( nFroshLevel[nCurrentLevel + 1] < nFroshTarget[nCurrentLevel + 1] 
			// and there is someone near me
			&& bSameLevel
			// And there are just about enough people on my level
			// PERFORMANCE BOOST: COURAGE: 4/3 IS MORE CONSERVATIVE, 3/2 IS MORE DARING.
			// AT 3/2 THE FROSH WILL CLIMB EVEN WHEN THEIR LEVEL ISN'T FINISHED
			// START AT 3/2 AND MOVE TOWARDS 4/3.AS t->INFINITY
			&& (nFroshLevel[nCurrentLevel] * (nBooster4 != 0 ? 3 : 4))/(nBooster4 != 0 ? 2 : 3) >= nFroshTarget[nCurrentLevel]
			// and the level above doesn't have more frosh than this level
			&& nFroshLevel[nCurrentLevel + 1] < nFroshLevel[nCurrentLevel]
			// and with some degree of randomness
			// PERFORMANCE BOOST: KEENNESS (ENTHUSIASM): 2 makes them keen climbers, 6 makes them cautious
			// THIS WOULD BE AN INDICATOR OF "KEENNESS" -- 1 WOULD BE HYPERKEEN,
			// 6 WOULD BE CAUTIOUS.  2 SEEMS TO BE IDEAL IF THEY'RE REALLY STRONG.
			// START AT 6, MOVE TOWARDS 2 AS t.INFINITY.
			&& 0 == R.Next(Globals.myGameConditions.GetBooster(6))) {
			s.nY = ClosestFroshToPole.nY + (s.nY < ClosestFroshToPole.nY ? -1 : 1);
			s.nZ = ClosestFroshToPole.nZ;
			aiInit9A(s, (ClosestFroshToPole.nY > s.nY));
		}
		// Else If there are too many people on my side of the pole
		else if (  (s.nX < dPOLEX && nFroshLevelL[nCurrentLevel] - 2 > nFroshLevelR[nCurrentLevel])
			||(s.nX > dPOLEX && nFroshLevelR[nCurrentLevel] - 2 > nFroshLevelL[nCurrentLevel])) {
			// Move to the other side
			if (s.nX < dPOLEX)
				s.nDestX = dPOLEX + (dPOLEX - s.nX);
			else
				s.nDestX = dPOLEX - (s.nX - dPOLEX);
			s.nDestY = s.nY;
            s.bAttrib[((int)battrFrosh.attrLookingLeft)] = !(s.bAttrib[((int)battrFrosh.attrLookingLeft)]);
			aiInit11B(s);
		}
		// Else if there aren't enough people on this level
		else if (nFroshLevel[nCurrentLevel] < nFroshTarget[nCurrentLevel] && 0 ==  R.Next(6) )
			// Encourage other frosh to come up.
			aiInit11C(s);
	}
	
	if (s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] == ((int)UpperLevelGoals.upperGoalCling) && !bDoNotCling 
		&& Math.Abs(s.nX - dPOLEX) < 40 && Math.Abs(s.nY - dPOLEY) < (dFROSHWIDTHY * 3 / 2)) {
		aiInit14(s);
	}
	
	if (nSupport < 1) {
		aisSendFroshFlying(s);
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR1_1) + R.Next(nsprFR1)]);
	}
//	delete Support;
}


//******************************************************
// (11B)  Walking across the pyramid to balance it (NEW)
//******************************************************

public static void aiInit11B(TSprite s)  {
	s.nvZ = 0;
	s.nvX = 1 + R.Next(4);
	s.nCC = 0;
	if (s.nY >= dPOLEY && s.nY <= (dPOLEY + 2))
		s.nY = dPOLEY + 3;
	s.SetBehavior(aiAct11B);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 11;
}

public static void aiAct11B(TSprite s)  {
	// THIS PART BLASTED FROM aiAct11
	SpriteSet  Support;
	// Note that this support set can differ from the one used in aiAct10.
	Support = GetFroshInRange(s.nX - 50, s.nY - (dFROSHARMLINKOFFSETY / 2), s.nX + 50, s.nY + (dFROSHARMLINKOFFSETY / 2));
	
	aisMoveTowardsDestination(s);
	switch(s.nCC) {
	case  3: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR11B_1)]); break;
	case  6: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR11B_2)]); break;
	case  9: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR11B_3)]); break;
	case 12: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR11B_2)]); break;
	case 15: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR11B_1)]); break;
	case 18: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR11B_1)]); break;
	case 21: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR11B_2)]); break;
	case 24: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR11B_3)]); break;
	case 27: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR11B_2)]); break;
	case 30: s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR11B_1)]); s.nCC = 0; break;
	}


	if ((Math.Abs(s.nX - s.nDestX) <= s.nvX) && (Math.Abs(s.nY - s.nDestY) <= s.nvY)) { 
		// WHEN THE FROSH REACHES THEIR GOAL
		
		// Force them (near)perfectly into position
		s.nX = s.nDestX; s.nY = s.nDestY;
		int nCurrentLevel = s.nAttrib[((int)nattrFrosh.attrPyramidLevel)];

		// If there are too many people on my level
		if (nFroshLevel[nCurrentLevel] >= nFroshTarget[nCurrentLevel]) {
			// If the are not enough people on the next level
			if (nFroshLevel[nCurrentLevel + 1] < nFroshTarget[nCurrentLevel + 1]) {
				// Climb to the next level
				aiInit10(s);
				s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] = ((int)UpperLevelGoals.upperGoalClimb);
			}
			else {
				// PERFORMANCE BOOST: THEY GOTTA BE KEEN TO JUMP
				if(0 != Globals.myGameConditions.GetBooster(7)) {
					aiInit2(s);
					if (s.nX < dPOLEX) {
						s.nvX = -s.nvX;
						s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpFR2_1) + R.Next(nsprFR2)]);
					}
				}
			}
		}
		else {
			// Put your arms up
			aiInit11(s);
		}
	}	

	// GET THREE STATS about this frosh:
	
	// Make sure the Frosh is supported.
	int nSupport = 0;
	// Determine if anyone else is on the same level with you at this spot
	int bSameLevel = 0;
	// See if someone's weighing you down.
	int bUnderPressure = 0;
	bool bDoNotCling = false;
	int n = Support.GetNumberOfSprites();

	for (int i = 0; i < n; i++) {
		if (!(s == Support.GetSprite(i))) {
            if (Math.Abs(s.nZ - Support.GetZ(i) - 90) < 50)
            {
				nSupport++;
				aisWeightOnShoulders(Support.GetSprite(i));
			}
            if (Math.Abs(s.nZ - Support.GetZ(i)) < 50)
            {
				bSameLevel++;
				if (Support.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)] == 14)
					bDoNotCling = true;
			}
			if ((Support.GetZ(i) - s.nZ) > 30) {
				bUnderPressure++;
			}
		}
	}
	if (s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] == ((int)UpperLevelGoals.upperGoalCling) && !bDoNotCling 
		&& Math.Abs(s.nX - dPOLEX) < 40 && Math.Abs(s.nY - dPOLEY) < (dFROSHWIDTHY * 3 / 2)) {
		aiInit14(s);
	}
	
	if (nSupport < 1) {
		aisSendFroshFlying(s);
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR1_1) + R.Next(nsprFR1)]);
	}
//	delete Support;
}

//***********************************************
// (11C) Beckoning Frosh Up (access from 11 only)
//***********************************************

public static void aiInit11C(TSprite s)  {
	s.nCC = 0;
	s.SetBehavior(aiAct11C);
}

public static void aiAct11C(TSprite s)  {
	SpriteSet  Support;
	// Note that this support set can differ from the one used in aiAct10.
	Support = GetFroshInRange(s.nX - 50, s.nY - (dFROSHARMLINKOFFSETY / 2), s.nX + 50, s.nY + (dFROSHARMLINKOFFSETY / 2));
	
	int nCurrentLevel = s.nAttrib[((int)nattrFrosh.attrPyramidLevel)];

	aisMoveTowardsDestination(s);
	switch(s.nCC) {
	case 1:
	case 10:
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_3)] : frm[((int)GameBitmapEnumeration.bmpFR11_3)]); break;
	case 5:
	case 15:
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_4)] : frm[((int)GameBitmapEnumeration.bmpFR11_4)]); break;
	case 20:
		s.nCC = 0;
		s.SetBehavior(aiAct11);
		break;
	}
	
	// GET THREE STATS about this frosh:
	
	// Make sure the Frosh is supported.
	int nSupport = 0;
	// Determine if anyone else is on the same level with you at this spot
	//int bSameLevel = 0;
	//TSprite ClosestFroshToPole = null;
	// See if someone's weighing you down.
	//int bUnderPressure = 0;
	//bool bDoNotCling = false;
	int n = Support.GetNumberOfSprites();

	for (int i = 0; i < n; i++) {
		if (!(s == Support.GetSprite(i))) {
            if (Math.Abs(s.nZ - Support.GetZ(i) - 90) < 50)
            {
				nSupport++;
				aisWeightOnShoulders(Support.GetSprite(i));
			}
		}
	}


	if (nSupport < 1) {
		aisSendFroshFlying(s);
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR1_1) + R.Next(nsprFR1)]);
	}
//	delete Support;
}

//*************************************************
// (11D) Eating Pizza on high (access from 11 only)
//*************************************************
public static void aiInit11D(TSprite s)  {
	if (s.pfAI == aiAct11D)
		return;
	s.nCC = 0;
	s.SetBehavior(aiAct11D);
}


public static void aiAct11D(TSprite s)  {
	SpriteSet  Support;
	// Note that this support set can differ from the one used in aiAct10.
	Support = GetFroshInRange(s.nX - 50, s.nY - (dFROSHARMLINKOFFSETY / 2), s.nX + 50, s.nY + (dFROSHARMLINKOFFSETY / 2));
	
	int nCurrentLevel = s.nAttrib[((int)nattrFrosh.attrPyramidLevel)];

	aisMoveTowardsDestination(s);
	if (0 == (s.nCC % timePIZZAMUNCH)) { 
		switch (s.nCC / timePIZZAMUNCH) { 
		case 1:
			if (!sSound[((int)ASSList.ssndEFFECTS_PIZZAEAT)].IsPlaying())
				sSound[((int)ASSList.ssndEFFECTS_PIZZAEAT)].Play(SoundbankInfo.volNORMAL, panONX(s));
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_9)] : frm[((int)GameBitmapEnumeration.bmpFR11_9)]); 
			break;
		case 2:	case 4:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_10)] : frm[((int)GameBitmapEnumeration.bmpFR11_10)]); 
			break;
		case 3:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_9)] : frm[((int)GameBitmapEnumeration.bmpFR11_9)]); 
			break;
		case 5:
			if (0 != R.Next(nPIZZAMUNCHAVERAGE))
				s.nCC = timePIZZAMUNCH;
			else {
				s.nCC = 0;
				s.nvX = 0; s.nvY = 0; s.nDestX = s.nX; s.nDestY = s.nY;
				s.SetBehavior(aiAct11);
			}
			break;
		}
	}
	
	// GET THREE STATS about this frosh:
	
	// Make sure the Frosh is supported.
	int nSupport = 0;
	// Determine if anyone else is on the same level with you at this spot
	//int bSameLevel = 0;
	//TSprite ClosestFroshToPole = null;
	// See if someone's weighing you down.
	//int bUnderPressure = 0;
	//bool bDoNotCling = false;
	int n = Support.GetNumberOfSprites();

	for (int i = 0; i < n; i++) {
		if (!(s == Support.GetSprite(i))) {
            if (Math.Abs(s.nZ - Support.GetZ(i) - 90) < 50)
            {
				nSupport++;
				aisWeightOnShoulders(Support.GetSprite(i));
			}
		}
	}

	if (nSupport < 1) {
		aisSendFroshFlying(s);
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR1_1) + R.Next(nsprFR1)]);
	}
//	delete Support;
}



//***********************************************
// (11E) Boozing Up High (access from 11 only)
//***********************************************

public static void aiInit11E(TSprite s)  {
	if (s.pfAI == aiAct11E)
		return;
	s.nCC = 0;
	s.SetBehavior(aiAct11E);
}

public static void aiAct11E(TSprite s)  {
	SpriteSet  Support;
	// Note that this support set can differ from the one used in aiAct10.
	Support = GetFroshInRange(s.nX - 50, s.nY - (dFROSHARMLINKOFFSETY / 2), s.nX + 50, s.nY + (dFROSHARMLINKOFFSETY / 2));
	
	int nCurrentLevel = s.nAttrib[((int)nattrFrosh.attrPyramidLevel)];

	aisMoveTowardsDestination(s);
	if (0 == (s.nCC % AIMethods.BEER_DRINKING_SPEED)) {
		switch(s.nCC / BEER_DRINKING_SPEED) {
		case 1:	
			sSound[((int)ASSList.ssndEFFECTS_CHUG)].Play(SoundbankInfo.volNORMAL, panONX(s));
            break;
		case 3:	case 5:	case 7:	case 9:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_5)] : frm[((int)GameBitmapEnumeration.bmpFR11_5)]); 
			break;
		case 2:	case 4:	case 6:	case 8:	case 10:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_6)] : frm[((int)GameBitmapEnumeration.bmpFR11_6)]); 
			break;
		case 12:
			if (0 != R.Next(2))
				sSound[((int)ASSList.ssndEFFECTS_CHUGLASTDROP)].Play(SoundbankInfo.volNORMAL, panONX(s));
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_7)] : frm[((int)GameBitmapEnumeration.bmpFR11_7)]); 
			break;
		case 24:
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR11_8)] : frm[((int)GameBitmapEnumeration.bmpFR11_8)]); 
			if (0 != R.Next(6))
				lSound[((int)ASLList.lsndFROSH_CLARKFINISH1) + R.Next(SoundbankInfo.nsndFROSH_CLARKFINISH)].Play(SoundbankInfo.volHOLLAR, panONX(s));
			else
				lSound[((int)ASLList.lsndFROSH_CLARKFINISH3)].Play(SoundbankInfo.volHOLLAR, panONX(s));
			break;
		case 30:
			if ((0 == R.Next((Globals.myGameConditions.GetBooster(0)/2)+1))) {
				aisSendFroshFlying(s);
				s.nvZ = 10;
				s.nvX = s.nX < dPOLEX ? -6 : 6;
				s.SetFrame(s.nX < dPOLEX ? (frmM[((int)GameBitmapEnumeration.bmpFR2_1) + R.Next(nsprFR2)]) : (frm[((int)GameBitmapEnumeration.bmpFR2_1 )+ R.Next(nsprFR2)]));
			}
			break;
		case 32:
			s.nCC = 0;
			s.nvX = 0; s.nvY = 0; s.nDestX = s.nX; s.nDestY = s.nY;
			s.SetBehavior(aiAct11);
			break;
		}
	}	
	// GET THREE STATS about this frosh:
	
	// Make sure the Frosh is supported.
	int nSupport = 0;
	// Determine if anyone else is on the same level with you at this spot
	//int bSameLevel = 0;
	//TSprite ClosestFroshToPole = null;
	// See if someone's weighing you down.
	//int bUnderPressure = 0;
	//bool bDoNotCling = false;
	int n = Support.GetNumberOfSprites();

	for (int i = 0; i < n; i++) {
		if (!(s == Support.GetSprite(i))) {
            if (Math.Abs(s.nZ - Support.GetZ(i) - 90) < 50)
            {
				nSupport++;
				aisWeightOnShoulders(Support.GetSprite(i));
			}
		}
	}

	if (nSupport < 1) {
		aisSendFroshFlying(s);
		s.SetFrame(frm[((int)GameBitmapEnumeration.bmpFR1_1) + R.Next(nsprFR1)]);
	}
//	delete Support;
}






//************************************************
// (14) Cling desperately to the pole
//************************************************

public static void aiInit14(TSprite s)  {
	s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR14_1)] : frm[((int)GameBitmapEnumeration.bmpFR14_1)]);				  
	s.nX = s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? dPOLEX : dPOLEX;
	s.nY = dPOLEY + 2;
	s.nvX = 0;
	s.nvY = 0;
	s.nvZ = 0;
	s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 0;
	s.SetBehavior(aiAct14);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 14;
}

public static void aiAct14(TSprite s)  {
	if ((0 == R.Next(16))) {
		s.nZ--;
		if (s.nZ < 20)
			s.nCC = timeDROPFROMCLINGING;  // Force them to drop
	}
	
	// Eventually they lose their grip and fall.
	if (s.nCC > timeDROPFROMCLINGING) {
		if (s.nvZ < 10)
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR14_3)] : frm[((int)GameBitmapEnumeration.bmpFR14_3)]);
		else
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR14_4)] : frm[((int)GameBitmapEnumeration.bmpFR14_4)]);
		
		aisPlummet(s);
		SpriteSet  Support;
		Support = GetFroshInRange(s.nX - 50, s.nY - (dFROSHARMLINKOFFSETY / 2), s.nX + 50, s.nY + (dFROSHARMLINKOFFSETY / 2));
		int n = Support.GetNumberOfSprites();
		if (0 != n) {
			for (int i = 0; i < n; i++) {
                if ((Math.Abs(s.nZ - Support.GetZ(i)) < 50) && Support.GetSprite(i).nAttrib[((int)nattrFrosh.attrBehavior)] == 14 && !(s == Support.GetSprite(i)))
                {
					Support.GetSprite(i).nCC = timeDROPFROMCLINGING;
					Support.GetSprite(i).nvZ = s.nvZ;
				} // if
			} // for
		} // if
//		delete Support;
		
		if (s.nZ < dBELLYBUTTONZ && s.nvZ < 0) {
			// The frosh has hit the water
			s.nZ = 0;
            ssPit.Include(SpriteInit.CreateSprite((SpriteType.sprSPLASHL), s.nX, s.nY));
            ssWater.Include(SpriteInit.CreateSprite((SpriteType.sprRIPPLE), s.nX, s.nY));
			aiInit3(s);
		}
	}
	else {
		if (s.nCC > timeREACHFORCLING)
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR14_2)] : frm[((int)GameBitmapEnumeration.bmpFR14_2)]);
	}
}


//************************************************
// (16A) Reach and yank on the Tam
//************************************************

static int nNumberOfTugs;

public static void aiInit16A(TSprite s)  {
	nNumberOfTugs = 0;
	Globals.myGameConditions.AddEnergy(15);
	s.nvX = 0;
	s.nvY = 0;
	s.nvZ = 0;
	s.nX = dPOLEX;
	s.nY = dPOLEY + 2;
	s.nZ = dTAMZ + 150; // Get this right
	s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16C_1)] : frm[((int)GameBitmapEnumeration.bmpFR16C_1)]);
	s.SetBehavior(aiAct16A);
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 16;
	s.nAttrib[((int)nattrFrosh.attrPyramidLevel)] = 6; // Tam Level
}

public const int timeTAMTUG = 2;

public static void aiAct16A(TSprite s)  {
	sprTam.nAttrib[((int)attrTam.attrTamStatus)] = 1;
	Globals.myGameConditions.AddEnergy(2);

	s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16C_1)] : frm[((int)GameBitmapEnumeration.bmpFR16C_1)]);

	if (0 == (nFroshLevel[5]))
		aiInit16B(s);
	else { // What's a Frosh to do when at the tam?
	
		if (nNumberOfTugs < 3) { // New at this
			switch(R.Next(3)) {
			case 0:  aiInit16D(s); break;
			default: aiInit16C(s); break;
			}
		}
		else if (nNumberOfTugs < 8) { // Up for a while
			switch(R.Next(5)) {
			case 0:  aiInit16C(s); break;
			case 1:  aiInit16E(s); break;
			default: aiInit16D(s); break;
			}
		}
		else { // Tired
			switch(R.Next(5)) {
			case 0:  aiInit16C(s); break;
			case 1:  aiInit16D(s); break;
			default: aiInit16E(s); break;
			}
		}
	}
}

public static void aiInit16B(TSprite s)  {
	nNumberOfTugs ++;
	s.SetBehavior(aiAct16B); s.nCC = 0;
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 16;
}


public static void aiAct16B(TSprite s)  {
	sprTam.nAttrib[((int)attrTam.attrTamStatus)] = 1;

	if (s.nCC == 1)
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16B_1)] : frm[((int)GameBitmapEnumeration.bmpFR16B_1)]);
	else if (s.nCC > 25 && s.nCC < 50 && 0 == R.Next(5))
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16B_2)] : frm[((int)GameBitmapEnumeration.bmpFR16B_2)]);
	if (s.nCC > 50 && s.nCC < 100 && 0 == R.Next(5))
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16B_3)] : frm[((int)GameBitmapEnumeration.bmpFR16B_3)]);
	if (s.nCC > 100  && 0 == R.Next(50)) {
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR14_4)] : frm[((int)GameBitmapEnumeration.bmpFR14_4)]);
		s.nZ -= 85;
		aisSendFroshFlying(s);
		s.nvX = 0; s.nvY = 0;
	}
	// Last chance to get back into tugging mode
	if ((s.nCC == 28 || s.nCC == 52) && 0 != Globals.myGameConditions.GetFroshLameness() && (0 == R.Next(4))) {
		Globals.myGameConditions.AddEnergy(60);
		s.SetBehavior(aiAct16A);
	}
}

public static void aiInit16C(TSprite s)  {
	nNumberOfTugs ++;
	s.SetBehavior(aiAct16C); s.nCC = 0;
}


public static int bDidHeMakeIt;
public static void aiAct16C(TSprite s)
{
	sprTam.nAttrib[((int)attrTam.attrTamStatus)] = 1;
	if (s.nCC == 1)
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16C_1)] : frm[((int)GameBitmapEnumeration.bmpFR16C_1)]);
	else if (s.nCC == 5) {
		bDidHeMakeIt = (0 == R.Next(3) ? 1 : 0);
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16C_2)] : frm[((int)GameBitmapEnumeration.bmpFR16C_2)]);
		sprTam.nAttrib[((int)attrTam.attrNailsInTam)]--;
	}
	if (s.nCC == 8 && 0 != bDidHeMakeIt) {
		if (sprTam.nAttrib[((int)attrTam.attrNailsInTam)] >= 0)
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16C_1)] : frm[((int)GameBitmapEnumeration.bmpFR16C_1)]);
		else
			aiInit16F(s);
	}
	if (s.nCC == 10) {
		if(0 != R.Next(3))
			s.nCC = 0;
		else
			s.SetBehavior(aiAct16A);
	}
}

public static void aiInit16D(TSprite s)  {
	nNumberOfTugs ++;
	s.SetBehavior(aiAct16D); s.nCC = 0;
}


public static void aiAct16D(TSprite s)  {
	sprTam.nAttrib[((int)attrTam.attrTamStatus)] = 1;

	if (s.nCC == 1)
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16D_1)] : frm[((int)GameBitmapEnumeration.bmpFR16D_1)]);
	else if (s.nCC == 5) {
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16D_2)] : frm[((int)GameBitmapEnumeration.bmpFR16D_2)]);
	}
	else if (s.nCC == 8) {
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16D_3)] : frm[((int)GameBitmapEnumeration.bmpFR16D_3)]);
	}
	else if (s.nCC == 15) {
		if(0 != R.Next(7)) {
			s.nCC = 4;
			if (0 == R.Next(2))
				sprTam.nAttrib[((int)attrTam.attrNailsInTam)]--;
		}
		else {
			s.nCC = 100;
			s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16D_4)] : frm[((int)GameBitmapEnumeration.bmpFR16D_4)]);
		}
	}
	else if (s.nCC == 110) {
		s.SetBehavior(aiAct16A);
	}
}

public static void aiInit16E(TSprite s)  {
	nNumberOfTugs ++;
	s.SetBehavior(aiAct16E); s.nCC = 0;
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 16;
}


public static void aiAct16E(TSprite s)  {
	sprTam.nAttrib[((int)attrTam.attrTamStatus)] = 1;

	if (s.nCC == 1) {
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16E_1)] : frm[((int)GameBitmapEnumeration.bmpFR16E_1)]);
	}
	if (s.nCC == 15) {
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16E_2)] : frm[((int)GameBitmapEnumeration.bmpFR16E_2)]);
	}
	if (s.nCC == 20) {
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16E_1)] : frm[((int)GameBitmapEnumeration.bmpFR16E_1)]);
	}
	if (s.nCC == 28) {
		if((0 == R.Next(3))) 
			aiInit16B(s);
		else
			s.SetBehavior(aiAct16A);
	}

}

public static int nNewHighScore;
public static void aiInit16F(TSprite s)
{
	// They got the Tam.  The crowd goes apeshit.
	sprTam.nAttrib[((int)attrTam.attrTamStatus)] = 2;
	s.nAttrib[((int)nattrFrosh.attrBehavior)] = 16;
	NOSPEECHFOR(500);
	s.nAttrib[((int)nattrFrosh.attrUpperLevelGoal)] = 0;
	sprPrez.SetFrame(frm[((int)GameBitmapEnumeration.bmpPREZ2_2)]);
	sprPrez.nAttrib[((int)attrPrez.attrPrezAction)] = 5; 
	Globals.myGameConditions.GameOver();
	//nNewHighScore = (gnPitTimeH * 3600) + (gnPitTimeM * 60) + (gnPitTimeS);
    nNewHighScore = (int)(Globals.GameTimerService.GetCurrentGameTimeScoreMilliseconds());

    Globals.Analytic("GameEndTam;" + nNewHighScore.ToString());

    if (nNewHighScore > Globals.myGameConditions.GetHighScore(Globals.myGameConditions.GetFroshLameness())) {
		lSound[((int)ASLList.lsndNARRATOR_CONGRATS)].Play(SoundbankInfo.volHOLLAR);
		Globals.myGameConditions.SetHighScore(Globals.myGameConditions.GetFroshLameness(), nNewHighScore);
	}
	else {
		lSound[((int)ASLList.lsndFROSH_GOTTAM1)].Play(SoundbankInfo.volHOLLAR);
	}
	s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16F_1)] : frm[((int)GameBitmapEnumeration.bmpFR16F_1)]);
	aisStartAMosh();
	sprFrecsR.nAttrib[((int)nattrCrowd.attrFEnergy)] = energySlam + 150;
	sprFrecsC.nAttrib[((int)nattrCrowd.attrFEnergy)] = energySlam - 20;
	sprFrecsL.nAttrib[((int)nattrCrowd.attrFEnergy)] = energySlam + 150;
	aisSetFrecAction(sprFrecsL, ((int)attrCrowdActions.faSlamming));
	aisSetFrecAction(sprFrecsC, ((int)attrCrowdActions.faCheering));
	aisSetFrecAction(sprFrecsR, ((int)attrCrowdActions.faSlamming));
	s.SetBehavior(aiAct16F); s.nCC = 0;
}


public static void aiAct16F(TSprite s)  {
	sprTam.nAttrib[((int)attrTam.attrTamStatus)] = 1;

	if (Globals.myGameConditions.IsPopBoyInPit()) {
		sprPopBoy.nAttrib[((int)nattrFrosh.attrBehavior)] = 7;
	}
	if (0 == (s.nCC % 15) && (0 == R.Next(3))) {
		lSound[((int)ASLList.lsndFROSH_GOTTAM2) + R.Next(2)].Play(SoundbankInfo.volHOLLAR);
	}
	if (s.nCC == 56 || s.nCC == 200) {
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16F_2)] : frm[((int)GameBitmapEnumeration.bmpFR16F_2)]);
	}
	if (s.nCC == 56) {
		if (Globals.myGameConditions.IsPopBoyInPit())
			lSound[((int)ASLList.lsndPOPBOY_GOTTAM1)].Play(SoundbankInfo.volHOLLAR);
	}
	if (s.nCC > 58) {
		s.nZ-=2;
	}
	if (s.nCC == 120) {
		s.SetFrame(s.bAttrib[((int)battrFrosh.attrLookingLeft)] ? frmM[((int)GameBitmapEnumeration.bmpFR16F_1)] : frm[((int)GameBitmapEnumeration.bmpFR16F_1)]);
	}
	if (s.nCC == 275) {
		Globals.myGameLoop.ChangeGameState(((int)GameStates.STATETITLE));
	}

//Now what?
}
}