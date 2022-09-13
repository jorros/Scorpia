namespace Scorpia.Game;

public class GameState
{
    public enum State
    {
        Lobby,
        InGame,
        Stats
    }

    public State Current { get; set; }
}