using System;
using System.Collections.Generic;
using System.Linq;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Engine;

public abstract class Node : Component
{
    protected ulong Identifier { get; private set; }
    protected Scene Scene { get; private set; }
    internal IEnumerable<Component> Components { get; } = new List<Component>();

    protected void AttachComponent(Component component)
    {
        ((List<Component>) Components).Add(component);
        component.Init(this, ServiceProvider);
    }

    protected T FindComponent<T>() where T : Component
    {
        return Components.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
    }

    internal void Create(ulong identifier, IServiceProvider serviceProvider, Scene scene)
    {
        Identifier = identifier;
        Scene = scene;
        
        Init(this, serviceProvider);
        OnInit();
    }
}