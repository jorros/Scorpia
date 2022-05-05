﻿using Actors;
using Actors.Entities;
using Blueprints;
using Map;
using UI.Tooltip;
using Utils;

namespace UI.TileInfo
{
    public class CityTileInfo : ITileInfo
    {
        private readonly InfoUISystem system;

        public CityTileInfo(InfoUISystem system)
        {
            this.system = system;
        }

        public bool ShouldRender(MapTile tile)
        {
            return tile.Location is not null;
        }

        public void Render(MapTile mapTile)
        {
            if (mapTile.Location is null)
            {
                return;
            }

            var location = mapTile.Location;
            system.SetName(location.Name.ValueAsString());

            SetAvatar(location);
            AddPlayerIcon(location);
            AddResourceIcon(mapTile);
            AddFertilityIcon(mapTile);
            AddStats(location);
        }

        private void AddPlayerIcon(Location location)
        {
            var player = Game.GetPlayer(location.Player.ValueAsString());
            system.AddInfoIcon(7 + (int) player.Colour.Value, TooltipDescription.Empty);
        }

        private void SetAvatar(Location location)
        {
            var i = location.Type.Value switch
            {
                LocationType.Village => 7,
                LocationType.Town => 6,
                LocationType.City => 1,
                _ => -1
            };

            if (i > -1)
            {
                system.SetAvatarIcon(i);
            }
        }

        private void AddResourceIcon(MapTile tile)
        {
            var i = tile.Resource switch
            {
                Resource.Sofrum => (5, "Sofrum", "Can produce sofrum here"),
                Resource.Gold => (3, "Gold", "Can mine gold here"),
                Resource.Zellos => (6, "Zellos", "Can extract zellos here"),
                Resource.Nitra => (4, "Nitra", "Can mine nitra here"),
                _ => (-1, "Error", "")
            };

            if (i.Item1 > -1)
            {
                system.AddInfoIcon(i.Item1, new TooltipDescription(i.Item2, string.Empty, i.Item3));
            }
        }

        private void AddFertilityIcon(MapTile tile)
        {
            var i = tile.Fertility switch
            {
                Fertility.Low => (0, "Barren", "Barely able to produce food here"),
                Fertility.High => (2, "Fertile ground", "Can produce more food here"),
                _ => (-1, "Error", "")
            };

            if (i.Item1 > -1)
            {
                system.AddInfoIcon(i.Item1, new TooltipDescription(i.Item2, string.Empty, i.Item3));
            }
        }

        private void AddStats(Location location)
        {
            system.AddStat(3, $"{location.Population.Value.Format()} / {location.MaxPopulation.Value.Format()}",
                new TooltipDescription("Population",
                    $"The current population. Living space can be increased by building houses or upgrading the {location.Type.Value} to a higher tier"));

            system.AddStat(0, $"{location.Income.Value.Total.FormatBalance()}",
                new TooltipDescription("Income", $"Current monthly income generated."));

            system.AddStat(1,
                $"{location.FoodStorage.Value.Format()} / {LocationBlueprint.GetMaxFoodStorage(location.Type.Value)} ({location.FoodProduction.Value.Total.FormatBalance()})",
                new TooltipDescription("Food",
                    $"You are currently storing {location.FoodStorage.Value.Format()} and the monthly food production is at {location.FoodProduction.Value.Total.FormatBalance()}. You can increase the storage room by upgrading this {location.Type.Value}. All produced food that cannot stored here will be transferred to your global storage."));

            system.AddStat(2, location.Garrison.Value.Format(),
                new TooltipDescription("Garrison",
                    $"There are currently {location.Garrison.Value.Format()} troops stationed in this {location.Type.Value}"));
        }
    }
}