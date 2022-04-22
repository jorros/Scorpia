using System.Collections.Generic;
using Actors;
using Blueprints.Requirements;
using Map;

namespace Blueprints.Buildings
{
    public class RefineryBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Refinery;
        public BuildingType? Upgrade => BuildingType.RefineryComplex;
        public BuildingType Family => BuildingType.Refinery;
        public int ConstructionCost => 20;
        public string Name => "Refinery";
        public string Description => "Produces zellos.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            new ZellosDepositRequirement(),
            new UpkeepRequirement(3),
            new CostRequirement(10)
        };
        public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
    }
}