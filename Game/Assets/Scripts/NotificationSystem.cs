using Unity.Netcode;

namespace Scorpia.Assets.Scripts
{
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
            EventManager.Trigger(EventManager.ReceiveNotification, notification);
        }
    }
}