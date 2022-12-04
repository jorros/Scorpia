using CommunityToolkit.HighPerformance;
using Scorpian.Network;

namespace Scorpia.Game.Player;

public record ServerPlayer : Player
{
    public string DeviceId { get; set; }
    
    public byte LoadingProgress { get; set; }

    public override void Read(BinaryReader reader, PacketManager packetManager)
    {
        base.Read(reader, packetManager);
        DeviceId = reader.ReadString();
        LoadingProgress = reader.ReadByte();
    }

    public override void Write(BinaryWriter writer, PacketManager packetManager)
    {
        base.Write(writer, packetManager);
        writer.Write(DeviceId);
        writer.Write(LoadingProgress);
    }
}