using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Extensions.Logging;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Asset.Font;
using Scorpia.Engine.Helper;
using Scorpia.Engine.Maths;
using static SDL2.SDL;

namespace Scorpia.Engine.Graphics;

public class RenderContext
{
    private readonly GraphicsManager _graphicsManager;
    private readonly ILogger<RenderContext> _logger;
    public Camera Camera { get; private set; }
    private ulong _currentTick;
    private readonly Dictionary<Action, ulong> _timedActions;
    public int FPS => _graphicsManager.FPS;

    public RenderContext(GraphicsManager graphicsManager, ILogger<RenderContext> logger)
    {
        _graphicsManager = graphicsManager;
        _logger = logger;
        _timedActions = new Dictionary<Action, ulong>();
    }

    internal void Init()
    {
        SDL_RenderGetViewport(_graphicsManager.Renderer, out var rect);
        Camera = new Camera(this, rect);
    }

    internal void Begin(Color clearColor, ScaleQuality scaleQuality = ScaleQuality.Nearest)
    {
        ErrorHandling.Handle(_logger, SDL_SetRenderDrawColor(_graphicsManager.Renderer, clearColor.R, clearColor.G, clearColor.B, 255));
        ErrorHandling.Handle(_logger, SDL_RenderClear(_graphicsManager.Renderer));
        
        SDL_SetHint(SDL_HINT_RENDER_SCALE_QUALITY, $"{(int) scaleQuality}");

        _currentTick = SDL_GetTicks64();
    }

    internal void End()
    {
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

    public Size GetDrawSize()
    {
        SDL_GetRendererOutputSize(_graphicsManager.Renderer, out var w, out var h);

        return new Size(w, h);
    }

    public void SetClipping(Rectangle? rectangle)
    {
        if (rectangle is null)
        {
            SDL_RenderSetClipRect(_graphicsManager.Renderer, IntPtr.Zero);

            return;
        }

        var rect = new SDL_Rect
        {
            x = rectangle.Value.Left,
            y = rectangle.Value.Top,
            h = rectangle.Value.Height,
            w = rectangle.Value.Width
        };

        SDL_RenderGetViewport(_graphicsManager.Renderer, out var viewport);

        if (viewport.x == rect.x && viewport.y == rect.y && viewport.w == rect.w && viewport.h == rect.h)
        {
            SDL_RenderSetClipRect(_graphicsManager.Renderer, IntPtr.Zero);

            return;
        }

        SDL_RenderSetClipRect(_graphicsManager.Renderer, ref rect);
    }

    public Rectangle GetClipping()
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
    
    public void Draw(Sprite sprite, PointF position)
    {
        var dest = new RectangleF(position.X, position.Y, sprite.Size.Width, sprite.Size.Height);
        Draw(sprite, null, dest, 0, Color.White, 255);
    }

    public void Draw(Sprite sprite, RectangleF dest, double angle, Color color, byte alpha, bool inWorld = true)
    {
        Draw(sprite, null, dest, angle, color, alpha, inWorld);
    }

    public void Draw(Sprite sprite, Rectangle? src, RectangleF dest, double angle, Color color, byte alpha,
        bool inWorld = true)
    {
        if (inWorld)
        {
            var position = Camera.WorldToScreen(dest.Location.ToVector2());
            var trans = Camera.WorldToScreen(dest.Size.ToVector2() + dest.Location.ToVector2());
            
            var size = trans - position;
            
            dest = new RectangleF(position.ToPointF(), size.ToSize());
        }

        sprite.Render(_graphicsManager, src, dest, angle, color, alpha);
    }

    public void DrawText(Font font, PointF position, string text, FontSettings settings, bool inWorld = true)
    {
        if (inWorld)
        {
            position = Camera.WorldToScreen(position.ToVector2()).ToPointF();
        }

        font.Render(new Point((int)position.X, (int)position.Y), text, settings);
    }

    public void DrawLine(Point from, Point to, Color color)
    {
        SDL_SetRenderDrawColor(_graphicsManager.Renderer, color.R, color.G, color.B, color.A);
        SDL_RenderDrawLine(_graphicsManager.Renderer, from.X, from.Y, to.X, to.Y);
    }
}