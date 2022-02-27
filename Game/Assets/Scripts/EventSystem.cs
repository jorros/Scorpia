using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

namespace Scorpia.Assets.Scripts
{
    public class EventSystem : NetworkBehaviour
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
        public void SendClientRpc(Event @event, ClientRpcParams clientRpcParams = default)
        {
            print($"{@event.Title}: {@event.Text}");
        }
    }
}