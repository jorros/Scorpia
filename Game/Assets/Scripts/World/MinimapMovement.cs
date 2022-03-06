using UnityEngine;
using UnityEngine.EventSystems;

namespace Scorpia.Assets.Scripts.World
{
    public class MinimapMovement : MonoBehaviour,  IPointerDownHandler, IPointerUpHandler
    {
        private Vector2 uiSize;

        private Vector3 minimapPos;

        private Vector2 lastPointerPosition;
        private bool dragging = false;

        private CameraMovement camMovement;

        private void Start()
        {
            var rect = GetComponent<RectTransform>();
            uiSize = rect.sizeDelta;
            minimapPos = Camera.main.WorldToScreenPoint(rect.position);
            lastPointerPosition = Input.mousePosition;

            camMovement = Camera.main.GetComponent<CameraMovement>();
        }

        void Update()
        {
            if (!dragging) return;

            Vector2 delta = (Vector2)Input.mousePosition - lastPointerPosition;
            lastPointerPosition = Input.mousePosition;

            if (delta.magnitude > Mathf.Epsilon)
            {
                var terrainSize = Game.MapRenderer.mapSize;

                Vector3 uiPos = Input.mousePosition - minimapPos;
                Vector3 realPos = new Vector3(
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