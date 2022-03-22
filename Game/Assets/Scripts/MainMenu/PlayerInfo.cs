using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

namespace Scorpia.Assets.Scripts.MainMenu
{
    public class PlayerInfo : INetworkSerializable, IEnumerable<PlayerInfo.PlayerDetail>
    {
        public class PlayerDetail : INetworkSerializable
        {
            public string Name;
            public PlayerColour Colour;
            public bool IsReady;
            public bool FinishedLoading;
            public ulong ID;

            public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
            {
                serializer.SerializeValue(ref Name);
                serializer.SerializeValue(ref Colour);
                serializer.SerializeValue(ref IsReady);
                serializer.SerializeValue(ref FinishedLoading);
            }
        }

        public PlayerInfo()
        {
            players = new PlayerDetail[0];
        }

        public PlayerDetail this[ulong id] => players.First(x => x.ID == id);

        public delegate void OnUpdateHandler(PlayerInfo playerInfo);

        public event OnUpdateHandler OnUpdate;

        public void Add(ulong id, PlayerDetail player)
        {
            var lastIndex = players.Length;
            Array.Resize(ref players, lastIndex + 1);
            player.ID = id;
            players[lastIndex] = player;

            OnUpdate?.Invoke(this);
        }

        public void Remove(ulong id)
        {
            players = players.Where(x => x.ID != id).ToArray();

            OnUpdate?.Invoke(this);
        }

        public void SetColour(ulong id, PlayerColour colour)
        {
            this[id].Colour = colour;

            OnUpdate?.Invoke(this);
        }

        public void SetReady(ulong id, bool ready)
        {
            this[id].IsReady = ready;
        }

        public bool HasDistinctColours()
        {
            var colours = new List<PlayerColour>();

            foreach(var player in this)
            {
                if(colours.Contains(player.Colour))
                {
                    return false;
                }
                colours.Add(player.Colour);
            }

            return true;
        }

        public PlayerDetail Get(ulong id) => this[id];

        public PlayerColour GetFreeColour()
        {
            var length = Enum.GetNames(typeof(PlayerColour)).Length;
            for(var i = 0; i < length; i++)
            {
                if(!players.Any(x => (int)x.Colour == i))
                {
                    return (PlayerColour)i;
                }
            }

            return PlayerColour.Blue;
        }

        public bool Exists(ulong id) => players.Any(x => x.ID == id);

        public bool AreReady => players.All(x => x.IsReady);

        private PlayerDetail[] players;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            var length = 0;
            if (!serializer.IsReader)
            {
                length = players.Length;
            }

            serializer.SerializeValue(ref length);

            if (serializer.IsReader)
            {
                players = new PlayerDetail[length];
            }

            for (int n = 0; n < length; ++n)
            {
                players[n] ??= new PlayerDetail();

                players[n].NetworkSerialize(serializer);
            }
        }

        public IEnumerator<PlayerDetail> GetEnumerator() => players.Cast<PlayerDetail>().GetEnumerator();
        
        IEnumerator IEnumerable.GetEnumerator() => players.GetEnumerator();
    }
}