using Scorpia.Assets.Scripts.Map;
using UnityEngine;

namespace Scorpia.Assets.Scripts.UI.ActionBar
{
	public class CityActionBar : IActionBar
	{
        private readonly ActionBarSystem system;

        public CityActionBar(ActionBarSystem system)
		{
            this.system = system;
        }

        public string Type => "city";

        public void Render(MapTile tile)
        {
            system.SetMButtons(6);
            system.AddMAction(0, new TooltipDescription("Build", "Empty build slot"), () =>
            {

                if(!system.ToggleExtra())
                {
                    return;
                }

                system.SetExtraButtons(8);
                system.AddExtraAction(0, new TooltipDescription("Blabla", "Test"), null);
            });

            system.SetSButtons(2);
        }
    }
}

