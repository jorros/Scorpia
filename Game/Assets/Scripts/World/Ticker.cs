using System.Linq;
using Server;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace World
{
    public class Ticker : NetworkBehaviour
    {
        public static Ticker current;

        public NetworkVariable<int> currentTick = new(0);

        private float step;

        private TextMeshProUGUI dateText;

        private void Awake()
        {
            current = this;
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
                dateText = GameObject.Find("DateText")?.GetComponent<TextMeshProUGUI>();

                currentTick.OnValueChanged += (oldVal, newVal) =>
                {
                    Tick();
                };
            }
        }

        private void FixedUpdate()
        {
            if (!IsServer)
            {
                return;
            }
            
            // 1 Tick = 1 second; Every update is 0.02s
            step += 0.02f;

            if (step >= 0.4f)
            {
                step = 0;
                currentTick.Value++;
                Tick();
            }
        }

        private void Tick()
        {
            if (IsServer)
            {
                var locations = Game.GetLocations();
                var players = Game.GetPlayers().ToArray();

                foreach (var location in locations)
                {
                    location.DailyTick();
                }
                
                foreach (var player in players)
                {
                    player.DailyTick();
                }
                
                if (currentTick.Value % 30 != 0)
                {
                    return;
                }
                
                foreach (var player in players)
                {
                    player.PreTick();
                }

                foreach (var location in locations)
                {
                    location.MonthlyTick();
                }

                foreach (var player in players)
                {
                    player.MonthlyTick();
                }
            }
            else
            {
                if (dateText == null)
                {
                    dateText = GameObject.Find("DateText").GetComponent<TextMeshProUGUI>();
                }

                dateText.SetText(new ScorpiaDate(currentTick.Value).ToString());
            }
        }
    }
}