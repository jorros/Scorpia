using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;

namespace Scorpia.Game.World.Render;

public abstract class TileRenderer
{
    private readonly AssetManager _assetManager;

    public TileRenderer(TilemapLayer layer, AssetManager assetManager)
    {
        _assetManager = assetManager;
        Layer = layer;
    }
    
    public TilemapLayer Layer { get; }

    public abstract Sprite GetTile(MapTile tile);

    protected Sprite GetSprite(string name, int index)
    {
        return _assetManager.Get<Sprite>($"Game:{name}_{index}");
    }
}