using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.Netcode;

namespace Scorpia.Assets.Scripts.Actors
{
    public class Player : NetworkBehaviour
    {
        public NetworkVariable<int> Doubloons { get; set; } = new NetworkVariable<int>();

        public NetworkVariable<FixedString64Bytes> PlayerName { get; set; } = new NetworkVariable<FixedString64Bytes>();

        public NetworkVariable<FixedString64Bytes> Uid { get; set; } = new NetworkVariable<FixedString64Bytes>();

        public override void OnNetworkSpawn()
        {
            
        }
    }
}