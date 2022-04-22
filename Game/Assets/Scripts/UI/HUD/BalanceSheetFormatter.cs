using System.Text;
using Actors.Entities;
using Utils;

namespace UI.HUD
{
    public class BalanceSheetFormatter
    {
        private readonly BalanceSheet balance;

        public BalanceSheetFormatter(BalanceSheet balance)
        {
            this.balance = balance;
        }

        public string GetSummary()
        {
            var sb = new StringBuilder();
            sb.AppendLine("<b>Incoming</b>");
            Append(sb, "Taxes", balance.TaxIn);
            Append(sb, "Buildings", balance.BuildingIn);

            if (balance.TaxIn == null && balance.BuildingIn == null)
            {
                sb.AppendLine("<i>None</i>");
            }

            sb.AppendLine();
            
            sb.AppendLine("<b>Outgoing</b>");
            Append(sb, "Buildings", balance.BuildingOut, true);
            
            if (balance.BuildingOut == null)
            {
                sb.AppendLine("<i>None</i>");
            }


            return sb.ToString();
        }
        
        private void Append(StringBuilder sb, string infoName, float? value, bool inverse = false)
        {
            if (value is not null)
            {
                sb.AppendLine($"{infoName}: {value.Value.FormatBalance(inverse)}");
            }
        }
    }
}