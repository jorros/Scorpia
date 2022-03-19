using UnityEngine;

namespace Scorpia.Assets.Scripts.UI
{
    public class TooltipSystem : MonoBehaviour
    {
        private static TooltipSystem current;

        [SerializeField]
        private Tooltip tooltip;

        void Awake()
        {
            current = this;
        }

        public static void Show(RectTransform target, TooltipDescription desc, float offset)
        {
            current.tooltip.headerText.text = desc.Header;
            current.tooltip.contentText.text = desc.Content;
            current.tooltip.underText.text = desc.SubHeader;
            current.tooltip.target = target;
            current.tooltip.offset = offset;
            current.tooltip.position = desc.Position;

            current.tooltip.gameObject.SetActive(true);
        }

        public static void Hide()
        {
            current.tooltip.gameObject.SetActive(false);
        }
    }
}

