using System;
using static SDL2.SDL;

namespace Scorpia.Engine;

public struct OffsetVector
{
    public OffsetVector(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; init; }

    public double Y { get; init; }

    public static OffsetVector Zero { get; } = new(0, 0);

    public static OffsetVector One { get; } = new(1, 1);
    
    public static OffsetVector operator +(OffsetVector a) => a;
    public static OffsetVector operator -(OffsetVector a) => new(-a.X, -a.Y);

    public static OffsetVector operator +(OffsetVector a, OffsetVector b)
        => new(a.X + b.X, a.Y + b.Y);

    public static OffsetVector operator -(OffsetVector a, OffsetVector b)
        => a + (-b);

    public static OffsetVector operator *(OffsetVector a, OffsetVector b)
        => new OffsetVector(a.X * b.X, a.Y * b.Y);

    public static OffsetVector operator /(OffsetVector a, OffsetVector b)
    {
        if (b.X == 0 || b.Y == 0)
        {
            throw new DivideByZeroException();
        }
        return new OffsetVector(a.X / b.X, a.Y / b.Y);
    }

    internal SDL_Point ToSdl()
    {
        return new SDL_Point
        {
            x = (int)X,
            y = (int)Y
        };
    }
}