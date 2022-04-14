using Unity.Collections;
using Unity.Netcode;

namespace Actors
{
    public class Player : NetworkBehaviour
    {
        public NetworkVariable<int> Doubloons { get; set; } = new();

        public NetworkVariable<FixedString64Bytes> Name { get; set; } = new();
        public NetworkVariable<FixedString64Bytes> UID { get; set; } = new();
        public NetworkVariable<int> Colour { get; set; } = new();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Name.OnValueChanged += (_, currentVal) =>
                {
                    var name = currentVal.Value;
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        EventManager.Trigger(EventManager.PlayerInfo, name, Colour.Value);
                    }
                };
                Colour.OnValueChanged += (_, currentVal) =>
                {
                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        EventManager.Trigger(EventManager.PlayerInfo, Name.Value.Value, currentVal);
                    }
                };
            }
        }
    }
}