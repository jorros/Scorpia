using Map;

namespace UI.TileInfo
{
    public interface ITileInfo
    {
        bool ShouldRender(MapTile tile);
        void Render(MapTile tile);
    }
}