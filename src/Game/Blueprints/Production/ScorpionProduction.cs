using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Production;

public class ScorpionProduction : Production
{
    public ScorpionProduction(float value)
    {
        Value = value;
    }

    public override float Value { get; }

    public override void Apply(PlayerNode player, LocationNode location)
    {
        if (location.MapTile.Resource == Resource.Gold)
        {
            location.Income.Value = AddToBalance(location.Income.Value);
            player.ScorpionsBalance.Value = AddToBalance(player.ScorpionsBalance.Value);
        }
    }
}