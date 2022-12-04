using Scorpia.Game.Nodes.Entities;
using Scorpian.Asset;
using Scorpian.HexMap;

namespace Scorpia.Game.World.Render;

public class LocationsRenderer : TileRenderer
{
    public LocationsRenderer(HexMapLayer<MapTile> layer, AssetManager assetManager) : base(layer, assetManager)
    {
    }

    public override Sprite? GetTile(MapTile tile) => tile.Location?.Type.Value switch
    {
        (byte)LocationType.Village => GetSprite("overlay_location_village"),
        (byte)LocationType.City => GetSprite("overlay_location_city"),
        (byte)LocationType.Outpost => GetSprite("overlay_location_outpost"),
        (byte)LocationType.Farmland => GetSprite("overlay_location_farmland"),
        (byte)LocationType.Fortification => GetSprite("overlay_location_fortification"),
        (byte)LocationType.Mine => GetSprite("overlay_location_mine"),
        (byte)LocationType.University => GetSprite("overlay_location_university"),
        _ => null
    };
}