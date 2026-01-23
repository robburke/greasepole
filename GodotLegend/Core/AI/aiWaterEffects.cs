public static partial class AIMethods
{
    public static void aiSplashM(TSprite s)
    {
        // Animates a medium-sized splash, then deletes the associated sprite
        if (s.nCC < 12)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSPLASHM1) + (s.nCC / 4)]);
        else
            s.bDeleted = true;
    }

    public static void aiSplashML(TSprite s)
    {
        // Animates a medium-sized left-oriented splash, then deletes the associated sprite
        if (s.nCC < 12)
            s.SetFrame(frmM[((int)GameBitmapEnumeration.bmpSPLASHM1) + (s.nCC / 4)]);
        else
            s.bDeleted = true;
    }

    public static void aiSplashL(TSprite s)
    {
        // Animates a medium-sized splash, then deletes the associated sprite
        if (s.nCC < 12)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSPLASHL1) + (s.nCC / 4)]);
        else
            s.bDeleted = true;
    }
    public static void aiSplashS(TSprite s)
    {
        // Animates a medium-sized splash, then deletes the associated sprite
        if (s.nCC < 12)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpSPLASHS1) + (s.nCC / 4)]);
        else
            s.bDeleted = true;
    }


    public static void aiRipple(TSprite s)
    {
        // Animates a ripple, then deletes the associated sprite
        if (s.nCC < 9)
            s.SetFrame(frm[((int)GameBitmapEnumeration.bmpRIPPLE1) + (s.nCC / 3)]);
        else
            s.bDeleted = true;
    }

}