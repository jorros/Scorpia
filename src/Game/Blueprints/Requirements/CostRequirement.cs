using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Requirements;

public sealed class CostRequirement : Requirement
{
    public CostRequirement(int value)
    {
        Value = value;
    }

    public override int Value { get; }

    public override int Index => 0;

    public override bool IsFulfilled(PlayerNode player, MapTile mapTile)
    {
        return player.Scorpions.Value >= Value;
    }
}