namespace Scorpia.Game.Lobby;

public abstract class PlayerLobby
{
    public abstract string ConfirmLabel { get; }
    public abstract string CancelLabel { get; }
    public abstract bool ShowLobby { get; }
    
    public abstract bool EnableNameInput { get; }
    public abstract bool EnableColorSelect { get; }

    public abstract void ConfirmAction();
    public abstract void CancelAction();
}