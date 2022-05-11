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
        private MapTitle mapTitle;

        [SerializeField] private GameObject titlePrefab;

        public NetworkList<Building> Buildings;
        [NonSerialized] public NetworkVariable<LocationType> Type = new();
        [NonSerialized] public NetworkVariable<ForceNetworkSerializeByMemcpy<FixedString64Bytes>> Name = new();
        [NonSerialized] public NetworkVariable<ForceNetworkSerializeByMemcpy<FixedString64Bytes>> Player = new();
        [NonSerialized] public NetworkVariable<int> Population = new();
        [NonSerialized] public NetworkVariable<int> MaxPopulation = new();
        [NonSerialized] public NetworkVariable<int> Garrison = new();
        [NonSerialized] public NetworkVariable<BalanceSheet> FoodProduction = new();
        [NonSerialized] public NetworkVariable<float> FoodStorage = new();
        [NonSerialized] public NetworkVariable<BalanceSheet> Income = new();
        [NonSerialized] public NetworkVariable<int> InvestedConstruction = new();
        [NonSerialized] public NetworkVariable<bool> IsCapital = new();
        public NetworkList<ForceNetworkSerializeByMemcpy<FixedString64Bytes>> Tags;

        public MapTile MapTile;

        private void Awake()
        {
            Buildings = new NetworkList<Building>();
            Tags = new NetworkList<ForceNetworkSerializeByMemcpy<FixedString64Bytes>>();
        }

        public override void OnNetworkSpawn()
        {
            Game.AddLocation(this);

            Tags.OnListChanged += evt => mapTitle.SetLocation(this);

            MapTile = MapRenderer.current.GetTile(transform.position);
            MapTile.Location = this;
            EventManager.Trigger(Events.LocationUpdated, MapTile.Position);
            EventManager.Trigger(Events.UpdateFog);
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

            mapTitle = title.GetComponent<MapTitle>();
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
            foreach (var building in MapTile.Location.Buildings)
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

                building.OnFinishBuilding(this, true);
                Buildings.Remove(building);

                break;
            }
        }

        [ServerRpc]
        public void BuildServerRpc(BuildingType type)
        {
            var player = Game.GetPlayer(OwnerClientId);

            if (!BuildingBlueprints.FulfillsRequirements(player, MapTile, type))
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
            Game.RemoveLocation(this);

            if (title != null)
            {
                Destroy(title);
            }
            
            EventManager.Trigger(Events.UpdateFog, MapTile.Position);
            MapTile.Location = null;
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

                    ScorpiaServer.Singleton.SendNotification(Notification.Format(Notifications.BuildingFinished,
                        building.Type.ToString(), Name.Value));
                    finishedBuilding.OnFinishBuilding(this, false);
                }
            }
        }

        public void MonthlyTick()
        {
            var player = Game.GetPlayer(Player.Value.Value.Value);

            Income.Value = new BalanceSheet();

            // Tax collection
            var collectedTax = Population.Value * 0.001f;
            Income.Value = Income.Value.Add(nameof(BalanceSheet.PopulationIn), collectedTax);
            player.ScorpionsBalance.Value =
                player.ScorpionsBalance.Value.Add(nameof(BalanceSheet.PopulationIn), collectedTax);

            // Pop consumption
            FoodProduction.Value =
                FoodProduction.Value.Set(nameof(BalanceSheet.PopulationOut), Population.Value * 0.0005f);

            foreach (var building in Buildings)
            {
                if (building.IsBuilding)
                {
                    continue;
                }

                building.Tick(player, this);
            }

            var foodAfterConsumption = FoodStorage.Value + FoodProduction.Value.Total;

            FoodStorage.Value = Math.Max(0, foodAfterConsumption);

            var maxStorage = LocationBlueprint.GetMaxFoodStorage(Type.Value);
            var difference = 0f;

            if (foodAfterConsumption > 0)
            {
                Tags.Remove(LocationTags.Famine);
            }

            if (foodAfterConsumption > maxStorage)
            {
                difference = FoodStorage.Value - maxStorage;
                player.Food.Value += difference;
                FoodStorage.Value = maxStorage;
            }
            else if (foodAfterConsumption < 0)
            {
                difference = Math.Abs(foodAfterConsumption);
                var globalFood = player.Food.Value;
                foodAfterConsumption += globalFood;

                player.Food.Value = Math.Max(0, foodAfterConsumption);

                if (foodAfterConsumption < 0 && !Tags.Contains(LocationTags.Famine))
                {
                    ScorpiaServer.Singleton.SendNotification(Notification.Format(Notifications.Hunger, Name.Value));
                    Tags.Add(LocationTags.Famine);
                }
                else if (foodAfterConsumption > 0)
                {
                    Tags.Remove(LocationTags.Famine);
                }
            }

            player.FoodBalance.Value = player.FoodBalance.Value.Add(nameof(BalanceSheet.PopulationOut), difference);

            Population.Value += GrowPop(foodAfterConsumption);
            player.Population.Value += Population.Value;
        }

        private int GrowPop(float food)
        {
            const float growthRate = 0.2f;
            var growthByFood = Mathf.Clamp(food, -1f, 0);
            growthByFood = growthByFood == 0 ? growthRate : growthByFood;

            return (int) Math.Round(growthByFood * Population.Value *
                                    (((float) MaxPopulation.Value - Population.Value) / MaxPopulation.Value));
        }
    }
}