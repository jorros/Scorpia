using System;
using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.Maths;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class Button : UIElement
{
    public string Type { get; set; }
    public Content Content { get; set; }

    public delegate void ClickedEventHandler(object sender, MouseButtonEventArgs e);

    public event ClickedEventHandler OnClick;

    private Rectangle? _bounds;
    private bool _isPressed;

    public Button()
    {
        Input.OnMouseButton += (_, args) =>
        {
            if (!Show || !Enabled)
            {
                return;
            }

            if (args.Type == MouseEventType.ButtonUp)
            {
                _isPressed = false;
            }

            if (_bounds is null)
            {
                return;
            }

            if (_bounds.Value.Contains(new Point(args.X, args.Y)))
            {
                switch (args.Type)
                {
                    case MouseEventType.ButtonDown:
                        _isPressed = true;
                        break;
                    case MouseEventType.ButtonUp:
                        OnClick?.Invoke(this, args);
                        break;
                }

                return;
            }

            _isPressed = false;
        };
    }

    private bool IsInButton(Point position)
    {
        if (_bounds is null)
        {
            return false;
        }

        return position.X >= _bounds.Value.X && position.X <= _bounds.Value.X + _bounds.Value.Width &&
               position.Y >= _bounds.Value.Y && position.Y <= _bounds.Value.Y + _bounds.Value.Height;
    }

    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        var style = stylesheet.GetButton(Type);

        var content = Content.Value;
        var paddedTextWidth = style.Padding.X * 2 + content.Width;
        var paddedTextHeight = style.Padding.Y * 2 + content.Height;

        Width = style.MinWidth is not null ? Math.Max(style.MinWidth.Value, paddedTextWidth) : style.FixedWidth;
        Height = style.MinHeight is not null ? Math.Max(style.MinHeight.Value, paddedTextHeight) : style.FixedHeight;

        var position = GetPosition();

        _bounds = new Rectangle(stylesheet.Scale(position.X), stylesheet.Scale(position.Y), stylesheet.Scale(Width),
            stylesheet.Scale(Height));
        
        if (!Show)
        {
            return;
        }

        var tint = Color.White;

        if (style.HoveredTint is not null && Enabled && IsInButton(Input.MousePosition))
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

        renderContext.Draw(style.Button, _bounds.Value, 0, tint, 255, -1, inWorld);

        content.Position = new Point(Width / 2, Height / 2).Add(position).Add(style.ContentPosition);
        content.Render(renderContext, stylesheet, inWorld);
    }
}