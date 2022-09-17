using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Protocol;
using Scorpia.Game.Player;

namespace Scorpia.Game.Scenes;

public partial class LoadingScene
{
    private PlayerManager _playerManager;
    
    protected override void ServerOnLoad()
    {
        SceneManager.Load<GameScene>();

        _playerManager = ServiceProvider.GetRequiredService<PlayerManager>();
    }

    [ServerRpc]
    protected void UpdateProgressServerRpc(byte progress, SenderInfo sender)
    {
        var player = _playerManager.Get(sender.Id);
        if (player is not null)
        {
            player.LoadingProgress = progress;
        }
    }

    protected void ServerOnTick()
    {
        if (_playerManager.AllLoaded())
        {
            SceneManager.Switch(nameof(GameScene));
        }
    }
}