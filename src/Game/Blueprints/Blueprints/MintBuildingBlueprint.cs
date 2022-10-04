using Scorpia.Game.Blueprints.Production;
using Scorpia.Game.Blueprints.Requirements;
using Scorpia.Game.Location;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.Blueprints.Blueprints;

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