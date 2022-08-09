using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.Nodes;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public class MainMenuScene : Scene
{
    private readonly AssetBundle _assets;

    public MainMenuScene(IServiceProvider serviceProvider, AssetManager assetManager) : base(serviceProvider)
    {
        _assets = assetManager.Load("Menu");

        CreateNode<TestNode>();
    }

    protected override void OnRender(RenderContext context)
    {
        context.Draw(_assets.Get<Sprite>("Sprites/menu_background"), OffsetVector.Zero);
    }
}