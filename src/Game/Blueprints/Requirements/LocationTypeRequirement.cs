using Scorpia.Game.Nodes;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Requirements;

public sealed class LocationTypeRequirement : Requirement
{
    public LocationTypeRequirement(LocationType type)
    {
        Value = (int) type;
    }

    public override int Value { get; }
    public override int Index => 30;

    public override bool IsFulfilled(PlayerNode player, MapTile mapTile)
    {
        return mapTile.Location.Type.Value == (byte) Value;
    }
}