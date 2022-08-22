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
    private readonly AssetManager _assetManager;
    private readonly Font _font;

    public MainMenuScene(IServiceProvider serviceProvider, AssetManager assetManager) : base(serviceProvider)
    {
        _assetManager = assetManager;
        assetManager.Load("UI");
        
        _font = assetManager.Get<Font>("UI:MYRIADPRO-REGULAR");

        CreateNode<TestNode>();
    }

    protected override void OnRender(RenderContext context)
    {
        context.Draw(_assetManager.Get<Sprite>("UI:button_regular"), new Rectangle(300, 200, 400, 150));
        context.DrawText(_font, new OffsetVector(500, 500), "<text style='italic,bold'>Test</text><text color='#000000'>123</text>", 30, Color.Beige);
        
        context.DrawText(_font, new OffsetVector(700, 500), $"<outline color='#ff0000' size='1'>{Input.MousePosition.X}:{Input.MousePosition.Y}</outline>", 30, Color.Beige);
    }
}