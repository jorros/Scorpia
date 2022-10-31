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
    public static CurrentPlayer CurrentPlayer { get; private set; } = null!;
    public static ServerPlayerManager ServerPlayerManager { get; private set; } = null!;
    public static ScorpiaSettings ScorpiaSettings { get; private set; } = null!;
    public static EventManager EventManager { get; private set; } = null!;

    public static bool AutoConnect = false;
    
    protected override void Init(IServiceCollection services, List<Type> networkedNodes)
    {
        services.AddScene<MainMenuScene>();

        services.AddSingleton<GameState>();
        services.AddSingleton<ServerPlayerManager>();
        services.AddSingleton<CurrentPlayer>();
        services.AddSingleton<ScorpiaSettings>();
        
        networkedNodes.Add(typeof(LocationNode));
        networkedNodes.Add(typeof(PlayerNode));
        networkedNodes.Add(typeof(NotificationNode));
        AddNetworkPacketsFrom(GetType().Assembly);
        
        SetAuthentication(Auth);
    }

    private static LoginResponsePacket Auth(string authString, IServiceProvider sp)
    {
        var playerManager = sp.GetRequiredService<ServerPlayerManager>();

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

        CurrentPlayer = serviceProvider.GetRequiredService<CurrentPlayer>();
        ServerPlayerManager = serviceProvider.GetRequiredService<ServerPlayerManager>();
        ScorpiaSettings = serviceProvider.GetRequiredService<ScorpiaSettings>();
        EventManager = serviceProvider.GetRequiredService<EventManager>();
        
        sceneManager.Load<MainMenuScene>();
        sceneManager.Load<LoadingScene>();
        
        sceneManager.Switch(nameof(MainMenuScene));
    }
}