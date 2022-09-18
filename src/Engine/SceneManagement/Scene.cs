using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.SceneManagement;

public abstract class Scene : IDisposable
{
    protected IServiceProvider ServiceProvider { get; private set; }
    public SceneManager SceneManager { get; private set; }
    public UserDataManager UserDataManager { get; private set; }
    public Camera Camera { get; private set; }
    public Dictionary<ulong, Node> Nodes { get; } = new();
    public ILogger Logger { get; private set; }

    private ulong _nodeIdCounter;

    protected T CreateNode<T>() where T : Node
    {
        return (T)CreateNode(typeof(T));
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

    internal virtual void Update()
    {
        OnUpdate();

        foreach (var node in Nodes.Values)
        {
            foreach (var component in node.Components)
            {
                component.Update();
            }

            node.Update();
        }
    }

    internal void Tick()
    {
        OnTick();

        foreach (var node in Nodes.Values)
        {
            foreach (var component in node.Components)
            {
                component.Tick();
            }
            
            node.Tick();
        }
    }

    internal virtual void Load(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        SceneManager = serviceProvider.GetRequiredService<SceneManager>();
        var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();
        Logger = loggerFactory.CreateLogger(GetType());
        UserDataManager = serviceProvider.GetRequiredService<UserDataManager>();

        var assetManager = serviceProvider.GetService<AssetManager>();

        if (assetManager is not null)
        {
            Camera = serviceProvider.GetRequiredService<RenderContext>().Camera;
            OnLoad(assetManager);
        }
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

    public virtual void Dispose()
    {
        foreach (var node in Nodes.Values)
        {
            node.Dispose();
        }

        Nodes.Clear();

        OnLeave();
    }
}