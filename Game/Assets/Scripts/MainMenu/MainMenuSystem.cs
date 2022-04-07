using System.Text;
using Server;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace MainMenu
{
    public class MainMenuSystem : MonoBehaviour
    {
        [SerializeField]
        private TMP_InputField nameInput;

        [SerializeField]
        private TextMeshProUGUI statusText;

        [SerializeField]
        private GameObject lobby;

        public GameObject lobbyMenu;

        public GameObject playerListMenu;

        private string otherStatus;

        public static MainMenuSystem Current;

        public ToggleGroup colourToggle;

        public GameObject playerPreviewPrefab;

        public Sprite[] playerColourSprites;

        private void Awake()
        {
            Current = this;
        }

        private void Start()
        {
            nameInput.text = ScorpiaSettings.PlayerName;

            if (Application.isBatchMode || ParrelSyncHelper.IsClone())
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

            if (ScorpiaServer.Singleton.State == GameState.Lobby)
            {
                approve = true;
            }
            else if (ScorpiaServer.Singleton.Players.FindByUID(uid) != null)
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

                NetworkManager.Singleton.StartClient();
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

            var instance = Instantiate(lobby);
            instance.GetComponent<NetworkObject>().Spawn();

            Debug.Log("Starting server");
        }

        private void StartClient()
        {
            NetworkManager.Singleton.OnClientConnectedCallback += OnConnect;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnect;
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.ASCII.GetBytes(ScorpiaSettings.Uid);

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