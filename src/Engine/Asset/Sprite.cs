using System;

namespace Scorpia.Engine.Asset;

public class Sprite : IAsset
{
    internal IntPtr Texture { get; }
    
    public int Width { get; }
    
    public int Height { get; }
    
    internal int? SrcX { get; set; }
    
    internal int? SrcY { get; set; }

    internal Sprite(IntPtr texture, int width, int height)
    {
        Texture = texture;
        Width = width;
        Height = height;
    }

    internal Sprite(IntPtr texture, int srcX, int srcY, int width, int height) : this(texture, width, height)
    {
        SrcX = srcX;
        SrcY = srcY;
    }
}