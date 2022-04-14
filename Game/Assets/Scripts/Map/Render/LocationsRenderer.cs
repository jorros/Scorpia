using System.Collections.Generic;
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
            return tile.Location?.Type switch
            {
                MapLocation.LocationType.Village => tiles[0],
                MapLocation.LocationType.Town => tiles[1],
                MapLocation.LocationType.City => tiles[2],
                MapLocation.LocationType.Outpost => tiles[3],
                MapLocation.LocationType.Fob => tiles[4],
                MapLocation.LocationType.MilitaryBase => tiles[5],
                _ => null
            };
        }
    }
}