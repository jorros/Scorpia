using Scorpia.Assets.Scripts.Actors;
using Scorpia.Assets.Scripts.Server;
using Unity.Netcode;
using UnityEngine;

namespace Scorpia.Assets.Scripts.World
{
    public class WorldManager : NetworkBehaviour
    {
        [SerializeField]
        public int Coins { get; set; }

        [SerializeField]
        private GameObject mapPrefab;

        [SerializeField]
        private GameObject tickerPrefab;

        [SerializeField]
        private GameObject playerPrefab;

        private void Start()
        {
            if (IsServer)
            {
                NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnect;

                var map = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
                map.GetComponent<NetworkObject>().Spawn();

                var ticker = Instantiate(tickerPrefab);
                ticker.GetComponent<NetworkObject>().Spawn();
            }

            if (IsClient)
            {
                LoginServerRpc(ScorpiaSettings.Uid);
            }
        }

        private void OnDisconnect(ulong clientId)
        {
            if (IsServer)
            {
                var player = ScorpiaServer.Singleton.Players.Get(clientId);

                ScorpiaServer.Singleton.SendNotification(Notification.Format(Notifications.PLAYER_DISCONNECTED, $"{player.Name}"));

                print($"[{clientId}]{player.Name} disconnected");
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void LoginServerRpc(string uid, ServerRpcParams serverRpcParams = default)
        {
            var senderId = serverRpcParams.Receive.SenderClientId;
            var player = ScorpiaServer.Singleton.Players.FindByUID(uid);

            if (player.ID != senderId)
            {
                print($"reconnecting [{player.ID}=>{senderId}]{player.Name}");

                var objs = NetworkManager.SpawnManager.SpawnedObjects;

                foreach (var obj in objs)
                {
                    if (obj.Key == player.ID)
                    {
                        obj.Value.ChangeOwnership(senderId);
                    }
                }

                player.ID = senderId;

                return;
            }

            print($"connecting {uid} ({senderId})");

            var instance = Instantiate(playerPrefab);
            instance.GetComponent<NetworkObject>().SpawnWithOwnership(senderId);

            var playerInstance = instance.GetComponent<Player>();

            playerInstance.Name.Value = player.Name;
            playerInstance.UID.Value = player.UID;
            playerInstance.Colour.Value = (int)player.Colour;
        }
    }
}