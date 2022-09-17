namespace Scorpia.Game.World;

public enum Biome
{
    Water,
    Grass,
    Mountain
}

public static class BiomeHelper
{
    public static Biome GetByNoise(double noise)
    {
        if (noise < 0.2)
        {
            return Biome.Water;
        }
        return noise < 0.8 ? Biome.Grass : Biome.Mountain;
    }
}