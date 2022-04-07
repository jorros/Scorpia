public static class Notifications
{
    public static Notification PlayerDisconnected => new Notification
    {
        TooltipHeader = "Player disconnected",
        TooltipText = "{0} has disconnected from the game.",
        Cover = 0,
        Icon = 0,
        Header = "{0} has left the realm",
        Text = "{0} armies will stand down. In case of a random disconnect don't attack his/her cities or armies."
    };
}