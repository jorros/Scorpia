using System.Collections.Generic;
using System.IO;
using System.Linq;
using Scorpia.Engine.Asset.AssetLoaders;
using Scorpia.Engine.Graphics;
using SharpCompress.Archives;
using SharpCompress.Common;
using SharpCompress.Writers.Tar;

namespace Scorpia.Engine.Asset;

public class AssetManager
{
    private GraphicsManager _graphicsManager;
    private IEnumerable<IAssetLoader> _assetLoaders;

    internal void SetGraphicsManager(GraphicsManager graphicsManager, IEnumerable<IAssetLoader> assetLoaders)
    {
        _graphicsManager = graphicsManager;
        _assetLoaders = assetLoaders;
    }

    public AssetBundle Load(string name)
    {
        var bundle = new Dictionary<string, IAsset>();

        using var stream = File.Open(Path.Combine("Content", $"{name}.pack"), FileMode.Open);
        using var archive = ArchiveFactory.Open(stream);

        var allowedExtensions = GetAllowedExtensions();

        foreach (var entry in archive.Entries.Where(entry =>
                     !entry.IsDirectory && allowedExtensions.Contains(Path.GetExtension(entry.Key))))
        {
            var ext = Path.GetExtension(entry.Key);
            var loader = _assetLoaders.First(x => x.Extensions.Contains(ext));

            var assets = loader.Load(entry, archive);
            
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

    private IEnumerable<string> GetAllowedExtensions()
    {
        return _assetLoaders.SelectMany(x => x.Extensions);
    }

    public static void Pack(string src, string output)
    {
        using var archive = ArchiveFactory.Create(ArchiveType.Zip);

        archive.AddAllFromDirectory(src);
        archive.SaveTo(output, new TarWriterOptions(CompressionType.Deflate, true));
    }
}