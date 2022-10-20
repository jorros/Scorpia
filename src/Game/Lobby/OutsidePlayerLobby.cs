using Scorpia.Game.Scenes;

namespace Scorpia.Game.Lobby;

public class OutsidePlayerLobby : PlayerLobby
{
    private readonly MainMenuScene _scene;

    public OutsidePlayerLobby(MainMenuScene scene)
    {
        _scene = scene;
    }

    public override string ConfirmLabel => string.Empty;
    public override string CancelLabel => string.Empty;
    public override bool ShowLobby => false;
    public override bool ShowLogin => true;
    public override bool EnablePlayerSettings => false;

    public override void ConfirmAction()
    {
    }

    public override void CancelAction()
    {
    }
}