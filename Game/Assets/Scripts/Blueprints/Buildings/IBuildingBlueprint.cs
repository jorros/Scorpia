using System.Collections.Generic;
using Actors;
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
    }
}