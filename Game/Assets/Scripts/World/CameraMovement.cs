using Unity.Netcode;
using UnityEngine;

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

        private Vector3 dragOrigin;

        [SerializeField]
        private GameObject worldManagerPrefab;

        void Start()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var instance = GameObject.Instantiate(worldManagerPrefab);
                instance.GetComponent<NetworkObject>().Spawn();
            }
        }

        void Update()
        {
            PanCamera();
        }

        private void PanCamera()
        {
            if (Input.GetMouseButtonDown(0))
            {
                dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
            }

            if (Input.GetMouseButton(0))
            {
                var delta = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);
                cam.transform.position = ClampCamera(cam.transform.position + delta);
            }
        }

        public void ZoomIn()
        {
            var newSize = cam.orthographicSize - zoomStep;
            cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

            cam.transform.position = ClampCamera(cam.transform.position);
        }

        public void ZoomOut()
        {
            var newSize = cam.orthographicSize + zoomStep;
            cam.orthographicSize = Mathf.Clamp(newSize, minCamSize, maxCamSize);

            cam.transform.position = ClampCamera(cam.transform.position);
        }

        private Vector3 ClampCamera(Vector3 targetPosition)
        {
            var camHeight = cam.orthographicSize;
            var camWidth = cam.orthographicSize * cam.aspect;

            var minX = camWidth;
            var maxX = mapWidth - camWidth;
            var minY = camHeight;
            var maxY = mapHeight - camHeight;

            // print($"camHeight:{camHeight}; camWidth:{camWidth}; mapHeight:{mapHeight}; mapWidth:{mapWidth}; minX:{minX}; maxX:{maxX}; minY:{minY}; maxY:{maxY}; targetX:{targetPosition.x}; targetY:{targetPosition.y}");

            var newX = Mathf.Clamp(targetPosition.x, minX, maxX);
            var newY = Mathf.Clamp(targetPosition.y, minY, maxY);

            return new Vector3(newX, newY, targetPosition.z);
        }
    }
}