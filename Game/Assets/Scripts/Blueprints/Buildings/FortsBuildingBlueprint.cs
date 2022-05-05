using System.Collections.Generic;
using Actors.Entities;
using Blueprints.Requirements;

namespace Blueprints.Buildings
{
    public class FortsBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Forts;
        public BuildingType? Upgrade => BuildingType.Bunker;
        public BuildingType Family => BuildingType.Forts;
        public int ConstructionCost => 100;
        public int Upkeep => 3;
        public int DoubloonCost => 30;
        public int NitraCost => 0;
        public string Name => "Forts";
        public string Description => "Increases the troops that are garrisoned here.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            R.Or(new LocationTypeRequirement(LocationType.Town), new LocationTypeRequirement(LocationType.City)),
            new UpkeepRequirement(3),
            new CostRequirement(30)
        };

        public IEnumerable<Production.Production> Production => null;
        public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
    }
}