using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Scorpia.Assets.Scripts.UI
{
    public class NotificationUI
    {
        private GameObject prefab;
        private GameObject parent;
        private List<GameObject> notifications;
        private IReadOnlyList<Sprite> icons;

        public NotificationUI(GameObject prefab, GameObject parent, IReadOnlyList<Sprite> icons)
        {
            this.prefab = prefab;
            this.parent = parent;
            notifications = new List<GameObject>();
            this.icons = icons;
        }

        public void Add(Notification notification)
        {
            var instance = GameObject.Instantiate(prefab);
            var positionX = CalculateX(notifications.Count);
            instance.GetComponent<RectTransform>().position = new Vector3(positionX, 852, 0);

            instance.transform.SetParent(parent.transform, false);

            var icon = instance.GetComponentsInChildren<Image>().First(x => x.name == "Icon");
            icon.sprite = icons[notification.Icon];
            icon.rectTransform.sizeDelta = new Vector2(icons[notification.Icon].rect.width, icons[notification.Icon].rect.height);

            var button = instance.GetComponent<NotificationButton>();

            button.notification = notification;
            button.shouldX = positionX;

            notifications.Add(instance);

        }

        public void Remove(Notification notification)
        {
            var instance = notifications.First(x => x.GetComponent<NotificationButton>().notification.Id == notification.Id);
            GameObject.Destroy(instance);
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

        private float CalculateX(int position)
        {
            return -1820 + position * 160;
        }
    }
}

