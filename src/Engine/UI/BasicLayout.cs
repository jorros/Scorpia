using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Redzen.Sorting;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class BasicLayout : UIElement, Container
{
    protected List<UIElement> Elements { get; } = new();
    
    public Sprite Background { get; set; }

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
        if (Width == 0)
        {
            var renderSize = renderContext.GetDrawSize();
            Width = renderSize.Width;
            Height = renderSize.Height;
        }
    }

    protected override void OnRender(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        if (!Show)
        {
            return;
        }
        
        if (Background is not null)
        {
            Rectangle backgroundRect;
            var position = GetPosition();

            if (Background is NinePatchSprite)
            {
                backgroundRect = new Rectangle(position.X, position.Y, Width, Height);
            }
            else
            {
                backgroundRect = new Rectangle(position.X + Width / 2, position.Y + Height / 2, Background.Size.Width,
                    Background.Size.Height);
                var ratio = Background.Size.Width / (double) Background.Size.Height;
                switch (ratio)
                {
                    case > 1:
                        backgroundRect.Height = (int) (Width / ratio);
                        backgroundRect.Width = Width;
                        break;
                    case < 1:
                        backgroundRect.Height = Height;
                        backgroundRect.Width = (int) (Height * ratio);
                        break;
                }

                backgroundRect.X = position.X + Width / 2 - backgroundRect.Width / 2;
                backgroundRect.Y = position.Y + Height / 2 - backgroundRect.Height / 2;
            }

            renderContext.Draw(Background, backgroundRect, 0, Color.White, 255, -1, inWorld);
        }

        var span = CollectionsMarshal.AsSpan(Elements);
        TimSort<UIElement>.Sort(span);
        foreach (var element in span)
        {
            element.Render(renderContext, stylesheet, inWorld);
        }
    }
}