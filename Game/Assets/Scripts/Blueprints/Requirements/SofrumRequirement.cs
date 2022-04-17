using Actors;
using Map;

namespace Blueprints.Requirements
{
    public class SofrumRequirement : Requirement
    {
        public SofrumRequirement(int sofrum)
        {
            Value = sofrum;
        }

        public override int Value { get; }
        public override int Index => 2;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return player.Sofrum.Value >= Value;
        }
    }
}