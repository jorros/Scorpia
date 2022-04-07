using System.Collections.Generic;
using System.Linq;
using UI.Popup;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class NotificationUISystem : MonoBehaviour
    {
        [SerializeField] private Sprite[] notificationIcons;

        [SerializeField] private Sprite[] notificationCovers;

        [SerializeField] private GameObject prefab;

        private List<GameObject> notifications;

        private const float START_POS_X = 100;
        private const float POS_Y = -330;

        private void Awake()
        {
            EventManager.Register(EventManager.ReceiveNotification, ReceiveNotification);
            EventManager.Register(EventManager.RemoveNotification, RemoveNotification);

            notifications = new List<GameObject>();
        }

        private void OnDestroy()
        {
            EventManager.Remove(EventManager.ReceiveNotification, ReceiveNotification);
            EventManager.Remove(EventManager.RemoveNotification, RemoveNotification);
        }

        private void ReceiveNotification(IReadOnlyList<object> args)
        {
            var notification = args[0] as Notification;

            Add(notification);
        }

        private void RemoveNotification(IReadOnlyList<object> args)
        {
            var notification = args[0] as Notification;

            Remove(notification);
        }

        private void Add(Notification notification)
        {
            var instance = Instantiate(prefab, transform);

            var positionX = CalculateX(notifications.Count);
            instance.GetComponent<RectTransform>().anchoredPosition = new Vector3(positionX, POS_Y, 0);

            var icon = instance.GetComponentsInChildren<Image>().First(x => x.name == "Icon");
            icon.sprite = notificationIcons[notification.Icon];
            icon.rectTransform.sizeDelta = new Vector2(notificationIcons[notification.Icon].rect.width,
                notificationIcons[notification.Icon].rect.height);

            var button = instance.GetComponent<NotificationButton>();
            button.notification = notification;
            button.shouldX = positionX;

            var tooltip = instance.GetComponent<TooltipTrigger>();
            tooltip.header = notification.TooltipHeader;
            tooltip.content = notification.TooltipText;

            var popup = instance.GetComponent<PopupTrigger>();
            popup.cover = notificationCovers[notification.Cover];
            popup.header = notification.Header;
            popup.text = notification.Text;

            notifications.Add(instance);
        }

        private void Remove(Notification notification)
        {
            var instance =
                notifications.First(x => x.GetComponent<NotificationButton>().notification.Id == notification.Id);
            Destroy(instance);
            notifications.Remove(instance);

            Refresh();
        }

        private void Refresh()
        {
            for (var i = 0; i < notifications.Count; i++)
            {
                notifications[i].GetComponent<NotificationButton>().shouldX = CalculateX(i);
            }
        }

        private float CalculateX(int position) => START_POS_X + position * 160;
    }
}