using Scorpia.Engine;
using Scorpia.Engine.Maths;

namespace Scorpia.Game.Utils;

public class NoiseMap
{
    private double[] noiseMap;
    private int mapWidth, mapHeight;

    private static readonly List<OffsetVector> Directions = new()
    {
        new OffsetVector(0, 1), //N
        new OffsetVector(1, 1), //NE
        new OffsetVector(1, 0), //E
        new OffsetVector(-1, 1), //SE
        new OffsetVector(-1, 0), //S
        new OffsetVector(-1, -1), //SW
        new OffsetVector(0, -1), //W
        new OffsetVector(1, -1) //NW
    };

    public NoiseMap(int width, int height)
    {
        mapWidth = width;
        mapHeight = height;
    }

    public double GetPosition(int x, int y)
    {
        return noiseMap[y * mapWidth + x];
    }

    public IReadOnlyList<OffsetVector> FindLocalMaxima()
    {
        var maximas = new List<OffsetVector>();
        for (var x = 0; x < mapWidth; x++)
        {
            for (var y = 0; y < mapHeight; y++)
            {
                var noiseVal = GetPosition(x, y);
                if (CheckNeighbours(x, y, neighbourNoise => neighbourNoise > noiseVal))
                {
                    maximas.Add(new OffsetVector(x, y));
                }
            }
        }

        return maximas;
    }

    public IReadOnlyList<OffsetVector> FindLocalMinima()
    {
        var minimas = new List<OffsetVector>();
        for (var x = 0; x < mapWidth; x++)
        {
            for (var y = 0; y < mapHeight; y++)
            {
                var noiseVal = GetPosition(x, y);
                if (CheckNeighbours(x, y, neighbourNoise => neighbourNoise < noiseVal))
                {
                    minimas.Add(new OffsetVector(x, y));
                }
            }
        }

        return minimas;
    }

    private bool CheckNeighbours(int x, int y, Func<double, bool> failCondition)
    {
        foreach (var dir in Directions)
        {
            var newPost = new OffsetVector(x + dir.X, y + dir.Y);

            if (newPost.X < 0 || newPost.X >= mapWidth || newPost.Y < 0 || newPost.Y >= mapHeight)
            {
                continue;
            }

            if (failCondition(GetPosition(x + dir.X, y + dir.Y)))
            {
                return false;
            }
        }

        return true;
    }

    public void Generate(int seed, float scale, int octaves, float persistance, float lacunarity, OffsetVectorF offset)
    {
        noiseMap = new double[mapWidth * mapHeight];

        var random = new Random(seed);

        // We need atleast one octave
        if (octaves < 1)
        {
            octaves = 1;
        }

        var octaveOffsets = new OffsetVectorF[octaves];
        for (var i = 0; i < octaves; i++)
        {
            var offsetX = random.Next(-100000, 100000) + offset.X;
            var offsetY = random.Next(-100000, 100000) + offset.Y;
            octaveOffsets[i] = new OffsetVectorF(offsetX, offsetY);
        }

        if (scale <= 0f)
        {
            scale = 0.0001f;
        }

        var maxNoiseHeight = double.MinValue;
        var minNoiseHeight = double.MaxValue;

        // When changing noise scale, it zooms from top-right corner
        // This will make it zoom from the center
        var halfWidth = mapWidth / 2f;
        var halfHeight = mapHeight / 2f;

        for (int x = 0, y; x < mapWidth; x++)
        {
            for (y = 0; y < mapHeight; y++)
            {
                double amplitude = 1;
                double frequency = 1;
                double noiseHeight = 0;
                for (var i = 0; i < octaves; i++)
                {
                    var sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].X;
                    var sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].Y;
                    
                    var perlinValue = MathExt.PerlinNoise(sampleX, sampleY) * 2 - 1;

                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }

                if (noiseHeight > maxNoiseHeight)
                    maxNoiseHeight = noiseHeight;
                else if (noiseHeight < minNoiseHeight)
                    minNoiseHeight = noiseHeight;

                noiseMap[y * mapWidth + x] = noiseHeight;
            }
        }

        for (int x = 0, y; x < mapWidth; x++)
        {
            for (y = 0; y < mapHeight; y++)
            {
                // Returns a value between 0f and 1f based on noiseMap value
                // minNoiseHeight being 0f, and maxNoiseHeight being 1f
                noiseMap[y * mapWidth + x] =
                    MathExt.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[y * mapWidth + x]);
            }
        }

        // Generate island gradient
        // for (int x = 0; x < mapWidth; x++)
        // {
        //     for (int y = 0; y < mapHeight; y++)
        //     {
        //         // Value between 0 and 1 where * 2 - 1 makes it between -1 and 0
        //         float i = x / (float)mapWidth * 2 - 1;
        //         float j = y / (float)mapHeight * 2 - 1;

        //         // Find closest x or y to the edge of the map
        //         float value = Mathf.Max(Mathf.Abs(i), Mathf.Abs(j));

        //         // Apply a curve graph to have more values around 0 on the edge, and more values >= 3 in the middle
        //         float a = 3;
        //         float b = 4f;
        //         float islandGradientValue = Mathf.Pow(value, a) / (Mathf.Pow(value, a) + Mathf.Pow(b - b * value, a));

        //         // Apply gradient in the map
        //         noiseMap[y * mapWidth + x] = Mathf.Clamp01(noiseMap[y * mapWidth + x] - islandGradientValue);
        //     }
        // }
    }
}