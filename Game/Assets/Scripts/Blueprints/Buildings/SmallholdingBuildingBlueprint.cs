using System.Collections.Generic;
using Actors;
using Blueprints.Requirements;

namespace Blueprints.Buildings
{
    public class SmallholdingBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Smallholding;
        public BuildingType? Upgrade => BuildingType.Farm;
        public BuildingType Family => BuildingType.Smallholding;
        public int ConstructionCost => 100;
        public string Name => "Smallholding";
        public string Description => "Produces food for own consumption.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            new UpkeepRequirement(1),
            new CostRequirement(10)
        };
    }
}