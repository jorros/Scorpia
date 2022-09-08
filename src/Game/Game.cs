using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine;
using Scorpia.Engine.Network.Packets;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Nodes;
using Scorpia.Game.Packets;
using Scorpia.Game.Scenes;

namespace Scorpia.Game;

public class Game : Engine.Engine
{
    protected override void Init(IServiceCollection services, List<Type> networkedNodes)
    {
        services.AddScene<MainMenuScene>();
        
        networkedNodes.Add(typeof(TestNode));
        AddNetworkPacketsFrom(GetType().Assembly);
        
        SetAuthentication(Auth);
    }

    private static LoginResponsePacket Auth(string authString, IServiceProvider sp)
    {
        if (authString == "123")
        {
            return new LoginResponsePacket
            {
                Succeeded = true
            };
        }

        return new LoginResponsePacket
        {
            Succeeded = false,
            Reason = "RUNNING"
        };
    }

    protected override void Load(IServiceProvider serviceProvider)
    {
        var sceneManager = serviceProvider.GetRequiredService<SceneManager>();

        sceneManager.Load<MainMenuScene>();
        sceneManager.Switch(nameof(MainMenuScene));
    }
}