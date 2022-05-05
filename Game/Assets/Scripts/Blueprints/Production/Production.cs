using Actors;
using Actors.Entities;

namespace Blueprints.Production
{
    public abstract class Production
    {
        public abstract float Value { get; }

        public abstract void Apply(Player player, Location location);

        protected BalanceSheet AddToBalance(BalanceSheet balance, float multiplier = 1f)
        {
            balance.BuildingIn = balance.BuildingIn ?? 0 + Value * multiplier;

            return balance;
        }
    }
}