using System.Collections.Generic;
using Actors;
using Blueprints.Requirements;
using Map;

namespace Blueprints.Buildings
{
    public class FarmBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Farm;
        public BuildingType? Upgrade => BuildingType.Estate;
        public BuildingType Family => BuildingType.Smallholding;
        public int ConstructionCost => 200;
        public string Name => "Farm";
        public string Description => "Produces food required for higher demand.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            R.Or(new LocationTypeRequirement(LocationType.Town), new LocationTypeRequirement(LocationType.City)),
            R.Or(new FertilityRequirement(Fertility.Normal), new FertilityRequirement(Fertility.High)),
            new UpkeepRequirement(3),
            new CostRequirement(20)
        };
        public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
    }
}