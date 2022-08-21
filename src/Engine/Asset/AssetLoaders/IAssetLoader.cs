using System.Collections.Generic;
using SharpCompress.Archives;

namespace Scorpia.Engine.Asset.AssetLoaders;

internal interface IAssetLoader
{
    IEnumerable<string> Extensions { get; }

    IReadOnlyList<(string key, IAsset asset)> Load(IArchiveEntry entry, IArchive archive);
}