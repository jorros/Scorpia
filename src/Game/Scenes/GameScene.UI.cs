using System.Drawing;
using Scorpian.Asset;
using Scorpian.UI;

namespace Scorpia.Game.Scenes;

public partial class GameScene
{
    public BasicLayout layout = null!;
    public Window infoWindow = null!;
    private Label _fpsLabel = null!;
    public BasicLayout topContainer = null!;
    public Window mapWindow = null!;
    public HorizontalGridLayout menuButtons = null!;
    public List<Window> notificationWindows = new();
    public List<Button> notificationButtons = new();
    private Label _dateLabel = null!;
    private Label _currentTileDebugLabel = null!;
    private Label _playerLabel = null!;

    private void SetupUI(AssetManager assetManager)
    {
        layout = new BasicLayout();

        ScorpiaStyle.SetupInGame(assetManager);

        infoWindow = new Window
        {
            Anchor = UIAnchor.BottomRight,
            Show = false,
            Position = new Point(-15, -55),
            Type = "info",
            Width = 1120,
            Height = 575
        };
        layout.Attach(infoWindow);
        
        topContainer = new BasicLayout
        {
            Anchor = UIAnchor.TopLeft,
            Background = assetManager.Get<Sprite>("Game:HUD/top_bar"),
            Width = 1220,
            Height = 40
        };
        layout.Attach(topContainer);

        mapWindow = new Window
        {
            Anchor = UIAnchor.TopRight,
            Type = "map",
            Width = 660,
            Height = 500
        };
        layout.Attach(mapWindow);
        
        _playerLabel = new Label
        {
            Type = "map",
            Text = " - "
        };
        mapWindow.AttachTitle(_playerLabel);

        _dateLabel = new Label
        {
            Type = "map",
            Text = DateTime.Now.ToString("yyyy MMMM")
        };
        mapWindow.AttachTitle(_dateLabel);

        menuButtons = new HorizontalGridLayout
        {
            Height = 117,
            Anchor = UIAnchor.TopRight,
            Position = new Point(10, mapWindow.Height + 10),
            SpaceBetween = 15
        };
        layout.Attach(menuButtons);
        
        var formationButton = new Button
        {
            Content = new Image
            {
                Sprite = assetManager.Get<Sprite>("Game:HUD/menu_formation")
            },
            Type = "menu",
        };
        menuButtons.Attach(formationButton);
        
        var diplomacyButton = new Button
        {
            Content = new Image
            {
                Sprite = assetManager.Get<Sprite>("Game:HUD/menu_diplomacy")
            },
            Type = "menu",
        };
        menuButtons.Attach(diplomacyButton);
        
        var researchButton = new Button
        {
            Content = new Image
            {
                Sprite = assetManager.Get<Sprite>("Game:HUD/menu_research")
            },
            Type = "menu",
        };
        menuButtons.Attach(researchButton);
        
        var financeButton = new Button
        {
            Content = new Image
            {
                Sprite = assetManager.Get<Sprite>("Game:HUD/menu_finance")
            },
            Type = "menu",
        };
        menuButtons.Attach(financeButton);

        var settingsButton = new Button
        {
            Content = new Image
            {
                Sprite = assetManager.Get<Sprite>("Game:HUD/menu_settings")
            },
            Type = "menu",
        };
        menuButtons.Attach(settingsButton);

        var debugPanel = new VerticalGridLayout
        {
            Anchor = UIAnchor.BottomLeft,
            Position = new Point(20, 20)
        };
        layout.Attach(debugPanel);
        
        _currentTileDebugLabel = new Label
        {
            Text = "",
            Type = "debug"
        };
        debugPanel.Attach(_currentTileDebugLabel);

        _fpsLabel = new Label
        {
            Text = "0",
            Type = "debug"
        };
        debugPanel.Attach(_fpsLabel);
    }
}