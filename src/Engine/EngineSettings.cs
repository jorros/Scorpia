using Scorpia.Engine.Network;

namespace Scorpia.Engine;

public record EngineSettings
{
    public bool NetworkEnabled { get; set; }
    
    public NetworkMode NetworkMode { get; set; }
    
    public bool Headless { get; set; }
    
    public string Name { get; set; }
    
    public string DisplayName { get; set; }
};