using System.Collections.Generic;
using DevConsole;

namespace Scorpia.Assets.Scripts.ConsoleCommands
{
	[ConsoleCommand(new string[] { "notification" })]
	public class NotificationCommand
	{
		public static string Help(string command, bool verbose)
		{
			return "Sends a notification";
		}

        public static string Execute(string[] tokens)
        {
            var notification = new Notification(tokens[0], tokens[1], int.Parse(tokens[2]));

            EventManager.Trigger(EventManager.ReceiveNotification, notification);

            return "Notification triggered";
        }

        public static List<string> FetchAutocompleteOptions(string command, string[] tokens)
        {
            var list = new List<string>();

            if (tokens.Length < 1)
            {
                list.Add("<TITLE>");
            }
            if (tokens.Length < 2)
            {
                list.Add("<TEXT>");
            }
            if (tokens.Length < 3)
            {
                list.Add("<ICON>");
            }

            return list;
        }
    }
}

