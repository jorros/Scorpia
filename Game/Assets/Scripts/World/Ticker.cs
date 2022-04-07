using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace World
{
    public class Ticker : NetworkBehaviour
    {
        public static Ticker current;

        public NetworkVariable<int> currentTick = new NetworkVariable<int>(0);

        private float step = 0f;

        private TextMeshProUGUI dateText;

        void Awake()
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

        void FixedUpdate()
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

        void Tick()
        {
            if (IsServer)
            {
            }
            else
            {
                if (dateText == null)
                {
                    dateText = GameObject.Find("DateText")?.GetComponent<TextMeshProUGUI>();
                }

                dateText?.SetText(new ScorpiaDate(currentTick.Value).ToString());
            }
        }
    }
}