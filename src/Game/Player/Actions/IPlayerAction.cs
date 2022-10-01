using Scorpia.Game.World;

namespace Scorpia.Game.Player.Actions;

public interface IPlayerAction
{
    string? Description { get; }
        
    void LeftClick(MapTile mapTile);

    void RightClick(MapTile? mapTile);

    void Hover(MapTile mapTile);
}