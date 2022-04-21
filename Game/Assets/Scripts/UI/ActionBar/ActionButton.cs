using TMPro;
using UI.Tooltip;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.ActionBar
{
    public class ActionButton : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private GameObject upgradeIcon;
        [SerializeField] private GameObject inProgressIcon;
        [SerializeField] private TextMeshProUGUI levelText;
        [SerializeField] private Image progressBar;
        [SerializeField] private Button button;
        [SerializeField] private TooltipTrigger tooltip;

        public void SetIcon(ActionButtonOptions.ActionButtonType? icon)
        {
            if (upgradeIcon == null || inProgressIcon == null)
            {
                return;
            }

            switch (icon)
            {
                case ActionButtonOptions.ActionButtonType.Upgrade:
                    upgradeIcon.SetActive(true);
                    inProgressIcon.SetActive(false);
                    break;
                case ActionButtonOptions.ActionButtonType.InProgress:
                    upgradeIcon.SetActive(false);
                    inProgressIcon.SetActive(true);
                    break;
                default:
                    upgradeIcon.SetActive(false);
                    inProgressIcon.SetActive(false);
                    break;
            }
        }

        public void SetImage(Sprite sprite)
        {
            image.sprite = sprite;
        }

        public void SetLevel(int? level)
        {
            if (levelText == null)
            {
                return;
            }

            if (level == null)
            {
                levelText.gameObject.SetActive(false);
                return;
            }

            levelText.text = level.ToString();
            levelText.gameObject.SetActive(true);
        }

        public void SetProgress(int progress)
        {
            if (progressBar == null)
            {
                return;
            }

            progressBar.rectTransform.sizeDelta =
                new Vector2(progressBar.rectTransform.sizeDelta.x, 100 - progress);
        }

        public void SetEnabled(bool enable)
        {
            button.interactable = enable;
        }

        public void SetClickAction(UnityAction action)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(action);
        }

        public void SetTooltip(TooltipDescription desc)
        {
            tooltip.header = desc.Header;
            tooltip.content = desc.Content;
            tooltip.subHeader = desc.SubHeader;
            tooltip.position = desc.Position;
        }
    }
}