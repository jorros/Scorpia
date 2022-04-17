using System.Collections.Generic;
using Actors;
using Blueprints.Requirements;
using Map;

namespace Blueprints.Buildings
{
    public class ResidenceBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Residence;
        public BuildingType? Upgrade => BuildingType.Residence;
        public BuildingType Family => BuildingType.Residence;
        public int ConstructionCost => 100;
        public string Name => "Residences";
        public string Description => "A residence district able to house up to 1000 pop.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            new CostRequirement(10)
        };
    }
}