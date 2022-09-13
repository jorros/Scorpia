using Scorpia.Game.Player;
using Scorpia.Game.Scenes;

namespace Scorpia.Game.Lobby;

public class NotReadyPlayerLobby : PlayerLobby
{
    private readonly MainMenuScene _scene;
    public override string ConfirmLabel => "Rotob?";
    public override string CancelLabel => "Leave";
    public override bool ShowLobby => true;
    public override bool EnableNameInput => false;
    public override bool EnableColorSelect => true;

    public NotReadyPlayerLobby(MainMenuScene scene)
    {
        _scene = scene;
    }

    public override void ConfirmAction()
    {
        if (_scene._colourGroup!.GetValue() is PlayerColor selected)
        {
            _scene.Invoke(nameof(MainMenuScene.ReadyServerRpc), (byte) selected);
        }
    }

    public override void CancelAction()
    {
        _scene.Invoke(nameof(MainMenuScene.LeaveServerRpc));
    }
}