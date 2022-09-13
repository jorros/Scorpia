using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.Network;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Lobby;
using Scorpia.Game.Player;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class MainMenuScene : NetworkedScene
{
    private PlayerLobby _currentLobby;
    public PlayerManager PlayerManager { get; private set; }

    protected override void OnLoad(AssetManager assetManager)
    {
        PlayerManager = ServiceProvider.GetRequiredService<PlayerManager>();
        
        assetManager.Load("UI");
        ScorpiaStyle.Setup(assetManager);
        SetupUI(assetManager);
        
        _nameInput!.Text = UserDataManager.Get("player_name", string.Empty);

        _currentLobby = new OutsidePlayerLobby(this);
        RefreshButtons();

        _quitButton!.OnClick += QuitButtonOnOnClick;
        _joinButton!.OnClick += JoinButtonOnOnClick;

        NetworkManager.OnUserDisconnect += OnUserDisconnect;
        NetworkManager.OnUserConnect += OnUserConnect;

        NetworkManager.OnAuthenticationFail += OnAuthenticationFail;
    }

    [ClientRpc]
    public void PlayerJoinedClientRpc()
    {
        Logger.LogInformation("Joined server successfully");
        _currentLobby = new NotReadyPlayerLobby(this);
        RefreshButtons();
    }

    [ClientRpc]
    public void LeaveClientRpc(string deviceId)
    {
        var ownId = PlayerManager.GetDeviceId();

        if (deviceId == ownId)
        {
            _currentLobby = new OutsidePlayerLobby(this);
            RefreshButtons();
        }
    }

    [ClientRpc]
    public void PlayerReadyClientRpc(Player.Player player)
    {
        var ownId = PlayerManager.GetDeviceId();

        if (player.DeviceId == ownId)
        {
            _currentLobby = new ReadyPlayerLobby(this);
            RefreshButtons();
        }
        else
        {
            var button = _colourGroup.GetButton(player.Color);
            button.Enabled = false;
        }
        
        _playerList.Attach(new PlayerPreviewUI(_assetManager, player.Name, player.Color!.Value));
    }
    
    [ClientRpc]
    public void PlayerNotReadyClientRpc(Player.Player player)
    {
        var ownId = PlayerManager.GetDeviceId();

        if (player.DeviceId == ownId)
        {
            _currentLobby = new NotReadyPlayerLobby(this);
            RefreshButtons();
        }
        else
        {
            var button = _colourGroup.GetButton(player.Color);
            button.Enabled = true;
        }
        
        _playerList.Remove(x => ((PlayerPreviewUI)x).Name == player.Name);
    }
    
    private void JoinButtonOnOnClick(object sender, MouseButtonEventArgs e)
    {
        _currentLobby.ConfirmAction();
    }

    private void QuitButtonOnOnClick(object sender, MouseButtonEventArgs e)
    {
        _currentLobby.CancelAction();
    }

    private void RefreshButtons()
    {
        _quitButton!.Text = _currentLobby.CancelLabel;
        _joinButton!.Text = _currentLobby.ConfirmLabel;
        
        _colourGroup!.Show = _currentLobby.ShowLobby;
        _divider.Show = _currentLobby.ShowLobby;
        _colorLabel.Show = _currentLobby.ShowLobby;
        _playerList.Show = _currentLobby.ShowLobby;

        _colourGroup.Enabled = _currentLobby.EnableColorSelect;
        _nameInput!.Enabled = _currentLobby.EnableNameInput;
    }

    protected override void OnTick()
    {
        if (NetworkManager.IsServer)
        {
            return;
        }

        var renderContext = ServiceProvider.GetService<RenderContext>();
        if (renderContext is not null)
        {
            _fpsLabel!.Text = renderContext.FPS.ToString();
        }

        if (!NetworkManager.IsConnected)
        {
            NetworkManager.Connect("127.0.0.1", 1992, "1234");
        }
    }

    protected override void OnRender(RenderContext context)
    {
        _layout!.Render(context, false);
    }

    private void OnUserConnect(object? sender, UserConnectedEventArgs e)
    {
        _serverStatus!.Text = "Server <text color='green' size='70'>ONLINE</text>";
    }

    private void OnUserDisconnect(object? sender, UserDisconnectedEventArgs e)
    {
        _serverStatus!.Text = "Server <text color='red' size='70'>OFFLINE</text>";
    }

    private void OnAuthenticationFail(object? sender, AuthenticationFailedEventArgs e)
    {
        _serverStatus!.Text = $"Server <text color='orange' size='70'>IN PROGRESS</text>";
    }
}