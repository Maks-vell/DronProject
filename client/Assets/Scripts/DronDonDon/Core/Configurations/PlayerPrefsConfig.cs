using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace DronDonDon.Core.Configurations
{
    [PublicAPI]
    public static class PlayerPrefsConfig
    {
        public static bool MusicMute
        {
            get { return PlayerPrefs.GetInt("musicMute", 0) == 1; }
            set { PlayerPrefs.SetInt("musicMute", value ? 1 : 0); }
        }

        public static bool SoundMute
        {
            get { return PlayerPrefs.GetInt("soundMute", 0) == 1; }
            set { PlayerPrefs.SetInt("soundMute", value ? 1 : 0); }
        }

        public static string GraphicsQuality
        {
            get { return PlayerPrefs.GetString("graphicsQuality", null); }
            set { PlayerPrefs.SetString("graphicsQuality", value); }
        }

        public static string UserLang
        {
            get { return PlayerPrefs.GetString("userLang", null); }
            set { PlayerPrefs.SetString("userLang", value); }
        }

        public static int GetIntValue(string key)
        {
            return PlayerPrefs.GetInt(key, 0);
        }

        public static void IncrementIntValue(string key)
        {
            PlayerPrefs.SetInt(key, GetIntValue(key) + 1);
        }

        public static void DeletePrefs(List<string> playerPrefs)
        {
            foreach (string pref in playerPrefs) {
                PlayerPrefs.DeleteKey(pref);
            }
        }
    }
}