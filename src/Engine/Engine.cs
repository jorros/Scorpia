using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Asset.AssetLoaders;
using Scorpia.Engine.Asset.Markup;
using Scorpia.Engine.Asset.SpriteSheetParsers;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.SceneManagement;
using SDL2;
using static SDL2.SDL;

namespace Scorpia.Engine;

public abstract class Engine
{
    protected abstract void Init(IServiceCollection services);

    protected abstract void Load(IServiceProvider serviceProvider);

    public void Run(bool headless = false, IntPtr? viewHandler = null)
    {
        var services = new ServiceCollection();

        services.AddSingleton<GraphicsManager>();
        services.AddSingleton<SceneManager>();
        services.AddSingleton<AssetManager>();
        services.AddSingleton<UserDataManager>();
        services.AddSingleton<RenderContext>();
        services.AddSingleton<FontMarkupReader>();

        services.AddSingleton<IAssetLoader, SpriteLoader>();
        services.AddSingleton<IAssetLoader, FontLoader>();

        services.AddSingleton<LibgdxSpriteSheetParser>();

        Init(services);

        var sp = services.BuildServiceProvider();

        var graphicsManager = sp.GetRequiredService<GraphicsManager>();
        var sceneManager = sp.GetRequiredService<SceneManager>();
        var assetManager = sp.GetRequiredService<AssetManager>();
        var userDataManager = sp.GetRequiredService<UserDataManager>();
        var renderContext = sp.GetRequiredService<RenderContext>();
        
        var running = new CancellationTokenSource();
        sceneManager.SetCancellationToken(running);

        userDataManager.Load();

        if (!headless)
        {
            graphicsManager.Init(viewHandler);
            renderContext.Init();
        }

        var highRes = graphicsManager.IsHighRes();

        var assetLoaders = sp.GetServices<IAssetLoader>();
        assetManager.Init(assetLoaders, highRes);

        Load(sp);

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
                try
                {
                    stopwatch.Start();

                    sceneManager.Update();

                    await Task.Delay((int) Math.Floor(Math.Max(1000 / 30.0 - stopwatch.ElapsedMilliseconds, 0)), token);

                    stopwatch.Stop();
                    stopwatch.Reset();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }, token);
    }

    private void StartRender(GraphicsManager graphicsManager, SceneManager sceneManager,
        CancellationTokenSource cancellationTokenSource)
    {
        var stopwatch = new Stopwatch();

        var highDpi = graphicsManager.IsHighDpiMode();

        while (!cancellationTokenSource.IsCancellationRequested)
        {
            stopwatch.Start();
            var start = SDL_GetPerformanceCounter();

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

                    case SDL_EventType.SDL_MOUSEMOTION:
                        Input.CaptureMouseMotion(e.motion, highDpi);
                        break;

                    case SDL_EventType.SDL_MOUSEWHEEL:
                        Input.CaptureMouseWheel(e.wheel);
                        break;

                    case SDL_EventType.SDL_MOUSEBUTTONUP:
                    case SDL_EventType.SDL_MOUSEBUTTONDOWN:
                        Input.CaptureMouseButton(e.button, highDpi);
                        break;

                    case SDL_EventType.SDL_TEXTINPUT:
                        var ch = new byte[Marshal.SizeOf<char>()];
                        unsafe
                        {
                            Marshal.Copy((IntPtr)e.text.text, ch, 0, Marshal.SizeOf<char>());
                        }
                        Input.RaiseTextInput(Encoding.UTF8.GetChars(ch)[0]);

                        break;
                }
            }

            graphicsManager.Clear();

            sceneManager.Render(stopwatch.Elapsed);

            graphicsManager.Flush();
            
            var end = SDL_GetPerformanceCounter();
            var elapsed = (end - start) / (float)SDL_GetPerformanceFrequency();

            stopwatch.Stop();
            graphicsManager.FPS = (int)(1.0f / elapsed);
            Thread.Sleep((int) Math.Floor(Math.Max(4.0 - stopwatch.ElapsedMilliseconds, 0)));
            stopwatch.Reset();
        }
    }
}