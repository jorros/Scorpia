using System;
using System.Linq;
using Actors.Entities;
using Blueprints;
using Map;
using Server;
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
        [NonSerialized] public NetworkVariable<BalanceSheet> Income = new();
        [NonSerialized] public NetworkVariable<int> InvestedConstruction = new();

        private MapTile mapTile;

        private void Awake()
        {
            Buildings = new NetworkList<Building>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                ScorpiaServer.Singleton.AddLocation(this);
            }
            
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

        public Building? GetCurrentConstruction()
        {
            foreach (var building in Buildings)
            {
                if (building.IsBuilding)
                {
                    return building;
                }
            }

            return null;
        }

        public Building? GetBuildingByFamily(BuildingType type)
        {
            foreach (var building in mapTile.Location.Buildings)
            {
                if (!BuildingBlueprints.IsSameFamily(building.Type, type))
                {
                    continue;
                }

                return building;
            }

            return null;
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

                if (building.IsBuilding)
                {
                    InvestedConstruction.Value = 0;
                }
                
                var downgrade = BuildingBlueprints.GetDowngrade(building.Type);
                
                if (downgrade != null && building.Level > 0)
                {
                    var build = new Building
                    {
                        IsBuilding = false,
                        Type = downgrade.Value,
                        Level = building.Level - 1
                    };
                    
                    Buildings.Replace(building, build);
                    break;
                }

                Buildings.Remove(building);
                
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
            InvestedConstruction.Value = 1;

            var started = false;

            foreach (var building in Buildings.ToEnumerable())
            {
                if (!BuildingBlueprints.IsSameFamily(building.Type, type))
                {
                    continue;
                }

                build.Level = building.Level + 1;
                
                Buildings.Replace(building, build);
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
            if (IsServer)
            {
                ScorpiaServer.Singleton.RemoveLocation(this);
            }
            
            if (title != null)
            {
                Destroy(title);
            }
            
            var tile = MapRenderer.current.GetTile(transform.position);
            tile.Location = null;
        }

        public void DailyTick()
        {
            if (InvestedConstruction.Value > 0)
            {
                InvestedConstruction.Value += 10;

                var building = Buildings.ToEnumerable().FirstOrDefault(x => x.IsBuilding);
                var constructionCost = BuildingBlueprints.GetConstructionCost(building.Type);
                if (InvestedConstruction.Value >= constructionCost)
                {
                    var finishedBuilding = new Building
                    {
                        Level = building.Level,
                        Type = building.Type
                    };
                    InvestedConstruction.Value = 0;
                    Buildings.Replace(building, finishedBuilding);
                }
            }
        }

        public void MonthlyTick()
        {
            var player = Game.GetPlayer(Player.Value.Value);
            
            Income.Value = new BalanceSheet();

            foreach (var building in Buildings)
            {
                if (building.IsBuilding)
                {
                    continue;
                }

                building.Tick(player, this);
            }
        }
    }
}