using System;

namespace Scorpia.Engine.InputManagement;

public class TextInputEventArgs : EventArgs
{
    public char Character { get; set; }
}