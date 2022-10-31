using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.UI;
using Scorpia.Engine.UI.Style;

namespace Scorpia.Game.HUD.Tooltip;

public class TooltippedElement<T> : UIElement where T : UIElement
{
    public T Value { get; }
    public TooltipDescription? Description { get; set; }

    private readonly Sprite _tooltipBackground;
    private const int Offset = 20;
    private const int MaxWidth = 600;
    private const int MinWidth = 200;
    private const int Margin = 15;

    public new PointF Position
    {
        get => Value.Position;
        set => Value.Position = value;
    }

    public new PointF GetPosition()
    {
        return Value.GetPosition();
    }

    public TooltippedElement(T value, AssetManager assetManager)
    {
        Value = value;
        _tooltipBackground = assetManager.Get<Sprite>("UI:tooltip");
        Depth = -100;
    }

    protected override void OnInit(RenderContext renderContext, Stylesheet stylesheet)
    {
        if (Value.Parent is null && Parent is Container container)
        {
            container.Attach(Value);
        }
    }

    protected override void OnRender(RenderContext renderContext, Stylesheet stylesheet, bool inWorld)
    {
        if (Description is null || (string.IsNullOrWhiteSpace(Description.Header) && string.IsNullOrWhiteSpace(Description.Content)))
        {
            return;
        }

        var pos = Value.GetPosition();

        var rect = new RectangleF(pos.X, pos.Y, Value.Width, Value.Height);
        if (!rect.Contains(Input.MousePosition))
        {
            return;
        }

        var headerStyle = stylesheet.GetLabel("tooltip-header");
        var subHeaderStyle = stylesheet.GetLabel("tooltip-subheader");
        var contentStyle = stylesheet.GetLabel("tooltip-content");

        var headerSize = headerStyle.Font.CalculateSize(Description.Header, headerStyle.ToFontSettings());
        var subHeaderSize = subHeaderStyle.Font.CalculateSize(Description.SubHeader, subHeaderStyle.ToFontSettings());
        var contentSize = contentStyle.Font.CalculateSize(Description.Content, contentStyle.ToFontSettings());

        var actualWidth = Math.Max(Math.Max(headerSize.Width, subHeaderSize.Width), contentSize.Width);
        actualWidth = Math.Min(actualWidth, MaxWidth) + Margin * 2;
        actualWidth = Math.Max(actualWidth, MinWidth);
        var actualHeight = headerSize.Height + subHeaderSize.Height + contentSize.Height + Margin * 2;
        
        var screenSize = renderContext.DrawSize;

        float actualY;

        switch (Description.Position)
        {
            case TooltipPosition.Info:
                actualY = screenSize.Height - 575 - actualHeight;
                break;
            
            case TooltipPosition.None:
            default:
                var offset = Offset;
                if (rect.Top > screenSize.Height / 2f)
                {
                    offset *= -1;
                    offset -= actualHeight;
                }
                actualY = pos.Y + offset;
                break;
            
            case TooltipPosition.Top:
                actualY = 120;
                break;
        }

        var actualX = pos.X;
        if (actualX + actualWidth > screenSize.Width)
        {
            actualX = screenSize.Width - actualWidth;
        }
        
        var target = new RectangleF(actualX, actualY, actualWidth, actualHeight);
        
        renderContext.Draw(_tooltipBackground, target, 0, Color.White, 255, -1, false);

        var headerPos = new PointF(actualX + Margin, actualY + Margin);
        renderContext.DrawText(headerStyle.Font, headerPos, Description.Header, headerStyle.ToFontSettings(), false);

        var subHeaderPos = new PointF(actualX + Margin, actualY + headerSize.Height * 1.1f + Margin);
        renderContext.DrawText(subHeaderStyle.Font, subHeaderPos, Description.SubHeader, subHeaderStyle.ToFontSettings(), false);

        var contentPos = new PointF(actualX + Margin,
            actualY + headerSize.Height * 1.1f + subHeaderSize.Height * 1.2f + Margin);
        renderContext.DrawText(contentStyle.Font, contentPos, Description.Content, contentStyle.ToFontSettings(), false);
    }
}