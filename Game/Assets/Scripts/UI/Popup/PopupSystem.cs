using TMPro;
using UnityEngine;
using UnityEngine.UI;
using World;

namespace UI.Popup
{
    public class PopupSystem : MonoBehaviour
    {
        private static PopupSystem _current;

        [SerializeField] private GameObject popupWindow;
        [SerializeField] private Image coverImg;
        [SerializeField] private TextMeshProUGUI headerTxt;
        [SerializeField] private Button btn1;
        [SerializeField] private Button btn2;
        
        void Awake()
        {
            _current = this;
        }

        public static void Show(Sprite cover, string header, string content)
        {
            _current.btn2.gameObject.SetActive(false);
            _current.coverImg.sprite = cover;
            _current.headerTxt.text = header;
            
            _current.btn1.onClick.RemoveAllListeners();
            _current.btn1.onClick.AddListener(Hide);
            
            _current.popupWindow.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            
            _current.popupWindow.SetActive(true);
        }

        public static void Hide()
        {
            CameraMovement.clickIsBlocked = true;
            _current.popupWindow.SetActive(false);
        }
    }
}