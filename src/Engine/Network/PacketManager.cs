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

    private enum Mapping
    {
        Null,
        String,
        Byte,
        Short,
        Ushort,
        Int,
        Uint,
        Long,
        Ulong,
        Float,
        Double,
        Bool,
        Packet
    };

    public PacketManager(EngineSettings settings, ILogger<PacketManager> logger)
    {
        _settings = settings;
        _logger = logger;
    }

    public object Read(Stream stream)
    {
        var firstByte = (Mapping) stream.ReadByte();

        switch (firstByte)
        {
            case Mapping.Null:
                return null;
            case Mapping.Byte:
                return stream.Read<byte>();
            case Mapping.String:
                return stream.ReadString();
            case Mapping.Short:
                return stream.Read<short>();
            case Mapping.Ushort:
                return stream.Read<ushort>();
            case Mapping.Int:
                return stream.Read<int>();
            case Mapping.Uint:
                return stream.Read<uint>();
            case Mapping.Long:
                return stream.Read<long>();
            case Mapping.Ulong:
                return stream.Read<ulong>();
            case Mapping.Float:
                return stream.Read<float>();
            case Mapping.Double:
                return stream.Read<double>();
            case Mapping.Bool:
                return stream.Read<bool>();
            case Mapping.Packet:
            {
                var id = stream.Read<ushort>();

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
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    public void Write<T>(T data, Stream stream)
    {
        switch (data)
        {
            case null:
                stream.WriteByte((byte) Mapping.Null);
                break;
            case byte val:
                stream.WriteByte((byte) Mapping.Byte);
                stream.Write(val);
                break;
            case string val:
                stream.WriteByte((byte) Mapping.String);
                stream.Write(val);
                break;
            case short val:
                stream.WriteByte((byte) Mapping.Short);
                stream.Write(val);
                break;
            case ushort val:
                stream.WriteByte((byte) Mapping.Ushort);
                stream.Write(val);
                break;
            case int val:
                stream.WriteByte((byte) Mapping.Int);
                stream.Write(val);
                break;
            case uint val:
                stream.WriteByte((byte) Mapping.Uint);
                stream.Write(val);
                break;
            case long val:
                stream.WriteByte((byte) Mapping.Long);
                stream.Write(val);
                break;
            case ulong val:
                stream.WriteByte((byte) Mapping.Ulong);
                stream.Write(val);
                break;
            case float val:
                stream.WriteByte((byte) Mapping.Float);
                stream.Write(val);
                break;
            case double val:
                stream.WriteByte((byte) Mapping.Double);
                stream.Write(val);
                break;
            case bool val:
                stream.WriteByte((byte) Mapping.Bool);
                stream.Write(val);
                break;
            case INetworkPacket packet:
            {
                stream.WriteByte((byte) Mapping.Packet);
                
                var hash = packet.GetType().FullName.GetDeterministicHashCode16();

                stream.Write(hash);
                packet.Write(stream, this);

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}