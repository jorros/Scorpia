using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.Components;

public class RenderComponent : Component
{
    public Sprite Sprite { get; set; }
    public OffsetVector Position { get; set; }
    public float Scale { get; set; }

    public RenderComponent(Sprite sprite, OffsetVector position)
    {
        Sprite = sprite;
        Position = position;
        Scale = 1;
    }

    public override void OnRender(RenderContext context)
    {
        context.Draw(Sprite, Position, new OffsetVector(Scale, Scale), null);
    }
}