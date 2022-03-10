namespace Scorpia.Assets.Scripts
{
    public static class Notifications
    {
        public static Notification PLAYER_DISCONNECTED => new Notification("Player disconnected", "{0} has disconnected from the game.", 0);
    }
}