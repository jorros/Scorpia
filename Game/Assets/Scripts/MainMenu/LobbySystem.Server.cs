using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scorpia.Assets.Scripts.MainMenu
{
    public partial class LobbySystem
    {
        private PlayerInfo players;

        private void StartServer()
        {
            var sendToAllParams = new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = NetworkManager.Singleton.ConnectedClientsIds
                }
            };
            players = new PlayerInfo();
            players.OnUpdate += (info) =>
            {
                UpdatePlayerInfoClientRpc(info, sendToAllParams);
            };

            NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnectServer;
            NetworkManager.Singleton.SceneManager.OnLoadComplete += LoadingCompletedServer;
        }

        private void LoadingCompletedServer(ulong clientId, string sceneName, LoadSceneMode loadSceneMode)
        {
            if (!players.Exists(clientId))
            {
                return;
            }

            var player = players[clientId];
            print($"[{clientId}]{player.Name} completed loading");

            player.FinishedLoading = true;
        }

        [ServerRpc(RequireOwnership = false)]
        private void JoinServerRpc(string name, PlayerColour colour, ServerRpcParams serverRpcParams = default)
        {
            var senderId = serverRpcParams.Receive.SenderClientId;
            print($"[{senderId}]{name} joined match");

            var playerInfo = new PlayerInfo.PlayerDetail
            {
                Name = name,
                Colour = colour
            };
            players.Add(senderId, playerInfo);
        }

        [ServerRpc(RequireOwnership = false)]
        private void LeaveServerRpc(ServerRpcParams serverRpcParams = default)
        {
            var senderId = serverRpcParams.Receive.SenderClientId;
            PlayerLeft(senderId);
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetStatusServerRpc(bool isReady, ServerRpcParams serverRpcParams = default)
        {
            var senderId = serverRpcParams.Receive.SenderClientId;

            var player = players[senderId];
            print($"[{senderId}]{player.Name} is ready: {isReady}");

            players.SetReady(senderId, isReady);

            if (players.AreReady && players.HasDistinctColours())
            {
                var timeToStart = Application.isEditor ? 0 : 3;
                Invoke(nameof(TriggerLoadingServer), timeToStart);
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void SetColourServerRpc(PlayerColour colour, ServerRpcParams serverRpcParams = default)
        {
            var senderId = serverRpcParams.Receive.SenderClientId;

            players.SetColour(senderId, colour);
        }

        private void PlayerLeft(ulong senderId)
        {
            if (players.Exists(senderId))
            {
                var player = players[senderId];
                print($"[{senderId}]{player.Name} left match");

                players.Remove(senderId);
            }
        }

        private void OnDisconnectServer(ulong senderId)
        {
            PlayerLeft(senderId);
        }

        private void TriggerLoadingServer()
        {
            if (players.AreReady)
            {
                var loading = NetworkManager.Singleton.SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
            }
        }
    }
}