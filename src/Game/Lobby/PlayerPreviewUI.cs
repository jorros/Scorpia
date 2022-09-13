using Scorpia.Engine;
using Scorpia.Engine.Asset;
using Scorpia.Engine.UI;
using Scorpia.Game.Player;

namespace Scorpia.Game.Lobby;

public class PlayerPreviewUI : BasicLayout
{
    public string Name { get; }
    
    public PlayerPreviewUI(AssetManager assetManager, string name, PlayerColor color)
    {
        Name = name;
        
        SetSize(250, 400);
        
        var playerImage = new Image
        {
            Sprite = assetManager.Get<Sprite>($"UI:player_icon_{color.ToString().ToLower()}"),
            Width = 250,
            Height = 250
        };
        Attach(playerImage);

        var playerLabel = new Label
        {
            Text = name,
            TextAlign = TextAlign.Center,
            Type = "PlayerPreview",
            Position = new OffsetVector(125, 270)
        };
        Attach(playerLabel);
    }
}