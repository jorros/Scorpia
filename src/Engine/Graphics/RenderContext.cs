using System;
using System.Runtime.InteropServices;
using Scorpia.Engine.Asset;
using static SDL2.SDL;

namespace Scorpia.Engine.Graphics;

public class RenderContext
{
    private readonly GraphicsManager _graphicsManager;

    internal RenderContext(GraphicsManager graphicsManager)
    {
        _graphicsManager = graphicsManager;
    }

    public TimeSpan ElapsedTime { get; set; }

    internal void Begin(ScaleQuality scaleQuality = ScaleQuality.Nearest)
    {
        SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, $"{(int) scaleQuality}");
    }

    internal void End()
    {
    }

    public void Draw(Sprite sprite, OffsetVector position)
    {
        Draw(sprite, position, 0, new OffsetVector(0, 0));
    }

    public void Draw(Sprite sprite, OffsetVector position, double angle, OffsetVector center)
    {
        var target = new SDL_Rect
        {
            x = position.X,
            y = position.Y,
            h = sprite.Height,
            w = sprite.Width
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

        var centerSdl = center.ToSdl();

        SDL_RenderCopyEx(_graphicsManager.Renderer, sprite.Texture, src, ref target, angle, ref centerSdl,
            SDL_RendererFlip.SDL_FLIP_NONE);
    }
}