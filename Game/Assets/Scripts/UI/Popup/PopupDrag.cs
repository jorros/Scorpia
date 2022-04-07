using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Popup
{
    public class PopupDrag : MonoBehaviour, IDragHandler
    {
        [SerializeField] private RectTransform popupRect;

        public void OnDrag(PointerEventData eventData)
        {
            popupRect.anchoredPosition += eventData.delta;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                PopupSystem.Hide();
            }
        }
    }
}