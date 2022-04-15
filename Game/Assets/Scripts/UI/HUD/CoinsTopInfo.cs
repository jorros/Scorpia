using Actors;
using TMPro;
using UI.Tooltip;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace UI.HUD
{
    public class CoinsTopInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coins, income;
        
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
            coins.text = player.Coins.Value.Format();
            income.text = player.Income.Value.FormatBalance();
            tooltip.content = "Income text yadayada";
        }
    }
}