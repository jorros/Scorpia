namespace Actors.Entities
{
    public struct BalanceSheet
    {
        public float? BuildingOut;

        public float? BuildingIn;

        public float? TaxIn;

        public float Total => TaxIn ?? 0 + BuildingIn ?? 0 - BuildingOut ?? 0;
    }
}