using System.Collections.Generic;
using System.Drawing;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class Window : UIElement
{
    public string Type { get; set; }

    private BasicLayout Content { get; set; }
    
    private HorizontalGridLayout ActionBar { get; set; }

    private List<UIElement> _tempList = new();
    private List<Button> _tempActionList = new();
    
    public void Attach(UIElement element)
    {
        if (Content is null)
        {
            _tempList.Add(element);
            
            return;
        }
        Content.Attach(element);
    }

    public void AttachAction(Button button)
    {
        if (ActionBar is null)
        {
            _tempActionList.Add(button);
            return;
        }

        ActionBar.Attach(button);
    }

    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
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

        if (Content is null)
        {
            Content = new BasicLayout(stylesheet)
            {
                Parent = this,
                Position = style.Padding
            };
            Content.SetSize(Width - style.Padding.X * 2, Height - style.Padding.Y * 2 - style.ActionBarHeight);

            foreach (var element in _tempList)
            {
                Attach(element);
            }

            _tempList = null;
        }

        var pos = GetPosition();

        if (ActionBar is null)
        {
            ActionBar = new HorizontalGridLayout
            {
                Background = style.ActionBarBackground,
                Parent = this,
                Padding = style.ActionBarPadding,
                Position = new OffsetVector(0, Height / 2 - style.ActionBarHeight),
                SpaceBetween = style.ActionBarSpaceBetween,
                MinWidth = style.ActionBarMinWidth,
                Anchor = UIAnchor.Center,
                Margin = style.ActionBarMargin
            };
            ActionBar.SetHeight(style.ActionBarHeight);

            foreach (var element in _tempActionList)
            {
                ActionBar.Attach(element);
            }

            _tempActionList = null;
        }
        
        if (!Show)
        {
            return;
        }

        var position = stylesheet.Scale(pos);
        var rect = new Rectangle(position.X, position.Y, stylesheet.Scale(Width), stylesheet.Scale(Height - style.ActionBarHeight));

        renderContext.Viewport.Draw(style.Background, rect, 0, Color.White, 255, inWorld);
        Content.Render(renderContext, inWorld);
        
        ActionBar.Render(renderContext, stylesheet, inWorld);
    }
}