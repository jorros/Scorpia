using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using Scorpia.Engine.Asset.SpriteSheetParsers;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Helper;
using SharpCompress.Archives;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace Scorpia.Engine.Asset.AssetLoaders;

internal class SpriteLoader : IAssetLoader
{
    private readonly GraphicsManager _graphicsManager;
    private readonly LibgdxSpriteSheetParser _libgdxSpriteSheetParser;

    public SpriteLoader(GraphicsManager graphicsManager, LibgdxSpriteSheetParser libgdxSpriteSheetParser)
    {
        _graphicsManager = graphicsManager;
        _libgdxSpriteSheetParser = libgdxSpriteSheetParser;
    }

    public IEnumerable<string> Extensions => new[] {".png"};

    public IReadOnlyList<(string key, IAsset asset)> Load(IArchiveEntry entry, IArchive archive)
    {
        var sprites = new List<(string key, IAsset asset)>();
        var key = entry.GetAssetKey();

        var data = GetData(entry);
        var texture = LoadTexture(data, "png");

        SDL_QueryTexture(texture, out _, out _, out var width, out var height);

        var metaEntry = archive.Entries.FirstOrDefault(x =>
            x.Key.Equals($"{key}.txt", StringComparison.InvariantCultureIgnoreCase));

        key = key.EndsWith("@2x") ? key.Remove(key.Length - 3, 3) : key;

        if (metaEntry is null)
        {
            sprites.Add((key, new TextureSprite(texture, width, height)));

            return sprites;
        }

        using var spriteMeta = metaEntry.OpenEntryStream();

        var descriptor = _libgdxSpriteSheetParser.Read(spriteMeta);

        if (descriptor == null)
        {
            sprites.Add((key, new TextureSprite(texture, width, height)));

            return sprites;
        }

        foreach (var frame in descriptor.Frames)
        {
            Sprite sprite;
            if (frame.Split is not null)
            {
                sprite = new NinePatchSprite(texture, frame);
            }
            else
            {
                sprite = new TextureSprite(texture, frame);
            }

            var path = Path.GetDirectoryName(key);
            var frameKey = Path.Combine(path!, frame.Name);

            sprites.Add((frameKey, sprite));
        }

        return sprites;
    }
    
    private IntPtr LoadTexture(byte[] data, string format)
    {
        unsafe
        {
            fixed (byte* p = data)
            {
                var ptr = (IntPtr)p;
                var size = Marshal.SizeOf(typeof(byte)) * data.Length;
                
                var rw = SDL_RWFromMem(ptr, size);
                var surface = IMG_LoadTyped_RW(rw, 1, format);
                var texture = SDL_CreateTextureFromSurface(_graphicsManager.Renderer, surface);
                SDL_FreeSurface(surface);
                
                return texture;
            }
        }
    }

    private static byte[] GetData(IArchiveEntry entry)
    {
        using var entryStream = entry.OpenEntryStream();
        using var memory = new MemoryStream();
        entryStream.CopyTo(memory);
        var data = memory.ToArray();

        return data;
    }
}