using System.Collections.Generic;
using Actors;
using Blueprints.Requirements;
using Map;

namespace Blueprints.Buildings
{
    public class SchoolBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.School;
        public BuildingType? Upgrade => BuildingType.University;
        public BuildingType Family => BuildingType.School;
        public int ConstructionCost => 40;
        public string Name => "School";
        public string Description => "Unlocks research.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            new UpkeepRequirement(2),
            new CostRequirement(10)
        };
        public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
    }
}