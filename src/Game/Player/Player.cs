using Scorpian.Network;
using Scorpian.Network.Packets;

namespace Scorpia.Game.Player;

public record Player : INetworkPacket
{
    public string Name { get; set; }

    public PlayerColor? Color { get; set; }
    
    public PlayerFaction? Faction { get; set; }

    public uint NetworkId { get; set; }

    public bool Ready => Color is not null && !string.IsNullOrWhiteSpace(Name);

    public virtual void Write(BinaryWriter writer, PacketManager packetManager)
    {
        writer.Write(Name);
        writer.Write(Color is not null);
        if (Color is not null)
        {
            writer.Write((byte) Color);
        }
        
        writer.Write(Faction is not null);
        if (Faction is not null)
        {
            writer.Write((byte) Faction);
        }
    }

    public virtual void Read(BinaryReader reader, PacketManager packetManager)
    {
        Name = reader.ReadString();
        if (reader.ReadBoolean())
        {
            Color = (PlayerColor) reader.ReadByte();
        }
        
        if (reader.ReadBoolean())
        {
            Faction = (PlayerFaction) reader.ReadByte();
        }
    }
}