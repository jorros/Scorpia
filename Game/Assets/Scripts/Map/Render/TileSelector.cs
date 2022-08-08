using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Map.Render
{
	public class TileSelector
	{
        private readonly Dictionary<Direction, IReadOnlyList<int>> mapping;
        private readonly RandomIndex index2;
        private readonly RandomIndex index3;

        public TileSelector(Dictionary<Direction, IReadOnlyList<int>> mapping)
        {
            this.mapping = mapping;
            index2 = new RandomIndex(2);
            index3 = new RandomIndex(3);
        }

		public int? GetIndex(Direction direction)
        {
            if (!mapping.ContainsKey(direction))
            {
                return null;
            }
            
            var mappings = mapping[direction];

            return mappings.Count switch
            {
                2 => mappings[index2.Next(0)],
                3 => mappings[index3.Next(0)],
                _ => mappings[0]
            };
        }
    }
}

