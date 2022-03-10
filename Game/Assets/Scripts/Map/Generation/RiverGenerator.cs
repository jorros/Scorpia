using System.Collections.Generic;

namespace Scorpia.Assets.Scripts.Map.Generation
{
    public class RiverGenerator : IGenerator
    {
        private const int RIVER_MIN_DISTANCE = 20;
        private const int RIVER_MAX_COUNT = 6;
        private const int RIVER_MAX_BUILD_ATTEMPTS = 5;

        public void Generate(Map map)
        {
            var rbs = new List<RiverBuilder>();
            FindRiverSpawns(rbs, map);

            foreach (var rb in rbs)
            {
                rb.Build();
            }
        }

        private void FindRiverSpawns(List<RiverBuilder> rivers, Map map)
        {
            for (int i = 0; i < map.Rnd.Next(0, RIVER_MAX_COUNT); i++)
            {
                for (int attempt = 0; attempt < RIVER_MAX_BUILD_ATTEMPTS; attempt++)
                {
                    var tile = map.GetRandomTile(tile => tile.Biome == Biome.Water || tile.Biome == Biome.Mountain);

                    var hasNoRiverStartingNearby = !map.HasNeighbour(tile, tile => tile.River != null, RIVER_MIN_DISTANCE);
                    var hasGrasslandNext = map.HasNeighbour(tile, tile => tile.Biome == Biome.Grass);

                    if (hasNoRiverStartingNearby && hasGrasslandNext)
                    {
                        rivers.Add(new RiverBuilder(tile, map, map.Rnd));
                        continue;
                    }
                }
            }
        }
    }
}

