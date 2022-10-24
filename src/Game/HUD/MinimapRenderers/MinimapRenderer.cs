using Scorpia.Engine.Asset;
using Scorpia.Engine.HexMap;
using Scorpia.Game.World;

namespace Scorpia.Game.HUD.MinimapRenderers;

public abstract class MinimapRenderer
{
    public abstract bool ShouldRender(MapTile tile);

    public abstract void Init(AssetManager assetManager);
    
    protected Sprite[]? Sprites { get; set; }
    
    protected static Sprite[] LoadSprites(AssetManager assetManager, string name, int count)
    {
        var sprites = new Sprite[count];
        foreach (var i in Enumerable.Range(1, count))
        {
            sprites[i - 1] = assetManager.Get<Sprite>($"Game:HUD/minimap_{name}_{i}");
        }

        return sprites;
    }
    
    public Sprite? GetSprite(Hex position)
    {
        return Sprites?[Math.Abs(position.GetHashCode() % Sprites.Length)];
    }
}