using System;

// Sprite Class Declaration
public delegate void AIMethod(TSprite sprite);

public class TSprite : IDisposable 
{

    public TSprite()
    {
        nZ = 0;
        nY = 0;
        nX = 0;
        nR = 0;
        nG = 0;
        nB = 0;
        nA = 255;
        ppChosen = null;
    }

    public void Dispose()
    {
        ppAbandon();
    }

    public void SetFrame(FrameDesc frmNewImage)
    {
        frmFrame = frmNewImage;
    }

    public void ppAbandon()
    {
        // CHECK FOR A STRANDED POLE POSITION
        if (!(ppChosen == null))
        {
            if (ppChosen.GetClaimer() == this)
            {
                ppChosen.ReleaseClaim();
                ppChosen = null;
            }
        }
    }
    public void SetBehavior(AIMethod fNewBehavior)
    {
        SetBehavior(fNewBehavior, false); // keep position defaults to false.
    }

    public void SetBehavior(AIMethod fNewBehavior, bool bKeepPosition)
    {
        // Toggles the behavior used by the Think routine

        // INITIALIZE THE BEHAVIOR
        pfAI = fNewBehavior;

        // Abandon the pole position if one existed.
        if (!bKeepPosition)
            ppAbandon();
    }

    public void SetSecondaryBehavior(AIMethod fNewBehavior)
    {
        // Toggles the Secondary Behaviour stored in the Frosh

        // INITIALIZE THE BEHAVIOR
        pfAISecondary = fNewBehavior;
    }

    public void SwitchToSecondaryBehavior() {
	// Swap the AI and Secondary AI
	AIMethod pfAITemp = pfAISecondary;
	pfAISecondary = pfAI;
	pfAI = pfAITemp;
}


    public void SetGoal(int nNewGoal)
    {
        // Sets the goal attribute (particularly, of a Frosh)
        nAttrib[1] = nNewGoal;
    }

    public void CalculateScreenCoordinates(int nViewportY)
    {
        // Compute Screen X-Y co-ordinates using nX, nY, nZ
        // and information about the current ViewPort and HotSpot.
        nScrx = nX - frmFrame.nHotSpotX;
        nScry = nY + nViewportY - frmFrame.nHotSpotY - nZ;
    }

    public void Draw()
    {
        if (SpriteText == SpriteTextType.None)
        {
            if(UseColorSwap)
                frmFrame.Draw(this, nScrx, nScry, ReplaceRGB, SubstituteRGB);
            else
                frmFrame.Draw(this, nScrx, nScry, BlackBytes, BlackBytes);
        }
        else
        {
            Globals.RenderingService.DrawText(this, SpriteText, Text, bAttrib[5], nScrx, nScry, nR, nG, nB, nA);
        }

    }

    public void Think()
    {
        // Increment the cyclecounter and call the appropriate AI function
        nCC++;
        pfAI(this);
    }


    public AIMethod pfAI;
    public AIMethod pfAISecondary;


    public int nScrx;
    public int nScry;  // (x, y) co-ordinates of the sprite in ScreenSpace
    public FrameDesc frmFrame;             // Pointer to the image to be displayed


    public SpriteType SpriteType;
    public int nTag;           // A Tag, used in the Frosh to assign a default "order" to them.

    public bool bDeleted;      // Is this sprite marked to be deleted?
    public int nX, nY, nZ;     // (x, y, z) co-ordinates of sprite in FroshSpace, positive right, back, up
    public int nvX, nvY, nvZ;  // (x, y, z) velocity
    public byte[] ReplaceRGB = new byte[3] { 0, 0, 0 };    // Color to replace
    public byte[] SubstituteRGB = new byte[3] { 0, 0, 0 }; // Color to substitute
    public bool UseColorSwap = true; // Controls whether ReplaceRGB and SubstituteRGB are passed, or two instances of BlackBytes.
    private static readonly byte[] BlackBytes = new byte[3] { 0, 0, 0 };
    public byte nR, nG, nB, nA;     // Palette Entries for text
    public int GetScrX() { return nScrx; }
    public int GetScrY() { return nScry; }
    public int nDestX, nDestY; // (x, y) co-ordinates of destination location in FroshSpace
    public int nDestZ;         // z co-ordinate of desination location in FroshSpace
    public int nCC;            // The cyclecounter of the sprite

    public SpriteTextType SpriteText = SpriteTextType.None;
    public string Text = null;

    public PolePosition ppChosen;

    public int[] nAttrib = new int[10];
    // 0 - ((int)nattrFrosh.attrBehavior) - The number of the current behavior
    // 1 - ((int)nattrFrosh.attrGoal)     - The frosh's goal = {
    // 0 - Senseless Wandering
    // 1 - Moving towards a pyramid spot
    // 2 - Providing a boost up
    // 3 - About to be boosted up
    // 4 - Clark Mug o' Beer
    // 5 - 'Za
    // 6 - ArtSci
    // 7 - Commie      }
    // 2 - ((int)nattrFrosh.attrMotivation)
    // 3 - ((int)nattrFrosh.attrStr)
    // 4 - ((int)nattrFrosh.attrFrame)
    // 5 - ((int)nattrFrosh.attrPersonality) - The frosh's personality = {
    // 0  goofy 
    // 1  heavyweight
    // 2  hoister
    // 3  climber   }
    // 6 - ((int)nattrFrosh.attrUpperLevelGoal)
    // 0  ((int)UpperLevelGoals.upperGoalCling)
    // 1  ((int)UpperLevelGoals.upperGoalClimb)
    // 2  ((int)UpperLevelGoals.upperGoalSupport)
    // 7 - ((int)nattrFrosh.attrMindSet)
    // 0  ((int)mindsets.mindsetMotivated)
    // 1  mindsetExcited (will hit on anything that moves)
    // 2  ((int)mindsets.mindsetHungry)  (I dream of pizza)
    // 3  ((int)mindsets.mindsetThirsty) (beer?)
    // 4  ((int)mindsets.mindsetDrunk)
    // 8 - ((int)nattrFrosh.attrPyramidLevel)
    // 1  Lowest Level
    // ,,,
    // 6  Tam
    // 9 - ((int)nattrFrosh.attrHeightOfInducedFall) (see gameinit.cpp)

    public bool[] bAttrib = new bool[6];
    // 0 - ((int)battrFrosh.attrExcited)
    // 1 - ((int)battrFrosh.attrLookingLeft)
    // 2 - ((int)battrFrosh.attrLookingAtScreen)
    // 3 - ((int)battrFrosh.attrWeightOnShoulders)
    // 4 - ((int)battrFrosh.attrThirsty)
    // 5 - ((int)battrFrosh.attrHungry)


}

public enum SpriteTextType
{
    None,
    /// <summary>
    /// Was: Franklin Gothic Book Bold 12 point, TextRenderingHint.AntiAlias
    /// Is: Franklin Gothic Medium Regular 24 point
    /// </summary>
    Small,
    Large
}

