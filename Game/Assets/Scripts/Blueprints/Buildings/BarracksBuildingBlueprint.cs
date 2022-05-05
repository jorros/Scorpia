using System.Collections.Generic;
using Actors.Entities;
using Blueprints.Requirements;

namespace Blueprints.Buildings
{
    public class BarracksBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Barracks;
        public BuildingType? Upgrade => BuildingType.Academy;
        public BuildingType Family => BuildingType.Barracks;
        public int ConstructionCost => 60;
        public string Name => "Barracks";
        public string Description => "Allows troops to be trained.";
        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            new UpkeepRequirement(2),
            new CostRequirement(15)
        };

        public IEnumerable<Production.Production> Production => null;
        public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
    }
}