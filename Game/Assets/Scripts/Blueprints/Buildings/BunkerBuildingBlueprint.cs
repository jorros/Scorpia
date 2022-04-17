using System.Collections.Generic;
using Actors;
using Blueprints.Requirements;
using Map;

namespace Blueprints.Buildings
{
    public class BunkerBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Bunker;
        public BuildingType? Upgrade => null;
        public BuildingType Family => BuildingType.Forts;
        public int ConstructionCost => 200;
        public string Name => "Bunker";
        public string Description => "Increases the troops that are garrisoned here.";
        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            new LocationTypeRequirement(LocationType.City),
            new UpkeepRequirement(6),
            new CostRequirement(50)
        };
    }
}