using Scorpia.Game.Blueprints.Production;
using Scorpia.Game.Blueprints.Requirements;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.Blueprints.Blueprints;

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

    public IEnumerable<Production.Production> Production => new Production.Production[]
    {
        new ZellosProduction(2)
    };

    public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
}