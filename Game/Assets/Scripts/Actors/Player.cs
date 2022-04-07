using Unity.Collections;
using Unity.Netcode;

namespace Actors
{
    public class Player : NetworkBehaviour
    {
        public NetworkVariable<int> Doubloons { get; set; } = new NetworkVariable<int>();

        public NetworkVariable<FixedString64Bytes> Name { get; set; } = new NetworkVariable<FixedString64Bytes>();
        public NetworkVariable<FixedString64Bytes> UID { get; set; } = new NetworkVariable<FixedString64Bytes>();
        public NetworkVariable<int> Colour { get; set; } = new NetworkVariable<int>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Name.OnValueChanged += (previousVal, currentVal) =>
                {
                    var name = currentVal.ToString();
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        EventManager.Trigger(EventManager.PlayerInfo, name, Colour.Value);
                    }
                };
                Colour.OnValueChanged += (previousVal, currentVal) =>
                {
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        EventManager.Trigger(EventManager.PlayerInfo, Name.Value, currentVal);
                    }
                };
            }
        }
    }
}