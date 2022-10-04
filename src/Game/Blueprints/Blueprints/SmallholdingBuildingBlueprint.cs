using Scorpia.Game.Blueprints.Production;
using Scorpia.Game.Blueprints.Requirements;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.Blueprints.Blueprints;

public class SmallholdingBuildingBlueprint : IBuildingBlueprint
{
    public BuildingType Type => BuildingType.Smallholding;
    public BuildingType? Upgrade => BuildingType.Farm;
    public BuildingType Family => BuildingType.Smallholding;
    public int ConstructionCost => 100;
    public string Name => "Smallholding";
    public string Description => "Produces food for own consumption.";

    public IEnumerable<Requirement> Requirements => new Requirement[]
    {
        new UpkeepRequirement(1),
        new CostRequirement(10)
    };

    public IEnumerable<Production.Production> Production => new Production.Production[]
    {
        new FoodProduction(2)
    };

    public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
}