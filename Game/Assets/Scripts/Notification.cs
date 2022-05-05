using System;
using Unity.Netcode;

public class Notification : INetworkSerializable
{
    public string TooltipHeader;

    public string TooltipText;

    public int Icon;

    public int Cover;

    public string Header;

    public string Text;

    public readonly string Id;

    public Notification()
    {
        Id = Guid.NewGuid().ToString();
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref TooltipHeader);
        serializer.SerializeValue(ref TooltipText);
        serializer.SerializeValue(ref Icon);
        serializer.SerializeValue(ref Cover);
        serializer.SerializeValue(ref Header);
        serializer.SerializeValue(ref Text);
    }

    public static Notification Format(Notification notification, params object[] args)
    {
        var ret = new Notification
        {
            TooltipHeader = string.Format(notification.TooltipHeader, args), 
            TooltipText = string.Format(notification.TooltipText, args),
            Icon = notification.Icon,
            Cover = notification.Cover,
            Header = string.Format(notification.Header, args), 
            Text = string.Format(notification.Text, args),
        };

        return ret;
    }
}