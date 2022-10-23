using System.Drawing;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Maths;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class Window : UIElement, Container
{
    public string Type { get; set; }

    private BasicLayout Content { get; set; }
    public HorizontalGridLayout ActionBar { get; set; }
    public HorizontalGridLayout Title { get; set; }

    public Window()
    {
        Content = new BasicLayout
        {
            Parent = this
        };
        Title = new HorizontalGridLayout
        {
            Parent = this
        };
        ActionBar = new HorizontalGridLayout
        {
            Parent = this
        };
    }

    public void Attach(UIElement element)
    {
        Content.Attach(element);
    }

    public void Clear()
    {
        Content.Clear();
    }

    public void Remove(UIElement element)
    {
        Content.Remove(element);
    }

    public void AttachTitle(Content content)
    {
        Title.Attach(content.Value);
    }

    protected override void OnInit(RenderContext renderContext, Stylesheet stylesheet)
    {
        var style = stylesheet.GetWindow(Type);

        if (Width == 0)
        {
            Width = style.Width;
        }

        if (Height == 0)
        {
            Height = style.Height + style.ActionBarHeight;
        }

        Content.Position = style.Padding.Add(new Point(0, style.HasTitle ? style.TitleHeight : 0));
        Content.Width = Width - style.Padding.X * 2;
        Content.Height = Height - style.Padding.Y * 2 - style.ActionBarHeight;

        if (style.HasActionBar)
        {
            ActionBar.Padding = style.ActionBarPadding;
            ActionBar.Position = new Point(0, Height / 2 - style.ActionBarHeight);
            ActionBar.SpaceBetween = style.ActionBarSpaceBetween;
            ActionBar.Anchor = style.ActionBarAnchor;
            ActionBar.Height = style.ActionBarHeight;
        }

        if (style.HasTitle)
        {
            Title.Padding = style.TitlePadding;
            Title.SpaceBetween = style.TitleSpaceBetween;
            Title.Anchor = style.TitleAnchor;
            Title.Height = style.TitleHeight;
            
            if (style.TitleLabelStyle is not null)
            {
                foreach (var element in Title.Elements)
                {
                    if (element is Label {Type: null} label)
                    {
                        label.Type = style.TitleLabelStyle;
                    }
                }
            }
        }
    }

    protected override void OnRender(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        var style = stylesheet.GetWindow(Type);
        
        if (!Show)
        {
            return;
        }

        var position = stylesheet.Scale(GetPosition());
        var rect = new Rectangle(position.X, position.Y, stylesheet.Scale(Width),
            stylesheet.Scale(Height - style.ActionBarHeight));

        renderContext.Draw(style.Background, rect, 0, Color.White, 255, -1, inWorld);
        Content.Render(renderContext, stylesheet, inWorld);

        Title.Render(renderContext, stylesheet, inWorld);
        ActionBar.Render(renderContext, stylesheet, inWorld);
    }
}