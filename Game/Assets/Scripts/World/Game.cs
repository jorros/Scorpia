using Scorpia.Assets.Scripts.Map;
using UnityEngine;

namespace Scorpia.Assets.Scripts.World
{
    public static class Game
    {
        public static GameObject TickerObject { get; set; }
        public static GameObject MapObject { get; set; }
        public static GameObject WorldManagerObject { get; set; }
        public static MapRenderer MapRenderer => MapObject?.GetComponent<MapRenderer>();
        public static Map.Map Map => MapRenderer?.map;
        public static NotificationSystem NotificationSystem => WorldManagerObject?.GetComponent<NotificationSystem>();

        public static int CurrentTick
        {
            get
            {
                return TickerObject?.GetComponent<Ticker>()?.currentTick?.Value ?? default(int);
            }
        }

        private static string version;

        public static string Version
        {
            get
            {
                if (string.IsNullOrWhiteSpace(version))
                {
                    var request = Resources.Load<VersionScriptableObject>("Build");
                    version = request.BuildNumber;
                }

                return version;
            }
        }
    }
}