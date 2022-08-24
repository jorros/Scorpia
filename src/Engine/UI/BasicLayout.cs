using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.UI;

public class BasicLayout : UIElement
{
    public bool InWorld { get; set; }

    public override void Render(OffsetVector position)
    {
        foreach (var element in Elements)
        {
            var modifier = OffsetVector.Zero;
            if (InWorld)
            {
                modifier = RenderContext.Viewport.WorldPosition;
            }
            
            element.Render(element.Position + modifier);
        }
    }

    public void Render()
    {
        Render(OffsetVector.Zero);
    }

    public BasicLayout(RenderContext renderContext) : base(renderContext)
    {
    }
}