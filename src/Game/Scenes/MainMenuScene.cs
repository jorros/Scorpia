using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public partial class MainMenuScene : Scene
{
    private void QuitButtonOnOnClick(object sender, MouseButtonEventArgs e)
    {
        SceneManager.Quit();
    }

    protected override void OnLoad(AssetManager? assetManager)
    {
        if (assetManager is not null)
        {
            assetManager.Load("UI");
            ScorpiaStyle.Setup(assetManager);
            SetupUI(assetManager);

            _quitButton.OnClick += QuitButtonOnOnClick;
        }
    }

    protected override void OnUpdate()
    {
        var renderContext = ServiceProvider.GetService<RenderContext>();
        if (renderContext is not null)
        {
            _fpsLabel.Text = renderContext.FPS.ToString();
        }
    }

    protected override void OnRender(RenderContext context)
    {
        _layout.Render(context, false);
    }
}