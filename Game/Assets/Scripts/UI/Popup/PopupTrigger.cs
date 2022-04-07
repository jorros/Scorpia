using UnityEngine;
using UnityEngine.EventSystems;

namespace UI.Popup
{
    public class PopupTrigger : MonoBehaviour, IPointerClickHandler
    {
        public Sprite cover;
        public string header;
        public string text;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            PopupSystem.Show(cover, header, text);
        }
    }
}