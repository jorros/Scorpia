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

    internal TextureSprite(IntPtr texture, int width, int height) : base(texture, new Size(width, height))
    {
    }

    internal TextureSprite(IntPtr texture, SpritesheetFrame frame) : this(texture, frame.OriginalSize.X,
        frame.OriginalSize.Y)
    {
        _frame = frame;
    }

    internal override void Render(GraphicsManager context, Rectangle? src, RectangleF dest, double angle, Color color, byte alpha, int index)
    {
        var target = new SDL_FRect
        {
            x = dest.X,
            y = dest.Y,
            h = dest.Height,
            w = dest.Width
        };

        var source = IntPtr.Zero;

        if (_frame is not null)
        {
            var srcRect = new SDL_Rect
            {
                w = _frame.Size.X,
                h = _frame.Size.Y,
                x = _frame.Position.X,
                y = _frame.Position.Y
            };

            if (src is not null)
            {
                var maxW = srcRect.x + srcRect.w;
                var maxH = srcRect.y + srcRect.h;
                var actW = srcRect.x + src.Value.X + src.Value.Width;
                var actH = srcRect.y + src.Value.Y + src.Value.Height;

                srcRect = new SDL_Rect
                {
                    x = srcRect.x + src.Value.X,
                    y = srcRect.y + src.Value.Y,
                    w = actW > maxW ? maxW - (srcRect.x + src.Value.X) : src.Value.Width,
                    h = actH > maxH ? maxH - (srcRect.y + src.Value.Y) : src.Value.Height
                };
            }

            var offX = _frame.Offset.X;
            var offY = _frame.Offset.Y;

            var w = _frame.Size.X;
            var h = _frame.Size.Y;

            var otherOffX = Size.Width - offX - w;
            var otherOffY = Size.Height - offY - h;

            target = new SDL_FRect
            {
                x = target.x + offX,
                y = target.y + otherOffY,
                w = (int)(dest.Width / (double)Size.Width * w),
                h = (int)(dest.Height / (double)Size.Height * h)
            };
            
            // Center = new OffsetVector(w / 2, h / 2);

            source = Marshal.AllocHGlobal(Marshal.SizeOf(srcRect));
            Marshal.StructureToPtr(srcRect, source, false);
        }
        else if(src is not null)
        {
            var srcRect = src.Value.ToSdl();
            
            source = Marshal.AllocHGlobal(Marshal.SizeOf(srcRect));
            Marshal.StructureToPtr(srcRect, source, false);
        }

        // var debugRect = dest.ToSdl();
        // SDL_SetRenderDrawColor(context.Renderer, 255, 0, 0, 255);
        // SDL_RenderDrawRect(context.Renderer, ref debugRect);
        //
        // SDL_RenderDrawRect(context.Renderer, ref target);

        // var centerSdl = Center.ToSdl();

        SDL_SetTextureColorMod(Texture, color.R, color.G, color.B);
        SDL_SetTextureAlphaMod(Texture, alpha);

        SDL_RenderCopyExF(context.Renderer, Texture, source, ref target, angle, IntPtr.Zero, 
            SDL_RendererFlip.SDL_FLIP_NONE);

        if (source != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(source);
        }
    }
}