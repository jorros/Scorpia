using System.Collections.Generic;
using Actors.Entities;
using Blueprints.Requirements;

namespace Blueprints.Buildings
{
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
}