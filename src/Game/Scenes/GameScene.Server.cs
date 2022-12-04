using Microsoft.Extensions.DependencyInjection;
using Scorpia.Game.Nodes;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.Player;
using Scorpian.HexMap;

namespace Scorpia.Game.Scenes;

public partial class GameScene
{
    protected override void ServerOnLoad()
    {
        var playerManager = ServiceProvider.GetRequiredService<ServerPlayerManager>();
        
        foreach (var player in playerManager.Players)
        {
            var node = SpawnNode<PlayerNode>(n =>
            {
                n.Color.Value = (byte)player.Color!.Value;
                n.Name.Value = player.Name;
                n.Uid.Value = player.NetworkId;
                n.FoodBalance.Value = new BalanceSheet();
                n.NitraBalance.Value = new BalanceSheet();
                n.ScorpionsBalance.Value = new BalanceSheet();
                n.SofrumBalance.Value = new BalanceSheet();
                n.ZellosBalance.Value = new BalanceSheet();
            });
        }
        
        _map = CreateNode<MapNode>();
        SpawnNode<NotificationNode>();

        var location = SpawnNode<LocationNode>(n =>
        {
            n.Name.Value = "Inglewood";
            n.Player.Value = playerManager.Players.First().NetworkId;
            n.Position.Value = new Hex(6, 5, -11);
            n.Population.Value = 3;
            n.IsCapital.Value = true;
        });
    }
}