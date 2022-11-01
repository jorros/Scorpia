using System;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Engine.SceneManagement.PacketHandlers;

public interface IPacketHandler
{
    Type Type { get; }
    
    NetworkMode Receiver { get; }

    void Process(ISyncPacket syncPacket, NetworkedScene networkedScene, ushort senderId, NetworkedSceneManager sceneManager);
}