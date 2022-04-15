using Actors;
using TMPro;
using UI.Tooltip;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace UI.HUD
{
    public class SofrumTopInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI storage, production;

        [SerializeField] private TooltipTrigger tooltip;

        private void Awake()
        {
            EventManager.RegisterAll(this);
        }

        void OnDestroy()
        {
            EventManager.RemoveAll(this);
        }

        [Event(Events.PlayerInfo)]
        public void UpdateInfo(Player player)
        {
            storage.text = player.Sofrum.Value.Format();
            production.text = player.SofrumProduction.Value.FormatBalance();
            tooltip.content = "Sofrum text yadayada";
        }
    }
}