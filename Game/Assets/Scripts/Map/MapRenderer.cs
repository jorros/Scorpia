using System.Collections.Generic;
using Map.Render;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Map.Render.BiomeRenderer;

namespace Map
{
    public class MapRenderer : NetworkBehaviour
    {
        [SerializeField] public Tile[] riverTile;

        [SerializeField] private Tile[] minimapTiles;

        [SerializeField] private Tile[] flairTiles;

        [SerializeField] private Tile[] locationTiles;

        [SerializeField] private TileBase selectedTile;

        [SerializeField] private BiomeRendererTiles biomeTiles;

        private readonly NetworkVariable<int> width = new();
        private readonly NetworkVariable<int> height = new();
        private readonly NetworkVariable<int> seed = new();

        private Tilemap groundLayer;
        private Tilemap minimapLayer;
        private Tilemap riverLayer;
        private Tilemap flairLayer;
        private Tilemap locationsLayer;
        private Tilemap selectedLayer;

        [HideInInspector] public Map map;

        [HideInInspector] public Vector3 mapSize;

        public static MapRenderer current;

        private LocationsRenderer locationsRenderer;

        private ITileRenderer[] renderers;

        private void Awake()
        {
            current = this;

            var tilemaps = GetComponentsInChildren<Tilemap>();
            groundLayer = tilemaps[0];
            minimapLayer = tilemaps[1];
            riverLayer = tilemaps[2];
            flairLayer = tilemaps[3];
            locationsLayer = tilemaps[4];
            selectedLayer = tilemaps[5];

            renderers = new ITileRenderer[]
            {
                new BiomeRenderer(groundLayer, biomeTiles),
                new RiverTileRenderer(riverLayer, riverTile),
                new MinimapTileRenderer(minimapLayer, minimapTiles),
                new FlairTileRenderer(flairLayer, flairTiles),
                new LocationsRenderer(locationsLayer, locationTiles)
            };

            EventManager.RegisterAll(this);
        }

        public override void OnDestroy()
        {
            EventManager.RemoveAll(this);
        }

        public override void OnNetworkSpawn()
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

        public Vector3 GetTileWorldPosition(Vector2Int position)
        {
            return groundLayer.CellToWorld(new Vector3Int(position.x, position.y));
        }

        public void Refresh()
        {
            print($"S:{seed.Value};H:{height.Value};W:{width.Value}");

            map.Generate();

            for (var y = 0; y < height.Value; y++)
            {
                for (var x = 0; x < width.Value; x++)
                {
                    RenderTile(x, y);
                }
            }

            mapSize = groundLayer.CellToWorld(new Vector3Int(groundLayer.size.x - 1, groundLayer.size.y - 1)) +
                      (groundLayer.cellSize / 2);

            EventManager.Trigger(EventManager.MapRendered);
        }

        private void RenderTile(int x, int y)
        {
            var tile = map.GetTile(x, y);
            var pos = new Vector3Int(x, y, 0);

            foreach (var tileRenderer in renderers)
            {
                tileRenderer.Layer.SetTile(pos, tileRenderer.GetTile(tile));
            }
        }

        [Event(EventManager.LocationUpdated)]
        private void OnLocationUpdate(Vector2Int position)
        {
            RenderTile(position.x, position.y);
        }

        [Event(EventManager.SelectTile)]
        private void SelectTile(MapTile selected)
        {
            selectedLayer.ClearAllTiles();
            selectedLayer.SetTile(new Vector3Int(selected.Position.x, selected.Position.y, 0), selectedTile);
        }

        [Event(EventManager.DeselectTile)]
        private void DeselectTile()
        {
            selectedLayer.ClearAllTiles();
        }
    }
}