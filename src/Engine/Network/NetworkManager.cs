using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;
using Microsoft.Extensions.Logging;
using Scorpia.Engine.Helper;
using Scorpia.Engine.Network.Packets;

namespace Scorpia.Engine.Network;

public class NetworkManager
{
    private readonly EngineSettings _settings;
    private readonly ILogger<NetworkManager> _logger;
    private readonly PacketManager _packetManager;
    private readonly Dictionary<ushort, TcpClient> _connectedClients = new();
    private TcpClient _client;
    private TcpListener _listener;

    private ushort _clientCounter = 1;

    public event EventHandler<PacketReceivedEventArgs> OnPacketReceive;
    public event EventHandler<UserConnectedEventArgs> OnUserConnect;

    public NetworkManager(EngineSettings settings, ILogger<NetworkManager> logger, PacketManager packetManager)
    {
        _settings = settings;
        _logger = logger;
        _packetManager = packetManager;
    }

    internal void Start()
    {
        if (_settings.NetworkMode == NetworkMode.Server)
        {
            try
            {
                var endPoint = new IPEndPoint(IPAddress.Any, _settings.Port);
                _listener = new TcpListener(endPoint);

                try
                {
                    Task.Run(RunServer);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
            catch (SocketException e)
            {
                _logger.LogCritical("Failed to start server: {Error}", e.Message);
                _listener.Stop();
                throw;
            }

            return;
        }

        _client = new TcpClient();
    }

    private void RunServer()
    {
        try
        {
            _logger.LogInformation("Start server listening on {Address}:{Port}", IPAddress.Any.ToString(),
                _settings.Port);

            _listener?.Start();

            while (_listener is not null)
            {
                var client = _listener.AcceptTcpClient();
                _logger.LogDebug("Incoming connection");
                var clientId = _clientCounter;
                _clientCounter++;

                var endpoint = client.Client.RemoteEndPoint as IPEndPoint;
                _logger.LogInformation("[{ClientId}] New client connected from {IpAddress}", clientId,
                    endpoint?.Address);

                OnUserConnect?.Invoke(this, new UserConnectedEventArgs
                {
                    ClientId = clientId
                });

                _connectedClients.Add(clientId, client);
                Task.Run(() => ReceiveData(client, clientId));
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private void ReceiveData(TcpClient client, ushort clientId)
    {
        var stream = client.GetStream();
        using var buffer = new MemoryStream();
        short bufferSize = 0;
        Span<byte> networkBuffer = stackalloc byte[255];
        
        while (client.Connected)
        {
            if (!stream.DataAvailable)
            {
                continue;
            }

            if (buffer.Length == 0)
            {
                bufferSize = stream.Read<byte>();
            }

            networkBuffer.Clear();
            var data = stream.Read(networkBuffer);
            buffer.Write(networkBuffer[..data]);

            if (buffer.Length == bufferSize)
            {
                buffer.Seek(0, SeekOrigin.Begin);
                _logger.LogDebug("[{ClientId}] Receiving data from remote endpoint", clientId);

                var packet = _packetManager.Read(buffer);
                
                buffer.SetLength(0);

                OnPacketReceive?.Invoke(this, new PacketReceivedEventArgs
                {
                    Packet = packet,
                    SenderId = clientId
                });
            }
        }
    }

    public void Send<T>(T packet, ushort client = 0) where T : INetworkPacket
    {
        using var buffer = new MemoryStream();
        _packetManager.Write(packet, buffer);

        NetworkStream stream;

        if (IsClient)
        {
            stream = _client.GetStream();
            WriteToStream(stream);

            return;
        }

        if (client == 0)
        {
            foreach (var clientStream in _connectedClients.Keys.Select(clientId => _connectedClients[clientId].GetStream()))
            {
                WriteToStream(clientStream);
            }

            return;
        }

        stream = _connectedClients[client].GetStream();
        WriteToStream(stream);

        void WriteToStream(Stream networkStream)
        {
            networkStream.Write((byte) buffer.Length);
            buffer.Seek(0, SeekOrigin.Begin);
            buffer.WriteTo(networkStream);
        }
    }

    public void Connect(string hostname, int port)
    {
        if (_client is null)
        {
            return;
        }

        _logger.LogInformation("Connecting to server on port {Port}", port);
        try
        {
            _client.Connect(hostname, port);

            if (_client.Connected)
            {
                Task.Run(() => ReceiveData(_client, 0));
            }
        }
        catch (SocketException e)
        {
            _logger.LogError("Failed to connect to server: {Error}", e.Message);
        }
    }

    public bool IsConnected()
    {
        if (_client is null)
        {
            return false;
        }

        return GetStatus() == ConnectionStatus.Connected;
    }

    public ConnectionStatus GetStatus()
    {
        if (_client.Client.LocalEndPoint is IPEndPoint localEndpoint)
        {
            return TcpStatus.GetState(1992, localEndpoint.Port);
        }

        return ConnectionStatus.NotInitialized;
    }

    public bool IsClient => _client is not null;

    public bool IsServer => _listener is not null;

    internal void Stop()
    {
        if (_settings.NetworkMode == NetworkMode.Server)
        {
            _logger.LogInformation("Stop listener");
            _listener.Stop();
            _listener = null;

            return;
        }

        _logger.LogInformation("Close remote connection");
        _client.Close();
        _client = null;
    }
}