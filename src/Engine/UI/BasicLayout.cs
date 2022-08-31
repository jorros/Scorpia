using System.Collections.Generic;
using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class BasicLayout : UIElement
{
    private readonly Stylesheet _stylesheet;
    
    protected List<UIElement> Elements { get; } = new();
    
    public Sprite Background { get; set; }

    public BasicLayout(Stylesheet stylesheet)
    {
        _stylesheet = stylesheet;
    }
    
    public void Attach(UIElement element)
    {
        element.Parent = this;
        Elements.Add(element);
    }

    public void SetSize(int width, int height)
    {
        Width = width;
        Height = height;
    }
    
    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        if (Width == 0)
        {
            var renderSize = renderContext.GetDrawSize();
            Width = renderSize.X;
            Height = renderSize.Y;
        }

        if (Background is not null)
        {
            var position = GetPosition();
            var backgroundRect = new Rectangle(position.X + Width / 2, position.Y + Height / 2, Background.Size.X, Background.Size.Y);
            var ratio = Background.Size.X / (double)Background.Size.Y;
            switch (ratio)
            {
                case > 1:
                    backgroundRect.Height = Height;
                    backgroundRect.Width = (int)(Height * ratio);
                    backgroundRect.X = position.X + Width / 2 - backgroundRect.Width / 2;
                    backgroundRect.Y = position.Y + Height / 2 - backgroundRect.Height / 2;
                    break;
                case < 1:
                    backgroundRect.Height = (int)(Width * ratio);
                    backgroundRect.Width = Width;
                    backgroundRect.X = position.X + backgroundRect.Width / 2;
                    backgroundRect.Y = position.Y + backgroundRect.Height / 2;
                    break;
            }
            
            renderContext.Viewport.Draw(Background, backgroundRect, 0, Color.White, 255, inWorld);
        }
        
        foreach (var element in Elements)
        {
            element.Render(renderContext, stylesheet, inWorld);
        }
    }
    
    public void Render(RenderContext renderContext, bool inWorld)
    {
        Render(renderContext, _stylesheet, inWorld);
    }
}