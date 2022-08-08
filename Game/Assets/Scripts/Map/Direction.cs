using System;
using UnityEngine;
using Utils;

namespace Map
{
    [Flags]
    public enum Direction
    {
        NorthEast = 1,
        East = 2,
        SouthEast = 4,
        SouthWest = 8,
        West = 16,
        NorthWest = 32
    }

    public static class DirectionExtensions
    {
        public static Direction GetOpposite(this Direction current)
        {
            var opposing = (int)(current + 3) % 6;
            var opposingDir = (Direction)opposing;
            
            return opposingDir;
        }
    }
}