using System.Drawing;
using Scorpian.Asset;
using Scorpian.UI;

namespace Scorpia.Game.Scenes;

public partial class LoadingScene
{
    private BasicLayout _layout;
    
    private AssetManager _assetManager;
    private ProgressBar _loadingBar;

    private void SetupUI(AssetManager assetManager)
    {
        _assetManager = assetManager;
        
        _layout = new BasicLayout
        {
            Background = assetManager.Get<Sprite>("UI:loading_background")
        };

        _loadingBar = new ProgressBar
        {
            Anchor = UIAnchor.Bottom,
            Position = new Point(0, 100),
            Type = "loading"
        };
        _loadingBar.SetWidth(1942);
        _layout.Attach(_loadingBar);
    }
}