using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace Scorpia.Assets.Scripts.Map.Render
{
    public class RiverTileRenderer : RiverIndexSelector, ITileRenderer
    {
        private readonly Tilemap layer;
        private readonly IReadOnlyList<Tile> tiles;

        public RiverTileRenderer(Tilemap layer, IReadOnlyList<Tile> tiles) : base()
        {
            this.layer = layer;
            this.tiles = tiles;
        }

        public Tilemap Layer => layer;

        public Tile GetTile(MapTile tile)
        {
            if (tile.River is not null)
            {
                return tiles[GetIndex(tile)];
            }

            return null;
        }
    }
}
