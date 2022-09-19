using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Game.Utils;

namespace Scorpia.Game.World.Render;

public class BiomeRenderer : TileRenderer
{
    private readonly IndexHelper _counter = new(10);

    public BiomeRenderer(TilemapLayer layer, AssetManager assetManager) : base(layer, assetManager)
    {
    }

    public override Sprite GetTile(MapTile tile) =>
        tile switch
        {
            {Biome: Biome.Water} when tile.HasFeature(MapTileFeature.Wave) => GetSprite("tile_ocean_waves_small_dark",
                _counter.Next()),
            {Biome: Biome.Water} => GetSprite("tile_ocean_plain_dark", 0),
            {Biome: Biome.Grass, Fertility: Fertility.Low, Location: not null} when tile.HasFeature(MapTileFeature.Hill)
                => GetSprite("tile_moor_sparse_clear_blue", _counter.Next()),
            {Biome: Biome.Grass, Fertility: Fertility.Low} when tile.HasFeature(MapTileFeature.Hill) => GetSprite(
                "tile_moor_sparse_covered_blue", _counter.Next()),

            {Biome: Biome.Grass, Fertility: Fertility.Normal, Location: not null} when
                tile.HasFeature(MapTileFeature.Hill) => GetSprite(
                    "tile_hills_sparse_clear_green", _counter.Next()),
            {Biome: Biome.Grass, Fertility: Fertility.Normal} when tile.HasFeature(MapTileFeature.Hill) => GetSprite(
                "tile_hills_sparse_covered_green", _counter.Next()),

            {Biome: Biome.Grass, Location: not null} when tile.HasFeature(MapTileFeature.Hill) => GetSprite(
                "tile_hills_dense_clear_green", _counter.Next()),
            {Biome: Biome.Grass} when tile.HasFeature(MapTileFeature.Hill) => GetSprite(
                "tile_hills_dense_covered_green", _counter.Next()),

            {Biome: Biome.Grass, Fertility: Fertility.Low, Location: not null} when
                tile.HasFeature(MapTileFeature.Forest) => GetSprite(
                    "tile_forest_deciduous_sparse_clear_green", _counter.Next()),
            {Biome: Biome.Grass, Fertility: Fertility.Low} when tile.HasFeature(MapTileFeature.Forest) => GetSprite(
                "tile_forest_deciduous_sparse_covered_green", _counter.Next()),

            {Biome: Biome.Grass, Location: not null} when tile.HasFeature(MapTileFeature.Forest) => GetSprite(
                "tile_forest_deciduous_dense_clear_green", _counter.Next()),
            {Biome: Biome.Grass} t when t.HasFeature(MapTileFeature.Forest) => GetSprite(
                "tile_forest_deciduous_dense_covered_green", _counter.Next()),

            {Biome: Biome.Grass, Fertility: Fertility.Low, Location: not null} => GetSprite(
                "tile_moor_sparse_clear_blue", _counter.Next()),
            {Biome: Biome.Grass, Fertility: Fertility.Low} => GetSprite(
                "tile_moor_sparse_covered_blue", _counter.Next()),

            {Biome: Biome.Grass, Fertility: Fertility.Normal, Location: not null} => GetSprite(
                "tile_grassland_sparse_clear_green", _counter.Next()),
            {Biome: Biome.Grass, Fertility: Fertility.Normal} => GetSprite(
                "tile_grassland_sparse_covered_green", _counter.Next()),

            {Biome: Biome.Grass, Location: not null} => GetSprite(
                "tile_grassland_dense_clear_green", _counter.Next()),
            {Biome: Biome.Grass} => GetSprite(
                "tile_grassland_dense_covered_green", _counter.Next()),

            {Biome: Biome.Mountain} when tile.HasFeature(MapTileFeature.Forest) => GetSprite(
                "tile_mountain_peak_forest_deciduous_green", _counter.Next()),
            {Biome: Biome.Mountain} => GetSprite(
                "tile_mountain_peak_grassland_green", _counter.Next()),
            _ => GetSprite("tile_grassland_sparse_covered_green", _counter.Next())
        };
}