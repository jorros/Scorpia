using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.SceneManagement;

namespace Server.Actions;

public class TestAction : IAction
{
    public string Name => "test";
    public void Execute(IServiceProvider serviceProvider)
    {
        var sceneManager = serviceProvider.GetRequiredService<SceneManager>();
        var scene = sceneManager.GetCurrentScene() as NetworkedScene;
        
        scene?.Invoke("Abc");
    }
}