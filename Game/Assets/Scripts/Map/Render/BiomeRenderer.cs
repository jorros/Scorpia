using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace Scorpia.Assets.Scripts.Map.Render
{
    public class BiomeRenderer : ITileRenderer
    {
        private readonly Tilemap layer;
        private readonly IReadOnlyList<Tile> grass;
        private readonly Tile water;
        private readonly IReadOnlyList<Tile> wave;
        private readonly IReadOnlyList<Tile> hill;
        private readonly IReadOnlyList<Tile> mountain;
        private readonly IReadOnlyList<Tile> forest;
        private readonly IReadOnlyList<Tile> mountainForest;

        private readonly RandomIndex grassCounter;
        private readonly RandomIndex waveCounter;
        private readonly RandomIndex hillCounter;
        private readonly RandomIndex mountainCounter;
        private readonly RandomIndex forestCounter;
        private readonly RandomIndex mountainForestCounter;

        public BiomeRenderer(Tilemap layer, IReadOnlyList<Tile> grass, Tile water, IReadOnlyList<Tile> wave, IReadOnlyList<Tile> hill, IReadOnlyList<Tile> mountain, IReadOnlyList<Tile> forest, IReadOnlyList<Tile> mountainForest)
        {
            this.layer = layer;
            this.grass = grass;
            this.water = water;
            this.wave = wave;
            this.hill = hill;
            this.mountain = mountain;
            this.forest = forest;
            this.mountainForest = mountainForest;

            grassCounter = new RandomIndex(grass.Count);
            waveCounter = new RandomIndex(wave.Count);
            hillCounter = new RandomIndex(hill.Count);
            mountainCounter = new RandomIndex(mountain.Count);
            forestCounter = new RandomIndex(forest.Count);
            mountainForestCounter = new RandomIndex(mountainForest.Count);
        }

        public Tilemap Layer => layer;

        public Tile GetTile(MapTile tile) =>
            tile switch
            {
                { Biome: Biome.Water, Feature: TileFeature.Wave } => wave[waveCounter.Next(0)],
                { Biome: Biome.Water } => water,
                { Biome: Biome.Grass, Feature: TileFeature.Hill } => hill[hillCounter.Next(0)],
                { Biome: Biome.Grass, Feature: TileFeature.Forest } => forest[forestCounter.Next(0)],
                { Biome: Biome.Grass } => grass[grassCounter.Next(0)],
                { Biome: Biome.Mountain, Feature: TileFeature.Forest } => mountainForest[mountainForestCounter.Next(0)],
                { Biome: Biome.Mountain } => mountain[mountainCounter.Next(0)],
                _ => grass[grassCounter.Next(0)]
            };
    }
}

