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

        _renderContext = serviceProvider.GetService<RenderContext>();
    }

    public Scene Load<T>() where T : Scene
    {
        var scene = Activator.CreateInstance(typeof(T), true) as Scene;
        
        scene?.Load(_serviceProvider);

        return scene;
    }

    public void Switch(Scene scene)
    {
        if (scene is null)
        {
            throw new EngineException($"Switching scene failed. {nameof(scene)} is null.");
        }
        if (_currentScene is not null)
        {
            _currentScene.Dispose();
        }

        _currentScene = scene;
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
        _currentScene.Update();
    }

    internal void Render(TimeSpan elapsedTime)
    {
        _renderContext.Begin();
        _currentScene.Render(_renderContext);
        _renderContext.End();
    }
}