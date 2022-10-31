using Scorpia.Engine.Network.Packets;

namespace Scorpia.Game.Notifications;

public interface INotification : INetworkPacket
{
    string Cover { get; }

    string Icon { get; }
    
    string Title { get; }
    
    string Text { get; }
    
    bool Immediate { get; }
    
    NotificationAction? Action1 { get; }
    
    NotificationAction? Action2 { get; }
    
    NotificationAction? Action3 { get; }
}