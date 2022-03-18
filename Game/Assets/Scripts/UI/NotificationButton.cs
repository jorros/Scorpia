using UnityEngine;
using UnityEngine.EventSystems;

namespace Scorpia.Assets.Scripts.UI
{
    public class NotificationButton : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector]
        public Notification notification;

        [HideInInspector]
        public float shouldX;

        private RectTransform rectTransform;

        [SerializeField]
        private float MOVE_SPEED = 400;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                EventManager.Trigger(EventManager.RemoveNotification, notification);
            }
        }

        void Update()
        {
            if(shouldX <= rectTransform.anchoredPosition.x)
            {
                var moveTo = new Vector2(shouldX, rectTransform.anchoredPosition.y);

                rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, moveTo, Time.deltaTime * MOVE_SPEED);
            }
        }
    }
}

