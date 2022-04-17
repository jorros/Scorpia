using Actors;
using UI.Tooltip;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

namespace UI.HUD
{
    public class PlayerIcon : MonoBehaviour
    {
        [SerializeField] private Image image;

        [SerializeField] private Sprite[] icons;

        [SerializeField] private TooltipTrigger tooltip;

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

        [Event(Events.PlayerInfo)]
        public void UpdateInfo(Player player)
        {
            image.sprite = icons[(int) player.Colour.Value];
            tooltip.header = player.Name.Value.Value;
        }
    }
}