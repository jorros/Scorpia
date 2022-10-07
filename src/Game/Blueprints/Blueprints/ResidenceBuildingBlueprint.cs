using Scorpia.Game.Blueprints.Requirements;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.Blueprints.Blueprints;

public class ResidenceBuildingBlueprint : IBuildingBlueprint
{
    public BuildingType Type => BuildingType.Residence;
    public BuildingType? Upgrade => BuildingType.Residence;
    public BuildingType Family => BuildingType.Residence;
    public int ConstructionCost => 100;
    public string Name => "Residences";
    public string Description => "A residence district able to house up to 1000 pop.";

    public IEnumerable<Requirement> Requirements => new Requirement[]
    {
        new CostRequirement(10)
    };

    public IEnumerable<Production.Production> Production => null;

    public IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements =>
        new Dictionary<int, IEnumerable<Requirement>>
        {
            [2] = new Requirement[]
            {
                new LocationTypeRequirement(LocationType.Town)
            },
            [4] = new Requirement[]
            {
                new LocationTypeRequirement(LocationType.City)
            }
        };
}