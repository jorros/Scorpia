using Unity.Collections;
using Unity.Netcode;

namespace Actors
{
    public class Player : NetworkBehaviour
    {
        public NetworkVariable<FixedString64Bytes> Name { get; } = new();
        public NetworkVariable<FixedString64Bytes> Uid { get; } = new();
        public NetworkVariable<PlayerColour> Colour { get; } = new();

        public NetworkVariable<float> Coins { get; set; } = new(1000, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> Zellos { get; } = new(0, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> Nitra { get; } = new(0, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> Sofrum { get; } = new(0, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> Food { get; } = new(10, NetworkVariableReadPermission.Owner);

        public NetworkVariable<int> Population { get; } = new(0, NetworkVariableReadPermission.Owner);

        public NetworkVariable<float> Income { get; } = new(0, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> FoodProduction { get; } = new(0, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> ZellosProduction { get; } = new(0, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> NitraProduction { get; } = new(0, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> SofrumProduction { get; } = new(0, NetworkVariableReadPermission.Owner);


        public override void OnNetworkSpawn()
        {
            Game.AddPlayer(this);

            if (!IsOwner)
            {
                return;
            }

            void ProductionChange(float a, float b)
            {
                EventManager.Trigger(Events.PlayerInfo, this);
            }

            Coins.OnValueChanged += ProductionChange;
            Zellos.OnValueChanged += ProductionChange;
            Nitra.OnValueChanged += ProductionChange;
            Sofrum.OnValueChanged += ProductionChange;
            Food.OnValueChanged += ProductionChange;
            NitraProduction.OnValueChanged += ProductionChange;
            Income.OnValueChanged += ProductionChange;
            FoodProduction.OnValueChanged += ProductionChange;
            ZellosProduction.OnValueChanged += ProductionChange;
            SofrumProduction.OnValueChanged += ProductionChange;

            Population.OnValueChanged += (_, _) =>
            {
                EventManager.Trigger(Events.PlayerInfo, this);
            };

            Name.OnValueChanged += (_, currentVal) =>
            {
                var playerName = currentVal.Value;
                if (!string.IsNullOrWhiteSpace(playerName))
                {
                    EventManager.Trigger(Events.PlayerInfo, this);
                }
            };

            Colour.OnValueChanged += (_, currentVal) =>
            {
                if (!string.IsNullOrWhiteSpace(name))
                {
                    EventManager.Trigger(Events.PlayerInfo, this);
                }
            };
            
            EventManager.Trigger(Events.PlayerInfo, this);
        }

        public override void OnNetworkDespawn()
        {
            Game.RemovePlayer(Uid.Value.Value);
        }
    }
}