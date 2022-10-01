using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine;
using Scorpia.Engine.Network.Packets;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Nodes;
using Scorpia.Game.Player;
using Scorpia.Game.Scenes;

namespace Scorpia.Game;

public class Game : Engine.Engine
{
    protected override void Init(IServiceCollection services, List<Type> networkedNodes)
    {
        services.AddScene<MainMenuScene>();

        services.AddSingleton<GameState>();
        services.AddSingleton<PlayerManager>();
        services.AddSingleton<CurrentPlayer>();
        
        networkedNodes.Add(typeof(LocationNode));
        AddNetworkPacketsFrom(GetType().Assembly);
        
        SetAuthentication(Auth);
    }

    private static LoginResponsePacket Auth(string authString, IServiceProvider sp)
    {
        var playerManager = sp.GetRequiredService<PlayerManager>();

        if (playerManager.HasAccess(authString))
        {
            return new LoginResponsePacket
            {
                Succeeded = true
            };
        }

        return new LoginResponsePacket
        {
            Succeeded = false,
            Reason = "match in progress"
        };
    }

    protected override void Load(IServiceProvider serviceProvider)
    {
        var sceneManager = serviceProvider.GetRequiredService<SceneManager>();
        
        sceneManager.Load<MainMenuScene>();
        sceneManager.Load<LoadingScene>();
        
        sceneManager.Switch(nameof(MainMenuScene));
    }
}