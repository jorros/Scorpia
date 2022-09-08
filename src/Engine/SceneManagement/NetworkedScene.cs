using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Helper;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Engine.SceneManagement;

public abstract class NetworkedScene : Scene
{
    public NetworkManager NetworkManager { get; private set; }
    private EngineSettings Settings { get; set; }

    private readonly Dictionary<uint, Node> _nodes = new();
    private uint _lastNetworkId;

    internal IDictionary<int, MethodBase> ClientRpcs { get; private set; }
    internal IDictionary<int, MethodBase> ServerRpcs { get; private set; }

    protected NetworkedNode SpawnNode<T>() where T : NetworkedNode
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

        _nodes.Add(_lastNetworkId, node);

        _lastNetworkId++;

        return node;
    }

    internal void SpawnNode(string name, uint id)
    {
        foreach (var node in from nodeType in Settings.NetworkedNodes
                 where nodeType.Name == name
                 select CreateNode(nodeType) as NetworkedNode)
        {
            node?.Create(id);

            _nodes.Add(id, node);
            break;
        }
    }

    internal new void Load(IServiceProvider serviceProvider)
    {
        NetworkManager = serviceProvider.GetRequiredService<NetworkManager>();
        Settings = serviceProvider.GetRequiredService<EngineSettings>();

        ClientRpcs = new Dictionary<int, MethodBase>();
        foreach (var method in GetType().GetRuntimeMethods().Where(x => Attribute.IsDefined(x, typeof(ClientRpcAttribute))))
        {
            if (method.GetParameters().Length > 1)
            {
                throw new EngineException($"Malformed RPC method: {method.Name} on {GetType().Name}");
            }
            
            ClientRpcs.Add(method.Name.GetDeterministicHashCode(), method);
        }
        
        ServerRpcs = new Dictionary<int, MethodBase>();
        foreach (var method in GetType().GetRuntimeMethods().Where(x => Attribute.IsDefined(x, typeof(ServerRpcAttribute))))
        {
            if (method.GetParameters().Length > 2)
            {
                throw new EngineException($"Malformed RPC method: {method.Name} on {GetType().Name}");
            }
            
            ServerRpcs.Add(method.Name.GetDeterministicHashCode(), method);
        }

        base.Load(serviceProvider);
    }

    public void Invoke<T>(string name, T args, ushort clientId = 0) where T : INetworkPacket
    {
        var networkedMgr = (NetworkedSceneManager) SceneManager;
        
        networkedMgr.InvokeRpc(this, null, name.GetDeterministicHashCode(), args, clientId);
    }

    public void Invoke(string name, ushort clientId = 0)
    {
        var networkedMgr = (NetworkedSceneManager) SceneManager;
        
        networkedMgr.InvokeRpc(this, null, name.GetDeterministicHashCode(), (INetworkPacket)null, clientId);
    }
}