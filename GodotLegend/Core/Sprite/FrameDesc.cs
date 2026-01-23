public class FrameDesc
{
    public string szBitmap;	// filename the bitmap comes from

    public int nHotSpotX;   // X-location of the Hotspot
    public int nHotSpotY;   // Y-location of the Hotspot

    public int nX1;   // "Collision" Co-ordinates
    public int nZ1;   //  1 *---|
    public int nX2;   //    |   |
    public int nZ2;   //    |---* 2

    public int nBitmapWidthSilverlight; //
    public int nBitmapHeightSilverlight; //

    ////// VERY IMPORTANT THAT YOU SALVAGE THIS FUNCTIONALITY IN THE LOADER WHEN LOADING MIRRORS
    //int nTemp;
    //nTemp = nX1; nX1 = nX2; nX2 = nTemp;
    //nHotSpotX = frmUnMirrored.nWidth - nHotSpotX;

    public bool bIsMirror; // is it the mirror of a frame?
    public FrameDesc frmUnMirrored;

    public object Tag; // Reserved for implementation details - link to sprite
    public bool IsInitialized { get { return bInitialized; } }
    private bool bInitialized;
    public void InitFrame(string s, int nNewHotSpotX, int nNewHotSpotY,
                int nnX1, int nnZ1, int nnX2, int nnZ2, bool bNewIsMirror
                )
    {
        InitFrame(s, nNewHotSpotX, nNewHotSpotY,
                nnX1, nnZ1, nnX2, nnZ2, bNewIsMirror, null);
    }

    public void InitFrame(string s, int nNewHotSpotX, int nNewHotSpotY,
                    int nnX1, int nnZ1, int nnX2, int nnZ2, bool bNewIsMirror,
                    FrameDesc myOriginal)
    {
        szBitmap = s;
        nHotSpotX = nNewHotSpotX;
        nHotSpotY = nNewHotSpotY;
        nX1 = nnX1;
        nZ1 = nnZ1;
        nX2 = nnX2;
        nZ2 = nnZ2;

        bIsMirror = bNewIsMirror;
        frmUnMirrored = myOriginal;
        bInitialized = true;
    }

    public void Draw(TSprite associatedSprite, int nScrx, int nScry, byte[] replaceRGB, byte[] substituteRGB)
    {
        Globals.RenderingService.DrawBitmap(associatedSprite, this, nScrx, nScry, replaceRGB, substituteRGB);
    }


}


