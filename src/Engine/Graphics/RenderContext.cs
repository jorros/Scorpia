using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Asset.Font;
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

    public OffsetVector GetDrawSize()
    {
        SDL_GetRendererOutputSize(_graphicsManager.Renderer, out var w, out var h);

        return new OffsetVector(w, h);
    }

    public void Draw(Sprite sprite, OffsetVector position)
    {
        Viewport.Draw(sprite, position);
    }

    public void Draw(Sprite sprite, Rectangle target)
    {
        Viewport.Draw(sprite, target, 0, Color.White, 255);
    }
    
    public void DrawText(Font font, OffsetVector position, string text, FontSettings settings)
    {
        Viewport.DrawText(font, position, text, settings);
    }
}