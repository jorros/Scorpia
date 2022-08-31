using System.Drawing;
using Scorpia.Engine.Asset.Markup;

namespace Scorpia.Engine.Asset.Font;

public readonly struct FontSettings
{
    public int Size { get; init; }

    public Color Color { get; init; }

    public FontStyle Style { get; init; }

    public TextAlign Alignment { get; init; }

    public int Outline { get; init; }
    
    public Color OutlineColor { get; init; }

    public CachedTextOptions ToCached()
    {
        return new CachedTextOptions(Size, Outline, OutlineColor, Style);
    }

    public TextBlock ToTextBlock()
    {
        return new TextBlock
        {
            Color = Color,
            Outline = Outline,
            Size = Size,
            Style = Style,
            OutlineColor = OutlineColor
        };
    }
}