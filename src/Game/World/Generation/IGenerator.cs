using Scorpia.Game.Nodes;
using Scorpia.Game.Utils;

namespace Scorpia.Game.World.Generation;

public interface IGenerator
{
    void Generate(MapNode map, NoiseMap noiseMap);
}