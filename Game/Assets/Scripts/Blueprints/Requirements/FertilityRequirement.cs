using Actors;
using Map;

namespace Blueprints.Requirements
{
    public class FertilityRequirement : Requirement
    {
        public FertilityRequirement(Fertility fertility)
        {
            Value = (int) fertility;
        }

        public override int Value { get; }
        public override int Index => 20;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return mapTile.Fertility == (Fertility) Value;
        }
    }
}