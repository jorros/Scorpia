using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Player.Actions;

namespace Scorpia.Game.Player;

public class CurrentPlayer
{
    private readonly EventManager _eventManager;

    public CurrentPlayer(EventManager eventManager)
    {
        _eventManager = eventManager;

        CurrentAction = new DefaultPlayerAction(eventManager);
    }
    
    public IPlayerAction CurrentAction { get; set; }
}