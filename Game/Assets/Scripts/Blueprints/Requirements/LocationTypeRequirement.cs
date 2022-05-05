using Actors;
using Actors.Entities;
using Map;

namespace Blueprints.Requirements
{
    public class LocationTypeRequirement : Requirement
    {
        public LocationTypeRequirement(LocationType type)
        {
            Value = (int) type;
        }

        public override int Value { get; }
        public override int Index => 30;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return mapTile.Location.Type.Value == (LocationType) Value;
        }
    }
}