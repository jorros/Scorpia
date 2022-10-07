using Scorpia.Game.Blueprints.Production;
using Scorpia.Game.Blueprints.Requirements;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Blueprints;

public class FarmBuildingBlueprint : IBuildingBlueprint
{
    public BuildingType Type => BuildingType.Farm;
    public BuildingType? Upgrade => BuildingType.Estate;
    public BuildingType Family => BuildingType.Smallholding;
    public int ConstructionCost => 200;
    public string Name => "Farm";
    public string Description => "Produces food required for higher demand.";

    public IEnumerable<Requirement> Requirements => new Requirement[]
    {
        R.Or(new LocationTypeRequirement(LocationType.Town), new LocationTypeRequirement(LocationType.City)),
        R.Or(new FertilityRequirement(Fertility.Normal), new FertilityRequirement(Fertility.High)),
        new UpkeepRequirement(3),
        new CostRequirement(20)
    };

    public IEnumerable<Production.Production> Production => new Production.Production[]
    {
        new FoodProduction(4)
    };

    public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
}