using Scorpia.Game.Scenes;

namespace Scorpia.Game.Lobby;

public class OutsidePlayerLobby : PlayerLobby
{
    private readonly MainMenuScene _scene;

    public OutsidePlayerLobby(MainMenuScene scene)
    {
        _scene = scene;
    }
    
    public override string ConfirmLabel => "Join";
    public override string CancelLabel => "Quit";
    public override bool ShowLobby => false;
    public override bool EnableNameInput => true;
    public override bool EnableColorSelect => false;

    public override void ConfirmAction()
    {
        if (!string.IsNullOrWhiteSpace(_scene._nameInput!.Text))
        {
            Game.ScorpiaSettings.PlayerName = _scene._nameInput!.Text;
            
            _scene.Invoke(nameof(MainMenuScene.JoinServerRpc), new JoinMatchPacket
            {
                Name = _scene._nameInput!.Text,
                DeviceId = Game.ServerPlayerManager.GetDeviceId()
            });
        }
    }

    public override void CancelAction()
    {
        _scene.SceneManager.Quit();
    }
}