using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Tooltip
{
    public class Tooltip : MonoBehaviour
    {
        public TextMeshProUGUI headerText;

        public TextMeshProUGUI underText;

        public TextMeshProUGUI contentText;

        [HideInInspector]
        public TooltipPosition position;

        [HideInInspector]
        public float offset;

        [HideInInspector]
        public RectTransform target;

        [SerializeField]
        private Canvas canvas;

        [SerializeField]
        private LayoutElement layoutElement;

        [SerializeField]
        private int characterWrapLimit;

        private RectTransform rectTransform;

        void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        void OnEnable()
        {
            ResizeLayout();

            var position = canvas.worldCamera.WorldToScreenPoint(target.position);

            var pivotX = position.x / Screen.width;
            var pivotY = 0;

            if (position.y > Screen.height / 2)
            {
                pivotY = 1;
            }

            position.y = GetY(position);

            rectTransform.pivot = new Vector2(pivotX, pivotY);

            var worldPos = canvas.worldCamera.ScreenToWorldPoint(position);
            worldPos.z = rectTransform.position.z;
            rectTransform.position = worldPos;
        }

        private float GetY(Vector3 targetPosition)
        {
            if(position == TooltipPosition.None)
            {
                if (targetPosition.y > Screen.height / 2)
                {
                    return targetPosition.y - offset;
                }
                else
                {
                    return targetPosition.y + offset;
                }
            }
            else
            {
                return position switch
                {
                    TooltipPosition.Info => ViewportToScreen(425.0f / Screen.height),
                    TooltipPosition.Top => ViewportToScreen(1 - 140.0f / Screen.height),
                    TooltipPosition.Action => ViewportToScreen(140f / Screen.height),
                    TooltipPosition.ExtraAction => ViewportToScreen(240f / Screen.height),
                    _ => 0
                };
            }
        }

        private float ViewportToScreen(float val) => canvas.worldCamera.ViewportToScreenPoint(new Vector3(0, val)).y;

        private void ResizeLayout()
        {
            var headerLength = headerText.text.Length;
            var underLength = underText.text.Length;
            var contentLength = contentText.text.Length;

            layoutElement.enabled = (headerLength > characterWrapLimit || underLength > characterWrapLimit || contentLength > characterWrapLimit);
        }
    }
}

