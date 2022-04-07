using System;
using UnityEngine;

namespace Map
{
    public class MapTile : IEquatable<MapTile>
    {
        public Biome Biome { get; set; }

        public TileFeature Feature { get; set; } = TileFeature.None;

        public River River { get; set; }

        public Fertility Fertility { get; set; }

        public Resource Resource { get; set; }

        public Vector2Int Position { get; set; }

        public double DistanceTo(MapTile other)
        {
            return Math.Sqrt((Math.Pow(Position.x - other.Position.x, 2) + Math.Pow(Position.y - other.Position.y, 2)));
        }

        public bool Equals(MapTile other)
        {
            return Position == other?.Position;
        }

        public override bool Equals(object obj)
        {
            return obj != null && obj is MapTile other && Position == other.Position;
        }

        public override int GetHashCode() => Position.GetHashCode();

        public override string ToString()
        {
            return Position.ToString();
        }

        public static bool operator ==(MapTile lhs, MapTile rhs)
        {
            if (lhs is null)
            {
                if (rhs is null)
                {
                    return true;
                }

                return false;
            }
            return lhs.Equals(rhs);
        }

        public static bool operator !=(MapTile lhs, MapTile rhs) => !(lhs == rhs);
    }
}