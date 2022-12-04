using Scorpian;

namespace Scorpia.Game.Player;

public class ServerPlayerManager
{
    private readonly GameState _gameState;
    private readonly UserDataManager _userDataManager;
    private readonly Dictionary<uint, ServerPlayer> _players = new();

    public ServerPlayerManager(GameState gameState, UserDataManager userDataManager)
    {
        _gameState = gameState;
        _userDataManager = userDataManager;
    }

    public bool Add(string deviceId, string name, uint networkId)
    {
        if (_gameState.Current != GameState.State.Lobby)
        {
            return false;
        }

        if (_players.Values.Any(x => string.Equals(x.Name, name, StringComparison.CurrentCultureIgnoreCase)))
        {
            return false;
        }

        _players[networkId] = new ServerPlayer
        {
            DeviceId = deviceId,
            Name = name,
            NetworkId = networkId
        };

        return true;
    }

    public bool AllReady()
    {
        return _players.Any() && _players.All(x => x.Value.Ready);
    }

    public IEnumerable<ServerPlayer> Players => _players.Values;

    public bool AllLoaded()
    {
        return _players.Any() && _players.All(x => x.Value.LoadingProgress == 100);
    }

    public bool HasAccess(string deviceId)
    {
        return _gameState.Current == GameState.State.Lobby || _players.Any(x => x.Value.DeviceId == deviceId);
    }

    public void Remove(uint networkId)
    {
        _players.Remove(networkId);
    }

    public ServerPlayer? Get(uint networkId)
    {
        return !_players.ContainsKey(networkId) ? null : _players[networkId];
    }

    public string GetDeviceId()
    {
        return _userDataManager.Get("did", Guid.NewGuid().ToString());
    }
}