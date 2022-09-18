using System;
using System.Collections.Generic;
using System.Drawing;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Maths;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public abstract class UIElement
{
    public int Width { get; protected set; }
    public int Height { get; protected set; }
    public Point Position { get; set; } = Point.Empty;
    public UIElement Parent { get; internal set; }
    public UIAnchor Anchor { get; set; } = UIAnchor.TopLeft;
    
    private bool? _show;
    private bool? _enabled;

    public bool Enabled
    {
        get
        {
            if (_enabled is null)
            {
                return Parent is null || Parent.Enabled;
            }

            return _enabled.Value;
        }
        set => _enabled = value;
    }

    public bool Show
    {
        get
        {
            if (_show is null)
            {
                return Parent is null || Parent.Show;
            }

            return _show.Value;
        }
        set => _show = value;
    }

    protected Point GetPosition()
    {
        if (Parent is null)
        {
            return Position;
        }

        var parentsPos = Parent.GetPosition();
        var relativePos = parentsPos.Add(Position);

        return Anchor switch
        {
            UIAnchor.TopLeft => relativePos,
            UIAnchor.Top => new Point(relativePos.X + Parent.Width / 2 - Width / 2, relativePos.Y),
            UIAnchor.TopRight => new Point(parentsPos.X + Parent.Width - Width - Position.X, relativePos.Y),
            UIAnchor.Left => new Point(relativePos.X, relativePos.Y + Parent.Height / 2 - Height / 2),
            UIAnchor.Center => new Point(relativePos.X + Parent.Width / 2 - Width / 2,
                relativePos.Y + Parent.Height / 2 - Height / 2),
            UIAnchor.Right => new Point(parentsPos.X + Parent.Width - Width - Position.X,
                relativePos.Y + Parent.Height / 2 - Height / 2),
            UIAnchor.BottomLeft => new Point(relativePos.X, parentsPos.Y + Parent.Height - Height - Position.Y),
            UIAnchor.Bottom => new Point(relativePos.X + Parent.Width / 2 - Width / 2,
                parentsPos.Y + Parent.Height - Height - Position.Y),
            UIAnchor.BottomRight => new Point(parentsPos.X + Parent.Width - Width - Position.X,
                parentsPos.Y + Parent.Height - Height - Position.Y),
            _ => relativePos
        };
    }

    public abstract void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld);
}