using System.Linq;
using Actors;
using Map;
using Server;
using Unity.Netcode;
using UnityEngine;

namespace World
{
    public class WorldManager : NetworkBehaviour
    {
        [SerializeField] private GameObject mapPrefab;

        [SerializeField] private GameObject tickerPrefab;

        [SerializeField] private GameObject playerPrefab;

        [SerializeField] private GameObject testPrefab;

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
        }

        public override void OnNetworkSpawn()
        {
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

                ScorpiaServer.Singleton.SendNotification(Notification.Format(Notifications.PlayerDisconnected,
                    $"{player.Name}"));

                print($"[{clientId}]{player.Name} disconnected");
            }
        }

        [ServerRpc(RequireOwnership = false)]
        private void LoginServerRpc(string uid, ServerRpcParams serverRpcParams = default)
        {
            var senderId = serverRpcParams.Receive.SenderClientId;
            var player = ScorpiaServer.Singleton.Players.FindByUID(uid);

            if (player.ID != senderId)
            {
                print($"reconnecting [{player.ID}=>{senderId}]{player.Name}");

                var objs = NetworkManager.SpawnManager.SpawnedObjects;
                var playersObjs = objs.Where(obj => obj.Key == player.ID);

                foreach (var obj in playersObjs)
                {
                    obj.Value.ChangeOwnership(senderId);
                }

                player.ID = senderId;

                return;
            }

            print($"connecting {uid} ({senderId})");

            var instance = Instantiate(playerPrefab);
            var playerInstance = instance.GetComponent<Player>();

            playerInstance.Name.Value = player.Name;
            playerInstance.Uid.Value = player.UID;
            playerInstance.Colour.Value = player.Colour;
            
            instance.GetComponent<NetworkObject>().SpawnWithOwnership(senderId);
            
            var testLocation = Instantiate(testPrefab,
                MapRenderer.current.GetTileWorldPosition(new Vector2Int(5, 5)), Quaternion.identity);
            testLocation.GetComponent<Location>().location.Value = new MapLocation
            {
                Name = "Inglewood",
                Player = player.UID,
                Type = MapLocation.LocationType.Village,
                Income = 20,
                Garrison = 1000,
                Population = 500,
                MaxPopulation = 2000,
                FoodProduction = 2,
                FoodStorage = 100
            };
            testLocation.GetComponent<NetworkObject>().SpawnWithOwnership(senderId);
        }
    }
}