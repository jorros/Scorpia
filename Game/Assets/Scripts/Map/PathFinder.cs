using System.Collections.Generic;
using System.Linq;
using DataStructures.PriorityQueue;
using UnityEngine;

namespace Map
{
    public static class PathFinder
    {
        public static IReadOnlyList<MapTile> Find(MapTile startTile, MapTile endTile)
        {
            var start = new Node(startTile);
            var end = new Node(endTile);
            
            var openList = new PriorityQueue<Node, float>(0);
            var closedList = new List<Node>();
            var path = new Stack<Node>();
            var current = start;

            openList.Insert(start, start.F);

            while (openList.Count != 0 && !closedList.Contains(end))
            {
                current = openList.Pop();
                closedList.Add(current);

                var neighbours = current.MapTile.Neighbours.Select(x => new Node(x));

                foreach (var n in neighbours)
                {
                    if (closedList.Contains(n))
                    {
                        continue;
                    }
                    
                    var isFound = false;
                    foreach (var oLNode in openList.UnorderedItems)
                    {
                        if (oLNode.Equals(n))
                        {
                            isFound = true;
                        }
                    }

                    if (isFound)
                    {
                        continue;
                    }
                    
                    n.Parent = current;
                    n.DistanceToTarget = GetEstimatedPathCost(n.MapTile.HexPosition, endTile.HexPosition);
                    n.Cost = n.Weight + n.Parent.Cost;
                    openList.Insert(n, n.F);
                }
            }

            if (!closedList.Contains(end))
            {
                return path.Select(x => x.MapTile).ToList();
            }

            var temp = closedList[closedList.IndexOf(current)];
            if (temp == null)
            {
                return path.Select(x => x.MapTile).ToList();
            }

            do
            {
                path.Push(temp);
                temp = temp.Parent;
            } while (!start.Equals(temp) && temp != null);

            return path.Select(x => x.MapTile).ToList();
        }

        private static int GetEstimatedPathCost(Vector3Int startPosition, Vector3Int targetPosition)
        {
            return Mathf.Max(Mathf.Abs(startPosition.z - targetPosition.z),
                Mathf.Max(Mathf.Abs(startPosition.x - targetPosition.x),
                    Mathf.Abs(startPosition.y - targetPosition.y)));
        }

        private class Node
        {
            public Node(MapTile tile, int weight = 1)
            {
                MapTile = tile;
                DistanceToTarget = -1;
                Cost = 1;
                Weight = weight;
            }
            
            public Node Parent { get; set; }

            public int F
            {
                get
                {
                    if (DistanceToTarget != -1 && Cost != -1)
                        return DistanceToTarget + Cost;
                    else
                        return -1;
                }
            }

            public int DistanceToTarget { get; set; }
            
            public int Cost { get; set; }

            public int Weight { get; set; }

            public MapTile MapTile { get; }

            public override bool Equals(object obj)
            {
                return obj switch
                {
                    Node pfTile => MapTile.Equals(pfTile.MapTile),
                    _ => false
                };
            }

            public override int GetHashCode()
            {
                return MapTile.GetHashCode();
            }
        }
    }
}