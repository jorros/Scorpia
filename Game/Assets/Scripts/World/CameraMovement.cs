using System.Collections.Generic;
using System.Linq;
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

        [SerializeField]
        public float mapWidth, mapHeight;

        private Vector3? dragOrigin;

        private bool isDragging;

        [SerializeField]
        private GameObject worldManagerPrefab;

        private EventSystem eventSystem;
        private GraphicRaycaster raycaster;

        void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var instance = GameObject.Instantiate(worldManagerPrefab);
                instance.GetComponent<NetworkObject>().Spawn();
            }

            raycaster = GameObject.Find("HUD").GetComponent<GraphicRaycaster>();

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
            PanCamera();
            SelectTile();
        }

        private void SelectTile()
        {
            if (Input.GetMouseButtonUp(0) && !isDragging)
            {
                var mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                Game.SelectedTile = Game.MapRenderer.GetTile(mousePos);
                EventManager.Trigger(EventManager.SelectTile, Game.SelectedTile);
            }

            if(Input.GetMouseButtonUp(1) && Game.SelectedTile != null)
            {
                Game.SelectedTile = null;
                EventManager.Trigger(EventManager.DeselectTile);
            }
        }

        private void PanCamera()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var pointData = new PointerEventData(EventSystem.current);
                pointData.position = Input.mousePosition;
                var results = new List<RaycastResult>();
                raycaster.Raycast(pointData, results);

                if (results.Any())
                {
                    dragOrigin = null;
                }
                else
                {
                    dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
                }

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
            var maxX = mapWidth - camWidth + 1;
            var minY = camHeight - 1;
            var maxY = mapHeight - camHeight - 1;

            var newX = Mathf.Clamp(targetPosition.x, minX, maxX);
            var newY = Mathf.Clamp(targetPosition.y, minY, maxY);

            return new Vector3(newX, newY, cam.transform.position.z);
        }
    }
}