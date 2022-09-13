namespace Scorpia.Game.Lobby;

using Scorpia.Game.Player;
using Scorpia.Game.Scenes;

public class ReadyPlayerLobby : PlayerLobby
{
    private readonly MainMenuScene _scene;
    public override string ConfirmLabel => "Ackshually";
    public override string CancelLabel => "Leave";
    public override bool ShowLobby => true;
    public override bool EnableNameInput => false;
    public override bool EnableColorSelect => false;

    public ReadyPlayerLobby(MainMenuScene scene)
    {
        _scene = scene;
    }

    public override void ConfirmAction()
    {
        _scene.Invoke(nameof(MainMenuScene.NotReadyServerRpc));
    }

    public override void CancelAction()
    {
        _scene.Invoke(nameof(MainMenuScene.LeaveServerRpc));
    }
}