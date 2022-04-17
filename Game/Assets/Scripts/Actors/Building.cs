using System;

namespace Actors
{
    public struct Building : IEquatable<Building>
    {
        public BuildingType Type { get; set; }

        public bool IsBuilding { get; set; }
        
        public int Level { get; set; }
        
        public bool Equals(Building other)
        {
            return Type == other.Type;
        }
    }
}