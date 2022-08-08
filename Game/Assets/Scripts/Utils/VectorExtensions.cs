using System;
using UnityEngine;

namespace Utils
{
    public static class VectorExtensions
    {
        public static Vector3Int ToCube(this Vector2Int position)
        {
            var q = position.x - (position.y - (position.y & 1)) / 2;
            var r = position.y;
            return new Vector3Int(q, r, -q - r);
        }

        public static Vector2Int ToOffset(this Vector3Int position)
        {
            return new Vector2Int(position.x + (position.y - (position.y & 1)) / 2, position.y);
        }

        public static Vector3Int AngleToVector(this double angle) => angle switch
        {
            > 330 => new Vector3Int(1, 0, -1),
            > 270 => new Vector3Int(1, -1, 0),
            > 210 => new Vector3Int(0, -1, 1),
            > 150 => new Vector3Int(-1, 0, 1),
            > 90 => new Vector3Int(-1, +1, 0),
            > 30 => new Vector3Int(0, +1, -1),
            _ => new Vector3Int(1, 0, -1)
        };

        public static double DistanceTo(this Vector3Int from, Vector3Int to)
        {
            var vector = to - from;

            return (Math.Abs(vector.x) + Math.Abs(vector.y) + Math.Abs(vector.z)) / 2;
        }
    }
}