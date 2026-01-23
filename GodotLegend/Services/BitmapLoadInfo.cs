/// <summary>
/// Bitmap loading info - matches the original Silverlight BitmapLoadInfo structure
/// </summary>
public class BitmapLoadInfo
{
    public string BitmapName;
    public int HotspotX;
    public int HotspotY;
    public int SizeX1;
    public int SizeY1;
    public int SizeX2;
    public int SizeY2;
    public int Width;
    public int Height;
    public bool CreateMirror;
    public bool Flag2;
    public bool Flag3;

    public BitmapLoadInfo(string bitmapName, int hotspotX, int hotspotY,
        int sizeX1, int sizeY1, int sizeX2, int sizeY2,
        int width, int height, bool createMirror, bool flag2, bool flag3)
    {
        BitmapName = bitmapName;
        HotspotX = hotspotX;
        HotspotY = hotspotY;
        SizeX1 = sizeX1;
        SizeY1 = sizeY1;
        SizeX2 = sizeX2;
        SizeY2 = sizeY2;
        Width = width;
        Height = height;
        CreateMirror = createMirror;
        Flag2 = flag2;
        Flag3 = flag3;
    }
}
