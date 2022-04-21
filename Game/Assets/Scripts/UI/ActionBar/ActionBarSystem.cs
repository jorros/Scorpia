using System.Collections.Generic;
using System.Linq;
using Map;
using TMPro;
using UI.Tooltip;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using World;

namespace UI.ActionBar
{
    public class ActionBarSystem : MonoBehaviour
    {
        [SerializeField] private Sprite[] sIcons;

        [SerializeField] private Sprite[] mIcons;

        [SerializeField] private GameObject sPrefab;

        [SerializeField] private GameObject mPrefab;

        [SerializeField] private GameObject actionBar;

        [SerializeField] private GameObject extraActionBar;

        private int mButtonCounter, sButtonCounter, extraCounter;

        private List<GameObject> mButtons, sButtons, extraButtons;

        private MapTile selected;

        private IReadOnlyList<IActionBar> actionBars;

        private void Awake()
        {
            EventManager.RegisterAll(this);

            mButtons = new List<GameObject>();
            sButtons = new List<GameObject>();
            extraButtons = new List<GameObject>();

            actionBars = new[]
            {
                new CityActionBar(this)
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
                mButtonCounter = 0;
                sButtonCounter = 0;
                extraCounter = 0;

                actionBars.First(x => x.ShouldRender(selected)).Render(selected);

                for (var i = mButtonCounter; i < mButtons.Count; i++)
                {
                    AddMAction(0, TooltipDescription.Empty, null);
                }

                for (var i = sButtonCounter; i < sButtons.Count; i++)
                {
                    AddSAction(0, TooltipDescription.Empty, null);
                }
            }
        }

        public void SetMButtons(int count)
        {
            if (mButtons.Count == count)
            {
                return;
            }

            if (mButtons.Any())
            {
                foreach (var button in mButtons)
                {
                    Destroy(button);
                }
            }

            mButtons.Clear();

            for (var i = 0; i < count; i++)
            {
                var action = Instantiate(mPrefab, actionBar.transform);

                mButtons.Add(action);
            }
        }

        public void SetSButtons(int count)
        {
            if (sButtons.Count == count)
            {
                return;
            }

            if (sButtons.Any())
            {
                foreach (var button in sButtons)
                {
                    Destroy(button);
                }
            }

            sButtons.Clear();

            for (var i = 0; i < count; i++)
            {
                var action = Instantiate(sPrefab, actionBar.transform);

                sButtons.Add(action);
            }
        }

        public bool ToggleExtra()
        {
            var toggle = !extraActionBar.activeInHierarchy;

            extraActionBar.SetActive(toggle);
            return toggle;
        }

        public void SetExtraButtons(int count)
        {
            if (extraButtons.Count == count)
            {
                return;
            }

            if (extraButtons.Any())
            {
                foreach (var button in extraButtons)
                {
                    Destroy(button);
                }
            }

            extraButtons.Clear();

            for (var i = 0; i < count; i++)
            {
                var action = Instantiate(mPrefab, extraActionBar.transform);

                extraButtons.Add(action);
            }

            extraActionBar.SetActive(true);
        }

        public void AddMAction(int icon, TooltipDescription tooltipDesc, UnityAction action,
            ActionButtonOptions options = null)
        {
            if (mButtonCounter >= mButtons.Count)
            {
                return;
            }

            var button = mButtons[mButtonCounter].GetComponent<ActionButton>();

            button.SetImage(mIcons[icon]);
            ApplyOptions(options, button);

            if (action != null)
            {
                button.SetClickAction(action);
            }

            tooltipDesc.Position = TooltipPosition.Action;
            if (extraActionBar.activeInHierarchy)
            {
                tooltipDesc.Position = TooltipPosition.ExtraAction;
            }

            button.SetTooltip(tooltipDesc);

            mButtonCounter++;
        }

        public void AddSAction(int icon, TooltipDescription tooltipDesc, UnityAction action,
            ActionButtonOptions options = null)
        {
            if (sButtonCounter >= sButtons.Count)
            {
                return;
            }

            var button = sButtons[sButtonCounter].GetComponent<ActionButton>();

            button.SetImage(sIcons[icon]);
            ApplyOptions(options, button);

            if (action != null)
            {
                button.SetClickAction(action);
            }

            tooltipDesc.Position = TooltipPosition.Action;
            if (extraActionBar.activeInHierarchy)
            {
                tooltipDesc.Position = TooltipPosition.ExtraAction;
            }

            button.SetTooltip(tooltipDesc);

            sButtonCounter++;
        }

        public void AddExtraAction(int icon, TooltipDescription tooltipDesc, UnityAction action,
            ActionButtonOptions options = null)
        {
            if (extraCounter >= extraButtons.Count)
            {
                return;
            }

            var button = extraButtons[extraCounter].GetComponent<ActionButton>();

            button.SetImage(mIcons[icon]);
            ApplyOptions(options, button);

            if (action != null)
            {
                button.SetClickAction(() =>
                {
                    CameraMovement.clickIsBlocked = true;
                    action.Invoke();
                });
            }

            tooltipDesc.Position = TooltipPosition.ExtraAction;
            button.SetTooltip(tooltipDesc);

            extraCounter++;
        }

        private static void ApplyOptions(ActionButtonOptions options, ActionButton button)
        {
            button.SetIcon(options?.Type);
            button.SetEnabled(options is not {Disabled: true});
            button.SetLevel(options?.UpgradeLevel);
            button.SetProgress(options?.Progress ?? 100);
        }

        [Event(Events.SelectTile)]
        private void SelectTile(MapTile tile)
        {
            if (!actionBars.Any(x => x.ShouldRender(tile)))
            {
                Deselect();
                return;
            }

            selected = tile;

            actionBar.SetActive(true);
            extraActionBar.SetActive(false);
        }

        [Event(Events.DeselectTile)]
        private void Deselect()
        {
            selected = null;

            actionBar.SetActive(false);
            extraActionBar.SetActive(false);
        }
    }
}