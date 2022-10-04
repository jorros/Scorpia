using Scorpia.Game.Blueprints.Production;
using Scorpia.Game.Blueprints.Requirements;
using Scorpia.Game.Location;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.Blueprints.Blueprints;

public class DeepMineBuildingBlueprint : IBuildingBlueprint
{
    public BuildingType Type => BuildingType.DeepMine;
    public BuildingType? Upgrade => null;
    public BuildingType Family => BuildingType.Mine;
    public int ConstructionCost => 200;
    public string Name => "Deep Mine";
    public string Description => "This deep mine produces more of either nitra or sofrum.";

    public IEnumerable<Requirement> Requirements => new Requirement[]
    {
        R.Or(new LocationTypeRequirement(LocationType.Town), new LocationTypeRequirement(LocationType.City)),
        R.Or(new NitraDepositRequirement(), new SofrumDepositRequirement()),
        new UpkeepRequirement(8),
        new CostRequirement(40)
    };

    public IEnumerable<Production.Production> Production => new Production.Production[]
    {
        new NitraProduction(4),
        new SofrumProduction(4)
    };

    public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements => null;
}