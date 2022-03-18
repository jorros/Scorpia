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
        private Sprite[] notificationIcons;

        [SerializeField]
        private Sprite[] fieldAvatarIcons;

        [SerializeField]
        private GameObject notificationPrefab;

        private BaseTileInfo tileInfo;
        private NotificationUI notificationUI;

        void Awake()
        {
            EventManager.Register(EventManager.SelectTile, SelectTile);
            EventManager.Register(EventManager.DeselectTile, Deselect);
            EventManager.Register(EventManager.ReceiveNotification, ReceiveNotification);
            EventManager.Register(EventManager.RemoveNotification, RemoveNotification);

            notificationUI = new NotificationUI(notificationPrefab, gameObject, notificationIcons);
        }

        void OnDestroy()
        {
            EventManager.Remove(EventManager.SelectTile, SelectTile);
            EventManager.Remove(EventManager.DeselectTile, Deselect);
            EventManager.Remove(EventManager.ReceiveNotification, ReceiveNotification);
            EventManager.Remove(EventManager.RemoveNotification, RemoveNotification);
        }

        private void SelectTile(IReadOnlyList<object> args)
        {
            var mapTile = args[0] as MapTile;

            if(tileInfo is not null)
            {
                tileInfo.Close();
            }

            tileInfo = new EmptyTileInfo(infoUI, gameObject, infoIcons, fieldAvatarIcons);
            tileInfo.Open(mapTile);
        }

        private void Deselect(IReadOnlyList<object> args)
        {
            tileInfo.Close();
            tileInfo = null;
        }

        private void ReceiveNotification(IReadOnlyList<object> args)
        {
            var notification = args[0] as Notification;

            notificationUI.Add(notification);
        }

        private void RemoveNotification(IReadOnlyList<object> args)
        {
            var notification = args[0] as Notification;

            notificationUI.Remove(notification);
        }
    }
}