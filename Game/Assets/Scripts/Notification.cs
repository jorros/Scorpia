using System;
using Unity.Netcode;
using UnityEngine;

public class Notification : INetworkSerializable
{
    public string TooltipHeader;

    public string TooltipText;

    public int Icon;

    public int Cover;

    public string Header;

    public string Text;

    public readonly string Id;

    public Vector2Int Position = new Vector2Int(-1, -1);

    public Notification()
    {
        Id = Guid.NewGuid().ToString();
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref Position);
        serializer.SerializeValue(ref TooltipHeader);
        serializer.SerializeValue(ref TooltipText);
        serializer.SerializeValue(ref Icon);
        serializer.SerializeValue(ref Cover);
        serializer.SerializeValue(ref Header);
        serializer.SerializeValue(ref Text);
    }

    public static Notification Format(Notification notification, Vector2Int? target, params object[] args)
    {
        var ret = new Notification
        {
            TooltipHeader = string.Format(notification.TooltipHeader, args), 
            TooltipText = string.Format(notification.TooltipText, args),
            Icon = notification.Icon,
            Cover = notification.Cover,
            Header = string.Format(notification.Header, args), 
            Text = string.Format(notification.Text, args),
            Position = target ?? new Vector2Int(-1, -1)
        };

        return ret;
    }

    public static Notification Format(Notification @event, params object[] args)
    {
        return Format(@event, null, args);
    }
}