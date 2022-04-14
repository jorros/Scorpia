using Map;

namespace UI.ActionBar
{
	public interface IActionBar
	{
		bool ShouldRender(MapTile tile);
		void Render(MapTile tile);
	}
}

