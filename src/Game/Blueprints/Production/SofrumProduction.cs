using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Production;

public class SofrumProduction : Production
{
    public SofrumProduction(float value)
    {
        Value = value;
    }

    public override float Value { get; }

    public override void Apply(PlayerNode player, LocationNode location)
    {
        if (location.MapTile.Resource == Resource.Sofrum)
        {
            player.SofrumBalance.Value = AddToBalance(player.SofrumBalance.Value);
        }
    }
}