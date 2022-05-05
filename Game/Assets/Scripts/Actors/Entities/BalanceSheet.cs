using Unity.Netcode;

namespace Actors.Entities
{
    public struct BalanceSheet : INetworkSerializeByMemcpy
    {
        public float? BuildingOut;

        public float? BuildingIn;

        public float? PopulationIn;

        public float? PopulationOut;

        public float Total => PopulationIn ?? 0 + BuildingIn ?? 0 - BuildingOut ?? 0 - PopulationOut ?? 0;

        public BalanceSheet Add(string name, float value)
        {
            object boxed = this;
            var field = GetType().GetField(name);
            var origVal = (float?) field.GetValue(boxed) ?? 0;
            field.SetValue(boxed, origVal + value);
            return (BalanceSheet) boxed;
        }

        public BalanceSheet Set(string name, float value)
        {
            object boxed = this;
            var field = GetType().GetField(name);
            field.SetValue(boxed, value);
            return (BalanceSheet) boxed;
        }
    }
}