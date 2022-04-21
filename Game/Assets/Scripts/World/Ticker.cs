using Actors;
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
            if (IsServer)
            {
                // 1 Tick = 1 second; Every update is 0.02s
                step += 0.02f;

                if (step >= 1)
                {
                    step = 0;
                    currentTick.Value++;
                    Tick();
                }
            }
        }

        private void Tick()
        {
            if (IsServer)
            {
                var locations = ScorpiaServer.Singleton.GetLocations();
                
                foreach (var location in locations)
                {
                    location.DailyTick();
                }
                
                if (currentTick.Value % 30 != 0)
                {
                    return;
                }

                foreach (var location in locations)
                {
                    location.MonthlyTick();
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