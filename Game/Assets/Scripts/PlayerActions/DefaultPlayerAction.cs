using Map;

namespace PlayerActions
{
    public class DefaultPlayerAction : IPlayerAction
    {
        public string Description => null;

        public void LeftClick(MapTile mapTile)
        {
            EventManager.Trigger(Events.SelectTile, mapTile);
        }

        public void RightClick(MapTile mapTile)
        {
            EventManager.Trigger(Events.DeselectTile);
        }

        public void Hover(MapTile mapTile)
        {
        }
    }
}