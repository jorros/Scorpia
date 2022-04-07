using Unity.Netcode;

namespace MainMenu
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

