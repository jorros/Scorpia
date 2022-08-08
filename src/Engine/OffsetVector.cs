using static SDL2.SDL;

namespace Scorpia.Engine;

public struct OffsetVector
{
    public OffsetVector(int x, int y)
    {
        X = x;
        Y = y;
    }

    public int X { get; }

    public int Y { get; }

    public static OffsetVector Zero { get; } = new(0, 0);

    internal SDL_Point ToSdl()
    {
        return new SDL_Point
        {
            x = X,
            y = Y
        };
    }
}