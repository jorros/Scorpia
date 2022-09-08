using System;
using System.Linq;
using Microsoft.Extensions.Logging;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Engine.SceneManagement;

public class NetworkedSceneManager : DefaultSceneManager
{
    private readonly IServiceProvider _serviceProvider;
    private readonly NetworkManager _networkManager;
    private readonly ILogger<NetworkedSceneManager> _logger;

    public NetworkedSceneManager(IServiceProvider serviceProvider, NetworkManager networkManager,
        ILogger<NetworkedSceneManager> logger) : base(
        serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _networkManager = networkManager;
        _logger = logger;
        _networkManager.OnPacketReceive += NetworkManagerOnOnPacketReceive;
    }

    private void NetworkManagerOnOnPacketReceive(object sender, PacketReceivedEventArgs e)
    {
        switch (e.Packet)
        {
            case SwitchScenePacket switchScenePacket:
                SwitchInternally(switchScenePacket.Name);
                break;
            case CreateNodePacket createNodePacket:
            {
                var scene = GetScene(createNodePacket.Scene);
                scene.SpawnNode(createNodePacket.Node, createNodePacket.NetworkId);
                break;
            }
            case RemoteCallPacket remoteCallPacket:
            {
                if (remoteCallPacket.NodeId == 0)
                {
                    var scene = GetScene(remoteCallPacket.Scene);
                    var method = _networkManager.IsClient
                        ? scene.ClientRpcs[remoteCallPacket.Method]
                        : scene.ServerRpcs[remoteCallPacket.Method];

                    var onlySenderId = method.GetParameters().FirstOrDefault()?.ParameterType == typeof(ushort);

                    var args = method.GetParameters().Length switch
                    {
                        0 => null,
                        1 => onlySenderId ? new object[] {e.SenderId} : new object[] {remoteCallPacket.Arguments},
                        2 => new object[] {remoteCallPacket.Arguments, e.SenderId}
                    };

                    // var args = method.GetParameters().Length == 1 ? new object[] {remoteCallPacket.Arguments} : null;
                    method.Invoke(scene, args);
                }

                break;
            }
        }
    }

    internal void InvokeRpc<T>(NetworkedScene scene, NetworkedNode node, int method, T args, ushort clientId = 0)
        where T : INetworkPacket
    {
        ulong nodeId = 0;

        if (node is not null)
        {
            nodeId = scene.Nodes.First(x => x.Value == node).Key;
        }

        _networkManager.Send(new RemoteCallPacket
        {
            Arguments = args,
            Method = method,
            Scene = scene.GetType().Name,
            NodeId = nodeId
        }, clientId);
    }

    public override void Load<T>()
    {
        var scene = Activator.CreateInstance(typeof(T), true) as NetworkedScene;
        scene?.Load(_serviceProvider);

        loadedScenes.Add(typeof(T).Name, scene);
    }

    public override void Switch(string scene)
    {
        if (_networkManager.IsClient)
        {
            base.Switch(scene);

            return;
        }

        _networkManager.Send(new SwitchScenePacket
        {
            Name = scene
        });

        base.Switch(scene);
    }

    private void SwitchInternally(string scene)
    {
        if (_networkManager.IsServer)
        {
            return;
        }

        base.Switch(scene);
    }

    private NetworkedScene GetScene(string name)
    {
        return loadedScenes[name] as NetworkedScene;
    }
}