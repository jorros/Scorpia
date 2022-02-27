using System.Text;
using Scorpia.Assets.Scripts.Server;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scorpia.Assets.Scripts
{
    public class Network : MonoBehaviour
    {
        [SerializeField]
        private GameObject nameInputObj;
        private TMP_InputField nameInput;

        void Awake()
        {
            nameInput = nameInputObj.GetComponent<TMP_InputField>();
            nameInput.text = ScorpiaSettings.PlayerName;

            if (Application.isBatchMode)
            {
                Create();
            }
        }

        private void ApprovalCheck(byte[] connectionData, ulong clientId, NetworkManager.ConnectionApprovedDelegate callback)
        {
            var approve = false;
            var uid = Encoding.ASCII.GetString(connectionData);

            if (ScorpiaServer.Singleton.State == Server.GameState.Lobby)
            {
                approve = true;
            }
            else if (ScorpiaServer.Singleton.PlayerMap.ContainsKey(uid))
            {
                approve = true;
            }
            else
            {
                var message = "Game is already in progress.";
                using FastBufferWriter writer = new FastBufferWriter(message.Length * 4, Allocator.Temp);
                writer.WriteValueSafe(message);
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ErrorMessage", clientId, writer, NetworkDelivery.Reliable);
            }


            callback(false, null, approve, null, null);
        }

        private void OnErrorMessage(ulong senderId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out string message);
            print(message);
        }

        public void Join()
        {
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(ScorpiaSettings.Uid);
            NetworkManager.Singleton.StartClient();
            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ErrorMessage", (senderClientId, reader) => OnErrorMessage(senderClientId, reader));
        }

        public void Create()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartServer();

            // Switch scene when everyone is ready
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }

        public void OnNameChange()
        {
            ScorpiaSettings.PlayerName = nameInput.text;
        }
    }
}