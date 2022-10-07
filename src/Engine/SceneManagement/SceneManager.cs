using System;
using System.Threading;

namespace Scorpia.Engine.SceneManagement;

public abstract class SceneManager
{
    public abstract T Load<T>() where T : Scene;
    public abstract void Switch(string scene, bool unloadCurrent = true);
    public abstract bool IsLoaded(string scene);
    public abstract void Quit();
    public abstract Scene GetCurrentScene();
    internal abstract void SetCancellationToken(CancellationTokenSource source);
    internal abstract void Tick();
    internal abstract void Update();
    internal abstract void Render(TimeSpan elapsedTime);
}