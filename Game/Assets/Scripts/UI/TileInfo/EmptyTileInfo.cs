using Map;

namespace UI.TileInfo
{
	public class EmptyTileInfo : ITileInfo
	{
        private InfoUISystem system;

        public EmptyTileInfo(InfoUISystem system)
        {
            this.system = system;
        }

        public void Render(MapTile mapTile)
        {
            system.SetName("Empty");

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
    }
}

