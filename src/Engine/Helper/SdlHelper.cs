using static SDL2.SDL;

namespace Scorpia.Engine.Helper;

public static class SdlHelper
{
    public static SDL_Rect Create(int x, int y, int w, int h)
    {
        return new SDL_Rect
        {
            x = x,
            y = y,
            w = w,
            h = h
        };
    }
}