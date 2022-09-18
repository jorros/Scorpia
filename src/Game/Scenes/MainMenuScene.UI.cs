using System.Drawing;
using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.UI;
using Scorpia.Game.Lobby;
using Scorpia.Game.Player;

namespace Scorpia.Game.Scenes;

public partial class MainMenuScene
{
    private BasicLayout? _layout;
    private Label? _fpsLabel;
    public TextInput? _nameInput;
    public RadioGroup? _colourGroup;
    private Button? _quitButton;
    private Button? _joinButton;
    private Button? _settingsButton;
    private Label? _serverStatus;
    private HorizontalDivider _divider;
    private Label _colorLabel;
    private HorizontalGridLayout _playerList;

    private AssetManager _assetManager;

    private void SetupUI(AssetManager assetManager)
    {
        _assetManager = assetManager;
        
        _layout = new BasicLayout(ScorpiaStyle.Stylesheet)
        {
            Background = assetManager.Get<Sprite>("UI:background")
        };

        var versionLabel = new Label
        {
            Text = "<outline color=\"#000\" size=\"2\">Version 0.1 Pre-Alpha</outline>",
            Anchor = UIAnchor.BottomRight,
            Color = Color.White,
            Size = 42,
            Position = new Point(20, 20)
        };
        _layout.Attach(versionLabel);
        
        _fpsLabel = new Label
        {
            Text = "0",
            Anchor = UIAnchor.TopLeft,
            Color = Color.White,
            Size = 36,
            Position = new Point(20, 20)
        };
        _layout.Attach(_fpsLabel);

        var window = new Window
        {
            Anchor = UIAnchor.Center
        };
        _layout.Attach(window);
        
        var nameLabel = new Label
        {
            Position = new Point(0, 40),
            Type = "Form",
            Text = "Your name:"
        };
        window.Attach(nameLabel);
        
        _nameInput = new TextInput
        {
            Position = new Point(180, 10)
        };
        window.Attach(_nameInput);

        _serverStatus = new Label
        {
            Position = new Point(0, 20),
            Type = "Header",
            Text = "Server <text color='red' size='70'>OFFLINE</text>",
            Anchor = UIAnchor.TopRight
        };
        window.Attach(_serverStatus);
        
        _colorLabel = new Label
        {
            Position = new Point(0, 140),
            Type = "Form",
            Text = "Your colour:"
        };
        window.Attach(_colorLabel);

        _colourGroup = new RadioGroup();
        window.Attach(_colourGroup);
        
        var colours = Enum.GetValues<PlayerColor>();

        for (var i = 0; i < colours.Length; i++)
        {
            var colour = colours[i];
            var radioButton = new RadioButton
            {
                Type = "content",
                Value = colour,
                Position = new Point(-20 + 240 * i, 180),
                Content = new Image
                {
                    Sprite = assetManager.Get<Sprite>($"UI:player_icon_{colour.ToString().ToLower()}"),
                    Width = 150,
                    Height = 150,
                    Anchor = UIAnchor.Center
                }
            };
            _colourGroup.Attach(radioButton);
        }

        _divider = new HorizontalDivider
        {
            Position = new Point(0, 450),
            Width = 1900,
            Show = false
        };
        window.Attach(_divider);

        _playerList = new HorizontalGridLayout
        {
            Position = new Point(0, 500),
            MinWidth = 1900,
            SpaceBetween = 80,
            Margin = new Point(20, 20)
        };
        _playerList.SetHeight(400);
        window.Attach(_playerList);

        _quitButton = new Button
        {
            Position = new Point(0, 0),
            Text = "QUIT",
            Type = "action_red"
        };
        window.AttachAction(_quitButton);

        _joinButton = new Button
        {
            Position = new Point(0, 0),
            Text = "JOIN",
            Type = "action_green"
        };
        window.AttachAction(_joinButton);

        _settingsButton = new Button
        {
            Position = new Point(0, 0),
            Text = "SETTINGS",
            Type = "action_regular"
        };
        window.AttachAction(_settingsButton);
    }
}