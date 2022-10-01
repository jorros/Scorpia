using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class Image : UIElement
{
    private Sprite _sprite;
    private int? _width;
    private int? _height;

    public new int Width
    {
        get => base.Width;
        set
        {
            _width = value;
            base.Width = value;
        }
    }

    public new int Height
    {
        get => base.Height;
        set
        {
            _height = value;
            base.Height = value;
        }
    }

    public Sprite Sprite
    {
        get => _sprite;
        set
        {
            _sprite = value;
            if (_width is null)
            {
                base.Width = value.Size.Width;
            }

            if (_height is null)
            {
                base.Height = value.Size.Height;
            }
        }
    }

    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        if (!Show)
        {
            return;
        }
        
        var position = stylesheet.Scale(GetPosition());
        var bounds = new Rectangle(position.X, position.Y, stylesheet.Scale(base.Width), stylesheet.Scale(base.Height));

        renderContext.Draw(Sprite, bounds, 0, Color.White, 255, -1, inWorld);
    }
}