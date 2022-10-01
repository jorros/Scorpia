using Scorpia.Engine.Network.Protocol;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Location;

namespace Scorpia.Game.Nodes;

public class LocationNode : NetworkedNode
{
    public NetworkedVar<LocationType> Type { get; } = new();
}