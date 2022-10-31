using Scorpia.Game.HUD;

namespace Scorpia.Game.Notifications;

public record NotificationAction
{
    public string Label { get; set; }
    
    public Action? Action { get; set; }
    
    public NotificationActionType Type { get; set; }
}