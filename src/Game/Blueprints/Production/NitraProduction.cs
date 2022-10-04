using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Production;

public class NitraProduction : Production
{
    public NitraProduction(float value)
    {
        Value = value;
    }

    public override float Value { get; }

    public override void Apply(PlayerNode player, LocationNode location)
    {
        if (location.MapTile.Resource == Resource.Nitra)
        {
            player.NitraBalance.Value = AddToBalance(player.NitraBalance.Value);
        }
    }
}