using CommunityToolkit.HighPerformance;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;
using Scorpia.Game.Player;

namespace Scorpia.Game.Lobby;

public struct ReadyPacket : INetworkPacket
{
    public PlayerColor Color { get; set; }
    
    public PlayerFaction Faction { get; set; }
    
    public void Write(Stream stream, PacketManager packetManager)
    {
        stream.Write((byte)Color);
        stream.Write((byte)Faction);
    }

    public void Read(Stream stream, PacketManager packetManager)
    {
        Color = (PlayerColor) stream.ReadByte();
        Faction = (PlayerFaction) stream.ReadByte();
    }
}