using System;
using System.Collections.Generic;
using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Asset.Font;
using static SDL2.SDL;

namespace Scorpia.Engine.Graphics;

public class RenderContext
{
    private readonly GraphicsManager _graphicsManager;
    public Viewport Viewport { get; private set; }
    private ulong _currentTick;
    private readonly Dictionary<Action, ulong> _timedActions;
    public int FPS => _graphicsManager.FPS;

    public RenderContext(GraphicsManager graphicsManager)
    {
        _graphicsManager = graphicsManager;
        _timedActions = new Dictionary<Action, ulong>();
    }

    internal void Init()
    {
        SDL_RenderGetViewport(_graphicsManager.Renderer, out var rect);
        Viewport = new Viewport(_graphicsManager, rect);
    }
    
    internal void Begin(ScaleQuality scaleQuality = ScaleQuality.Nearest)
    {
        SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, $"{(int) scaleQuality}");

        _currentTick = SDL_GetTicks64();
        
        Viewport.Begin();
    }

    internal void End()
    {
        Viewport.End();
    }

    public void InvokeIn(uint ms, Action action)
    {
        if (!_timedActions.ContainsKey(action))
        {
            _timedActions[action] = _currentTick + ms;
            return;
        }
        
        if (_timedActions[action] <= _currentTick)
        {
            action.Invoke();
            _timedActions.Remove(action);
        }
    }

    public void RunEvery(uint ms, Action action)
    {
        if (_currentTick % ms == 0)
        {
            action.Invoke();
        }
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