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
                NetworkManager.Singleton.OnClientConnectedCallback += OnConnect;
                NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnect;

                var map = Instantiate(mapPrefab, Vector3.zero, Quaternion.identity);
                map.GetComponent<NetworkObject>().Spawn();

                var ticker = Instantiate(tickerPrefab);
                ticker.GetComponent<NetworkObject>().Spawn();
            }

            if (IsClient)
            {
                print("log in");
                LoginServerRpc(ScorpiaSettings.PlayerName, ScorpiaSettings.Uid);
            }
        }

        private void OnConnect(ulong clientId)
        {
            if (IsServer)
            {
            }
        }

        private void OnDisconnect(ulong clientId)
        {
            if (IsServer)
            {
                var uid = ScorpiaServer.Singleton.FindUid(clientId);

                ScorpiaServer.Singleton.SendNotification(Notification.Format(Notifications.PLAYER_DISCONNECTED, $"{clientId}"));

                print($"{uid} ({clientId}) disconnected");
            }
        }

        [ServerRpc(RequireOwnership = false)]
        public void LoginServerRpc(string name, string uid, ServerRpcParams serverRpcParams = default)
        {
            var senderId = serverRpcParams.Receive.SenderClientId;

            if (ScorpiaServer.Singleton.PlayerMap.ContainsKey(uid))
            {
                print($"reconnecting {uid} ({senderId})");

                var objs = ScorpiaServer.Singleton.GetPlayerObjects(uid);

                foreach (var obj in objs)
                {
                    var nObj = obj.GetComponent<NetworkObject>();
                    nObj.ChangeOwnership(senderId);
                }

                ScorpiaServer.Singleton.PlayerMap[uid] = senderId;
            }
            else
            {
                print($"connecting {uid} ({senderId})");

                var instance = Instantiate(playerPrefab);
                instance.GetComponent<NetworkObject>().SpawnWithOwnership(senderId);
                ScorpiaServer.Singleton.AddPlayerObject(uid, instance);

                var player = instance.GetComponent<Player>();

                player.PlayerName.Value = name;
                player.Uid.Value = uid;

                ScorpiaServer.Singleton.PlayerMap.Add(uid, serverRpcParams.Receive.SenderClientId);
            }
        }
    }
}