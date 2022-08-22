using System;
using System.Collections.Generic;
using System.Drawing;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Helper;
using static SDL2.SDL;
using static SDL2.SDL_ttf;

namespace Scorpia.Engine.Asset;

public class Font : IAsset, IDisposable
{
    private readonly IntPtr _sdlRw;
    private readonly Dictionary<CachedTextOptions, IntPtr> _cache;
    private readonly Dictionary<(OffsetVector position, bool isOutline), (string text, IntPtr texture)> _textureCache;

    internal Font(IntPtr sdlRw)
    {
        _sdlRw = sdlRw;
        _cache = new Dictionary<CachedTextOptions, IntPtr>();
        _textureCache = new Dictionary<(OffsetVector position, bool isOutline), (string text, IntPtr texture)>();
    }

    private IntPtr LoadFont(CachedTextOptions options)
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

    internal void Render(GraphicsManager context, OffsetVector position, string text, int size, Color color)
    {
        var options = new CachedTextOptions(size, 0, Color.Empty, FontStyle.Normal);
        var startBlock = options.ToTextBlock() with {Color = color};

        var markup = FontMarkup.Read(text, startBlock);
        var lastX = (int) position.X;

        foreach (var block in markup.TextBlocks)
        {
            var cachedOptions = CachedTextOptions.FromTextBlock(block);

            RenderTexture(cachedOptions.Outline > 0);
            
            if (cachedOptions.Outline > 0)
            {
                cachedOptions = cachedOptions with {Outline = 0};
                RenderTexture(false);
            }

            void RenderTexture(bool isOutline)
            {
                var font = LoadFont(cachedOptions);

                var (texture, surface) = GenerateTexture(context, font, block, position, isOutline);

                SDL_QueryTexture(texture, out _, out _, out var w, out var h);

                var target = new SDL_Rect
                {
                    x = lastX,
                    y = (int) position.Y,
                    w = w,
                    h = h
                };

                SDL_RenderCopy(context.Renderer, texture, IntPtr.Zero, ref target);

                if (!isOutline)
                {
                    lastX += w;
                }

                if (surface != IntPtr.Zero)
                {
                    SDL_FreeSurface(surface);
                }
            }
        }
    }

    private (IntPtr texture, IntPtr surface) GenerateTexture(GraphicsManager context, IntPtr font,
        FontMarkup.TextBlock text, OffsetVector position, bool isOutline)
    {
        var texture = IntPtr.Zero;
        var surface = IntPtr.Zero;

        if (_textureCache.ContainsKey((position, isOutline)))
        {
            var cached = _textureCache[(position, isOutline)];
            if (cached.text == text.Text)
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
            surface = TTF_RenderUTF8_Blended(font, text.Text,
                isOutline ? text.OutlineColor.ToSdl() : text.Color.ToSdl());

            texture = SDL_CreateTextureFromSurface(context.Renderer, surface);

            _textureCache[(position, isOutline)] = (text.Text, texture);
        }

        return (texture, surface);
    }

    public OffsetVector CalculateSize(string text, int size)
    {
        var options = new CachedTextOptions(size, 0, Color.Empty, FontStyle.Normal);
        var font = LoadFont(options);

        TTF_SizeUTF8(font, text, out var w, out var h);

        return new OffsetVector(w, h);
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
        foreach (var font in _cache.Values)
        {
            TTF_CloseFont(font);
        }

        SDL_FreeRW(_sdlRw);

        _cache.Clear();
    }

    private record CachedTextOptions(int Size, int Outline, Color OutlineColor, FontStyle Style)
    {
        public FontMarkup.TextBlock ToTextBlock()
        {
            return new FontMarkup.TextBlock
            {
                Size = Size,
                Outline = Outline,
                Style = Style
            };
        }

        public static CachedTextOptions FromTextBlock(FontMarkup.TextBlock block)
        {
            return new CachedTextOptions(block.Size, block.Outline, block.OutlineColor, block.Style);
        }
    };
}