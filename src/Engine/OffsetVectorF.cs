using System;
using static SDL2.SDL;

namespace Scorpia.Engine;

public struct OffsetVectorF
{
    public OffsetVectorF(double x, double y)
    {
        X = x;
        Y = y;
    }

    public double X { get; init; }

    public double Y { get; init; }

    public static OffsetVectorF Zero { get; } = new(0, 0);

    public static OffsetVectorF One { get; } = new(1, 1);
    
    public static OffsetVectorF operator +(OffsetVectorF a) => a;
    public static OffsetVectorF operator -(OffsetVectorF a) => new(-a.X, -a.Y);

    public static OffsetVectorF operator +(OffsetVectorF a, OffsetVectorF b)
        => new(a.X + b.X, a.Y + b.Y);

    public static OffsetVectorF operator -(OffsetVectorF a, OffsetVectorF b)
        => a + (-b);

    public static OffsetVectorF operator *(OffsetVectorF a, OffsetVectorF b)
        => new OffsetVectorF(a.X * b.X, a.Y * b.Y);

    public static OffsetVectorF operator /(OffsetVectorF a, OffsetVectorF b)
    {
        if (b.X == 0 || b.Y == 0)
        {
            throw new DivideByZeroException();
        }
        return new OffsetVectorF(a.X / b.X, a.Y / b.Y);
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