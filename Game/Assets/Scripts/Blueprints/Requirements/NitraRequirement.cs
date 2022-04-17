using Actors;
using Map;

namespace Blueprints.Requirements
{
    public class NitraRequirement : Requirement
    {
        public NitraRequirement(int nitra)
        {
            Value = nitra;
        }

        public override int Value { get; }
        public override int Index => 1;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return player.Nitra.Value >= Value;
        }
    }
}