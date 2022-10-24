using Scorpia.Engine.Asset;
using Scorpia.Game.World;

namespace Scorpia.Game.HUD.MinimapRenderers;

public class MountainMinimapRenderer : MinimapRenderer
{
    public override bool ShouldRender(MapTile tile)
    {
        return tile.Biome == Biome.Mountain;
    }

    public override void Init(AssetManager assetManager)
    {
        Sprites = LoadSprites(assetManager, "mountain", 6);
    }
}