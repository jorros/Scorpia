using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Requirements;

public class NitraDepositRequirement : Requirement
{
    public NitraDepositRequirement()
    {
        Value = 1;
    }

    public override int Value { get; }
    public override int Index => 22;

    public override bool IsFulfilled(PlayerNode player, MapTile mapTile)
    {
        return mapTile.Resource == Resource.Nitra;
    }
}