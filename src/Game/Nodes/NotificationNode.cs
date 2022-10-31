using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.Network.Protocol;
using Scorpia.Engine.SceneManagement;
using Scorpia.Engine.UI;
using Scorpia.Game.HUD;
using Scorpia.Game.Notifications;
using Scorpia.Game.Scenes;

namespace Scorpia.Game.Nodes;

public class NotificationNode : NetworkedNode
{
    private BasicLayout _layout = null!;
    private List<Window> _notificationWindows = null!;
    private List<Button> _notificationButtons = null!;

    private const int SpaceBetween = 15;
    private const int YPosition = 60;
    private const int XPosition = 20;
    private const int MovementSpeed = 400;

    private static NotificationNode _node = null!;

    public override void OnInit()
    {
        _node = this;

        if (!NetworkManager.IsClient || Scene is not GameScene scene)
        {
            return;
        }
        
        _layout = scene.layout;
        _notificationButtons = scene.notificationButtons;
        _notificationWindows = scene.notificationWindows;
    }

    [ClientRpc]
    public void Receive(INotification notification)
    {
        AddButton(notification);
    }

    public static void Send(INotification notification, ushort clientId)
    {
        _node.Invoke(nameof(Receive), notification, clientId);
    }

    private void AddWindow(INotification notification)
    {
        var window = new NotificationWindow(AssetManager, notification.Title, notification.Cover, notification.Text);

        void CloseAction()
        {
            MapNodeCamera.clickIsBlocked = true;
            _notificationWindows.Remove(window);
            _layout.Remove(window);
        }

        if (notification.Action1 is not null)
        {
            window.AddAction(notification.Action1.Label, notification.Action1.Action, CloseAction, notification.Action1.Type);
        }
        if (notification.Action2 is not null)
        {
            window.AddAction(notification.Action2.Label, notification.Action2.Action, CloseAction, notification.Action2.Type);
        }
        if (notification.Action3 is not null)
        {
            window.AddAction(notification.Action3.Label, notification.Action3.Action, CloseAction, notification.Action3.Type);
        }
        
        _layout.Attach(window);
        _notificationWindows.Add(window);
    }

    private void AddButton(INotification notification)
    {
        var button = new Button
        {
            Position = new Point(XPosition + _notificationButtons.Count * (130 + SpaceBetween), -130),
            Content = new Image
            {
                Sprite = AssetManager.Get<Sprite>($"Game:HUD/notification_icon_{notification.Icon}")
            },
            Type = "notification",
            Depth = 10
        };
        _layout.Attach(button);
        _notificationButtons.Add(button);

        button.OnClick += (sender, args) =>
        {
            var btn = (Button)sender;
            MapNodeCamera.clickIsBlocked = true;
            if (args.Button == MouseButton.Left)
            {
                _notificationButtons.Remove(btn);
                _layout.Remove(btn);
                AddWindow(notification);
            }
            else
            {
                _notificationButtons.Remove(btn);
                _layout.Remove(btn);
            }
        };
    }

    public override void OnRender(RenderContext context, float dT)
    {
        for (var index = 0; index < _notificationButtons.Count; index++)
        {
            var button = _notificationButtons[index];
            var pos = button.Position;

            var posX = pos.X;
            var posY = pos.Y;
            
            if (posY < YPosition)
            {
                posY += MovementSpeed * dT;
            }
            else if (posY != YPosition)
            {
                posY = YPosition;
            }

            var calculatedX = XPosition + index * (130 + SpaceBetween);
            if (posX > calculatedX)
            {
                posX -= MovementSpeed * dT;
            }
            else if (posX != calculatedX)
            {
                posX = calculatedX;
            }

            button.Position = new PointF(posX, posY);
        }
    }
}