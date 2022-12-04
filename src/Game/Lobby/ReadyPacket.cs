using Scorpia.Game.Player;
using Scorpian.Network;
using Scorpian.Network.Packets;

namespace Scorpia.Game.Lobby;

public struct ReadyPacket : INetworkPacket
{
    public PlayerColor Color { get; set; }
    
    public PlayerFaction Faction { get; set; }
    
    public void Write(BinaryWriter writer, PacketManager packetManager)
    {
        writer.Write((byte)Color);
        writer.Write((byte)Faction);
    }

    public void Read(BinaryReader reader, PacketManager packetManager)
    {
        Color = (PlayerColor) reader.ReadByte();
        Faction = (PlayerFaction) reader.ReadByte();
    }
}