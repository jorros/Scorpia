using System;
using System.Collections.Generic;
using System.Linq;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Engine;

public abstract class Node : Component
{
    protected int Identifier { get; init; }
    
    protected IServiceProvider ServiceProvider { get; init; }
    
    protected Scene Scene { get; init; }
    internal IEnumerable<Component> Components { get; } = new List<Component>();

    protected void AttachComponent(Component component)
    {
        ((List<Component>) Components).Add(component);
    }

    protected T FindComponent<T>() where T : Component
    {
        return Components.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
    }

    protected Node() : base(null)
    {
    }
}