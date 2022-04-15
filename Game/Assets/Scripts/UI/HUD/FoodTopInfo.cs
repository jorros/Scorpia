using Actors;
using TMPro;
using UI.Tooltip;
using Unity.Netcode;
using UnityEngine;
using Utils;

namespace UI.HUD
{
    public class FoodTopInfo : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI food,
            foodProduction;

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
            food.text = player.Food.Value.Format();
            foodProduction.text = player.FoodProduction.Value.FormatBalance();
            tooltip.content = "Food text yadayada";
        }
    }
}