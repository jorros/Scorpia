using System.Collections.Generic;
using Actors;
using Blueprints.Requirements;
using Map;

namespace Blueprints.Buildings
{
    public class RefineryComplexBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.RefineryComplex;
        public BuildingType? Upgrade => null;
        public BuildingType Family => BuildingType.Refinery;
        public int ConstructionCost => 50;
        public string Name => "Refinery Complex";
        public string Description => "Produces more zellos.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            new UpkeepRequirement(8),
            new CostRequirement(30),
            R.Or(new LocationTypeRequirement(LocationType.Town), new LocationTypeRequirement(LocationType.City)),
            new ZellosDepositRequirement()
        };
    }
}