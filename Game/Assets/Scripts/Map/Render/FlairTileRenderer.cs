using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace Scorpia.Assets.Scripts.Map.Render
{
	public class FlairTileRenderer : ITileRenderer
	{
        private readonly Tilemap layer;
        private readonly IReadOnlyList<Tile> tiles;

        public FlairTileRenderer(Tilemap layer, IReadOnlyList<Tile> tiles)
		{
            this.layer = layer;
            this.tiles = tiles;
        }

        public Tilemap Layer => layer;

        public Tile GetTile(MapTile tile) =>
            tile.Resource switch
            {
                Resource.Gold => tiles[0],
                Resource.Nitra => tiles[1],
                Resource.Sofrum => tiles[2],
                Resource.Zellos => tiles[3],
                _ => null
            };
    }
}