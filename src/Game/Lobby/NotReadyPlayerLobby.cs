using Scorpia.Game.Player;
using Scorpia.Game.Scenes;

namespace Scorpia.Game.Lobby;

public class NotReadyPlayerLobby : PlayerLobby
{
    private readonly MainMenuScene _scene;
    public override string ConfirmLabel => "Rotob?";
    public override string CancelLabel => "Leave";
    public override bool ShowLobby => true;
    public override bool ShowLogin => false;
    public override bool EnablePlayerSettings => true;

    public NotReadyPlayerLobby(MainMenuScene scene)
    {
        _scene = scene;
    }

    public override void ConfirmAction()
    {
        if (_scene.colorGroup.GetValue() is PlayerColor selected &&
            _scene.factionSelection.GetValue() is PlayerFaction faction)
        {
            _scene.Invoke(nameof(MainMenuScene.ReadyServerRpc), new ReadyPacket {Color = selected, Faction = faction});
        }
    }

    public override void CancelAction()
    {
        _scene.Invoke(nameof(MainMenuScene.LeaveServerRpc));
    }
}