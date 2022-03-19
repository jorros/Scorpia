using System.Collections.Generic;
using System.Linq;
using Scorpia.Assets.Scripts.Map;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scorpia.Assets.Scripts.World
{
    public class CameraMovement : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;

        [SerializeField]
        private float zoomStep, minCamSize, maxCamSize;

        private Vector3? dragOrigin;

        private bool isDragging;

        [SerializeField]
        private GameObject worldManagerPrefab;

        private EventSystem eventSystem;
        private GraphicRaycaster raycaster;

        private MapTile tile;
        private PointerEventData pointData;
        private List<RaycastResult> raycastResults;

        void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var instance = GameObject.Instantiate(worldManagerPrefab);
                instance.GetComponent<NetworkObject>().Spawn();
            }

            raycaster = GameObject.Find("HUD").GetComponent<GraphicRaycaster>();
            pointData = new PointerEventData(EventSystem.current);
            raycastResults = new List<RaycastResult>();

            EventManager.Register(EventManager.PanCamera, SetPosition);
            EventManager.Register(EventManager.ZoomInCamera, ZoomIn);
            EventManager.Register(EventManager.ZoomOutCamera, ZoomOut);
        }

        void OnDestroy()
        {
            EventManager.Remove(EventManager.PanCamera, SetPosition);
            EventManager.Remove(EventManager.ZoomInCamera, ZoomIn);
            EventManager.Remove(EventManager.ZoomOutCamera, ZoomOut);
        }

        void Update()
        {
            if(ClickedOnUI())
            {
                return;
            }

            StartDragging();
            SelectTile();
            StopDragging();
        }

        private void SelectTile()
        {
            if (Input.GetMouseButtonUp(0) && !isDragging)
            {
                var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                tile = MapRenderer.current.GetTile(mousePos);
                EventManager.Trigger(EventManager.SelectTile, tile);
            }

            if(Input.GetMouseButtonUp(1) && tile != null)
            {
                tile = null;
                EventManager.Trigger(EventManager.DeselectTile);
            }
        }

        private bool ClickedOnUI()
        {
            if (Input.GetMouseButtonUp(0) || Input.GetMouseButton(0))
            {
                pointData.position = Input.mousePosition;
                raycaster.Raycast(pointData, raycastResults);

                if (raycastResults.Any())
                {
                    raycastResults.Clear();
                    return true;
                }
            }

            return false;
        }

        private void StartDragging()
        {
            if(Input.GetMouseButtonDown(0))
            {
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0) && dragOrigin != null)
            {
                var delta = dragOrigin.Value - cam.ScreenToWorldPoint(Input.mousePosition);

                if (delta != Vector3.zero)
                {
                    isDragging = true;
                }

                cam.transform.position = ClampCamera(cam.transform.position + delta);
            }
        }

        private void StopDragging()
        {
            if (Input.GetMouseButtonUp(0) && isDragging)
            {
                isDragging = false;
            }
        }

        public void ZoomIn(IReadOnlyList<object> args = null)
        {
            var newSize = cam.orthographicSize - zoomStep;
            cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

            cam.transform.position = ClampCamera(cam.transform.position);
        }

        public void ZoomOut(IReadOnlyList<object> args = null)
        {
            var newSize = cam.orthographicSize + zoomStep;
            cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

            cam.transform.position = ClampCamera(cam.transform.position);
        }

        private void SetPosition(IReadOnlyList<object> args)
        {
            var pos = (Vector3)args[0];
            cam.transform.position = ClampCamera(pos);
        }

        private Vector3 ClampCamera(Vector3 targetPosition)
        {
            var camHeight = cam.orthographicSize;
            var camWidth = cam.orthographicSize * cam.aspect;

            var minX = camWidth - 1;
            var maxX = MapRenderer.current.mapSize.x - camWidth + 1;
            var minY = camHeight - 1;
            var maxY = MapRenderer.current.mapSize.y - camHeight - 1;

            var newX = Mathf.Clamp(targetPosition.x, minX, maxX);
            var newY = Mathf.Clamp(targetPosition.y, minY, maxY);

            return new Vector3(newX, newY, cam.transform.position.z);
        }
    }
}