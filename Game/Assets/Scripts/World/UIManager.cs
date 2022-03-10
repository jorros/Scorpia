using System.Collections.Generic;
using System.Linq;
using Scorpia.Assets.Scripts.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scorpia.Assets.Scripts.World
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject infoUI;

        private GameObject infoInstance;

        [SerializeField]
        private Sprite[] infoIcons;

        [SerializeField]
        private GameObject infoIconPrefab;

        private int infoCounter = 0;

        void Awake()
        {
            EventManager.Register(EventManager.SelectTile, SelectTile);
            EventManager.Register(EventManager.DeselectTile, Deselect);
        }

        void OnDestroy()
        {
            EventManager.Remove(EventManager.SelectTile, SelectTile);
            EventManager.Remove(EventManager.DeselectTile, Deselect);
        }

        private void SelectTile(IReadOnlyList<object> args)
        {
            var mapTile = args[0] as MapTile;
            infoCounter = 0;

            if (infoInstance == null)
            {
                infoInstance = Instantiate(infoUI, transform);
            }

            var children = infoInstance.GetComponentsInChildren<TextMeshProUGUI>();
            children.First(x => x.name == "TileName").SetText("Free");

            foreach (var icon in children.Where(x => x.name.StartsWith("InfoIcon")))
            {
                Destroy(icon.gameObject);
            }

            AddBiomeIcon(mapTile);
            AddResourceIcon(mapTile);
        }

        private void AddInfoIcon(int icon)
        {
            var obj = Instantiate(infoIconPrefab, infoInstance.transform);
            obj.name = $"InfoIcon{infoCounter}";
            obj.GetComponent<RectTransform>().localPosition = new Vector3(-604 + 179 * infoCounter, 93.5f);

            var img = obj.GetComponent<Image>();
            img.sprite = infoIcons[icon];

            infoCounter++;
        }

        private void AddBiomeIcon(MapTile tile)
        {
            var i = tile switch
            {
                { Biome: Biome.Water } => 0,
                { Biome: Biome.Grass, Feature: TileFeature.Forest } => 2,
                { Biome: Biome.Grass } => 1,
                { Biome: Biome.Mountain } => 3,
                _ => -1
            };

            if (i > -1)
            {
                AddInfoIcon(i);
            }
        }

        private void AddResourceIcon(MapTile tile)
        {
            var i = tile.Resource switch
            {
                Resource.Sofrum => 8,
                Resource.Gold => 6,
                Resource.Zellos => 9,
                Resource.Nitra => 7,
                _ => -1
            };

            if (i > -1)
            {
                AddInfoIcon(i);
            }
        }

        private void Deselect(IReadOnlyList<object> args)
        {
            Destroy(infoInstance);
            infoInstance = null;
        }
    }
}