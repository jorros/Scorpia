using Actors;
using TMPro;
using UI.Tooltip;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace UI.HUD
{
    public class PopulationTopInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI population;

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
            population.text = player.Coins.Value.Format();
            tooltip.content = "Population text yadayada";
        }
    }
}