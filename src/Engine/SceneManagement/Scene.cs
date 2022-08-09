using System;
using System.Collections.Generic;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.SceneManagement;

public abstract class Scene : IDisposable
{
    private readonly IServiceProvider _serviceProvider;

    public Scene(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Dictionary<ulong, Node> Nodes { get; } = new();

    private ulong _nodeIdCounter;

    public Node CreateNode<T>() where T : Node
    {
        var node = Activator.CreateInstance<T>();
        node.Create(_nodeIdCounter, _serviceProvider, this);

        Nodes.Add(_nodeIdCounter, node);

        _nodeIdCounter++;

        return node;
    }

    public void Render(RenderContext context)
    {
        OnRender(context);
        
        foreach (var node in Nodes.Values)
        {
            foreach (var component in node.Components)
            {
                component.OnRender(context);
            }
            node.OnRender(context);
        }
    }

    protected virtual void OnRender(RenderContext context)
    {
    }

    public virtual void Dispose()
    {
    }
}