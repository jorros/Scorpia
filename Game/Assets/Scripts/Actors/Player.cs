using System.Collections.Generic;
using Actors.Entities;
using Blueprints.Requirements;
using Unity.Collections;
using Unity.Netcode;
using Utils;

namespace Actors
{
    public class Player : NetworkBehaviour, IActor
    {
        public NetworkVariable<ForceNetworkSerializeByMemcpy<FixedString64Bytes>> Name { get; } = new();
        public NetworkVariable<ForceNetworkSerializeByMemcpy<FixedString64Bytes>> Uid { get; } = new();
        public NetworkVariable<PlayerColour> Colour { get; } = new();

        public NetworkVariable<float> Scorpions { get; set; } = new(100, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> Zellos { get; } = new(0, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> Nitra { get; } = new(0, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> Sofrum { get; } = new(0, NetworkVariableReadPermission.Owner);
        public NetworkVariable<float> Food { get; } = new(10, NetworkVariableReadPermission.Owner);

        public NetworkVariable<int> Population { get; } = new(0, NetworkVariableReadPermission.Owner);

        public NetworkVariable<BalanceSheet> ScorpionsBalance { get; } = new(new BalanceSheet(), NetworkVariableReadPermission.Owner);
        public NetworkVariable<BalanceSheet> FoodBalance { get; } = new(new BalanceSheet(), NetworkVariableReadPermission.Owner);
        public NetworkVariable<BalanceSheet> ZellosBalance { get; } = new(new BalanceSheet(), NetworkVariableReadPermission.Owner);
        public NetworkVariable<BalanceSheet> NitraBalance { get; } = new(new BalanceSheet(), NetworkVariableReadPermission.Owner);
        public NetworkVariable<BalanceSheet> SofrumBalance { get; } = new(new BalanceSheet(), NetworkVariableReadPermission.Owner);


        public override void OnNetworkSpawn()
        {
            Game.AddPlayer(this);

            if (!IsOwner)
            {
                return;
            }

            void StorageChange(float a, float b)
            {
                EventManager.Trigger(Events.PlayerInfo, this);
            }

            void ProductionChange(BalanceSheet a, BalanceSheet b)
            {
                EventManager.Trigger(Events.PlayerInfo, this);
            }

            Scorpions.OnValueChanged += StorageChange;
            Zellos.OnValueChanged += StorageChange;
            Nitra.OnValueChanged += StorageChange;
            Sofrum.OnValueChanged += StorageChange;
            Food.OnValueChanged += StorageChange;
            NitraBalance.OnValueChanged += ProductionChange;
            ScorpionsBalance.OnValueChanged += ProductionChange;
            FoodBalance.OnValueChanged += ProductionChange;
            ZellosBalance.OnValueChanged += ProductionChange;
            SofrumBalance.OnValueChanged += ProductionChange;

            Population.OnValueChanged += (_, _) => EventManager.Trigger(Events.PlayerInfo, this);

            Name.OnValueChanged += (_, currentVal) =>
            {
                var playerName = currentVal.Value.Value;
                if (!string.IsNullOrWhiteSpace(playerName))
                {
                    EventManager.Trigger(Events.PlayerInfo, this);
                }
            };

            Colour.OnValueChanged += (_, _) =>
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
            Game.RemovePlayer(Uid.ValueAsString());
        }

        public void PreTick()
        {
            ScorpionsBalance.Value = new BalanceSheet();
            FoodBalance.Value = new BalanceSheet();
            NitraBalance.Value = new BalanceSheet();
            SofrumBalance.Value = new BalanceSheet();
            ZellosBalance.Value = new BalanceSheet();
            Population.Value = 0;
        }

        public void DailyTick()
        {
            
        }

        public void MonthlyTick()
        {
            Scorpions.Value += ScorpionsBalance.Value.Total;
            Nitra.Value += NitraBalance.Value.Total;
            Sofrum.Value += SofrumBalance.Value.Total;
            Zellos.Value += ZellosBalance.Value.Total;
        }
    }
}