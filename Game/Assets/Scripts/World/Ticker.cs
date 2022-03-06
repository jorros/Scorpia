using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Scorpia.Assets.Scripts.World
{
    public class Ticker : NetworkBehaviour
    {
        public NetworkVariable<int> currentTick = new NetworkVariable<int>(0);

        private float step = 0f;

        private TextMeshProUGUI dateText;

        void Awake()
        {
            Game.TickerObject = gameObject;
            dateText = GameObject.Find("DateText").GetComponent<TextMeshProUGUI>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsClient)
            {
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
            dateText.SetText(new ScorpiaDate(currentTick.Value).ToString());
            
            if (IsServer)
            {
            }
            else
            {

            }
        }
    }
}