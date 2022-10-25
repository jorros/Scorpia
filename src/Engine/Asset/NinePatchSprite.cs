using System;
using System.Drawing;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Helper;
using static SDL2.SDL;

namespace Scorpia.Engine.Asset;

internal class NinePatchSprite : Sprite
{
    private SDL_Rect _topLeft;
    private SDL_Rect _top;
    private SDL_Rect _topRight;
    private SDL_Rect _right;
    private SDL_Rect _bottomRight;
    private SDL_Rect _bottom;
    private SDL_Rect _bottomLeft;
    private SDL_Rect _left;
    private SDL_Rect _center;

    private SpritesheetFrame _frame;

    public NinePatchSprite(IntPtr texture, SpritesheetFrame frame) : base(texture,
        new Size(frame.OriginalSize.X, frame.OriginalSize.Y))
    {
        _frame = frame;

        var split = frame.Split!.Value;
        var size = frame.OriginalSize;

        _topLeft = SdlHelper.Create(frame.Position.X, frame.Position.Y, split.X, split.Y);
        _top = SdlHelper.Create(frame.Position.X + split.X, frame.Position.Y, size.X - (split.Width + split.X), split.Y);
        _topRight = SdlHelper.Create(frame.Position.X + size.X - split.Width, frame.Position.Y, split.Width, split.Y);
        _right = SdlHelper.Create(frame.Position.X + size.X - split.Width, frame.Position.Y + split.Y, split.Width, size.Y - (split.Height + split.Y));
        _bottomRight = SdlHelper.Create(frame.Position.X + size.X - split.Width, frame.Position.Y + size.Y - split.Y, split.Width, split.Height);
        _bottom = SdlHelper.Create(frame.Position.X + split.X, frame.Position.Y + size.Y - split.Y, size.X - (split.Width + split.X), split.Height);
        _bottomLeft = SdlHelper.Create(frame.Position.X, frame.Position.Y + size.Y - split.Y, split.X, split.Height);
        _left = SdlHelper.Create(frame.Position.X, frame.Position.Y + split.Y, split.X, size.Y - (split.Height + split.Y));
        _center = SdlHelper.Create(frame.Position.X + split.X, frame.Position.Y + split.Y, size.X - (split.Width + split.X), size.Y - (split.Height + split.Y));
    }

    internal override void Render(GraphicsManager context, Rectangle? src, RectangleF? dest, double angle, Color color, byte alpha, int index)
    {
        SDL_SetTextureColorMod(Texture, color.R, color.G, color.B);
        SDL_SetTextureAlphaMod(Texture, alpha);

        var center = new SDL_FPoint()
        {
            x = 0,
            y = 0
        };

        if (dest is null)
        {
            SDL_RenderGetLogicalSize(context.Renderer, out var w, out var h);
            if (w == 0 && h == 0)
            {
                SDL_RenderGetViewport(context.Renderer, out var viewport);
                dest = new RectangleF(viewport.x, viewport.y, viewport.w, viewport.h);
            }
            else
            {
                dest = new RectangleF(0, 0, w, h);
            }
        }

        var centerWidth = dest.Value.Width - (_frame.Split!.Value.X + _frame.Split.Value.Width);
        var centerHeight = dest.Value.Height - (_frame.Split.Value.Y + _frame.Split.Value.Height);

        SDL_SetRenderDrawColor(context.Renderer, 255, 0, 0, 100);

        // Draw top left
        var target = new SDL_FRect
        {
            x = dest.Value.X,
            y = dest.Value.Y,
            w = _topLeft.w,
            h = _topLeft.h
        };
        SDL_RenderCopyExF(context.Renderer, Texture, ref _topLeft, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);

        // Draw top
        target = new SDL_FRect
        {
            x = dest.Value.X + _topLeft.w,
            y = dest.Value.Y,
            w = centerWidth,
            h = _top.h
        };
        SDL_RenderCopyExF(context.Renderer, Texture, ref _top, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);

        // Draw top right
        target = new SDL_FRect
        {
            x = dest.Value.X + _topLeft.w + centerWidth,
            y = dest.Value.Y,
            w = _topRight.w,
            h = _topRight.h
        };
        SDL_RenderCopyExF(context.Renderer, Texture, ref _topRight, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);

        // Draw left
        target = new SDL_FRect
        {
            x = dest.Value.X,
            y = dest.Value.Y + _topLeft.h,
            w = _left.w,
            h = centerHeight
        };
        SDL_RenderCopyExF(context.Renderer, Texture, ref _left, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
        
        // // Draw center
        target = new SDL_FRect
        {
            x = dest.Value.X + _topLeft.w,
            y = dest.Value.Y + _topLeft.h,
            w = centerWidth,
            h = centerHeight
        };
        SDL_RenderCopyExF(context.Renderer, Texture, ref _center, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
        
        // Draw right
        target = new SDL_FRect
        {
            x = dest.Value.X + _left.w + centerWidth,
            y = dest.Value.Y + _topLeft.h,
            w = _right.w,
            h = centerHeight
        };
        SDL_RenderCopyExF(context.Renderer, Texture, ref _right, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
        
        // Draw bottom left
        target = new SDL_FRect
        {
            x = dest.Value.X,
            y = dest.Value.Y + _topLeft.h + centerHeight,
            w = _bottomLeft.w,
            h = _bottomLeft.h
        };
        SDL_RenderCopyExF(context.Renderer, Texture, ref _bottomLeft, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
        
        // Draw bottom
        target = new SDL_FRect
        {
            x = dest.Value.X + _bottomLeft.w,
            y = dest.Value.Y + _topLeft.h + centerHeight,
            w = centerWidth,
            h = _bottom.h
        };
        SDL_RenderCopyExF(context.Renderer, Texture, ref _bottom, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
        
        // Draw bottom right
        target = new SDL_FRect
        {
            x = dest.Value.X + _bottomLeft.w + centerWidth,
            y = dest.Value.Y + _topLeft.h + centerHeight,
            w = _bottomRight.w,
            h = _bottomRight.h
        };
        SDL_RenderCopyExF(context.Renderer, Texture, ref _bottomRight, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
    }
}