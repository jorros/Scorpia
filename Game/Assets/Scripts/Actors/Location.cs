using System;
using Blueprints;
using Map;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace Actors
{
    public class Location : NetworkBehaviour, IActor
    {
        private GameObject title;

        [SerializeField] private GameObject titlePrefab;

        public NetworkList<Building> Buildings;
        [NonSerialized] public NetworkVariable<LocationType> Type = new();
        [NonSerialized] public NetworkVariable<FixedString64Bytes> Name = new();
        [NonSerialized] public NetworkVariable<FixedString64Bytes> Player = new();
        [NonSerialized] public NetworkVariable<int> Population = new();
        [NonSerialized] public NetworkVariable<int> MaxPopulation = new();
        [NonSerialized] public NetworkVariable<int> Garrison = new();
        [NonSerialized] public NetworkVariable<int> FoodProduction = new();
        [NonSerialized] public NetworkVariable<int> FoodStorage = new();
        [NonSerialized] public NetworkVariable<int> Income = new();
        [NonSerialized] public NetworkVariable<int> InvestedConstruction = new();

        private MapTile mapTile;

        private void Awake()
        {
            Buildings = new NetworkList<Building>();
        }

        public override void OnNetworkSpawn()
        {
            mapTile = MapRenderer.current.GetTile(transform.position);
            mapTile.Location = this;
            EventManager.Trigger(Events.LocationUpdated, mapTile.Position);
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
            mapTitle.SetLocation(this);
        }

        [ServerRpc]
        public void DemolishServerRpc(BuildingType type)
        {
            var player = Game.GetPlayer(OwnerClientId);

            foreach (var building in Buildings.ToEnumerable())
            {
                if (building.Type != type)
                {
                    continue;
                }

                player.Refund(BuildingBlueprints.GetRequirements(type), building.IsBuilding ? 1f : 0.5f);
                
                var idx = Buildings.IndexOf(building);
                var downgrade = BuildingBlueprints.GetDowngrade(building.Type);
                
                Buildings.Remove(building);

                if (downgrade != null)
                {
                    var build = new Building
                    {
                        IsBuilding = false,
                        Type = downgrade.Value,
                        Level = building.Level - 1
                    };
                    
                    Buildings.Insert(idx, build);
                }
                
                break;
            }
        }

        [ServerRpc]
        public void BuildServerRpc(BuildingType type)
        {
            var player = Game.GetPlayer(OwnerClientId);

            if (!BuildingBlueprints.FulfillsRequirements(player, mapTile, type))
            {
                return;
            }

            var build = new Building
            {
                Type = type,
                IsBuilding = true
            };
            
            player.Pay(BuildingBlueprints.GetRequirements(type));
            InvestedConstruction.Value = 0;

            var started = false;

            foreach (var building in Buildings.ToEnumerable())
            {
                if (!BuildingBlueprints.IsSameFamily(building.Type, type))
                {
                    continue;
                }

                build.Level = building.Level + 1;
                
                var idx = Buildings.IndexOf(building);
                Buildings.RemoveAt(idx);
                Buildings.Insert(idx, build);
                started = true;
                break;
            }

            if (!started)
            {
                build.Level = 1;
                Buildings.Add(build);
            }
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