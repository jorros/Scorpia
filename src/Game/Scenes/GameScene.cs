using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Game.HUD;
using Scorpia.Game.HUD.TileInfo;
using Scorpia.Game.HUD.Top;
using Scorpia.Game.Nodes;
using Scorpia.Game.World;
using Scorpian.Asset;
using Scorpian.Graphics;
using Scorpian.InputManagement;
using Scorpian.SceneManagement;

namespace Scorpia.Game.Scenes;

public partial class GameScene : NetworkedScene
{
    private MapNode _map = null!;
    public override Color BackgroundColor => Color.FromArgb(105, 105, 108);
    private Minimap? _minimap;

    private bool _initialised;

    protected override void OnLoad(AssetManager assetManager)
    {
        SetupUI(assetManager);

        Input.OnKeyboard += OnKeyPressed;

        _map = CreateNode<MapNode>();
        _minimap = new Minimap(ServiceProvider.GetRequiredService<RenderContext>(), _map, assetManager,
            new Size(660, 440));

        CreateNode<TileInfoNode>(node => { node.Window = infoWindow; });
        CreateNode<TopNode>(node =>
        {
            node.TopBar = topContainer;
            node.playerLabel = _playerLabel;
        });
    }

    protected override Task OnLeave()
    {
        Input.OnKeyboard -= OnKeyPressed;
        
        return Task.CompletedTask;
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
    }

    protected override Task OnTick()
    {
        if (NetworkManager.IsServer)
        {
            return Task.CompletedTask;
        }

        var renderContext = ServiceProvider.GetService<RenderContext>();
        if (renderContext is not null)
        {
            _fpsLabel.Text = renderContext.FPS.ToString();
        }
        
        return Task.CompletedTask;
    }

    protected override Task OnUpdate()
    {
        _minimap?.Update();
        
        return Task.CompletedTask;
    }

    protected override void OnRender(RenderContext context)
    {
        if (!_initialised)
        {
            
        }
        layout.Render(context, ScorpiaStyle.Stylesheet, false);
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

        _currentTileDebugLabel.Text = $"[{select.Position}] {select.Biome.ToString()} - R:{riverText}";
    }

    [Event(nameof(DeselectTile))]
    public void DeselectTile()
    {
        _currentTileDebugLabel.Text = string.Empty;
    }
}