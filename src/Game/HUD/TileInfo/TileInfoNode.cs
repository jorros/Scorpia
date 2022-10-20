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
    private int _infoCounter, _statCounter;

    private Image _avatar = null!;
    // private Label[] _statTexts = null!;
    private Label _nameText = null!;

    public CurrentPlayer? CurrentPlayer => ServiceProvider.GetService<CurrentPlayer>();

    public Window Window { get; set; } = null!;

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
            Text = "Test"
        };
        Window.AttachTitle(_nameText);

        var test = new TooltippedElement<Image>(new Image
        {
            Sprite = AssetManager.Get<Sprite>("Game:HUD/info_icon_fertile"),
            Height = 60,
            Width = 60
        }, AssetManager);
        Window.AttachTitle(new Content(test));

        // _statTexts = new[]
        // {
        //     new Label
        //     {
        //         Anchor = UIAnchor.Left,
        //         Type = "StatLabel",
        //         Position = new Point(310, -132),
        //         Text = "12345"
        //     },
        // };
        // foreach (var text in _statTexts)
        // {
        //     Window.Attach(text);
        // }
    }

    public override void OnUpdate()
    {
        if (_selected is null)
        {
            return;
        }
        
        _infoCounter = 0;
        _statCounter = 0;

        foreach (var tileInfo in _tileInfos)
        {
            if (!tileInfo.ShouldRender(_selected))
            {
                continue;
            }

            tileInfo.Render(_selected);
            break;
        }

        // for (var i = _infoCounter; i < 6; i++)
        // {
        //     AddInfoIcon("empty", TooltipDescription.Empty);
        // }
                
        // for(var i = _statCounter; i < 6; i++)
        // {
        //     _statTexts[i].Text = string.Empty;
        // }
    }

    [Event(nameof(SelectTile))]
    private void SelectTile(MapTile tile)
    {
        _selected = tile;
        Window.Show = true;
    }

    [Event(nameof(DeselectTile))]
    private void DeselectTile()
    {
        _selected = null;
        Window.Show = false;
    }

    public void AddInfoIcon(string icon, TooltipDescription tooltipDesc)
    {
        // _infoIcons[_infoCounter].Value.Sprite = AssetManager.Get<Sprite>($"Game:HUD/info_icon_{icon}");
        // _infoIcons[_infoCounter].Description = tooltipDesc;

        _infoCounter++;
    }

    public void SetName(string name)
    {
        _nameText.Text = name.ToUpper();
    }

    public void SetAvatarIcon(string avatar)
    {
        _avatar.Sprite = AssetManager.Get<Sprite>($"Game:HUD/info_avatar_{avatar}");
    }

    public void AddStat(string name, string value, TooltipDescription tooltipDesc, Color? colour = null)
    {
        // _statTexts[_statCounter].Text = value;
        // if (colour != null)
        // {
        //     _statTexts[_statCounter].Color = colour.Value;
        // }
        
        // var tooltip = statTooltip[statCounter];
        // tooltip.header = tooltipDesc.Header;
        // tooltip.content = tooltipDesc.Content;
        // tooltip.subHeader = tooltipDesc.SubHeader;

        _statCounter++;
    }
}