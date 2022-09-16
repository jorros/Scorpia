using CommunityToolkit.HighPerformance;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Game.Player;

public record Player : INetworkPacket
{
    public string Name { get; set; }

    public PlayerColor? Color { get; set; }

    public ushort NetworkId { get; set; }

    public bool Ready => Color is not null && !string.IsNullOrWhiteSpace(Name);

    public virtual void Write(Stream stream, PacketManager packetManager)
    {
        stream.Write(Name);
        stream.Write(Color is not null);
        if (Color is not null)
        {
            stream.Write((byte) Color);
        }
    }

    public virtual void Read(Stream stream, PacketManager packetManager)
    {
        Name = stream.ReadString();
        if (stream.Read<bool>())
        {
            Color = (PlayerColor) stream.ReadByte();
        }
    }
}