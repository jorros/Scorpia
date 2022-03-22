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

        public static PlayerColour PlayerColour
        {
            get
            {
                return (PlayerColour)PlayerPrefs.GetInt("PlayerColour", 0);
            }
            set
            {
                PlayerPrefs.SetInt("PlayerColour", (int)value);
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