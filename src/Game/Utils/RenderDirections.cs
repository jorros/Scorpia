using Scorpia.Game.World;
using Scorpian.Asset;
using Scorpian.Graphics;

namespace Scorpia.Game.Utils;

public class RenderDirections
{
    private readonly Sprite _ne;
    private readonly Sprite _nw;
    private readonly Sprite _w;
    private readonly Sprite _sw;
    private readonly Sprite _se;
    private readonly Sprite _e;

    private readonly Dictionary<Direction[], Sprite> _cache;

    public RenderDirections(AssetManager assetManager, string ne, string nw, string w, string sw, string se, string e)
    {
        _ne = assetManager.Get<Sprite>(ne);
        _nw = assetManager.Get<Sprite>(nw);
        _w = assetManager.Get<Sprite>(w);
        _sw = assetManager.Get<Sprite>(sw);
        _se = assetManager.Get<Sprite>(se);
        _e = assetManager.Get<Sprite>(e);

        _cache = new Dictionary<Direction[], Sprite>();
    }

    public void GenerateCombinations(RenderContext renderContext)
    {
        var directions = new[]
        {
            Direction.East,
            Direction.West,
            Direction.NorthEast,
            Direction.NorthWest,
            Direction.SouthEast,
            Direction.SouthWest
        };

        var combinations = Enumerable
            .Range(0, 1 << (directions.Length))
            .Select(index => directions
                .Where((_, i) => (index & (1 << i)) != 0)
                .ToArray())
            .Where(x => x.Length > 2);

        foreach (var combination in combinations)
        {
            var sprites = combination.Select(MapSprite);
            _cache[combination] = renderContext.MergeSprites(sprites);
        }
    }

    private Sprite MapSprite(Direction direction) => direction switch
    {
        Direction.East => _e,
        Direction.West => _w,
        Direction.NorthEast => _ne,
        Direction.NorthWest => _nw,
        Direction.SouthEast => _se,
        Direction.SouthWest => _sw,
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };

    public Sprite Get(IEnumerable<Direction> directions)
    {
        var dir = directions.Distinct().ToArray();
        return _cache.First(x => x.Key.All(dir.Contains) && x.Key.Length == dir.Length).Value;
    }
}