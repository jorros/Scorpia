using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class MainMenuScene : Scene
{
    private readonly SceneManager _sceneManager;

    public MainMenuScene(IServiceProvider serviceProvider, AssetManager assetManager, SceneManager sceneManager) : base(serviceProvider)
    {
        _sceneManager = sceneManager;
        SetupUI(assetManager);
        
        _quitButton.OnClick += QuitButtonOnOnClick;
    }

    private void QuitButtonOnOnClick(object sender, MouseButtonEventArgs e)
    {
        _sceneManager.Quit();
    }

    protected override void OnRender(RenderContext context)
    {
        _fpsLabel.Text = context.FPS.ToString();
        _layout.Render(context, false);
    }
}