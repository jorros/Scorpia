namespace Map
{
    public enum Biome
    {
        Water,
        Grass,
        Mountain
    }

    public static class BiomeHelper
    {
        public static Biome GetByNoise(float noise)
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
    }
}