using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Production;

public class FoodProduction : Production
{
    public FoodProduction(float value)
    {
        Value = value;
    }

    public override float Value { get; }

    public override void Apply(PlayerNode player, LocationNode location)
    {
        var multiplier = location.MapTile.Fertility switch
        {
            Fertility.High => 2f,
            Fertility.Normal => 1f,
            Fertility.Low => 0.5f,
            _ => 1f
        };
        location.FoodProduction.Value = AddToBalance(location.FoodProduction.Value, multiplier);
    }
}