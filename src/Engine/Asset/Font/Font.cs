using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Scorpia.Engine.Asset.Markup;
using Scorpia.Engine.Graphics;
using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace Scorpia.Engine.Asset.Font;

public class Font : IAsset, IDisposable
{
    private readonly IntPtr _sdlRw;
    private readonly FontMarkupReader _fontMarkupReader;
    private readonly GraphicsManager _graphicsManager;
    private readonly IEnumerable<IFontRenderer> _renderers;
    private readonly Dictionary<CachedTextOptions, IntPtr> _cache;

    internal Font(IntPtr sdlRw, FontMarkupReader fontMarkupReader, GraphicsManager graphicsManager)
    {
        _sdlRw = sdlRw;
        _fontMarkupReader = fontMarkupReader;
        _graphicsManager = graphicsManager;
        _renderers = new []
        {
            new TextBlockRenderer(graphicsManager)
        };
        _cache = new Dictionary<CachedTextOptions, IntPtr>();
    }

    internal IntPtr LoadFont(CachedTextOptions options)
    {
        if (_cache.ContainsKey(options))
        {
            return _cache[options];
        }

        var font = TTF_OpenFontRW(_sdlRw, 0, options.Size);
        TTF_SetFontStyle(font, (int) options.Style);
        TTF_SetFontOutline(font, options.Outline);

        if (font == IntPtr.Zero)
        {
            Console.WriteLine($"Failed to load font: {TTF_GetError()}");
        }

        _cache[options] = font;

        return font;
    }

    internal void Render(OffsetVector position, string text, FontSettings settings)
    {
        if (string.IsNullOrEmpty(text))
        {
            return;
        }
        
        var startBlock = settings.ToTextBlock();

        var blocks = _fontMarkupReader.Read(text, startBlock);
        var cursor = new OffsetVector(position.X, position.Y);

        var toBeRendered = new List<(IntPtr texture, SDL_Rect target)>();

        foreach (var block in blocks)
        {
            var renderer = _renderers.First(x => x.Type == block.GetType());
            toBeRendered.AddRange(renderer.Render(this, block, position, ref cursor));
        }

        var xModifier = settings.Alignment switch
        {
            TextAlign.Center => (cursor.X - position.X) / 2,
            TextAlign.Right => cursor.X - position.X,
            _ => 0
        };

        foreach (var render in toBeRendered)
        {
            var target = render.target with {x = render.target.x - xModifier};
            SDL_RenderCopy(_graphicsManager.Renderer, render.texture, IntPtr.Zero, ref target);
        }
    }

    public int GetHeight(FontSettings settings)
    {
        var font = LoadFont(settings.ToCached());
        return TTF_FontHeight(font);
    }

    public OffsetVector CalculateSize(string text, FontSettings settings)
    {
        if (string.IsNullOrEmpty(text))
        {
            return OffsetVector.Zero;
        }
        
        var startBlock = settings.ToTextBlock();
        var blocks = _fontMarkupReader.Read(text, startBlock);

        var cursor = new OffsetVector();
        var size = new OffsetVector();

        foreach (var block in blocks)
        {
            if (block is NewLineBlock)
            {
                size = new OffsetVector(Math.Max(cursor.X, size.X), size.Y + cursor.Y);
                
                continue;
            }
            
            var renderer = _renderers.First(x => x.Type == block.GetType());
            renderer.CalculateSize(text, this, block, ref cursor);
        }
        
        size = new OffsetVector(Math.Max(cursor.X, size.X), size.Y + cursor.Y);

        return size;
    }

    public (int count, int neededWidth) Measure(string text, int size, int width)
    {
        var options = new CachedTextOptions(size, 0, Color.Empty, FontStyle.Normal);
        var font = LoadFont(options);

        TTF_MeasureUTF8(font, text, width, out var neededWidth, out var count);

        return (count, neededWidth);
    }

    public void Dispose()
    {
        foreach (var renderer in _renderers)
        {
            renderer.Clear();
        }
        
        foreach (var font in _cache.Values)
        {
            TTF_CloseFont(font);
        }

        SDL_RWclose(_sdlRw);

        _cache.Clear();
    }
}