using System.Collections.Generic;
using System.Linq;
using Actors;
using PlayerActions;
using TMPro;
using UnityEngine;
using Utils;
using World;

public static class Game
{
    public static int CurrentTick => Ticker.current.currentTick?.Value ?? default(int);

    private static readonly Dictionary<string, Player> Players = new();
    
    private static readonly List<Location> Locations = new();

    private static string _version;
    
    private static readonly DefaultPlayerAction DefaultPlayerAction = new();

    private static IPlayerAction _playerAction = DefaultPlayerAction;

    private static TextMeshProUGUI _playerActionDescription;

    public static IPlayerAction GetPlayerAction()
    {
        return _playerAction;
    }

    public static void SetPlayerAction(IPlayerAction playerAction)
    {
        _playerAction = playerAction;
        
        if (_playerActionDescription == null)
        {
            _playerActionDescription = GameObject.Find("PlayerActionDescription").GetComponent<TextMeshProUGUI>();
        }

        if (_playerAction.Description == null)
        {
            _playerActionDescription.text = "";

            return;
        }
        
        _playerActionDescription.text = _playerAction.Description;
    }

    public static void ResetPlayerAction()
    {
        SetPlayerAction(DefaultPlayerAction);
    }

    public static Player GetPlayer(string uid)
    {
        return Players[uid];
    }

    public static IEnumerable<Player> GetPlayers()
    {
        return Players.Values;
    }

    public static Player GetSelf()
    {
        return GetPlayer(ScorpiaSettings.Uid);
    }

    public static Player GetPlayer(ulong nid)
    {
        return Players.Values.First(x => x.OwnerClientId == nid);
    }

    public static void AddPlayer(Player player)
    {
        Players.Add(player.Uid.ValueAsString(), player);
    }

    public static void RemovePlayer(string uid)
    {
        Players.Remove(uid);
    }

    public static void AddLocation(Location location)
    {
        Locations.Add(location);
    }

    public static void RemoveLocation(Location location)
    {
        Locations.Remove(location);
    }

    public static IReadOnlyList<Location> GetLocations()
    {
        return Locations;
    }

    public static string Version
    {
        get
        {
            if (!string.IsNullOrWhiteSpace(_version))
            {
                return _version;
            }

            var request = Resources.Load<VersionScriptableObject>("Build");
            _version = request.BuildNumber;

            return _version;
        }
    }
}