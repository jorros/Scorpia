using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using Myra.Graphics2D;
using Myra.Platform;
using Scorpia.Engine.Graphics;
using static SDL2.SDL;
using Color = System.Drawing.Color;
using RenderContext = Scorpia.Engine.Graphics.RenderContext;

namespace Scorpia.Engine.MyraIntegration;

internal class SMRenderer : IMyraRenderer
{
    private readonly GraphicsManager _graphicsManager;
    private readonly RenderContext _renderContext;

    public SMRenderer(GraphicsManager graphicsManager, RenderContext renderContext)
    {
        _graphicsManager = graphicsManager;
        _renderContext = renderContext;
    }
    
    public void Begin(TextureFiltering textureFiltering)
    {
        _renderContext.Begin(textureFiltering == TextureFiltering.Nearest ? ScaleQuality.Nearest : ScaleQuality.Linear);
    }

    public void End()
    {
        _renderContext.End();
    }

    public void Draw(object texture, Vector2 pos, Rectangle? src, Color color, float rotation, Vector2 scale, float depth)
    {
        var srcRect = IntPtr.Zero;
        
        if (src is not null)
        {
            var sdlRect = src.Value.ToSdl();
            srcRect = Marshal.AllocHGlobal(Marshal.SizeOf(sdlRect));
            Marshal.StructureToPtr(sdlRect, srcRect, false);
        }

        int w;
        int h;

        if (src is null)
        {
            SDL_QueryTexture((IntPtr) texture, out _, out _, out w, out h);
        }
        else
        {
            w = src.Value.Width;
            h = src.Value.Height;
        }

        var sDstRect = new SDL_Rect
        {
            x = (int) pos.X,
            y = (int) pos.Y,
            w = (int)(w * scale.X),
            h = (int)(h * scale.Y)
        };
        var dstRect = Marshal.AllocHGlobal(Marshal.SizeOf(sDstRect));
        Marshal.StructureToPtr(sDstRect, dstRect, false);

        SDL_SetTextureColorMod((IntPtr) texture, color.R, color.G, color.B);
        SDL_RenderCopyEx(_graphicsManager.Renderer, (IntPtr) texture, srcRect, dstRect, rotation, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
        
        Marshal.FreeHGlobal(srcRect);
        Marshal.FreeHGlobal(dstRect);
    }

    public Rectangle Scissor
    {
        get
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

        set
        {
            var rect = new SDL_Rect
            {
                x = value.X,
                y = value.Y,
                h = value.Height,
                w = value.Width
            };

            SDL_RenderSetClipRect(_graphicsManager.Renderer, ref rect);
        }
    }
}