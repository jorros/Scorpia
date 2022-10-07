using System.Text;
using Scorpia.Game.Nodes.Entities;

namespace Scorpia.Game.HUD;

public class BalanceSheetFormatter
{
    public static string Format(BalanceSheet balance)
    {
        var sb = new StringBuilder();
        sb.AppendLine("<text style='bold'>Incoming</text>");
        Append(sb, "Taxes", balance.PopulationIn);
        Append(sb, "Buildings", balance.BuildingIn);

        if (balance.PopulationIn == null && balance.BuildingIn == null)
        {
            sb.AppendLine("<text style='italic'>None</text>");
        }

        sb.AppendLine();
            
        sb.AppendLine("<text style='bold'>Outgoing</text>");
        Append(sb, "Buildings", balance.BuildingOut, true);
        Append(sb, "Population", balance.PopulationOut, true);
            
        if (balance.BuildingOut == null && balance.PopulationOut == null)
        {
            sb.AppendLine("<text style='italic'>None</text>");
        }

        return sb.ToString();
    }
        
    private static void Append(StringBuilder sb, string infoName, float? value, bool inverse = false)
    {
        if (value is not null)
        {
            sb.AppendLine($"{infoName}: {value.Value.FormatBalance(inverse)}");
        }
    }
}