using Scorpian.Network;
using Scorpian.Network.Packets;

namespace Scorpia.Game.Lobby;

public class JoinMatchPacket : INetworkPacket
{
    public string DeviceId { get; set; }
    
    public string Name { get; set; }
    
    public void Write(BinaryWriter writer, PacketManager packetManager)
    {
        writer.Write(DeviceId);
        writer.Write(Name);
    }

    public void Read(BinaryReader reader, PacketManager packetManager)
    {
        DeviceId = reader.ReadString();
        Name = reader.ReadString();
    }
}