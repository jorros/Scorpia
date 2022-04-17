using System;
using System.Collections.Generic;
using Blueprints.Requirements;
using Unity.Collections;
using Unity.Netcode;

namespace Actors
{
    public class Player : NetworkBehaviour
    {
        public NetworkVariable<FixedString64Bytes> Name { get; } = new();
        public NetworkVariable<FixedString64Bytes> Uid { get; } = new();
        public NetworkVariable<PlayerColour> Colour { get; } = new();

        public NetworkVariable<float> Scorpions { get; set; } = new(1000, NetworkVariableReadPermission.Owner);
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

            Scorpions.OnValueChanged += ProductionChange;
            Zellos.OnValueChanged += ProductionChange;
            Nitra.OnValueChanged += ProductionChange;
            Sofrum.OnValueChanged += ProductionChange;
            Food.OnValueChanged += ProductionChange;
            NitraProduction.OnValueChanged += ProductionChange;
            Income.OnValueChanged += ProductionChange;
            FoodProduction.OnValueChanged += ProductionChange;
            ZellosProduction.OnValueChanged += ProductionChange;
            SofrumProduction.OnValueChanged += ProductionChange;

            Population.OnValueChanged += (_, _) => { EventManager.Trigger(Events.PlayerInfo, this); };

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

        public void Pay(IEnumerable<Requirement> requirements)
        {
            if (!IsServer)
            {
                return;
            }

            foreach (var requirement in requirements)
            {
                switch (requirement)
                {
                    case CostRequirement:
                        Scorpions.Value -= requirement.Value;
                        break;
                    
                    case NitraRequirement:
                        Nitra.Value -= requirement.Value;
                        break;
                    
                    case SofrumRequirement:
                        Sofrum.Value -= requirement.Value;
                        break;
                    
                    case ZellosRequirement:
                        Zellos.Value -= requirement.Value;
                        break;
                }
            }
        }

        public void Refund(IEnumerable<Requirement> requirements, float percentage)
        {
            if (!IsServer)
            {
                return;
            }

            foreach (var requirement in requirements)
            {
                switch (requirement)
                {
                    case CostRequirement:
                        Scorpions.Value += requirement.Value * percentage;
                        break;
                    
                    case NitraRequirement:
                        Nitra.Value += requirement.Value * percentage;
                        break;
                    
                    case SofrumRequirement:
                        Sofrum.Value += requirement.Value * percentage;
                        break;
                    
                    case ZellosRequirement:
                        Zellos.Value += requirement.Value * percentage;
                        break;
                }
            }
        }

        public override void OnNetworkDespawn()
        {
            Game.RemovePlayer(Uid.Value.Value);
        }
    }
}