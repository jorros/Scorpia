using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.SceneManagement;

namespace Scorpia.Game.Scenes;

// ReSharper disable once ClassNeverInstantiated.Global
public class MainMenuScene : Scene
{
    private readonly AssetBundle _assets;

    private readonly AssetBundle _game;

    public MainMenuScene(AssetManager assetManager)
    {
        _assets = assetManager.Load("Menu");
        _game = assetManager.Load("Game");
    }

    protected override void OnRender(RenderContext context)
    {
        context.Draw(_assets.Get<Sprite>("Sprites/menu_background"), OffsetVector.Zero);
        
        context.Draw(_game.Get<Sprite>("title_icon_blue"), new OffsetVector(50, 50));
    }
}