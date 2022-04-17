using Actors;
using Map;

namespace Blueprints.Requirements
{
    public class ZellosRequirement : Requirement
    {
        public ZellosRequirement(int zellos)
        {
            Value = zellos;
        }

        public override int Value { get; }
        public override int Index => 3;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return player.Zellos.Value >= Value;
        }
    }
}