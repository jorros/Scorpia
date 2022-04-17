using System.Collections.Generic;
using Actors;
using Blueprints.Requirements;
using Map;

namespace Blueprints.Buildings
{
    public class MineBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Mine;
        public BuildingType? Upgrade => BuildingType.DeepMine;
        public BuildingType Family => BuildingType.Mine;
        public int ConstructionCost => 100;
        public string Name => "Mine";
        public string Description => "This mine produces either nitra or sofrum.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            new UpkeepRequirement(4),
            new CostRequirement(20),
            R.Or(new NitraDepositRequirement(), new SofrumDepositRequirement())
        };
    }
}