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

    private readonly Dictionary<uint, Node> _networkedNodes = new();
    private uint _lastNetworkId = 1;

    private IDictionary<int, MethodBase> ClientRpcs { get; set; }
    private IDictionary<int, MethodBase> ServerRpcs { get; set; }

    private IDictionary<int, FieldInfo> NetworkedVars { get; set; }
    private IDictionary<int, FieldInfo> NetworkedLists { get; set; }

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

        _networkedNodes.Add(_lastNetworkId, node);

        _lastNetworkId++;

        return (T)node;
    }

    private void SyncNodes()
    {
        if (NetworkManager.IsServer)
        {
            return;
        }

        foreach (var node in _networkedNodes.Where(node => node.Value is NetworkedNode))
        {
            _networkedNodes.Remove(node.Key);
        }

        NetworkManager.Send(new SyncSceneRequest
        {
            Scene = GetType().Name
        });
    }

    private void SpawnNode(string name, uint id)
    {
        foreach (var node in from nodeType in Settings.NetworkedNodes
                 where nodeType.Name == name
                 select CreateNode(nodeType) as NetworkedNode)
        {
            node?.Create(id);

            _networkedNodes.Add(id, node);
            break;
        }
    }

    private void OnPacketReceive(object sender, DataReceivedEventArgs e)
    {
        switch (e.Data)
        {
            case CreateNodePacket createNodePacket:
            {
                if (createNodePacket.Scene != GetType().Name)
                {
                    break;
                }

                SpawnNode(createNodePacket.Node, createNodePacket.NetworkId);
                break;
            }
            case RemoteCallPacket remoteCallPacket:
            {
                if (remoteCallPacket.NodeId != 0 || remoteCallPacket.Scene != GetType().Name)
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
            case SyncSceneRequest syncSceneRequest:
            {
                if (syncSceneRequest.Scene != GetType().Name || NetworkManager.IsClient)
                {
                    break;
                }

                foreach (var node in _networkedNodes)
                {
                    NetworkManager.Send(new CreateNodePacket
                    {
                        NetworkId = node.Key,
                        Node = node.Value.GetType().Name,
                        Scene = GetType().Name
                    });
                }

                break;
            }
            case SyncVarPacket syncVarPacket:
            {
                if (syncVarPacket.NodeId != 0 || syncVarPacket.Scene != GetType().Name)
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
                if (syncListPacket.NodeId != 0 || syncListPacket.Scene != GetType().Name)
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

        NetworkManager.OnPacketReceive += OnPacketReceive;
        NetworkManager.OnUserConnect += OnUserConnect;

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
        NetworkManager.OnPacketReceive -= OnPacketReceive;
        NetworkManager.OnUserConnect -= OnUserConnect;
        _networkedNodes.Clear();

        base.Dispose();
    }
}