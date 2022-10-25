using Scorpia.Engine.HexMap;
using Scorpia.Engine.Maths;
using Scorpia.Game.Nodes;
using Scorpia.Game.Utils;

namespace Scorpia.Game.World.Generation;

public class RiverGenerator : IGenerator
{
    private const int RiverLength = 50;
    private const int RiverMaxCount = 5;
    private const int Octaves = 3;
    private const float StartFrequency = 0.01f;
    private const float Persistance = 0.5f;

    private const bool ConverganceEnabled = true;
    
    public void Generate(MapNode map, NoiseMap noiseMap)
    {
        var result = noiseMap.FindLocalMaxima();
        var toCreate = result
            .Where(pos => BiomeHelper.GetByNoise(noiseMap.GetPosition(pos.X, pos.Y)) == Biome.Mountain)
            .OrderBy(a => Guid.NewGuid())
            .Take(RiverMaxCount)
            .Select(x => x.ToCube())
            .ToList();

        var waterMinimas = noiseMap
            .FindLocalMinima()
            .Where(pos => BiomeHelper.GetByNoise(noiseMap.GetPosition(pos.X, pos.Y)) == Biome.Water)
            .OrderBy(pos => noiseMap.GetPosition(pos.X, pos.Y))
            .Take(20)
            .Select(x => x.ToCube())
            .ToList();

        foreach (var item in toCreate)
        {
            var river = CreateRiver(item, waterMinimas);
            MapTile previousTile = null;

            foreach (var position in river)
            {
                var currentTile = map.Map.GetData(position);

                if (currentTile == null)
                {
                    break;
                }

                Direction? flowDirection = null;

                if (previousTile is not null)
                {
                    previousTile.River ??= new List<Direction>();
                    var prevRiver = (List<Direction>)previousTile.River;

                    var dir = previousTile.GetDirection(currentTile).Value;
                    if (!prevRiver.Contains(dir))
                    {
                        prevRiver.Add(dir);
                    }

                    flowDirection = currentTile.GetDirection(previousTile);
                }

                if (flowDirection is not null)
                {
                    currentTile.River ??= new List<Direction>();

                    if (!currentTile.River.Contains(flowDirection.Value))
                    {
                        ((List<Direction>) currentTile.River).Add(flowDirection.Value);
                    }
                }

                previousTile = currentTile;
            }
        }
    }
    
    private IReadOnlyList<Hex> CreateRiver(Hex startPosition, IEnumerable<Hex> waterMinimas)
    {
        PerlinWorm worm;
        if (ConverganceEnabled)
        {
            var closestWaterPos = waterMinimas.MinBy(pos => pos.DistanceTo(startPosition));

            worm = new PerlinWorm(Octaves, Persistance, StartFrequency, startPosition, closestWaterPos);
        }
        else
        {
            worm = new PerlinWorm(Octaves, Persistance, StartFrequency, startPosition);
        }

        return worm
            .MoveLength(RiverLength)
            .ToList();
    }
}