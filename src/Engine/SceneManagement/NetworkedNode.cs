using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Scorpia.Engine.Helper;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;
using Scorpia.Engine.Network.Protocol;

namespace Scorpia.Engine.SceneManagement;

public abstract class NetworkedNode : Node
{
    protected uint NetworkId { get; private set; }
    protected NetworkManager NetworkManager => (Scene as NetworkedScene)?.NetworkManager;
    
    private IDictionary<int, MethodBase> ClientRpcs { get; set; }
    private IDictionary<int, MethodBase> ServerRpcs { get; set; }
    private IDictionary<int, FieldInfo> NetworkedVars { get; set; }
    private IDictionary<int, FieldInfo> NetworkedLists { get; set; }
    
    internal void Create(uint networkId)
    {
        NetworkId = networkId;
        
        ClientRpcs = GetType().GetClientRpcs();
        ServerRpcs = GetType().GetServerRpcs();
        NetworkedVars = GetType().GetNetworkedFields();
        NetworkedLists = GetType().GetNetworkedLists();
        
        NetworkManager.OnPacketReceive += OnPacketReceive;
    }

    private void OnPacketReceive(object sender, DataReceivedEventArgs e)
    {
        switch (e.Data)
        {
            case RemoteCallPacket remoteCallPacket:
            {
                if (remoteCallPacket.NodeId != NetworkId || remoteCallPacket.Scene != Scene.GetType().Name)
                {
                    break;
                }
                
                var method = NetworkManager.IsClient
                    ? ClientRpcs[remoteCallPacket.Method]
                    : ServerRpcs[remoteCallPacket.Method];

                var onlySenderInfo = method.GetParameters().FirstOrDefault()?.ParameterType == typeof(SenderInfo);

                var args = method.GetParameters().Length switch
                {
                    0 => null,
                    1 => onlySenderInfo
                        ? new object[] {new SenderInfo(e.SenderId)}
                        : new[] {remoteCallPacket.Arguments},
                    2 => new[] {remoteCallPacket.Arguments, new SenderInfo(e.SenderId)}
                };

                method.Invoke(this, args);
                break;
            }
            case SyncVarPacket syncVarPacket:
            {
                if (syncVarPacket.NodeId != NetworkId || syncVarPacket.Scene != Scene.GetType().Name)
                {
                    break;
                }

                var field = NetworkedVars[syncVarPacket.Field];
                dynamic netVar = field.GetValue(this);
                netVar.Accept(syncVarPacket.Value);

                break;
            }
            case SyncListPacket syncListPacket:
            {
                if (syncListPacket.NodeId != NetworkId || syncListPacket.Scene != Scene.GetType().Name)
                {
                    break;
                }
                
                var field = NetworkedLists[syncListPacket.Field];
                dynamic netList = field.GetValue(this);

                netList.Commit(syncListPacket);
                netList.packets.Clear();

                break;
            }
        }
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
            NetworkManager.Send(new SyncVarPacket
            {
                Field = netVar.Key,
                NodeId = 0,
                Scene = GetType().Name,
                Value = change
            });
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

    public override void Dispose()
    {
        NetworkManager.OnPacketReceive -= OnPacketReceive;
        
        base.Dispose();
    }
}