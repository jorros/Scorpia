using Actors;
using Map;

namespace Blueprints.Requirements
{
    public class ZellosDepositRequirement : Requirement
    {
        public ZellosDepositRequirement()
        {
            Value = 1;
        }

        public override int Value { get; }
        public override int Index => 24;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return mapTile.Resource == Resource.Zellos;
        }
    }
}