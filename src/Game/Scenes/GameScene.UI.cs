using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.UI;

namespace Scorpia.Game.Scenes;

public partial class GameScene
{
    private BasicLayout _layout = null!;
    private BasicLayout _infoContainer = null!;
    private Label _fpsLabel = null!;

    private void SetupUI(AssetManager assetManager)
    {
        _layout = new BasicLayout(ScorpiaStyle.Stylesheet);

        // var actionBarContainer = new HorizontalGridLayout
        // {
        //     Anchor = UIAnchor.Bottom,
        //     Background = assetManager.Get<Sprite>("Game:HUD/action_bar"),
        //     MinWidth = 600
        // };
        // actionBarContainer.SetHeight(194);
        // _layout.Attach(actionBarContainer);

        _infoContainer = new BasicLayout
        {
            Anchor = UIAnchor.BottomRight,
            Background = assetManager.Get<Sprite>("Game:HUD/info"),
            Show = false
        };
        _infoContainer.SetSize(1085, 425);
        _layout.Attach(_infoContainer);
        
        var topContainer = new BasicLayout
        {
            Anchor = UIAnchor.Top,
            Background = assetManager.Get<Sprite>("Game:HUD/top_bar")
        };
        topContainer.SetSize(2268, 237);
        _layout.Attach(topContainer);
        
        _fpsLabel = new Label
        {
            Text = "0",
            Anchor = UIAnchor.TopLeft,
            Color = Color.White,
            Size = 36,
            Position = new Point(20, 20)
        };
        _layout.Attach(_fpsLabel);
    }
}