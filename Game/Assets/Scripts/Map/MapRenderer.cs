using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;
using Scorpia.Assets.Scripts.World;

namespace Scorpia.Assets.Scripts.Map
{
    public class MapRenderer : NetworkBehaviour
    {
        [SerializeField]
        public Tile[] grassTile;
        private Counter grassCounter;

        [SerializeField]
        public Tile waterTile;

        [SerializeField]
        public Tile[] waveTile;
        private Counter waveCounter;

        [SerializeField]
        public Tile[] hillTile;
        private Counter hillCounter;

        [SerializeField]
        public Tile[] mountainTile;
        private Counter mountainCounter;

        [SerializeField]
        public Tile[] forestTile;
        private Counter forestCounter;

        [SerializeField]
        public Tile[] mountainForestTile;
        private Counter mountainForestCounter;

        [SerializeField]
        public Tile[] riverTile;
        private Counter river2Counter;
        private Counter river3Counter;

        private NetworkVariable<int> width = new NetworkVariable<int>();
        private NetworkVariable<int> height = new NetworkVariable<int>();
        private NetworkVariable<int> seed = new NetworkVariable<int>();

        private Tilemap groundLayer;
        private Tilemap riverLayer;
        private CameraMovement cam;

        [HideInInspector]
        public Map map;

        void Awake()
        {
            Game.MapObject = gameObject;
            
            var tilemaps = GetComponentsInChildren<Tilemap>();
            groundLayer = tilemaps[0];
            riverLayer = tilemaps[1];

            var camObj = GameObject.FindGameObjectWithTag("MainCamera");
            cam = camObj.GetComponent<CameraMovement>();
        }

        void Start()
        {
            if (IsServer)
            {
                seed.Value = 5213351;
                height.Value = 40;
                width.Value = 60;
            }

            map = new Map(width.Value, height.Value, seed.Value);
            Refresh();
        }

        public MapTile GetTile(Vector3 pos)
        {
            var mapPos = groundLayer.WorldToCell(pos);

            return map.GetTile(mapPos.x, mapPos.y);
        }

        public void Refresh()
        {
            map.Generate();

            grassCounter = new Counter(grassTile.Length);
            waveCounter = new Counter(waveTile.Length);
            hillCounter = new Counter(hillTile.Length);
            mountainCounter = new Counter(mountainTile.Length);
            forestCounter = new Counter(forestTile.Length);
            mountainForestCounter = new Counter(mountainForestTile.Length);
            river2Counter = new Counter(2);
            river3Counter = new Counter(3);

            for (int y = 0; y < height.Value; y++)
            {
                for (int x = 0; x < width.Value; x++)
                {
                    var tile = map.GetTile(x, y);
                    groundLayer.SetTile(new Vector3Int(x, y, 0), MapTile(tile));

                    if (tile.River != null)
                    {
                        riverLayer.SetTile(new Vector3Int(x, y, 0), MapRiver(tile));
                    }
                }
            }

            cam.mapWidth = groundLayer.size.x * groundLayer.cellSize.x;
            cam.mapHeight = groundLayer.size.y * groundLayer.cellSize.y;

            print($"width: {groundLayer.size.y}; cell width: {groundLayer.cellSize.y}; cam width: {cam.mapHeight}");
        }

        private class Counter
        {
            private readonly int max;
            private int current = 0;

            public Counter(int max)
            {
                this.max = max;
            }

            public int Next()
            {
                if (current < max)
                {
                    return current++;
                }

                current = 0;
                return current;
            }
        }

        private Tile MapRiver(MapTile tile)
        {
            var river = tile.River;
            if (river.IsDirection(Direction.East, null))
            {
                // 0, 1
                return riverTile[0 + river2Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthEast, null))
            {
                // 2, 3, 4
                return riverTile[2 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthWest, null))
            {
                // 5, 6, 7
                return riverTile[5 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.SouthEast, null))
            {
                // 8, 9, 10
                return riverTile[8 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.SouthWest, null))
            {
                // 11, 12, 13
                return riverTile[11 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.West, null))
            {
                // 14, 15
                return riverTile[14 + river2Counter.Next()];
            }
            else if (river.IsDirection(Direction.East, Direction.SouthEast))
            {
                // 16, 17
                return riverTile[16 + river2Counter.Next()];
            }
            else if (river.IsDirection(Direction.East, Direction.SouthWest))
            {
                // 18, 19
                return riverTile[18 + river2Counter.Next()];
            }
            else if (river.IsDirection(Direction.East, Direction.West))
            {
                // 20, 21, 22
                return riverTile[20 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthEast, Direction.East))
            {
                // 23, 24, 25
                return riverTile[23 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthEast, Direction.SouthEast))
            {
                // 26, 27, 28
                return riverTile[26 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthEast, Direction.SouthWest))
            {
                // 29, 30, 31
                return riverTile[29 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthEast, Direction.West))
            {
                // 32, 33, 34
                return riverTile[32 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthWest, Direction.East))
            {
                // 35, 36, 37
                return riverTile[35 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthWest, Direction.NorthEast))
            {
                // 38, 39
                return riverTile[38 + river2Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthWest, Direction.SouthEast))
            {
                // 40, 41, 42
                return riverTile[40 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthWest, Direction.SouthWest))
            {
                // 43, 44, 45
                return riverTile[43 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.NorthWest, Direction.West))
            {
                // 46, 47, 48
                return riverTile[46 + river3Counter.Next()];
            }
            else if (river.IsDirection(Direction.SouthEast, Direction.SouthWest))
            {
                // 49, 50
                return riverTile[49 + river2Counter.Next()];
            }
            else if (river.IsDirection(Direction.SouthEast, Direction.West))
            {
                // 51, 52
                return riverTile[51 + river2Counter.Next()];
            }
            else if (river.IsDirection(Direction.SouthWest, Direction.West))
            {
                // 53, 54
                return riverTile[53 + river2Counter.Next()];
            }

            return riverTile[0];
        }

        private Tile MapTile(MapTile tile)
        {
            switch (tile.Biome)
            {
                case Biome.Water:
                    if (tile.Feature == TileFeature.Wave)
                    {
                        return waveTile[waveCounter.Next()];
                    }

                    return waterTile;

                case Biome.Grass:
                    if (tile.Feature == TileFeature.Hill)
                    {
                        return hillTile[hillCounter.Next()];
                    }
                    if (tile.Feature == TileFeature.Forest)
                    {
                        return forestTile[forestCounter.Next()];
                    }

                    return grassTile[grassCounter.Next()];

                case Biome.Mountain:
                    if (tile.Feature == TileFeature.Forest)
                    {
                        return mountainForestTile[mountainForestCounter.Next()];
                    }
                    return mountainTile[mountainCounter.Next()];

                default:
                    return grassTile[grassCounter.Next()];
            }
        }
    }
}