using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public string header;
        public string subHeader;
        public float offset = 40;
        public TooltipPosition position;

        [TextArea]
        public string content;

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(string.IsNullOrWhiteSpace(header) && string.IsNullOrWhiteSpace(content))
            {
                return;
            }

            var desc = new TooltipDescription(header, content, subHeader, position);
            TooltipSystem.Show(GetComponent<RectTransform>(), desc, offset);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            TooltipSystem.Hide();
        }

        private void OnDestroy()
        {
            TooltipSystem.Hide();
        }
    }
}

