using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.SceneManagement;

public abstract class Scene : IDisposable
{
    private IServiceProvider _serviceProvider;

    internal void SetServiceProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Dictionary<ulong, Node> Nodes { get; } = new();

    private ulong _nodeIdCounter;

    public Node CreateNode<T>() where T : Node
    {
        var node = _serviceProvider.GetRequiredService<T>();
        var type = node.GetType();

        type.GetProperty("Identifier")!.SetValue(node, _nodeIdCounter);
        type.GetProperty("ServiceProvider")!.SetValue(node, _serviceProvider);
        type.GetProperty("Scene")!.SetValue(node, this);
        
        node.OnInit();
        foreach (var component in node.Components)
        {
            component.OnInit();
        }

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