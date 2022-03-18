using System.Collections.Generic;
using Scorpia.Assets.Scripts.World;
using UnityEngine;

namespace Scorpia.Assets.Scripts.UI
{
    public class MinimapRenderer : MonoBehaviour
    {
        private Camera cam;

        [SerializeField]
        private Material cameraBoxMaterial;
        public float lineWidth;

        [SerializeField]
        private Texture minimapTexture;

        void Awake()
        {
            cam = GetComponent<Camera>();
        }

        void Start()
        {
            EventManager.Register(EventManager.MapRendered, Refresh);
        }

        void OnDestroy()
        {
            EventManager.Remove(EventManager.MapRendered, Refresh);
        }

        private Vector3 GetCameraFrustumPoint(Vector3 position)
        {
            return Camera.main.ScreenToWorldPoint(position);
        }

        void OnPostRender()
        {
            var minViewportPoint = cam.WorldToViewportPoint(GetCameraFrustumPoint(new Vector3(0f, 0f)));
            var maxViewportPoint = cam.WorldToViewportPoint(GetCameraFrustumPoint(new Vector3(Screen.width, Screen.height)));

            float minX = minViewportPoint.x;
            float minY = minViewportPoint.y;

            float maxX = maxViewportPoint.x;
            float maxY = maxViewportPoint.y;

            GL.PushMatrix();
            {
                cameraBoxMaterial.SetPass(0);
                GL.LoadOrtho();

                GL.Begin(GL.QUADS);
                GL.Color(Color.white);
                {

                    GL.Vertex(new Vector3(minX, minY + lineWidth, 0));
                    GL.Vertex(new Vector3(minX, minY - lineWidth, 0));
                    GL.Vertex(new Vector3(maxX, minY - lineWidth, 0));
                    GL.Vertex(new Vector3(maxX, minY + lineWidth, 0));


                    GL.Vertex(new Vector3(minX + lineWidth, minY, 0));
                    GL.Vertex(new Vector3(minX - lineWidth, minY, 0));
                    GL.Vertex(new Vector3(minX - lineWidth, maxY, 0));
                    GL.Vertex(new Vector3(minX + lineWidth, maxY, 0));



                    GL.Vertex(new Vector3(minX, maxY + lineWidth, 0));
                    GL.Vertex(new Vector3(minX, maxY - lineWidth, 0));
                    GL.Vertex(new Vector3(maxX, maxY - lineWidth, 0));
                    GL.Vertex(new Vector3(maxX, maxY + lineWidth, 0));

                    GL.Vertex(new Vector3(maxX + lineWidth, minY, 0));
                    GL.Vertex(new Vector3(maxX - lineWidth, minY, 0));
                    GL.Vertex(new Vector3(maxX - lineWidth, maxY, 0));
                    GL.Vertex(new Vector3(maxX + lineWidth, maxY, 0));

                }
                GL.End();
            }
            GL.PopMatrix();
        }

        public void Refresh(IReadOnlyList<object> args = null)
        {
            var width = Game.MapRenderer.mapSize.x;
            var height = Game.MapRenderer.mapSize.y;

            float screenRatio = (float)minimapTexture.width / minimapTexture.height;
            float targetRatio = width / height;

            if (screenRatio >= targetRatio)
            {
                cam.orthographicSize = height / 2;
            }
            else
            {
                float differenceInSize = targetRatio / screenRatio;
                cam.orthographicSize = height / 2 * differenceInSize;
            }

            transform.position = new Vector3(width / 2, height / 2, -10);
            print("minimap: " + transform.position);
        }
    }
}