using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Scorpia.Engine.Graphics;
using static SDL2.SDL;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Writers.Tar;

namespace Scorpia.Engine.Asset;

public class AssetManager
{
    private GraphicsManager _graphicsManager;

    internal void SetGraphicsManager(GraphicsManager graphicsManager)
    {
        _graphicsManager = graphicsManager;
    }

    private readonly IReadOnlyList<string> _validExtensions = new[]
    {
        ".png"
    };

    public AssetBundle Load(string name)
    {
        var bundle = new Dictionary<string, IAsset>();

        using var stream = File.Open(Path.Combine("Content", $"{name}.pack"), FileMode.Open);
        using var archive = ArchiveFactory.Open(stream);

        foreach (var entry in archive.Entries.Where(entry =>
                     !entry.IsDirectory && _validExtensions.Any(x => x == Path.GetExtension(entry.Key))))
        {
            var data = GetData(entry);

            var ext = Path.GetExtension(entry.Key);
            var key = GetKey(entry.Key);

            var metaEntry = archive.Entries.FirstOrDefault(x =>
                x.Key.Equals($"{key}.json", StringComparison.InvariantCultureIgnoreCase));
            byte[] metaData = null;
            if (metaEntry is not null)
            {
                metaData = GetData(metaEntry);
            }

            var assets = ext switch
            {
                ".png" => LoadSprite(data, key, metaData),
                _ => null
            };

            if (assets is null || !assets.Any())
            {
                continue;
            }

            foreach (var asset in assets)
            {
                bundle.Add(asset.key, asset.asset);
            }
        }

        return new AssetBundle(bundle, _graphicsManager);
    }

    private static string GetKey(string file)
    {
        var ext = Path.GetExtension(file);
        
        return file.Remove(file.Length - ext.Length);
    }

    private static byte[] GetData(IArchiveEntry entry)
    {
        using var entryStream = entry.OpenEntryStream();
        using var memory = new MemoryStream();
        entryStream.CopyTo(memory);
        var data = memory.ToArray();

        return data;
    }

    public static void Pack(string src, string output)
    {
        using var archive = ArchiveFactory.Create(ArchiveType.Zip);

        archive.AddAllFromDirectory(src);
        archive.SaveTo(output, new TarWriterOptions(CompressionType.Deflate, true));
    }

    private IReadOnlyList<(string key, IAsset asset)> LoadSprite(byte[] data, string key, byte[] meta)
    {
        var texture = _graphicsManager.LoadTexture(data, "png");

        SDL_QueryTexture(texture, out _, out _, out var width, out var height);

        var sprites = new List<(string key, IAsset asset)>();

        if (meta is null)
        {
            sprites.Add((key, new Sprite(texture, width, height)));

            return sprites;
        }

        var descriptor = JsonSerializer.Deserialize<SpritesheetDescriptor>(meta, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (descriptor == null)
        {
            sprites.Add((key, new Sprite(texture, width, height)));

            return sprites;
        }

        foreach (var frame in descriptor.Frames)
        {
            var sprite = new Sprite(texture, frame.Frame.X, frame.Frame.Y, frame.Frame.W, frame.Frame.H);

            var path = Path.GetDirectoryName(key);
            var frameKey = Path.Combine(path!, GetKey(frame.Filename));
            
            sprites.Add((frameKey, sprite));
        }

        return sprites;
    }
}