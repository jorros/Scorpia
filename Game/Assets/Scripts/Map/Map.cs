using System;
using System.Collections.Generic;
using System.Linq;
using Scorpia.Assets.Scripts.Utils;
using UnityEngine;

namespace Scorpia.Assets.Scripts.Map
{
    public class Map
    {
        private MapTile[] tiles;
        private int seed, width, height;

        public int Seed => seed;

        public float SCALE = 20;
        public int OCTAVES = 5;
        public float PERSISTENCE = 0.35f;
        public float LACUNARITY = 5f;

        private const int forestSpawnRate = 30;
        private const int hillSpawnRate = 10;
        private const int waveSpawnRate = 10;
        private const int riverMinDistance = 20;
        private const int riverMaxCount = 6;
        private const int maxRiverBuildAttempts = 5;

        private System.Random rnd;

        public Map(int width, int height, int seed)
        {
            tiles = new MapTile[width * height];
            this.seed = seed;
            this.width = width;
            this.height = height;
            this.rnd = new System.Random(seed);
        }

        public MapTile GetTile(int x, int y)
        {
            if (x < 0 || y < 0 || x >= width || y >= height)
            {
                return null;
            }

            return tiles[y * width + x];
        }

        public MapTile GetTile(int q, int r, int s)
        {
            return GetTile(q + (r - (r & 1)) / 2, r);
        }

        public bool HasNeighbour(MapTile start, Func<MapTile, bool> predicate, int range = 1)
        {
            int min = -range, max = range;

            var position = start.Position.ToCube();

            for (int q = min; q <= max; q++)
            {
                for (int r = min; r <= max; r++)
                {
                    for (int s = min; s <= max; s++)
                    {
                        // Sum of cube coordinates should equal 0
                        if(q + r + s != 0)
                        {
                            continue;
                        }

                        var tile = GetTile(q + position.x, r + position.y, s + position.z);
                        if(tile == null || tile == start)
                        {
                            continue;
                        }

                        if (predicate.Invoke(tile))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public IReadOnlyList<MapTile> GetNeighbours(MapTile start, Func<MapTile, bool> predicate = null, int range = 1)
        {
            var list = new List<MapTile>();

            int min = -range, max = range;

            var position = start.Position.ToCube();

            for (int q = min; q <= max; q++)
            {
                for (int r = min; r <= max; r++)
                {
                    for (int s = min; s <= max; s++)
                    {
                        // Sum of cube coordinates should equal 0
                        if(q + r + s != 0)
                        {
                            continue;
                        }

                        var tile = GetTile(q + position.x, r + position.y, s + position.z);
                        if(tile == null || tile == start)
                        {
                            continue;
                        }

                        if (predicate == null || predicate.Invoke(tile))
                        {
                            list.Add(tile);
                        }
                    }
                }
            }

            return list;
        }

        public Direction? GetDirection(MapTile from, MapTile to)
        {
            var vectors = new [] { new Vector3Int(1, -1, 0), new Vector3Int(1, 0, -1), new Vector3Int(0, 1, -1), new Vector3Int(-1, 1, 0), new Vector3Int(-1, 0, 1), new Vector3Int(0, -1, 1) };
            var directions = new[] { Direction.SouthEast, Direction.East, Direction.NorthEast, Direction.NorthWest, Direction.West, Direction.SouthWest };

            var vector = to.Position.ToCube() - from.Position.ToCube();
            var index = -1;

            for(var i = 0; i < vectors.Length; i++)
            {
                if(vectors[i] == vector)
                {
                    index = i;
                    break;
                }
            }

            if(index < 0)
            {
                return null;
            }

            return directions[index];
        }

        public MapTile GetRandomTile(Func<MapTile, bool> predicate = null)
        {
            var validTiles = tiles;

            if (predicate != null)
            {
                validTiles = validTiles.Where(predicate).ToArray();
            }

            var idx = rnd.Next(0, validTiles.Count());

            return validTiles[idx];
        }

        public void Generate()
        {
            var noiseMap = new NoiseMap(width, height);

            noiseMap.Generate(seed, SCALE, OCTAVES, PERSISTENCE, LACUNARITY, new Vector2(0, 0));

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var noise = noiseMap.GetPosition(x, y);

                    var tile = new MapTile
                    {
                        Biome = MapBiome(noise),
                        Position = new Vector2Int(x, y)
                    };

                    tile.Feature = MapFeature(tile.Biome);

                    tiles[y * width + x] = tile;
                }
            }

            var rivers = new List<River>();
            FindRiverSpawns(rivers);

            foreach (var river in rivers)
            {
                river.Build();
            }
        }

        private void FindRiverSpawns(List<River> rivers)
        {
            for (int i = 0; i < rnd.Next(0, riverMaxCount); i++)
            {
                for (int attempt = 0; attempt < maxRiverBuildAttempts; attempt++)
                {
                    var tile = GetRandomTile(tile => tile.Biome == Biome.Water || tile.Biome == Biome.Mountain);

                    var hasNoRiverStartingNearby = !HasNeighbour(tile, tile => tile.River != null, riverMinDistance);
                    var hasGrasslandNext = HasNeighbour(tile, tile => tile.Biome == Biome.Grass);

                    if (hasNoRiverStartingNearby && hasGrasslandNext)
                    {
                        rivers.Add(new River(tile, this, rnd));
                        continue;
                    }
                }
            }
        }

        private TileFeature MapFeature(Biome biome)
        {
            if (biome != Biome.Water && rnd.Next(0, 100) <= forestSpawnRate)
            {
                return TileFeature.Forest;
            }
            else if (biome == Biome.Grass && rnd.Next(0, 100) <= hillSpawnRate)
            {
                return TileFeature.Hill;
            }

            if (biome == Biome.Water && rnd.Next(0, 100) <= waveSpawnRate)
            {
                return TileFeature.Wave;
            }

            return TileFeature.None;
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
    }
}