using System.Collections.Generic;
using System.Linq;
using Scorpia.Assets.Scripts.Server;
using Scorpia.Assets.Scripts.World;
using Unity.Netcode;
using UnityEngine;

namespace Scorpia.Assets.Scripts.Server
{
    public class ScorpiaServer
    {
        public Dictionary<string, ulong> PlayerMap { get; private set; }
        public GameState State { get; set; }

        private Dictionary<string, List<GameObject>> playerObjects;

        private static ScorpiaServer _singleton;

        public IReadOnlyList<GameObject> GetPlayerObjects(string uid)
        {
            if (!playerObjects.ContainsKey(uid))
            {
                return new List<GameObject>();
            }

            return playerObjects[uid];
        }

        public string FindUid(ulong clientId)
        {
            return PlayerMap.FirstOrDefault(x => x.Value == clientId).Key;
        }

        public void AddPlayerObject(string uid, GameObject obj)
        {
            if (!playerObjects.ContainsKey(uid))
            {
                playerObjects.Add(uid, new List<GameObject>());
            }

            playerObjects[uid].Add(obj);
        }

        public void SendNotification(Notification @event, IReadOnlyList<ulong> clients = null)
        {
            Game.NotificationSystem.SendClientRpc(@event, new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = clients ?? NetworkManager.Singleton.ConnectedClientsIds
                }
            });
        }

        public ScorpiaServer()
        {
            PlayerMap = new Dictionary<string, ulong>();
            playerObjects = new Dictionary<string, List<GameObject>>();
            State = GameState.Lobby;
        }

        public static ScorpiaServer Singleton
        {
            get
            {
                if (_singleton == null)
                {
                    _singleton = new ScorpiaServer();
                }

                return _singleton;
            }
        }
    }
}