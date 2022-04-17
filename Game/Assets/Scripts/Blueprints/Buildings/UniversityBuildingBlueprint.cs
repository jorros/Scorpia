using System.Collections.Generic;
using Actors;
using Blueprints.Requirements;

namespace Blueprints.Buildings
{
    public class UniversityBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.University;
        public BuildingType? Upgrade => null;
        public BuildingType Family => BuildingType.School;
        public int ConstructionCost => 200;
        public string Name => "University";
        public string Description => "Unlocks more research.";

        public IEnumerable<Requirement> Requirements => new[]
        {
            R.Or(new LocationTypeRequirement(LocationType.Town), new LocationTypeRequirement(LocationType.City)),
            new UpkeepRequirement(6),
            new CostRequirement(50)
        };
    }
}