using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Game.Scenes;

public partial class LoadingScene : NetworkedScene
{
    private byte _progress = 0;
    protected override void OnLoad(AssetManager assetManager)
    {
        SetupUI(assetManager);
    }

    protected override void OnTick()
    {
        if (NetworkManager.IsServer)
        {
            return;
        }
        
        _progress++;
        if (_progress > 100)
        {
            _progress = 0;
        }

        _loadingBar.Progress = _progress;
    }

    protected override void OnRender(RenderContext context)
    {
        _layout.Render(context, false);
    }
}