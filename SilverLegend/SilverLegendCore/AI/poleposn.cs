

public class PolePosition
{
    public const int MAXPPCHAINS = 20;
    public const float ENDOFCHAIN = 6000.0f;
    public const float ENDOFDATA = 60000.0f;

    private float fOffsetX; private float fOffsetY;
    private int nPitX; private int nPitY;
    private PolePosition ChildPosition;  // Position further from the pole is a child.
    private PolePosition ParentPosition; // Position closer to the pole is a parent.
    private PolePosition AdjacentChainFirstPosition; // The first Position in the adjacent chain.
    private TSprite sprClaimer;

    public int nOrdinal;  // How close are you to the pole itself? (no need to trace through parents)


    
    // Pole Position Chains
    public static PolePosition[] PoleChains = new PolePosition[MAXPPCHAINS];
    public static int nPPChains;



    public int GetX()
    {
        return nPitX;
    }

    public int GetY()
    {
        return nPitY;
    }

    public TSprite GetClaimer()
    {
        return sprClaimer;
    }

    public bool PositionIsTaken()
    {
        // Returns whether or not this particular Position is taken
        return !(PositionIsFree());
    }

    public bool PositionIsFree()
    {
        // Returns whether or not this particular Position is free
        return (sprClaimer == null);
    }

    public PolePosition GetParent()
    {
        return ParentPosition;
    }

    public PolePosition GetChild()
    {
        return ChildPosition;
    }

    public PolePosition AdjacentChain()
    {
        return AdjacentChainFirstPosition;
    }

    public void SetAdjacentChain(PolePosition ppChain)
    {
        AdjacentChainFirstPosition = ppChain;
    }


    public void ReleaseClaim()
    {
        sprClaimer = null;
    }

    public void SetClaim(TSprite sprNewClaimer)
    {
        sprClaimer = sprNewClaimer;
    }

    public bool IsClaimed()
    {
        return !(sprClaimer == null);
    }

    public PolePosition FirstTakenChild()
    {
        PolePosition ReturnSpot = this;
        if (ReturnSpot.PositionIsTaken())
            return ReturnSpot;
        else if (ChildPosition == null)
            return null;
        else
            return ChildPosition.FirstTakenChild();
    }

    public PolePosition FirstFreeChild()
    {
        PolePosition ReturnSpot = this;
        if (ReturnSpot.PositionIsFree())
            return ReturnSpot;
        else if (ChildPosition == null)
            return null;
        else
            return ChildPosition.FirstFreeChild();
    }

    public PolePosition LastTakenChild()
    {
        PolePosition ReturnSpot = this;
        if (ReturnSpot.PositionIsFree())
            return ReturnSpot.ParentPosition;
        else if (ChildPosition == null)
            return null;
        else
            return ChildPosition.LastTakenChild();
    }

    public void CalculateScreenPosition()
    {
        nPitX = AIMethods.dPOLEX + (int)(fOffsetX * AIMethods.dFROSHARMLINKOFFSETX);
        nPitY = AIMethods.dPOLEY + (int)(fOffsetY * AIMethods.dFROSHARMLINKOFFSETY);
    }

    public PolePosition(float fPolePositionX, float fPolePositionY, PolePosition Parent)
    {
        fOffsetX = fPolePositionX;
        fOffsetY = fPolePositionY;
        ParentPosition = Parent;
        ChildPosition = null;
        CalculateScreenPosition();
        SetClaim(null);
    }


    public PolePosition CreateChild(float fOffsetX, float fOffsetY)
    {
        ChildPosition = new PolePosition(fOffsetX, fOffsetY, this);
        return ChildPosition;
    }


    public static float[] nPolePositionData;
    static PolePosition()
    {
        nPolePositionData = new float[]{
	0, 1,
		0, 2,
		0, 3,
		ENDOFCHAIN, ENDOFCHAIN,
		1, 1,
		1, 2,
		1, 3,
		ENDOFCHAIN, ENDOFCHAIN,
		1, 0,
		2, 1,
		2, 2,
		2, 3,
		ENDOFCHAIN, ENDOFCHAIN,
		2, 0,
		3, 0,
		3, 1,
		3, 2,
		4, 1,
		4, 0,
		ENDOFCHAIN, ENDOFCHAIN,
		2, -1,
		3, -1,
		4, -1,
		3, -2,
		2, -2,
		2, -3,
		ENDOFCHAIN, ENDOFCHAIN,
		1, -1,
		1, -2,
		1, -3,
		ENDOFCHAIN, ENDOFCHAIN,
		0, -1,
		0, -2,
		0, -3,
		ENDOFCHAIN, ENDOFCHAIN,
		-1, -1,
		-1, -2,
		-1, -3,
		ENDOFCHAIN, ENDOFCHAIN,
		-2, -1,
		-3, -1,
		-4, -1,
		-3, -2,
		-2, -2,
		-2, -3,
		ENDOFCHAIN, ENDOFCHAIN,
		-2, 0,
		-3, 0,
		-3, 1,
		-3, 2,
		-4, 1,
		-4, 0,
		ENDOFCHAIN, ENDOFCHAIN,
		-1, 0,
		-2, 1,
		-2, 2,
		-2, 3,
		ENDOFCHAIN, ENDOFCHAIN,
		-1, 1,
		-1, 2,
		-1, 3,
		ENDOFCHAIN, ENDOFDATA};
    }

    public static bool Initialize_PolePositions()
    {
        int i = 0;
        int nPositionDistanceFromPole;
        nPPChains = 0;
        float fX = 0;
        float fY = 0;
        //int nCurrentChain = 0;
        PolePosition ppCurrentChain;

        while (fY != ENDOFDATA)
        {
            fX = nPolePositionData[i]; i++;
            fY = nPolePositionData[i]; i++;
            ppCurrentChain = new PolePosition(fX, fY, null); // Create a first-level entry
            PoleChains[nPPChains] = ppCurrentChain;

            nPositionDistanceFromPole = 0;
            ppCurrentChain.nOrdinal = nPositionDistanceFromPole;

            while (fX != ENDOFCHAIN)
            {
                fX = nPolePositionData[i]; i++;
                fY = nPolePositionData[i]; i++;
                if (fX != ENDOFCHAIN)
                {
                    ppCurrentChain = ppCurrentChain.CreateChild(fX, fY);
                    nPositionDistanceFromPole++;
                    ppCurrentChain.nOrdinal = nPositionDistanceFromPole;
                }
            }
            nPPChains++;
        }

        for (i = 0; i < nPPChains; i++)
        {
            ppCurrentChain = PoleChains[i];
            do
            {
                ppCurrentChain.SetAdjacentChain(PoleChains[(i + 1) % nPPChains]);
                ppCurrentChain = ppCurrentChain.GetChild();
            } while (!(ppCurrentChain == null));
        }
        return true;
    }


}


