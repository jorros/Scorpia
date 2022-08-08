using Scorpia.Engine.Graphics;

namespace Scorpia.Engine;

public abstract class Component
{
    protected Component(Node parent)
    {
        Parent = parent;
    }
    
    protected Node Parent { get; }
    
    public virtual void OnCleanUp()
    {
    }
    
    public virtual void OnInit()
    {
    }
    
    public virtual void OnUpdate()
    {
    }
    
    public virtual void OnRender(RenderContext context)
    {
    }
}