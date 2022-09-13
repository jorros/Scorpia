using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Scorpia.Engine.Helper;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Engine.SceneManagement;

public abstract class NetworkedNode : Node
{
    protected uint NetworkId { get; private set; }
    protected NetworkManager NetworkManager => (Scene as NetworkedScene)?.NetworkManager;
    
    private IDictionary<int, MethodBase> ClientRpcs { get; set; }
    private IDictionary<int, MethodBase> ServerRpcs { get; set; }
    
    internal void Create(uint networkId)
    {
        NetworkId = networkId;
        
        ClientRpcs = GetType().GetClientRpcs();
        ServerRpcs = GetType().GetServerRpcs();
        
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

    public void Invoke(string name, ushort clientId = 0)
    {
        Invoke<object>(name, null, clientId);
    }

    public new void Dispose()
    {
        NetworkManager.OnPacketReceive -= OnPacketReceive;
        
        base.Dispose();
    }
}