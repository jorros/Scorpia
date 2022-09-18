using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Utils;
using Scorpia.Game.World;
using Scorpia.Game.World.Generation;

namespace Scorpia.Game.Nodes;

public class MapNode : Node
{
    public Tilemap Map { get; private set; } = null!;
    public Random Rnd { get; private set; } = new();
    public MapTile[] Tiles { get; private set; } = null!;
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    private const float Scale = 20;
    private const int Octaves = 5;
    private const float Persistence = 0.35f;
    private const float Lacunarity = 5f;
    
    private IReadOnlyList<IGenerator> _generators = null!;

    public override void OnInit()
    {
        Width = 60;
        Height = 40;
        Tiles = new MapTile[Width * Height];

        _generators = new IGenerator[]
        {
            new BiomeGenerator(),
            // new RiverGenerator(),
            new FertilityGenerator(),
            new ResourceGenerator()
        };

        Map = new Tilemap(Width, Height, new OffsetVector(77, 77), TilemapOrientation.Pointy);
        var ground = Map.AddLayer();
        var river = Map.AddLayer();
        var locations = Map.AddLayer();
        var fow = Map.AddLayer();
        var flair = Map.AddLayer();
        var temp = Map.AddLayer();
        var selected = Map.AddLayer();

        var assetManager = ServiceProvider.GetService<AssetManager>();

        if (assetManager is not null)
        {
            var grass = assetManager.Get<Sprite>("Game:tile_grassland_dense_clear_green_4");
            for (var y = 0; y < 20; y++)
            {
                for (var x = 0; x < 20; x++)
                {
                    ground.SetTile(new OffsetVector(x, y), grass);
                }
            }
        }
    }

    public void Generate(int seed)
    {
        Rnd = new Random(seed);
        for (var x = 0; x < Width; x++)
        {
            for (var y = 0; y < Height; y++)
            {
                var tile = new MapTile(new OffsetVector(x, y));

                Tiles[y * Width + x] = tile;
            }
        }

        foreach (var tile in Tiles)
        {
            tile.Neighbours = GetNeighbours(tile);
        }
        
        var noiseMap = new NoiseMap(Width, Height);
        noiseMap.Generate(seed, Scale, Octaves, Persistence, Lacunarity, new OffsetVectorF(0, 0));
            
        foreach(var generator in _generators)
        {
            generator.Generate(this, noiseMap);
        }
    }
    
    public MapTile? GetTile(OffsetVector position)
    {
        if (position.X < 0 || position.Y < 0 || position.X >= Width || position.Y >= Height)
        {
            return null;
        }

        return Tiles[position.Y * Width + position.X];
    }

    public MapTile? GetTile(CubeVector position)
    {
        return GetTile(position.ToOffset());
    }
    
    public bool HasNeighbour(MapTile start, Func<MapTile, bool> predicate, int range = 1)
    {
        var min = -range;

        var position = start.Position.ToCube();

        for (var q = min; q <= range; q++)
        {
            for (var r = min; r <= range; r++)
            {
                for (var s = min; s <= range; s++)
                {
                    // Sum of cube coordinates should equal 0
                    if(q + r + s != 0)
                    {
                        continue;
                    }

                    var pos = new CubeVector(q, r, s);
                    var tile = GetTile(pos + position);
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
    
    public IReadOnlyList<MapTile> GetNeighbours(MapTile start, Func<MapTile, bool>? predicate = null, int range = 1)
    {
        var list = new List<MapTile>();

        var min = -range;

        var position = start.Position.ToCube();

        for (var q = min; q <= range; q++)
        {
            for (var r = min; r <= range; r++)
            {
                for (var s = min; s <= range; s++)
                {
                    // Sum of cube coordinates should equal 0
                    if(q + r + s != 0)
                    {
                        continue;
                    }

                    var pos = new CubeVector(q, r, s);
                    var tile = GetTile(pos + position);
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

    public override void OnRender(RenderContext context)
    {
        Map.Render(context);
    }
}