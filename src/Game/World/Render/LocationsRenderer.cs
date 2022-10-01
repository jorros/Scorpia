using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.HexMap;
using Scorpia.Game.Location;

namespace Scorpia.Game.World.Render;

public class LocationsRenderer : TileRenderer
{
    public LocationsRenderer(HexMapLayer<MapTile> layer, AssetManager assetManager) : base(layer, assetManager)
    {
    }

    public override Sprite? GetTile(MapTile tile) => tile.Location?.Type.Value switch
    {
        LocationType.Village => GetSprite("overlay_location_village"),
        LocationType.Town => GetSprite("overlay_location_town"),
        LocationType.City => GetSprite("overlay_location_town"),
        LocationType.Outpost => GetSprite("overlay_location_outpost"),
        LocationType.Fob => GetSprite("overlay_location_fob"),
        LocationType.MilitaryBase => GetSprite("overlay_location_militarybase"),
        _ => null
    };
}