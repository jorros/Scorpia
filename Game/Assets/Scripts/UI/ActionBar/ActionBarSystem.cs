using System.Collections.Generic;
using System.Linq;
using Map;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.ActionBar
{
	public class ActionBarSystem : MonoBehaviour
	{
		[SerializeField]
		private Sprite[] sIcons;

		[SerializeField]
		private Sprite[] mIcons;

		[SerializeField]
		private GameObject sPrefab;

		[SerializeField]
		private GameObject mPrefab;

		[SerializeField]
		private GameObject actionBar;

		[SerializeField]
		private GameObject extraActionBar;

		private int mButtonCounter, sButtonCounter, extraCounter;

		private List<GameObject> mButtons, sButtons, extraButtons;

		private MapTile selected;

		private IReadOnlyList<IActionBar> actionBars;

		private void Awake()
        {
			EventManager.Register(EventManager.SelectTile, SelectTile);
			EventManager.Register(EventManager.DeselectTile, Deselect);

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
			EventManager.Remove(EventManager.SelectTile, SelectTile);
			EventManager.Remove(EventManager.DeselectTile, Deselect);
		}

		private void FixedUpdate()
        {
			if(selected != null)
            {
				mButtonCounter = 0;
				sButtonCounter = 0;
				extraCounter = 0;

				actionBars.First().Render(selected);

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
			if(mButtons.Count == count)
            {
				return;
            }

			if(mButtons.Any())
            {
				foreach(var button in mButtons)
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
				foreach (var button in mButtons)
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

			print(toggle);

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
				foreach (var button in mButtons)
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

		public void AddMAction(int icon, TooltipDescription tooltipDesc, UnityAction action)
        {
			mButtons[mButtonCounter].GetComponentsInChildren<Image>().First(x => x.name == "Image").sprite = mIcons[icon];

			if (action != null)
			{
				mButtons[mButtonCounter].GetComponent<Button>().onClick.RemoveAllListeners();
				mButtons[mButtonCounter].GetComponent<Button>().onClick.AddListener(action);
			}

			var tooltip = mButtons[mButtonCounter].GetComponent<TooltipTrigger>();
			tooltip.header = tooltipDesc.Header;
			tooltip.content = tooltipDesc.Content;
			tooltip.subHeader = tooltipDesc.SubHeader;

			mButtonCounter++;
		}

		public void AddSAction(int icon, TooltipDescription tooltipDesc, UnityAction action)
		{
			sButtons[sButtonCounter].GetComponentsInChildren<Image>().First(x => x.name == "Image").sprite = sIcons[icon];

			if (action != null)
			{
				sButtons[sButtonCounter].GetComponent<Button>().onClick.RemoveAllListeners();
				sButtons[sButtonCounter].GetComponent<Button>().onClick.AddListener(action);
			}

			var tooltip = sButtons[sButtonCounter].GetComponent<TooltipTrigger>();
			tooltip.header = tooltipDesc.Header;
			tooltip.content = tooltipDesc.Content;
			tooltip.subHeader = tooltipDesc.SubHeader;

			sButtonCounter++;
		}

		public void AddExtraAction(int icon, TooltipDescription tooltipDesc, UnityAction action)
		{
			extraButtons[extraCounter].GetComponentsInChildren<Image>().First(x => x.name == "Image").sprite = mIcons[icon];

			if (action != null)
			{
				extraButtons[extraCounter].GetComponent<Button>().onClick.RemoveAllListeners();
				extraButtons[extraCounter].GetComponent<Button>().onClick.AddListener(action);
			}

			var tooltip = extraButtons[extraCounter].GetComponent<TooltipTrigger>();
			tooltip.header = tooltipDesc.Header;
			tooltip.content = tooltipDesc.Content;
			tooltip.subHeader = tooltipDesc.SubHeader;
			tooltip.position = TooltipPosition.ExtraAction;

			extraCounter++;
		}

		private void SelectTile(IReadOnlyList<object> args)
		{
			selected = args[0] as MapTile;

			actionBar.SetActive(true);
			extraActionBar.SetActive(false);
		}

		private void Deselect(IReadOnlyList<object> args)
		{
			selected = null;

			actionBar.SetActive(false);
			extraActionBar.SetActive(false);
		}
	}
}

