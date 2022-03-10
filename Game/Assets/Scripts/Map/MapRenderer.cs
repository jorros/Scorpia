using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;
using Scorpia.Assets.Scripts.World;
using Scorpia.Assets.Scripts.Map.Render;
using static Scorpia.Assets.Scripts.Map.Render.BiomeRenderer;
using Scorpia.Assets.Scripts.UI;

namespace Scorpia.Assets.Scripts.Map
{
    public class MapRenderer : NetworkBehaviour
    {
        [SerializeField]
        public Tile[] riverTile;

        [SerializeField]
        private Tile[] minimapTiles;

        [SerializeField]
        private Tile[] flairTiles;

        [SerializeField]
        private BiomeRendererTiles biomeTiles;

        private NetworkVariable<int> width = new NetworkVariable<int>();
        private NetworkVariable<int> height = new NetworkVariable<int>();
        private NetworkVariable<int> seed = new NetworkVariable<int>();

        private Tilemap groundLayer;
        private Tilemap minimapLayer;
        private Tilemap riverLayer;
        private Tilemap flairLayer;
        private CameraMovement cam;

        [HideInInspector]
        public Map map;

        [HideInInspector]
        public Vector3 mapSize;

        void Awake()
        {
            Game.MapObject = gameObject;

            var tilemaps = GetComponentsInChildren<Tilemap>();
            groundLayer = tilemaps[0];
            minimapLayer = tilemaps[1];
            riverLayer = tilemaps[2];
            flairLayer = tilemaps[3];

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

            var renderers = new ITileRenderer[]
            {
                new BiomeRenderer(groundLayer, biomeTiles),
                new RiverTileRenderer(riverLayer, riverTile),
                new MinimapTileRenderer(minimapLayer, minimapTiles),
                new FlairTileRenderer(flairLayer, flairTiles)
            };

            for (int y = 0; y < height.Value; y++)
            {
                for (int x = 0; x < width.Value; x++)
                {
                    var tile = map.GetTile(x, y);
                    var pos = new Vector3Int(x, y, 0);

                    foreach(var renderer in renderers)
                    {
                        renderer.Layer.SetTile(pos, renderer.GetTile(tile));
                    }
                }
            }

            mapSize = groundLayer.CellToWorld(new Vector3Int(groundLayer.size.x - 1, groundLayer.size.y - 1)) + (groundLayer.cellSize / 2);

            var mapWidth = mapSize.x;
            var mapHeight = mapSize.y;

            var minimap = GameObject.FindObjectOfType<MinimapRenderer>();
            minimap.GetComponent<MinimapRenderer>().Refresh();

            cam.mapWidth = mapWidth;
            cam.mapHeight = mapHeight;
        }
    }
}