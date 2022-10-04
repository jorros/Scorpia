using Scorpia.Engine.Network.Protocol;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Nodes.Entities;
using Scorpia.Game.Player;

namespace Scorpia.Game.Nodes;

public class PlayerNode : NetworkedNode
{
    public NetworkedVar<string> Name { get; } = new();
    public NetworkedVar<string> Uid { get; } = new();
    public NetworkedVar<PlayerColor> Color { get; } = new();
    public NetworkedVar<float> Scorpions { get; } = new();
    public NetworkedVar<float> Zellos { get; } = new();
    public NetworkedVar<float> Nitra { get; } = new();
    public NetworkedVar<float> Sofrum { get; } = new();
    public NetworkedVar<float> Food { get; } = new();
    public NetworkedVar<int> Population { get; } = new();
    
    public NetworkedVar<BalanceSheet> ScorpionsBalance { get; } = new();
    public NetworkedVar<BalanceSheet> FoodBalance { get; } = new();
    public NetworkedVar<BalanceSheet> ZellosBalance { get; } = new();
    public NetworkedVar<BalanceSheet> NitraBalance { get; } = new();
    public NetworkedVar<BalanceSheet> SofrumBalance { get; } = new();
}