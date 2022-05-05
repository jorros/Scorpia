using Actors;
using Map;

namespace Blueprints.Production
{
    public class ZellosProduction : Production
    {
        public ZellosProduction(float value)
        {
            Value = value;
        }

        public override float Value { get; }

        public override void Apply(Player player, Location location)
        {
            if (location.MapTile.Resource == Resource.Zellos)
            {
                player.ZellosBalance.Value = AddToBalance(player.ZellosBalance.Value);
            }
        }
    }
}