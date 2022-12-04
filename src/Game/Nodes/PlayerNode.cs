using Microsoft.Extensions.DependencyInjection;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.Player;
using Scorpian.Network.Protocol;
using Scorpian.SceneManagement;

namespace Scorpia.Game.Nodes;

public class PlayerNode : NetworkedNode
{
    public NetworkedVar<string> Name { get; } = new();
    public NetworkedVar<uint> Uid { get; } = new();
    public NetworkedVar<byte> Color { get; } = new();
    public NetworkedVar<float> Scorpions { get; }
    public NetworkedVar<float> Zellos { get; }
    public NetworkedVar<float> Nitra { get; }
    public NetworkedVar<float> Sofrum { get; }
    public NetworkedVar<float> Food { get; }
    public NetworkedVar<int> Population { get; }
    
    public NetworkedVar<BalanceSheet> ScorpionsBalance { get; }
    public NetworkedVar<BalanceSheet> FoodBalance { get; }
    public NetworkedVar<BalanceSheet> ZellosBalance { get; }
    public NetworkedVar<BalanceSheet> NitraBalance { get; }
    public NetworkedVar<BalanceSheet> SofrumBalance { get; }

    private bool OnlyOwner(uint client) => client == Uid.Value;

    public PlayerNode()
    {
        Scorpions = new NetworkedVar<float>(OnlyOwner);
        Zellos = new NetworkedVar<float>(OnlyOwner);
        Nitra = new NetworkedVar<float>(OnlyOwner);
        Sofrum = new NetworkedVar<float>(OnlyOwner);
        Food = new NetworkedVar<float>(OnlyOwner);
        Population = new NetworkedVar<int>(OnlyOwner);
        ScorpionsBalance = new NetworkedVar<BalanceSheet>(OnlyOwner);
        FoodBalance = new NetworkedVar<BalanceSheet>(OnlyOwner);
        ZellosBalance = new NetworkedVar<BalanceSheet>(OnlyOwner);
        NitraBalance = new NetworkedVar<BalanceSheet>(OnlyOwner);
        SofrumBalance = new NetworkedVar<BalanceSheet>(OnlyOwner);
    }

    public override Task OnInit()
    {
        ServiceProvider.GetRequiredService<CurrentPlayer>().AddPlayer(this);

        return Task.CompletedTask;
    }
}