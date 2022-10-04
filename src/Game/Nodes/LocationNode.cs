using Scorpia.Engine.Network.Protocol;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Blueprints;
using Scorpia.Game.Location;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.World;

namespace Scorpia.Game.Nodes;

public class LocationNode : NetworkedNode
{
    public NetworkedVar<LocationType> Type { get; } = new();
    public NetworkedVar<string> Name { get; } = new();
    public NetworkedVar<int> Population { get; } = new();
    public NetworkedVar<int> MaxPopulation { get; } = new();
    public NetworkedVar<int> Garrison { get; } = new();
    public NetworkedVar<BalanceSheet> FoodProduction { get; } = new();
    public NetworkedVar<float> FoodStorage { get; } = new();
    public NetworkedVar<BalanceSheet> Income { get; } = new();
    public NetworkedVar<int> InvestedConstruction { get; } = new();
    public NetworkedVar<bool> IsCapital { get; } = new();
    public NetworkedList<string> Tags { get; } = new();

    public NetworkedList<Building> Buildings { get; } = new();

    public MapTile MapTile { get; private set; }
    
    public Building? GetCurrentConstruction()
    {
        foreach (var building in Buildings)
        {
            if (building.IsBuilding)
            {
                return building;
            }
        }

        return null;
    }

    public Building? GetBuildingByFamily(BuildingType type)
    {
        foreach (var building in MapTile.Location.Buildings)
        {
            if (!BuildingBlueprints.IsSameFamily(building.Type, type))
            {
                continue;
            }

            return building;
        }

        return null;
    }
}