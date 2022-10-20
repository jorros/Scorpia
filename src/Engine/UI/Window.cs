using System.Collections.Generic;
using System.Drawing;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Maths;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class Window : UIElement
{
    public string Type { get; set; }

    private BasicLayout Content { get; set; }
    
    private HorizontalGridLayout ActionBar { get; set; }
    private HorizontalGridLayout Title { get; set; }

    private List<UIElement> _tempList = new();
    private List<Button> _tempActionList = new();
    private List<UIElement> _tempTitleList = new();

    public void SetSize(int width, int height)
    {
        Width = width;
        Height = height;
    }
    
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

    public void AttachTitle(Content content)
    {
        if (Title is null)
        {
            _tempTitleList.Add(content.Value);
            return;
        }
        
        Title.Attach(content.Value);
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
                Position = style.Padding.Add(new Point(0, style.HasTitle ? style.TitleHeight : 0))
            };
            Content.SetSize(Width - style.Padding.X * 2, Height - style.Padding.Y * 2 - style.ActionBarHeight);

            foreach (var element in _tempList)
            {
                Attach(element);
            }

            _tempList = null;
        }

        var pos = GetPosition();

        if (ActionBar is null && style.HasActionBar)
        {
            ActionBar = new HorizontalGridLayout
            {
                Parent = this,
                Padding = style.ActionBarPadding,
                Position = new Point(0, Height / 2 - style.ActionBarHeight),
                SpaceBetween = style.ActionBarSpaceBetween,
                Anchor = style.ActionBarAnchor
            };
            ActionBar.SetHeight(style.ActionBarHeight);

            foreach (var element in _tempActionList)
            {
                ActionBar.Attach(element);
            }

            _tempActionList = null;
        }
        
        if (Title is null && style.HasTitle)
        {
            Title = new HorizontalGridLayout
            {
                Parent = this,
                Padding = style.TitlePadding,
                SpaceBetween = style.TitleSpaceBetween,
                Anchor = UIAnchor.TopLeft
            };
            Title.SetHeight(style.TitleHeight);

            foreach (var element in _tempTitleList)
            {
                Title.Attach(element);
            }

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

            _tempTitleList = null;
        }
        
        if (!Show)
        {
            return;
        }

        var position = stylesheet.Scale(pos);
        var rect = new Rectangle(position.X, position.Y, stylesheet.Scale(Width), stylesheet.Scale(Height - style.ActionBarHeight));

        renderContext.Draw(style.Background, rect, 0, Color.White, 255, -1, inWorld);
        Content.Render(renderContext, inWorld);
        
        Title?.Render(renderContext, stylesheet, inWorld);
        ActionBar?.Render(renderContext, stylesheet, inWorld);
    }
}