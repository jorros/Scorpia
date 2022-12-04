using Scorpia.Game.Nodes;
using Scorpia.Game.Player.Actions;
using Scorpian.Network;

namespace Scorpia.Game.Player;

public class CurrentPlayer
{
    private readonly NetworkManager _networkManager;
    private IPlayerAction _currentAction;
    
    private readonly Dictionary<uint, PlayerNode> _players = new();

    public CurrentPlayer(NetworkManager networkManager)
    {
        _networkManager = networkManager;

        _currentAction = new DefaultPlayerAction();
    }

    public IPlayerAction CurrentAction
    {
        get => _currentAction;
        set
        {
            _currentAction = value;

            // _playerActionDescription.text = _playerAction.Description;
        }
    }

    public PlayerNode? GetSelf()
    {
        return GetPlayer(_networkManager.ClientId);
    }

    public PlayerNode? GetPlayer(uint uid)
    {
        return !_players.ContainsKey(uid) ? null : _players[uid];
    }
    
    public void AddPlayer(PlayerNode player)
    {
        _players.Add(player.Uid.Value, player);
    }

    public void RemovePlayer(uint uid)
    {
        _players.Remove(uid);
    }
}