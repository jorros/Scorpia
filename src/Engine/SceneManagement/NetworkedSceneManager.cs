using System;
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
        _networkManager.OnPacketReceive += OnPacketReceive;
    }

    private void OnPacketReceive(object sender, DataReceivedEventArgs e)
    {
        switch (e.Data)
        {
            case SwitchScenePacket switchScenePacket:
                SwitchInternally(switchScenePacket.Name);
                break;
        }
    }

    public override T Load<T>()
    {
        var scene = Activator.CreateInstance(typeof(T), true) as NetworkedScene;
        scene?.Load(_serviceProvider);

        loadedScenes.Add(typeof(T).Name, scene);

        var baseScene = (Scene) scene;

        return (T) baseScene;
    }

    public override void Switch(string scene, bool unloadCurrent = true)
    {
        if (_networkManager.IsClient)
        {
            base.Switch(scene);

            return;
        }

        _logger.LogInformation("Force all clients to switch scene to {Scene}", scene);

        _networkManager.Send(new SwitchScenePacket
        {
            Name = scene
        });

        base.Switch(scene, unloadCurrent);
    }

    private void SwitchInternally(string scene)
    {
        if (_networkManager.IsServer)
        {
            return;
        }

        _logger.LogInformation("Server switches scene to {Scene}", scene);

        base.Switch(scene);
    }

    internal override void Update()
    {
        (currentScene as NetworkedScene)?.Update();
    }
}