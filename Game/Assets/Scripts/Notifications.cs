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

    public static Notification Hunger => new Notification
    {
        TooltipHeader = "Hunger in {0}",
        TooltipText = "People are leaving {0}, because there is not enough food.",
        Cover = 1,
        Icon = 1,
        Header = "The Rise of Hunger and How to Make It Stop",
        Text =
            "Your people are suffering from hunger in {0}. Build more farms either in {0} or your empire to increase your food production. As of now you are losing people in {0}."
    };
    
    public static Notification BuildingFinished => new Notification
    {
        TooltipHeader = "Construction finished in {1}",
        TooltipText = "The construction of {0} in {1} has been finished.",
        Cover = 2,
        Icon = 2,
        Header = "What Construction Has in Common With Lady Gaga",
        Text =
            "You should be proud. Your underlings managed to finish the construction of {0}. Really cool!"
    };
}