using Scorpia.Game.World;
using Scorpian.Asset;

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