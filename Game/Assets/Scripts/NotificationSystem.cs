using Unity.Netcode;

public class NotificationSystem : NetworkBehaviour
{
    public static NotificationSystem current;

    private void Awake()
    {
        current = this;
    }

    [ClientRpc]
    public void SendClientRpc(Notification notification, ClientRpcParams clientRpcParams = default)
    {
        EventManager.Trigger(Events.ReceiveNotification, notification);
    }
}