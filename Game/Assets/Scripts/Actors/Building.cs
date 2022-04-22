using System;
using Actors.Entities;
using Blueprints;
using Blueprints.Requirements;

namespace Actors
{
    public struct Building : IEquatable<Building>
    {
        public BuildingType Type { get; set; }

        public bool IsBuilding { get; set; }

        public int Level { get; set; }

        public bool Equals(Building other)
        {
            return Type == other.Type && Level == other.Level;
        }

        public override string ToString()
        {
            var buildText = IsBuilding ? "B" : "F";
            return $"{Type}:{Level}:{buildText}";
        }

        public void Tick(Player player, Location location)
        {
            // Maintenance costs
            var requirements = BuildingBlueprints.GetRequirements(Type);

            foreach (var requirement in requirements)
            {
                switch (requirement)
                {
                    case UpkeepRequirement:
                        UpdateIncome(player, location, nameof(BalanceSheet.BuildingOut), requirement.Value);
                        break;
                }
            }
        }

        private static void UpdateIncome(Player player, Location location, string fieldName, int value)
        {
            var balance = location.Income.Value;
            var field = balance.GetType().GetField(fieldName);
            var origVal = (float?) field.GetValue(balance) ?? 0;
            object boxed = balance;
            field.SetValue(boxed, origVal + value);
            location.Income.Value = (BalanceSheet)boxed;

            balance = player.ScorpionsBalance.Value;
            field = balance.GetType().GetField(fieldName);
            origVal = (float?) field.GetValue(balance) ?? 0;
            boxed = balance;
            field.SetValue(boxed, origVal + value);
            player.ScorpionsBalance.Value = (BalanceSheet)boxed;
        }
    }
}