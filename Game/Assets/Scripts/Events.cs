namespace Scorpia.Assets.Scripts
{
    public static class Events
    {
        public static Event PLAYER_DISCONNECTED => new Event("Player disconnected", "{0} has disconnected from the game.", "None");
    }
}