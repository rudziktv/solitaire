using UnityEngine;

namespace Preferences
{
    public static class Prefs
    {
        // Floats
        public static float GetFloat(string key, float defaultValue)
        {
            return PlayerPrefs.GetFloat(key, defaultValue);
        }

        public static float GetFloat(PrefPair<float> pair)
        {
            return GetFloat(pair.Key, pair.DefaultValue);
        }

        public static void SetFloat(string key, float value)
        {
            PlayerPrefs.SetFloat(key, value);
        }

        public static void SetFloat(PrefPair<float> pair, float? value)
        {
            SetFloat(pair.Key, value ?? pair.DefaultValue);
        }
        
        //Booleans
        public static bool GetBool(string key, bool defaultValue)
        {
            var boolAsInt = defaultValue ? 1 : 0;
            return PlayerPrefs.GetInt(key, boolAsInt) != 0;
        }

        public static bool GetBool(PrefPair<bool> pair)
        {
            return GetBool(pair.Key, pair.DefaultValue);
        }

        public static void SetBool(string key, bool value)
        {
            var boolAsInt = value ? 1 : 0;
            PlayerPrefs.SetInt(key, boolAsInt);
        }

        public static void SetBool(PrefPair<bool> pair, bool? value)
        {
            SetBool(pair.Key, value ?? pair.DefaultValue);
        }
        
        
        //Strings

        public static string GetString(string key, string defaultValue)
        {
            return PlayerPrefs.GetString(key, defaultValue);
        }
    }
}