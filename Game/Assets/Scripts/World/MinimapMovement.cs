using Scorpia.Assets.Scripts.Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Scorpia.Assets.Scripts.World
{
    public class MinimapMovement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        private Vector2 lastPointerPosition;
        private bool dragging = false;

        private RectTransform rect;
        private Canvas canvas;

        [SerializeField]
        private Vector2 referenceScreenSize;

        void Start()
        {
            rect = GetComponent<RectTransform>();
            canvas = FindObjectOfType<Canvas>();

            lastPointerPosition = Input.mousePosition;
        }

        void Update()
        {
            if (!dragging) return;

            Vector2 delta = (Vector2)Input.mousePosition - lastPointerPosition;
            lastPointerPosition = Input.mousePosition;

            if (delta.magnitude > Mathf.Epsilon)
            {
                var scaleFactor = canvas.renderingDisplaySize.x / referenceScreenSize.x;
                var uiSize = rect.sizeDelta * scaleFactor;

                var minimapPos = Camera.main.WorldToScreenPoint(rect.position);

                var terrainSize = MapRenderer.current.mapSize;

                var uiPos = Input.mousePosition - minimapPos;

                var realPos = new Vector3(
                    uiPos.x / uiSize.x * terrainSize.x,
                    uiPos.y / uiSize.y * terrainSize.y
                );
                EventManager.Trigger(EventManager.PanCamera, realPos);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            dragging = true;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            dragging = false;
        }
    }
}