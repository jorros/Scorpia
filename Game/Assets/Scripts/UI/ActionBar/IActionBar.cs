using Scorpia.Assets.Scripts.Map;

namespace Scorpia.Assets.Scripts.UI.ActionBar
{
	public interface IActionBar
	{
		string Type { get; }
		void Render(MapTile tile);
	}
}

