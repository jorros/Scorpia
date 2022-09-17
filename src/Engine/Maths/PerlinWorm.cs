using System;
using System.Collections.Generic;
using System.Linq;

namespace Scorpia.Engine.Maths;

public class PerlinWorm
{
    private readonly int _octaves;
        private readonly float _persistance;
        private readonly float _startFrequency;
        private double _currentDirection;
        private CubeVector _currentPosition;
        private readonly CubeVector _convergencePoint;

        private readonly bool _moveToConvergencePoint;
        private float _weight = 0.6f;

        public PerlinWorm(int octaves, float persistance, float startFrequency, CubeVector startPosition,
            CubeVector convergencePoint)
        {
            this._octaves = octaves;
            this._persistance = persistance;
            this._startFrequency = startFrequency;
            _currentPosition = startPosition;
            this._convergencePoint = convergencePoint;
            _moveToConvergencePoint = true;
        }

        public PerlinWorm(int octaves, float persistance, float startFrequency, CubeVector startPosition)
        {
            this._octaves = octaves;
            this._persistance = persistance;
            this._startFrequency = startFrequency;
            _currentPosition = startPosition;
            _moveToConvergencePoint = false;
        }

        private CubeVector MoveTowardsConvergencePoint()
        {
            var angle = GetPerlinNoiseAngle();
            var directionToConvergencePoint = (_convergencePoint - _currentPosition).ToOffset();

            var angleToConvergencePoint =
                Math.Atan2(directionToConvergencePoint.Y, directionToConvergencePoint.X) * 180 / Math.PI;

            var endAngle = (angle * (1 - _weight) + angleToConvergencePoint * _weight) % 360;
            endAngle = ClampAngle(endAngle);
            var endVector = CubeVector.FromAngle(endAngle);

            _currentPosition += endVector;

            return _currentPosition;
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
            var noise = MathExt.SumNoise(_currentPosition.Q, _currentPosition.R, _octaves, _persistance,
                _startFrequency);
            var degrees = MathExt.RangeMap(noise, 0, 1, -90, 90);

            _currentDirection += degrees;
            _currentDirection = ClampAngle(_currentDirection);
            
            return _currentDirection;
        }

        private CubeVector Move()
        {
            var angle = GetPerlinNoiseAngle();
            _currentPosition += CubeVector.FromAngle(angle);
            return _currentPosition;
        }

        public IReadOnlyList<CubeVector> MoveLength(int length)
        {
            var list = new List<CubeVector>();
            foreach (var item in Enumerable.Range(0, length))
            {
                if (_moveToConvergencePoint)
                {
                    var result = MoveTowardsConvergencePoint();
                    list.Add(result);
                    if (_convergencePoint.DistanceTo(result) < 1)
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

            if (!_moveToConvergencePoint)
            {
                return list;
            }

            var debugCounter = 0;

            while (_convergencePoint.DistanceTo(_currentPosition) > 1)
            {
                if (debugCounter > 150)
                {
                    break;
                }
                
                _weight = 0.9f;
                var result = MoveTowardsConvergencePoint();
                list.Add(result);
                if (_convergencePoint.DistanceTo(result) < 1)
                {
                    break;
                }

                debugCounter++;
            }


            return list;
        }
}