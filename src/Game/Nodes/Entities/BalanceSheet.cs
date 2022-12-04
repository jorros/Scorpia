using Scorpian.Network;
using Scorpian.Network.Packets;

namespace Scorpia.Game.Nodes.Entities;

public struct BalanceSheet : INetworkPacket
{
    public float? BuildingOut;

    public float? BuildingIn;

    public float? PopulationIn;

    public float? PopulationOut;

    public float Total => PopulationIn ?? 0 + BuildingIn ?? 0 - BuildingOut ?? 0 - PopulationOut ?? 0;

    public BalanceSheet Add(string name, float value)
    {
        object boxed = this;
        var field = GetType().GetField(name);
        var origVal = (float?) field.GetValue(boxed) ?? 0;
        field.SetValue(boxed, origVal + value);
        return (BalanceSheet) boxed;
    }

    public BalanceSheet Set(string name, float value)
    {
        object boxed = this;
        var field = GetType().GetField(name);
        field.SetValue(boxed, value);
        return (BalanceSheet) boxed;
    }
    
    public void Write(BinaryWriter writer, PacketManager packetManager)
    {
        writer.Write(BuildingOut is not null);
        if (BuildingOut is not null)
        {
            writer.Write(BuildingOut.Value);
        }
        writer.Write(BuildingIn is not null);
        if (BuildingIn is not null)
        {
            writer.Write(BuildingIn.Value);
        }
        writer.Write(PopulationIn is not null);
        if (PopulationIn is not null)
        {
            writer.Write(PopulationIn.Value);
        }
        writer.Write(PopulationOut is not null);
        if (PopulationOut is not null)
        {
            writer.Write(PopulationOut.Value);
        }
    }

    public void Read(BinaryReader reader, PacketManager packetManager)
    {
        if (reader.ReadBoolean())
        {
            BuildingOut = reader.ReadSingle();
        }
        if (reader.ReadBoolean())
        {
            BuildingIn = reader.ReadSingle();
        }
        if (reader.ReadBoolean())
        {
            PopulationIn = reader.ReadSingle();
        }
        if (reader.ReadBoolean())
        {
            PopulationOut = reader.ReadSingle();
        }
    }
}