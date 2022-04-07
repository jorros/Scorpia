using System.Collections.Generic;
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
                EventManager.Register(EventManager.PlayerInfo, SetIcon);
            }
        }

        void OnDestroy()
        {
            if (NetworkManager.Singleton.IsClient)
            {
                EventManager.Remove(EventManager.PlayerInfo, SetIcon);
            }
        }

        public void SetIcon(IReadOnlyList<object> list)
        {
            var name = list[0] as string;
            var colour = list[1] as int?;

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