using System;
using System.Runtime.InteropServices;
using static SDL2.SDL;

namespace Scorpia.Engine.InputManagement;

public static class Input
{
    public static event EventHandler<KeyboardEventArgs> OnKeyboard;

    internal static void RaiseEvent(SDL_KeyboardEvent key)
    {
        OnKeyboard?.Invoke(null, new KeyboardEventArgs
        {
            Type = key.type == SDL_EventType.SDL_KEYUP ? KeyboardEventType.KeyUp : KeyboardEventType.KeyDown,
            Repeated = key.repeat != 0,
            Key = key.keysym.scancode.ToKey()
        });
    }

    public static bool IsKeyDown(KeyboardKey key)
    {
        var ptr = SDL_GetKeyboardState(out var num);
        var keys = new byte[num];
        Marshal.Copy(ptr, keys, 0, num);

        return keys[(int) key] == 1;
    }
}