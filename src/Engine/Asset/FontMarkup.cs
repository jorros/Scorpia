using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using Scorpia.Engine.Helper;

namespace Scorpia.Engine.Asset;

public class FontMarkup
{
    public IReadOnlyList<TextBlock> TextBlocks { get; private init; }

    private static readonly XmlReaderSettings Settings = new XmlReaderSettings
    {
        IgnoreWhitespace = true,
        CheckCharacters = false,
        ConformanceLevel = ConformanceLevel.Fragment,
    };

    public static FontMarkup Read(string content, TextBlock def = null)
    {
        using var stringReader = new StringReader(content);
        using var reader = XmlReader.Create(stringReader, Settings);

        var blocks = new List<TextBlock>();
        var current = def ?? new TextBlock();
        blocks.Add(current);

        var currentElement = string.Empty;

        while (reader.MoveToNextAttribute() || reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    current = current with { Text = string.Empty};
                    blocks.Add(current);

                    currentElement = reader.Name;
                    break;

                case XmlNodeType.Text:
                    current.Text = reader.Value;
                    break;

                case XmlNodeType.Attribute:
                    Process(currentElement.ToLowerInvariant(), reader.Name.ToLowerInvariant(), reader.Value, current);
                    break;

                case XmlNodeType.EndElement:
                    current = blocks[^2] with { Text = string.Empty };
                    blocks.Add(current);
                    break;
            }
        }

        var markup = new FontMarkup
        {
            TextBlocks = blocks.Where(x => !string.IsNullOrEmpty(x.Text)).ToArray()
        };

        return markup;
    }

    private static void Process(string type, string attribute, string value, TextBlock textBlock)
    {
        switch (type)
        {
            case "text":
                ProcessText(attribute, value, textBlock);
                break;
            
            case "outline":
                ProcessOutline(attribute, value, textBlock);
                break;
        }
    }

    private static void ProcessText(string attribute, string value, TextBlock textBlock)
    {
        switch (attribute)
        {
            case "color":
                textBlock.Color = ColorHelper.HtmlToColor(value);
                break;

            case "size":
                if (int.TryParse(value, out var size))
                {
                    textBlock.Size = size;
                }

                break;

            case "style":
                if (Enum.TryParse(value, true, out FontStyle fontStyle))
                {
                    textBlock.Style = fontStyle;
                }

                break;
        }
    }
    
    private static void ProcessOutline(string attribute, string value, TextBlock textBlock)
    {
        switch (attribute)
        {
            case "color":
                textBlock.OutlineColor = ColorHelper.HtmlToColor(value);
                break;

            case "size":
                if (int.TryParse(value, out var size))
                {
                    textBlock.Outline = size;
                }

                break;
        }
    }

    public record TextBlock
    {
        public int Size { get; set; }
        public int Outline { get; set; }
        public Color OutlineColor { get; set; }
        public FontStyle Style { get; set; }
        public Color Color { get; set; }
        public string Text { get; set; }
    }
}