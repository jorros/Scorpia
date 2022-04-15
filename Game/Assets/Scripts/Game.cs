using System.Collections.Generic;
using Actors;
using UnityEngine;
using World;

public static class Game
{
    public static int CurrentTick => Ticker.current.currentTick?.Value ?? default(int);

    private static readonly Dictionary<string, Player> Players = new();

    private static string _version;

    public static Player GetPlayer(string uid)
    {
        return Players[uid];
    }

    public static void AddPlayer(Player player)
    {
        Players.Add(player.Uid.Value.Value, player);
    }

    public static void RemovePlayer(string uid)
    {
        Players.Remove(uid);
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