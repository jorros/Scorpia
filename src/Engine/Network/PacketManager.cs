using System;
using System.IO;
using CommunityToolkit.HighPerformance;
using Microsoft.Extensions.Logging;
using Scorpia.Engine.Helper;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Engine.Network;

public class PacketManager
{
    private readonly EngineSettings _settings;
    private readonly ILogger<PacketManager> _logger;

    public PacketManager(EngineSettings settings, ILogger<PacketManager> logger)
    {
        _settings = settings;
        _logger = logger;
    }
    
    public INetworkPacket Read(Stream stream)
    {
        var id = stream.Read<ushort>();

        var isNull = stream.Read<bool>();

        if (isNull)
        {
            return null;
        }

        if (!_settings.NetworkPackets.ContainsKey(id))
        {
            _logger.LogError("Trying to read unknown packet with ID {Id}", id);
            return null;
        }

        var packetType = _settings.NetworkPackets[id];

        var packet = Activator.CreateInstance(packetType) as INetworkPacket;
        packet?.Read(stream, this);

        return packet;
    }

    public void Write<T>(T packet, Stream stream) where T : INetworkPacket
    {
        if (packet is null)
        {
            stream.Write<ushort>(0);
            stream.Write(true);

            return;
        }

        var hash = packet.GetType().FullName.GetDeterministicHashCode16();
        
        stream.Write(hash);
        stream.Write(false);
        packet.Write(stream, this);
    }
}