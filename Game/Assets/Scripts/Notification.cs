using Unity.Netcode;
using UnityEngine;

namespace Scorpia.Assets.Scripts
{
    public class Notification : INetworkSerializable
    {
        public string Title;

        public string Text;

        public string Icon;

        public Vector2Int Position = new Vector2Int(-1, -1);

        public Notification(string title, string text, string icon)
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

        public static Notification Format(Notification @event, Vector2Int? target, params string[] args)
        {
            var ret = new Notification(@event.Title, string.Format(@event.Text, args), @event.Icon)
            {
                Position = target ?? new Vector2Int(-1, -1)
            };

            return ret;
        }

        public static Notification Format(Notification @event, params string[] args)
        {
            return Format(@event, null, args);
        }
    }
}