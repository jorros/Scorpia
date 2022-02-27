using UnityEngine;

namespace Scorpia.Assets.Scripts.Utils
{
    public static class VectorExtensions
    {
        public static Vector3Int ToCube(this Vector2Int position)
        {
            var q = position.x - (position.y - (position.y & 1)) / 2;
            var r = position.y;
            return new Vector3Int(q, r, -q - r);
        }
    }
}