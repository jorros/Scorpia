using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public class Image : UIElement
{
    private Sprite _sprite;

    public Sprite Sprite
    {
        get => _sprite;
        set
        {
            _sprite = value;
            Width = value.Size.X;
            Height = value.Size.Y;
        }
    }

    public override void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        var position = stylesheet.Scale(GetPosition());
        var bounds = new Rectangle(position.X, position.Y, stylesheet.Scale(Width), stylesheet.Scale(Height));

        renderContext.Viewport.Draw(Sprite, bounds, 0, Color.White, 255, inWorld);
    }
}