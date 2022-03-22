using Unity.Netcode;

namespace Scorpia.Assets.Scripts.MainMenu
{
    public partial class LobbySystem : NetworkBehaviour
    {
        private void Start()
        {
            if (IsServer)
            {
                StartServer();
            }
            else
            {
                StartClient();
            }
        }
    }
}

