using System.Collections.Generic;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;
using Scorpia.Engine.SceneManagement.PacketHandlers;

namespace Scorpia.Engine.SceneManagement;

public class ScenePacketManager
{
    private readonly IEnumerable<IPacketHandler> _handlers;
    private readonly NetworkManager _networkManager;

    public ScenePacketManager(IEnumerable<IPacketHandler> handlers, NetworkManager networkManager)
    {
        _handlers = handlers;
        _networkManager = networkManager;
    }

    internal void Process(ISyncPacket packet, NetworkedSceneManager sceneManager, ushort senderId)
    {
        var scene = sceneManager.GetLoadedScene(packet.Scene) as NetworkedScene;
        
        foreach (var handler in _handlers)
        {
            if (handler.Type != packet.GetType())
            {
                continue;
            }
            if (handler.Receiver.HasFlag(NetworkMode.Client) && _networkManager.IsClient || handler.Receiver.HasFlag(NetworkMode.Server) && _networkManager.IsServer)
            {
                handler.Process(packet, scene, senderId, sceneManager);
            }
        }
    }
}