using System.Text;
using Scorpia.Assets.Scripts.Server;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Scorpia.Assets.Scripts
{
    public class MainMenuSystem : MonoBehaviour
    {
        [SerializeField]
        private GameObject nameInputObj;
        private TMP_InputField nameInput;

        [SerializeField]
        private TextMeshProUGUI statusText;

        private string otherStatus;

        void Start()
        {
            nameInput = nameInputObj.GetComponent<TMP_InputField>();
            nameInput.text = ScorpiaSettings.PlayerName;

            print(gameObject.name);

            if (Application.isBatchMode)
            {
                StartServer();
            }
            else
            {
                StartClient();
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
                var message = "in progress";
                using FastBufferWriter writer = new FastBufferWriter(message.Length * 4, Allocator.Temp);
                writer.WriteValueSafe(message);
                NetworkManager.Singleton.CustomMessagingManager.SendNamedMessage("ErrorMessage", clientId, writer, NetworkDelivery.Reliable);
            }


            callback(false, null, approve, null, null);
        }

        private void CheckConnectivity()
        {
            if (!NetworkManager.Singleton.IsConnectedClient)
            {
                otherStatus = null;
                OnDisconnect(0);
                //NetworkManager.Singleton.
            }
        }

        private void OnErrorMessage(ulong senderId, FastBufferReader reader)
        {
            reader.ReadValueSafe(out otherStatus);
        }

        public void Quit()
        {
            Application.Quit();
        }

        private void StartServer()
        {
            NetworkManager.Singleton.ConnectionApprovalCallback += ApprovalCheck;
            NetworkManager.Singleton.StartServer();

            // Switch scene when everyone is ready
            //SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }

        private void StartClient()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnect;
            NetworkManager.Singleton.NetworkConfig.ConnectionData = System.Text.Encoding.ASCII.GetBytes(ScorpiaSettings.Uid);

            NetworkManager.Singleton.StartClient();

            NetworkManager.Singleton.CustomMessagingManager.RegisterNamedMessageHandler("ErrorMessage", (senderClientId, reader) => OnErrorMessage(senderClientId, reader));

            InvokeRepeating(nameof(CheckConnectivity), 0, 2);
        }

        private void OnDisconnect(ulong obj)
        {
            if (!string.IsNullOrWhiteSpace(otherStatus))
            {
                statusText.text = $"Server <size=140%><color=#a8982d>{otherStatus}</color></size>";
            }
            else
            {
                statusText.text = "Server <size=140%><color=#8f1501>OFFLINE</color></size>";
            }
        }

        private void OnConnect(ulong obj)
        {
            statusText.text = "Server <size=140%><color=#248d44>ONLINE</color></size>";
        }

        public void OnNameChange()
        {
            ScorpiaSettings.PlayerName = nameInput.text;
        }
    }
}