using System.Diagnostics;
using Map;
using Debug = UnityEngine.Debug;

namespace PlayerActions
{
    public class BuildRoadPlayerAction : IPlayerAction
    {
        private readonly MapTile start;

        public BuildRoadPlayerAction(MapTile start)
        {
            this.start = start;
        }

        public string Description => "Build road";

        public void LeftClick(MapTile mapTile)
        {
            EventManager.Trigger(Events.ResetTempTile);
        }

        public void RightClick(MapTile mapTile)
        {
            EventManager.Trigger(Events.ResetTempTile);
            Game.ResetPlayerAction();
        }

        public void Hover(MapTile mapTile)
        {
            if (mapTile.Location == null)
            {
                EventManager.Trigger(Events.ResetTempTile);
                return;
            }
            
            var path = PathFinder.Find(start, mapTile);

            foreach (var tile in path)
            {
                EventManager.Trigger(Events.DrawTempTile, tile, TempTileType.Road);
            }

            Debug.Log(
                $"Draw road from {start.Location.Name.Value.Value.Value} to {mapTile.Location.Name.Value.Value.Value}");
        }
    }
}