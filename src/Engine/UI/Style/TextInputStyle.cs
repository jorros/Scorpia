using Scorpia.Engine.Asset;

namespace Scorpia.Engine.UI.Style;

public record TextInputStyle
{
    public LabelStyle Text { get; set; }
    
    public Sprite Background { get; set; }
    
    public OffsetVector Padding { get; set; }
    
    public int Width { get; set; }
    
    public int Height { get; set; }
}