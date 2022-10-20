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

    private void SetupUI(AssetManager assetManager)
    {
        _layout = new BasicLayout(ScorpiaStyle.Stylesheet);

        ScorpiaStyle.SetupInGame(assetManager);

        infoWindow = new Window
        {
            Anchor = UIAnchor.BottomRight,
            Show = false,
            Position = new Point(20, 20),
            Type = "info"
        };
        infoWindow.SetSize(1120, 575);
        _layout.Attach(infoWindow);
        
        topContainer = new BasicLayout
        {
            Anchor = UIAnchor.TopLeft,
            Background = assetManager.Get<Sprite>("Game:HUD/top_bar")
        };
        topContainer.SetSize(1220, 59);
        _layout.Attach(topContainer);
        
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