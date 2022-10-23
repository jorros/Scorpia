using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.UI;

namespace Scorpia.Game.Scenes;

public partial class GameScene
{
    private BasicLayout _layout = null!;
    public Window infoWindow = null!;
    private Label _fpsLabel = null!;
    public BasicLayout topContainer = null!;
    public Window mapWindow = null!;
    public HorizontalGridLayout menuButtons;

    private void SetupUI(AssetManager assetManager)
    {
        _layout = new BasicLayout();

        ScorpiaStyle.SetupInGame(assetManager);

        infoWindow = new Window
        {
            Anchor = UIAnchor.BottomRight,
            Show = false,
            Position = new Point(5, -25),
            Type = "info",
            Width = 1120,
            Height = 575
        };
        _layout.Attach(infoWindow);
        
        topContainer = new BasicLayout
        {
            Anchor = UIAnchor.TopLeft,
            Background = assetManager.Get<Sprite>("Game:HUD/top_bar"),
            Width = 1220,
            Height = 59
        };
        _layout.Attach(topContainer);

        mapWindow = new Window
        {
            Anchor = UIAnchor.TopRight,
            Type = "map",
            Width = 660,
            Height = 500
        };
        _layout.Attach(mapWindow);
        
        var playerLabel = new Label
        {
            Type = "map",
            Text = "jorros - "
        };
        mapWindow.AttachTitle(playerLabel);

        var dateLabel = new Label
        {
            Type = "map",
            Text = DateTime.Now.ToString("yyyy MMMM")
        };
        mapWindow.AttachTitle(dateLabel);

        menuButtons = new HorizontalGridLayout
        {
            Height = 117,
            Anchor = UIAnchor.TopRight,
            Position = new Point(10, mapWindow.Height + 10),
            SpaceBetween = 15
        };
        _layout.Attach(menuButtons);
        
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

        _fpsLabel = new Label
        {
            Text = "0",
            Anchor = UIAnchor.BottomLeft,
            Position = new Point(20, 20),
            Type = "debug"
        };
        _layout.Attach(_fpsLabel);
    }
}