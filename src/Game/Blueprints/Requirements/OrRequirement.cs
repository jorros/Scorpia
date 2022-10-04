using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Blueprints.Requirements;

public sealed class OrRequirement : Requirement
{
    public IEnumerable<Requirement> Requirements { get; set; }

    public override int Value => default;
    public override int Index => Requirements.First().Index;

    public override bool IsFulfilled(PlayerNode player, MapTile mapTile)
    {
        return Requirements.Any(x => x.IsFulfilled(player, mapTile));
    }
}