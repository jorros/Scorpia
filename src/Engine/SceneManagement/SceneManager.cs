using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.SceneManagement;

public class SceneManager
{
    private readonly IServiceProvider _serviceProvider;
    private Scene _currentScene;
    private RenderContext _renderContext;
    private CancellationTokenSource _cancellationTokenSource;

    public SceneManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _renderContext = serviceProvider.GetRequiredService<RenderContext>();
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

    public void Quit()
    {
        _cancellationTokenSource?.Cancel();
    }

    internal void SetCancellationToken(CancellationTokenSource source)
    {
        _cancellationTokenSource = source;
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

        _currentScene.Render(_renderContext);
        foreach (var node in _currentScene.Nodes.Values)
        {
            node.OnRender(_renderContext);
        }
        _renderContext.End();
    }
}