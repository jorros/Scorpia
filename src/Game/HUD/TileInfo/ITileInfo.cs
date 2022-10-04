using Scorpia.Game.World;

namespace Scorpia.Game.HUD.TileInfo;

public interface ITileInfo
{
    bool ShouldRender(MapTile tile);
    void Render(MapTile tile);
}