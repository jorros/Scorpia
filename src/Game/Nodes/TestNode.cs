using Scorpia.Engine.InputManagement;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Protocol;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Game.Nodes;

public class TestNode : NetworkedNode
{
    public override void OnInit()
    {

    }

    public override void OnTick()
    {
    }

    [ServerRpc]
    public void ServerRpc()
    {
        Console.WriteLine("On Server");

        Invoke(nameof(ClientRpc));
    }

    [ClientRpc]
    public void ClientRpc()
    {
        Console.WriteLine("On client");
    }

    public override void OnUpdate()
    {
        if (Input.IsKeyDown(KeyboardKey.A))
        {
            Invoke(nameof(ServerRpc));
        }
    }
}