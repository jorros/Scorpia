using Actors;
using Map;

namespace Blueprints.Production
{
    public class NitraProduction : Production
    {
        public NitraProduction(float value)
        {
            Value = value;
        }

        public override float Value { get; }

        public override void Apply(Player player, Location location)
        {
            if (location.MapTile.Resource == Resource.Nitra)
            {
                player.NitraBalance.Value = AddToBalance(player.NitraBalance.Value);
            }
        }
    }
}