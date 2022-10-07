using Microsoft.Extensions.DependencyInjection;
using Scorpia.Game.Nodes;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.Player;

namespace Scorpia.Game.Scenes;

public partial class GameScene
{
    protected override void ServerOnLoad()
    {
        var playerManager = ServiceProvider.GetRequiredService<ServerPlayerManager>();
        
        foreach (var player in playerManager.Players)
        {
            var node = SpawnNode<PlayerNode>();
            node.Color.Value = (byte)player.Color!.Value;
            node.Name.Value = player.Name;
            node.Uid.Value = player.NetworkId;
            node.FoodBalance.Value = new BalanceSheet();
            node.NitraBalance.Value = new BalanceSheet();
            node.ScorpionsBalance.Value = new BalanceSheet();
            node.SofrumBalance.Value = new BalanceSheet();
            node.ZellosBalance.Value = new BalanceSheet();
        }
        
        _map = CreateNode<MapNode>();
    }
}