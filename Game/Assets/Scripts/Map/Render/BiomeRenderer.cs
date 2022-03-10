using System;
using System.Collections.Generic;
using Scorpia.Assets.Scripts.Utils;
using UnityEngine.Tilemaps;

namespace Scorpia.Assets.Scripts.Map.Render
{
    public class BiomeRenderer : ITileRenderer
    {
        private readonly Tilemap layer;
        private readonly BiomeRendererTiles tiles;

        private readonly RandomIndex counter;

        [Serializable]
        public class BiomeRendererTiles
        {
            public Tile[] Grass;

            public Tile[] GrassClear;

            public Tile Water;

            public Tile[] Wave;

            public Tile[] Hill;

            public Tile[] HillClear;

            public Tile[] Mountain;

            public Tile[] Forest;

            public Tile[] ForestClear;

            public Tile[] MountainForest;
        }

        public BiomeRenderer(Tilemap layer, BiomeRendererTiles tiles)
        {
            this.layer = layer;
            this.tiles = tiles;

            counter = new RandomIndex(10);
        }

        public Tilemap Layer => layer;

        public Tile GetTile(MapTile tile) =>
            tile switch
            {
                { Biome: Biome.Water, Feature: TileFeature.Wave } => tiles.Wave[counter.Next(0)],
                { Biome: Biome.Water } => tiles.Water,
                { Biome: Biome.Grass, Feature: TileFeature.Hill } => tiles.Hill[counter.Next(0)],
                { Biome: Biome.Grass, Feature: TileFeature.Forest } => tiles.Forest[counter.Next(0)],
                { Biome: Biome.Grass } => tiles.Grass[counter.Next(0)],
                { Biome: Biome.Mountain, Feature: TileFeature.Forest } => tiles.MountainForest[counter.Next(0)],
                { Biome: Biome.Mountain } => tiles.Mountain[counter.Next(0)],
                _ => tiles.Grass[counter.Next(0)]
            };
    }
}

