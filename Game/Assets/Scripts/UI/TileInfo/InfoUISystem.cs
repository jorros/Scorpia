using System.Collections.Generic;
using System.Linq;
using Map;
using TMPro;
using UI.Tooltip;
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
            EventManager.RegisterAll(this);

            tileInfos = new ITileInfo[]
            {
                new EmptyTileInfo(this),
                new CityTileInfo(this)
            };
        }

        private void OnDestroy()
        {
            EventManager.RemoveAll(this);
        }

        private void FixedUpdate()
        {
            if (selected != null)
            {
                infoCounter = 0;
                statCounter = 0;

                foreach (var tileInfo in tileInfos)
                {
                    if (!tileInfo.ShouldRender(selected))
                    {
                        continue;
                    }
                    
                    tileInfo.Render(selected);
                    break;
                }

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
        
        [Event(EventManager.SelectTile)]
        private void SelectTile(MapTile tile)
        {
            selected = tile;
            instance.SetActive(true);
        }

        [Event(EventManager.DeselectTile)]
        private void Deselect()
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

