using System;
using System.Drawing;
using static SDL2.SDL;

namespace Scorpia.Engine;

public readonly struct OffsetVector : IEquatable<OffsetVector>
{
    public OffsetVector(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; init; }

    public int Y { get; init; }

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

    public static OffsetVector operator +(OffsetVector a, int b)
        => new(a.X + b, a.Y + b);

    public static OffsetVector operator -(OffsetVector a, int b)
        => a + (-b);

    public static OffsetVector operator *(OffsetVector a, int b)
        => new(a.X * b, a.Y * b);

    public static OffsetVector operator /(OffsetVector a, int b)
    {
        if (b == 0)
        {
            throw new DivideByZeroException();
        }

        return new OffsetVector(a.X / b, a.Y / b);
    }
    
    public static bool operator ==(OffsetVector obj1, OffsetVector obj2) => obj1.Equals(obj2);
    public static bool operator !=(OffsetVector obj1, OffsetVector obj2) => !(obj1 == obj2);

    internal SDL_Point ToSdl()
    {
        return new SDL_Point
        {
            x = X,
            y = Y
        };
    }

    internal Point ToPoint()
    {
        return new Point(X, Y);
    }

    public CubeVector ToCube()
    {
        var q = X - (Y - (X & 1)) / 2;
        var r = Y;
        var s = -q - r;

        return new CubeVector(q, r, s);
    }

    public bool Equals(OffsetVector other)
    {
        return X == other.X && Y == other.Y;
    }

    public override bool Equals(object obj)
    {
        return obj is OffsetVector other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(X, Y);
    }
}