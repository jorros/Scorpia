using Scorpia.Game.Nodes;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.Blueprints;

public static class LocationBlueprint
{
    public static int GetMaxSlots(LocationType type) =>
        type switch
        {
            LocationType.Village => 2,
            LocationType.City => 4,
            _ => 0
        };

    public static int GetMaxPopulation(LocationType type) =>
        type switch
        {
            LocationType.Village => 1000,
            LocationType.City => 10000,
            _ => 0
        };

    public static int GetMaxFoodStorage(LocationType type) =>
        type switch
        {
            LocationType.Village => 50,
            LocationType.City => 500,
            _ => 0
        };

    public static int GetViewDistance(LocationNode location) =>
        location switch
        {
            {Type.Value: (byte)LocationType.Village} => 2,
            {Type.Value: (byte)LocationType.City} => 4,
            {Type.Value: (byte)LocationType.Outpost} => 6,
            _ => 0
        };
}