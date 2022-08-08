using System;
using System.Drawing;
using System.Numerics;
using System.Runtime.InteropServices;
using Myra.Graphics2D;
using Myra.Platform;
using Scorpia.Engine.Graphics;
using static SDL2.SDL;
using RenderContext = Scorpia.Engine.Graphics.RenderContext;

namespace Scorpia.Engine.Myra;

internal class SMRenderer : IMyraRenderer
{
    private readonly GraphicsManager _graphicsManager;
    private readonly RenderContext _renderContext;

    public SMRenderer(GraphicsManager graphicsManager)
    {
        _graphicsManager = graphicsManager;
        _renderContext = graphicsManager.CreateContext();
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
            Marshal.StructureToPtr(sdlRect, srcRect, false);
        }

        SDL_RenderCopyEx(_graphicsManager.Renderer, (IntPtr) texture, srcRect, IntPtr.Zero, rotation, IntPtr.Zero,
            SDL_RendererFlip.SDL_FLIP_NONE);
    }

    public Rectangle Scissor { get; set; }
}