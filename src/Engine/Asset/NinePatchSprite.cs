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
        new OffsetVector(frame.OriginalSize.X, frame.OriginalSize.Y))
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

    internal override void Render(GraphicsManager context, Rectangle dest, double angle, Color color, byte alpha)
    {
        SDL_SetTextureColorMod(Texture, color.R, color.G, color.B);
        SDL_SetTextureAlphaMod(Texture, alpha);

        var center = new SDL_Point()
        {
            x = 0,
            y = 0
        };

        var centerWidth = dest.Width - (_frame.Split!.Value.X + _frame.Split.Value.Width);
        var centerHeight = dest.Height - (_frame.Split.Value.Y + _frame.Split.Value.Height);

        SDL_SetRenderDrawColor(context.Renderer, 255, 0, 0, 100);

        // Draw top left
        var target = new SDL_Rect
        {
            x = dest.X,
            y = dest.Y,
            w = _topLeft.w,
            h = _topLeft.h
        };
        SDL_RenderCopyEx(context.Renderer, Texture, ref _topLeft, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);

        // Draw top
        target = new SDL_Rect
        {
            x = dest.X + _topLeft.w,
            y = dest.Y,
            w = centerWidth,
            h = _top.h
        };
        SDL_RenderCopyEx(context.Renderer, Texture, ref _top, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);

        // Draw top right
        target = new SDL_Rect
        {
            x = dest.X + _topLeft.w + centerWidth,
            y = dest.Y,
            w = _topRight.w,
            h = _topRight.h
        };
        SDL_RenderCopyEx(context.Renderer, Texture, ref _topRight, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);

        // Draw left
        target = new SDL_Rect
        {
            x = dest.X,
            y = dest.Y + _topLeft.h,
            w = _left.w,
            h = centerHeight
        };
        SDL_RenderCopyEx(context.Renderer, Texture, ref _left, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
        
        // // Draw center
        target = new SDL_Rect
        {
            x = dest.X + _topLeft.w,
            y = dest.Y + _topLeft.h,
            w = centerWidth,
            h = centerHeight
        };
        SDL_RenderCopyEx(context.Renderer, Texture, ref _center, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
        
        // Draw right
        target = new SDL_Rect
        {
            x = dest.X + _left.w + centerWidth,
            y = dest.Y + _topLeft.h,
            w = _right.w,
            h = centerHeight
        };
        SDL_RenderCopyEx(context.Renderer, Texture, ref _right, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
        
        // Draw bottom left
        target = new SDL_Rect
        {
            x = dest.X,
            y = dest.Y + _topLeft.h + centerHeight,
            w = _bottomLeft.w,
            h = _bottomLeft.h
        };
        SDL_RenderCopyEx(context.Renderer, Texture, ref _bottomLeft, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
        
        // Draw bottom
        target = new SDL_Rect
        {
            x = dest.X + _bottomLeft.w,
            y = dest.Y + _topLeft.h + centerHeight,
            w = centerWidth,
            h = _bottom.h
        };
        SDL_RenderCopyEx(context.Renderer, Texture, ref _bottom, ref target, 0, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
        
        // Draw bottom right
        target = new SDL_Rect
        {
            x = dest.X + _bottomLeft.w + centerWidth,
            y = dest.Y + _topLeft.h + centerHeight,
            w = _bottomRight.w,
            h = _bottomRight.h
        };
        SDL_RenderCopyEx(context.Renderer, Texture, ref _bottomRight, ref target, 0, ref center,
            SDL_RendererFlip.SDL_FLIP_NONE);
        // SDL_RenderDrawRect(context.Renderer, ref target);
    }
}