using System;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Engine;

public abstract class Component
{
    protected IServiceProvider ServiceProvider { get; private set; }
    protected AssetManager AssetManager => ServiceProvider.GetService<AssetManager>();
    protected DefaultSceneManager SceneManager => ServiceProvider.GetRequiredService<DefaultSceneManager>();
    protected UserDataManager UserDataManager => ServiceProvider.GetRequiredService<UserDataManager>();
    protected Viewport Viewport => ServiceProvider.GetRequiredService<RenderContext>().Viewport;
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

    public virtual void OnTick()
    {
    }
    
    public virtual void OnRender(RenderContext context)
    {
    }

    internal void Init(Node parent, IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
        Parent = parent;
    }
}