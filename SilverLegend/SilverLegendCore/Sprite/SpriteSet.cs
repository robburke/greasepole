public class SpriteSet
{
    public const int MAX_SPRITES_PER_SET = 600;
    public const bool ssDONOTDELETE = true;

    public TSprite[] sprites = new TSprite[MAX_SPRITES_PER_SET]; // The sprites themselves (should be private)

    public Layer myLayer;              // The Layer co-ordiate(s)
    public int n;                       // The number of sprites in this set


    public SpriteSet(Layer nNewLayer)
    {
        // Initialize the Layer co-ordinate(s) and set the number of sprites to 0
        SetLayer(nNewLayer);
        n = 0;
    }

    public void SetLayer(Layer nNewLayer)
    {
        // Sets the layer associated with this spriteset.  Note that x-coordinate
        // functionality may be coded later on.
        myLayer = nNewLayer;
    }

    public void Include(TSprite aSprite)
    {
        // Insert a sprite into the set

        if (n > MAX_SPRITES_PER_SET)
            Globals.Debug("Too many sprites in Spriteset!  Increase MAX_SPRITES_PER_SET!");
        // This Sprite is going to get "lost in space" -- prepare for a crash.
        else
        {
            sprites[n] = aSprite;
            n++;
        }
    }

    public TSprite GetSprite(int i)
    {
        return sprites[i];
    }

    public int GetLeftMostPointOnSprite(int i)
    {
        return sprites[i].nX - sprites[i].frmFrame.nX1;
    }
    public int GetTopMostPointOnSprite(int i)
    {
        return sprites[i].nY - AIMethods.dFROSHWIDTHY;//- sprites[i].frmFrame.nZ1;
    }
    public int GetRightMostPointOnSprite(int i)
    {
        return sprites[i].nX + sprites[i].frmFrame.nX2;
    }
    public int GetBottomMostPointOnSprite(int i)
    {
        return sprites[i].nY + AIMethods.dFROSHWIDTHY;//+ sprites[i].frmFrame.nZ2;
    }

    public int GetLeftMostScrPointOnSprite(int i)
    {
        return sprites[i].nScrx
            + sprites[i].frmFrame.nHotSpotX - sprites[i].frmFrame.nX1;
    }
    public int GetTopMostScrPointOnSprite(int i)
    {
        return sprites[i].nScry
            + sprites[i].frmFrame.nHotSpotY - sprites[i].frmFrame.nZ1;
    }
    public int GetRightMostScrPointOnSprite(int i)
    {
        return sprites[i].nScrx
            + sprites[i].frmFrame.nHotSpotX + sprites[i].frmFrame.nX2;
    }
    public int GetBottomMostScrPointOnSprite(int i)
    {
        return sprites[i].nScry
            + sprites[i].frmFrame.nHotSpotY + sprites[i].frmFrame.nZ2;
    }

    public int GetX(int i)
    {
        return sprites[i].nX;
    }

    public int GetY(int i)
    {
        return sprites[i].nY;
    }

    public int GetZ(int i)
    {
        return sprites[i].nZ;
    }


    public int GetNumberOfSprites()
    {
        return n;
    }


    public void Remove(TSprite aSprite)
    {
        // Remove a sprite from the set

        int i = 0;

        while (i < n)
        {
            if (sprites[i] == aSprite)
            {
                n--;
                while (i < n)
                {
                    sprites[i] = sprites[(i + 1)];
                    i++;
                }
            }
            else
                i++;
        }  // end while
    }
    public void RemoveAll()
    {
        n = 0;
    }

    public void Flush()
    {
        Flush(false);
    }

    public void Flush(bool bDoNotDelete)
    {	// Mark all of my sprites as deleted.
        if (bDoNotDelete)
        {
            n = 0;
            return;
        }
        else
        {
            for (int i = 0; i < n; i++)
                sprites[i].bDeleted = true;
            // Compact me.
            Compact();
        }
    }

    public void Compact()
    {
        // Destroy sprites marked for deletion within the set

        int nFree = 0;
        for (int nTest = 0; nTest < n; nTest++)
        {
            if (sprites[nTest].bDeleted)
                SpriteInit.DeleteSprite(sprites[nTest]);
            else
            {
                sprites[nFree] = sprites[nTest];
                nFree++;
            }
        }
        n = nFree;
    }


    ///////////////////////////////////////////////////////////////////
    // OrderByY and Support Functions
    ///////////////////////////////////////////////////////////////////

    public void BubbleSortY()
    {
        //  return;
        int i; int j;
        TSprite Temp;
        for (i = 0; i < n; i++)
            for (j = i + 1; j < n; j++)
                if (sprites[i].nY > sprites[j].nY)
                {
                    Temp = sprites[i];
                    sprites[i] = sprites[j];
                    sprites[j] = Temp;
                    //				Globals.Debug("The Y-coordinate made a difference");
                }
                else if (sprites[i].nY == sprites[j].nY)
                {
                    if (sprites[i].nZ < sprites[j].nZ)
                    {
                        Temp = sprites[i];
                        sprites[i] = sprites[j];
                        sprites[j] = Temp;
                        //					Globals.Debug("The Z-coordinate made a difference");
                    }
                    else if (sprites[i].nZ == sprites[j].nZ)
                    {
                        if (sprites[i].nTag > sprites[j].nTag)
                        {
                            Temp = sprites[i];
                            sprites[i] = sprites[j];
                            sprites[j] = Temp;
                            //					Globals.Debug("Had to resort to the Tag.");
                        }
                    }
                }
    }

    public void OrderByY()
    {
        // Order a set by:
        //1.	y-coordinates, then by
        //2.	z-coordinates, then by
        //3.	The tag

        // TEMPORARY SOLUTION: USE A BUBBLE-SORT
        BubbleSortY();
    }


    ///////////////////////////////////////////////////////////////////
    // OrderByX and Support Functions
    ///////////////////////////////////////////////////////////////////
    public void BubbleSortX()
    {
        //  return;
        int i; int j;
        TSprite Temp;
        for (i = 0; i < n; i++)
            for (j = i + 1; j < n; j++)
                if (sprites[i].nX > sprites[j].nX)
                {
                    Temp = sprites[i];
                    sprites[i] = sprites[j];
                    sprites[j] = Temp;
                }
    }

    public void OrderByX()
    {
        // Order a set by X-coordinates

        // TEMPORARY SOLUTION: USE A BUBBLE-SORT
        BubbleSortX();
    }

    public void Think()
    {
        // Cause each Sprite to "Think"
        int nTemp = n;  // If new sprites end up getting added they shouldn't be drawn
        for (int i = 0; i < nTemp; i++)
            if (!(sprites[i].bDeleted))
                sprites[i].Think();
    }

    public void CalculateScreenCoordinates()
    {
        // Calculate the ScrX, ScrY coordinates for each sprite.

        for (int i = 0; i < n; i++)
            if (!(sprites[i].bDeleted))
                sprites[i].CalculateScreenCoordinates(myLayer.GetY());
    }

    public void Draw()
    {
        // Blit each sprite to the DDraw back-buffer, using color key.
        for (int i = 0; i < n; i++)
            if (!(sprites[i].bDeleted))
                sprites[i].Draw();
    }

    public void DrawMultiCultural()
    {
        TSprite s;

        // Blit each sprite to the DDraw back-buffer, using color key.
        for (int i = 0; i < n; i++)
        {
            s = sprites[i];
            if (!(sprites[i].bDeleted))
            {
                sprites[i].Draw();
            }
        }
    }



}

