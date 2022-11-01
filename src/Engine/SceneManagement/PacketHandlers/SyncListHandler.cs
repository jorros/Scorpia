using System;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Engine.SceneManagement.PacketHandlers;

public class SyncListHandler : IPacketHandler
{
    public Type Type => typeof(SyncListPacket);
    public NetworkMode Receiver => NetworkMode.Client | NetworkMode.Server;
    
    public void Process(ISyncPacket syncPacket, NetworkedScene networkedScene, ushort senderId, NetworkedSceneManager sceneManager)
    {
        if (networkedScene is null)
        {
            return;
        }
        
        var packet = (SyncListPacket) syncPacket;
        NetworkedNode node = null;

        if (packet.NodeId > 0)
        {
            node = networkedScene.GetNetworkedNode(packet.NodeId);
        }

        if (node is null)
        {
            ProcessScene(packet, networkedScene);
            
            return;
        }
        
        ProcessNode(packet, node);
    }

    private void ProcessNode(SyncListPacket packet, NetworkedNode node)
    {
        var field = node.NetworkedLists[packet.Field];
        dynamic netList = field.GetValue(node);

        netList.Commit(packet);
        netList.packets.Clear();
    }
    
    private void ProcessScene(SyncListPacket packet, NetworkedScene scene)
    {
        var field = scene.NetworkedLists[packet.Field];
        dynamic netList = field.GetValue(scene);

        netList.Commit(packet);
        netList.packets.Clear();
    }
}