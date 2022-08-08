using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.Components;

public class RenderComponent : Component
{
    public Sprite Sprite { get; }
    public OffsetVector Position { get; }

    public RenderComponent(Node node, Sprite sprite, OffsetVector position) : base(node)
    {
        Sprite = sprite;
        Position = position;
    }

    public override void OnRender(RenderContext context)
    {
        context.Draw(Sprite, Position);
    }
}