using System;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Engine.Network;

public class PacketReceivedEventArgs : EventArgs
{
    public INetworkPacket Packet { get; set; }
    
    public ushort SenderId { get; set; }
}