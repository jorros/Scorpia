using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.UI;

namespace Scorpia.Game.HUD;

public class NotificationWindow : Window
{
    public NotificationWindow(AssetManager assetManager) : base()
    {
        Anchor = UIAnchor.Center;
        Type = "notification";
        Height = 1000;
        
        AttachTitle("Test");

        var cover = new Image
        {
            Sprite = assetManager.Get<Sprite>("Game:HUD/notification_cover_famine"),
            Width = 745,
            Height = 373,
            Position = new Point(1, 0)
        };
        Attach(cover);

        var text =
            "Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren, no sea takimata sanctus est Lorem ipsum dolor sit amet. Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et";

        var contentLabel = new Label
        {
            Type = "notification",
            Position = new Point(20, 390),
            Text = text,
            MaxWidth = 745 - 46
        };
        Attach(contentLabel);

        var goodButton = new Button
        {
            Type = "action_green",
            Content = "Cool"
        };
        ActionBar.Attach(goodButton);

        var badButton = new Button
        {
            Type = "action_red",
            Content = "Not cool"
        };
        ActionBar.Attach(badButton);
        
        var neutralButton = new Button
        {
            Type = "action_regular",
            Content = "Hmm"
        };
        ActionBar.Attach(neutralButton);
    }
}