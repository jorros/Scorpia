using System;

namespace Scorpia.Engine.Graphics;

internal struct TilemapOrientationMatrix
{
    public readonly float f0, f1, f2, f3;
    public readonly float b0, b1, b2, b3;
    public readonly float startAngle;

    private TilemapOrientationMatrix(float f0, float f1, float f2, float f3, float b0, float b1, float b2, float b3,
        float startAngle)
    {
        this.f0 = f0;
        this.f1 = f1;
        this.f2 = f2;
        this.f3 = f3;
        this.b0 = b0;
        this.b1 = b1;
        this.b2 = b2;
        this.b3 = b3;
        this.startAngle = startAngle;
    }

    public static TilemapOrientationMatrix Pointy { get; } = new((float)Math.Sqrt(3.0), (float)Math.Sqrt(3.0) / 2.0f, 0.0f,
        3.0f / 2.0f,
        ((float)Math.Sqrt(3.0)) / 3.0f, -1.0f / 3.0f, 0.0f, 2.0f / 3.0f,
        0.5f);

    public static TilemapOrientationMatrix Flat { get; } = new(3.0f / 2.0f, 0.0f, (float)Math.Sqrt(3.0) / 2.0f,
        (float)Math.Sqrt(3.0),
        2.0f / 3.0f, 0.0f, -1.0f / 3.0f, (float)Math.Sqrt(3.0) / 3.0f,
        0.0f);
}

public enum TilemapOrientation
{
    Pointy,
    Flat
}