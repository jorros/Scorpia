using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.Network.Protocol;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Game.Scenes;

public partial class LoadingScene : NetworkedScene
{
    public NetworkedVar<int> Seed { get; } = new();
    
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

        while (Seed.Value == 0)
        {
            Thread.Sleep(100);
        }
        
        var game = SceneManager.Load<GameScene>();
        game.InitMap(Seed.Value, true);
        SetProgress(100);
    }

    private void SetProgress(byte progress)
    {
        _loadingBar.Progress = progress;
        Invoke(nameof(UpdateProgressServerRpc), progress);
    }

    protected override void OnRender(RenderContext context)
    {
        _layout.Render(context, ScorpiaStyle.Stylesheet, false);
    }
}