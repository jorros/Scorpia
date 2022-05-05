using System.Collections.Generic;
using Actors.Entities;
using UnityEngine.Tilemaps;

namespace Map.Render
{
    public class LocationsRenderer : ITileRenderer
    {
        private readonly IReadOnlyList<Tile> tiles;

        public LocationsRenderer(Tilemap layer, IReadOnlyList<Tile> tiles)
        {
            Layer = layer;
            this.tiles = tiles;
        }

        public Tilemap Layer { get; }

        public Tile GetTile(MapTile tile)
        {
            return tile.Location?.Type.Value switch
            {
                LocationType.Village => tiles[0],
                LocationType.Town => tiles[1],
                LocationType.City => tiles[2],
                LocationType.Outpost => tiles[3],
                LocationType.Fob => tiles[4],
                LocationType.MilitaryBase => tiles[5],
                _ => null
            };
        }
    }
}