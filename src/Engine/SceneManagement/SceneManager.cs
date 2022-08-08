using System;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.SceneManagement;

public class SceneManager
{
    private readonly IServiceProvider _serviceProvider;
    private Scene _currentScene;
    private RenderContext _renderContext;

    public SceneManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    internal void SetGraphicsManager(GraphicsManager graphicsManager)
    {
        _renderContext = graphicsManager.CreateContext();
    }

    public void Switch<T>() where T : Scene
    {
        if (_currentScene is not null)
        {
            foreach (var node in _currentScene.Nodes.Values)
            {
                node.OnCleanUp();
            }
            _currentScene.Nodes.Clear();
        }
        
        var scene = _serviceProvider.GetService<T>();

        _currentScene = scene ?? throw new EngineException($"PreLoading: Could not find scene {nameof(T)}");
    }

    internal void Update()
    {
        foreach (var node in _currentScene.Nodes.Values)
        {
            node.OnUpdate();
        }
    }

    internal void Render(TimeSpan elapsedTime)
    {
        _renderContext.Begin();
        _renderContext.ElapsedTime = elapsedTime;
        
        _currentScene.Render(_renderContext);
        foreach (var node in _currentScene.Nodes.Values)
        {
            node.OnRender(_renderContext);
        }
        _renderContext.End();
    }
}