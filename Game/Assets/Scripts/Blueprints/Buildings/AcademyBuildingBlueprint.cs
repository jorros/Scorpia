using System.Collections.Generic;
using Actors.Entities;
using Blueprints.Requirements;

namespace Blueprints.Buildings
{
    public class AcademyBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Academy;
        public BuildingType? Upgrade => null;
        public BuildingType Family => BuildingType.Barracks;
        public int ConstructionCost => 200;
        public string Name => "Academy";
        public string Description => "More advanced troops can be trained here.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            R.Or(new LocationTypeRequirement(LocationType.Town), new LocationTypeRequirement(LocationType.City)),
            new UpkeepRequirement(5),
            new CostRequirement(40)
        };

        public IEnumerable<Production.Production> Production => null;

        public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
    }
}