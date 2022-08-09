using System;

namespace Scorpia.Engine.InputManagement;

public class KeyboardEventArgs : EventArgs
{
    public KeyboardEventType Type { get; init; }
    
    public bool Repeated { get; init; }
    
    public KeyboardKey Key { get; init; }
}

public enum KeyboardEventType
{
    KeyDown,
    KeyUp
}