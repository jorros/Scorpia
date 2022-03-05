using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Scorpia.Assets.Scripts
{
    public class NotificationSystem : NetworkBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        [ClientRpc]
        public void SendClientRpc(Notification @event, ClientRpcParams clientRpcParams = default)
        {
            print($"{@event.Title}: {@event.Text}");
        }
    }
}