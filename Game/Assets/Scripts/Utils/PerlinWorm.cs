using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public class PerlinWorm
    {
        private readonly int octaves;
        private readonly float persistance;
        private readonly float startFrequency;
        private double currentDirection;
        private Vector3Int currentPosition;
        private readonly Vector3Int convergencePoint;

        private readonly bool moveToConvergencePoint;
        private float weight = 0.6f;

        public PerlinWorm(int octaves, float persistance, float startFrequency, Vector3Int startPosition,
            Vector3Int convergencePoint)
        {
            this.octaves = octaves;
            this.persistance = persistance;
            this.startFrequency = startFrequency;
            currentPosition = startPosition;
            this.convergencePoint = convergencePoint;
            moveToConvergencePoint = true;
        }

        public PerlinWorm(int octaves, float persistance, float startFrequency, Vector3Int startPosition)
        {
            this.octaves = octaves;
            this.persistance = persistance;
            this.startFrequency = startFrequency;
            currentPosition = startPosition;
            moveToConvergencePoint = false;
        }

        private Vector3Int MoveTowardsConvergencePoint()
        {
            var angle = GetPerlinNoiseAngle();
            var directionToConvergencePoint = (convergencePoint - currentPosition).ToOffset();

            var angleToConvergencePoint =
                Math.Atan2(directionToConvergencePoint.y, directionToConvergencePoint.x) * 180 / Math.PI;

            var endAngle = (angle * (1 - weight) + angleToConvergencePoint * weight) % 360;
            endAngle = ClampAngle(endAngle);
            var endVector = endAngle.AngleToVector();

            currentPosition += endVector;

            return currentPosition;
        }

        private double ClampAngle(double angle)
        {
            return angle switch
            {
                >= 360 => angle % 360,
                < 0 => 360 + angle,
                _ => angle
            };
        }

        private double GetPerlinNoiseAngle()
        {
            var noise = NoiseHelper.SumNoise(currentPosition.x, currentPosition.y, octaves, persistance,
                startFrequency);
            var degrees = NoiseHelper.RangeMap(noise, 0, 1, -90, 90);

            currentDirection += degrees;
            currentDirection = ClampAngle(currentDirection);
            
            return currentDirection;
        }

        private Vector3Int Move()
        {
            var angle = GetPerlinNoiseAngle();
            currentPosition += angle.AngleToVector();
            return currentPosition;
        }

        public IReadOnlyList<Vector3Int> MoveLength(int length)
        {
            var list = new List<Vector3Int>();
            foreach (var item in Enumerable.Range(0, length))
            {
                if (moveToConvergencePoint)
                {
                    var result = MoveTowardsConvergencePoint();
                    list.Add(result);
                    if (convergencePoint.DistanceTo(result) < 1)
                    {
                        break;
                    }
                }
                else
                {
                    var result = Move();
                    list.Add(result);
                }
            }

            if (!moveToConvergencePoint)
            {
                return list;
            }

            var debugCounter = 0;

            while (convergencePoint.DistanceTo(currentPosition) > 1)
            {
                if (debugCounter > 150)
                {
                    break;
                }
                
                weight = 0.9f;
                var result = MoveTowardsConvergencePoint();
                list.Add(result);
                if (convergencePoint.DistanceTo(result) < 1)
                {
                    break;
                }

                debugCounter++;
            }


            return list;
        }
    }
}