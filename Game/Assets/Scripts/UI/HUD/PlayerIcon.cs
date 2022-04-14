using System.Collections.Generic;
using UI.Tooltip;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class PlayerIcon : MonoBehaviour
    {
        [SerializeField]
        private Image image;

        [SerializeField]
        private Sprite[] icons;

        [SerializeField]
        private TooltipTrigger tooltip;

        private void Awake()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                EventManager.RegisterAll(this);
            }
        }

        void OnDestroy()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                EventManager.RemoveAll(this);
            }
        }

        [Event(EventManager.PlayerInfo)]
        public void SetIcon(string name, int? colour)
        {
            if (colour != null)
            {
                image.sprite = icons[colour.Value];
            }

            if (name != null)
            {
                tooltip.header = name;
            }
        }
    }
}