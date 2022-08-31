using System.Drawing;
using Scorpia.Engine.Asset.Markup;

namespace Scorpia.Engine.Asset.Font;

public record CachedTextOptions(int Size, int Outline, Color OutlineColor, FontStyle Style)
{
    public static CachedTextOptions FromTextBlock(TextBlock block)
    {
        return new CachedTextOptions(block.Size, block.Outline, block.OutlineColor, block.Style);
    }
};