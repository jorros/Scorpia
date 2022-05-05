using Actors.Entities;

namespace Blueprints
{
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
    }
}