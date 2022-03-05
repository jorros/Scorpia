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
        }

        void Update()
        {
            PanCamera();
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
                SetPosition(cam.transform.position + delta);
            }
        }

        public void ZoomIn()
        {
            var newSize = cam.orthographicSize - zoomStep;
            cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

            SetPosition(cam.transform.position);
        }

        public void ZoomOut()
        {
            var newSize = cam.orthographicSize + zoomStep;
            cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

            SetPosition(cam.transform.position);
        }

        public void SetPosition(Vector3 pos)
        {
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