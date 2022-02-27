using Unity.Netcode;
using UnityEngine;

namespace Scorpia.Assets.Scripts
{
    public class Event : INetworkSerializable
    {
        public string Title;

        public string Text;

        public string Icon;

        public Vector2Int Position = new Vector2Int(-1, -1);

        public Event(string title, string text, string icon)
        {
            Title = title;
            Text = text;
            Icon = icon;
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref Position);
            serializer.SerializeValue(ref Title);
            serializer.SerializeValue(ref Text);
            serializer.SerializeValue(ref Icon);
        }

        public static Event Format(Event @event, Vector2Int? target, params string[] args)
        {
            var ret = new Event(@event.Title, string.Format(@event.Text, args), @event.Icon)
            {
                Position = target ?? new Vector2Int(-1, -1)
            };

            return ret;
        }

        public static Event Format(Event @event, params string[] args)
        {
            return Format(@event, null, args);
        }
    }
}