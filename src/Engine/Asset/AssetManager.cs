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
    private Dictionary<string, AssetBundle> _assetBundles;

    internal void SetGraphicsManager(GraphicsManager graphicsManager, IEnumerable<IAssetLoader> assetLoaders)
    {
        _graphicsManager = graphicsManager;
        _assetLoaders = assetLoaders;
        _assetBundles = new Dictionary<string, AssetBundle>();
    }

    public AssetBundle Load(string name)
    {
        var bundle = new Dictionary<string, IAsset>();

        using var stream = File.Open(Path.Combine("Content", $"{name}.pack"), FileMode.Open);
        using var archive = ArchiveFactory.Open(stream);

        var allowedExtensions = GetAllowedExtensions();

        foreach (var entry in archive.Entries.Where(entry =>
                     !entry.IsDirectory && allowedExtensions.Contains(Path.GetExtension(entry.Key).ToLowerInvariant())))
        {
            var ext = Path.GetExtension(entry.Key).ToLowerInvariant();
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

        var assetBundle = new AssetBundle(bundle, _graphicsManager);
        _assetBundles[name] = assetBundle;

        return assetBundle;
    }

    private IEnumerable<string> GetAllowedExtensions()
    {
        return _assetLoaders.SelectMany(x => x.Extensions);
    }
    
    public T Get<T>(string name) where T : class, IAsset
    {
        var split = name.Split(':');

        if (!_assetBundles.ContainsKey(split[0]))
        {
            throw new EngineException($"Tried to access not loaded asset bundle {name}.");
        }

        var assetBundle = _assetBundles[split[0]];

        return assetBundle.Get<T>(split[1]);
    }

    public static void Pack(string src, string output)
    {
        using var archive = ArchiveFactory.Create(ArchiveType.Zip);

        archive.AddAllFromDirectory(src);
        archive.SaveTo(output, new TarWriterOptions(CompressionType.Deflate, true));
    }
}