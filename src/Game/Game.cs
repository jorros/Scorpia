using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Nodes;
using Scorpia.Game.Scenes;

namespace Scorpia.Game;

public class Game : Engine.Engine
{
    protected override void Init(IServiceCollection services, List<Type> networkedNodes)
    {
        services.AddScene<MainMenuScene>();
        
        networkedNodes.Add(typeof(TestNode));
        AddNetworkPacketsFrom(GetType().Assembly);
    }

    protected override void Load(IServiceProvider serviceProvider)
    {
        var sceneManager = serviceProvider.GetRequiredService<SceneManager>();

        sceneManager.Load<MainMenuScene>();
        sceneManager.Switch(nameof(MainMenuScene));
    }
}