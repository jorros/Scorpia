using Scorpia.Game.Nodes;
using Scorpia.Game.Utils;

namespace Scorpia.Game.World.Generation;

public class FertilityGenerator : IGenerator
{
    private const int HighFertilityCloseWater = 60;
    private const int LowFertility = 30;
    private const int NormalFertility = 80;
    
    public void Generate(MapNode map, NoiseMap noiseMap)
    {
        foreach(var position in map.Map)
        {
            var tile = map.Map.GetData(position);
            if(tile.Biome != Biome.Grass)
            {
                continue;
            }

            if(map.Map.HasNeighbour(position, x => x.River != null))
            {
                tile.Fertility = Fertility.High;
            }
            else if(map.Map.HasNeighbour(position, x => x.River != null || x.Biome == Biome.Water, 4))
            {
                tile.Fertility = HasChance(HighFertilityCloseWater, map) ? Fertility.High : Fertility.Normal;
            }
            else
            {
                var chance = map.Rnd.Next(100);

                tile.Fertility = chance switch
                {
                    < LowFertility => Fertility.Low,
                    < NormalFertility => Fertility.Normal,
                    _ => Fertility.High
                };
            }
        }
    }
    
    private bool HasChance(int chance, MapNode map) => map.Rnd.Next(100) < chance;
}