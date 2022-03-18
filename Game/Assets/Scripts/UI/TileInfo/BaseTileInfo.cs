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
        private Sprite[] icons;
        private Sprite[] avatars;

        private int infoCounter = 0;

        public BaseTileInfo(GameObject prefab, GameObject parent, Sprite[] icons, Sprite[] avatars)
        {
            this.prefab = prefab;
            this.parent = parent;
            this.icons = icons;
            this.avatars = avatars;
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

            Render(mapTile);
        }

        protected void AddInfoIcon(int icon)
        {
            var imgs = instance.GetComponentsInChildren<Image>();
            var img = imgs.First(x => x.name == $"InfoIcon{infoCounter}");

            img.sprite = icons[icon];

            infoCounter++;
        }

        protected void SetAvatarIcon(int avatar)
        {
            var imgs = instance.GetComponentsInChildren<Image>();
            var img = imgs.First(x => x.name == "TileAvatar");

            img.sprite = avatars[avatar];
        }

        public void Close()
        {
            GameObject.Destroy(instance);
            instance = null;
        }
    }
}

