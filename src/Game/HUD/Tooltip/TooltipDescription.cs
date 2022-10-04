namespace Scorpia.Game.HUD.Tooltip;

public class TooltipDescription
{
    public TooltipDescription(string header, string content) : this(header, content, string.Empty)
    {
    }

    public TooltipDescription(string header, string content, string subHeader, TooltipPosition position = TooltipPosition.None)
    {
        Header = header;
        Content = content;
        SubHeader = subHeader;
        Position = position;
    }

    public static TooltipDescription Empty => new TooltipDescription(string.Empty, string.Empty);

    public TooltipPosition Position { get; }

    public string Header { get; }

    public string SubHeader { get; }

    public string Content { get; }
}