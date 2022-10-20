using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Maths;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class VerticalGridLayout : UIElement
{
    public List<UIElement> Elements { get; } = new();
    public Sprite Background { get; set; }
    
    public int MinHeight { get; set; }
    public Rectangle Padding { get; set; }
    public int SpaceBetween { get; set; }
    public Point Margin { get; set; } = Point.Empty;
    
    public void Attach(UIElement element)
    {
        element.Parent = this;
        Elements.Add(element);
    }
    
    public void Remove(Func<UIElement, bool> predicate)
    {
        var element = Elements.FirstOrDefault(predicate);
        if (element is not null)
        {
            Elements.Remove(element);
        }
    }
    
    public void SetWidth(int width)
    {
        Width = width;
    }

    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        Height = Padding.Y + Padding.Height + Elements.Sum(element => element.Height);
        Height = Height > MinHeight ? Height : MinHeight;
        
        if (Background is not null && Show)
        {
            var scaledPos = stylesheet.Scale(GetPosition()).Add(stylesheet.Scale(Margin));
            var rect = new Rectangle(scaledPos.X, scaledPos.Y, stylesheet.Scale(Width), stylesheet.Scale(Height));
            renderContext.Draw(Background, rect, 0, Color.White, 255, -1, inWorld);
        }
        
        var currentPos = new Point(Padding.X, Padding.Y).Add(Margin);

        foreach (var element in Elements)
        {
            element.Position = currentPos;

            element.Render(renderContext, stylesheet, inWorld);

            currentPos = currentPos.Add(new Point(0, SpaceBetween + element.Height));
        }
    }
}