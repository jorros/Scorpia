using System.IO;

namespace Scorpia.Engine.Network.Packets;

public struct SyncSceneRequest : INetworkPacket
{
    public string Scene { get; set; }
    
    public void Write(Stream stream, PacketManager packetManager)
    {
        stream.Write(Scene);
    }

    public void Read(Stream stream, PacketManager packetManager)
    {
        Scene = stream.ReadString();
    }
}