using Map;
using Unity.Netcode;
using UnityEngine;

namespace Actors
{
    public class Location : NetworkBehaviour, IActor
    {
        [HideInInspector] public NetworkVariable<MapLocation> location = new();
        private GameObject title;

        [SerializeField] private GameObject titlePrefab;

        public override void OnNetworkSpawn()
        {
            var tile = MapRenderer.current.GetTile(transform.position);
            tile.Location = location.Value;
            EventManager.Trigger(Events.LocationUpdated, tile.Position);
        }

        private void Update()
        {
            if (title != null)
            {
                return;
            }
            
            var titleCanvas = GameObject.FindGameObjectWithTag("TitleCanvas");

            if (titleCanvas == null)
            {
                return;
            }
            
            title = Instantiate(titlePrefab, titleCanvas.GetComponent<RectTransform>());
            
            var worldPos = transform.position;
            worldPos.y += 10;
            title.transform.position = worldPos;
            
            var mapTitle = title.GetComponent<MapTitle>();
            mapTitle.SetLocation(location.Value);
        }

        public override void OnNetworkDespawn()
        {
            if (title != null)
            {
                Destroy(title);
            }
            
            var tile = MapRenderer.current.GetTile(transform.position);
            tile.Location = null;
        }

        public void Tick()
        {
            
        }
    }
}