using System.IO;
using CommunityToolkit.HighPerformance;

namespace Scorpia.Engine.Network.Packets;

public struct CreateNodePacket : INetworkPacket
{
    public uint NetworkId { get; set; }
    public string Node { get; set; }
    public string Scene { get; set; }

    public void Write(Stream stream, PacketManager packetManager)
    {
        stream.Write(NetworkId);
        stream.Write(Node);
        stream.Write(Scene);
    }

    public void Read(Stream stream, PacketManager packetManager)
    {
        NetworkId = stream.Read<uint>();
        Node = stream.ReadString();
        Scene = stream.ReadString();
    }
}