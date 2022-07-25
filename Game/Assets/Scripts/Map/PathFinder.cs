using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map
{
    public class PathFinder
    {
        private readonly Map map;

        public PathFinder(Map map)
        {
            this.map = map;
        }

        public IReadOnlyList<MapTile> Find(MapTile start, MapTile end)
        {
            var openPathTiles = new List<PFTile>();
            var closedPathTiles = new List<PFTile>();

            var currentTile = new PFTile(start)
            {
                G = 0,
                H = GetEstimatedPathCost(start.HexPosition, end.HexPosition)
            };

            openPathTiles.Add(currentTile);

            while (openPathTiles.Count != 0)
            {
                openPathTiles = openPathTiles.OrderBy(x => x.F).ThenByDescending(x => x.G).ToList();
                currentTile = openPathTiles[0];

                openPathTiles.Remove(currentTile);
                closedPathTiles.Add(currentTile);

                var g = currentTile.G + 1;

                if (openPathTiles.Exists(x => x.MapTile == end))
                {
                    break;
                }

                var adjacentTiles = currentTile.MapTile.Neighbours;
                foreach (var adjacentTile in adjacentTiles)
                {
                    if (closedPathTiles.Exists(x => x.MapTile == adjacentTile))
                    {
                        continue;
                    }

                    var adjacentPFTile = openPathTiles.FirstOrDefault(x => x.MapTile == adjacentTile);
                    if (adjacentPFTile == null)
                    {
                        openPathTiles.Add(new PFTile(adjacentTile)
                        {
                            G = g,
                            H = GetEstimatedPathCost(adjacentTile.HexPosition, end.HexPosition)
                        });
                    }
                    else if (adjacentPFTile.F > g + adjacentPFTile.H)
                    {
                        adjacentPFTile.G = g;
                    }
                }
            }

            var finalPathTiles = new List<MapTile>();

            var endTile = closedPathTiles.FirstOrDefault(x => x.MapTile == end);

            if (endTile == null)
            {
                return finalPathTiles;
            }
            
            currentTile = endTile;
            finalPathTiles.Add(currentTile.MapTile);

            for (var i = endTile.G - 1; i >= 0; i--)
            {
                currentTile = closedPathTiles.Find(x =>
                    x.G == i && currentTile.MapTile.Neighbours.Contains(x.MapTile));
                finalPathTiles.Add(currentTile.MapTile);
            }

            finalPathTiles.Reverse();


            return finalPathTiles;
        }

        private static int GetEstimatedPathCost(Vector3Int startPosition, Vector3Int targetPosition)
        {
            return Mathf.Max(Mathf.Abs(startPosition.z - targetPosition.z),
                Mathf.Max(Mathf.Abs(startPosition.x - targetPosition.x),
                    Mathf.Abs(startPosition.y - targetPosition.y)));
        }

        private class PFTile
        {
            public PFTile(MapTile tile)
            {
                MapTile = tile;
            }

            public int F => G + H;

            public int G { get; set; }

            public int H { get; set; }

            public MapTile MapTile { get; }
        }
    }
}