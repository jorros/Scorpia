namespace Map.Generation
{
	public class FertilityGenerator : IGenerator
	{
        private const int HIGH_FERTILITY_CLOSE_WATER = 60;
        private const int LOW_FERTILITY = 30;
        private const int NORMAL_FERTILITY = 80;

        public void Generate(Map map)
        {
            foreach(var tile in map.Tiles)
            {
                if(tile.Biome != Biome.Grass)
                {
                    continue;
                }

                if(map.HasNeighbour(tile, x => x.River is not null))
                {
                    tile.Fertility = Fertility.High;
                }
                else if(map.HasNeighbour(tile, x => x.River is not null || x.Biome == Biome.Water, 4))
                {
                    tile.Fertility = HasChance(HIGH_FERTILITY_CLOSE_WATER, map) ? Fertility.High : Fertility.Normal;
                }
                else
                {
                    var chance = map.Rnd.Next(100);

                    if(chance < LOW_FERTILITY)
                    {
                        tile.Fertility = Fertility.Low;
                    }
                    else if(chance < NORMAL_FERTILITY)
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

