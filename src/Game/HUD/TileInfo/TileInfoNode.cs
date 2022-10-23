using System.Drawing;
using Microsoft.Extensions.DependencyInjection;
using Scorpia.Engine.Asset;
using Scorpia.Engine.SceneManagement;
using Scorpia.Engine.UI;
using Scorpia.Game.HUD.Tooltip;
using Scorpia.Game.Player;
using Scorpia.Game.World;

namespace Scorpia.Game.HUD.TileInfo;

public class TileInfoNode : Node
{
    private IReadOnlyList<ITileInfo> _tileInfos = null!;
    private MapTile? _selected;

    private Image _avatar = null!;
    private Label _nameText = null!;
    private HorizontalGridLayout _infoIcons = null!;

    public CurrentPlayer? CurrentPlayer => ServiceProvider.GetService<CurrentPlayer>();

    public Window Window { get; set; } = null!;

    private int _lastTileInfo = -1;

    public override void OnInit()
    {
        _tileInfos = new ITileInfo[]
        {
            new EmptyTileInfo(this),
            new CityTileInfo(this)
        };

        _avatar = new Image
        {
            Sprite = AssetManager.Get<Sprite>("Game:HUD/info_avatar_grass"),
            Height = 102,
            Width = 127
        };
        Window.AttachTitle(_avatar);
        
        _nameText = new Label
        {
            Type = "header",
            Text = "Test",
            Margin = new Point(0, 23)
        };
        Window.AttachTitle(_nameText);

        _infoIcons = new HorizontalGridLayout
        {
            SpaceBetween = 15,
            Margin = new Point(0, 23)
        };
        Window.Title.Attach(_infoIcons);
    }

    public override void OnUpdate()
    {
        if (_selected is null)
        {
            return;
        }

        for (var index = 0; index < _tileInfos.Count; index++)
        {
            var tileInfo = _tileInfos[index];
            if (!tileInfo.ShouldRender(_selected))
            {
                continue;
            }

            if (_lastTileInfo != index)
            {
                Window.Height = tileInfo.WindowHeight;
                tileInfo.Init(_selected);
                _lastTileInfo = index;
            }

            tileInfo.Update(_selected);
            break;
        }
    }

    private void ClearWindow()
    {
        _lastTileInfo = -1;
        Window.Clear();
        _infoIcons.Clear();
    }

    [Event(nameof(SelectTile))]
    private void SelectTile(MapTile tile)
    {
        _selected = tile;
        ClearWindow();
        Window.Show = true;
    }

    [Event(nameof(DeselectTile))]
    private void DeselectTile()
    {
        _selected = null;
        ClearWindow();
        Window.Show = false;
    }

    public void AddInfoIcon(string icon, TooltipDescription tooltipDesc)
    {
        var item = new TooltippedElement<Image>(new Image
        {
            Sprite = AssetManager.Get<Sprite>($"Game:HUD/info_icon_{icon}"),
            Height = 60,
            Width = 60
        }, AssetManager);
        _infoIcons.Attach(item);

        item.Description = tooltipDesc;
    }

    public void SetName(string name)
    {
        _nameText.Text = name.ToUpper();
    }

    public void SetAvatarIcon(string avatar)
    {
        _avatar.Sprite = AssetManager.Get<Sprite>($"Game:HUD/info_avatar_{avatar}");
    }
}