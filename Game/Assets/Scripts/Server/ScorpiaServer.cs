using System.Collections.Generic;
using Actors;
using MainMenu;
using Unity.Netcode;

namespace Server
{
    public class ScorpiaServer
    {
        public GameState State { get; set; }
        public PlayerInfo Players { get; set; }
        private static ScorpiaServer _singleton;
        
        public void SendNotification(Notification @event, IReadOnlyList<ulong> clients = null)
        {
            NotificationSystem.current.SendClientRpc(@event, new ClientRpcParams
            {
                Send = new ClientRpcSendParams
                {
                    TargetClientIds = clients ?? NetworkManager.Singleton.ConnectedClientsIds
                }
            });
        }

        private ScorpiaServer()
        {
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