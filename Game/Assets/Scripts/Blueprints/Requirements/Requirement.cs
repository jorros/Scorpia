using System;
using Actors;
using Map;

namespace Blueprints.Requirements
{
    public abstract class Requirement : IComparable<Requirement>
    {
        public abstract int Value { get; }
        
        public abstract int Index { get; }

        public abstract bool IsFulfilled(Player player, MapTile mapTile);
        
        public int CompareTo(Requirement other)
        {
            return Index - other.Index;
        }
    }
}