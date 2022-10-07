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
    private TooltippedElement<Image>[] _infoIcons = null!;
    private Image[] _statIcons = null!;
    private Label[] _statTexts = null!;
    private Label _nameText = null!;

    public CurrentPlayer? CurrentPlayer => ServiceProvider.GetService<CurrentPlayer>();

    public BasicLayout InfoBox { get; set; } = null!;

    public override void OnInit()
    {
        _tileInfos = new ITileInfo[]
        {
            new EmptyTileInfo(this),
            new CityTileInfo(this)
        };

        _avatar = new Image
        {
            Position = new Point(25, 128),
            Anchor = UIAnchor.BottomRight,
            Sprite = AssetManager.Get<Sprite>("Game:HUD/info_avatar_grass"),
            Height = 156,
            Width = 190
        };
        InfoBox.Attach(_avatar);

        _infoIcons = new[]
        {
            new TooltippedElement<Image>(new Image
            {
                Anchor = UIAnchor.Right,
                Position = new Point(409, -41),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_icon_fertile"),
                Height = 77,
                Width = 77
            }, AssetManager),
            new TooltippedElement<Image>(new Image
            {
                Anchor = UIAnchor.Right,
                Position = new Point(317, -41),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_icon_fertile"),
                Height = 77,
                Width = 77
            }, AssetManager),
            new TooltippedElement<Image>(new Image
            {
                Anchor = UIAnchor.Right,
                Position = new Point(225, -41),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_icon_fertile"),
                Height = 77,
                Width = 77
            }, AssetManager),
            new TooltippedElement<Image>(new Image
            {
                Anchor = UIAnchor.Right,
                Position = new Point(409, 51),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_icon_fertile"),
                Height = 77,
                Width = 77
            }, AssetManager),
            new TooltippedElement<Image>(new Image
            {
                Anchor = UIAnchor.Right,
                Position = new Point(317, 51),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_icon_fertile"),
                Height = 77,
                Width = 77
            }, AssetManager),
            new TooltippedElement<Image>(new Image
            {
                Anchor = UIAnchor.Right,
                Position = new Point(225, 51),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_icon_fertile"),
                Height = 77,
                Width = 77
            }, AssetManager),
        };
        foreach (var icon in _infoIcons)
        {
            InfoBox.Attach(icon);
        }

        _statIcons = new[]
        {
            new Image
            {
                Anchor = UIAnchor.Left,
                Position = new Point(270, -134),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_stat_icons_balance"),
                Height = 36,
                Width = 36
            },
            new Image
            {
                Anchor = UIAnchor.Left,
                Position = new Point(270, -93),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_stat_icons_balance"),
                Height = 36,
                Width = 36
            },
            new Image
            {
                Anchor = UIAnchor.Left,
                Position = new Point(270, -51),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_stat_icons_balance"),
                Height = 36,
                Width = 36
            },
            new Image
            {
                Anchor = UIAnchor.Left,
                Position = new Point(270, -7),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_stat_icons_balance"),
                Height = 36,
                Width = 36
            },
            new Image
            {
                Anchor = UIAnchor.Left,
                Position = new Point(270, 38),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_stat_icons_balance"),
                Height = 36,
                Width = 36
            },
            new Image
            {
                Anchor = UIAnchor.Left,
                Position = new Point(270, 84),
                Sprite = AssetManager.Get<Sprite>("Game:HUD/info_stat_icons_balance"),
                Height = 36,
                Width = 36
            },
        };
        foreach (var icon in _statIcons)
        {
            InfoBox.Attach(icon);
        }

        _statTexts = new[]
        {
            new Label
            {
                Anchor = UIAnchor.Left,
                Type = "StatLabel",
                Position = new Point(310, -132),
                Text = "12345"
            },
            new Label
            {
                Anchor = UIAnchor.Left,
                Type = "StatLabel",
                Position = new Point(310, -91),
                Text = "12345"
            },
            new Label
            {
                Anchor = UIAnchor.Left,
                Type = "StatLabel",
                Position = new Point(310, -49),
                Text = "12345"
            },
            new Label
            {
                Anchor = UIAnchor.Left,
                Type = "StatLabel",
                Position = new Point(310, -5),
                Text = "12345"
            },
            new Label
            {
                Anchor = UIAnchor.Left,
                Type = "StatLabel",
                Position = new Point(310, 40),
                Text = "12345"
            },
            new Label
            {
                Anchor = UIAnchor.Left,
                Type = "StatLabel",
                Position = new Point(310, 86),
                Text = "12345"
            }
        };
        foreach (var text in _statTexts)
        {
            InfoBox.Attach(text);
        }

        _nameText = new Label
        {
            Anchor = UIAnchor.Top,
            Position = new Point(330, 63),
            Type = "InfoNameLabel",
            Text = "",
            TextAlign = TextAlign.Center
        };
        InfoBox.Attach(_nameText);
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

        for (var i = _infoCounter; i < 6; i++)
        {
            AddInfoIcon("empty", TooltipDescription.Empty);
        }
                
        for(var i = _statCounter; i < 6; i++)
        {
            _statIcons[i].Show = false;
            _statTexts[i].Text = string.Empty;
        }
    }

    [Event(nameof(SelectTile))]
    private void SelectTile(MapTile tile)
    {
        _selected = tile;
        InfoBox.Show = true;
    }

    [Event(nameof(DeselectTile))]
    private void DeselectTile()
    {
        _selected = null;
        InfoBox.Show = false;
    }

    public void AddInfoIcon(string icon, TooltipDescription tooltipDesc)
    {
        _infoIcons[_infoCounter].Value.Sprite = AssetManager.Get<Sprite>($"Game:HUD/info_icon_{icon}");
        _infoIcons[_infoCounter].Description = tooltipDesc;

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

    public void AddStat(string icon, string value, TooltipDescription tooltipDesc, Color? colour = null)
    {
        _statIcons[_statCounter].Show = true;
        _statIcons[_statCounter].Sprite = AssetManager.Get<Sprite>($"Game:HUD/info_stat_icons_{icon}");
        
        _statTexts[_statCounter].Text = value;
        if (colour != null)
        {
            _statTexts[_statCounter].Color = colour.Value;
        }
        
        // var tooltip = statTooltip[statCounter];
        // tooltip.header = tooltipDesc.Header;
        // tooltip.content = tooltipDesc.Content;
        // tooltip.subHeader = tooltipDesc.SubHeader;

        _statCounter++;
    }
}