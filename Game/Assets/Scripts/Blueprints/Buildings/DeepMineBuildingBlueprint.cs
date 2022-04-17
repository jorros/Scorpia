using System.Collections.Generic;
using Actors;
using Blueprints.Requirements;
using Map;

namespace Blueprints.Buildings
{
    public class DeepMineBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.DeepMine;
        public BuildingType? Upgrade => null;
        public BuildingType Family => BuildingType.Mine;
        public int ConstructionCost => 200;
        public int Upkeep => 8;
        public int DoubloonCost => 40;
        public int NitraCost => 0;
        public string Name => "Deep Mine";
        public string Description => "This deep mine produces more of either nitra or sofrum.";
        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            R.Or(new LocationTypeRequirement(LocationType.Town), new LocationTypeRequirement(LocationType.City)),
            new NitraDepositRequirement(),
            new SofrumDepositRequirement(),
            new UpkeepRequirement(8),
            new CostRequirement(40)
        };
    }
}