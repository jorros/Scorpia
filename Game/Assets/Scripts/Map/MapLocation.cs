using System;
using Unity.Collections;

namespace Map
{
    public struct MapLocation : IEquatable<MapLocation>
    {
        public enum LocationType
        {
            Village,
            Town,
            City,
            Outpost,
            Fob,
            MilitaryBase
        }

        public LocationType Type;

        public FixedString64Bytes Name;

        public FixedString64Bytes Player;

        public int Population;

        public bool Equals(MapLocation other)
        {
            return Type == other.Type && Name.Equals(other.Name) && Player.Equals(other.Player);
        }

        public override bool Equals(object obj)
        {
            return obj is MapLocation other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine((int) Type, Name, Player);
        }
    }
}