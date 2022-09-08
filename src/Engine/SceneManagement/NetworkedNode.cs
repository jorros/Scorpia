using System;

namespace Scorpia.Engine.SceneManagement;

public abstract class NetworkedNode : Node
{
    protected uint NetworkId { get; private set; }
    protected new ulong Identifier { get; private set; }
    
    protected new NetworkedScene Scene { get; private set; }
    
    internal void Create(uint networkId)
    {
        NetworkId = networkId;
    }
}