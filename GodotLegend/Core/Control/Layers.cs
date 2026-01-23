public class Layer
{
    private int nStartOffset;
    private float fFactor;
    private int nY;

    public Layer(int NnStartOffset, float NfFactor)
    {
        nStartOffset = NnStartOffset;
        fFactor = NfFactor;
    }

    public void SetScrollDistance(int nDistance)
    {
        nY = nDistance;
    }

    public int GetY()
    {
        return nStartOffset + (int)((float)nY * fFactor);
    }
}

public enum LayerNames
{
    LAYERSKY, LAYERTREE, LAYERSKYLINE, LAYERFREC, LAYERPIT, LAYERMISC
};

public class Layers
{
    public Layers()
    {

        nScrollDistance = 0;
        nScrollVelocity = 0;
        bForceScrollToBottom = false;

        myLayer[((int)LayerNames.LAYERSKY)] = new Layer(0, (float)3.2); // Upper-left corner of screen
        myLayer[((int)LayerNames.LAYERTREE)] = new Layer(166, (float)6.1); // Base-of-trees-level
        myLayer[((int)LayerNames.LAYERSKYLINE)] = new Layer(251, (float)5.6); // Base-of-skyline-level
        myLayer[((int)LayerNames.LAYERFREC)] = new Layer(260, (float)4.9); // Frecs
        myLayer[((int)LayerNames.LAYERPIT)] = new Layer(275, (float)4.3); // Water level
        myLayer[((int)LayerNames.LAYERMISC)] = new Layer(0, (float)0.0); // Screen (typ. 0)
    }
    private int nScrollDistance;
    private int nScrollVelocity;
    private bool bForceScrollToBottom;
    private Layer[] myLayer = new Layer[NUM_LAYERS];

    public const int MAXSCROLLVELOCITY = 4;
    public const int dMAXGAMESCROLLDISTANCE = 85;
    public const int dGAMESTARTSCROLLDISTANCE = 85;
    public const int dGAMESTARTSCROLLVELOCITY = -10;

    public const int dSCROLLACCELERATION = 1;

    public const int NUM_LAYERS = 6;


    public int GetOffset(int nLayerName) { return myLayer[nLayerName].GetY(); }

    public void SetScrollDistance(int nDistance)
    {
        for (int i = 0; i < NUM_LAYERS; i++)
            myLayer[i].SetScrollDistance(nDistance);
    }

    public void ForceScroll(int nDistance)
    {
        nScrollDistance += nDistance;

        if (nScrollDistance <= 0) nScrollDistance = 0;
        else if (nScrollDistance >= dMAXGAMESCROLLDISTANCE)
            nScrollDistance = dMAXGAMESCROLLDISTANCE;

    }

    public void ScrollScreen()
    {
        if ((!Globals.myGameConditions.IsDemo()) && (!bForceScrollToBottom))
        {
            if (Globals.InputService.GetMouseY() < 40)
                nScrollVelocity += dSCROLLACCELERATION;
            else if (Globals.InputService.GetMouseY() > 440)
                nScrollVelocity -= dSCROLLACCELERATION;
            else
            {
                if (nScrollVelocity > 0)
                    nScrollVelocity--;
                if (nScrollVelocity < 0 && !(bForceScrollToBottom))
                    nScrollVelocity++;
            }
        }

        if (nScrollDistance < 30 && nScrollVelocity < -2)
            nScrollVelocity++;
        else if (nScrollDistance > 50 && nScrollVelocity > 2)
            nScrollVelocity--;

        if (bForceScrollToBottom)
        {
            if (nScrollDistance == 0)
                bForceScrollToBottom = false;
            else
                nScrollVelocity = -(nScrollDistance / 16) - 1;
        }


        if (nScrollVelocity > MAXSCROLLVELOCITY)
            nScrollVelocity = MAXSCROLLVELOCITY;
        if (nScrollVelocity < -MAXSCROLLVELOCITY)
            nScrollVelocity = -MAXSCROLLVELOCITY;

        nScrollDistance += nScrollVelocity;

        // Don't go over the boundaries
        if (nScrollDistance <= 0 && nScrollVelocity < 0)
        {
            nScrollDistance = 0;
            nScrollVelocity = 0;
        }
        else if (nScrollDistance >= dMAXGAMESCROLLDISTANCE && nScrollVelocity > 0)
        {
            nScrollDistance = dMAXGAMESCROLLDISTANCE;
            nScrollVelocity = 0;
        }

        Globals.myLayers.SetScrollDistance(nScrollDistance);
    }

    public Layer GetLayer(int nLayer)
    {
        return myLayer[nLayer];
    }

    public void ResetTo(int nD, int nV, bool bForce)
    {
        SetScrollDistance(nD);
        nScrollVelocity = nV;
        bForceScrollToBottom = bForce;
    }

    public void ResetForGame()
    {
        ResetTo(dGAMESTARTSCROLLDISTANCE, dGAMESTARTSCROLLVELOCITY, true);
    }

    public void ResetForMenu()
    {
        ResetTo(0, 0, false);
    }
}