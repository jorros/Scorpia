using Actors;
using Map;

namespace Blueprints.Production
{
    public class ScorpionProduction : Production
    {
        public ScorpionProduction(float value)
        {
            Value = value;
        }

        public override float Value { get; }

        public override void Apply(Player player, Location location)
        {
            if (location.MapTile.Resource == Resource.Gold)
            {
                location.Income.Value = AddToBalance(location.Income.Value);
                player.ScorpionsBalance.Value = AddToBalance(player.ScorpionsBalance.Value);
            }
        }
    }
}