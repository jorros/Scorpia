using System.IO;

namespace Scorpia.Engine.Network.Packets;

public struct SwitchScenePacket : INetworkPacket
{
    public string Name { get; set; }
    public void Write(Stream stream, PacketManager packetManager)
    {
        stream.Write(Name);
    }

    public void Read(Stream stream, PacketManager packetManager)
    {
        Name = stream.ReadString();
    }
}