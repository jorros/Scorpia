using System.Drawing;
using Scorpia.Engine.Asset;

namespace Scorpia.Engine.UI.Style;

public record ButtonStyle
{
    public Sprite Button { get; set; }
    
    public LabelStyle LabelStyle { get; set; }

    public int MinWidth { get; set; }
    
    public int MinHeight { get; set; }
    
    public OffsetVector TextPosition { get; set; } = OffsetVector.Zero;
    
    public OffsetVector Padding { get; set; } = OffsetVector.Zero;
    
    public Color? PressedTint { get; set; }
    
    public Color? HoveredTint { get; set; }
}