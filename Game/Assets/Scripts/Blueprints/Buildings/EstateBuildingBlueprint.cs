using System.Collections.Generic;
using Actors.Entities;
using Blueprints.Production;
using Blueprints.Requirements;
using Map;

namespace Blueprints.Buildings
{
    public class EstateBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Estate;
        public BuildingType? Upgrade => null;
        public BuildingType Family => BuildingType.Smallholding;
        public int ConstructionCost => 500;
        public string Name => "Estate";
        public string Description => "Produces enough food to satisfy local and global needs.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            new UpkeepRequirement(10),
            new CostRequirement(60),
            R.Or(new LocationTypeRequirement(LocationType.Town), new LocationTypeRequirement(LocationType.City)),
            R.Or(new FertilityRequirement(Fertility.Normal), new FertilityRequirement(Fertility.High)),
        };

        public IEnumerable<Production.Production> Production => new Production.Production[]
        {
            new FoodProduction(6)
        };

        public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
    }
}