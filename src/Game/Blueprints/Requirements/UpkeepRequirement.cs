using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Requirements;

public class UpkeepRequirement : Requirement
{
    public UpkeepRequirement(int upkeep)
    {
        Value = upkeep;
    }

    public override int Value { get; }
    public override int Index => 10;

    public override bool IsFulfilled(PlayerNode player, MapTile mapTile)
    {
        return true;
    }
}