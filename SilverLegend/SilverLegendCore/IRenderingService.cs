using System;
using System.Collections.Generic;
using System.Text;

public interface IRenderingService
{
    bool Initialize();

    /// <summary>
    /// Load a bitmap set into memory
    /// </summary>
    /// <param name="bitmapSet">The bitmap set to load</param>
    /// <param name="percentageToLoad">The percentage of the set to load</param>
    /// <returns>True if 100% of the bitmap set has been loaded into memory</returns>
    bool LoadBitmapSet(BitmapSets bitmapSet, int percentageToLoad);
    bool DisposeAll();

    void DrawBitmap(TSprite associatedSprite, FrameDesc frameDesc, int nScrx, int nScry, 
        byte[] replaceRGB, 
        byte[] substituteRGB);
    void DrawText(TSprite associatedSprite, SpriteTextType textType, string text, bool boldWeight, int x, int y, byte r, byte g, byte b, byte a);

    void Drawing();
    void Drawn();
    void RenderToScreen();

    int GetFps();
}


    //public void DisplaySplashScreen(int nDistance)
    //{
    //    if (!DDSetDDPal(AIMethods.frm[((int)GameBitmapEnumeration.bmpSPLASHSCREEN)]))
    //        return;

    //    AIMethods.frm[((int)GameBitmapEnumeration.bmpSPLASHSCREEN)].Draw(0, 0, false);
    //    AIMethods.frm[((int)GameBitmapEnumeration.bmpBTNTOGGLE)].Draw(25 + (nDistance * 540 / 100), 410, true);
    //    DDFlip(gbIsWindowed, hwnd);   // Flip front and back buffers (WAIT UNTIL CLOCK)
    //}


    //public bool Initialize_Bitmaps(int nSet)
    //{
    //    // Initialize the bitmaps used in the game.  Call a function in ai.cpp.

    //    if (!(bFrameSetLoaded[nSet] && !(nSet == ((int)BitmapSets.bmpLoadBitmaps))))
    //    {
    //        switch (nSet)
    //        {
    //            case ((int)BitmapSets.bmpLoadBitmaps):
    //                bFrameSetLoaded[nSet] = Initialize_Frames("graphics\\loadbitmaps.txt", ((int)GameBitmapEnumeration.bmpSPLASHSCREEN), true);
    //                break;
    //            case ((int)BitmapSets.bmpGameBitmaps):
    //                bFrameSetLoaded[nSet] = Initialize_Frames("graphics\\gamebitmaps.txt", ((int)GameBitmapEnumeration.bmpFR1_1));
    //                break;
    //            case ((int)BitmapSets.bmpMenuBitmaps):
    //                bFrameSetLoaded[nSet] = Initialize_Frames("graphics\\menubitmaps.txt", ((int)GameBitmapEnumeration.bmpJACKETBACK));
    //                break;
    //            case ((int)BitmapSets.bmpTransBitmaps):
    //                bFrameSetLoaded[nSet] = Initialize_Frames("graphics\\transbitmaps.txt", ((int)GameBitmapEnumeration.bmpSLAMJACKET1));
    //                break;
    //        }
    //    }

    //    return bFrameSetLoaded[nSet];
    //}

    //public bool Deallocate_Bitmaps(int nSet)
    //{

    //    if (!(bFrameSetLoaded[nSet]))
    //        return true;

    //    // Initialize the bitmaps used in the game.  Call a function in ai.cpp.
    //    switch (nSet)
    //    {
    //        case ((int)BitmapSets.bmpLoadBitmaps):
    //            bFrameSetLoaded[nSet] = !(Release_Frames("graphics\\loadbitmaps.txt", ((int)GameBitmapEnumeration.bmpSPLASHSCREEN)));
    //            break;
    //        case ((int)BitmapSets.bmpGameBitmaps):
    //            bFrameSetLoaded[nSet] = !(Release_Frames("graphics\\gamebitmaps.txt", ((int)GameBitmapEnumeration.bmpFR1_1)));
    //            break;
    //        case ((int)BitmapSets.bmpMenuBitmaps):
    //            bFrameSetLoaded[nSet] = !(Release_Frames("graphics\\menubitmaps.txt", ((int)GameBitmapEnumeration.bmpJACKETBACK)));
    //            break;
    //        case ((int)BitmapSets.bmpTransBitmaps):
    //            bFrameSetLoaded[nSet] = !(Release_Frames("graphics\\transbitmaps.txt", ((int)GameBitmapEnumeration.bmpSLAMJACKET1)));
    //            break;
    //    }
    //    return false;
    //}

    //public bool Release_Frames(string sFilename, int fr)
    //{
    //    return true; // No functionality
    //}


    //public bool Initialize_Frames(string sFilename, int fr)
    //{
    //    return Initialize_Frames(sFilename, fr, false);
    //}

    //public bool Initialize_Frames(string sFilename, int fr, bool bDisplayFirst)
    //{

    //    // Initialize a set of frames after copying in size and hotspot info.
    //    char[] s = new char[MAXFILELINESIZE + 10];
    //    char[] sBMPFileName = new char[MAXFILELINESIZE + 10];
    //    int nNewHotSpotX, nNewHotSpotY;
    //    bool bMakeMirrorImage, bLoadImmediately;
    //    int nX1, nX2, nZ1, nZ2;

    //    bool GetCollisionCoordinates;

    //    // This function is slimy and filthy and vomitous.
    //    // Vomitous is now a word.

    //    // Open the Bitmap Registry file.
    //    FILE* fInput;
    //    FILE* fTemp;
    //    if ((fInput = fopen(sFilename, "rt")) == null)
    //        return false;

    //    while (!feof(fInput))
    //    {
    //        AIMethods.strcpy(s, "graphics\\");
    //        fgets(&s[9], 80, fInput);
    //        if (!(s[9] == '/' || s[9] == '\n' || feof(fInput)))
    //        {

    //            // Obtain info re: Collision Coordinates and file name
    //            if (s[strlen(s) - 2] == '*')
    //            {
    //                GetCollisionCoordinates = true; s[strlen(s) - 2] = '\0';
    //            }
    //            else
    //            {
    //                GetCollisionCoordinates = false; s[strlen(s) - 1] = '\0';
    //            }
    //            strcpy(sBMPFileName, s);

    //            if ((fTemp = fopen(sBMPFileName, "r")) == null)
    //            {
    //                Globals.Debug("The following bitmap was not found; a silly graphic was substituted:");
    //                Globals.Debug(sBMPFileName);
    //                strcpy(sBMPFileName, "graphics\\dummy.bmp");
    //            }
    //            else
    //                fclose(fTemp);


    //            fgets(s, 20, fInput);
    //            if (s[strlen(s) - 2] == '^')
    //            {
    //                bMakeMirrorImage = true; s[strlen(s) - 2] = '\0';
    //            }
    //            else
    //            {
    //                bMakeMirrorImage = false; s[strlen(s) - 1] = '\0';
    //            }
    //            nNewHotSpotX = System.Int32.Parse(s);

    //            fgets(s, 20, fInput);
    //            if (s[strlen(s) - 2] == '!')
    //            {
    //                bLoadImmediately = true; s[strlen(s) - 2] = '\0';
    //            }
    //            else
    //            {
    //                bLoadImmediately = false; s[strlen(s) - 1] = '\0';
    //            }
    //            nNewHotSpotY = atoi(s);

    //            nX1 = nZ1 = nX2 = nZ2 = 0;

    //            if (GetCollisionCoordinates)
    //            {
    //                fgets(s, 20, fInput); nX1 = atoi(s);
    //                fgets(s, 20, fInput); nZ1 = atoi(s);
    //                fgets(s, 20, fInput); nX2 = atoi(s);
    //                fgets(s, 20, fInput); nZ2 = atoi(s);
    //            }

    //            // Right now the code is setting itself up to mirror ANYTHING
    //            // it wants to.
    //            if (fr == ((int)GameBitmapEnumeration.bmpSPLASHSCREEN) || (fr >= ((int)GameBitmapEnumeration.bmpJACKETBACK)))
    //                AIMethods.frm[fr].bForceToMemory = true;
    //            AIMethods.frm[fr].InitFrame(sBMPFileName, nNewHotSpotX, nNewHotSpotY,
    //                nX1, nZ1, nX2, nZ2, false);
    //            AIMethods.frmM[fr].InitFrame(sBMPFileName, nNewHotSpotX, nNewHotSpotY,
    //                nX1, nZ1, nX2, nZ2, true, AIMethods.frm[fr]);
    //            if (bLoadImmediately)
    //            {
    //                //But it actually only loads the mirrored images on demand.
    //                AIMethods.frm[fr].LoadIntoMemory(false);
    //                if (bMakeMirrorImage) AIMethods.frmM[fr].LoadIntoMemoryMirrored(false);
    //            }
    //            fr++;
    //        }
    //    }

    //    // Close the bitmap registry
    //    fclose(fInput);
    //    return true;
    //}

