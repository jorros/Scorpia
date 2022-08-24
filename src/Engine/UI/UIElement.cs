using System;
using System.Collections.Generic;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.UI;

public abstract class UIElement
{
    public UIElement(RenderContext renderContext)
    {
        RenderContext = renderContext;
    }

    protected List<UIElement> Elements { get; } = new();
    protected RenderContext RenderContext { get; }

    public int Width { get; protected set; }
    public int Height { get; protected set; }
    public OffsetVector Position { get; set; }

    public T Attach<T>() where T : UIElement
    {
        var element = Activator.CreateInstance(typeof(T), RenderContext) as T;

        Elements.Add(element);

        return element;
    }

    public abstract void Render(OffsetVector position);
}