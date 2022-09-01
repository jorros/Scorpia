using System;
using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Asset.Font;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class Button : UIElement
{
    public string Type { get; set; }
    public string Text { get; set; }

    public delegate void ClickedEventHandler(object sender, MouseButtonEventArgs e);

    public event ClickedEventHandler OnClick;

    private Rectangle? _bounds;
    private bool _isPressed;

    public Button()
    {
        Input.OnMouseButton += (sender, args) =>
        {
            if (args.Type == MouseEventType.ButtonUp)
            {
                _isPressed = false;
            }

            if (_bounds is null)
            {
                return;
            }

            if (IsInButton(new OffsetVector(args.X, args.Y)))
            {
                if (args.Type == MouseEventType.ButtonDown)
                {
                    _isPressed = true;
                }

                OnClick?.Invoke(this, args);
            }
        };
    }

    private bool IsInButton(OffsetVector position)
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

        var fontSettings = style.LabelStyle.ToFontSettings();
        fontSettings = fontSettings with {Alignment = TextAlign.Center, Size = stylesheet.Scale(fontSettings.Size)};

        var textSize = style.LabelStyle.Font.CalculateSize(Text, fontSettings);
        var paddedTextWidth = style.Padding.X * 2 + textSize.X;
        var paddedTextHeight = style.Padding.Y * 2 + textSize.Y;

        Width = Math.Max(style.MinWidth, paddedTextWidth);
        Height = Math.Max(style.MinHeight, paddedTextHeight);

        var position = GetPosition();

        _bounds = new Rectangle(stylesheet.Scale(position.X), stylesheet.Scale(position.Y), stylesheet.Scale(Width),
            stylesheet.Scale(Height));

        var tint = Color.White;

        if (style.HoveredTint is not null && IsInButton(Input.MousePosition))
        {
            tint = style.HoveredTint.Value;
        }

        if (style.PressedTint is not null && _isPressed)
        {
            tint = style.PressedTint.Value;
        }

        renderContext.Viewport.Draw(style.Button, _bounds.Value, 0, tint, 255, inWorld);

        var textPosition = new OffsetVector(Width / 2, Height / 2) + position + style.TextPosition;
        renderContext.Viewport.DrawText(style.LabelStyle.Font, 
            stylesheet.Scale(textPosition), 
            Text,
            fontSettings, 
            inWorld);
    }
}