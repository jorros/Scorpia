using Scorpia.Engine.Asset;
using Scorpia.Engine.HexMap;
using Scorpia.Game.Utils;

namespace Scorpia.Game.World.Render;

public class RiverRenderer : TileRenderer
{
    private readonly RenderDirections _renderDirections;

    public RiverRenderer(HexMapLayer<MapTile> layer, AssetManager assetManager, RenderDirections renderDirections) : base(layer, assetManager)
    {
        _renderDirections = renderDirections;
    }

    public override Sprite? GetTile(MapTile tile)
    {
        if (tile.River is null)
        {
            return null;
        }

        switch (tile.River.Count)
        {
            case > 2:
                return _renderDirections.Get(tile.River);
            case 1:
                return tile.River[0] switch
                {
                    Direction.West => GetRandom(tile.Position, "c_w", 2),
                    Direction.East => GetRandom(tile.Position, "c_e", 2),
                    Direction.SouthEast => GetRandom(tile.Position, "c_se", 3),
                    Direction.SouthWest => GetRandom(tile.Position, "c_sw", 3),
                    Direction.NorthWest => GetRandom(tile.Position, "c_nw", 3),
                    Direction.NorthEast => GetRandom(tile.Position, "c_ne", 3),
                    _ => throw new ArgumentOutOfRangeException(nameof(tile.River), "Unmapped river tile")
                };
        }

        if (tile.River.Contains(Direction.East))
        {
            if (tile.River.Contains(Direction.SouthEast))
            {
                return GetRandom(tile.Position, "e_se", 2);
            }

            if (tile.River.Contains(Direction.SouthWest))
            {
                return GetRandom(tile.Position, "e_sw", 2);
            }

            if (tile.River.Contains(Direction.West))
            {
                return GetRandom(tile.Position, "e_w", 3);
            }
        }

        if (tile.River.Contains(Direction.NorthEast))
        {
            if (tile.River.Contains(Direction.East))
            {
                return GetRandom(tile.Position, "ne_e", 3);
            }

            if (tile.River.Contains(Direction.SouthEast))
            {
                return GetRandom(tile.Position, "ne_se", 3);
            }

            if (tile.River.Contains(Direction.SouthWest))
            {
                return GetRandom(tile.Position, "ne_sw", 3);
            }

            if (tile.River.Contains(Direction.West))
            {
                return GetRandom(tile.Position, "ne_w", 3);
            }
        }

        if (tile.River.Contains(Direction.NorthWest))
        {
            if (tile.River.Contains(Direction.East))
            {
                return GetRandom(tile.Position, "nw_e", 3);
            }

            if (tile.River.Contains(Direction.NorthEast))
            {
                return GetRandom(tile.Position, "nw_ne", 2);
            }

            if (tile.River.Contains(Direction.SouthWest))
            {
                return GetRandom(tile.Position, "nw_sw", 3);
            }

            if (tile.River.Contains(Direction.SouthEast))
            {
                return GetRandom(tile.Position, "nw_se", 3);
            }

            if (tile.River.Contains(Direction.West))
            {
                return GetRandom(tile.Position, "nw_w", 3);
            }
        }

        if (tile.River.Contains(Direction.SouthEast))
        {
            if (tile.River.Contains(Direction.SouthWest))
            {
                return GetRandom(tile.Position, "se_sw", 2);
            }

            if (tile.River.Contains(Direction.West))
            {
                return GetRandom(tile.Position, "se_w", 2);
            }
        }

        if (tile.River.Contains(Direction.SouthWest))
        {
            if (tile.River.Contains(Direction.West))
            {
                return GetRandom(tile.Position, "sw_w", 2);
            }
        }

        return tile.River[0] switch
        {
            Direction.West => GetRandom(tile.Position, "c_w", 2),
            Direction.East => GetRandom(tile.Position, "c_e", 2),
            Direction.SouthEast => GetRandom(tile.Position, "c_se", 3),
            Direction.SouthWest => GetRandom(tile.Position, "c_sw", 3),
            Direction.NorthWest => GetRandom(tile.Position, "c_nw", 3),
            Direction.NorthEast => GetRandom(tile.Position, "c_ne", 3),
            _ => throw new ArgumentOutOfRangeException(nameof(tile.River), "Unmapped river tile")
        };
    }

    private Sprite GetRandom(Hex position, string name, int count)
    {
        var index = Math.Abs(position.GetHashCode() % count) + 1;

        if (index > 1)
        {
            name = $"{name}_{index}";
        }

        return GetSprite($"overlay_river_{name}");
    }
}