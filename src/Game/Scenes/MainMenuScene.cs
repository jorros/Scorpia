using System.Drawing;
using Scorpia.Engine;
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
    private readonly Font _font;

    public MainMenuScene(IServiceProvider serviceProvider, AssetManager assetManager) : base(serviceProvider)
    {
        _assets = assetManager.Load("UI");
        
        _font = _assets.Get<Font>("MYRIADPRO-REGULAR");

        CreateNode<TestNode>();
    }

    protected override void OnRender(RenderContext context)
    {
        context.Draw(_assets.Get<Sprite>("button_regular"), new Rectangle(300, 200, 400, 150));
        context.DrawText(_font, new OffsetVector(500, 500), "Test 123", 30, Color.Beige);
        
        context.DrawText(_font, new OffsetVector(700, 500), $"{Input.MousePosition.X}:{Input.MousePosition.Y}", 30, Color.Beige);
    }
}