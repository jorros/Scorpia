using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Maths;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class VerticalGridLayout : UIElement, Container
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

    public void Clear()
    {
        Elements.Clear();
    }

    public void Remove(UIElement element)
    {
        Elements.Remove(element);
    }

    public void Remove(Func<UIElement, bool> predicate)
    {
        var element = Elements.FirstOrDefault(predicate);
        if (element is not null)
        {
            Elements.Remove(element);
        }
    }

    protected override void OnInit(RenderContext renderContext, Stylesheet stylesheet)
    {
    }

    protected override void OnRender(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        if (!Show)
        {
            return;
        }
        
        Height = Padding.Y + Padding.Height + Elements.Sum(element => element.Height);
        Height = Height > MinHeight ? Height : MinHeight;
        
        if (Background is not null)
        {
            var scaledPos = stylesheet.Scale(GetPosition()).Add(stylesheet.Scale(Margin));
            var rect = new Rectangle(scaledPos.X, scaledPos.Y, stylesheet.Scale(Width), stylesheet.Scale(Height));
            renderContext.Draw(Background, rect, 0, Color.White, 255, -1, inWorld);
        }
        
        var currentPos = new Point(Padding.X, Padding.Y).Add(Margin);
        
        var span = CollectionsMarshal.AsSpan(Elements);

        foreach (var element in span)
        {
            element.Position = currentPos;

            element.Render(renderContext, stylesheet, inWorld);

            currentPos = currentPos.Add(new Point(0, SpaceBetween + element.Height));
        }
    }
}