using System.Drawing;
using Scorpia.Game.Player;
using Scorpia.Game.Utils;
using Scorpian.Asset;
using Scorpian.UI;

namespace Scorpia.Game.Lobby;

public class PlayerPreviewUI : BasicLayout
{
    public string Name { get; }
    
    public PlayerPreviewUI(AssetManager assetManager, string name, PlayerColor color, PlayerFaction faction)
    {
        Name = name;
        Background = assetManager.Get<Sprite>("UI:list");
        Width = 560;
        Height = 80;
        
        var playerImage = new Image
        {
            Sprite = assetManager.Get<Sprite>($"UI:player_icon_{color.ToString().ToLower()}"),
            Width = 60,
            Height = 60,
            Anchor = UIAnchor.Left,
            Position = new Point(10, 0)
        };
        Attach(playerImage);

        var playerLabel = new Label
        {
            Text = name,
            Type = "form",
            Position = new Point(100, -15),
            Anchor = UIAnchor.Left,
            Font = "SemiBold"
        };
        Attach(playerLabel);
        
        var factionLabel = new Label
        {
            Text = faction.GetDescription(),
            Type = "form",
            Position = new Point(100, 15),
            Anchor = UIAnchor.Left,
            Style = FontStyle.Italic
        };
        Attach(factionLabel);
    }
}