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
}