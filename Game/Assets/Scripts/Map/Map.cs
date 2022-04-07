using System;
using System.Collections.Generic;
using System.Linq;
using Map.Generation;
using UnityEngine;
using Utils;

namespace Map
{
    public class Map
    {
        private MapTile[] tiles;
        private int seed, width, height;
        private System.Random rnd;

        private IReadOnlyList<IGenerator> generators;

        public int Seed => seed;
        public System.Random Rnd => rnd;
        public int Width => width;
        public int Height => height;
        public IReadOnlyList<MapTile> Tiles => tiles;

        public Map(int width, int height, int seed)
        {
            tiles = new MapTile[width * height];
            this.seed = seed;
            this.width = width;
            this.height = height;
            this.rnd = new System.Random(seed);

            generators = new IGenerator[]
            {
                new BiomeGenerator(),
                new RiverGenerator(),
                new FertilityGenerator(),
                new ResourceGenerator()
            };
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

        public void SetTile(int x, int y, MapTile tile)
        {
            tiles[y * width + x] = tile;
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
            foreach(var generator in generators)
            {
                generator.Generate(this);
            }
        }       
    }
}