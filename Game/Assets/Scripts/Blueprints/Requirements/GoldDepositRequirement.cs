using Actors;
using Map;

namespace Blueprints.Requirements
{
    public class GoldDepositRequirement : Requirement
    {
        public GoldDepositRequirement()
        {
            Value = 1;
        }

        public override int Value { get; }
        public override int Index => 21;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return mapTile.Resource == Resource.Gold;
        }
    }
}