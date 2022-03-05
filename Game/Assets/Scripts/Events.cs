namespace Scorpia.Assets.Scripts
{
    public static class Events
    {
        public static Notification PLAYER_DISCONNECTED => new Notification("Player disconnected", "{0} has disconnected from the game.", "None");
    }
}