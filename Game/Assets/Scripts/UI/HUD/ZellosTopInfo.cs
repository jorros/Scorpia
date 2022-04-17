using Actors;
using TMPro;
using UI.Tooltip;
using UnityEngine;
using Utils;

namespace UI.HUD
{
    public class ZellosTopInfo : MonoBehaviour
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
            storage.text = player.Zellos.Value.Format();
            production.text = player.ZellosProduction.Value.FormatBalance();
            tooltip.content = "Zellos text yadayada";
        }
    }
}