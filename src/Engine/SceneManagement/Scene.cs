using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.SceneManagement;

public abstract class Scene : IDisposable
{
    protected IServiceProvider ServiceProvider { get; private set; }
    protected SceneManager SceneManager { get; private set; }
    public Dictionary<ulong, Node> Nodes { get; } = new();

    private ulong _nodeIdCounter;

    protected Node CreateNode<T>() where T : Node
    {
        return CreateNode(typeof(T));
    }

    public Node FindNode<T>() where T : Node
    {
        return Nodes.FirstOrDefault(x => x.Value is T).Value;
    }

    protected Node CreateNode(Type type)
    {
        if (!type.IsAssignableTo(typeof(Node)))
        {
            return null;
        }
        
        var node = (Node)Activator.CreateInstance(type);
        node?.Create(_nodeIdCounter, ServiceProvider, this);

        Nodes.Add(_nodeIdCounter, node);

        _nodeIdCounter++;

        return node;
    }
    
    internal void Render(RenderContext context)
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

    internal void Update()
    {
        OnUpdate();

        foreach (var node in Nodes.Values)
        {
            foreach (var component in node.Components)
            {
                component.OnUpdate();
            }

            node.OnUpdate();
        }
    }

    internal void Tick()
    {
        OnTick();

        foreach (var node in Nodes.Values)
        {
            foreach (var component in node.Components)
            {
                component.OnTick();
            }
            
            node.OnTick();
        }
    }

    internal void Load(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        SceneManager = serviceProvider.GetRequiredService<SceneManager>();

        OnLoad(serviceProvider.GetService<AssetManager>());
    }

    protected abstract void OnLoad(AssetManager assetManager);

    protected virtual void OnTick()
    {
    }

    protected virtual void OnUpdate()
    {
    }

    protected virtual void OnRender(RenderContext context)
    {
    }

    protected virtual void OnLeave()
    {
    }

    public void Dispose()
    {
        foreach (var node in Nodes.Values)
        {
            node.OnCleanUp();
        }

        Nodes.Clear();

        OnLeave();
    }
}