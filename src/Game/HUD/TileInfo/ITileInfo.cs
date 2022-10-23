using Scorpia.Game.World;

namespace Scorpia.Game.HUD.TileInfo;

public interface ITileInfo
{
    int WindowHeight { get; }
    bool ShouldRender(MapTile tile);
    void Init(MapTile tile);
    void Update(MapTile tile);
}