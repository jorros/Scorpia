using Scorpia.Engine;

namespace Scorpia.Game.Player;

public class PlayerManager
{
    private readonly GameState _gameState;
    private readonly UserDataManager _userDataManager;
    private readonly Dictionary<ushort, ServerPlayer> _players = new();

    public PlayerManager(GameState gameState, UserDataManager userDataManager)
    {
        _gameState = gameState;
        _userDataManager = userDataManager;
    }

    public bool Add(string deviceId, string name, ushort networkId)
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

    public bool HasAccess(string deviceId)
    {
        return _gameState.Current == GameState.State.Lobby || _players.Any(x => x.Value.DeviceId == deviceId);
    }

    public void Remove(ushort networkId)
    {
        _players.Remove(networkId);
    }

    public ServerPlayer? Get(ushort networkId)
    {
        return !_players.ContainsKey(networkId) ? null : _players[networkId];
    }

    public string GetDeviceId()
    {
        return _userDataManager.Get("did", Guid.NewGuid().ToString());
    }
}