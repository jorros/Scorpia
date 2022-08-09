using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.SceneManagement;
using static SDL2.SDL;

namespace Scorpia.Engine;

public abstract class Engine
{
    protected abstract void Init(IServiceCollection services);

    protected abstract void Load(IServiceProvider serviceProvider);

    public void Run(bool headless = false)
    {
        var services = new ServiceCollection();

        services.AddSingleton<GraphicsManager>();
        services.AddSingleton<SceneManager>();
        services.AddSingleton<AssetManager>();
        services.AddSingleton<UserDataManager>();

        Init(services);

        var sp = services.BuildServiceProvider();

        var graphicsManager = sp.GetRequiredService<GraphicsManager>();
        var sceneManager = sp.GetRequiredService<SceneManager>();
        var assetManager = sp.GetRequiredService<AssetManager>();
        var userDataManager = sp.GetRequiredService<UserDataManager>();

        sceneManager.SetGraphicsManager(graphicsManager);
        assetManager.SetGraphicsManager(graphicsManager);

        userDataManager.Load();

        if (!headless)
        {
            graphicsManager.Init();
        }

        Load(sp);

        var running = new CancellationTokenSource();

        StartUpdate(sceneManager, running.Token);

        if (headless)
        {
            return;
        }

        StartRender(graphicsManager, sceneManager, running);

        graphicsManager.Quit();
    }

    private void StartUpdate(SceneManager sceneManager, CancellationToken token)
    {
        var stopwatch = new Stopwatch();
        
        Task.Run(async () =>
        {
            while (!token.IsCancellationRequested)
            {
                stopwatch.Start();
                
                sceneManager.Update();

                await Task.Delay((int)Math.Floor(Math.Max(1000 / 30.0 - stopwatch.ElapsedMilliseconds, 0)), token);
                
                stopwatch.Stop();
                stopwatch.Reset();
            }
        }, token);
    }

    private void StartRender(GraphicsManager graphicsManager, SceneManager sceneManager,
        CancellationTokenSource cancellationTokenSource)
    {
        var stopwatch = new Stopwatch();

        while (!cancellationTokenSource.IsCancellationRequested)
        {
            stopwatch.Start();
            
            while (SDL_PollEvent(out var e) == 1)
            {
                switch (e.type)
                {
                    case SDL_EventType.SDL_QUIT:
                        cancellationTokenSource.Cancel();
                        break;

                    case SDL_EventType.SDL_KEYUP:
                    case SDL_EventType.SDL_KEYDOWN:
                        Input.RaiseEvent(e.key);
                        break;
                }
            }

            graphicsManager.Clear();

            sceneManager.Render(stopwatch.Elapsed);

            graphicsManager.Flush();
            
            stopwatch.Stop();
            Thread.Sleep((int)Math.Floor(Math.Max(4.0 - stopwatch.ElapsedMilliseconds, 0)));
            stopwatch.Reset();
        }
    }
}