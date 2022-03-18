using Scorpia.Assets.Scripts.Map;
using UnityEngine;

namespace Scorpia.Assets.Scripts.UI.TileInfo
{
	public class EmptyTileInfo : BaseTileInfo
	{
        public EmptyTileInfo(GameObject prefab, GameObject parent, Sprite[] icons, Sprite[] avatars) : base(prefab, parent, icons, avatars)
        {
        }

        protected override string GetName(MapTile mapTile)
        {
            return "Empty";
        }

        protected override void Render(MapTile mapTile)
        {
            SetAvatar(mapTile);
            AddResourceIcon(mapTile);
            AddFertilityIcon(mapTile);
        }

        private void SetAvatar(MapTile tile)
        {
            var i = tile switch
            {
                { Biome: Biome.Water } => 8,
                { Biome: Biome.Grass, Feature: TileFeature.Forest } => 2,
                { Biome: Biome.Grass } => 3,
                { Biome: Biome.Mountain } => 4,
                _ => -1
            };

            if (i > -1)
            {
                SetAvatarIcon(i);
            }
        }

        private void AddResourceIcon(MapTile tile)
        {
            var i = tile.Resource switch
            {
                Resource.Sofrum => 5,
                Resource.Gold => 3,
                Resource.Zellos => 6,
                Resource.Nitra => 4,
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
                Fertility.Low => 0,
                Fertility.High => 2,
                _ => -1
            };

            if (i > -1)
            {
                AddInfoIcon(i);
            }
        }
    }
}

