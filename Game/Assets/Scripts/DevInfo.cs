using TMPro;
using UnityEngine;
using Scorpia.Assets.Scripts.Map;
using Unity.Netcode;
using System.Linq;
using Scorpia.Assets.Scripts.Actors;
using Scorpia.Assets.Scripts.World;

namespace Scorpia.Assets.Scripts
{
    public class DevInfo : MonoBehaviour
    {
        public bool enable;

        private TextMeshProUGUI text;
        private MapRenderer map;
        private MapTile selected;

        void Awake()
        {
            text = GetComponent<TextMeshProUGUI>();
        }

        void Update()
        {
            if(NetworkManager.Singleton.IsServer)
            {
                return;
            }

            if (map == null)
            {
                var mapRenderer = GameObject.FindGameObjectWithTag("Map");
                map = mapRenderer?.GetComponent<MapRenderer>();

                return;
            }
            if (enable)
            {
                text.SetText($"{GetTileInfo()}\n{GetNetworkDetails()}\n{Game.Version}");
            }
        }

        private string GetTileInfo()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var currentTile = map.GetTile(mousePos);

            if (currentTile == null)
            {
                return "No tile";
            }

            var direction = string.Empty;
            if(selected != null)
            {
                direction = $"D:{map.map.GetDirection(selected, currentTile)}";
            }

            return $"{PrintTileInfo(currentTile)} {direction}";
        }

        private string PrintTileInfo(MapTile tile)
        {
            var riverInfo = string.Empty;

            if (tile.River != null)
            {
                riverInfo = $"River: {tile.River.From} / {tile.River.To}";
            }

            return $"{tile.Position} {tile.Biome} {tile.Feature} {riverInfo}";
        }

        private string GetNetworkDetails()
        {
            var spawnCount = NetworkManager.Singleton.SpawnManager.SpawnedObjectsList.Count;
            var status = NetworkManager.Singleton.IsClient ? "Client" : "Server";

            return $"Status:{status} Pool:{spawnCount}";
        }

        private string GetPlayerNames()
        {
            if(NetworkManager.Singleton.IsClient)
            {
                return string.Empty;
            }

            var clients = NetworkManager.Singleton.ConnectedClients;
            var players = clients.Values.Select(x => x.PlayerObject?.GetComponent<Player>().Name.Value);

            return string.Join(",", players);
        }
    }
}