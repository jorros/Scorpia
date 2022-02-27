using UnityEngine;

namespace Scorpia.Assets.Scripts.Map
{
    public enum Direction
    {
        NorthEast,
        East,
        SouthEast,
        SouthWest,
        West,
        NorthWest
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