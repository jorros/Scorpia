using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<ushort, TcpClient> _connectedClients = new();
    private TcpClient _client;
    private TcpListener _listener;

    private ushort _clientCounter = 1;

    public event EventHandler<PacketReceivedEventArgs> OnPacketReceive;
    public event EventHandler<UserConnectedEventArgs> OnUserConnect;
    public event EventHandler<UserDisconnectedEventArgs> OnUserDisconnect;
    public event EventHandler<AuthenticationFailedEventArgs> OnAuthenticationFail;

    public bool IsConnected { get; private set; }

    public NetworkManager(EngineSettings settings, ILogger<NetworkManager> logger, PacketManager packetManager,
        IServiceProvider serviceProvider)
    {
        _settings = settings;
        _logger = logger;
        _packetManager = packetManager;
        _serviceProvider = serviceProvider;
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

                if (_settings.Authentication is not null)
                {
                    _logger.LogDebug("Receive authentication");
                    
                    var stream = client.GetStream();

                    try
                    {
                        stream.ReadTimeout = 2000;

                        var buffer = stream.ReadIntoBuffer();

                        stream.ReadTimeout = Timeout.Infinite;

                        var authString = buffer.ReadString();

                        var response = _settings.Authentication(authString, _serviceProvider);

                        response.Write(stream, _packetManager);

                        if (!response.Succeeded)
                        {
                            _logger.LogDebug("Failed authentication: {Reason}", response.Reason);
                            client.Close();
                            continue;
                        }
                    }
                    catch (Exception e)
                    {
                        _logger.LogDebug("Failed authentication: {Reason}", e.Message);
                        
                        var responsePacket = new LoginResponsePacket
                        {
                            Reason = "NO_AUTH",
                            Succeeded = false
                        };
                        responsePacket.Write(stream, _packetManager);

                        client.Close();
                        continue;
                    }
                }

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
            foreach (var clientStream in _connectedClients.Keys.Select(clientId =>
                         _connectedClients[clientId].GetStream()))
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

    public void Connect(string hostname, int port, string loginRequest = null)
    {
        if (_client is null)
        {
            return;
        }

        _logger.LogInformation("Connecting to server on port {Port}", port);
        try
        {
            _client.Connect(hostname, port);

            if (!_client.Connected)
            {
                return;
            }

            if (loginRequest is not null)
            {
                var stream = _client.GetStream();
                
                stream.Write(loginRequest);

                var buffer = stream.ReadIntoBuffer();

                var response = new LoginResponsePacket();
                response.Read(buffer, _packetManager);

                if (!response.Succeeded)
                {
                    _client.Close();
                    _client = new TcpClient();
                    
                    OnAuthenticationFail?.Invoke(this, new AuthenticationFailedEventArgs
                    {
                        Reason = response.Reason
                    });
                    return;
                }
            }

            OnUserConnect?.Invoke(this, new UserConnectedEventArgs());
            Task.Run(() => ReceiveData(_client, 0));
        }
        catch (SocketException e)
        {
            _logger.LogWarning("Failed to connect to server: {Error}", e.Message);
        }
    }

    internal void UpdateStatus()
    {
        if (IsClient)
        {
            if (_client.Client.LocalEndPoint is not IPEndPoint localEndpoint ||
                _client.Client.RemoteEndPoint is not IPEndPoint remoteEndpoint)
            {
                return;
            }

            var state = TcpStatus.GetState(remoteEndpoint.Port, localEndpoint.Port);

            switch (state)
            {
                case ConnectionStatus.Disconnecting or ConnectionStatus.NotReady when IsConnected:
                    IsConnected = false;
                    _client.Close();
                    _client = new TcpClient();
                    OnUserDisconnect?.Invoke(this, new UserDisconnectedEventArgs
                    {
                        ClientId = 0
                    });
                    break;
                case ConnectionStatus.Connected:
                    IsConnected = true;
                    break;
            }

            return;
        }

        foreach (var client in _connectedClients)
        {
            if (client.Value.Client.LocalEndPoint is not IPEndPoint localEndpoint ||
                client.Value.Client.RemoteEndPoint is not IPEndPoint remoteEndpoint)
            {
                return;
            }

            var state = TcpStatus.GetState(remoteEndpoint.Port, localEndpoint.Port);

            if (state != ConnectionStatus.Disconnecting)
            {
                continue;
            }

            client.Value.Close();
            _connectedClients.Remove(client.Key);
            OnUserDisconnect?.Invoke(this, new UserDisconnectedEventArgs
            {
                ClientId = client.Key
            });
        }
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