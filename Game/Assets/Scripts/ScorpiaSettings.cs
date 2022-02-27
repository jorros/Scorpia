using System;
using UnityEngine;

namespace Scorpia.Assets.Scripts
{
    public static class ScorpiaSettings
    {
        public static string PlayerName
        {
            get
            {
                return PlayerPrefs.GetString("PlayerName", "");
            }
            set
            {
                PlayerPrefs.SetString("PlayerName", value);
                PlayerPrefs.Save();
            }
        }

        public static string Uid
        {
            get
            {
                var id = PlayerPrefs.GetString("UID");
                
                if(string.IsNullOrWhiteSpace(id))
                {
                    id = Guid.NewGuid().ToString();
                    PlayerPrefs.SetString("UID", id);
                    PlayerPrefs.Save();
                }

                return id;
            }
        }
    }
}