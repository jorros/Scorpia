using System.Drawing;
using Scorpia.Game.Player;
using Scorpian.Asset;
using Scorpian.UI;

namespace Scorpia.Game.Scenes;

public partial class MainMenuScene
{
    private BasicLayout _layout = null!;
    private TextInput _nameInput = null!;
    private Button _loginButton = null!;
    
    private Label _fpsLabel = null!;
    private Label _serverStatus = null!;
    
    public RadioGroup colorGroup = null!;
    private Button _leaveButton = null!;
    private Button _joinButton = null!;

    private AssetManager _assetManager = null!;
    private Window _loginWindow = null!;
    public RadioGroup factionSelection = null!;
    private HorizontalGridLayout _lobbyContainer = null!;
    private VerticalGridLayout _playerList = null!;
    private HorizontalGridLayout _factionSelectionContainer = null!;
    private GridLayout _colorContainer = null!;
    private Button _quitButton = null!;
    private Button _settingsButton = null!;

    private void SetupUI(AssetManager assetManager)
    {
        _assetManager = assetManager;
        
        _layout = new BasicLayout
        {
            Background = assetManager.Get<Sprite>("UI:menu_background")
        };

        var versionLabel = new Label
        {
            Text = "playtest20221019",
            Anchor = UIAnchor.BottomRight,
            Type = "debug",
            Position = new Point(20, 20),
        };
        _layout.Attach(versionLabel);
        
        _fpsLabel = new Label
        {
            Text = "0",
            Anchor = UIAnchor.TopLeft,
            Type = "debug",
            Position = new Point(20, 20)
        };
        _layout.Attach(_fpsLabel);
        
        var statusTextContainer = new HorizontalGridLayout
        {
            Anchor = UIAnchor.TopRight,
            Position = new Point(20, 20),
            Height = 60
        };
        _layout.Attach(statusTextContainer);

        var statusText = new Label
        {
            Type = "corner",
            Text = "Server ",
        };
        statusTextContainer.Attach(statusText);
        
        _serverStatus = new Label
        {
            Text = "unknown",
            Color = Color.FromArgb(154, 154, 154),
            Type = "corner",
            Font = "Medium"
        };
        statusTextContainer.Attach(_serverStatus);
        
        

        _loginWindow = new Window
        {
            Anchor = UIAnchor.Center,
            Width = 680,
            Height = 480
        };
        _layout.Attach(_loginWindow);
        
        var loginLabel = new Label
        {
            Position = new Point(0, 0),
            Type = "header",
            Text = "LOGIN"
        };
        _loginWindow.Attach(loginLabel);
        
        _nameInput = new TextInput
        {
            Position = new Point(0, 140)
        };
        _loginWindow.Attach(_nameInput);

        _loginButton = new Button
        {
            Content = "LOG IN",
            Type = "action_login",
            Position = new Point(0, 280)
        };
        _loginWindow.Attach(_loginButton);

        _loginWindow.Show = false;


        _lobbyContainer = new HorizontalGridLayout
        {
            Anchor = UIAnchor.Center,
            SpaceBetween = 30,
            Height = 900
        };
        _layout.Attach(_lobbyContainer);

        var playerSettingsWindow = new Window
        {
            Type = "full",
            Width = 1200,
            Height = 1050
        };
        playerSettingsWindow.AttachTitle("Lobby");
        _lobbyContainer.Attach(playerSettingsWindow);
        
        _leaveButton = new Button
        {
            Position = new Point(0, 0),
            Content = "LEAVE",
            Type = "action_red"
        };
        playerSettingsWindow.ActionBar.Attach(_leaveButton);
        
        _joinButton = new Button
        {
            Position = new Point(0, 0),
            Content = "JOIN",
            Type = "action_green"
        };
        playerSettingsWindow.ActionBar.Attach(_joinButton);

        var factionSelectionLabel = new Label
        {
            Type = "form",
            Text = "Choose your faction:",
            Position = new Point(0, 0)
        };
        playerSettingsWindow.Attach(factionSelectionLabel);

        factionSelection = new RadioGroup();

        _factionSelectionContainer = new HorizontalGridLayout
        {
            Position = new Point(0, 60),
            SpaceBetween = 40,
            Height = 164
        };
        playerSettingsWindow.Attach(_factionSelectionContainer);

        var freeCityFaction = new RadioButton
        {
            Content = new Image
            {
                Sprite = assetManager.Get<Sprite>("UI:player_faction_freecity"),
                Width = 164,
                Height = 164,
                Anchor = UIAnchor.Center
            },
            Type = "empty",
            Value = PlayerFaction.FreeCity
        };
        factionSelection.Attach(freeCityFaction);
        _factionSelectionContainer.Attach(freeCityFaction);
        
        var imperialFaction = new RadioButton
        {
            Content = new Image
            {
                Sprite = assetManager.Get<Sprite>("UI:player_faction_dragonlord"),
                Width = 164,
                Height = 164,
                Anchor = UIAnchor.Center
            },
            Type = "empty",
            Value = PlayerFaction.Imperial
        };
        factionSelection.Attach(imperialFaction);
        _factionSelectionContainer.Attach(imperialFaction);

        var divider = new HorizontalDivider
        {
            Position = new Point(0, 250),
            Width = 1120
        };
        playerSettingsWindow.Attach(divider);
        
        var colorLabel = new Label
        {
            Position = new Point(0, 280),
            Type = "form",
            Text = "Choose your colour:"
        };
        playerSettingsWindow.Attach(colorLabel);
        
        colorGroup = new RadioGroup();

        _colorContainer = new GridLayout
        {
            GridSize = new Size(5, 2),
            Position = new Point(0, 330),
            Width = 1120,
            Height = 400
        };
        playerSettingsWindow.Attach(_colorContainer);

        var colors = Enum.GetValues<PlayerColor>();
        
        for (var i = 0; i < colors.Length; i++)
        {
            var color = colors[i];
            var radioButton = new RadioButton
            {
                Type = "empty",
                Value = color,
                Content = new Image
                {
                    Sprite = assetManager.Get<Sprite>($"UI:player_icon_{color.ToString().ToLower()}"),
                    Width = 160,
                    Height = 160,
                    Anchor = UIAnchor.Center
                }
            };
            _colorContainer.Attach(radioButton);
            colorGroup.Attach(radioButton);
        }

        var playerListWindow = new Window
        {
            Type = "light",
            Width = 600,
            Height = 960
        };
        playerListWindow.AttachTitle("Players");
        _lobbyContainer.Attach(playerListWindow);

        _playerList = new VerticalGridLayout
        {
            SpaceBetween = 15,
            Width = 560
        };
        playerListWindow.Attach(_playerList);



        _quitButton = new Button
        {
            Anchor = UIAnchor.BottomLeft,
            Position = new Point(20, 20),
            Content = new Image
            {
                Sprite = assetManager.Get<Sprite>("UI:button_exit")
            },
            Type = "corner",
        };
        _layout.Attach(_quitButton);
        
        _settingsButton = new Button
        {
            Anchor = UIAnchor.BottomLeft,
            Position = new Point(150, 20),
            Content = new Image
            {
                Sprite = assetManager.Get<Sprite>("UI:button_settings")
            },
            Type = "corner"
        };
        _layout.Attach(_settingsButton);
    }
}