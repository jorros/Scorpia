using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Maths;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class GridLayout : UIElement
{
    public List<UIElement> Elements { get; } = new();
    public Sprite Background { get; set; }
    public Rectangle Padding { get; set; }
    public Point Margin { get; set; } = Point.Empty;
    public Size GridSize { get; set; }
    
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

    public void SetSize(int width, int height)
    {
        Width = width;
        Height = height;
    }

    private Point GetPoint(int x, int y)
    {
        var cellWidth = Width / GridSize.Width;
        var cellHeight = Height / GridSize.Height;

        return new Point(x * cellWidth, y * cellHeight);
    }

    private Point GetPoint(int pos)
    {
        var y = (int)Math.Floor(pos / (float)GridSize.Width);
        var x = pos % GridSize.Width;

        return GetPoint(x, y);
    }
    
    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        if (Background is not null && Show)
        {
            var scaledPos = stylesheet.Scale(GetPosition()).Add(stylesheet.Scale(Margin));
            var rect = new Rectangle(scaledPos.X, scaledPos.Y, stylesheet.Scale(Width), stylesheet.Scale(Height));
            renderContext.Draw(Background, rect, 0, Color.White, 255, -1, inWorld);
        }
        
        var currentPos = new Point(Padding.X, Padding.Y).Add(Margin);
        
        for (var i = 0; i < Elements.Count; i++)
        {
            Elements[i].Position = currentPos.Add(GetPoint(i));
            Elements[i].Render(renderContext, stylesheet, inWorld);
        }
    }
}