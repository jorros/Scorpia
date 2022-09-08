using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Game.Packets;

public struct AuthPacket : INetworkPacket
{
    public string Uid { get; set; }

    public void Write(Stream stream, PacketManager packetManager)
    {
        stream.Write(Uid);
    }

    public void Read(Stream stream, PacketManager packetManager)
    {
        Uid = stream.ReadString();
    }
}