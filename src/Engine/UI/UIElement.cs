using System;
using System.Collections.Generic;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Engine.UI;

public abstract class UIElement
{
    public int Width { get; protected set; }
    public int Height { get; protected set; }
    public OffsetVector Position { get; set; } = OffsetVector.Zero;
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

    protected OffsetVector GetPosition()
    {
        if (Parent is null)
        {
            return Position;
        }

        var parentsPos = Parent.GetPosition();
        var relativePos = parentsPos + Position;

        return Anchor switch
        {
            UIAnchor.TopLeft => relativePos,
            UIAnchor.Top => new OffsetVector(relativePos.X + Parent.Width / 2 - Width / 2, relativePos.Y),
            UIAnchor.TopRight => new OffsetVector(parentsPos.X + Parent.Width - Width - Position.X, relativePos.Y),
            UIAnchor.Left => new OffsetVector(relativePos.X, relativePos.Y + Parent.Height / 2 - Height / 2),
            UIAnchor.Center => new OffsetVector(relativePos.X + Parent.Width / 2 - Width / 2,
                relativePos.Y + Parent.Height / 2 - Height / 2),
            UIAnchor.Right => new OffsetVector(parentsPos.X + Parent.Width - Width - Position.X,
                relativePos.Y + Parent.Height / 2 - Height / 2),
            UIAnchor.BottomLeft => new OffsetVector(relativePos.X, parentsPos.Y + Parent.Height - Height - Position.Y),
            UIAnchor.Bottom => new OffsetVector(relativePos.X + Parent.Width / 2 - Width / 2,
                parentsPos.Y + Parent.Height - Height - Position.Y),
            UIAnchor.BottomRight => new OffsetVector(parentsPos.X + Parent.Width - Width - Position.X,
                parentsPos.Y + Parent.Height - Height - Position.Y),
            _ => relativePos
        };
    }

    public abstract void Render(RenderContext renderContext, Stylesheet stylesheet, bool inWorld);
}