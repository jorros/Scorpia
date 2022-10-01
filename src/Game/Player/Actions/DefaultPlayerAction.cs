using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Player.Actions;

public class DefaultPlayerAction : IPlayerAction
{
    private readonly EventManager _eventManager;

    public DefaultPlayerAction(EventManager eventManager)
    {
        _eventManager = eventManager;
    }
    
    public string? Description => null;

    public void LeftClick(MapTile mapTile)
    {
        _eventManager.Trigger(nameof(MapNode.SelectTile), mapTile);
    }

    public void RightClick(MapTile mapTile)
    {
        _eventManager.Trigger(nameof(MapNode.DeselectTile));
    }

    public void Hover(MapTile mapTile)
    {
    }
}