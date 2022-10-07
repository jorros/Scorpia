using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.SceneManagement;
using Scorpia.Engine.UI;
using Scorpia.Game.HUD.Tooltip;
using Scorpia.Game.Player;

namespace Scorpia.Game.HUD.Top;

public class TopNode : Node
{
    public BasicLayout TopBar { get; set; } = null!;
    private TooltippedElement<BasicLayout> _scorpions = null!;
    private Label _scorpionsLabel = null!;
    private Label _scorpionsBalanceLabel = null!;
    private TooltippedElement<BasicLayout> _food = null!;
    private Label _foodLabel = null!;
    private Label _foodBalanceLabel = null!;
    private TooltippedElement<BasicLayout> _population = null!;
    private Label _populationLabel = null!;
    private Label _populationBalanceLabel = null!;
    private TooltippedElement<BasicLayout> _nitra = null!;
    private Label _nitraLabel = null!;
    private Label _nitraBalanceLabel = null!;
    private TooltippedElement<BasicLayout> _sofrum = null!;
    private Label _sofrumLabel = null!;
    private Label _sofrumBalanceLabel = null!;
    private TooltippedElement<BasicLayout> _zellos = null!;
    private Label _zellosLabel = null!;
    private Label _zellosBalanceLabel = null!;
    private Image _playerIcon = null!;

    public override void OnInit()
    {
        _playerIcon = new Image
        {
            Position = new Point(268, -58),
            Anchor = UIAnchor.Left,
            Sprite = AssetManager.Get<Sprite>("Game:HUD/top_player_blue")
        };
        TopBar.Attach(_playerIcon);

        (_scorpions, _scorpionsLabel, _scorpionsBalanceLabel) = CreateStat("doubloons", "Scorpions", new Point(364, -80));
        (_food, _foodLabel, _foodBalanceLabel) = CreateStat("food", "Food", new Point(564, -80));
        (_population, _populationLabel, _populationBalanceLabel) = CreateStat("population", "Population", new Point(764, -80));
        (_nitra, _nitraLabel, _nitraBalanceLabel) = CreateStat("nitra", "Nitra", new Point(364, -25));
        (_sofrum, _sofrumLabel, _sofrumBalanceLabel) = CreateStat("sofrum", "Sofrum", new Point(564, -25));
        (_zellos, _zellosLabel, _zellosBalanceLabel) = CreateStat("zellos", "Zellos", new Point(764, -25));

        CreateButton("finance", "Finance", "Blabla", new Point(819, -59));
        CreateButton("research", "Research", "Blabla", new Point(710, -59));
        CreateButton("diplomacy", "Diplomacy", "Blabla", new Point(601, -59));
        CreateButton("military", "Military", "Blabla", new Point(492, -59));
        CreateButton("cities", "Cities", "Blabla", new Point(383, -59));
        CreateButton("notifications", "Notifications", "Blabla", new Point(260, -59), true);
    }

    public override void OnTick()
    {
        var player = Game.CurrentPlayer.GetSelf();
        if (player is null)
        {
            return;
        }

        _playerIcon.Sprite = AssetManager.Get<Sprite>($"Game:HUD/top_player_{((PlayerColor)player.Color.Value).ToString().ToLower()}");

        _scorpionsLabel.Text = player.Scorpions.Value.Format();
        _scorpionsBalanceLabel.Text = player.ScorpionsBalance.Value.Total.FormatBalance();
        _scorpions.Description!.Content = BalanceSheetFormatter.Format(player.ScorpionsBalance.Value);
        
        _foodLabel.Text = player.Food.Value.Format();
        _foodBalanceLabel.Text = player.FoodBalance.Value.Total.FormatBalance();
        _food.Description!.Content = BalanceSheetFormatter.Format(player.FoodBalance.Value);
        
        _populationLabel.Text = player.Population.Value.Format();
        
        _nitraLabel.Text = player.Nitra.Value.Format();
        _nitraBalanceLabel.Text = player.NitraBalance.Value.Total.FormatBalance();
        _nitra.Description!.Content = BalanceSheetFormatter.Format(player.NitraBalance.Value);
        _sofrumLabel.Text = player.Sofrum.Value.Format();
        _sofrumBalanceLabel.Text = player.SofrumBalance.Value.Total.FormatBalance();
        _sofrum.Description!.Content = BalanceSheetFormatter.Format(player.SofrumBalance.Value);
        _zellosLabel.Text = player.Zellos.Value.Format();
        _zellosBalanceLabel.Text = player.ZellosBalance.Value.Total.FormatBalance();
        _zellos.Description!.Content = BalanceSheetFormatter.Format(player.ZellosBalance.Value);
    }

    private Button CreateButton(string icon, string name, string description, Point position, bool isCorner = false)
    {
        var btn = new TooltippedElement<Button>(new Button
        {
            Anchor = UIAnchor.Right,
            Content = new Image
            {
                Sprite = AssetManager.Get<Sprite>($"Game:HUD/top_button_{icon}"),
                Anchor = UIAnchor.Center
            },
            Position = position,
            Type = isCorner ? "top_button_corner" : "top_button"
        }, AssetManager)
        {
            Description = new TooltipDescription(name, string.Empty, description, TooltipPosition.Top)
        };
        TopBar.Attach(btn);

        return btn.Value;
    }

    private (TooltippedElement<BasicLayout>, Label, Label) CreateStat(string icon, string name, Point position)
    {
        var container = new TooltippedElement<BasicLayout>(new BasicLayout
        {
            Anchor = UIAnchor.Left,
            Position = position
        }, AssetManager);
        container.Value.SetSize(180, 38);
        container.Description = new TooltipDescription(name, string.Empty, string.Empty, TooltipPosition.Top);
        TopBar.Attach(container);

        var iconImage = new Image
        {
            Position = new Point(0, -6),
            Sprite = AssetManager.Get<Sprite>($"Game:HUD/top_{icon}")
        };
        container.Value.Attach(iconImage);

        var label = new Label
        {
            Position = new Point(36, 0),
            Type = "TopStats",
            Text = "0"
        };
        container.Value.Attach(label);

        var balanceLabel = new Label
        {
            Position = new Point(10, 0),
            Type = "TopStats",
            Text = "",
            Anchor = UIAnchor.TopRight
        };
        container.Value.Attach(balanceLabel);

        return (container, label, balanceLabel);
    }
}