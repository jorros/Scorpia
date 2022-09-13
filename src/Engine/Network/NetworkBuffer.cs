using System;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;

namespace Scorpia.Engine.Network;

public class NetworkBuffer : IDisposable
{
    private readonly NetworkStream _stream;
    private readonly int _readTimeout;
    private readonly MemoryStream _buffer;
    private int _packetSize;

    public NetworkBuffer(NetworkStream stream, int readTimeout = 0)
    {
        _stream = stream;
        _readTimeout = readTimeout;
        _buffer = new MemoryStream();
    }

    public Stream Stream => _buffer;

    public void Write()
    {
        if (_stream is null)
        {
            return;
        }

        _stream.Write((byte) _buffer.Length);
        _buffer.Seek(0, SeekOrigin.Begin);
        _buffer.WriteTo(_stream);
    }

    public void WriteTo(NetworkStream networkStream)
    {
        networkStream.Write((byte) _buffer.Length);
        _buffer.Seek(0, SeekOrigin.Begin);
        _buffer.WriteTo(networkStream);
    }

    public async Task Read(CancellationToken cancellationToken = default)
    {
        var stopwatch = new Stopwatch();

        if (_readTimeout > 0)
        {
            stopwatch.Start();
        }

        while (!cancellationToken.IsCancellationRequested)
        {
            if (stopwatch.ElapsedMilliseconds > _readTimeout)
            {
                return;
            }

            if (_stream is null)
            {
                return;
            }

            if (!_stream.DataAvailable)
            {
                continue;
            }

            if (_buffer.Length == 0)
            {
                _packetSize = _stream.Read<byte>();
            }

            Memory<byte> networkBuffer = new byte[255];
            var data = await _stream.ReadAsync(networkBuffer, cancellationToken);
            _buffer.Write(networkBuffer[..data].Span);

            if (_buffer.Length < _packetSize)
            {
                continue;
            }

            _buffer.Seek(0, SeekOrigin.Begin);
            break;
        }
    }

    public void Flush()
    {
        _buffer.SetLength(0);
    }

    public void Dispose()
    {
        _buffer.Dispose();
    }
}