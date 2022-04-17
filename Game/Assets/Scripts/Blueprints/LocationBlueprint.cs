using Actors;

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
    }
}