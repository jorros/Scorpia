using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Helper;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;
using Scorpia.Engine.Network.Protocol;

namespace Scorpia.Engine.SceneManagement;

public abstract class NetworkedScene : Scene
{
    public NetworkManager NetworkManager { get; private set; }
    public EngineSettings Settings { get; set; }

    internal readonly Dictionary<ulong, NetworkedNode> networkedNodes = new();
    private ulong _lastNetworkId = 1;

    internal IDictionary<int, MethodBase> ClientRpcs { get; set; }
    internal IDictionary<int, MethodBase> ServerRpcs { get; set; }

    internal IDictionary<int, FieldInfo> NetworkedVars { get; set; }
    internal IDictionary<int, FieldInfo> NetworkedLists { get; set; }

    protected T SpawnNode<T>() where T : NetworkedNode
    {
        if (NetworkManager.IsClient)
        {
            return null;
        }

        var node = CreateNode<T>() as NetworkedNode;
        node?.Create(_lastNetworkId);

        NetworkManager.Send(new CreateNodePacket
        {
            Node = typeof(T).Name,
            Scene = GetType().Name,
            NetworkId = _lastNetworkId
        });

        networkedNodes.Add(_lastNetworkId, node);

        _lastNetworkId++;

        return (T)node;
    }

    private void SyncNodes()
    {
        if (NetworkManager.IsServer)
        {
            return;
        }

        foreach (var node in networkedNodes.Where(node => node.Value is NetworkedNode))
        {
            networkedNodes.Remove(node.Key);
        }

        NetworkManager.Send(new SyncSceneRequestPacket
        {
            Scene = GetType().Name
        });
    }

    internal void SpawnNode(string name, ulong id)
    {
        foreach (var node in from nodeType in Settings.NetworkedNodes
                 where nodeType.Name == name
                 select CreateNode(nodeType) as NetworkedNode)
        {
            node?.Create(id);

            networkedNodes.Add(id, node);
            break;
        }
    }

    internal NetworkedNode GetNetworkedNode(ulong id)
    {
        networkedNodes.TryGetValue(id, out var node);

        return node;
    }

    private void OnUserConnect(object sender, UserConnectedEventArgs e)
    {
        SyncNodes();
    }

    internal override void Load(IServiceProvider serviceProvider)
    {
        NetworkManager = serviceProvider.GetRequiredService<NetworkManager>();
        Settings = serviceProvider.GetRequiredService<EngineSettings>();

        ClientRpcs = GetType().GetClientRpcs();
        ServerRpcs = GetType().GetServerRpcs();
        NetworkedVars = GetType().GetNetworkedFields();
        NetworkedLists = GetType().GetNetworkedLists();
        
        NetworkManager.OnUserConnect += OnUserConnect;
        
        SyncNodes();

        base.Load(serviceProvider);

        if (NetworkManager.IsServer)
        {
            ServerOnLoad();
        }
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

            var packet = new SyncVarPacket
            {
                Field = netVar.Key,
                NodeId = 0,
                Scene = GetType().Name,
                Value = change
            };
            
            foreach (var client in NetworkManager.ConnectedClients)
            {
                if (field.shouldReceive?.Invoke(client) != false)
                {
                    NetworkManager.Send(packet, client);
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

                var toBeSend = packet with {Field = netList.Key, NodeId = 0, Scene = GetType().Name};
                
                foreach (var client in NetworkManager.ConnectedClients)
                {
                    if (field.shouldReceive?.Invoke(client) != false)
                    {
                        NetworkManager.Send(toBeSend, client);
                    }
                }

                field.Commit(packet);
            }
        }

        base.Update();
    }

    protected virtual void ServerOnLoad()
    {
    }

    public void Invoke<T>(string name, T args, ushort clientId = 0)
    {
        NetworkManager.Send(new RemoteCallPacket
        {
            Arguments = args,
            Method = name.GetDeterministicHashCode(),
            Scene = GetType().Name,
            NodeId = 0
        }, clientId);
    }

    public void Invoke(string name, ushort clientId = 0)
    {
        Invoke<object>(name, null, clientId);
    }

    public override void Dispose()
    {
        NetworkManager.OnUserConnect -= OnUserConnect;
        networkedNodes.Clear();

        base.Dispose();
    }
}