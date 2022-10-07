using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Protocol;

namespace Scorpia.Game.Scenes;

public partial class LoadingScene
{
    protected override void ServerOnLoad()
    {
        Seed.Value = 5213351;
    }

    [ServerRpc]
    protected void UpdateProgressServerRpc(byte progress, SenderInfo sender)
    {
        var player = Game.ServerPlayerManager.Get(sender.Id);
        if (player is not null)
        {
            player.LoadingProgress = progress;
        }
    }

    private void ServerOnTick()
    {
        if (Game.ServerPlayerManager.AllLoaded())
        {
            var scene = SceneManager.Load<GameScene>();
            scene.InitMap(Seed.Value, false);
            SceneManager.Switch(nameof(GameScene));
        }
    }
}