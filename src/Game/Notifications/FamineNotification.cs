using Scorpia.Game.HUD;
using Scorpian.Network;

namespace Scorpia.Game.Notifications;

public struct FamineNotification : INotification
{
    public string Cover => "famine";
    public string Icon => "famine";
    public string Title => $"Famine in {CityName}";
    public string Text => $"The silos are empty and your people are dying on the streets of {CityName}. You need to produce more food by building farms.";
    public bool Immediate => false;
    public string CityName { get; set; }

    public NotificationAction? Action1 => new()
    {
        Label = "That is unfortunate",
        Type = NotificationActionType.Standard
    };

    public NotificationAction? Action2 => null;
    public NotificationAction? Action3 => null;
    
    public void Write(BinaryWriter writer, PacketManager packetManager)
    {
        writer.Write(CityName);
    }

    public void Read(BinaryReader reader, PacketManager packetManager)
    {
        CityName = reader.ReadString();
    }
}