using UnityEngine;
using Utils;

namespace Map.Generation
{
	public class BiomeGenerator : IGenerator
	{
        private const float SCALE = 20;
        private const int OCTAVES = 5;
        private const float PERSISTENCE = 0.35f;
        private const float LACUNARITY = 5f;

        private const int FOREST_SPAWN_RATE = 30;
        private const int HILL_SPAWN_RATE = 10;
        private const int WAVE_SPAWN_RATE = 10;

        public void Generate(Map map)
        {
            var noiseMap = new NoiseMap(map.Width, map.Height);

            noiseMap.Generate(map.Seed, SCALE, OCTAVES, PERSISTENCE, LACUNARITY, new Vector2(0, 0));

            for (int x = 0; x < map.Width; x++)
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var noise = noiseMap.GetPosition(x, y);

                    var tile = new MapTile
                    {
                        Biome = MapBiome(noise),
                        Position = new Vector2Int(x, y)
                    };

                    tile.Feature = MapFeature(tile.Biome, map.Rnd);

                    map.SetTile(x, y, tile);
                }
            }
        }

        private Biome MapBiome(float noise)
        {
            if (noise < 0.2)
            {
                return Biome.Water;
            }
            if (noise < 0.8)
            {
                return Biome.Grass;
            }

            return Biome.Mountain;
        }

        private TileFeature MapFeature(Biome biome, System.Random rnd)
        {
            if (biome != Biome.Water && rnd.Next(0, 100) <= FOREST_SPAWN_RATE)
            {
                return TileFeature.Forest;
            }
            else if (biome == Biome.Grass && rnd.Next(0, 100) <= HILL_SPAWN_RATE)
            {
                return TileFeature.Hill;
            }

            if (biome == Biome.Water && rnd.Next(0, 100) <= WAVE_SPAWN_RATE)
            {
                return TileFeature.Wave;
            }

            return TileFeature.None;
        }
    }
}

