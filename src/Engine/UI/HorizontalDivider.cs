using System;
using System.Drawing;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class HorizontalDivider : UIElement
{
    public string Type { get; set; }

    public new int Width
    {
        get => base.Width;
        set => base.Width = value;
    }
    
    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        var style = stylesheet.GetHorizontalDivider(Type);
        var position = GetPosition();

        Height = style.Height;
        
        if (!Show)
        {
            return;
        }

        var width = Math.Max(Width, style.MinWidth);
        var target = new Rectangle(stylesheet.Scale(position.X), stylesheet.Scale(position.Y), stylesheet.Scale(width),
            stylesheet.Scale(Height));
        
        renderContext.Viewport.Draw(style.Background, target, 0, Color.White, 255, inWorld);
    }
}