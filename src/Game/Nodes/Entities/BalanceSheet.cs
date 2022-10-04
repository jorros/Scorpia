using CommunityToolkit.HighPerformance;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

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
    
    public void Write(Stream stream, PacketManager packetManager)
    {
        stream.Write(BuildingOut is not null);
        if (BuildingOut is not null)
        {
            stream.Write(BuildingOut.Value);
        }
        stream.Write(BuildingIn is not null);
        if (BuildingIn is not null)
        {
            stream.Write(BuildingIn.Value);
        }
        stream.Write(PopulationIn is not null);
        if (PopulationIn is not null)
        {
            stream.Write(PopulationIn.Value);
        }
        stream.Write(PopulationOut is not null);
        if (PopulationOut is not null)
        {
            stream.Write(PopulationOut.Value);
        }
    }

    public void Read(Stream stream, PacketManager packetManager)
    {
        if (stream.Read<bool>())
        {
            BuildingOut = stream.Read<float>();
        }
        if (stream.Read<bool>())
        {
            BuildingIn = stream.Read<float>();
        }
        if (stream.Read<bool>())
        {
            PopulationIn = stream.Read<float>();
        }
        if (stream.Read<bool>())
        {
            PopulationOut = stream.Read<float>();
        }
    }
}