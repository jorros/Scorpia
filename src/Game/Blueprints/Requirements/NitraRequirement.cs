using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Requirements;

public class NitraRequirement : Requirement
{
    public NitraRequirement(int nitra)
    {
        Value = nitra;
    }

    public override int Value { get; }
    public override int Index => 1;

    public override bool IsFulfilled(PlayerNode player, MapTile mapTile)
    {
        return player.Nitra.Value >= Value;
    }
}