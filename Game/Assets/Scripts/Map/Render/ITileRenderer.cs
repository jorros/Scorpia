using UnityEngine.Tilemaps;

namespace Scorpia.Assets.Scripts.Map.Render
{
	public interface ITileRenderer
	{
		Tilemap Layer { get; }

		Tile GetTile(MapTile tile);
	}
}
