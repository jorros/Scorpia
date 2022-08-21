using System.Drawing;
using static SDL2.SDL;

namespace Scorpia.Engine.Helper;

public static class SdlExtensions
{
    public static SDL_Rect ToSdl(this Rectangle rectangle)
    {
        return new SDL_Rect
        {
            h = rectangle.Height,
            w = rectangle.Width,
            x = rectangle.Left,
            y = rectangle.Top
        };
    }

    public static SDL_Color ToSdl(this Color color)
    {
        return new SDL_Color
        {
            r = color.R,
            g = color.G,
            b = color.B,
            a = color.A
        };
    }
}