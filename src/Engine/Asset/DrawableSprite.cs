using System;
using System.Drawing;
using Scorpia.Engine.Graphics;
using static SDL2.SDL;

namespace Scorpia.Engine.Asset;

public class DrawableSprite : Sprite
{
    private readonly GraphicsManager _graphicsManager;

    internal override void Render(GraphicsManager context, Rectangle? src, RectangleF dest, double angle, Color color, byte alpha, int index)
    {
        var target = new SDL_FRect
        {
            x = dest.X,
            y = dest.Y,
            h = dest.Height,
            w = dest.Width
        };

        SDL_SetTextureColorMod(Texture, color.R, color.G, color.B);
        SDL_SetTextureAlphaMod(Texture, alpha);

        SDL_RenderCopyExF(context.Renderer, Texture, IntPtr.Zero, ref target, angle, IntPtr.Zero, 
            SDL_RendererFlip.SDL_FLIP_NONE);
    }

    public void BeginDraw()
    {
        SDL_SetRenderTarget(_graphicsManager.Renderer, Texture);
    }

    public void EndDraw()
    {
        SDL_SetRenderTarget(_graphicsManager.Renderer, IntPtr.Zero);
    }
    
    internal DrawableSprite(GraphicsManager graphicsManager, IntPtr texture, Size size) : base(texture, size)
    {
        _graphicsManager = graphicsManager;
    }
}