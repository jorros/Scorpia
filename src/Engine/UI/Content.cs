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
            Text = text
        });
    }
    
    public static implicit operator Content(Image image)
    {
        return new Content(image);
    }
}