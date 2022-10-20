using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Maths;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class HorizontalGridLayout : UIElement
{
    public List<UIElement> Elements { get; } = new();
    public Sprite Background { get; set; }
    
    public int MinWidth { get; set; }
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
    
    public void SetHeight(int height)
    {
        Height = height;
    }

    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        var scaledHeight = stylesheet.Scale(Height);

        Width = Padding.X + Padding.Width + Elements.Sum(element => element.Width);
        Width = Width > MinWidth ? Width : MinWidth;
        
        if (Background is not null && Show)
        {
            var scaledPos = stylesheet.Scale(GetPosition()).Add(stylesheet.Scale(Margin));
            var rect = new Rectangle(scaledPos.X, scaledPos.Y, stylesheet.Scale(Width), scaledHeight);
            renderContext.Draw(Background, rect, 0, Color.White, 255, -1, inWorld);
        }
        
        var currentPos = new Point(Padding.X, Padding.Y).Add(Margin);

        foreach (var element in Elements)
        {
            element.Position = currentPos;

            element.Render(renderContext, stylesheet, inWorld);

            currentPos = currentPos.Add(new Point(SpaceBetween + element.Width, 0));
        }
    }
}