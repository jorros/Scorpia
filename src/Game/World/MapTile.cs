using System.Drawing;
using Scorpia.Engine;
using Scorpia.Engine.Maths;
using Scorpia.Game.Nodes;

namespace Scorpia.Game.World;

public class MapTile : IEquatable<MapTile>
{
    public MapTile(Point position)
    {
        Position = position;
        _features = new HashSet<MapTileFeature>();
    }

    public Biome Biome { get; set; }

    private readonly HashSet<MapTileFeature> _features;

    public IEnumerable<MapTileFeature> Features => _features;

    public IReadOnlyList<MapTile> Neighbours { get; set; }

    public Fertility Fertility { get; set; }

    public Resource Resource { get; set; }

    public Direction? River { get; set; }

    public Point Position { get; }

    public LocationNode Location { get; set; }

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
            Direction.SouthEast, Direction.East, Direction.NorthEast, Direction.NorthWest, Direction.West,
            Direction.SouthWest
        };

        var vector = to.Position.ToCube() - Position.ToCube();
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

    public double DistanceTo(MapTile other)
    {
        return Math.Sqrt((Math.Pow(Position.X - other.Position.X, 2) + Math.Pow(Position.Y - other.Position.Y, 2)));
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