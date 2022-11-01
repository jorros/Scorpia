using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.HexMap;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.World.Render;

public class LocationsRenderer : TileRenderer
{
    public LocationsRenderer(HexMapLayer<MapTile> layer, AssetManager assetManager) : base(layer, assetManager)
    {
    }

    public override Sprite? GetTile(MapTile tile) => tile.Location?.Type.Value switch
    {
        LocationType.Village => GetSprite("overlay_location_village"),
        LocationType.City => GetSprite("overlay_location_city"),
        LocationType.Outpost => GetSprite("overlay_location_outpost"),
        LocationType.Farmland => GetSprite("overlay_location_farmland"),
        LocationType.Fortification => GetSprite("overlay_location_fortification"),
        LocationType.Mine => GetSprite("overlay_location_mine"),
        LocationType.University => GetSprite("overlay_location_university"),
        _ => null
    };
}