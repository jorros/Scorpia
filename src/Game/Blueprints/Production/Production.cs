using Scorpia.Game.Nodes;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.Blueprints.Production;

public abstract class Production
{
    public abstract float Value { get; }

    public abstract void Apply(PlayerNode player, LocationNode location);

    protected BalanceSheet AddToBalance(BalanceSheet balance, float multiplier = 1f)
    {
        balance.BuildingIn ??= 0 + Value * multiplier;

        return balance;
    }
}