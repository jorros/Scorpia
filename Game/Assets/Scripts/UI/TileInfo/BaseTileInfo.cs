using System;
using System.Linq;
using Scorpia.Assets.Scripts.Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Scorpia.Assets.Scripts.UI.TileInfo
{
    public abstract class BaseTileInfo
    {
        protected GameObject instance;
        private GameObject prefab;
        private GameObject parent;
        private GameObject iconPrefab;
        private Sprite[] icons;

        private int infoCounter = 0;

        public BaseTileInfo(GameObject prefab, GameObject parent, GameObject iconPrefab, Sprite[] icons)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.iconPrefab = iconPrefab;
            this.icons = icons;
        }

        protected abstract string GetName(MapTile mapTile);

        protected abstract void Render(MapTile mapTile);

        public void Open(MapTile mapTile)
        {
            if (instance == null)
            {
                instance = GameObject.Instantiate(prefab, parent.transform);
            }

            infoCounter = 0;

            var textComponents = instance.GetComponentsInChildren<TextMeshProUGUI>();
            textComponents.First(x => x.name == "TileName").SetText(GetName(mapTile));

            var imageComponents = instance.GetComponentsInChildren<Image>();
            foreach (var icon in imageComponents.Where(x => x.name.StartsWith("InfoIcon")))
            {
                GameObject.Destroy(icon.gameObject);
            }

            Render(mapTile);
        }

        protected void AddInfoIcon(int icon)
        {
            var obj = GameObject.Instantiate(iconPrefab, instance.transform);
            obj.name = $"InfoIcon{infoCounter}";
            obj.GetComponent<RectTransform>().localPosition = new Vector3(-604 + 179 * infoCounter, 93.5f);

            var img = obj.GetComponent<Image>();
            img.sprite = icons[icon];

            infoCounter++;
        }

        public void Close()
        {
            GameObject.Destroy(instance);
            instance = null;
        }
    }
}

