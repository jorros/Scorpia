using UnityEngine;

namespace Scorpia.Assets.Scripts.World
{
    public static class Game
    {
        public static int CurrentTick
        {
            get
            {
                return Ticker.current.currentTick?.Value ?? default(int);
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