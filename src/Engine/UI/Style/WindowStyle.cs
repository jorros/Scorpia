using System.Drawing;
using Scorpia.Engine.Asset;

namespace Scorpia.Engine.UI.Style;

public record WindowStyle
{
    public int Width { get; set; }
    
    public int Height { get; set; }
    
    public Sprite Background { get; set; }

    public Point Padding { get; set; }
    
    public bool HasActionBar { get; set; }
    
    public Rectangle ActionBarPadding { get; set; }
    
    public int ActionBarSpaceBetween { get; set; }
    
    public int ActionBarHeight { get; set; }
    
    public UIAnchor ActionBarAnchor { get; set; }
    
    public bool HasTitle { get; set; }
    
    public Rectangle TitlePadding { get; set; }
    
    public int TitleSpaceBetween { get; set; }
    
    public int TitleHeight { get; set; }
    
    public string TitleLabelStyle { get; set; }
    
    public UIAnchor TitleAnchor { get; set; }
}