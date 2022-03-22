using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Scorpia.Assets.Scripts.MainMenu
{
    public partial class LobbySystem
    {
        private GameObject quitButton;
        private GameObject joinButton;
        private GameObject nameInput;
        private bool isInLobby;
        private bool isReady;

        private void StartClient()
        {
            NetworkManager.Singleton.SceneManager.OnLoad += StartLoadingClient;
            NetworkManager.Singleton.OnClientDisconnectCallback += OnDisconnectClient;

            quitButton = GameObject.Find("QuitBtn");
            joinButton = GameObject.Find("JoinBtn");
            nameInput = GameObject.Find("NameInput");
            quitButton.GetComponent<Button>().onClick.AddListener(Quit);
            joinButton.GetComponent<Button>().onClick.AddListener(Join);
            RefreshButtons();

            var toggles = MainMenuSystem.Current.colourToggle.GetComponentsInChildren<Toggle>(true);

            foreach (var toggle in toggles)
            {
                var toggleColour = toggle.GetComponent<ColourToggle>().Colour;

                toggle.onValueChanged.AddListener((val) =>
                {
                    if (val)
                    {
                        ScorpiaSettings.PlayerColour = toggleColour;
                        SetColourServerRpc(toggleColour);
                    }
                });
            }
        }

        private void StartLoadingClient(ulong clientId, string sceneName, LoadSceneMode loadSceneMode, AsyncOperation asyncOperation)
        {
            print("started loading");
        }

        private void Quit()
        {
            if (isInLobby)
            {
                LeaveServerRpc();
                DisableLobbyMenu();

                return;
            }

            Application.Quit();
        }

        private void Join()
        {
            if (isInLobby)
            {
                isReady = !isReady;
                SetStatusServerRpc(isReady);

                RefreshButtons();

                return;
            }

            EnableLobbyMenu();
        }

        private void DisableLobbyMenu()
        {
            isReady = false;
            isInLobby = false;
            nameInput.GetComponent<TMP_InputField>().interactable = true;
            MainMenuSystem.Current.lobbyMenu.SetActive(false);

            RefreshButtons();
        }

        private void EnableLobbyMenu()
        {
            nameInput.GetComponent<TMP_InputField>().interactable = false;
            MainMenuSystem.Current.lobbyMenu.SetActive(true);

            foreach (var toggle in MainMenuSystem.Current.colourToggle.GetComponentsInChildren<Toggle>())
            {
                var toggleColour = toggle.GetComponent<ColourToggle>().Colour;

                if (toggleColour == ScorpiaSettings.PlayerColour)
                {
                    toggle.isOn = true;
                }
            }

            JoinServerRpc(ScorpiaSettings.PlayerName, ScorpiaSettings.PlayerColour);
            isInLobby = true;

            RefreshButtons();
        }

        private void RefreshButtons()
        {
            var quitText = isInLobby ? "Leave" : "Quit";
            quitButton.GetComponentInChildren<TextMeshProUGUI>().text = quitText;

            var joinText = "Join";

            if (isInLobby)
            {
                if (isReady)
                {
                    joinText = "Not rotob";

                    foreach (var toggle in MainMenuSystem.Current.colourToggle.GetComponentsInChildren<Toggle>())
                    {
                        toggle.enabled = false;
                    }
                }
                else
                {
                    joinText = "Rotob";

                    foreach (var toggle in MainMenuSystem.Current.colourToggle.GetComponentsInChildren<Toggle>())
                    {
                        toggle.enabled = true;
                    }
                }
            }

            joinButton.GetComponentInChildren<TextMeshProUGUI>().text = joinText;
        }

        private void OnDisconnectClient(ulong senderId)
        {
            DisableLobbyMenu();
        }

        [ClientRpc]
        private void UpdatePlayerInfoClientRpc(PlayerInfo playerInfo, ClientRpcParams clientRpcParams = default)
        {
            // var players = playerInfo.Where(x => x.ID != NetworkManager.ServerClientId);
            players = playerInfo;

            var playerPreviews = MainMenuSystem.Current.playerListMenu.GetComponentsInChildren<RectTransform>().Where(x => x.name == "PlayerPreview(Clone)");

            foreach (var preview in playerPreviews)
            {
                Destroy(preview.gameObject);
            }

            foreach (var player in players)
            {
                var preview = Instantiate(MainMenuSystem.Current.playerPreviewPrefab, MainMenuSystem.Current.playerListMenu.transform);
                preview.GetComponentInChildren<Image>().sprite = MainMenuSystem.Current.playerColourSprites[(int)player.Colour];
                preview.GetComponentInChildren<TextMeshProUGUI>().text = player.Name;
            }
        }
    }
}