using Scorpia.Game.Nodes;
using Scorpian.HexMap;

namespace Scorpia.Game.World;

public class MapTile : IEquatable<MapTile>
{
    public MapTile(Hex position)
    {
        Position = position;
        _features = new HashSet<MapTileFeature>();
    }
    
    public Hex Position { get; set; }

    public Biome Biome { get; set; }

    private readonly HashSet<MapTileFeature> _features;

    public IEnumerable<MapTileFeature> Features => _features;

    public IReadOnlyList<MapTile> Neighbours { get; set; }

    public Fertility Fertility { get; set; }

    public Resource Resource { get; set; }

    public IReadOnlyList<Direction>? River { get; set; }

    public LocationNode? Location { get; set; }

    public bool HasFeature(MapTileFeature feature) => _features.Contains(feature);

    public void AddFeature(MapTileFeature feature) => _features.Add(feature);

    public Direction? GetDirection(MapTile to)
    {
        var vectors = new[]
        {
            new Hex(1, -1, 0), new Hex(1, 0, -1), new Hex(0, 1, -1), new Hex(-1, 1, 0),
            new Hex(-1, 0, 1), new Hex(0, -1, 1)
        };
        var directions = new[]
        {
            Direction.NorthEast, Direction.East, Direction.SouthEast, Direction.SouthWest, Direction.West,
            Direction.NorthWest
        };

        var vector = to.Position - Position;
        var index = -1;

        for (var i = 0; i < vectors.Length; i++)
        {
            if (vectors[i] == vector)
            {
                index = i;
                break;
            }
        }

        if (index < 0)
        {
            return null;
        }

        return directions[index];
    }

    public bool Equals(MapTile? other)
    {
        return Position == other?.Position;
    }

    public override bool Equals(object? obj)
    {
        return obj is MapTile other && Position == other.Position;
    }

    public override int GetHashCode() => Position.GetHashCode();

    public override string ToString()
    {
        return Position.ToString() ?? string.Empty;
    }

    public static bool operator ==(MapTile? lhs, MapTile? rhs)
    {
        if (lhs is not null)
        {
            return lhs.Equals(rhs);
        }

        return rhs is null;
    }

    public static bool operator !=(MapTile lhs, MapTile rhs) => !(lhs == rhs);
}