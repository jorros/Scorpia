using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Engine.Components;

public class RenderComponent : Component
{
    public Sprite Sprite { get; set; }
    public Point Position { get; set; }
    public RenderComponent(Sprite sprite, Point position)
    {
        Sprite = sprite;
        Position = position;
    }

    public override void OnRender(RenderContext context)
    {
        context.Draw(Sprite, Position);
    }
}