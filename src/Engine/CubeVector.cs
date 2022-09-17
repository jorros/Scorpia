using System;

namespace Scorpia.Engine;

public readonly struct CubeVector : IEquatable<CubeVector>
{
    public int Q { get; init; }
    public int R { get; init; }
    public int S { get; init; }

    public static CubeVector FromAngle(double angle) => angle switch
    {
        > 330 => new CubeVector(1, 0, -1),
        > 270 => new CubeVector(1, -1, 0),
        > 210 => new CubeVector(0, -1, 1),
        > 150 => new CubeVector(-1, 0, 1),
        > 90 => new CubeVector(-1, +1, 0),
        > 30 => new CubeVector(0, +1, -1),
        _ => new CubeVector(1, 0, -1)
    };

    public CubeVector(int q, int r, int s)
    {
        Q = q;
        R = r;
        S = s;
    }
    
    public double DistanceTo(CubeVector to)
    {
        var vector = to - this;

        return (Math.Abs(vector.Q) + Math.Abs(vector.R) + Math.Abs(vector.S)) / 2;
    }
    
    public static CubeVector operator +(CubeVector a) => a;
    public static CubeVector operator -(CubeVector a) => new(-a.Q, -a.R, -a.S);

    public static CubeVector operator +(CubeVector a, CubeVector b)
        => new(a.Q + b.Q, a.R + b.R, a.S + b.S);

    public static CubeVector operator -(CubeVector a, CubeVector b)
        => a + (-b);

    public static CubeVector operator *(CubeVector a, CubeVector b)
        => new CubeVector(a.Q * b.Q, a.R * b.R, a.S * b.S);

    public static CubeVector operator /(CubeVector a, CubeVector b)
    {
        if (b.Q == 0 || b.R == 0 || b.S == 0)
        {
            throw new DivideByZeroException();
        }

        return new CubeVector(a.Q / b.Q, a.R / b.R, a.S / b.S);
    }
    
    public static bool operator ==(CubeVector obj1, CubeVector obj2) => obj1.Equals(obj2);
    public static bool operator !=(CubeVector obj1, CubeVector obj2) => !(obj1 == obj2);

    public OffsetVector ToOffset()
    {
        return new OffsetVector(Q + (R - (R & 1)) / 2, R);
    }

    public bool Equals(CubeVector other)
    {
        return Q == other.Q && R == other.R && S == other.S;
    }

    public override bool Equals(object obj)
    {
        return obj is CubeVector other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Q, R, S);
    }
}