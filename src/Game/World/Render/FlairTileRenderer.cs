using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.HexMap;

namespace Scorpia.Game.World.Render;

public class FlairTileRenderer : TileRenderer
{
    public FlairTileRenderer(HexMapLayer<MapTile> layer, AssetManager assetManager) : base(layer, assetManager)
    {
    }

    public override Sprite? GetTile(MapTile tile) =>
        tile.Resource switch
        {
            Resource.Gold => GetSprite("flair_gold"),
            Resource.Nitra => GetSprite("flair_nitra"),
            Resource.Sofrum => GetSprite("flair_sofrum"),
            Resource.Zellos => GetSprite("flair_zellos"),
            _ => null
        };
}