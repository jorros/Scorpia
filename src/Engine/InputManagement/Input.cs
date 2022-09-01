using System;
using System.Drawing;
using System.Runtime.InteropServices;
using Scorpia.Engine.Helper;
using static SDL2.SDL;

namespace Scorpia.Engine.InputManagement;

public static class Input
{
    public static event EventHandler<KeyboardEventArgs> OnKeyboard;
    public static event EventHandler<TextInputEventArgs> OnTextInput;
    public static event EventHandler<MouseMoveEventArgs> OnMouseMove;
    public static event EventHandler<MouseButtonEventArgs> OnMouseButton; 
    public static event EventHandler<MouseWheelEventArgs> OnMouseWheel; 

    public static OffsetVector MousePosition { get; private set; }

    private static int CalculateWithDpi(int val, bool highDpi)
    {
        return highDpi ? val * 2 : val;
    }
    
    internal static void RaiseEvent(SDL_KeyboardEvent key)
    {
        OnKeyboard?.Invoke(null, new KeyboardEventArgs
        {
            Type = key.type == SDL_EventType.SDL_KEYUP ? KeyboardEventType.KeyUp : KeyboardEventType.KeyDown,
            Repeated = key.repeat != 0,
            Key = key.keysym.scancode.ToKey()
        });
    }
    
    internal static void RaiseTextInput(char input)
    {
        OnTextInput?.Invoke(null, new TextInputEventArgs
        {
            Character = input
        });
    }

    internal static void CaptureMouseMotion(SDL_MouseMotionEvent motion, bool highDpi)
    {
        MousePosition = new OffsetVector(motion.x, motion.y);
        OnMouseMove?.Invoke(null, new MouseMoveEventArgs
        {
            X = CalculateWithDpi(motion.x, highDpi),
            Y = CalculateWithDpi(motion.y, highDpi),
            DeltaX = motion.xrel,
            DeltaY = motion.yrel
        });
    }

    internal static void CaptureMouseWheel(SDL_MouseWheelEvent wheel)
    {
        OnMouseWheel?.Invoke(null, new MouseWheelEventArgs
        {
            X = wheel.x,
            Y = wheel.y,
            PreciseX = wheel.preciseX,
            PreciseY = wheel.preciseY
        });
    }

    internal static void CaptureMouseButton(SDL_MouseButtonEvent button, bool highDpi)
    {
        OnMouseButton?.Invoke(null, new MouseButtonEventArgs
        {
            Clicks = button.clicks,
            Type = button.type == SDL_EventType.SDL_MOUSEBUTTONUP ? MouseEventType.ButtonUp : MouseEventType.ButtonDown,
            X = CalculateWithDpi(button.x, highDpi),
            Y = CalculateWithDpi(button.y, highDpi),
            Button = (MouseButton)button.button
        });
    }

    public static bool IsKeyDown(KeyboardKey key)
    {
        var ptr = SDL_GetKeyboardState(out var num);
        var keys = new byte[num];
        Marshal.Copy(ptr, keys, 0, num);

        return keys[(int) key] == 1;
    }

    public static bool IsButtonDown(MouseButton button)
    {
        var state = SDL_GetMouseState(IntPtr.Zero, IntPtr.Zero);

        return SDL_BUTTON(state) == (uint) button;
    }
}