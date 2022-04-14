using Map;
using UI.Tooltip;

namespace UI.TileInfo
{
    public class CityTileInfo : ITileInfo
    {
        private InfoUISystem system;

        public CityTileInfo(InfoUISystem system)
        {
            this.system = system;
        }

        public bool ShouldRender(MapTile tile)
        {
            return tile.Location.HasValue;
        }

        public void Render(MapTile mapTile)
        {
            if (!mapTile.Location.HasValue)
            {
                return;
            }

            var location = mapTile.Location.Value;
            system.SetName(location.Name.Value);

            SetAvatar(location);
            AddPlayerIcon(mapTile);
            AddResourceIcon(mapTile);
            AddFertilityIcon(mapTile);
            AddStats(location);
        }

        private void AddPlayerIcon(MapTile mapTile)
        {
            // system.AddInfoIcon();
        }

        private void SetAvatar(MapLocation location)
        {
            var i = location switch
            {
                {Type: MapLocation.LocationType.Village} => 7,
                {Type: MapLocation.LocationType.Town} => 6,
                {Type: MapLocation.LocationType.City} => 1,
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

        private void AddStats(MapLocation location)
        {
            system.AddStat(3, location.Population.ToString(),
                new TooltipDescription("Population", "Current population of this location."));
        }
    }
}