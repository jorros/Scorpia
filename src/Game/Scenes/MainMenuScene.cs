using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Menu;
using Scorpia.Game.Nodes;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public class MainMenuScene : Scene
{
    private readonly AssetBundle _assets;
    private readonly MainMenu _menu;

    public MainMenuScene(IServiceProvider serviceProvider, AssetManager assetManager, Desktop desktop) : base(serviceProvider)
    {
        _assets = assetManager.Load("UI", true);

        CreateNode<TestNode>();

        var stylesheet = _assets.Get<MyraMarkup>("ui_stylesheet");
        Stylesheet.Current = stylesheet.Stylesheet;
        
        _menu = new MainMenu();
        desktop.Root = _menu;
    }

    protected override void OnRender(RenderContext context)
    {
        // _menu._position.Text = $"{Input.MousePosition.X}:{Input.MousePosition.Y}";
    }
}