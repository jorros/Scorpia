using System.Collections.Generic;
using Scorpia.Assets.Scripts.Map;
using Scorpia.Assets.Scripts.UI.TileInfo;
using UnityEngine;

namespace Scorpia.Assets.Scripts.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject infoUI;

        [SerializeField]
        private Sprite[] infoIcons;

        [SerializeField]
        private GameObject infoIconPrefab;

        private BaseTileInfo tileInfo;

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

            if(tileInfo is not null)
            {
                tileInfo.Close();
            }

            tileInfo = new EmptyTileInfo(infoUI, gameObject, infoIconPrefab, infoIcons);
            tileInfo.Open(mapTile);
        }

        private void Deselect(IReadOnlyList<object> args)
        {
            tileInfo.Close();
            tileInfo = null;
        }
    }
}