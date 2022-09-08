using System;

namespace Scorpia.Engine.Network;

public class UserDisconnectedEventArgs : EventArgs
{
    public ushort ClientId { get; set; }
}