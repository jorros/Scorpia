using System.Drawing;
using Scorpian.Asset;
using Scorpian.InputManagement;
using Scorpian.UI;

namespace Scorpia.Game.HUD;

public class NotificationWindow : Window
{
    public NotificationWindow(AssetManager assetManager, string title, string cover, string text) : base()
    {
        Anchor = UIAnchor.Center;
        Type = "notification";
        Height = 1000;
        
        AttachTitle(title);

        var coverImage = new Image
        {
            Sprite = assetManager.Get<Sprite>($"Game:HUD/notification_cover_{cover}"),
            Width = 745,
            Height = 373,
            Position = new Point(1, 0)
        };
        Attach(coverImage);
        
        var contentLabel = new Label
        {
            Type = "notification",
            Position = new Point(20, 390),
            Text = text,
            MaxWidth = 745 - 46
        };
        Attach(contentLabel);
    }

    public void AddAction(string text, Action? action, Action closeAction, NotificationActionType type = NotificationActionType.Standard)
    {
        var button = new Button
        {
            Type = type switch
            {
                NotificationActionType.Positive => "action_positive",
                NotificationActionType.Negative => "action_negative",
                _ => "action_regular"
            },
            Content = text,
        };
        button.OnClick += (_, args) =>
        {
            if (args.Button == MouseButton.Left)
            {
                action?.Invoke();
                closeAction.Invoke();
            }
        };
        ActionBar.Attach(button);
    }
}