using System.IO;
using System.Linq;
using Myra.Assets;
using SharpCompress.Archives;

namespace Scorpia.Engine.MyraIntegration;

public class ArchiveAssetResolver : IAssetResolver
{
    private readonly IArchive _archive;

    public ArchiveAssetResolver(IArchive archive)
    {
        _archive = archive;
    }
    
    public Stream Open(string assetName)
    {
        var entry = _archive.Entries.FirstOrDefault(x => x.Key == assetName);

        if (entry == null)
        {
            throw new EngineException("[Myra] Could not find file '" + assetName + "'");
        }

        return entry.OpenEntryStream();
    }
}