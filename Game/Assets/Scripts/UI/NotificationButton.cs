using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using World;

namespace UI
{
    public class NotificationButton : MonoBehaviour, IPointerClickHandler
    {
        [HideInInspector] public Notification notification;

        [HideInInspector] public float shouldX;

        private RectTransform rectTransform;

        [FormerlySerializedAs("MOVE_SPEED")] [SerializeField] private float moveSpeed = 400;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            CameraMovement.clickIsBlocked = true;
            EventManager.Trigger(Events.RemoveNotification, notification);
        }

        void Update()
        {
            if (shouldX <= rectTransform.anchoredPosition.x)
            {
                var moveTo = new Vector2(shouldX, rectTransform.anchoredPosition.y);

                rectTransform.anchoredPosition = Vector2.MoveTowards(rectTransform.anchoredPosition, moveTo,
                    Time.deltaTime * moveSpeed);
            }
        }
    }
}