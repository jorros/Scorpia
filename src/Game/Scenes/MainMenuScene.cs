using Myra.Graphics2D.UI;
using Myra.Graphics2D.UI.Styles;
using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Menu;
using Scorpia.Game.Nodes;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public class MainMenuScene : Scene
{
    private readonly AssetBundle _assets;

    public MainMenuScene(IServiceProvider serviceProvider, AssetManager assetManager, Desktop desktop) : base(serviceProvider)
    {
        _assets = assetManager.Load("Menu");

        CreateNode<TestNode>();

        var stylesheet = _assets.Get<MyraMarkup>("ui_stylesheet");
        Stylesheet.Current = stylesheet.Stylesheet;

        // var project = _assets.Get<MyraMarkup>("MainMenu").Project;
        
        desktop.Root = new MainMenu();
    }

    protected override void OnRender(RenderContext context)
    {
        context.Draw(_assets.Get<Sprite>("Sprites/menu_background"), OffsetVector.Zero, OffsetVector.One, OffsetVector.Zero);
    }
}