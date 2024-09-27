namespace Preferences
{
    public static class PreferencesList
    {
        public static readonly PrefPair<float> Volume = new("volume", 1f);
        public static readonly PrefPair<bool> Mute = new("mute", false);
    }
}