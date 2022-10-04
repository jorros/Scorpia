using Scorpia.Game.Location;
using Scorpia.Game.Nodes;

namespace Scorpia.Game.Blueprints;

public static class LocationBlueprint
{
    public static int GetMaxSlots(LocationType type) =>
        type switch
        {
            LocationType.Village => 2,
            LocationType.Town => 4,
            LocationType.City => 6,
            _ => 0
        };

    public static int GetMaxPopulation(LocationType type) =>
        type switch
        {
            LocationType.Village => 1000,
            LocationType.Town => 5000,
            LocationType.City => 10000,
            _ => 0
        };

    public static int GetMaxFoodStorage(LocationType type) =>
        type switch
        {
            LocationType.Village => 50,
            LocationType.Town => 200,
            LocationType.City => 500,
            _ => 0
        };

    public static int GetViewDistance(LocationNode location) =>
        location switch
        {
            {Type: {Value: LocationType.Village}} => 2,
            {Type: {Value: LocationType.Town}} => 3,
            {Type: {Value: LocationType.City}} => 4,
            {Type: {Value: LocationType.Outpost}} => 6,
            {Type: {Value: LocationType.Fob}} => 6,
            {Type: {Value: LocationType.MilitaryBase}} => 6,
            _ => 0
        };
}