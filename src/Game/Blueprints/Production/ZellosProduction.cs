using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Production;

public class ZellosProduction : Production
{
    public ZellosProduction(float value)
    {
        Value = value;
    }

    public override float Value { get; }

    public override void Apply(PlayerNode player, LocationNode location)
    {
        if (location.MapTile.Resource == Resource.Zellos)
        {
            player.ZellosBalance.Value = AddToBalance(player.ZellosBalance.Value);
        }
    }
}