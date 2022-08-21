using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Nodes;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public class MainMenuScene : Scene
{
    private readonly AssetBundle _assets;

    public MainMenuScene(IServiceProvider serviceProvider, AssetManager assetManager) : base(serviceProvider)
    {
        _assets = assetManager.Load("UI");

        CreateNode<TestNode>();
    }

    protected override void OnRender(RenderContext context)
    {
        context.Draw(_assets.Get<Sprite>("button_regular"), new Rectangle(300, 200, 400, 150));
        // _menu._position.Text = $"{Input.MousePosition.X}:{Input.MousePosition.Y}";
    }
}