using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.Graphics;
using Scorpia.Engine.InputManagement;
using Scorpia.Engine.SceneManagement;
using Scorpia.Game.HUD;
using Scorpia.Game.HUD.TileInfo;
using Scorpia.Game.HUD.Top;
using Scorpia.Game.Nodes;
using Scorpia.Game.World;

namespace Scorpia.Game.Scenes;

public partial class GameScene : NetworkedScene
{
    private MapNode _map = null!;
    public override Color BackgroundColor => Color.FromArgb(105, 105, 108);
    private Minimap? _minimap;

    protected override void OnLoad(AssetManager assetManager)
    {
        SetupUI(assetManager);

        Input.OnKeyboard += OnKeyPressed;

        _map = CreateNode<MapNode>();
        _minimap = new Minimap(ServiceProvider.GetRequiredService<RenderContext>(), _map, assetManager,
            new Size(660, 440));

        CreateNode<TileInfoNode>(node => { node.Window = infoWindow; });
        CreateNode<TopNode>(node => { node.TopBar = topContainer; });
    }

    protected override void OnLeave()
    {
        Input.OnKeyboard -= OnKeyPressed;
    }

    private void OnKeyPressed(object? sender, KeyboardEventArgs e)
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

    protected override void OnUpdate()
    {
        _minimap?.Update();
    }

    protected override void OnRender(RenderContext context)
    {
        _layout.Render(context, ScorpiaStyle.Stylesheet, false);
        _minimap?.Render();
    }
    
    [Event(nameof(SelectTile))]
    public void SelectTile(MapTile select)
    {
        var riverText = "none";

        if (select.River is not null)
        {
            riverText = string.Join(",", select.River.Select(x => x.ToString()));
        }
        
        _currentTileDebugLabel.Text = $"{select.Biome.ToString()} - R:{riverText}";
    }

    [Event(nameof(DeselectTile))]
    public void DeselectTile()
    {
        _currentTileDebugLabel.Text = string.Empty;
    }
}