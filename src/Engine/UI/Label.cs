using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Asset.Font;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class Label : UIElement
{
    public string Type { get; set; }
    public string Font { get; set; }
    public string Text { get; set; } = string.Empty;
    public int? Size { get; set; }
    public Color? Color { get; set; }
    public TextAlign? TextAlign { get; set; }
    public FontStyle? Style { get; set; }
    public int? Outline { get; set; }
    public Color? OutlineColor { get; set; }

    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        var style = stylesheet.GetLabel(Type);
        
        var font = Font is null ? style.Font : stylesheet.GetFont(Font);

        var fontSettings = new FontSettings
        {
            Alignment = TextAlign ?? style.TextAlign,
            Color = Color ?? style.Color,
            Size = stylesheet.Scale(Size ?? style.Size),
            Style = Style ?? style.Style,
            Outline = Outline ?? style.Outline,
            OutlineColor = OutlineColor ?? style.OutlineColor
        };

        var size = font.CalculateSize(Text, fontSettings);
        Width = size.Width;
        Height = size.Height;
        
        if (!Show)
        {
            return;
        }
        
        renderContext.DrawText(font, stylesheet.Scale(GetPosition()), Text, fontSettings, inWorld);
    }
}