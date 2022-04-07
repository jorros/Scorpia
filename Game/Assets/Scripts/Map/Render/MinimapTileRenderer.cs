using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace Map.Render
{
    public class MinimapTileRenderer : ITileRenderer
    {
        private readonly Tilemap layer;
        private readonly IReadOnlyList<Tile> tiles;

        public MinimapTileRenderer(Tilemap layer, IReadOnlyList<Tile> tiles)
        {
            this.layer = layer;
            this.tiles = tiles;
        }

        public Tilemap Layer => layer;

        public Tile GetTile(MapTile tile) =>
            tile switch
            {
                { Biome: Biome.Water } => tiles[0],
                { Biome: Biome.Grass, River: not null } => tiles[3],
                { Biome: Biome.Grass, Feature: TileFeature.Forest } => tiles[2],
                { Biome: Biome.Mountain } => tiles[4],
                _ => tiles[1]
            };
    }
}

