using System;

namespace Scorpia.Engine.Network;

public class AuthenticationFailedEventArgs : EventArgs
{
    public string Reason { get; set; }
}