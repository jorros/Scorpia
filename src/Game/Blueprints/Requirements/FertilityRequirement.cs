using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Requirements;

public class FertilityRequirement : Requirement
{
    public FertilityRequirement(Fertility fertility)
    {
        Value = (int) fertility;
    }

    public override int Value { get; }
    public override int Index => 20;

    public override bool IsFulfilled(PlayerNode player, MapTile mapTile)
    {
        return mapTile.Fertility == (Fertility) Value;
    }
}