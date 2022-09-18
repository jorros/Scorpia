using System;
using System.Drawing;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class ProgressBar : UIElement
{
    public string Type { get; set; }
    
    public byte Progress { get; set; }

    public void SetWidth(int width)
    {
        Width = width;
    }
    
    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        var style = stylesheet.GetProgressBar(Type);

        if (Height == 0)
        {
            Height = style.Height;
        }

        if (!Show)
        {
            return;
        }

        var width = stylesheet.Scale(Width);
        var height = stylesheet.Scale(Height);

        var position = stylesheet.Scale(GetPosition());

        var target = new Rectangle(position.X, position.Y, width, height);

        renderContext.Draw(style.Background, target, 0, Color.White, 255, inWorld);
        
        target = new Rectangle(position.X, position.Y, (int)Math.Round(width / 100.0 * Progress), height);
        var src = target with {X = 0, Y = 0};
        
        renderContext.Draw(style.Fill, src, target, 0, Color.White, 255, inWorld);
    }
}