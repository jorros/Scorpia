using System.Collections.Generic;
using Actors.Entities;
using Blueprints.Production;
using Blueprints.Requirements;

namespace Blueprints.Buildings
{
    public class MintBuildingBlueprint : IBuildingBlueprint
    {
        public BuildingType Type => BuildingType.Mint;
        public BuildingType? Upgrade => null;
        public BuildingType Family => BuildingType.Mint;
        public int ConstructionCost => 60;
        public string Name => "Mint";

        public string Description =>
            "Scorpions made from this mint will be added to your income. This will increase inflation.";

        public IEnumerable<Requirement> Requirements => new Requirement[]
        {
            R.Or(new LocationTypeRequirement(LocationType.Town), new LocationTypeRequirement(LocationType.City)),
            new GoldDepositRequirement(),
            new CostRequirement(30)
        };

        public IEnumerable<Production.Production> Production => new Production.Production[]
        {
            new ScorpionProduction(10)
        };

        public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
    }
}