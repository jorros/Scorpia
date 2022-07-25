using UnityEngine;

namespace Map.Generation
{
    public class InitialGenerator : IGenerator
    {
        public void Generate(Map map)
        {
            for (var x = 0; x < map.Width; x++)
            {
                for (var y = 0; y < map.Height; y++)
                {
                    var tile = new MapTile
                    {
                        Position = new Vector2Int(x, y)
                    };

                    map.SetTile(x, y, tile);
                }
            }

            foreach (var tile in map.Tiles)
            {
                tile.Neighbours = map.GetNeighbours(tile);
            }
        }
    }
}