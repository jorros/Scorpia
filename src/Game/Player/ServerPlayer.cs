using Scorpia.Engine.Network;

namespace Scorpia.Game.Player;

public record ServerPlayer : Player
{
    public string DeviceId { get; set; }

    public override void Read(Stream stream, PacketManager packetManager)
    {
        base.Read(stream, packetManager);
        DeviceId = stream.ReadString();
    }

    public override void Write(Stream stream, PacketManager packetManager)
    {
        base.Write(stream, packetManager);
        stream.Write(DeviceId);
    }
}