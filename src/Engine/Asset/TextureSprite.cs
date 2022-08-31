using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Scorpia.Engine.Graphics;
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

            target = new SDL_Rect
            {
                x = target.x + (_frame.Rotated ? _frame.Offset.Y : _frame.Offset.X),
                y = target.y + (_frame.Rotated ? _frame.Offset.X : _frame.Offset.Y),
                w = _frame.Rotated ? _frame.Size.Y : _frame.Size.X,
                h = _frame.Rotated ? _frame.Size.X : _frame.Size.Y
            };

            if (_frame.Rotated)
            {
                angle += 90;
            }

            src = Marshal.AllocHGlobal(Marshal.SizeOf(srcRect));
            Marshal.StructureToPtr(srcRect, src, false);
        }

        var centerSdl = Center.ToSdl();

        SDL_SetTextureColorMod(Texture, color.R, color.G, color.B);
        SDL_SetTextureAlphaMod(Texture, alpha);

        SDL_RenderCopyEx(context.Renderer, Texture, src, ref target, angle, ref centerSdl,
            SDL_RendererFlip.SDL_FLIP_NONE);

        if (src != IntPtr.Zero)
        {
            Marshal.FreeHGlobal(src);
        }
    }
}