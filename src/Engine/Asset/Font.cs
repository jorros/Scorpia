using System;
using System.Collections.Generic;
using System.Drawing;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Helper;
using SDL2;
using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace Scorpia.Engine.Asset;

public class Font : IAsset, IDisposable
{
    private readonly IntPtr _sdlRw;
    private readonly Dictionary<int, IntPtr> _cache;
    private readonly Dictionary<OffsetVector, (string text, IntPtr texture)> _textureCache;

    internal Font(IntPtr sdlRw)
    {
        _sdlRw = sdlRw;
        _cache = new Dictionary<int, IntPtr>();
        _textureCache = new Dictionary<OffsetVector, (string text, IntPtr texture)>();
    }

    public void PreLoad(int size)
    {
        if (_cache.ContainsKey(size))
        {
            return;
        }
        
        var font = TTF_OpenFontRW(_sdlRw, 0, size);

        if (font == IntPtr.Zero)
        {
            Console.WriteLine($"Failed to load font: {TTF_GetError()}");
        }
        
        _cache[size] = font;
    }

    internal void Render(GraphicsManager context, OffsetVector position, string text, int size, Color color)
    {
        PreLoad(size);

        var texture = IntPtr.Zero;
        var surface = IntPtr.Zero;

        if (_textureCache.ContainsKey(position))
        {
            var cached = _textureCache[position];
            if (cached.text == text)
            {
                texture = cached.texture;
            }
            else
            {
                SDL_DestroyTexture(cached.texture);
            }
        }

        if (texture == IntPtr.Zero)
        {
            surface = TTF_RenderText_Blended(_cache[size], text, color.ToSdl());

            texture = SDL_CreateTextureFromSurface(context.Renderer, surface);

            _textureCache[position] = (text, texture);
        }

        SDL_QueryTexture(texture, out _, out _, out var w, out var h);

        var target = new SDL_Rect
        {
            x = (int)position.X,
            y = (int)position.Y,
            w = w,
            h = h
        };
        
        SDL_RenderCopy(context.Renderer, texture, IntPtr.Zero, ref target);

        if (surface != IntPtr.Zero)
        {
            SDL_FreeSurface(surface);
        }
    }

    public OffsetVector CalculateSize(string text, int size)
    {
        PreLoad(size);

        TTF_SizeText(_cache[size], text, out var w, out var h);

        return new OffsetVector(w, h);
    }

    public (int count, int neededWidth) Measure(string text, int size, int width)
    {
        PreLoad(size);

        TTF_MeasureText(_cache[size], text, width, out var neededWidth, out var count);

        return (count, neededWidth);
    }

    public void Dispose()
    {
        foreach (var font in _cache.Values)
        {
            TTF_CloseFont(font);
        }
        
        SDL_FreeRW(_sdlRw);

        _cache.Clear();
    }
}