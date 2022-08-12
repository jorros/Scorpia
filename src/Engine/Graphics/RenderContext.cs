using System.Drawing;
using Scorpia.Engine.Asset;
using static SDL2.SDL;

namespace Scorpia.Engine.Graphics;

public class RenderContext
{
    private readonly GraphicsManager _graphicsManager;
    public Viewport Viewport { get; private set; }

    public RenderContext(GraphicsManager graphicsManager)
    {
        _graphicsManager = graphicsManager;
    }

    internal void Init()
    {
        SDL_RenderGetViewport(_graphicsManager.Renderer, out var rect);
        Viewport = new Viewport(_graphicsManager, rect);
    }
    
    internal void Begin(ScaleQuality scaleQuality = ScaleQuality.Nearest)
    {
        SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, $"{(int) scaleQuality}");
        
        Viewport.Begin();
    }

    internal void End()
    {
        Viewport.End();
    }

    public void Draw(Sprite sprite, OffsetVector position)
    {
        Viewport.Draw(sprite, position);
    }

    public void Draw(Sprite sprite, OffsetVector position, OffsetVector scale, OffsetVector? center)
    {
        Viewport.Draw(sprite, position, 0, scale, center, Color.White, 255);
    }

    public void Draw(Sprite sprite, OffsetVector position, double angle, OffsetVector scale, OffsetVector? center, Color color, byte alpha)
    {
        Viewport.Draw(sprite, position, angle, scale, center, color, alpha);
    }
}