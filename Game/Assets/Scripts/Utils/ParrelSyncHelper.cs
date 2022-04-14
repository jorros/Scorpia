// Taken from https://github.com/VeriorPies/ParrelSync/blob/master/ParrelSync/Editor/ClonesManager.cs

using System.IO;
using UnityEngine;

namespace Utils
{
    public class ParrelSyncHelper
    {
        /// <summary>
        /// Name used for an identifying file created in the clone project directory.
        /// </summary>
        /// <remarks>
        /// (!) Do not change this after the clone was created, because then connection will be lost.
        /// </remarks>
        public const string CloneFileName = ".clone";

        /// <summary>
        /// Get the path to the current unityEditor project folder's info
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentProjectPath()
        {
            return Application.dataPath.Replace("/Assets", "");
        }
        
        private static bool? isCloneFileExistCache = null;

        /// <summary>
        /// Returns true if the project currently open in Unity Editor is a clone.
        /// </summary>
        /// <returns></returns>
        public static bool IsClone()
        {
            if (isCloneFileExistCache == null)
            {
                /// The project is a clone if its root directory contains an empty file named ".clone".
                string cloneFilePath = Path.Combine(GetCurrentProjectPath(), CloneFileName);
                isCloneFileExistCache = File.Exists(cloneFilePath);
            }

            return (bool)isCloneFileExistCache;
        }
    }
}