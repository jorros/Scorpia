using System;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Engine.Network;

public class DataReceivedEventArgs : EventArgs
{
    public object Data { get; set; }
    
    public ushort SenderId { get; set; }
}