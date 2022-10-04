using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Requirements;

public abstract class Requirement : IComparable<Requirement>
{
    public abstract int Value { get; }

    public abstract int Index { get; }

    public abstract bool IsFulfilled(PlayerNode player, MapTile mapTile);

    public int CompareTo(Requirement other)
    {
        return Index - other.Index;
    }
}