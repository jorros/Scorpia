using Actors;
using Map;

namespace Blueprints.Production
{
    public class SofrumProduction : Production
    {
        public SofrumProduction(float value)
        {
            Value = value;
        }

        public override float Value { get; }

        public override void Apply(Player player, Location location)
        {
            if (location.MapTile.Resource == Resource.Sofrum)
            {
                player.SofrumBalance.Value = AddToBalance(player.SofrumBalance.Value);
            }
        }
    }
}