using System.Collections.Generic;
using UnityEngine;

namespace Scorpia.Assets.Scripts.UI
{
    public class NotificationUISystem : MonoBehaviour
    {
        [SerializeField]
        private Sprite[] notificationIcons;

        [SerializeField]
        private GameObject notificationPrefab;

        private NotificationUI notificationUI;

        void Awake()
        {
            EventManager.Register(EventManager.ReceiveNotification, ReceiveNotification);
            EventManager.Register(EventManager.RemoveNotification, RemoveNotification);

            notificationUI = new NotificationUI(notificationPrefab, gameObject, notificationIcons);
        }

        void OnDestroy()
        {
            EventManager.Remove(EventManager.ReceiveNotification, ReceiveNotification);
            EventManager.Remove(EventManager.RemoveNotification, RemoveNotification);
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