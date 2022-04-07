using UnityEngine.Tilemaps;

namespace Map.Render
{
	public interface ITileRenderer
	{
		Tilemap Layer { get; }

		Tile GetTile(MapTile tile);
	}
}
