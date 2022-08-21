using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using Scorpia.Engine.Helper;
using SharpCompress.Archives;
using static SDL2.SDL;

namespace Scorpia.Engine.Asset.AssetLoaders;

public class FontLoader : IAssetLoader
{
    public IEnumerable<string> Extensions => new[] {".otf", ".ttf"};
    public IReadOnlyList<(string key, IAsset asset)> Load(IArchiveEntry entry, IArchive archive)
    {
        using var entryStream = entry.OpenEntryStream();
        using var memory = new MemoryStream();
        entryStream.CopyTo(memory);
        var data = memory.ToArray();
        
        var size = Marshal.SizeOf(typeof(byte)) * data.Length;
        var pnt = Marshal.AllocHGlobal(size);
        Marshal.Copy(data, 0, pnt, size);

        var rw = SDL_RWFromMem(pnt, size);

        var fonts = new (string key, IAsset asset)[]
        {
            (entry.GetAssetKey(), new Font(rw))
        };

        return fonts;
    }
}