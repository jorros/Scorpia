using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Utils;

namespace Map.Generation
{
    public class RiverGenerator : IGenerator
    {
        private const int RiverLength = 50;
        private const int RiverMaxCount = 5;
        private const int Octaves = 3;
        private const float StartFrequency = 0.01f;
        private const float Persistance = 0.5f;

        private const bool ConverganceEnabled = true;

        public void Generate(Map map, NoiseMap noiseMap)
        {
            var result = noiseMap.FindLocalMaxima();
            var toCreate = result
                .Where(pos => BiomeHelper.GetByNoise(noiseMap.GetPosition(pos.x, pos.y)) == Biome.Mountain)
                .OrderBy(a => Guid.NewGuid())
                .Take(RiverMaxCount)
                .Select(x => x.ToCube())
                .ToList();

            var waterMinimas = noiseMap
                .FindLocalMinima()
                .Where(pos => BiomeHelper.GetByNoise(noiseMap.GetPosition(pos.x, pos.y)) == Biome.Water)
                .OrderBy(pos => noiseMap.GetPosition(pos.x, pos.y))
                .Take(20)
                .Select(x => x.ToCube())
                .ToList();

            foreach (var item in toCreate)
            {
                var river = CreateRiver(item, waterMinimas);
                MapTile previousTile = null;

                foreach (var position in river)
                {
                    var currentTile = map.GetTile(position);

                    if (currentTile == null)
                    {
                        break;
                    }

                    Direction? flowDirection = null;

                    if (previousTile != null)
                    {
                        var dir = new List<Direction>();

                        if (previousTile.River != null)
                        {
                            dir.Add(previousTile.River.Value);
                        }

                        dir.Add(previousTile.GetDirection(currentTile).Value);

                        previousTile.River = dir.Combine();

                        flowDirection = currentTile.GetDirection(previousTile);
                    }

                    currentTile.River = flowDirection;

                    previousTile = currentTile;
                }
            }
        }

        private IReadOnlyList<Vector3Int> CreateRiver(Vector3Int startPosition, List<Vector3Int> waterMinimas)
        {
            PerlinWorm worm;
            if (ConverganceEnabled)
            {
                var closestWaterPos = waterMinimas
                    .OrderBy(pos => pos.DistanceTo(startPosition))
                    .First();

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
}