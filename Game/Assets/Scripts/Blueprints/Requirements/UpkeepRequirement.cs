using Actors;
using Map;

namespace Blueprints.Requirements
{
    public class UpkeepRequirement : Requirement
    {
        public UpkeepRequirement(int upkeep)
        {
            Value = upkeep;
        }

        public override int Value { get; }
        public override int Index => 10;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return true;
        }
    }
}