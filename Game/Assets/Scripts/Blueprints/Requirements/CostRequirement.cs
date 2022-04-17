using Actors;
using Map;

namespace Blueprints.Requirements
{
    public class CostRequirement : Requirement
    {
        public CostRequirement(int value)
        {
            Value = value;
        }

        public override int Value { get; }

        public override int Index => 0;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return player.Scorpions.Value >= Value;
        }
    }
}