using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace Map.Render
{
    public class RiverTileRenderer : ITileRenderer
    {
        private readonly IReadOnlyList<Tile> tiles;

        private static readonly Dictionary<Direction, IReadOnlyList<int>> Mappings =
            new()
            {
                [Direction.East] = new[] {0, 1},
                [Direction.NorthEast] = new[] {2, 3, 4},
                [Direction.NorthWest] = new[] {5, 6, 7},
                [Direction.SouthEast] = new[] {8, 9, 10},
                [Direction.SouthWest] = new[] {11, 12, 13},
                [Direction.West] = new[] {14, 15},
                [Direction.East | Direction.SouthEast] = new[] {16, 17},
                [Direction.East | Direction.SouthWest] = new[] {18, 19},
                [Direction.East | Direction.West] = new[] {20, 21, 22},
                [Direction.NorthEast | Direction.East] = new[] {23, 24, 25},
                [Direction.NorthEast | Direction.SouthEast] = new[] {26, 27, 28},
                [Direction.NorthEast | Direction.SouthWest] = new[] {29, 30, 31},
                [Direction.NorthEast | Direction.West] = new[] {32, 33, 34},
                [Direction.NorthWest | Direction.East] = new[] {35, 36, 37},
                [Direction.NorthWest | Direction.NorthEast] = new[] {38, 39},
                [Direction.NorthWest | Direction.SouthEast] = new[] {40, 41, 42},
                [Direction.NorthWest | Direction.SouthWest] = new[] {43, 44, 45},
                [Direction.NorthWest | Direction.West] = new[] {46, 47, 48},
                [Direction.SouthEast | Direction.SouthWest] = new[] {49, 50},
                [Direction.SouthEast | Direction.West] = new[] {51, 52},
                [Direction.SouthWest | Direction.West] = new[] {53, 54}
            };

        public RiverTileRenderer(Tilemap layer, IReadOnlyList<Tile> tiles)
        {
            Layer = layer;
            this.tiles = tiles;
        }

        public Tilemap Layer { get; }

        public Tile GetTile(MapTile tile)
        {
            if (tile.River == null)
            {
                return null;
            }

            var selector = new TileSelector(Mappings);

            var index = selector.GetIndex(tile.River.Value);

            return index == null ? null : tiles[index.Value];
        }
    }
}