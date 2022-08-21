using System.IO;
using SharpCompress.Archives;

namespace Scorpia.Engine.Helper;

internal static class ArchiveExtensions
{
    public static string GetAssetKey(this IArchiveEntry entry)
    {
        var ext = Path.GetExtension(entry.Key);
        
        return entry.Key.Remove(entry.Key.Length - ext.Length);
    }
}