using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Protocol;
using Scorpia.Game.Lobby;
using Scorpia.Game.Nodes;
using Scorpia.Game.Player;

namespace Scorpia.Game.Scenes;

public partial class MainMenuScene
{
    protected override void ServerOnLoad()
    {
        NetworkManager.OnUserDisconnect += ServerOnUserDisconnect;
        NetworkManager.OnUserConnect += ServerOnUserConnect;

        PlayerManager = ServiceProvider.GetRequiredService<PlayerManager>();
    }

    private void ServerOnUserDisconnect(object? sender, UserDisconnectedEventArgs e)
    {
        PlayerManager.Remove(e.ClientId);
    }
    
    private void ServerOnUserConnect(object? sender, UserConnectedEventArgs e)
    {
    }

    private void ServerOnTick()
    {
        if (PlayerManager.AllReady())
        {
            SceneManager.Switch(nameof(LoadingScene));
        }
    }

    [ServerRpc]
    public void JoinServerRpc(JoinMatchPacket packet, SenderInfo sender)
    {
        if (PlayerManager.Add(packet.DeviceId, packet.Name, sender.Id))
        {
            Logger.LogInformation("New player {Name} joined with device id {DeviceId}", packet.Name, packet.DeviceId);
            Invoke(nameof(SetLobbyClientRpc), (byte)LobbyEnum.NotReady);

            return;
        }
        
        Logger.LogInformation("Player {Name} with device id {DeviceId} was not allowed to join", packet.Name, packet.DeviceId);
    }

    [ServerRpc]
    public void LeaveServerRpc(SenderInfo sender)
    {
        var playerDevice = PlayerManager.Get(sender.Id)?.DeviceId;

        if (playerDevice is null)
        {
            return;
        }
        
        PlayerManager.Remove(sender.Id);
        Invoke(nameof(SetLobbyClientRpc), (byte)LobbyEnum.Outside);
    }

    [ServerRpc]
    public void ReadyServerRpc(byte color, SenderInfo sender)
    {
        var player = PlayerManager.Get(sender.Id);

        if (player is null)
        {
            return;
        }

        player.Color = (PlayerColor) color;
        
        Players.Add(player);
        
        Invoke(nameof(SetLobbyClientRpc), (byte)LobbyEnum.Ready);
    }

    [ServerRpc]
    public void NotReadyServerRpc(SenderInfo sender)
    {
        var player = PlayerManager.Get(sender.Id);

        if (player is null)
        {
            return;
        }

        player.Color = null;

        Players.Remove(player);
        
        Invoke(nameof(SetLobbyClientRpc), (byte)LobbyEnum.NotReady);
    }
}