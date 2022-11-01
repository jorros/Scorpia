using System.Collections.Generic;
using System.Reflection;
using Scorpia.Engine.Helper;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;
using Scorpia.Engine.Network.Protocol;

namespace Scorpia.Engine.SceneManagement;

public abstract class NetworkedNode : Node
{
    protected ulong NetworkId { get; private set; }
    protected NetworkManager NetworkManager => (Scene as NetworkedScene)?.NetworkManager;
    
    internal IDictionary<int, MethodBase> ClientRpcs { get; set; }
    internal IDictionary<int, MethodBase> ServerRpcs { get; set; }
    internal IDictionary<int, FieldInfo> NetworkedVars { get; set; }
    internal IDictionary<int, FieldInfo> NetworkedLists { get; set; }
    
    internal void Create(ulong networkId)
    {
        NetworkId = networkId;
        
        ClientRpcs = GetType().GetClientRpcs();
        ServerRpcs = GetType().GetServerRpcs();
        NetworkedVars = GetType().GetNetworkedFields();
        NetworkedLists = GetType().GetNetworkedLists();
    }
    
    public void Invoke<T>(string name, T args, ushort clientId = 0)
    {
        NetworkManager.Send(new RemoteCallPacket
        {
            Arguments = args,
            Method = name.GetDeterministicHashCode(),
            Scene = Scene.GetType().Name,
            NodeId = NetworkId
        }, clientId);
    }

    internal override void Update()
    {
        if (NetworkManager.IsClient)
        {
            base.Update();

            return;
        }

        foreach (var netVar in NetworkedVars)
        {
            dynamic field = netVar.Value.GetValue(this);

            if (field is null || !field.IsDirty)
            {
                continue;
            }

            var change = field.GetProposedVal();
            field.Accept(change);

            var toBeSent = new SyncVarPacket
            {
                Field = netVar.Key,
                NodeId = NetworkId,
                Scene = Scene.GetType().Name,
                Value = change
            };
            
            foreach (var client in NetworkManager.ConnectedClients)
            {
                if (field.shouldReceive?.Invoke(client) != false)
                {
                    NetworkManager.Send(toBeSent, client);
                }
            }
        }

        foreach (var netList in NetworkedLists)
        {
            dynamic field = netList.Value.GetValue(this);

            if (field is null)
            {
                continue;
            }

            Queue<SyncListPacket> queue = field.packets;
            while (queue.Count > 0)
            {
                var packet = queue.Dequeue();
                NetworkManager.Send(packet with {Field = netList.Key, NodeId = 0, Scene = GetType().Name});
                field.Commit(packet);
            }
        }

        base.Update();
    }

    public void Invoke(string name, ushort clientId = 0)
    {
        Invoke<object>(name, null, clientId);
    }
}