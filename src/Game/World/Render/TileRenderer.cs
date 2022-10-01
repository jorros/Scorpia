using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.HexMap;

namespace Scorpia.Game.World.Render;

public abstract class TileRenderer
{
    private readonly AssetManager _assetManager;

    public TileRenderer(HexMapLayer<MapTile> layer, AssetManager assetManager)
    {
        _assetManager = assetManager;
        Layer = layer;
    }
    
    public HexMapLayer<MapTile> Layer { get; }

    public abstract Sprite? GetTile(MapTile tile);

    protected Sprite GetSprite(string name, int index)
    {
        return _assetManager.Get<Sprite>($"Game:{name}_{index}");
    }
    
    protected Sprite GetSprite(string name)
    {
        return _assetManager.Get<Sprite>($"Game:{name}");
    }
}