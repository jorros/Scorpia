using System;
using System.Drawing;
using Scorpia.Engine.Graphics;
using static SDL2.SDL;

namespace Scorpia.Engine.Asset;

public abstract class Sprite : IAsset, IDisposable
{
    public OffsetVector Size { get; }
    public OffsetVector Center { get; set; }
    
    internal IntPtr Texture { get; }

    protected Sprite(IntPtr texture, OffsetVector size)
    {
        Texture = texture;
        Size = size;
    }
    
    internal abstract void Render(GraphicsManager context, Rectangle dest, double angle, Color color, byte alpha);
    
    public void Dispose()
    {
        SDL_DestroyTexture(Texture);
    }
}