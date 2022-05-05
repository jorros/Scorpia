using Actors;
using Actors.Entities;
using TMPro;
using UnityEngine;
using Utils;

namespace Map
{
    public class MapTitle : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;

        public void SetLocation(Location location)
        {
            var player = Game.GetPlayer(location.Player.ValueAsString());
            var colourText = player.Colour.Value switch
            {
                PlayerColour.Blue => "<sprite name=title_icon_blue>",
                PlayerColour.Brown => "<sprite name=title_icon_brown>",
                PlayerColour.Green => "<sprite name=title_icon_green>",
                PlayerColour.Grey => "<sprite name=title_icon_grey>",
                PlayerColour.Orange => "<sprite name=title_icon_orange>",
                PlayerColour.Purple => "<sprite name=title_icon_purple>",
                PlayerColour.Red => "<sprite name=title_icon_red>",
                PlayerColour.Yellow => "<sprite name=title_icon_yellow>",
                _ => ""
            };

            var capitalText = location.IsCapital.Value ? " <sprite name=title_icon_capital> " : "";

            var famineText = location.Tags.Contains(LocationTags.Famine) ? " <sprite name=title_icon_hunger> " : "";
            
            text.text = $"{colourText} {location.Name.Value.Value} {capitalText} {famineText}";
        }
    }
}