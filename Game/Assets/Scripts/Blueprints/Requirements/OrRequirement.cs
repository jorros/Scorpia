using System.Collections.Generic;
using System.Linq;
using Actors;
using Map;

namespace Blueprints.Requirements
{
    public class OrRequirement : Requirement
    {
        public IEnumerable<Requirement> Requirements { get; set; }

        public override int Value => default;
        public override int Index => Requirements.First().Index;

        public override bool IsFulfilled(Player player, MapTile mapTile)
        {
            return Requirements.Any(x => x.IsFulfilled(player, mapTile));
        }
    }
}