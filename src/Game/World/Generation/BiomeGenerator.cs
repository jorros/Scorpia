using Scorpia.Game.Nodes;
using Scorpia.Game.Utils;

namespace Scorpia.Game.World.Generation;

public class BiomeGenerator : IGenerator
{
    private const int ForestSpawnRate = 30;
    private const int HillSpawnRate = 10;
    private const int WaveSpawnRate = 10;

    public void Generate(MapNode map, NoiseMap noiseMap)
    {
        foreach (var tile in map.Tiles)
        {
            var noise = noiseMap.GetPosition(tile.Position.X, tile.Position.Y);

            tile.Biome = BiomeHelper.GetByNoise(noise);

            var feature = MapFeature(tile.Biome, map.Rnd);
            if (feature != null)
            {
                tile.AddFeature(feature.Value);
            }
        }
    }

    private MapTileFeature? MapFeature(Biome biome, Random rnd)
    {
        if (biome != Biome.Water && rnd.Next(0, 100) <= ForestSpawnRate)
        {
            return MapTileFeature.Forest;
        }
        else if (biome == Biome.Grass && rnd.Next(0, 100) <= HillSpawnRate)
        {
            return MapTileFeature.Hill;
        }

        if (biome == Biome.Water && rnd.Next(0, 100) <= WaveSpawnRate)
        {
            return MapTileFeature.Wave;
        }

        return null;
    }
}