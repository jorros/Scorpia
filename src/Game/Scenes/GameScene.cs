using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.HUD.TileInfo;
using Scorpia.Game.HUD.Top;
using Scorpia.Game.Nodes;

namespace Scorpia.Game.Scenes;

public partial class GameScene : NetworkedScene
{
    private MapNode _map = null!;
    public override Color BackgroundColor => Color.FromArgb(105, 105, 108);

    protected override void OnLoad(AssetManager assetManager)
    {
        SetupUI(assetManager);
        
        Input.OnKeyboard += InputOnOnKeyboard;
        
        _map = CreateNode<MapNode>();

        CreateNode<TileInfoNode>(node =>
        {
            node.Window = infoWindow;
        });
        CreateNode<TopNode>(node =>
        {
            node.TopBar = topContainer;
        });
    }

    private void InputOnOnKeyboard(object? sender, KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case KeyboardKey.A:
                Camera.ZoomIn(0.05f);
                break;
            
            case KeyboardKey.D:
                Camera.ZoomOut(0.05f);
                break;
        }
    }

    public void InitMap(int seed, bool graphics)
    {
        _map.Generate(seed);

        if (graphics)
        {
            _map.RefreshTilemap();
        }
    }

    protected override void OnTick()
    {
        if (NetworkManager.IsServer)
        {
            return;
        }
        
        var renderContext = ServiceProvider.GetService<RenderContext>();
        if (renderContext is not null)
        {
            _fpsLabel.Text = renderContext.FPS.ToString();
        }
    }

    protected override void OnRender(RenderContext context)
    {
        _layout.Render(context, ScorpiaStyle.Stylesheet, false);
    }
}