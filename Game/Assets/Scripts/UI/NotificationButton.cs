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
            if(shouldX <= rectTransform.localPosition.x)
            {
                var newPos = Vector2.MoveTowards(rectTransform.localPosition, new Vector2(shouldX, rectTransform.localPosition.y), Time.deltaTime * MOVE_SPEED);
                rectTransform.localPosition = new Vector3(newPos.x, rectTransform.localPosition.y, rectTransform.localPosition.z);
            }
        }
    }
}

