using Scorpia.Assets.Scripts.Map;
using UnityEngine;

namespace Scorpia.Assets.Scripts.UI.TileInfo
{
	public class EmptyTileInfo : BaseTileInfo
	{
        public EmptyTileInfo(GameObject prefab, GameObject parent, GameObject iconPrefab, Sprite[] icons) : base(prefab, parent, iconPrefab, icons)
        {
        }

        protected override string GetName(MapTile mapTile)
        {
            return "Empty";
        }

        protected override void Render(MapTile mapTile)
        {
            AddBiomeIcon(mapTile);
            AddResourceIcon(mapTile);
            AddFertilityIcon(mapTile);
        }

        private void AddBiomeIcon(MapTile tile)
        {
            var i = tile switch
            {
                { Biome: Biome.Water } => 0,
                { Biome: Biome.Grass, Feature: TileFeature.Forest } => 2,
                { Biome: Biome.Grass } => 1,
                { Biome: Biome.Mountain } => 3,
                _ => -1
            };

            if (i > -1)
            {
                AddInfoIcon(i);
            }
        }

        private void AddResourceIcon(MapTile tile)
        {
            var i = tile.Resource switch
            {
                Resource.Sofrum => 8,
                Resource.Gold => 6,
                Resource.Zellos => 9,
                Resource.Nitra => 7,
                _ => -1
            };

            if (i > -1)
            {
                AddInfoIcon(i);
            }
        }

        private void AddFertilityIcon(MapTile tile)
        {
            var i = tile.Fertility switch
            {
                Fertility.Low => 4,
                Fertility.High => 5,
                _ => -1
            };

            if (i > -1)
            {
                AddInfoIcon(i);
            }
        }
    }
}

