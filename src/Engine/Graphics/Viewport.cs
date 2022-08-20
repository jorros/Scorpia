using System;
using System.Drawing;
using System.Runtime.InteropServices;
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
        Draw(sprite, position, 0, OffsetVector.One, null, Color.White, 255);
    }

    public void Draw(Sprite sprite, OffsetVector position, OffsetVector scale, OffsetVector? center)
    {
        Draw(sprite, position, 0, scale, center, Color.White, 255);
    }

    public void Draw(Sprite sprite, OffsetVector position, double angle, OffsetVector scale, OffsetVector? center,
        Color color, byte alpha)
    {
        var scaledW = sprite.Width * scale.X;
        var scaledH = sprite.Height * scale.Y;

        center ??= new OffsetVector(scaledW / 2, scaledH / 2);

        var target = new SDL_Rect
        {
            x = (int) (position.X - center.Value.X) - (int) WorldPosition.X,
            y = (int) (position.Y - center.Value.Y) - (int) WorldPosition.Y,
            h = (int) scaledH,
            w = (int) scaledW
        };

        var src = IntPtr.Zero;

        if (sprite.SrcX is not null && sprite.SrcY is not null)
        {
            var srcRect = new SDL_Rect
            {
                w = sprite.Width,
                h = sprite.Height,
                x = sprite.SrcX.Value,
                y = sprite.SrcY.Value
            };

            src = Marshal.AllocHGlobal(Marshal.SizeOf(srcRect));
            Marshal.StructureToPtr(srcRect, src, false);
        }

        var centerSdl = center.Value.ToSdl();

        SDL_SetTextureColorMod(sprite.Texture, color.R, color.G, color.B);
        SDL_SetTextureAlphaMod(sprite.Texture, alpha);

        SDL_RenderCopyEx(_graphicsManager.Renderer, sprite.Texture, src, ref target, angle, ref centerSdl,
            SDL_RendererFlip.SDL_FLIP_NONE);

        if (src != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(src);
        }
    }
}