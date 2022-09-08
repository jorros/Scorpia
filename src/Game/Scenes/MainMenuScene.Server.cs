using CommunityToolkit.HighPerformance;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Game.Scenes;

public partial class MainMenuScene
{
    [ServerRpc]
    private void Test(TestParam param, ushort senderId)
    {
        Console.WriteLine(param.Param);
        Invoke(nameof(Abc), senderId);
    }
    
    private void ServerOnUserDisconnect(object? sender, UserDisconnectedEventArgs e)
    {
    }
    
    private void ServerOnUserConnect(object? sender, UserConnectedEventArgs e)
    {
    }
}

public struct TestParam : INetworkPacket
{
    public int Param { get; set; }
    
    public void Write(Stream stream, PacketManager packetManager)
    {
        stream.Write(Param);
    }

    public void Read(Stream stream, PacketManager packetManager)
    {
        Param = stream.Read<int>();
    }
}