using Scorpia.Engine.Asset;
using Scorpia.Game.World;

namespace Scorpia.Game.HUD.MinimapRenderers;

public class TreeMinimapRenderer : MinimapRenderer
{
    public override bool ShouldRender(MapTile tile)
    {
        return tile.Features.Contains(MapTileFeature.Forest);
    }

    public override void Init(AssetManager assetManager)
    {
        Sprites = LoadSprites(assetManager, "tree", 5);
    }
}