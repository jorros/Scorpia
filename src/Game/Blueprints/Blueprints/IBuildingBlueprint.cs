using Scorpia.Game.Blueprints.Requirements;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.Blueprints.Blueprints;

public interface IBuildingBlueprint
{
    BuildingType Type { get; }
        
    BuildingType? Upgrade { get; }
        
    BuildingType Family { get; }
        
    int ConstructionCost { get; }

    string Name { get; }
        
    string Description { get; }
        
    IEnumerable<Requirement> Requirements { get; }
        
    IEnumerable<Production.Production> Production { get; }

    IDictionary<int, IEnumerable<Requirement>> AdditionalLevelRequirements { get; }
}