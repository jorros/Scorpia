using System;
using System.Collections.Generic;
using Actors;
using UnityEngine;
using Utils;

namespace Map
{
    public class MapTile : IEquatable<MapTile>
    {
        public MapTile(Vector2Int position)
        {
            Position = position;
            features = new HashSet<MapTileFeature>();
        }
        
        public Biome Biome { get; set; }

        private readonly HashSet<MapTileFeature> features;

        public IEnumerable<MapTileFeature> Features => features;

        public IReadOnlyList<MapTile> Neighbours { get; set; }

        public Fertility Fertility { get; set; }

        public Resource Resource { get; set; }
        
        public Direction? River { get; set; }

        public Vector2Int Position { get; }

        public Vector3Int HexPosition
        {
            get
            {
                var q = Position.x - (Position.y - (Position.y & 1)) / 2;
                var r = Position.y;

                return new Vector3Int(q, r, -q - r);
            }
        }

        public Location Location { get; set; }

        public bool HasFeature(MapTileFeature feature) => features.Contains(feature);

        public void AddFeature(MapTileFeature feature) => features.Add(feature);
        
        public Direction? GetDirection(MapTile to)
        {
            var vectors = new [] { new Vector3Int(1, -1, 0), new Vector3Int(1, 0, -1), new Vector3Int(0, 1, -1), new Vector3Int(-1, 1, 0), new Vector3Int(-1, 0, 1), new Vector3Int(0, -1, 1) };
            var directions = new[] { Direction.SouthEast, Direction.East, Direction.NorthEast, Direction.NorthWest, Direction.West, Direction.SouthWest };

            var vector = to.Position.ToCube() - Position.ToCube();
            var index = -1;

            for(var i = 0; i < vectors.Length; i++)
            {
                if(vectors[i] == vector)
                {
                    index = i;
                    break;
                }
            }

            if(index < 0)
            {
                return null;
            }

            return directions[index];
        }

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
            if (lhs is not null)
            {
                return lhs.Equals(rhs);
            }
            
            return rhs is null;
        }

        public static bool operator !=(MapTile lhs, MapTile rhs) => !(lhs == rhs);
    }
}