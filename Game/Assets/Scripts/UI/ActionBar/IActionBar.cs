using Map;

namespace UI.ActionBar
{
	public interface IActionBar
	{
		string Type { get; }
		void Render(MapTile tile);
	}
}

