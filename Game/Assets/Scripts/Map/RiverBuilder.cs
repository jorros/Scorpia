using System.Collections.Generic;
using System.Linq;
using Scorpia.Assets.Scripts.Utils;

namespace Scorpia.Assets.Scripts.Map
{
    public class RiverBuilder
    {
        private List<MapTile> river;
        private readonly Map map;
        private const int RiverContinuationChance = 90;
        private readonly System.Random rnd;

        public RiverBuilder(MapTile start, Map map, System.Random rnd)
        {
            this.rnd = rnd;
            this.map = map;
            river = new List<MapTile>();
            river.Add(start);
            start.River = new River();
        }

        private bool TileCriteria(MapTile tile)
        {
            if (tile.Biome == Biome.Water)
            {
                return false;
            }
            if(tile.River != null)
            {
                return false;
            }

            return true;
        }

        public void Build()
        {
            var current = 0;

            do
            {
                var currentTile = river[current];
                MapTile previousTile = null;
                Direction? flowDirection = null;
                if (current > 0)
                {
                    previousTile = river[current - 1];
                    flowDirection = map.GetDirection(previousTile, currentTile);
                }

                var potentialNextTiles = map.GetNeighbours(currentTile, (t) => TileCriteria(t) && t != previousTile);

                if(potentialNextTiles.Count == 0)
                {
                    break;
                }

                var randomList = new RandomList<MapTile>(rnd, potentialNextTiles.Select(x => {
                    var chance = 1;
                    if(flowDirection != null && map.GetDirection(currentTile, x) == flowDirection)
                    {
                        chance += 4;
                    }

                    if(x.Biome == Biome.Grass)
                    {
                        chance += 3;
                    }

                    return (x, chance);
                }));

                var nextTile = randomList.Sample();

                river.Add(nextTile);

                currentTile.River.To = map.GetDirection(currentTile, nextTile);
                nextTile.River = new River { From = currentTile.River.To?.GetOpposite() };

                current++;
            }
            while (rnd.Next(0, 100) < RiverContinuationChance);
        }

        public override string ToString()
        {
            return string.Join(";", river.Select(x => $"({x.Position.x},{x.Position.y})"));
        }
    }
}