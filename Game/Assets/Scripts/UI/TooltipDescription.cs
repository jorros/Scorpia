using System;

namespace Scorpia.Assets.Scripts.UI
{
    public class TooltipDescription
    {
        public TooltipDescription(string header, string content) : this(header, content, String.Empty, TooltipPosition.None)
        {
        }

        public TooltipDescription(string header, string content, TooltipPosition position) : this(header, content, string.Empty, position)
        {
        }

        public TooltipDescription(string header, string content, string subHeader) : this(header, content, subHeader, TooltipPosition.None)
        {

        }

        public TooltipDescription(string header, string content, string subHeader, TooltipPosition position)
        {
            Header = header;
            Content = content;
            SubHeader = subHeader;
            Position = position;
        }

        public static TooltipDescription Empty => new TooltipDescription(string.Empty, string.Empty);

        public TooltipPosition Position { get; set; }

        public string Header { get; private set; }

        public string SubHeader { get; private set; }

        public string Content { get; private set; }
    }
}

