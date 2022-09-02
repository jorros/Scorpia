using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Helper;
using static SDL2.SDL;

namespace Scorpia.Engine.Asset;

public class TextureSprite : Sprite
{
    private SpritesheetFrame _frame;

    internal TextureSprite(IntPtr texture, int width, int height) : base(texture, new OffsetVector(width, height))
    {
        Center = new OffsetVector(width / 2, height / 2);
    }

    internal TextureSprite(IntPtr texture, SpritesheetFrame frame) : this(texture, frame.OriginalSize.X,
        frame.OriginalSize.Y)
    {
        _frame = frame;
    }

    internal override void Render(GraphicsManager context, Rectangle dest, double angle, Color color, byte alpha)
    {
        var target = new SDL_Rect
        {
            x = dest.X,
            y = dest.Y,
            h = dest.Height,
            w = dest.Width
        };

        var src = IntPtr.Zero;

        if (_frame is not null)
        {
            var srcRect = new SDL_Rect
            {
                w = _frame.Rotated ? _frame.Size.Y : _frame.Size.X,
                h = _frame.Rotated ? _frame.Size.X : _frame.Size.Y,
                x = _frame.Position.X,
                y = _frame.Position.Y
            };

            var offX = _frame.Rotated ? _frame.Offset.Y : _frame.Offset.X;
            var offY = _frame.Rotated ? _frame.Offset.X : _frame.Offset.Y;

            var w = _frame.Rotated ? _frame.Size.Y : _frame.Size.X;
            var h = _frame.Rotated ? _frame.Size.X : _frame.Size.Y;

            Center = new OffsetVector(w / 2 + offX, h / 2 + offY);

            var otherOffX = Size.X - offX - w;
            var otherOffY = Size.Y - offY - h;

            target = new SDL_Rect
            {
                x = target.x + offX,
                y = target.y + otherOffY,
                w = (int)(dest.Width / (double)Size.X * w),
                h = (int)(dest.Height / (double)Size.Y * h)
            };

            if (_frame.Rotated)
            {
                angle += 90;
            }

            // Center = new OffsetVector(w / 2, h / 2);

            src = Marshal.AllocHGlobal(Marshal.SizeOf(srcRect));
            Marshal.StructureToPtr(srcRect, src, false);
        }

        // var debugRect = dest.ToSdl();
        // SDL_SetRenderDrawColor(context.Renderer, 255, 0, 0, 255);
        // SDL_RenderDrawRect(context.Renderer, ref debugRect);
        //
        // SDL_RenderDrawRect(context.Renderer, ref target);

        // var centerSdl = Center.ToSdl();

        SDL_SetTextureColorMod(Texture, color.R, color.G, color.B);
        SDL_SetTextureAlphaMod(Texture, alpha);

        SDL_RenderCopyEx(context.Renderer, Texture, src, ref target, angle, IntPtr.Zero, 
            SDL_RendererFlip.SDL_FLIP_NONE);

        if (src != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(src);
        }
    }
}