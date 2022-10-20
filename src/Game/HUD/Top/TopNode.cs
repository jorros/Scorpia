using System.Drawing;
using Scorpia.Engine.Asset;
using Scorpia.Engine.SceneManagement;
using Scorpia.Engine.UI;
using Scorpia.Game.HUD.Tooltip;

namespace Scorpia.Game.HUD.Top;

public class TopNode : Node
{
    public BasicLayout TopBar { get; set; } = null!;
    private TooltippedElement<HorizontalGridLayout> _scorpions = null!;
    private Label _scorpionsLabel = null!;
    private Label _scorpionsBalanceLabel = null!;
    private TooltippedElement<HorizontalGridLayout> _food = null!;
    private Label _foodLabel = null!;
    private Label _foodBalanceLabel = null!;
    private TooltippedElement<HorizontalGridLayout> _population = null!;
    private Label _populationLabel = null!;
    private Label _populationBalanceLabel = null!;
    private TooltippedElement<HorizontalGridLayout> _nitra = null!;
    private Label _nitraLabel = null!;
    private Label _nitraBalanceLabel = null!;
    private TooltippedElement<HorizontalGridLayout> _sofrum = null!;
    private Label _sofrumLabel = null!;
    private Label _sofrumBalanceLabel = null!;
    private TooltippedElement<HorizontalGridLayout> _zellos = null!;
    private Label _zellosLabel = null!;
    private Label _zellosBalanceLabel = null!;

    public override void OnInit()
    {
        (_scorpions, _scorpionsLabel, _scorpionsBalanceLabel) = CreateStat("doubloons", "Scorpions", 0);
        (_food, _foodLabel, _foodBalanceLabel) = CreateStat("food", "Food", 1);
        (_nitra, _nitraLabel, _nitraBalanceLabel) = CreateStat("nitra", "Nitra", 2);
        (_sofrum, _sofrumLabel, _sofrumBalanceLabel) = CreateStat("sofrum", "Sofrum", 3);
        (_zellos, _zellosLabel, _zellosBalanceLabel) = CreateStat("zellos", "Zellos", 4);
        (_population, _populationLabel, _populationBalanceLabel) = CreateStat("population", "Population", 5);

        // CreateButton("finance", "Finance", "Blabla", new Point(819, -59));
        // CreateButton("research", "Research", "Blabla", new Point(710, -59));
        // CreateButton("diplomacy", "Diplomacy", "Blabla", new Point(601, -59));
        // CreateButton("military", "Military", "Blabla", new Point(492, -59));
        // CreateButton("cities", "Cities", "Blabla", new Point(383, -59));
        // CreateButton("notifications", "Notifications", "Blabla", new Point(260, -59), true);
    }

    public override void OnTick()
    {
        var player = Game.CurrentPlayer.GetSelf();
        if (player is null)
        {
            return;
        }

        _scorpionsLabel.Text = player.Scorpions.Value.Format();
        _scorpionsBalanceLabel.Text = $" ({player.ScorpionsBalance.Value.Total.FormatBalance()})";
        _scorpions.Description!.Content = BalanceSheetFormatter.Format(player.ScorpionsBalance.Value);
        
        _foodLabel.Text = player.Food.Value.Format();
        _foodBalanceLabel.Text = $" ({player.FoodBalance.Value.Total.FormatBalance()})";
        _food.Description!.Content = BalanceSheetFormatter.Format(player.FoodBalance.Value);
        
        _populationLabel.Text = player.Population.Value.Format();
        
        _nitraLabel.Text = player.Nitra.Value.Format();
        _nitraBalanceLabel.Text = $" ({player.NitraBalance.Value.Total.FormatBalance()})";
        _nitra.Description!.Content = BalanceSheetFormatter.Format(player.NitraBalance.Value);
        _sofrumLabel.Text = player.Sofrum.Value.Format();
        _sofrumBalanceLabel.Text = $" ({player.SofrumBalance.Value.Total.FormatBalance()})";
        _sofrum.Description!.Content = BalanceSheetFormatter.Format(player.SofrumBalance.Value);
        _zellosLabel.Text = player.Zellos.Value.Format();
        _zellosBalanceLabel.Text = $" ({player.ZellosBalance.Value.Total.FormatBalance()})";
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

    private (TooltippedElement<HorizontalGridLayout>, Label, Label) CreateStat(string icon, string name, int index)
    {
        var container = new TooltippedElement<HorizontalGridLayout>(new HorizontalGridLayout
        {
            Anchor = UIAnchor.Left,
            Position = new Point(index * 200 + 20, 0)
        }, AssetManager);
        container.Value.SetHeight(59);
        container.Description = new TooltipDescription(name, string.Empty, string.Empty, TooltipPosition.Top);
        TopBar.Attach(container);

        var iconImage = new Image
        {
            Sprite = AssetManager.Get<Sprite>($"Game:HUD/top_{icon}")
        };
        container.Value.Attach(iconImage);

        var label = new Label
        {
            Type = "top",
            Text = "0",
        };
        container.Value.Attach(label);

        var balanceLabel = new Label
        {
            Type = "top",
            Text = "",
        };
        container.Value.Attach(balanceLabel);

        return (container, label, balanceLabel);
    }
}