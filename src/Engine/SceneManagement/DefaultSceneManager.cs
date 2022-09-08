using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Graphics;

namespace Scorpia.Engine.SceneManagement;

public class DefaultSceneManager : SceneManager
{
    private readonly IServiceProvider _serviceProvider;
    private Scene _currentScene;
    private readonly RenderContext _renderContext;
    private CancellationTokenSource _cancellationTokenSource;
    
    protected readonly Dictionary<string, Scene> loadedScenes = new();

    public DefaultSceneManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;

        _renderContext = serviceProvider.GetService<RenderContext>();
    }

    public override void Load<T>()
    {
        var scene = Activator.CreateInstance(typeof(T), true) as Scene;
        scene?.Load(_serviceProvider);

        loadedScenes.Add(typeof(T).Name, scene);
    }

    public override void Switch(string scene)
    {
        if (string.IsNullOrEmpty(scene) || !loadedScenes.ContainsKey(scene))
        {
            throw new EngineException($"Switching scene failed. {nameof(scene)} is not loaded.");
        }

        _currentScene?.Dispose();

        _currentScene = loadedScenes[scene];
    }

    public override void Quit()
    {
        _cancellationTokenSource?.Cancel();
    }

    public override Scene GetCurrentScene()
    {
        return _currentScene;
    }

    internal override void SetCancellationToken(CancellationTokenSource source)
    {
        _cancellationTokenSource = source;
    }

    internal override void Tick()
    {
        _currentScene.Tick();
    }

    internal override void Update()
    {
        _currentScene.Update();
    }

    internal override void Render(TimeSpan elapsedTime)
    {
        _renderContext.Begin();
        _currentScene.Render(_renderContext);
        _renderContext.End();
    }
}