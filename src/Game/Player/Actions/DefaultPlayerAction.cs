using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Player.Actions;

public class DefaultPlayerAction : IPlayerAction
{
    public string? Description => null;

    public void LeftClick(MapTile mapTile)
    {
        Game.EventManager.Trigger(nameof(MapNode.SelectTile), mapTile);
    }

    public void RightClick(MapTile mapTile)
    {
        Game.EventManager.Trigger(nameof(MapNode.DeselectTile));
    }

    public void Hover(MapTile mapTile)
    {
    }
}