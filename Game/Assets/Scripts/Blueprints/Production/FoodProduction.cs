using Actors;
using Map;

namespace Blueprints.Production
{
    public class FoodProduction : Production
    {
        public FoodProduction(float value)
        {
            Value = value;
        }

        public override float Value { get; }

        public override void Apply(Player player, Location location)
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
}