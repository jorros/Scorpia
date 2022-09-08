using System.IO;
using CommunityToolkit.HighPerformance;

namespace Scorpia.Engine.Network.Packets;

public struct LoginResponsePacket : INetworkPacket
{
    public bool Succeeded { get; set; }
    public string Reason { get; set; }

    public LoginResponsePacket()
    {
        Reason = string.Empty;
        Succeeded = false;
    }
    
    public void Write(Stream stream, PacketManager packetManager)
    {
        stream.Write(Succeeded);
        stream.Write(Reason);
    }

    public void Read(Stream stream, PacketManager packetManager)
    {
        Succeeded = stream.Read<bool>();
        Reason = stream.ReadString();
    }
}