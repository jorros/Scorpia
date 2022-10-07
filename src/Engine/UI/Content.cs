using Scorpia.Engine.Asset;

namespace Scorpia.Engine.UI;

public class Content
{
    public UIElement Value { get; }
    
    private Content(UIElement element)
    {
        Value = element;
    }
    
    public static implicit operator Content(string text)
    {
        return new Content(new Label
        {
            Text = text,
            TextAlign = TextAlign.Center,
            Type = "Button"
        });
    }
    
    public static implicit operator Content(Image image)
    {
        return new Content(image);
    }
    
    public static implicit operator Content(Label label)
    {
        return new Content(label);
    }
}