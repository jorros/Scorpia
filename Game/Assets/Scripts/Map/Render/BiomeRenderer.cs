using System;
using UnityEngine.Tilemaps;
using Utils;

namespace Map.Render
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

            public Tile[] Barren;

            public Tile[] BarrenClear;
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
                {Biome: Biome.Water} t when t.HasFeature(MapTileFeature.Wave) => tiles.Wave[counter.Next(0)],
                { Biome: Biome.Water } => tiles.Water,
                { Biome: Biome.Grass, Fertility: Fertility.Low, Location: not null } t when t.HasFeature(MapTileFeature.Hill) => tiles.BarrenClear[counter.Next(0)],
                { Biome: Biome.Grass, Fertility: Fertility.Low } t when t.HasFeature(MapTileFeature.Hill) => tiles.Barren[counter.Next(0)],
                
                { Biome: Biome.Grass, Fertility: Fertility.Normal, Location: not null } t when t.HasFeature(MapTileFeature.Hill) => tiles.HillClear[counter.Next(0)],
                { Biome: Biome.Grass, Fertility: Fertility.Normal } t when t.HasFeature(MapTileFeature.Hill) => tiles.Hill[counter.Next(0)],
                
                { Biome: Biome.Grass, Location: not null } t when t.HasFeature(MapTileFeature.Hill) => tiles.HillClear[counter.Next(10)],
                { Biome: Biome.Grass } t when t.HasFeature(MapTileFeature.Hill) => tiles.Hill[counter.Next(10)],
                
                { Biome: Biome.Grass, Fertility: Fertility.Low, Location: not null } t when t.HasFeature(MapTileFeature.Forest) => tiles.ForestClear[counter.Next(0)],
                { Biome: Biome.Grass, Fertility: Fertility.Low } t when t.HasFeature(MapTileFeature.Forest) => tiles.Forest[counter.Next(0)],
                
                { Biome: Biome.Grass, Location: not null } t when t.HasFeature(MapTileFeature.Forest) => tiles.ForestClear[counter.Next(10)],
                { Biome: Biome.Grass } t when t.HasFeature(MapTileFeature.Forest) => tiles.Forest[counter.Next(10)],
                
                { Biome: Biome.Grass, Fertility: Fertility.Low, Location: not null } => tiles.BarrenClear[counter.Next(0)],
                { Biome: Biome.Grass, Fertility: Fertility.Low } => tiles.Barren[counter.Next(0)],
                
                { Biome: Biome.Grass, Fertility: Fertility.Normal, Location: not null } => tiles.GrassClear[counter.Next(0)],
                { Biome: Biome.Grass, Fertility: Fertility.Normal } => tiles.Grass[counter.Next(0)],
                
                { Biome: Biome.Grass, Location: not null } => tiles.GrassClear[counter.Next(10)],
                { Biome: Biome.Grass } => tiles.Grass[counter.Next(10)],
                
                { Biome: Biome.Mountain } t when t.HasFeature(MapTileFeature.Forest) => tiles.MountainForest[counter.Next(0)],
                { Biome: Biome.Mountain } => tiles.Mountain[counter.Next(0)],
                _ => tiles.Grass[counter.Next(0)]
            };
    }
}

