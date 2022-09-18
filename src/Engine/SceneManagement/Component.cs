using System;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.SceneManagement;

public abstract class Component : IDisposable
{
    protected IServiceProvider ServiceProvider { get; private set; }
    protected AssetManager AssetManager => ServiceProvider.GetService<AssetManager>();
    protected DefaultSceneManager SceneManager => ServiceProvider.GetRequiredService<DefaultSceneManager>();
    protected UserDataManager UserDataManager => ServiceProvider.GetRequiredService<UserDataManager>();
    protected Camera Camera => ServiceProvider.GetRequiredService<RenderContext>().Camera;
    protected Node Parent { get; private set; }

    public virtual void OnCleanUp()
    {
    }
    
    public virtual void OnInit()
    {
    }
    
    public virtual void OnUpdate()
    {
    }

    internal virtual void Update()
    {
        OnUpdate();
    }

    public virtual void OnTick()
    {
    }

    internal virtual void Tick()
    {
        OnTick();
    }
    
    public virtual void OnRender(RenderContext context)
    {
    }

    internal void Init(Node parent, IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Parent = parent;
    }

    public virtual void Dispose()
    {
        OnCleanUp();
    }
}