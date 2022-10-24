using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.HexMap;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Utils;
using Scorpia.Game.World;
using Scorpia.Game.World.Generation;
using Scorpia.Game.World.Render;

namespace Scorpia.Game.Nodes;

public class MapNode : Node
{
    public HexMap<MapTile> Map { get; private set; } = null!;
    public Random Rnd { get; private set; } = new();
    public int Width { get; private set; }
    public int Height { get; private set; }
    public SizeF WorldSize { get; private set; }

    private const float Scale = 20;
    private const int Octaves = 5;
    private const float Persistence = 0.35f;
    private const float Lacunarity = 5f;

    private IReadOnlyList<IGenerator> _generators = null!;
    private IReadOnlyList<TileRenderer> _renderers = null!;
    private HexMapLayer<MapTile> _selectedLayer = null!;

    private Sprite _selectedTile = null!;

    public override void OnInit()
    {
        Width = 60;
        Height = 40;

        _generators = new IGenerator[]
        {
            new BiomeGenerator(),
            // new RiverGenerator(),
            new FertilityGenerator(),
            new ResourceGenerator()
        };

        var tileSize = new Size(95, 95);
        Map = new HexMap<MapTile>(Width, Height, tileSize, new PointF(tileSize.Width, tileSize.Height), hex => new MapTile(hex));

        WorldSize = new SizeF(Map.Size.Width + tileSize.Width * 2, Map.Size.Height + tileSize.Height * 2);

        var biomeLayer = Map.AddLayer();
        var riverLayer = Map.AddLayer();
        var locationsLayer = Map.AddLayer();
        var fowLayer = Map.AddLayer();
        var flairLayer = Map.AddLayer();
        var tempLayer = Map.AddLayer();
        _selectedLayer = Map.AddLayer();

        var assetManager = ServiceProvider.GetService<AssetManager>();

        if (assetManager is null)
        {
            return;
        }
        
        _renderers = new TileRenderer[]
        {
            new BiomeRenderer(biomeLayer, AssetManager),
            new LocationsRenderer(locationsLayer, AssetManager),
            new FlairTileRenderer(flairLayer, AssetManager)
        };
        
        _selectedTile = assetManager.Get<Sprite>("Game:MAP/selected_tile");
            
        AttachComponent(new MapNodeCamera());
    }

    public void Generate(int seed)
    {
        Rnd = new Random(seed);

        foreach (var tile in Map)
        {
            Map.GetData(tile).Neighbours = Map.GetNeighbours(tile);
        }

        var noiseMap = new NoiseMap(Width, Height);
        noiseMap.Generate(seed, Scale, Octaves, Persistence, Lacunarity, new PointF(0, 0));

        foreach (var generator in _generators)
        {
            generator.Generate(this, noiseMap);
        }
    }

    public void RefreshTilemap()
    {
        foreach (var renderer in _renderers)
        {
            foreach (var position in Map)
            {
                var mapTile = Map.GetData(position);
                var sprite = renderer.GetTile(mapTile);
                renderer.Layer.SetSprite(mapTile.Position, sprite);
            }
        }
    }

    [Event(nameof(SelectTile))]
    public void SelectTile(MapTile select)
    {
        _selectedLayer.Clear();
        _selectedLayer.SetSprite(select.Position, _selectedTile);
    }

    [Event(nameof(DeselectTile))]
    public void DeselectTile()
    {
        _selectedLayer.Clear();
    }

    public override void OnRender(RenderContext context, float dT)
    {
        Map.Render(context);
    }
}