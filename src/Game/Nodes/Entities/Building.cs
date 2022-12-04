using CommunityToolkit.HighPerformance;
using Scorpia.Game.Blueprints;
using Scorpia.Game.Blueprints.Requirements;
using Scorpian.Network;
using Scorpian.Network.Packets;

namespace Scorpia.Game.Nodes.Entities;

public struct Building : IEquatable<Building>, INetworkPacket
{
    public BuildingType Type { get; set; }

    public bool IsBuilding { get; set; }

    public int Level { get; set; }

    public bool Equals(Building other)
    {
        return Type == other.Type && Level == other.Level;
    }

    public override string ToString()
    {
        var buildText = IsBuilding ? "B" : "F";
        return $"{Type}:{Level}:{buildText}";
    }

    public void Tick(PlayerNode player, LocationNode location)
    {
        // Maintenance costs
        var requirements = BuildingBlueprints.GetRequirements(Type);

        foreach (var requirement in requirements)
        {
            switch (requirement)
            {
                case UpkeepRequirement:
                    UpdateIncome(player, location, nameof(BalanceSheet.BuildingOut), requirement.Value);
                    break;
            }
        }

        // Production
        var productions = BuildingBlueprints.GetProduction(Type);
        if (productions != null)
        {
            foreach (var production in productions)
            {
                production.Apply(player, location);
            }
        }
    }

    public void OnFinishBuilding(LocationNode location, bool demolished)
    {
        // Population
        if (Type == BuildingType.Residence)
        {
            location.MaxPopulation.Value = LocationBlueprint.GetMaxPopulation((LocationType)location.Type.Value) + Level * 1000;
        }
    }

    private static void UpdateIncome(PlayerNode player, LocationNode location, string fieldName, int value)
    {
        var balance = location.Income.Value;
        var field = balance.GetType().GetField(fieldName);
        var origVal = (float?) field.GetValue(balance) ?? 0;
        object boxed = balance;
        field.SetValue(boxed, origVal + value);
        location.Income.Value = (BalanceSheet) boxed;

        balance = player.ScorpionsBalance.Value;
        field = balance.GetType().GetField(fieldName);
        origVal = (float?) field.GetValue(balance) ?? 0;
        boxed = balance;
        field.SetValue(boxed, origVal + value);
        player.ScorpionsBalance.Value = (BalanceSheet) boxed;
    }

    public void Write(BinaryWriter writer, PacketManager packetManager)
    {
        writer.Write((byte) Type);
        writer.Write(IsBuilding);
        writer.Write(Level);
    }

    public void Read(BinaryReader reader, PacketManager packetManager)
    {
        Type = (BuildingType) reader.ReadByte();
        IsBuilding = reader.ReadBoolean();
        Level = reader.ReadInt32();
    }
}