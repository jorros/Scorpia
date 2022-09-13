using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Game.Lobby;

public class JoinMatchPacket : INetworkPacket
{
    public string DeviceId { get; set; }
    
    public string Name { get; set; }
    
    public void Write(Stream stream, PacketManager packetManager)
    {
        stream.Write(DeviceId);
        stream.Write(Name);
    }

    public void Read(Stream stream, PacketManager packetManager)
    {
        DeviceId = stream.ReadString();
        Name = stream.ReadString();
    }
}