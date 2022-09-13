using System;
using System.Drawing;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class RadioButton : UIElement
{
    public string Type { get; set; }

    public object Value { get; set; }

    public Content Content
    {
        get => _content;
        set
        {
            if (value is not null)
            {
                value.Value.Parent = this;
            }

            _content = value;
        }
    }

    public bool IsSelected { get; set; }

    private Rectangle? _bounds;
    private bool _isPressed;
    private Content _content;

    public RadioButton()
    {
        Input.OnMouseButton += InputOnOnMouseButton;
    }

    private void InputOnOnMouseButton(object sender, MouseButtonEventArgs e)
    {
        if (!Show || !Enabled)
        {
            return;
        }

        if (e.Type == MouseEventType.ButtonUp)
        {
            _isPressed = false;
        }

        if (_bounds is null || Parent is not RadioGroup group)
        {
            return;
        }

        if (_bounds.Value.Contains(new Point(e.X, e.Y)))
        {
            switch (e.Type)
            {
                case MouseEventType.ButtonDown:
                    _isPressed = true;
                    break;
                case MouseEventType.ButtonUp:
                    group.Select(this);
                    break;
            }

            return;
        }

        _isPressed = false;
    }

    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        var style = stylesheet.GetRadioButton(Type);

        if (Content is not null)
        {
            Width = Math.Max(style.MinWidth, Content.Value.Width + style.Padding.X + style.Padding.Width);
            Height = Math.Max(style.MinHeight, Content.Value.Height + style.Padding.Y + style.Padding.Height);
        }
        else
        {
            Width = style.MinWidth;
            Height = style.MinHeight;
        }

        var position = GetPosition();
        _bounds = new Rectangle(stylesheet.Scale(position.X), stylesheet.Scale(position.Y), stylesheet.Scale(Width),
            stylesheet.Scale(Height));

        if (!Show)
        {
            return;
        }

        var tint = Color.White;

        if (style.HoveredTint is not null && Enabled && _bounds.Value.Contains(Input.MousePosition.ToPoint()))
        {
            tint = style.HoveredTint.Value;
        }

        if (style.PressedTint is not null && Enabled && _isPressed)
        {
            tint = style.PressedTint.Value;
        }

        if (!Enabled)
        {
            tint = Color.DarkGray;
        }

        var sprite = style.UncheckedButton;
        if (IsSelected)
        {
            sprite = style.CheckedButton;
        }

        renderContext.Viewport.Draw(sprite, _bounds.Value, 0, tint, 255, inWorld);

        if (Content is not null)
        {
            Content.Value.Position = new OffsetVector(style.Padding.X, style.Padding.Y);
            Content.Value.Render(renderContext, stylesheet, inWorld);
        }
    }
}