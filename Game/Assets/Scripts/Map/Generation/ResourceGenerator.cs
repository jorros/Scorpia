namespace Map.Generation
{
    public class ResourceGenerator : IGenerator
    {
        private const int GOLD_SPAWN_CHANCE = 6;
        private const int SOFRUM_SPAWN_CHANCE = 4;
        private const int NITRA_SPAWN_CHANCE = 1;
        private const int ZELLOS_SPAWN_CHANCE = 4;

        public void Generate(Map map)
        {
            foreach (var tile in map.Tiles)
            {
                if (tile.Biome != Biome.Grass)
                {
                    continue;
                }

                if(CheckGold(tile, map))
                {
                    continue;
                }

                if (CheckZellos(tile, map))
                {
                    continue;
                }

                if (CheckSofrum(tile, map))
                {
                    continue;
                }

                if(CheckNitra(tile, map))
                {
                    continue;
                }
            }
        }

        private bool HasChance(Map map, int chance)
        {
            return map.Rnd.Next(100) <= chance;
        }

        private bool CheckGold(MapTile tile, Map map)
        {
            if (HasChance(map, GOLD_SPAWN_CHANCE) && map.HasNeighbour(tile, x => x.Biome == Biome.Mountain, 2))
            {
                tile.Resource = Resource.Gold;

                return true;
            }

            return false;
        }

        private bool CheckSofrum(MapTile tile, Map map)
        {
            if (HasChance(map, SOFRUM_SPAWN_CHANCE) && tile.Feature == TileFeature.Forest)
            {
                tile.Resource = Resource.Sofrum;

                return true;
            }

            return false;
        }

        private bool CheckNitra(MapTile tile, Map map)
        {
            if (HasChance(map, NITRA_SPAWN_CHANCE))
            {
                tile.Resource = Resource.Nitra;

                return true;
            }

            return false;
        }

        private bool CheckZellos(MapTile tile, Map map)
        {
            if (HasChance(map, ZELLOS_SPAWN_CHANCE) && map.HasNeighbour(tile, x => x.Biome == Biome.Water || x.River != null, 4))
            {
                tile.Resource = Resource.Zellos;

                return true;
            }

            return false;
        }
    }
}

