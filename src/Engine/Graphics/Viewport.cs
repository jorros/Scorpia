using System;
using System.Drawing;
using Scorpia.Engine.Asset;
using static SDL2.SDL;

namespace Scorpia.Engine.Graphics;

public class Viewport
{
    private readonly GraphicsManager _graphicsManager;
    private SDL_Rect _rect;
    private SDL_Rect _previousRect;

    public OffsetVector WorldPosition { get; set; }

    internal Viewport(GraphicsManager graphicsManager, SDL_Rect rect)
    {
        _graphicsManager = graphicsManager;
        _rect = rect;
        WorldPosition = OffsetVector.Zero;
    }

    public void Begin()
    {
        SDL_RenderGetViewport(_graphicsManager.Renderer, out _previousRect);
        SDL_RenderSetViewport(_graphicsManager.Renderer, ref _rect);
    }

    public void End()
    {
        SDL_RenderSetViewport(_graphicsManager.Renderer, ref _previousRect);
    }

    public void SetClipping(Rectangle? rectangle)
    {
        if (rectangle is null)
        {
            SDL_RenderSetClipRect(_graphicsManager.Renderer, IntPtr.Zero);

            return;
        }

        var rect = new SDL_Rect
        {
            x = rectangle.Value.Left,
            y = rectangle.Value.Top,
            h = rectangle.Value.Height,
            w = rectangle.Value.Width
        };

        if (_rect.x == rect.x && _rect.y == rect.y && _rect.w == rect.w && _rect.h == rect.h)
        {
            SDL_RenderSetClipRect(_graphicsManager.Renderer, IntPtr.Zero);

            return;
        }

        SDL_RenderSetClipRect(_graphicsManager.Renderer, ref rect);
    }

    public Rectangle GetClipping()
    {
        SDL_RenderGetClipRect(_graphicsManager.Renderer, out var rect);

        return new Rectangle
        {
            X = rect.x,
            Y = rect.y,
            Height = rect.h,
            Width = rect.w
        };
    }

    public void Draw(Sprite sprite, OffsetVector position)
    {
        var dest = new Rectangle((int)position.X, (int)position.Y, (int)sprite.Size.X, (int)sprite.Size.Y);
        Draw(sprite, dest, 0, Color.White, 255);
    }

    public void Draw(Sprite sprite, OffsetVector position, OffsetVector scale)
    {
        
    }

    public void Draw(Sprite sprite, Rectangle dest, double angle, Color color, byte alpha)
    {
        var _dest = dest with {X = dest.X - (int)WorldPosition.X, Y = dest.Y - (int)WorldPosition.Y};

        sprite.Render(_graphicsManager, _dest, angle, color, alpha);
    }
}