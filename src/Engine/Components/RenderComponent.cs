using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.Components;

public class RenderComponent : Component
{
    public Sprite Sprite { get; set; }
    public OffsetVector Position { get; set; }

    public RenderComponent(Sprite sprite, OffsetVector position)
    {
        Sprite = sprite;
        Position = position;
    }

    public override void OnRender(RenderContext context)
    {
        context.Draw(Sprite, Position);
    }
}