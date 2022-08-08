using Utils;

namespace Map.Generation
{
	public class FertilityGenerator : IGenerator
	{
        private const int HighFertilityCloseWater = 60;
        private const int LowFertility = 30;
        private const int NormalFertility = 80;

        public void Generate(Map map, NoiseMap noiseMap)
        {
            foreach(var tile in map.Tiles)
            {
                if(tile.Biome != Biome.Grass)
                {
                    continue;
                }

                if(map.HasNeighbour(tile, x => x.River != null))
                {
                    tile.Fertility = Fertility.High;
                }
                else if(map.HasNeighbour(tile, x => x.River != null || x.Biome == Biome.Water, 4))
                {
                    tile.Fertility = HasChance(HighFertilityCloseWater, map) ? Fertility.High : Fertility.Normal;
                }
                else
                {
                    var chance = map.Rnd.Next(100);

                    if(chance < LowFertility)
                    {
                        tile.Fertility = Fertility.Low;
                    }
                    else if(chance < NormalFertility)
                    {
                        tile.Fertility = Fertility.Normal;
                    }
                    else
                    {
                        tile.Fertility = Fertility.High;
                    }
                }
            }
        }

        private bool HasChance(int chance, Map map) => map.Rnd.Next(100) < chance;
    }
}

