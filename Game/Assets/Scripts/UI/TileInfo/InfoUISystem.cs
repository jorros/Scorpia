using System.Collections.Generic;
using System.Linq;
using Map;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TileInfo
{
    public class InfoUISystem : MonoBehaviour
    {
        [SerializeField]
        private GameObject instance;

        [SerializeField]
        private Image[] statIcon;

        [SerializeField]
        private TextMeshProUGUI[] statText;

        [SerializeField]
        private TooltipTrigger[] statTooltip;

        [SerializeField]
        private TextMeshProUGUI nameText;

        [SerializeField]
        private Image[] infoIcon;

        [SerializeField]
        private TooltipTrigger[] infoTooltip;

        [SerializeField]
        private Image avatar;

        [SerializeField]
        private Sprite[] infoIconSprite;

        [SerializeField]
        private Sprite[] avatarSprite;

        [SerializeField]
        private Sprite[] statIconSprite;

        private int infoCounter, statCounter;

        private MapTile selected;

        private IReadOnlyList<ITileInfo> tileInfos;

        private void Start()
        {
            EventManager.Register(EventManager.SelectTile, SelectTile);
            EventManager.Register(EventManager.DeselectTile, Deselect);

            tileInfos = new[]
            {
                new EmptyTileInfo(this)
            };
        }

        private void OnDestroy()
        {
            EventManager.Remove(EventManager.SelectTile, SelectTile);
            EventManager.Remove(EventManager.DeselectTile, Deselect);
        }

        private void FixedUpdate()
        {
            if (selected != null)
            {
                infoCounter = 0;
                statCounter = 0;

                tileInfos.First().Render(selected);

                for(var i = infoCounter; i < 6; i++)
                {
                    AddInfoIcon(1, TooltipDescription.Empty);
                }
                
                for(var i = statCounter; i < 6; i++)
                {
                    statIcon[i].gameObject.SetActive(false);
                    statText[i].text = string.Empty;
                }
            }
        }

        private void SelectTile(IReadOnlyList<object> args)
        {
            selected = args[0] as MapTile;

            instance.SetActive(true);
        }

        private void Deselect(IReadOnlyList<object> args)
        {
            selected = null;

            instance.SetActive(false);
        }

        public void AddInfoIcon(int icon, TooltipDescription tooltipDesc)
        {
            infoIcon[infoCounter].sprite = infoIconSprite[icon];

            var tooltip = infoTooltip[infoCounter];
            tooltip.header = tooltipDesc.Header;
            tooltip.content = tooltipDesc.Content;
            tooltip.subHeader = tooltipDesc.SubHeader;

            infoCounter++;
        }

        public void SetName(string name)
        {
            nameText.text = name;
        }

        public void SetAvatarIcon(int avatarIndex)
        {
            avatar.sprite = avatarSprite[avatarIndex];
        }

        public void AddStat(int icon, string value, TooltipDescription tooltipDesc, Color? colour = null)
        {
            statIcon[statCounter].gameObject.SetActive(true);
            statIcon[statCounter].sprite = statIconSprite[icon];

            statText[statCounter].text = value;
            if (colour != null)
            {
                statText[statCounter].color = colour.Value;
            }

            var tooltip = statTooltip[statCounter];
            tooltip.header = tooltipDesc.Header;
            tooltip.content = tooltipDesc.Content;
            tooltip.subHeader = tooltipDesc.SubHeader;

            statCounter++;
        }
    }
}

