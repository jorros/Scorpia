using Actors;
using Map;

namespace Blueprints.Requirements
{
    public class SofrumDepositRequirement : Requirement
    {
        public SofrumDepositRequirement()
        {
            Value = 1;
        }

        public override int Value { get; }
        public override int Index => 23;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return mapTile.Resource == Resource.Sofrum;
        }
    }
}