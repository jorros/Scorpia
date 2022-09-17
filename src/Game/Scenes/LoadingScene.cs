using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Game.Scenes;

public partial class LoadingScene : NetworkedScene
{
    private bool _started;

    protected override void OnLoad(AssetManager assetManager)
    {
        _assetManager = assetManager;
        SetupUI(assetManager);
    }

    protected override void OnTick()
    {
        if (NetworkManager.IsServer)
        {
            ServerOnTick();
            return;
        }

        if (_started)
        {
            return;
        }

        _started = true;
        
        SetProgress(0);
        _assetManager.Load("Game");
        SetProgress(80);
        
        SceneManager.Load<GameScene>();
        SetProgress(100);
    }

    private void SetProgress(byte progress)
    {
        _loadingBar.Progress = progress;
        Invoke(nameof(UpdateProgressServerRpc), progress);
    }

    protected override void OnRender(RenderContext context)
    {
        _layout.Render(context, false);
    }
}