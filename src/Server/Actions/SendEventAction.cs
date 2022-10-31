using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Nodes;
using Scorpia.Game.Notifications;

namespace Server.Actions;

public class SendEventAction : IAction
{
    public string Name => "event";
    public void Execute(IServiceProvider serviceProvider)
    {
        NotificationNode.Send(new FamineNotification
        {
            CityName = "Inglewood"
        }, 1);
    }
}