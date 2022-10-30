using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Asset.Font;

namespace Scorpia.Engine.UI.Style;

public record LabelStyle
{
    public Font Font { get; set; }
    
    public int Size { get; set; }

    public Color Color { get; set; }
    
    public FontStyle Style { get; set; }
    
    public Color OutlineColor { get; set; }
    
    public int Outline { get; set; }
    
    public Alignment Alignment { get; set; }
    
    public int? MaxWidth { get; set; }

    public FontSettings ToFontSettings()
    {
        return new FontSettings
        {
            Alignment = Alignment,
            Color = Color,
            Outline = Outline,
            Size = Size,
            Style = Style,
            OutlineColor = OutlineColor
        };
    }
}