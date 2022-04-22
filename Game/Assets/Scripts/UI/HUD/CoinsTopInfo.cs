using System.Text;
using Actors;
using TMPro;
using UI.Tooltip;
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
            coins.text = player.Scorpions.Value.Format();
            income.text = player.ScorpionsBalance.Value.Total.FormatBalance();

            var formatter = new BalanceSheetFormatter(player.ScorpionsBalance.Value);
            tooltip.content = formatter.GetSummary();
        }

        private void Append(StringBuilder sb, string infoName, float? value)
        {
            if (value is not null)
            {
                sb.AppendLine($"{infoName}: {value.Value.FormatBalance()}");
            }
        }
    }
}