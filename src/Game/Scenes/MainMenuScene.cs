using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.Network;
using Scorpia.Engine.Network.Protocol;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Lobby;
using Scorpia.Game.Player;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class MainMenuScene : NetworkedScene
{
    private PlayerLobby _currentLobby = null!;
    public ServerPlayerManager ServerPlayerManager { get; private set; }
    private ScorpiaSettings _settings = null!;

    private NetworkedList<Player.Player> Players { get; } = new();

    protected override void OnLoad(AssetManager assetManager)
    {
        ServerPlayerManager = ServiceProvider.GetRequiredService<ServerPlayerManager>();
        _settings = ServiceProvider.GetRequiredService<ScorpiaSettings>();

        assetManager.Load("UI");
        ScorpiaStyle.Setup(assetManager);
        SetupUI(assetManager);

        _nameInput.Text = _settings.PlayerName;

        _currentLobby = new OutsidePlayerLobby(this);
        RefreshButtons();

        _leaveButton.OnClick += OnLeaveBtnClick;
        _joinButton.OnClick += OnJoinBtnClick;
        _loginButton.OnClick += OnLoginBtnClick;
        
        _quitButton.OnClick += OnQuitBtnClick;
        _settingsButton.OnClick += OnSettingsBtnClick;

        NetworkManager.OnUserDisconnect += OnUserDisconnect;
        NetworkManager.OnUserConnect += OnUserConnect;

        NetworkManager.OnAuthenticationFail += OnAuthenticationFail;
        
        Players.OnChange += OnPlayerChange;
    }

    private void OnSettingsBtnClick(object sender, MouseButtonEventArgs e)
    {
        
    }

    private void OnQuitBtnClick(object sender, MouseButtonEventArgs e)
    {
        SceneManager.Quit();
    }

    private void OnLoginBtnClick(object sender, MouseButtonEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(_nameInput.Text))
        {
            return;
        }
        
        Game.ScorpiaSettings.PlayerName = _nameInput.Text;
            
        Invoke(nameof(JoinServerRpc), new JoinMatchPacket
        {
            Name = _nameInput.Text,
            DeviceId = Game.ServerPlayerManager.GetDeviceId()
        });
    }

    private void OnPlayerChange(object? sender, ListChangedEventArgs<Player.Player> e)
    {
        if (e.Action == NetworkedListAction.Add)
        {
            _playerList.Attach(new PlayerPreviewUI(_assetManager, e.Value.Name, e.Value.Color!.Value, e.Value.Faction!.Value));
        }
        else if (e.Action == NetworkedListAction.Remove)
        {
            _playerList.Remove(x => ((PlayerPreviewUI) x).Name == e.Value.Name);
        }
    }

    [ClientRpc]
    public void SetLobbyClientRpc(LobbyEnum lobby)
    {
        Logger.LogInformation("Changing room to {Lobby}", lobby.ToString());
        _currentLobby = lobby switch
        {
            LobbyEnum.Outside => new OutsidePlayerLobby(this),
            LobbyEnum.NotReady => new NotReadyPlayerLobby(this),
            LobbyEnum.Ready => new ReadyPlayerLobby(this),
            _ => _currentLobby
        };
        RefreshButtons();
        
        if (Game.AutoConnect)
        {
            Invoke(nameof(ReadyServerRpc), new ReadyPacket {Color = PlayerColor.Blue, Faction = PlayerFaction.FreeCity});
        }
    }

    private void OnJoinBtnClick(object sender, MouseButtonEventArgs e)
    {
        _currentLobby.ConfirmAction();
    }

    private void OnLeaveBtnClick(object sender, MouseButtonEventArgs e)
    {
        _currentLobby.CancelAction();
    }

    private void RefreshButtons()
    {
        _leaveButton.Content = _currentLobby.CancelLabel;
        _joinButton.Content = _currentLobby.ConfirmLabel;

        _lobbyContainer.Show = _currentLobby.ShowLobby;
        _loginWindow.Show = _currentLobby.ShowLogin;

        _factionSelectionContainer.Enabled = _currentLobby.EnablePlayerSettings;
        _colorContainer.Enabled = _currentLobby.EnablePlayerSettings;
    }

    protected override void OnTick()
    {
        if (NetworkManager.IsServer)
        {
            ServerOnTick();
            
            return;
        }

        var renderContext = ServiceProvider.GetService<RenderContext>();
        if (renderContext is not null)
        {
            _fpsLabel.Text = renderContext.FPS.ToString();
        }

        if (!NetworkManager.IsConnected)
        {
            NetworkManager.Connect("127.0.0.1", 1992, _settings.Identifier);
        }
    }

    protected override void OnRender(RenderContext context)
    {
        _layout.Render(context, false);
    }

    private void OnUserConnect(object? sender, UserConnectedEventArgs e)
    {
        _serverStatus.Text = "online";
        _serverStatus.Color = Color.FromArgb(29, 247, 0);

        if (Game.AutoConnect)
        {
            Invoke(nameof(JoinServerRpc), new JoinMatchPacket
            {
                Name = "debug",
                DeviceId = Game.ServerPlayerManager.GetDeviceId()
            });
        }
    }

    private void OnUserDisconnect(object? sender, UserDisconnectedEventArgs e)
    {
        _serverStatus.Text = "offline";
        _serverStatus.Color = Color.FromArgb(247, 41, 0);
    }

    private void OnAuthenticationFail(object? sender, AuthenticationFailedEventArgs e)
    {
        _serverStatus.Text = "in progress";
        _serverStatus.Color = Color.FromArgb(247, 147, 0);
    }
}