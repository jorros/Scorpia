using System.Drawing;
using Scorpia.Engine.Asset;

namespace Scorpia.Engine.UI.Style;

public record WindowStyle
{
    public int Width { get; set; }
    
    public int Height { get; set; }
    
    public Sprite Background { get; set; }
    
    public Sprite ActionBarBackground { get; set; }
    
    public Rectangle ActionBarPadding { get; set; }
    
    public int ActionBarSpaceBetween { get; set; }
    
    public int ActionBarHeight { get; set; }
    
    public int ActionBarMinWidth { get; set; }
    
    public Point Padding { get; set; }
    
    public Point ActionBarMargin { get; set; }
}