using System;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Engine.SceneManagement.PacketHandlers;

public class SwitchSceneHandler : IPacketHandler
{
    public Type Type => typeof(SwitchScenePacket);
    public NetworkMode Receiver => NetworkMode.Client;

    public void Process(ISyncPacket syncPacket, NetworkedScene networkedScene, ushort senderId, NetworkedSceneManager sceneManager)
    {
        var packet = (SwitchScenePacket) syncPacket;

        sceneManager.Switch(packet.Scene);
    }
}