using System.Linq;
using Actors;
using Actors.Entities;
using Blueprints;
using Map;
using Server;
using Unity.Collections;
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

            playerInstance.Name.Value = new ForceNetworkSerializeByMemcpy<FixedString64Bytes>(player.Name);
            playerInstance.Uid.Value = new ForceNetworkSerializeByMemcpy<FixedString64Bytes>(player.UID);
            playerInstance.Colour.Value = player.Colour;

            instance.GetComponent<NetworkObject>().SpawnWithOwnership(senderId);

            var testLocation = Instantiate(testPrefab,
                MapRenderer.current.GetTileWorldPosition(new Vector2Int(9, 7)), Quaternion.identity);
            var location = testLocation.GetComponent<Location>();

            location.Name.Value = new ForceNetworkSerializeByMemcpy<FixedString64Bytes>("Inglewood");
            location.Player.Value = new ForceNetworkSerializeByMemcpy<FixedString64Bytes>(player.UID);
            location.Type.Value = LocationType.Town;
            location.Population.Value = 2000;
            location.MaxPopulation.Value = LocationBlueprint.GetMaxPopulation(LocationType.Town);
            location.IsCapital.Value = true;
            // location.FoodStorage.Value = 200;

            testLocation.GetComponent<NetworkObject>().SpawnWithOwnership(senderId);
            
            var adminTown = Instantiate(testPrefab,
                MapRenderer.current.GetTileWorldPosition(new Vector2Int(3, 4)), Quaternion.identity);
            var adminLocation = adminTown.GetComponent<Location>();

            adminLocation.Name.Value = new ForceNetworkSerializeByMemcpy<FixedString64Bytes>("Zorn des Admin");
            adminLocation.Player.Value = new ForceNetworkSerializeByMemcpy<FixedString64Bytes>(player.UID);
            adminLocation.Type.Value = LocationType.Village;
            adminLocation.Population.Value = 100;
            adminLocation.MaxPopulation.Value = LocationBlueprint.GetMaxPopulation(LocationType.Village);

            adminLocation.GetComponent<NetworkObject>().SpawnWithOwnership(senderId);
            
            var towerTown = Instantiate(testPrefab,
                MapRenderer.current.GetTileWorldPosition(new Vector2Int(13, 12)), Quaternion.identity);
            var towerLocation = towerTown.GetComponent<Location>();

            towerLocation.Name.Value = new ForceNetworkSerializeByMemcpy<FixedString64Bytes>("Diver of towers");
            towerLocation.Player.Value = new ForceNetworkSerializeByMemcpy<FixedString64Bytes>(player.UID);
            towerLocation.Type.Value = LocationType.Village;
            towerLocation.Population.Value = 100;
            towerLocation.MaxPopulation.Value = LocationBlueprint.GetMaxPopulation(LocationType.Village);

            towerLocation.GetComponent<NetworkObject>().SpawnWithOwnership(senderId);
        }
    }
}