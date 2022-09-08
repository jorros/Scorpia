using System;

namespace Scorpia.Engine.Network;

public class UserConnectedEventArgs : EventArgs
{
    public ushort ClientId { get; set; }
}