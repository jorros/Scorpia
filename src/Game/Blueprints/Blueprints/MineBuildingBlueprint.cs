using Scorpia.Game.Blueprints.Production;
using Scorpia.Game.Blueprints.Requirements;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.Blueprints.Blueprints;

public class MineBuildingBlueprint : IBuildingBlueprint
{
    public BuildingType Type => BuildingType.Mine;
    public BuildingType? Upgrade => BuildingType.DeepMine;
    public BuildingType Family => BuildingType.Mine;
    public int ConstructionCost => 100;
    public string Name => "Mine";
    public string Description => "This mine produces either nitra or sofrum.";

    public IEnumerable<Requirement> Requirements => new Requirement[]
    {
        new UpkeepRequirement(4),
        new CostRequirement(20),
        R.Or(new NitraDepositRequirement(), new SofrumDepositRequirement())
    };

    public IEnumerable<Production.Production> Production => new Production.Production[]
    {
        new NitraProduction(2),
        new SofrumProduction(2)
    };

    public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
}