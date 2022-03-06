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

            if (infoInstance == null)
            {
                infoInstance = GameObject.Instantiate(infoUI, transform);
            }

            var textObjs = infoInstance.GetComponentsInChildren<TextMeshProUGUI>();
            textObjs.First(x => x.name == "TileName").SetText("Free");

            SetInfoIcon(0, GetIconIndex(mapTile));
        }

        private void SetInfoIcon(int pos, int icon)
        {
            var objName = $"InfoIcon{pos}";
            var children = infoInstance.GetComponentsInChildren<Transform>();
            var obj = children.FirstOrDefault(x => x.name == objName)?.gameObject;

            if (obj == null)
            {
                obj = GameObject.Instantiate(infoIconPrefab, infoInstance.transform);
                obj.name = objName;
                obj.GetComponent<RectTransform>().localPosition = new Vector3(-604 + 179 * pos, 93.5f);
            }

            var img = obj.GetComponent<Image>();
            img.sprite = infoIcons[icon];
        }

        private int GetIconIndex(MapTile tile)
        {
            if (tile.Biome == Biome.Water)
            {
                return 0;
            }
            if (tile.Biome == Biome.Grass)
            {
                if (tile.Feature == TileFeature.Forest)
                {
                    return 2;
                }

                return 1;
            }
            if (tile.Biome == Biome.Mountain)
            {
                return 3;
            }

            return 1;
        }

        private void Deselect(IReadOnlyList<object> args)
        {
            infoInstance.SetActive(false);
            infoInstance = null;
        }
    }
}